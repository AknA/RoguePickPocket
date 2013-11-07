namespace RoguePickPocket.GUI {
    partial class RoguePickPocketGUI {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.openLockboxCheckbox = new System.Windows.Forms.CheckBox();
            this.pickLockLockboxCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.RPPCheckBox_AE = new System.Windows.Forms.CheckBox();
            this.RPPCheckBox_LC = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pickPocketFromYardsTrackBar = new System.Windows.Forms.TrackBar();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.RPPRadioButton_USON = new System.Windows.Forms.RadioButton();
            this.RPPRadioButton_DUS = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pickPocketFromYardsTrackBar)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, -2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(801, 496);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(793, 470);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox9);
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox8);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Location = new System.Drawing.Point(6, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 399);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Settings";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.openLockboxCheckbox);
            this.groupBox9.Controls.Add(this.pickLockLockboxCheckbox);
            this.groupBox9.Location = new System.Drawing.Point(8, 178);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(200, 71);
            this.groupBox9.TabIndex = 4;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Lockboxes";
            // 
            // openLockboxCheckbox
            // 
            this.openLockboxCheckbox.AutoSize = true;
            this.openLockboxCheckbox.Checked = true;
            this.openLockboxCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openLockboxCheckbox.Location = new System.Drawing.Point(10, 44);
            this.openLockboxCheckbox.Name = "openLockboxCheckbox";
            this.openLockboxCheckbox.Size = new System.Drawing.Size(96, 17);
            this.openLockboxCheckbox.TabIndex = 1;
            this.openLockboxCheckbox.Text = "Open Lockbox";
            this.openLockboxCheckbox.UseVisualStyleBackColor = true;
            this.openLockboxCheckbox.CheckedChanged += new System.EventHandler(this.openLockboxCheckbox_CheckedChanged);
            this.openLockboxCheckbox.MouseHover += new System.EventHandler(this.openLockboxCheckbox_MouseHover);
            // 
            // pickLockLockboxCheckbox
            // 
            this.pickLockLockboxCheckbox.AutoSize = true;
            this.pickLockLockboxCheckbox.Checked = true;
            this.pickLockLockboxCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pickLockLockboxCheckbox.Location = new System.Drawing.Point(10, 20);
            this.pickLockLockboxCheckbox.Name = "pickLockLockboxCheckbox";
            this.pickLockLockboxCheckbox.Size = new System.Drawing.Size(118, 17);
            this.pickLockLockboxCheckbox.TabIndex = 0;
            this.pickLockLockboxCheckbox.Text = "Pick Lock Lockbox";
            this.pickLockLockboxCheckbox.UseVisualStyleBackColor = true;
            this.pickLockLockboxCheckbox.CheckedChanged += new System.EventHandler(this.pickLockLockboxCheckbox_CheckedChanged);
            this.pickLockLockboxCheckbox.MouseHover += new System.EventHandler(this.pickLockLockboxCheckbox_MouseHover);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.RPPCheckBox_AE);
            this.groupBox7.Controls.Add(this.RPPCheckBox_LC);
            this.groupBox7.Location = new System.Drawing.Point(8, 255);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 62);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Misc Settings";
            // 
            // RPPCheckBox_AE
            // 
            this.RPPCheckBox_AE.AutoSize = true;
            this.RPPCheckBox_AE.BackColor = System.Drawing.Color.Transparent;
            this.RPPCheckBox_AE.Enabled = false;
            this.RPPCheckBox_AE.Location = new System.Drawing.Point(10, 42);
            this.RPPCheckBox_AE.Name = "RPPCheckBox_AE";
            this.RPPCheckBox_AE.Size = new System.Drawing.Size(81, 17);
            this.RPPCheckBox_AE.TabIndex = 3;
            this.RPPCheckBox_AE.Text = "Avoid Elites";
            this.RPPCheckBox_AE.UseVisualStyleBackColor = false;
            // 
            // RPPCheckBox_LC
            // 
            this.RPPCheckBox_LC.AutoSize = true;
            this.RPPCheckBox_LC.Location = new System.Drawing.Point(10, 19);
            this.RPPCheckBox_LC.Name = "RPPCheckBox_LC";
            this.RPPCheckBox_LC.Size = new System.Drawing.Size(88, 17);
            this.RPPCheckBox_LC.TabIndex = 0;
            this.RPPCheckBox_LC.Text = "Loot Corpses";
            this.RPPCheckBox_LC.UseVisualStyleBackColor = true;
            this.RPPCheckBox_LC.CheckedChanged += new System.EventHandler(this.RPPCheckBox_LC_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.pickPocketFromYardsTrackBar);
            this.groupBox2.Location = new System.Drawing.Point(8, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 75);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PickPocket From Yards";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "    1    2    3    4    5    6    7    8    9   10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 1;
            // 
            // pickPocketFromYardsTrackBar
            // 
            this.pickPocketFromYardsTrackBar.Location = new System.Drawing.Point(7, 20);
            this.pickPocketFromYardsTrackBar.Minimum = 1;
            this.pickPocketFromYardsTrackBar.Name = "pickPocketFromYardsTrackBar";
            this.pickPocketFromYardsTrackBar.Size = new System.Drawing.Size(187, 45);
            this.pickPocketFromYardsTrackBar.TabIndex = 0;
            this.pickPocketFromYardsTrackBar.Value = 5;
            this.pickPocketFromYardsTrackBar.Scroll += new System.EventHandler(this.RPPTrackBar_PPFY_Scroll);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.RPPRadioButton_USON);
            this.groupBox8.Controls.Add(this.RPPRadioButton_DUS);
            this.groupBox8.Location = new System.Drawing.Point(8, 102);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(200, 70);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Sap Control";
            // 
            // RPPRadioButton_USON
            // 
            this.RPPRadioButton_USON.AutoSize = true;
            this.RPPRadioButton_USON.BackColor = System.Drawing.Color.Transparent;
            this.RPPRadioButton_USON.Enabled = false;
            this.RPPRadioButton_USON.Location = new System.Drawing.Point(10, 44);
            this.RPPRadioButton_USON.Name = "RPPRadioButton_USON";
            this.RPPRadioButton_USON.Size = new System.Drawing.Size(147, 17);
            this.RPPRadioButton_USON.TabIndex = 1;
            this.RPPRadioButton_USON.TabStop = true;
            this.RPPRadioButton_USON.Text = "Use Sap On Nearest Mob";
            this.RPPRadioButton_USON.UseVisualStyleBackColor = false;
            // 
            // RPPRadioButton_DUS
            // 
            this.RPPRadioButton_DUS.AutoSize = true;
            this.RPPRadioButton_DUS.BackColor = System.Drawing.Color.Transparent;
            this.RPPRadioButton_DUS.Enabled = false;
            this.RPPRadioButton_DUS.Location = new System.Drawing.Point(10, 20);
            this.RPPRadioButton_DUS.Name = "RPPRadioButton_DUS";
            this.RPPRadioButton_DUS.Size = new System.Drawing.Size(94, 17);
            this.RPPRadioButton_DUS.TabIndex = 0;
            this.RPPRadioButton_DUS.TabStop = true;
            this.RPPRadioButton_DUS.Text = "Don\'t Use Sap";
            this.RPPRadioButton_DUS.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(213, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 297);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Info";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(187, 268);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "This is just the first ver of the UI. Everything isn\'t working yet, and everythin" +
    "g isn\'t added yet.";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(439, 414);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Location = new System.Drawing.Point(8, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(423, 305);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Advanced Settings";
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(213, 20);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 279);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(7, 20);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 279);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(194, 458);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RoguePickPocketGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(803, 493);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RoguePickPocketGUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RPP GUI";
            this.Load += new System.EventHandler(this.RoguePickPocketGUI_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pickPocketFromYardsTrackBar)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar pickPocketFromYardsTrackBar;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton RPPRadioButton_USON;
        private System.Windows.Forms.RadioButton RPPRadioButton_DUS;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox RPPCheckBox_AE;
        private System.Windows.Forms.CheckBox RPPCheckBox_LC;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox openLockboxCheckbox;
        private System.Windows.Forms.CheckBox pickLockLockboxCheckbox;
    }
}