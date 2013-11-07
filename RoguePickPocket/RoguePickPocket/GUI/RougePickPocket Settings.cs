#region Using

using System.IO;
using Styx.Common;
using Styx.Helpers;

#endregion

namespace RoguePickPocket.GUI {
    public class RoguePickPocketSettings : Settings {
        #region Variables
        public static readonly RoguePickPocketSettings Instance = new RoguePickPocketSettings();
        public RoguePickPocketSettings()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Settings\{0}\{0}.xml", "RoguePickPocket"))) {
            Load();
        }
        #endregion

        #region Loot Corpses
        [Setting, DefaultValue(true)]
        public bool LootCorpsesCheckBox { get; set; }
        #endregion

        #region PickPocket Range
        [Setting, DefaultValue(5)]
        public int PickPocketFromYardsTrackBar { get; set; }
        #endregion
        
        [Setting, DefaultValue(true)]
        public bool PickLockLockboxCheckbox { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool OpenLockboxCheckbox { get; set; }
    }
}
