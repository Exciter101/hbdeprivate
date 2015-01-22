namespace DK
{
    partial class DKGui
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cmbHotkeyCooldowns = new System.Windows.Forms.ComboBox();
            this.cmbModifKeyResDps = new System.Windows.Forms.ComboBox();
            this.cmbModifKeyResHealers = new System.Windows.Forms.ComboBox();
            this.cmbModifKeyResTanks = new System.Windows.Forms.ComboBox();
            this.cmbModifKeyPlayManual = new System.Windows.Forms.ComboBox();
            this.cmbModifKeyStopAoe = new System.Windows.Forms.ComboBox();
            this.cmbModifKeyPause = new System.Windows.Forms.ComboBox();
            this.cmbModifKeyCooldowns = new System.Windows.Forms.ComboBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnPresenceUnholy = new System.Windows.Forms.RadioButton();
            this.btnPresenceManual = new System.Windows.Forms.RadioButton();
            this.btnPresenceBlood = new System.Windows.Forms.RadioButton();
            this.btnPresenceFrost = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbHotkeyPause = new System.Windows.Forms.ComboBox();
            this.cmbHotkeyStopAoe = new System.Windows.Forms.ComboBox();
            this.cmbHotkeyPlayManual = new System.Windows.Forms.ComboBox();
            this.cmbHotkeyResTanks = new System.Windows.Forms.ComboBox();
            this.cmbHotkeyResHealers = new System.Windows.Forms.ComboBox();
            this.cmbHotkeyResDps = new System.Windows.Forms.ComboBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(7, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 93);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Movement";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(24, 65);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(83, 17);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "Auto Facing";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(24, 42);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(96, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Auto Targeting";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(24, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(101, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Auto Movement";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox6);
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Location = new System.Drawing.Point(6, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(221, 93);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Movement in Group";
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(25, 67);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(96, 17);
            this.checkBox6.TabIndex = 3;
            this.checkBox6.Text = "Disable Facing";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(25, 44);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(109, 17);
            this.checkBox5.TabIndex = 2;
            this.checkBox5.Text = "Disable Targeting";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(25, 21);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(114, 17);
            this.checkBox4.TabIndex = 1;
            this.checkBox4.Text = "Disable Movement";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(638, 371);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(630, 345);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.checkBox9);
            this.groupBox7.Controls.Add(this.cmbHotkeyResDps);
            this.groupBox7.Controls.Add(this.cmbHotkeyResHealers);
            this.groupBox7.Controls.Add(this.cmbHotkeyResTanks);
            this.groupBox7.Controls.Add(this.cmbHotkeyPlayManual);
            this.groupBox7.Controls.Add(this.cmbHotkeyStopAoe);
            this.groupBox7.Controls.Add(this.cmbHotkeyPause);
            this.groupBox7.Controls.Add(this.cmbHotkeyCooldowns);
            this.groupBox7.Controls.Add(this.cmbModifKeyResDps);
            this.groupBox7.Controls.Add(this.cmbModifKeyResHealers);
            this.groupBox7.Controls.Add(this.cmbModifKeyResTanks);
            this.groupBox7.Controls.Add(this.cmbModifKeyPlayManual);
            this.groupBox7.Controls.Add(this.cmbModifKeyStopAoe);
            this.groupBox7.Controls.Add(this.cmbModifKeyPause);
            this.groupBox7.Controls.Add(this.cmbModifKeyCooldowns);
            this.groupBox7.Controls.Add(this.textBox9);
            this.groupBox7.Controls.Add(this.textBox8);
            this.groupBox7.Controls.Add(this.textBox7);
            this.groupBox7.Controls.Add(this.textBox6);
            this.groupBox7.Controls.Add(this.textBox5);
            this.groupBox7.Controls.Add(this.textBox4);
            this.groupBox7.Controls.Add(this.textBox3);
            this.groupBox7.Location = new System.Drawing.Point(248, 9);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(293, 262);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Hotkeys [Using, Modfier, Key]";
            // 
            // cmbHotkeyCooldowns
            // 
            this.cmbHotkeyCooldowns.FormattingEnabled = true;
            this.cmbHotkeyCooldowns.Items.AddRange(new object[] {
            "None",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.cmbHotkeyCooldowns.Location = new System.Drawing.Point(203, 22);
            this.cmbHotkeyCooldowns.Name = "cmbHotkeyCooldowns";
            this.cmbHotkeyCooldowns.Size = new System.Drawing.Size(67, 21);
            this.cmbHotkeyCooldowns.TabIndex = 14;
            this.cmbHotkeyCooldowns.SelectedIndexChanged += new System.EventHandler(this.cmbHotkeyCooldowns_SelectedIndexChanged);
            // 
            // cmbModifKeyResDps
            // 
            this.cmbModifKeyResDps.FormattingEnabled = true;
            this.cmbModifKeyResDps.Items.AddRange(new object[] {
            "Alt",
            "Control",
            "Shift",
            "Windows"});
            this.cmbModifKeyResDps.Location = new System.Drawing.Point(133, 178);
            this.cmbModifKeyResDps.Name = "cmbModifKeyResDps";
            this.cmbModifKeyResDps.Size = new System.Drawing.Size(64, 21);
            this.cmbModifKeyResDps.TabIndex = 13;
            this.cmbModifKeyResDps.SelectedIndexChanged += new System.EventHandler(this.cmbModifKeyResDps_SelectedIndexChanged);
            // 
            // cmbModifKeyResHealers
            // 
            this.cmbModifKeyResHealers.FormattingEnabled = true;
            this.cmbModifKeyResHealers.Items.AddRange(new object[] {
            "Alt",
            "Control",
            "Shift",
            "Windows"});
            this.cmbModifKeyResHealers.Location = new System.Drawing.Point(133, 152);
            this.cmbModifKeyResHealers.Name = "cmbModifKeyResHealers";
            this.cmbModifKeyResHealers.Size = new System.Drawing.Size(64, 21);
            this.cmbModifKeyResHealers.TabIndex = 12;
            this.cmbModifKeyResHealers.SelectedIndexChanged += new System.EventHandler(this.cmbModifKeyResHealers_SelectedIndexChanged);
            // 
            // cmbModifKeyResTanks
            // 
            this.cmbModifKeyResTanks.FormattingEnabled = true;
            this.cmbModifKeyResTanks.Items.AddRange(new object[] {
            "Alt",
            "Control",
            "Shift",
            "Windows"});
            this.cmbModifKeyResTanks.Location = new System.Drawing.Point(133, 126);
            this.cmbModifKeyResTanks.Name = "cmbModifKeyResTanks";
            this.cmbModifKeyResTanks.Size = new System.Drawing.Size(64, 21);
            this.cmbModifKeyResTanks.TabIndex = 11;
            this.cmbModifKeyResTanks.SelectedIndexChanged += new System.EventHandler(this.cmbModifKeyResTanks_SelectedIndexChanged);
            // 
            // cmbModifKeyPlayManual
            // 
            this.cmbModifKeyPlayManual.FormattingEnabled = true;
            this.cmbModifKeyPlayManual.Items.AddRange(new object[] {
            "Alt",
            "Control",
            "Shift",
            "Windows"});
            this.cmbModifKeyPlayManual.Location = new System.Drawing.Point(133, 100);
            this.cmbModifKeyPlayManual.Name = "cmbModifKeyPlayManual";
            this.cmbModifKeyPlayManual.Size = new System.Drawing.Size(64, 21);
            this.cmbModifKeyPlayManual.TabIndex = 10;
            this.cmbModifKeyPlayManual.SelectedIndexChanged += new System.EventHandler(this.cmbModifKeyPlayManual_SelectedIndexChanged);
            // 
            // cmbModifKeyStopAoe
            // 
            this.cmbModifKeyStopAoe.FormattingEnabled = true;
            this.cmbModifKeyStopAoe.Items.AddRange(new object[] {
            "Alt",
            "Control",
            "Shift",
            "Windows"});
            this.cmbModifKeyStopAoe.Location = new System.Drawing.Point(133, 74);
            this.cmbModifKeyStopAoe.Name = "cmbModifKeyStopAoe";
            this.cmbModifKeyStopAoe.Size = new System.Drawing.Size(64, 21);
            this.cmbModifKeyStopAoe.TabIndex = 9;
            this.cmbModifKeyStopAoe.SelectedIndexChanged += new System.EventHandler(this.cmbModifKeyStopAoe_SelectedIndexChanged);
            // 
            // cmbModifKeyPause
            // 
            this.cmbModifKeyPause.FormattingEnabled = true;
            this.cmbModifKeyPause.Items.AddRange(new object[] {
            "Alt",
            "Control",
            "Shift",
            "Windows"});
            this.cmbModifKeyPause.Location = new System.Drawing.Point(133, 48);
            this.cmbModifKeyPause.Name = "cmbModifKeyPause";
            this.cmbModifKeyPause.Size = new System.Drawing.Size(64, 21);
            this.cmbModifKeyPause.TabIndex = 8;
            this.cmbModifKeyPause.SelectedIndexChanged += new System.EventHandler(this.cmbModifKeyPause_SelectedIndexChanged);
            // 
            // cmbModifKeyCooldowns
            // 
            this.cmbModifKeyCooldowns.FormattingEnabled = true;
            this.cmbModifKeyCooldowns.Items.AddRange(new object[] {
            "Alt",
            "Control",
            "Shift",
            "Windows"});
            this.cmbModifKeyCooldowns.Location = new System.Drawing.Point(133, 22);
            this.cmbModifKeyCooldowns.Name = "cmbModifKeyCooldowns";
            this.cmbModifKeyCooldowns.Size = new System.Drawing.Size(64, 21);
            this.cmbModifKeyCooldowns.TabIndex = 7;
            this.cmbModifKeyCooldowns.SelectedIndexChanged += new System.EventHandler(this.cmbModifKeyCooldowns_SelectedIndexChanged);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(17, 178);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(110, 20);
            this.textBox9.TabIndex = 6;
            this.textBox9.Text = "Combat Res DPS";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(17, 152);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(110, 20);
            this.textBox8.TabIndex = 5;
            this.textBox8.Text = "Combat Res Healers";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(17, 126);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(110, 20);
            this.textBox7.TabIndex = 4;
            this.textBox7.Text = "Combat Res Tanks";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(17, 100);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(110, 20);
            this.textBox6.TabIndex = 3;
            this.textBox6.Text = "Play Manual";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(17, 74);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(110, 20);
            this.textBox5.TabIndex = 2;
            this.textBox5.Text = "Stop Aoe";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(17, 48);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(110, 20);
            this.textBox4.TabIndex = 1;
            this.textBox4.Text = "Pause Rotation";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(17, 22);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(110, 20);
            this.textBox3.TabIndex = 0;
            this.textBox3.Text = "Use Cooldowns";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnPresenceUnholy);
            this.groupBox3.Controls.Add(this.btnPresenceManual);
            this.groupBox3.Controls.Add(this.btnPresenceBlood);
            this.groupBox3.Controls.Add(this.btnPresenceFrost);
            this.groupBox3.Location = new System.Drawing.Point(7, 201);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(220, 70);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Presence";
            // 
            // btnPresenceUnholy
            // 
            this.btnPresenceUnholy.AutoSize = true;
            this.btnPresenceUnholy.Location = new System.Drawing.Point(125, 42);
            this.btnPresenceUnholy.Name = "btnPresenceUnholy";
            this.btnPresenceUnholy.Size = new System.Drawing.Size(58, 17);
            this.btnPresenceUnholy.TabIndex = 3;
            this.btnPresenceUnholy.Text = "Unholy";
            this.btnPresenceUnholy.UseVisualStyleBackColor = true;
            this.btnPresenceUnholy.CheckedChanged += new System.EventHandler(this.btnPresenceUnholy_CheckedChanged);
            // 
            // btnPresenceManual
            // 
            this.btnPresenceManual.AutoSize = true;
            this.btnPresenceManual.Location = new System.Drawing.Point(25, 22);
            this.btnPresenceManual.Name = "btnPresenceManual";
            this.btnPresenceManual.Size = new System.Drawing.Size(60, 17);
            this.btnPresenceManual.TabIndex = 2;
            this.btnPresenceManual.Text = "Manual";
            this.btnPresenceManual.UseVisualStyleBackColor = true;
            this.btnPresenceManual.CheckedChanged += new System.EventHandler(this.btnPresenceManual_CheckedChanged);
            // 
            // btnPresenceBlood
            // 
            this.btnPresenceBlood.AutoSize = true;
            this.btnPresenceBlood.Checked = true;
            this.btnPresenceBlood.Location = new System.Drawing.Point(125, 22);
            this.btnPresenceBlood.Name = "btnPresenceBlood";
            this.btnPresenceBlood.Size = new System.Drawing.Size(52, 17);
            this.btnPresenceBlood.TabIndex = 1;
            this.btnPresenceBlood.TabStop = true;
            this.btnPresenceBlood.Text = "Blood";
            this.btnPresenceBlood.UseVisualStyleBackColor = true;
            this.btnPresenceBlood.CheckedChanged += new System.EventHandler(this.btnPresenceBlood_CheckedChanged);
            // 
            // btnPresenceFrost
            // 
            this.btnPresenceFrost.AutoSize = true;
            this.btnPresenceFrost.Location = new System.Drawing.Point(25, 42);
            this.btnPresenceFrost.Name = "btnPresenceFrost";
            this.btnPresenceFrost.Size = new System.Drawing.Size(48, 17);
            this.btnPresenceFrost.TabIndex = 0;
            this.btnPresenceFrost.Text = "Frost";
            this.btnPresenceFrost.UseVisualStyleBackColor = true;
            this.btnPresenceFrost.CheckedChanged += new System.EventHandler(this.btnPresenceFrost_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(630, 345);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Blood DK";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(259, 9);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(274, 330);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Protection";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.numericUpDown4);
            this.groupBox5.Controls.Add(this.numericUpDown3);
            this.groupBox5.Controls.Add(this.checkBox8);
            this.groupBox5.Controls.Add(this.textBox2);
            this.groupBox5.Location = new System.Drawing.Point(9, 91);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(244, 73);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Defile";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(194, 42);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown4.TabIndex = 5;
            this.numericUpDown4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown4.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(194, 19);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown3.TabIndex = 4;
            this.numericUpDown3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(8, 45);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(165, 17);
            this.checkBox8.TabIndex = 3;
            this.checkBox8.Text = "Use Deathrunes when count ";
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(8, 19);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(180, 20);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "Need number of adds before casting";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.numericUpDown2);
            this.groupBox4.Controls.Add(this.checkBox7);
            this.groupBox4.Controls.Add(this.numericUpDown1);
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.Location = new System.Drawing.Point(9, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(244, 82);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Death and Decay";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(194, 50);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown2.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(8, 51);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(165, 17);
            this.checkBox7.TabIndex = 2;
            this.checkBox7.Text = "Use Deathrunes when count ";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(194, 22);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(180, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Need number of adds before casting";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(498, 378);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 21);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save Settings";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbHotkeyPause
            // 
            this.cmbHotkeyPause.FormattingEnabled = true;
            this.cmbHotkeyPause.Items.AddRange(new object[] {
            "None",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.cmbHotkeyPause.Location = new System.Drawing.Point(203, 48);
            this.cmbHotkeyPause.Name = "cmbHotkeyPause";
            this.cmbHotkeyPause.Size = new System.Drawing.Size(67, 21);
            this.cmbHotkeyPause.TabIndex = 15;
            this.cmbHotkeyPause.SelectedIndexChanged += new System.EventHandler(this.cmbHotkeyPause_SelectedIndexChanged);
            // 
            // cmbHotkeyStopAoe
            // 
            this.cmbHotkeyStopAoe.FormattingEnabled = true;
            this.cmbHotkeyStopAoe.Items.AddRange(new object[] {
            "None",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.cmbHotkeyStopAoe.Location = new System.Drawing.Point(203, 74);
            this.cmbHotkeyStopAoe.Name = "cmbHotkeyStopAoe";
            this.cmbHotkeyStopAoe.Size = new System.Drawing.Size(67, 21);
            this.cmbHotkeyStopAoe.TabIndex = 16;
            this.cmbHotkeyStopAoe.SelectedIndexChanged += new System.EventHandler(this.cmbHotkeyStopAoe_SelectedIndexChanged);
            // 
            // cmbHotkeyPlayManual
            // 
            this.cmbHotkeyPlayManual.FormattingEnabled = true;
            this.cmbHotkeyPlayManual.Items.AddRange(new object[] {
            "None",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.cmbHotkeyPlayManual.Location = new System.Drawing.Point(203, 100);
            this.cmbHotkeyPlayManual.Name = "cmbHotkeyPlayManual";
            this.cmbHotkeyPlayManual.Size = new System.Drawing.Size(67, 21);
            this.cmbHotkeyPlayManual.TabIndex = 17;
            this.cmbHotkeyPlayManual.SelectedIndexChanged += new System.EventHandler(this.cmbHotkeyPlayManual_SelectedIndexChanged);
            // 
            // cmbHotkeyResTanks
            // 
            this.cmbHotkeyResTanks.FormattingEnabled = true;
            this.cmbHotkeyResTanks.Items.AddRange(new object[] {
            "None",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.cmbHotkeyResTanks.Location = new System.Drawing.Point(203, 126);
            this.cmbHotkeyResTanks.Name = "cmbHotkeyResTanks";
            this.cmbHotkeyResTanks.Size = new System.Drawing.Size(67, 21);
            this.cmbHotkeyResTanks.TabIndex = 18;
            this.cmbHotkeyResTanks.SelectedIndexChanged += new System.EventHandler(this.cmbHotkeyResTanks_SelectedIndexChanged);
            // 
            // cmbHotkeyResHealers
            // 
            this.cmbHotkeyResHealers.FormattingEnabled = true;
            this.cmbHotkeyResHealers.Items.AddRange(new object[] {
            "None",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.cmbHotkeyResHealers.Location = new System.Drawing.Point(203, 152);
            this.cmbHotkeyResHealers.Name = "cmbHotkeyResHealers";
            this.cmbHotkeyResHealers.Size = new System.Drawing.Size(67, 21);
            this.cmbHotkeyResHealers.TabIndex = 19;
            this.cmbHotkeyResHealers.SelectedIndexChanged += new System.EventHandler(this.cmbHotkeyResHealers_SelectedIndexChanged);
            // 
            // cmbHotkeyResDps
            // 
            this.cmbHotkeyResDps.FormattingEnabled = true;
            this.cmbHotkeyResDps.Items.AddRange(new object[] {
            "None",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.cmbHotkeyResDps.Location = new System.Drawing.Point(203, 178);
            this.cmbHotkeyResDps.Name = "cmbHotkeyResDps";
            this.cmbHotkeyResDps.Size = new System.Drawing.Size(67, 21);
            this.cmbHotkeyResDps.TabIndex = 20;
            this.cmbHotkeyResDps.SelectedIndexChanged += new System.EventHandler(this.cmbHotkeyResDps_SelectedIndexChanged);
            // 
            // checkBox9
            // 
            this.checkBox9.AutoSize = true;
            this.checkBox9.Location = new System.Drawing.Point(65, 225);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(161, 17);
            this.checkBox9.TabIndex = 21;
            this.checkBox9.Text = "Enable Raid Style Messages";
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
            // 
            // DKGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 411);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Name = "DKGui";
            this.Text = "DKgui";
            this.Load += new System.EventHandler(this.DKgui_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton btnPresenceUnholy;
        private System.Windows.Forms.RadioButton btnPresenceManual;
        private System.Windows.Forms.RadioButton btnPresenceBlood;
        private System.Windows.Forms.RadioButton btnPresenceFrost;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ComboBox cmbModifKeyCooldowns;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ComboBox cmbModifKeyResDps;
        private System.Windows.Forms.ComboBox cmbModifKeyResHealers;
        private System.Windows.Forms.ComboBox cmbModifKeyResTanks;
        private System.Windows.Forms.ComboBox cmbModifKeyPlayManual;
        private System.Windows.Forms.ComboBox cmbModifKeyStopAoe;
        private System.Windows.Forms.ComboBox cmbModifKeyPause;
        private System.Windows.Forms.ComboBox cmbHotkeyCooldowns;
        private System.Windows.Forms.ComboBox cmbHotkeyResDps;
        private System.Windows.Forms.ComboBox cmbHotkeyResHealers;
        private System.Windows.Forms.ComboBox cmbHotkeyResTanks;
        private System.Windows.Forms.ComboBox cmbHotkeyPlayManual;
        private System.Windows.Forms.ComboBox cmbHotkeyStopAoe;
        private System.Windows.Forms.ComboBox cmbHotkeyPause;
        private System.Windows.Forms.CheckBox checkBox9;
    }
}