#region Using
using System;
using System.Windows.Forms;
#endregion

namespace RoguePickPocket {
    public partial class RoguePickPocket_GUI : Form {
        public RoguePickPocket_GUI() {
            InitializeComponent();
        }

        #region Variables
        private void RoguePickPocket_GUI_Load(object sender, EventArgs e) {
            RoguePickPocket_Settings.Instance.Load();
            RPPCheckBox_LC.Checked = RoguePickPocket_Settings.Instance.RPPCheckBox_LC;
            RPPTrackBar_PPFY.Value = RoguePickPocket_Settings.Instance.RPPTrackBar_PPFY;
        }
        #endregion

        #region Save Button
        private void SaveButton_Click(object sender, EventArgs e) {
            MessageBox.Show("RPP Settings have been saved.", "Save");
            RoguePickPocket_Settings.Instance.Save();
            RoguePickPocket.RPPLog("Settings Saved");
            Close();
        }
        #endregion

        #region Loot Corpses
        private void RPPCheckBox_LC_CheckedChanged(object sender, EventArgs e) {
            RoguePickPocket_Settings.Instance.RPPCheckBox_LC = RPPCheckBox_LC.Checked;
        }
        #endregion

        #region PickPocket Range
        private void RPPTrackBar_PPFY_Scroll(object sender, EventArgs e) {
            RoguePickPocket_Settings.Instance.RPPTrackBar_PPFY = RPPTrackBar_PPFY.Value;
        }
        #endregion
    }
}
