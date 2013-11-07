#region Namespaces
using System;
using System.Windows.Forms;
using RoguePickPocket.GUI;
using RoguePickPocket.Helpers;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
#endregion

namespace RoguePickPocket {
    public class RoguePickPocket : BotBase {
        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        private static Composite _root;
        private static CustomLogging _customLog = new CustomLogging("RPP");
        private static Version _version = new Version(0, 8, 0);

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        public static CustomLogging CustomLog {
            get { return _customLog; }
            set { _customLog = value; }
        }

        public static Version Version {
            get { return _version; }
            set { _version = value; }
        }

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        public override string Name { get { return "RoguePickPocket"; } }

        public override Composite Root { get { return _root ?? (_root = CreateRoot()); } }

        public override PulseFlags PulseFlags { get { return PulseFlags.All; } }

        public override void Start() {
            try {
                ProfileManager.LoadEmpty();

                Targeting.Instance.IncludeTargetsFilter += Combat.IncludeTargetsFilter;

                if(PriorityTreeState.StartPoint == WoWPoint.Empty) {
                    PriorityTreeState.StartPoint = Character.Me.Location;
                }

                _customLog.CustomNormalLog("{0} version {1} initialized.", Name, _version);
            } catch(Exception e) {
                _customLog.CustomDiagnosticLog(e.ToString());
            }
        }

        public override void Stop() {
            Targeting.Instance.IncludeTargetsFilter -= Combat.IncludeTargetsFilter;

            PriorityTreeState.TargetLocation = WoWPoint.Empty;
            PriorityTreeState.NextTarget = null;
            PriorityTreeState.CurrentTarget = null;
            PriorityTreeState.StartPoint = WoWPoint.Empty;

            LocalBlacklist.RemoveAll();

            _customLog.CustomNormalLog("{0} shutdown complete.", Name);
        }

        public override Form ConfigurationForm {
            get {
                var gui = new RoguePickPocketGUI();
                gui.Activate();
                return gui;
            }
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static Composite CreateRoot() {
            return new PrioritySelector(
                new Action(context => PriorityTreeState.TreeStateHandler())
            );
        }
    }
}