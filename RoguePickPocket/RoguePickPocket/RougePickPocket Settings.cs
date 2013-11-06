#region Using
using System.IO;
using Styx.Common;
using Styx.Helpers;
#endregion

namespace RoguePickPocket {
    public class RoguePickPocket_Settings : Settings {
        #region Variables
        public static readonly RoguePickPocket_Settings Instance = new RoguePickPocket_Settings();
        public RoguePickPocket_Settings()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Settings\{0}\{0}.xml", "RoguePickPocket"))) {
            Load();
        }
        #endregion

        #region Loot Corpses
        [Setting, DefaultValue(true)]
        public bool RPPCheckBox_LC { get; set; }
        #endregion

        #region PickPocket Range
        [Setting, DefaultValue(5)]
        public int RPPTrackBar_PPFY { get; set; }
        #endregion
    }
}
