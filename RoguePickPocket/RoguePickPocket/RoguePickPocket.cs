#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Levelbot.Actions.Death;
using Levelbot.Decorators.Death;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using CommonBehaviors.Actions;
using Action = Styx.TreeSharp.Action;
#endregion

namespace RoguePickPocket {
    public class RoguePickPocket : BotBase {
        #region Variables
        public readonly Version Ver = new Version(0, 8, 0);
        public LocalPlayer Me { get { return StyxWoW.Me; } }
        public static void RPPLog(string message, params object[] args) { Logging.Write(Colors.DeepSkyBlue, "[RPP]: " + message, args); }
        public static void RPPDiag(string message, params object[] args) { Logging.WriteDiagnostic(Colors.DeepSkyBlue, "[RPP]: " + message, args); }

        private bool _OldLogoutForInactivity;
        private bool _LogMessage1Showed;
        private bool _LogMessage2Showed;
        private WoWPoint _StartPoint = WoWPoint.Empty;
        private Composite _Root;
        private WoWUnit _RPPNext;
        private WoWUnit _RPPTarget;
        private WoWUnit _RPPLoot;
        private WoWUnit _RPPSap;
        private WoWPoint _RPPTargetLocation = WoWPoint.Empty;
        private readonly LocalBlacklist _RPPBlacklist = new LocalBlacklist(TimeSpan.FromSeconds(30));
        private WoWItem _LockBox;
        #region _LockBoxes
        private readonly HashSet<uint> _LockBoxes = new HashSet<uint> {
            4632,	// Ornate Bronze Lockbox
            6354,	// Small Locked Chest
            16882,	// Battered Junkbox
            4633,	// Heavy Bronze Lockbox
            4634,	// Iron Lockbox
            6355,	// Sturdy Locked Chest
            16883,	// Worn Junkbox
            4636,	// Strong Iron Lockbox
            4637,	// Steel Lockbox
            16884,	// Sturdy Junkbox
            4638,	// Reinforced Steel Lockbox
            13875,	// Ironbound Locked Chest
            5758,	// Mithril Lockbox
            5759,	// Thorium Lockbox
            13918,	// Reinforced Locked Chest
            5760,	// Eternium Lockbox
            12033,	// Thaurissan Family Jewels
            29569,	// Strong Junkbox
            31952,	// Khorium Lockbox
            43575,	// Reinforced Junkbox
            43622,	// Froststeel Lockbox
            43624,	// Titanium Lockbox
            45986,	// Tiny Titanium Lockbox
            63349,	// Flame-Scarred Junkbox
            68729,	// Elementium Lockbox
            88567,	// Ghost Iron Lockbox
            88165,	// Vine-Cracked Junkbox
        };

        #endregion
        #endregion

        #region Overrides
        public override string Name { get { return "RoguePickPocket"; } }
        public override Composite Root { get { return _Root ?? (_Root = CreateRoot()); } }
        public override PulseFlags PulseFlags { get { return PulseFlags.All; } }
        public override void Pulse() { }
        public override void Start() {
            try {
                ProfileManager.LoadEmpty();
                Targeting.Instance.IncludeTargetsFilter += IncludeTargetsFilter;
                _OldLogoutForInactivity = GlobalSettings.Instance.LogoutForInactivity;
                GlobalSettings.Instance.LogoutForInactivity = false;
                if (_StartPoint == WoWPoint.Empty) { _StartPoint = Me.Location; }
                RPPLog("{0} ver {1} has been initialized.", Name, Ver);
            }
            catch (Exception initExept) { Logging.WriteDiagnostic(initExept.ToString()); }
        }
        public override void Stop() {
            Targeting.Instance.IncludeTargetsFilter -= IncludeTargetsFilter;
            GlobalSettings.Instance.LogoutForInactivity = _OldLogoutForInactivity;
            _RPPTargetLocation = WoWPoint.Empty;
            _RPPNext = null;
            _RPPTarget = null;
            _StartPoint = WoWPoint.Empty;
            _LogMessage1Showed = false;
            _LogMessage2Showed = false;
            _RPPBlacklist.RemoveAll();
        }
        public override Form ConfigurationForm { get { var gui = new RoguePickPocket_GUI(); gui.Activate(); return gui; } }
        #endregion

        #region Methods
        private bool IsCastingOrChannelling() { return Me.IsCasting || Me.IsChanneling; }

        private static void IncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits) {
            if (StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.Attackable) {
                outgoingUnits.Add(StyxWoW.Me.CurrentTarget);
            }
        }

