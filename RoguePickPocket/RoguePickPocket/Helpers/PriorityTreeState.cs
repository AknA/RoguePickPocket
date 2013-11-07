using System;
using System.Linq;
using RoguePickPocket.GUI;
using Styx;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace RoguePickPocket.Helpers {
    class PriorityTreeState {

        // ===========================================================
        // Constants
        // ===========================================================

        public enum State {
            CLASS_CHECK,
            NO_COMBAT,
            SCANNING,
            MOVING,
            SAP,
            PICKPOCKET,
            LOCKBOX,
        };

        // ===========================================================
        // Fields
        // ===========================================================

        public static State TreeState = State.CLASS_CHECK;

        public static WoWPoint StartPoint = WoWPoint.Empty;


        public static WoWUnit NextTarget;
        public static WoWUnit CurrentTarget;
        public static WoWUnit LootTarget;
        public static WoWUnit SapTarget;

        public static WoWPoint TargetLocation = WoWPoint.Empty;

        private static readonly LocalBlacklist LocalBlacklist = new LocalBlacklist(TimeSpan.FromSeconds(30));


        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        public static void TreeStateHandler() {
            RoguePickPocket.CustomLog.CustomNormalLog("In TreeStateHandler");

            switch(TreeState) {
                case State.CLASS_CHECK:
                    if(Character.Me.Class != WoWClass.Rogue) {
                        RoguePickPocket.CustomLog.CustomNormalLog("You need to be on a Rogue to use RoguePickPocket.");
                        TreeRoot.Stop("Not a Rogue.");
                    }

                    // TODO enforce Glyph of Pick Lock and Glyph of Pick Pocket

                    RoguePickPocket.CustomLog.CustomDiagnosticLog("In State.CLASS_CHECK");

                    TreeState = State.NO_COMBAT;

                    break;

                case State.NO_COMBAT:

                    RoguePickPocket.CustomLog.CustomDiagnosticLog("In State.NO_COMBAT");

                    // todo change
                    TreeState = State.LOCKBOX;

                    break;

                case State.LOCKBOX:

                    RoguePickPocket.CustomLog.CustomDiagnosticLog("In State.LOCKBOX");

                    if(RoguePickPocketSettings.Instance.PickLockLockboxCheckbox && LockboxHandler.FindLockedBox() != null) {
                        LockboxHandler.PickLock();
                    }

                    if(RoguePickPocketSettings.Instance.OpenLockboxCheckbox && LockboxHandler.FindUnlockedBox() != null && LockboxHandler.FindLockedBox() == null) {
                        LockboxHandler.OpenLockbox();
                    }

                    break;
            }

        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static bool CanLoot() {
            if(!RoguePickPocketSettings.Instance.LootCorpsesCheckBox) {
                return false;
            }

            LootTarget = null;
            LootTarget = ObjectManager.GetObjectsOfTypeFast<WoWUnit>()
                    .OrderBy(loot => loot.Distance)
                    .FirstOrDefault(loot => loot.Lootable);

            if(LootTarget == null) {
                return false;
            }

            if(LootTarget.IsWithinMeleeRange) {
                LootTarget.Interact();
            } else {
                Navigator.MoveTo(LootTarget.Location);
            }

            return true;
        }

        // Stealth (1784)
        private static void CastStealth() {
            if(!Character.Me.HasAura(1784) && SpellManager.CanCast(1784)) {
                SpellManager.Cast(1784);

                // TODO make a wait timer here (300 ms + )
            }
        }

        /*

        private PrioritySelector CreateRoot() {
            return new PrioritySelector(

            #region NoCombat
new PrioritySelector(
                    RoutineManager.Current.RestBehavior ?? new ActionAlwaysFail(),
                    RoutineManager.Current.PreCombatBuffBehavior ?? new ActionAlwaysFail()
                ),
                new Decorator(context => RPPReady(),
                    new Sequence(


            #region We are stealthed
 new DecoratorContinue(context => Me.HasAura(1784),
                            new Sequence(
                                new DecoratorContinue(context => TargetLocation == WoWPoint.Empty,
            #region ScanForTarget
 new Sequence(
                                        new Styx.TreeSharp.Action(context => ObjectManager.Update()),
                                        new Styx.TreeSharp.Action(context => Next = null),
                                        new Styx.TreeSharp.Action(context => Next = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsAlive && u.CanSelect && u.IsHumanoid && !u.IsFriendly && !u.IsPlayer && !PriorityTreeState.LocalBlacklist.Contains(u)).OrderBy(u => u.Distance).FirstOrDefault()),
                                        new DecoratorContinue(context => Next != null,
                                            new Sequence(
                                                new DecoratorContinue(context => !Navigator.CanNavigateFully(Me.Location, Next.Location),
                                                    new Sequence(
                                                        new DecoratorContinue(context => Target != Next,
                                                            new Sequence(
                                                                new Styx.TreeSharp.Action(context => PriorityTreeState.LocalBlacklist.Add(Next, TimeSpan.FromMinutes(10))),
                                                                new Styx.TreeSharp.Action(context => Target = Next)
                                                            )
                                                        ),
                                                        new Styx.TreeSharp.Action(context => TargetLocation = WoWPoint.Empty)
                                                    )
                                                ),
                                                new DecoratorContinue(context => Target != Next,
                                                    new Sequence(
                                                        new Styx.TreeSharp.Action(context => LogMessage1Showed = false),
                                                        new Styx.TreeSharp.Action(context => LogMessage2Showed = false),
                                                        new Styx.TreeSharp.Action(context => Target = Next),
                                                        new Styx.TreeSharp.Action(context => TargetLocation = WoWMovement.CalculatePointFrom(Target.Location, 0))
                                                    )
                                                )
                                            )
                                        ),
            #region Out of targets
 new DecoratorContinue(context => Next == null,
                                            new Sequence(
                                                new DecoratorContinue(context => !LogMessage1Showed,
                                                    new Styx.TreeSharp.Action(context => RPPLog("Can't find any more targets."))
                                                ),
                                                new DecoratorContinue(context => StartPoint != WoWPoint.Empty,
                                                    new Sequence(
                                                        new DecoratorContinue(context => !LogMessage1Showed,
                                                            new Sequence(
                                                                new Styx.TreeSharp.Action(context => RPPLog("Moving back to start position.")),
                                                                new Styx.TreeSharp.Action(context => LogMessage1Showed = true)
                                                            )
                                                        ),
                                                        new DecoratorContinue(context => StartPoint.Distance(Me.Location) > 1,
                                                            new Sequence(
                                                                new Styx.TreeSharp.Action(context => Me.ClearTarget()),
                                                                new Styx.TreeSharp.Action(context => Navigator.MoveTo(StartPoint))
                                                            )
                                                        ),
            #region We are at Start Position
 new DecoratorContinue(context => StartPoint.Distance(Me.Location) <= 1,
                                                            new Sequence(
                                                                new DecoratorContinue(context => !LogMessage2Showed,
                                                                    new Sequence(
                                                                        new Styx.TreeSharp.Action(context => RPPLog("We are at start position, waiting for mobs to reset.")),
                                                                        new Styx.TreeSharp.Action(context => RPPLog("Scanning inventory for LockBoxes.")),
                                                                        new Styx.TreeSharp.Action(context => LogMessage2Showed = true)
                                                                    )
                                                                ),
                                                                new Styx.TreeSharp.Action(context => Next = null),
                                                                new Styx.TreeSharp.Action(context => Target = null),
                                                                new Styx.TreeSharp.Action(context => TargetLocation = WoWPoint.Empty),

                                                        )
            #endregion
)
                                                ),
            #region For some reason we don't have a _StartPoint, Stop bot.
 new DecoratorContinue(context => StartPoint == WoWPoint.Empty,
                                                    new Sequence(
                                                        new Styx.TreeSharp.Action(context => Stop()),
                                                        new Styx.TreeSharp.Action(context => TreeRoot.Stop("Out of targets"))
                                                    )
                                                )
            #endregion
)
                                        )
            #endregion
)
            #endregion
),
                                new DecoratorContinue(context => TargetLocation != WoWPoint.Empty,
                                    new Sequence(
            #region Calculate Target position and move to it.
new DecoratorContinue(context => TargetLocation.Distance(Me.Location) > RoguePickPocket_Settings.Instance.RPPTrackBar_PPFY,
                                            new Sequence(
            #region Cast Sap (6770)
new Styx.TreeSharp.Action(context => ObjectManager.Update()),
                                                new Styx.TreeSharp.Action(context => Sap = null),
                                                new Styx.TreeSharp.Action(context => Sap = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsAlive && u.CanSelect && u.IsHostile && u.InLineOfSight && (u.Distance <= 10) && !u.Combat).OrderBy(u => u.Distance).FirstOrDefault()),
                                                new DecoratorContinue(context => Sap != null,
                                                    new DecoratorContinue(context => !Sap.HasAura(6770) && SpellManager.CanCast(6770),
                                                        new Sequence(
                                                            new DecoratorContinue(context => Me.CurrentTarget != Sap,
                                                                new Styx.TreeSharp.Action(context => Sap.Target())
                                                            ),
                                                            new DecoratorContinue(context => Me.CurrentTarget == Sap,
                                                                new Styx.TreeSharp.Action(context => SpellManager.Cast(6770))
                                                            )
                                                        )
                                                    )
                                                ),
            #endregion
 new DecoratorContinue(context => Target.IsAlive,
                                                    new Sequence(
                                                        new Styx.TreeSharp.Action(context => TargetLocation = WoWMovement.CalculatePointFrom(Target.Location, 0)),
                                                        new Styx.TreeSharp.Action(context => Navigator.MoveTo(TargetLocation))
                                                    )
                                                ),
                                                new DecoratorContinue(context => Target.IsDead,
                                                    new Sequence(
                                                        new Styx.TreeSharp.Action(context => Target = null),
                                                        new Styx.TreeSharp.Action(context => TargetLocation = WoWPoint.Empty)
                                                    )
                                                )
                                            )
                                        ),
            #endregion
            #region Cast Pick Pocket (921)
 new DecoratorContinue(context => TargetLocation.Distance(Me.Location) <= RoguePickPocket_Settings.Instance.RPPTrackBar_PPFY,
                                            new Sequence(
                                                new Styx.TreeSharp.Action(context => WoWMovement.MoveStop()),
            #region Cast Sap (6770)
 new Styx.TreeSharp.Action(context => ObjectManager.Update()),
                                                new Styx.TreeSharp.Action(context => Sap = null),
                                                new Styx.TreeSharp.Action(context => Sap = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsAlive && u.CanSelect && u.IsHostile && u.InLineOfSight && (u.Distance <= 10) && !u.Combat).OrderBy(u => u.Distance).FirstOrDefault()),
                                                new DecoratorContinue(context => Sap != null,
                                                    new DecoratorContinue(context => !Sap.HasAura(6770) && SpellManager.CanCast(6770),
                                                        new Sequence(
                                                            new DecoratorContinue(context => Me.CurrentTarget != Sap,
                                                                new Sequence(
                                                                    new Styx.TreeSharp.Action(context => Sap.Target()),
                                                                    new Styx.TreeSharp.Action(context => Sap.Face())
                                                                )
                                                            ),
                                                            new DecoratorContinue(context => Me.CurrentTarget == Sap,
                                                                new Styx.TreeSharp.Action(context => SpellManager.Cast(6770))
                                                            )
                                                        )
                                                    )
                                                ),
            #endregion
 new DecoratorContinue(context => SpellManager.CanCast(921),
                                                    new Sequence(
                                                        new DecoratorContinue(context => Me.CurrentTarget != Target,
                                                            new Styx.TreeSharp.Action(context => Target.Target())
                                                        ),
                                                        new DecoratorContinue(context => Me.CurrentTarget == Target,
                                                            new Sequence(
                                                                new Styx.TreeSharp.Action(context => SpellManager.Cast(921)),
                                                                new WaitContinue(TimeSpan.FromMilliseconds(1200), context => false, new ActionAlwaysSucceed()),
                                                                new Styx.TreeSharp.Action(context => PriorityTreeState.LocalBlacklist.Add(Target, TimeSpan.FromSeconds(600))),
                                                                new Styx.TreeSharp.Action(context => TargetLocation = WoWPoint.Empty),
                                                                new Styx.TreeSharp.Action(context => Me.ClearTarget())
                                                            )
                                                        )
                                                    )
                                                ),
                                                new DecoratorContinue(context => !SpellManager.CanCast(921),
                                                    new Sequence(
                                                        new Styx.TreeSharp.Action(context => PriorityTreeState.LocalBlacklist.Add(Target, TimeSpan.FromSeconds(600))),
                                                        new Styx.TreeSharp.Action(context => TargetLocation = WoWPoint.Empty)
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
          */
    }
}
