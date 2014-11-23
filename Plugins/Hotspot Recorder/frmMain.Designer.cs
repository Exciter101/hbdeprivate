namespace HotspotRecorder
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddBlackspot = new System.Windows.Forms.Button();
            this.btnAddVendor = new System.Windows.Forms.Button();
            this.btnAddMailbox = new System.Windows.Forms.Button();
            this.btnAddHotspot = new System.Windows.Forms.Button();
            this.btnSaveProfile = new System.Windows.Forms.Button();
            this.txtProfileName = new System.Windows.Forms.TextBox();
            this.btnLoadProfile = new System.Windows.Forms.Button();
            this.btnReverse = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblProfileNameLabel = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lstHotSpots = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstBlackSpots = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lstMailboxes = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lstNPC = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBlackspotRadius = new System.Windows.Forms.TextBox();
            this.chkGenerateHotspots = new System.Windows.Forms.CheckBox();
            this.txtSpacing = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblLocationNPC = new System.Windows.Forms.Label();
            this.lblLocationMe = new System.Windows.Forms.Label();
            this.lblNPCName = new System.Windows.Forms.Label();
            this.lblMeName = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnGoTo = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnCheckPoints = new System.Windows.Forms.Button();
            this.btnSetTop = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnRunPoints = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAdjustHeight = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAddBlackspot);
            this.groupBox1.Controls.Add(this.btnAddVendor);
            this.groupBox1.Controls.Add(this.btnAddMailbox);
            this.groupBox1.Controls.Add(this.btnAddHotspot);
            this.groupBox1.Location = new System.Drawing.Point(18, 42);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(215, 98);
            this.groupBox1.TabIndex = 72;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add";
            // 
            // btnAddBlackspot
            // 
            this.btnAddBlackspot.Location = new System.Drawing.Point(6, 60);
            this.btnAddBlackspot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddBlackspot.Name = "btnAddBlackspot";
            this.btnAddBlackspot.Size = new System.Drawing.Size(100, 29);
            this.btnAddBlackspot.TabIndex = 12;
            this.btnAddBlackspot.Text = "Blackspot";
            this.toolTip1.SetToolTip(this.btnAddBlackspot, "Add a blackspot at the current location, with the radius specified below");
            this.btnAddBlackspot.UseVisualStyleBackColor = true;
            this.btnAddBlackspot.Click += new System.EventHandler(this.btnAddBlackspot_Click);
            // 
            // btnAddVendor
            // 
            this.btnAddVendor.Location = new System.Drawing.Point(109, 60);
            this.btnAddVendor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddVendor.Name = "btnAddVendor";
            this.btnAddVendor.Size = new System.Drawing.Size(100, 29);
            this.btnAddVendor.TabIndex = 11;
            this.btnAddVendor.Text = "Vendor";
            this.toolTip1.SetToolTip(this.btnAddVendor, "Add a vendor at the current location");
            this.btnAddVendor.UseVisualStyleBackColor = true;
            this.btnAddVendor.Click += new System.EventHandler(this.btnAddVendor_Click);
            // 
            // btnAddMailbox
            // 
            this.btnAddMailbox.Location = new System.Drawing.Point(109, 25);
            this.btnAddMailbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddMailbox.Name = "btnAddMailbox";
            this.btnAddMailbox.Size = new System.Drawing.Size(100, 29);
            this.btnAddMailbox.TabIndex = 10;
            this.btnAddMailbox.Text = "Mailbox";
            this.toolTip1.SetToolTip(this.btnAddMailbox, "Add a mailbox at the current location");
            this.btnAddMailbox.UseVisualStyleBackColor = true;
            this.btnAddMailbox.Click += new System.EventHandler(this.btnAddMailbox_Click);
            // 
            // btnAddHotspot
            // 
            this.btnAddHotspot.Location = new System.Drawing.Point(6, 25);
            this.btnAddHotspot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddHotspot.Name = "btnAddHotspot";
            this.btnAddHotspot.Size = new System.Drawing.Size(100, 29);
            this.btnAddHotspot.TabIndex = 8;
            this.btnAddHotspot.Text = "Hotspot";
            this.toolTip1.SetToolTip(this.btnAddHotspot, "Add a single hotspot at the current location");
            this.btnAddHotspot.UseVisualStyleBackColor = true;
            this.btnAddHotspot.Click += new System.EventHandler(this.btnAddHotspot_Click);
            // 
            // btnSaveProfile
            // 
            this.btnSaveProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveProfile.Location = new System.Drawing.Point(504, 2);
            this.btnSaveProfile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveProfile.Name = "btnSaveProfile";
            this.btnSaveProfile.Size = new System.Drawing.Size(100, 36);
            this.btnSaveProfile.TabIndex = 71;
            this.btnSaveProfile.Text = "Save...";
            this.toolTip1.SetToolTip(this.btnSaveProfile, "Save profile");
            this.btnSaveProfile.UseVisualStyleBackColor = true;
            this.btnSaveProfile.Click += new System.EventHandler(this.btnSaveProfile_Click);
            // 
            // txtProfileName
            // 
            this.txtProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProfileName.Location = new System.Drawing.Point(119, 6);
            this.txtProfileName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtProfileName.Name = "txtProfileName";
            this.txtProfileName.Size = new System.Drawing.Size(324, 26);
            this.txtProfileName.TabIndex = 70;
            this.toolTip1.SetToolTip(this.txtProfileName, "The filename of the currently loaded profile.");
            // 
            // btnLoadProfile
            // 
            this.btnLoadProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadProfile.Location = new System.Drawing.Point(448, 2);
            this.btnLoadProfile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoadProfile.Name = "btnLoadProfile";
            this.btnLoadProfile.Size = new System.Drawing.Size(34, 36);
            this.btnLoadProfile.TabIndex = 68;
            this.btnLoadProfile.Text = "...";
            this.toolTip1.SetToolTip(this.btnLoadProfile, "Load profile");
            this.btnLoadProfile.UseVisualStyleBackColor = true;
            this.btnLoadProfile.Click += new System.EventHandler(this.btnLoadProfile_Click);
            // 
            // btnReverse
            // 
            this.btnReverse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReverse.Location = new System.Drawing.Point(251, 369);
            this.btnReverse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReverse.Name = "btnReverse";
            this.btnReverse.Size = new System.Drawing.Size(112, 36);
            this.btnReverse.TabIndex = 67;
            this.btnReverse.Text = "Reverse";
            this.toolTip1.SetToolTip(this.btnReverse, "Reverse XYZ locations");
            this.btnReverse.UseVisualStyleBackColor = true;
            this.btnReverse.Click += new System.EventHandler(this.btnReverse_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReplace.Location = new System.Drawing.Point(486, 329);
            this.btnReplace.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(112, 36);
            this.btnReplace.TabIndex = 64;
            this.btnReplace.Text = "Replace";
            this.toolTip1.SetToolTip(this.btnReplace, "Replace current XYZ location");
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(369, 369);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(112, 36);
            this.btnDelete.TabIndex = 62;
            this.btnDelete.Text = "Delete";
            this.toolTip1.SetToolTip(this.btnDelete, "Delete selected XYZ locations");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblProfileNameLabel
            // 
            this.lblProfileNameLabel.AutoSize = true;
            this.lblProfileNameLabel.Location = new System.Drawing.Point(10, 9);
            this.lblProfileNameLabel.Name = "lblProfileNameLabel";
            this.lblProfileNameLabel.Size = new System.Drawing.Size(103, 20);
            this.lblProfileNameLabel.TabIndex = 69;
            this.lblProfileNameLabel.Text = "Profile Name:";
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Controls.Add(this.tabPage2);
            this.tabMain.Controls.Add(this.tabPage3);
            this.tabMain.Controls.Add(this.tabPage4);
            this.tabMain.Location = new System.Drawing.Point(251, 42);
            this.tabMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(353, 244);
            this.tabMain.TabIndex = 61;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lstHotSpots);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(345, 211);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Hotspots";
            this.toolTip1.SetToolTip(this.tabPage1, "The primary hotspots in your profile.");
            this.tabPage1.ToolTipText = "The primary hotspots in your profile";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lstHotSpots
            // 
            this.lstHotSpots.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lstHotSpots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstHotSpots.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstHotSpots.Location = new System.Drawing.Point(3, 2);
            this.lstHotSpots.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstHotSpots.Name = "lstHotSpots";
            this.lstHotSpots.Size = new System.Drawing.Size(339, 207);
            this.lstHotSpots.TabIndex = 0;
            this.lstHotSpots.UseCompatibleStateImageBehavior = false;
            this.lstHotSpots.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 1031;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstBlackSpots);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(345, 211);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Blackspots";
            this.toolTip1.SetToolTip(this.tabPage2, "Spots that cause problems and need to be avoided.");
            this.tabPage2.ToolTipText = "Spots that cause problems and need to be avoided";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstBlackSpots
            // 
            this.lstBlackSpots.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.lstBlackSpots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBlackSpots.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstBlackSpots.Location = new System.Drawing.Point(3, 2);
            this.lstBlackSpots.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstBlackSpots.Name = "lstBlackSpots";
            this.lstBlackSpots.Size = new System.Drawing.Size(337, 202);
            this.lstBlackSpots.TabIndex = 1;
            this.lstBlackSpots.UseCompatibleStateImageBehavior = false;
            this.lstBlackSpots.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 1031;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lstMailboxes);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(345, 211);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Mailboxes";
            this.toolTip1.SetToolTip(this.tabPage3, "Mailbox locations.");
            this.tabPage3.ToolTipText = "Mailbox locations";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lstMailboxes
            // 
            this.lstMailboxes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.lstMailboxes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMailboxes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstMailboxes.Location = new System.Drawing.Point(0, 0);
            this.lstMailboxes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstMailboxes.Name = "lstMailboxes";
            this.lstMailboxes.Size = new System.Drawing.Size(344, 206);
            this.lstMailboxes.TabIndex = 1;
            this.lstMailboxes.UseCompatibleStateImageBehavior = false;
            this.lstMailboxes.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 1031;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lstNPC);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(345, 211);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "NPC";
            this.toolTip1.SetToolTip(this.tabPage4, "Repair/food vendors.");
            this.tabPage4.ToolTipText = "Repair and food vendors";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lstNPC
            // 
            this.lstNPC.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstNPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNPC.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstNPC.Location = new System.Drawing.Point(0, 0);
            this.lstNPC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstNPC.Name = "lstNPC";
            this.lstNPC.Size = new System.Drawing.Size(344, 206);
            this.lstNPC.TabIndex = 1;
            this.lstNPC.UseCompatibleStateImageBehavior = false;
            this.lstNPC.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 1031;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtBlackspotRadius);
            this.groupBox2.Controls.Add(this.chkGenerateHotspots);
            this.groupBox2.Controls.Add(this.txtSpacing);
            this.groupBox2.Location = new System.Drawing.Point(18, 154);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(215, 132);
            this.groupBox2.TabIndex = 73;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 20);
            this.label1.TabIndex = 40;
            this.label1.Text = "Hotspot spacing:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Blackspot radius:";
            // 
            // txtBlackspotRadius
            // 
            this.txtBlackspotRadius.Location = new System.Drawing.Point(158, 28);
            this.txtBlackspotRadius.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBlackspotRadius.Name = "txtBlackspotRadius";
            this.txtBlackspotRadius.Size = new System.Drawing.Size(34, 26);
            this.txtBlackspotRadius.TabIndex = 5;
            this.txtBlackspotRadius.Text = "20";
            this.toolTip1.SetToolTip(this.txtBlackspotRadius, "The radius of the blackspot");
            // 
            // chkGenerateHotspots
            // 
            this.chkGenerateHotspots.AutoSize = true;
            this.chkGenerateHotspots.Location = new System.Drawing.Point(25, 92);
            this.chkGenerateHotspots.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkGenerateHotspots.Name = "chkGenerateHotspots";
            this.chkGenerateHotspots.Size = new System.Drawing.Size(190, 30);
            this.chkGenerateHotspots.TabIndex = 11;
            this.chkGenerateHotspots.Text = "Generate hotspots";
            this.toolTip1.SetToolTip(this.chkGenerateHotspots, "Automatically generate hotspots with the spacing indicated above");
            this.chkGenerateHotspots.UseVisualStyleBackColor = true;
            // 
            // txtSpacing
            // 
            this.txtSpacing.Location = new System.Drawing.Point(158, 60);
            this.txtSpacing.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSpacing.Name = "txtSpacing";
            this.txtSpacing.Size = new System.Drawing.Size(34, 26);
            this.txtSpacing.TabIndex = 12;
            this.txtSpacing.Text = "20";
            this.toolTip1.SetToolTip(this.txtSpacing, "The minimum distance between autogenerated hotspots");
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(14, 448);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(588, 86);
            this.txtOutput.TabIndex = 60;
            this.toolTip1.SetToolTip(this.txtOutput, "Messages from Hotspot Recorder");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 548);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(612, 26);
            this.statusStrip1.TabIndex = 59;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Resize += new System.EventHandler(this.statusStrip1_Resize);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.AutoSize = false;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 20);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblLocationNPC);
            this.groupBox3.Controls.Add(this.lblLocationMe);
            this.groupBox3.Controls.Add(this.lblNPCName);
            this.groupBox3.Controls.Add(this.lblMeName);
            this.groupBox3.Location = new System.Drawing.Point(18, 296);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(215, 146);
            this.groupBox3.TabIndex = 75;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Location:";
            // 
            // lblLocationNPC
            // 
            this.lblLocationNPC.AutoSize = true;
            this.lblLocationNPC.Location = new System.Drawing.Point(21, 118);
            this.lblLocationNPC.Name = "lblLocationNPC";
            this.lblLocationNPC.Size = new System.Drawing.Size(128, 20);
            this.lblLocationNPC.TabIndex = 3;
            this.lblLocationNPC.Text = "<target location>";
            this.toolTip1.SetToolTip(this.lblLocationNPC, "Target location");
            // 
            // lblLocationMe
            // 
            this.lblLocationMe.AutoSize = true;
            this.lblLocationMe.Location = new System.Drawing.Point(21, 52);
            this.lblLocationMe.Name = "lblLocationMe";
            this.lblLocationMe.Size = new System.Drawing.Size(106, 20);
            this.lblLocationMe.TabIndex = 2;
            this.lblLocationMe.Text = "<my location>";
            this.toolTip1.SetToolTip(this.lblLocationMe, "My location");
            // 
            // lblNPCName
            // 
            this.lblNPCName.AutoSize = true;
            this.lblNPCName.Location = new System.Drawing.Point(6, 91);
            this.lblNPCName.Name = "lblNPCName";
            this.lblNPCName.Size = new System.Drawing.Size(113, 20);
            this.lblNPCName.TabIndex = 1;
            this.lblNPCName.Text = "<target name>";
            this.toolTip1.SetToolTip(this.lblNPCName, "Target Name");
            // 
            // lblMeName
            // 
            this.lblMeName.AutoSize = true;
            this.lblMeName.Location = new System.Drawing.Point(6, 32);
            this.lblMeName.Name = "lblMeName";
            this.lblMeName.Size = new System.Drawing.Size(35, 20);
            this.lblMeName.TabIndex = 0;
            this.lblMeName.Text = "Me:";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnGoTo
            // 
            this.btnGoTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGoTo.Location = new System.Drawing.Point(251, 288);
            this.btnGoTo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGoTo.Name = "btnGoTo";
            this.btnGoTo.Size = new System.Drawing.Size(112, 36);
            this.btnGoTo.TabIndex = 76;
            this.btnGoTo.Text = "Go To";
            this.toolTip1.SetToolTip(this.btnGoTo, "Go to selected XYZ location.");
            this.btnGoTo.UseVisualStyleBackColor = true;
            this.btnGoTo.Click += new System.EventHandler(this.btnGoTo_Click);
            // 
            // btnCheckPoints
            // 
            this.btnCheckPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckPoints.Location = new System.Drawing.Point(370, 329);
            this.btnCheckPoints.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCheckPoints.Name = "btnCheckPoints";
            this.btnCheckPoints.Size = new System.Drawing.Size(112, 36);
            this.btnCheckPoints.TabIndex = 77;
            this.btnCheckPoints.Text = "Check";
            this.toolTip1.SetToolTip(this.btnCheckPoints, "Check the points for navigability, commenting out those that can\'t be reached.");
            this.btnCheckPoints.UseVisualStyleBackColor = true;
            this.btnCheckPoints.Click += new System.EventHandler(this.btnCheckPoints_Click);
            // 
            // btnSetTop
            // 
            this.btnSetTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetTop.Location = new System.Drawing.Point(251, 328);
            this.btnSetTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSetTop.Name = "btnSetTop";
            this.btnSetTop.Size = new System.Drawing.Size(112, 36);
            this.btnSetTop.TabIndex = 78;
            this.btnSetTop.Text = "Set Top";
            this.toolTip1.SetToolTip(this.btnSetTop, "Set the currently selected hotspot as the first hotspot in the profile.");
            this.btnSetTop.UseVisualStyleBackColor = true;
            this.btnSetTop.Click += new System.EventHandler(this.btnSetTop_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev.Location = new System.Drawing.Point(370, 286);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(45, 36);
            this.btnPrev.TabIndex = 79;
            this.btnPrev.Text = "<--";
            this.toolTip1.SetToolTip(this.btnPrev, "Go to prev spot");
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNext.Location = new System.Drawing.Point(422, 286);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(45, 36);
            this.btnNext.TabIndex = 80;
            this.btnNext.Text = "-->";
            this.toolTip1.SetToolTip(this.btnNext, "Go to next spot");
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnRunPoints
            // 
            this.btnRunPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunPoints.Location = new System.Drawing.Point(474, 286);
            this.btnRunPoints.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRunPoints.Name = "btnRunPoints";
            this.btnRunPoints.Size = new System.Drawing.Size(56, 36);
            this.btnRunPoints.TabIndex = 81;
            this.btnRunPoints.Text = "Run";
            this.toolTip1.SetToolTip(this.btnRunPoints, "Run all the points");
            this.btnRunPoints.UseVisualStyleBackColor = true;
            this.btnRunPoints.Click += new System.EventHandler(this.btnRunPoints_Click);
            // 
            // btnMove
            // 
            this.btnMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMove.Location = new System.Drawing.Point(507, 406);
            this.btnMove.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(91, 36);
            this.btnMove.TabIndex = 84;
            this.btnMove.Text = "Move";
            this.toolTip1.SetToolTip(this.btnMove, "Adjust height of all points");
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStop.Location = new System.Drawing.Point(543, 286);
            this.btnStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(56, 36);
            this.btnStop.TabIndex = 85;
            this.btnStop.Text = "Stop";
            this.toolTip1.SetToolTip(this.btnStop, "Run all the points");
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(248, 414);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 20);
            this.label2.TabIndex = 82;
            this.label2.Text = "Move spots up/down by:";
            // 
            // txtAdjustHeight
            // 
            this.txtAdjustHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAdjustHeight.Location = new System.Drawing.Point(433, 410);
            this.txtAdjustHeight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAdjustHeight.Name = "txtAdjustHeight";
            this.txtAdjustHeight.Size = new System.Drawing.Size(73, 26);
            this.txtAdjustHeight.TabIndex = 83;
            this.txtAdjustHeight.Text = "2";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 574);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.txtAdjustHeight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRunPoints);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnSetTop);
            this.Controls.Add(this.btnCheckPoints);
            this.Controls.Add(this.btnGoTo);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSaveProfile);
            this.Controls.Add(this.txtProfileName);
            this.Controls.Add(this.btnLoadProfile);
            this.Controls.Add(this.btnReverse);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lblProfileNameLabel);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmMain";
            this.Text = "Hotspot Recorder by Kamilche";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAddBlackspot;
        private System.Windows.Forms.Button btnAddVendor;
        private System.Windows.Forms.Button btnAddMailbox;
        private System.Windows.Forms.Button btnAddHotspot;
        private System.Windows.Forms.Button btnSaveProfile;
        private System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Button btnLoadProfile;
        private System.Windows.Forms.Button btnReverse;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblProfileNameLabel;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBlackspotRadius;
        private System.Windows.Forms.CheckBox chkGenerateHotspots;
        private System.Windows.Forms.TextBox txtSpacing;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblLocationNPC;
        private System.Windows.Forms.Label lblLocationMe;
        private System.Windows.Forms.Label lblNPCName;
        private System.Windows.Forms.Label lblMeName;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnGoTo;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnCheckPoints;
        private System.Windows.Forms.Button btnSetTop;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnRunPoints;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAdjustHeight;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.ListView lstHotSpots;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView lstBlackSpots;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListView lstMailboxes;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ListView lstNPC;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnStop;

    }
}