        private bool RPPReady() {
            if (ObjectManager.GetObjectsOfType<WoWUnit>().Any(unit => unit.Aggro || unit.PetAggro)) { return false; }
            return !Me.IsOnTransport && !Me.IsGhost && !Me.IsDead && !Me.Combat && !Me.IsActuallyInCombat;
        }

        public WoWItem FindLockedBox() {
            if (Me.CarriedItems == null)
                return null;
            return Me.CarriedItems.FirstOrDefault(b => b != null && b.IsValid && b.ItemInfo != null
                                                       && b.ItemInfo.ItemClass == WoWItemClass.Miscellaneous
                                                       && b.ItemInfo.ContainerClass == WoWItemContainerClass.Container
                                                       && b.ItemInfo.Level <= Me.Level
                                                       && !b.IsOpenable
                                                       && b.Usable
                                                       && b.Cooldown <= 0
                                                       && !Blacklist.Contains(b.Guid, BlacklistFlags.Node)
                                                       && _LockBoxes.Contains(b.Entry));
        }

        public WoWItem FindUnlockedBox() {
            if (Me.CarriedItems == null)
                return null;
            return Me.CarriedItems.FirstOrDefault(b => b != null && b.IsValid && b.ItemInfo != null
                                                       && b.ItemInfo.ItemClass == WoWItemClass.Miscellaneous
                                                       && b.ItemInfo.ContainerClass == WoWItemContainerClass.Container
                                                       && b.IsOpenable
                                                       && b.Usable
                                                       && b.Cooldown <= 0
                                                       && !Blacklist.Contains(b.Guid, BlacklistFlags.Loot)
                                                       && _LockBoxes.Contains(b.Entry));
        }
        #endregion

