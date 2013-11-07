#region Using

using System;
using System.Windows.Forms;

#endregion

namespace RoguePickPocket.GUI {
    public partial class RoguePickPocketGUI : Form
    {

        private readonly ToolTip _tooltip = new ToolTip();
        
        public RoguePickPocketGUI() {
            InitializeComponent();
        }

        #region Variables
        private void RoguePickPocketGUI_Load(object sender, EventArgs e) {
            RoguePickPocketSettings.Instance.Load();
            RPPCheckBox_LC.Checked = RoguePickPocketSettings.Instance.LootCorpsesCheckBox;
            pickPocketFromYardsTrackBar.Value = RoguePickPocketSettings.Instance.PickPocketFromYardsTrackBar;
        }
        #endregion

        #region Save Button
        private void SaveButton_Click(object sender, EventArgs e) {
            MessageBox.Show("Settings have been saved.", "Save");
            RoguePickPocketSettings.Instance.Save();
            RoguePickPocket.CustomLog.CustomNormalLog("Settings saved.");
            Close();
        }
        #endregion

        #region Loot Corpses
        private void RPPCheckBox_LC_CheckedChanged(object sender, EventArgs e) {
            RoguePickPocketSettings.Instance.LootCorpsesCheckBox = RPPCheckBox_LC.Checked;
        }
        #endregion

        #region PickPocket Range
        private void RPPTrackBar_PPFY_Scroll(object sender, EventArgs e) {
            RoguePickPocketSettings.Instance.PickPocketFromYardsTrackBar = pickPocketFromYardsTrackBar.Value;
        }
        #endregion


        private void pickLockLockboxCheckbox_MouseHover(object sender, EventArgs e) {
            _tooltip.Show("This option pick locks all lockboxes in your inventory.", this);
        }

        private void openLockboxCheckbox_MouseHover(object sender, EventArgs e) {
            _tooltip.Show("This option opens all lockboxes in your inventory.", this);
        }

        private void pickLockLockboxCheckbox_CheckedChanged(object sender, EventArgs e) {
            RoguePickPocketSettings.Instance.PickLockLockboxCheckbox = pickLockLockboxCheckbox.Checked;
        }

        private void openLockboxCheckbox_CheckedChanged(object sender, EventArgs e) {
            RoguePickPocketSettings.Instance.OpenLockboxCheckbox = openLockboxCheckbox.Checked;
        }

        

        

        

        
    }
}