        #region Behavior Tree
        private PrioritySelector CreateRoot() {
            return new PrioritySelector(
                #region Class Check
                new Decorator(context => Me.Class != WoWClass.Rogue,
                    new Sequence(
                        new Action(context => RPPLog("You need to play a Rogue to use this Botbase.")),
                        new Action(context => Stop()),
                        new Action(context => TreeRoot.Stop("You're not a Rogue!"))
                    )
                ),
                #endregion
                #region NoCombat
                new PrioritySelector(
                    RoutineManager.Current.RestBehavior ?? new ActionAlwaysFail(),
                    RoutineManager.Current.PreCombatBuffBehavior ?? new ActionAlwaysFail()
                ),
                new Decorator(context => RPPReady(),
                    new Sequence(
                        #region Loot if there is any corpses and we have it active in the settings.
                        new DecoratorContinue(context => RoguePickPocket_Settings.Instance.RPPCheckBox_LC,
                            new Sequence(
                                new Action(context => ObjectManager.Update()),
                                new Action(context => _RPPLoot = null),
                                new Action(context => _RPPLoot = ObjectManager.GetObjectsOfType<WoWUnit>(true).OrderBy(l => l.Distance).FirstOrDefault(l => l.Lootable)),
                                new DecoratorContinue(context => _RPPLoot != null,
                                    new Sequence(
                                        new DecoratorContinue(context => _RPPLoot.IsWithinMeleeRange,
                                            new Action(context => _RPPLoot.Interact())
                                        ),
                                        new DecoratorContinue(context => !_RPPLoot.IsWithinMeleeRange,
                                            new Action(context => Navigator.MoveTo(_RPPLoot.Location))
                                        )
                                    )
                                )
                            )
                        ),
                        #endregion
                        #region Cast Stealth (1784)
                        new DecoratorContinue(context => _RPPLoot == null,
                            new Sequence(
                                new DecoratorContinue(context => !Me.HasAura(1784) && SpellManager.CanCast(1784),
                                    new Sequence(
                                        new Action(context => SpellManager.Cast(1784)),
                                        new WaitContinue(TimeSpan.FromMilliseconds(300), context => false, new ActionAlwaysSucceed())
                                    )
                                )
                            )
                        ),
                        #endregion
                        #region We are stealthed
                        new DecoratorContinue(context => Me.HasAura(1784),
                            new Sequence(
                                new DecoratorContinue(context => _RPPTargetLocation == WoWPoint.Empty,
                                    #region ScanForTarget
                                    new Sequence(
                                        new Action(context => ObjectManager.Update()),
                                        new Action(context => _RPPNext = null),
                                        new Action(context => _RPPNext = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsAlive && u.CanSelect && u.IsHumanoid && !u.IsFriendly && !u.IsPlayer && !_RPPBlacklist.Contains(u)).OrderBy(u => u.Distance).FirstOrDefault()),
                                        new DecoratorContinue(context => _RPPNext != null,
                                            new Sequence(
                                                new DecoratorContinue(context => !Navigator.CanNavigateFully(Me.Location, _RPPNext.Location),
                                                    new Sequence(
                                                        new DecoratorContinue(context => _RPPTarget != _RPPNext,
                                                            new Sequence(
                                                                new Action(context => _RPPBlacklist.Add(_RPPNext, TimeSpan.FromMinutes(10))),
                                                                new Action(context => _RPPTarget = _RPPNext)
                                                            )
                                                        ),
                                                        new Action(context => _RPPTargetLocation = WoWPoint.Empty)
                                                    )
                                                ),
                                                new DecoratorContinue(context => _RPPTarget != _RPPNext,
                                                    new Sequence(
                                                        new Action(context => _LogMessage1Showed = false),
                                                        new Action(context => _LogMessage2Showed = false),
                                                        new Action(context => _RPPTarget = _RPPNext),
                                                        new Action(context => _RPPTargetLocation = WoWMovement.CalculatePointFrom(_RPPTarget.Location, 0))
                                                    )
                                                )
                                            )
                                        ),
                                        #region Out of targets
                                        new DecoratorContinue(context => _RPPNext == null,
                                            new Sequence(
                                                new DecoratorContinue(context => !_LogMessage1Showed,
                                                    new Action(context => RPPLog("Can't find any more targets."))
                                                ),
                                                new DecoratorContinue(context => _StartPoint != WoWPoint.Empty,
                                                    new Sequence(
                                                        new DecoratorContinue(context => !_LogMessage1Showed,
                                                            new Sequence(
                                                                new Action(context => RPPLog("Moving back to start position.")),
                                                                new Action(context => _LogMessage1Showed = true)
                                                            )
                                                        ),
                                                        new DecoratorContinue(context => _StartPoint.Distance(Me.Location) > 1,
                                                            new Sequence(
                                                                new Action(context => Me.ClearTarget()),
                                                                new Action(context => Navigator.MoveTo(_StartPoint))
                                                            )
                                                        ),
                                                        #region We are at Start Position
                                                        new DecoratorContinue(context => _StartPoint.Distance(Me.Location) <= 1,
                                                            new Sequence(
                                                                new DecoratorContinue(context => !_LogMessage2Showed,
                                                                    new Sequence(
                                                                        new Action(context => RPPLog("We are at start position, waiting for mobs to reset.")),
                                                                        new Action(context => RPPLog("Scanning inventory for LockBoxes.")),
                                                                        new Action(context => _LogMessage2Showed = true)
                                                                    )
                                                                ),
                                                                new Action(context => _RPPNext = null),
                                                                new Action(context => _RPPTarget = null),
                                                                new Action(context => _RPPTargetLocation = WoWPoint.Empty),
                                                                #region Cast PickLock (1804)
                                                                new Action(context => _LockBox = FindLockedBox()),
                                                                new DecoratorContinue(context => _LockBox != null,
                                                                    new Sequence(
                                                                        new Action(context => RPPLog(string.Format("Unlocking {0}", _LockBox.Name))),
                                                                        new Action(context => SpellManager.Cast(1804)),
                                                                        new WaitContinue(TimeSpan.FromSeconds(1), context => Me.CurrentPendingCursorSpell != null, new ActionAlwaysSucceed()),
                                                                        new Action(context => _LockBox.Use()),
                                                                        new Action(context => Blacklist.Add(_LockBox.Guid, BlacklistFlags.Node, TimeSpan.FromSeconds(30))),
                                                                        new WaitContinue(TimeSpan.FromSeconds(1), context => IsCastingOrChannelling(), new ActionAlwaysSucceed()),
                                                                        new WaitContinue(TimeSpan.FromSeconds(6), context => !IsCastingOrChannelling(), new ActionAlwaysSucceed()),
                                                                        new WaitContinue(TimeSpan.FromMilliseconds(300), context => false, new ActionAlwaysSucceed())
                                                                    )
                                                                ),
                                                                #endregion
                                                                #region Open Lockboxes
                                                                new Action(context => _LockBox = FindUnlockedBox()),
                                                                new DecoratorContinue(context => _LockBox != null,
                                                                    new Sequence(
                                                                        new Action(context => RPPLog(string.Format("Opening {0}", _LockBox.Name))),
                                                                        new WaitContinue(TimeSpan.FromSeconds(2), context => (_LockBox.IsOpenable && !IsCastingOrChannelling()), new ActionAlwaysSucceed()),
                                                                        new Action(context => _LockBox.UseContainerItem()),
                                                                        new Action(context => Blacklist.Add(_LockBox.Guid, BlacklistFlags.Loot, TimeSpan.FromSeconds(30))),
                                                                        new WaitContinue(TimeSpan.FromMilliseconds(300), context => false, new ActionAlwaysSucceed())
                                                                    )
                                                                )
                                                                #endregion
                                                            )
                                                        )
                                                        #endregion
                                                    )
                                                ),
                                                #region For some reason we don't have a _StartPoint, Stop bot.
                                                new DecoratorContinue(context => _StartPoint == WoWPoint.Empty,
                                                    new Sequence(
                                                        new Action(context => Stop()),
                                                        new Action(context => TreeRoot.Stop("Out of targets"))
                                                    )
                                                )
                                                #endregion
                                            )
                                        )
                                        #endregion
                                    )
                                    #endregion
                                ),
                                new DecoratorContinue(context => _RPPTargetLocation != WoWPoint.Empty,
                                    new Sequence(
                                        #region Calculate Target position and move to it.
                                        new DecoratorContinue(context => _RPPTargetLocation.Distance(Me.Location) > RoguePickPocket_Settings.Instance.RPPTrackBar_PPFY,
                                            new Sequence(
                                                #region Cast Sap (6770)
                                                new Action(context => ObjectManager.Update()),
                                                new Action(context => _RPPSap = null),
                                                new Action(context => _RPPSap = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsAlive && u.CanSelect && u.IsHostile && u.InLineOfSight && (u.Distance <= 10) && !u.Combat).OrderBy(u => u.Distance).FirstOrDefault()),
                                                new DecoratorContinue(context => _RPPSap != null,
                                                    new DecoratorContinue(context => !_RPPSap.HasAura(6770) && SpellManager.CanCast(6770),
                                                        new Sequence(
                                                            new DecoratorContinue(context => Me.CurrentTarget != _RPPSap,
                                                                new Action(context => _RPPSap.Target())
                                                            ),
                                                            new DecoratorContinue(context => Me.CurrentTarget == _RPPSap,
                                                                new Action(context => SpellManager.Cast(6770))
                                                            )
                                                        )
                                                    )
                                                ),
                                                #endregion
                                                new DecoratorContinue(context => _RPPTarget.IsAlive,
                                                    new Sequence(
                                                        new Action(context => _RPPTargetLocation = WoWMovement.CalculatePointFrom(_RPPTarget.Location, 0)),
                                                        new Action(context => Navigator.MoveTo(_RPPTargetLocation))
                                                    )
                                                ),
                                                new DecoratorContinue(context => _RPPTarget.IsDead,
                                                    new Sequence(
                                                        new Action(context => _RPPTarget = null),
                                                        new Action(context => _RPPTargetLocation = WoWPoint.Empty)
                                                    )
                                                )
                                            )
                                        ),
                                        #endregion
                                        #region Cast Pick Pocket (921)
                                        new DecoratorContinue(context => _RPPTargetLocation.Distance(Me.Location) <= RoguePickPocket_Settings.Instance.RPPTrackBar_PPFY,
                                            new Sequence(
                                                new Action(context => WoWMovement.MoveStop()),
                                                #region Cast Sap (6770)
                                                new Action(context => ObjectManager.Update()),
                                                new Action(context => _RPPSap = null),
                                                new Action(context => _RPPSap = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsAlive && u.CanSelect && u.IsHostile && u.InLineOfSight && (u.Distance <= 10) && !u.Combat).OrderBy(u => u.Distance).FirstOrDefault()),
                                                new DecoratorContinue(context => _RPPSap != null,
                                                    new DecoratorContinue(context => !_RPPSap.HasAura(6770) && SpellManager.CanCast(6770),
                                                        new Sequence(
                                                            new DecoratorContinue(context => Me.CurrentTarget != _RPPSap,
                                                                new Sequence(
                                                                    new Action(context => _RPPSap.Target()),
                                                                    new Action(context => _RPPSap.Face())
                                                                )
                                                            ),
                                                            new DecoratorContinue(context => Me.CurrentTarget == _RPPSap,
                                                                new Action(context => SpellManager.Cast(6770))
                                                            )
                                                        )
                                                    )
                                                ),
                                                #endregion
                                                new DecoratorContinue(context => SpellManager.CanCast(921),
                                                    new Sequence(
                                                        new DecoratorContinue(context => Me.CurrentTarget != _RPPTarget,
                                                            new Action(context => _RPPTarget.Target())
                                                        ),
                                                        new DecoratorContinue(context => Me.CurrentTarget == _RPPTarget,
                                                            new Sequence(
                                                                new Action(context => SpellManager.Cast(921)),
                                                                new WaitContinue(TimeSpan.FromMilliseconds(1200), context => false, new ActionAlwaysSucceed()),
                                                                new Action(context => _RPPBlacklist.Add(_RPPTarget, TimeSpan.FromSeconds(600))),
                                                                new Action(context => _RPPTargetLocation = WoWPoint.Empty),
                                                                new Action(context => Me.ClearTarget())
                                                            )
                                                        )
                                                    )
                                                ),
                                                new DecoratorContinue(context => !SpellManager.CanCast(921),
                                                    new Sequence(
                                                        new Action(context => _RPPBlacklist.Add(_RPPTarget, TimeSpan.FromSeconds(600))),
                                                        new Action(context => _RPPTargetLocation = WoWPoint.Empty)
                                                    )
                                                )
                                            )
                                        )
                                        #endregion
                                    )
                                )
                            )
                        )
                        #endregion
                    )
                ),
                #endregion
                #region Combat
                new Decorator(ret => StyxWoW.Me.Combat,
                    new PrioritySelector(
                        RoutineManager.Current.HealBehavior ?? new ActionAlwaysFail(),
                        RoutineManager.Current.CombatBuffBehavior ?? new ActionAlwaysFail(),
                        RoutineManager.Current.CombatBehavior ?? new ActionAlwaysFail()
                    )
                ),
                #endregion
                #region Dead
                new Decorator(context => !Me.IsAlive,
                    new PrioritySelector(
                         new DecoratorNeedToRelease(new ActionReleaseFromCorpse()),
                         new DecoratorNeedToMoveToCorpse(new ActionMoveToCorpse()),
                         new DecoratorNeedToTakeCorpse(new ActionRetrieveCorpse()),
                         new ActionSuceedIfDeadOrGhost()
                    )
                )
                #endregion
            );
        }
        #endregion
    }
    #region LocalBlacklist
    //
    // This class is coded by Chinajade. I've just made some small modifications to suit my need.
    //
    public class LocalBlacklist {
        public LocalBlacklist(TimeSpan maxSweepTime) { _SweepTimer = new WaitTimer(maxSweepTime) { WaitTime = maxSweepTime }; }

        private readonly Dictionary<ulong, DateTime> _BlackList = new Dictionary<ulong, DateTime>();
        private readonly WaitTimer _SweepTimer;

        public void Add(ulong guid, TimeSpan timeSpan) {
            RemoveExpired();
            _BlackList[guid] = DateTime.Now.Add(timeSpan);
        }

        public void Add(WoWObject wowObject, TimeSpan timeSpan) { if (wowObject != null) { Add(wowObject.Guid, timeSpan); } }

        public bool Contains(ulong guid) {
            DateTime expiry;
            if (_BlackList.TryGetValue(guid, out expiry)) { return (expiry > DateTime.Now); }
            return false;
        }

        public bool Contains(WoWObject wowObject) { return (wowObject != null) && Contains(wowObject.Guid); }

        public void RemoveExpired() {
            if (_SweepTimer.IsFinished) {
                var now = DateTime.Now;
                var expiredEntries = (from key in _BlackList.Keys
                                      where (_BlackList[key] < now)
                                      select key).ToList();
                foreach (var entry in expiredEntries) { _BlackList.Remove(entry); }
                _SweepTimer.Reset();
            }
        }
        public void RemoveAll() {
            var everything = (from key in _BlackList.Keys
                              select key).ToList();
            foreach (var entry in everything) { _BlackList.Remove(entry); }
        }
    }
    #endregion
    #region RPPNavigation
    public class RPPNavigation : MeshNavigator {
        public override MoveResult MoveTo(WoWPoint location) {
            //Check for mob in path of location
            ObjectManager.Update();
            WoWUnit mobInPath = ObjectManager.GetObjectsOfType<WoWUnit>(true).OrderBy(m => m.Distance <= 10).FirstOrDefault();
            if (mobInPath != null) {
                // Alter location +/- yards
            }
            return base.MoveTo(location);
        }
    }
    #endregion
}