namespace Eclipse.ShadowBot
{
    partial class ShadowBotConfig
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTarget = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.boolGetQuests = new System.Windows.Forms.CheckBox();
            this.boolAssistLeader = new System.Windows.Forms.CheckBox();
            this.tbFollowDistance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkboxHealBotMode = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFollowName = new System.Windows.Forms.TextBox();
            this.boolFollowByName = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.boolSkinMobs = new System.Windows.Forms.CheckBox();
            this.boolLootMobs = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(173, 361);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Following";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(140, 333);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Following:";
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(200, 333);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(109, 13);
            this.lblTarget.TabIndex = 1;
            this.lblTarget.Text = "Not following anyone.";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Ignore Attackers";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // boolGetQuests
            // 
            this.boolGetQuests.AutoSize = true;
            this.boolGetQuests.Location = new System.Drawing.Point(9, 42);
            this.boolGetQuests.Name = "boolGetQuests";
            this.boolGetQuests.Size = new System.Drawing.Size(100, 17);
            this.boolGetQuests.TabIndex = 2;
            this.boolGetQuests.Text = "Pick Up Quests";
            this.boolGetQuests.UseVisualStyleBackColor = true;
            // 
            // boolAssistLeader
            // 
            this.boolAssistLeader.AutoSize = true;
            this.boolAssistLeader.Checked = true;
            this.boolAssistLeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boolAssistLeader.Location = new System.Drawing.Point(115, 42);
            this.boolAssistLeader.Name = "boolAssistLeader";
            this.boolAssistLeader.Size = new System.Drawing.Size(89, 17);
            this.boolAssistLeader.TabIndex = 2;
            this.boolAssistLeader.Text = "Assist Leader";
            this.boolAssistLeader.UseVisualStyleBackColor = true;
            // 
            // tbFollowDistance
            // 
            this.tbFollowDistance.Location = new System.Drawing.Point(100, 74);
            this.tbFollowDistance.Name = "tbFollowDistance";
            this.tbFollowDistance.Size = new System.Drawing.Size(61, 20);
            this.tbFollowDistance.TabIndex = 3;
            this.tbFollowDistance.Text = "12";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Follow Distance";
            // 
            // checkboxHealBotMode
            // 
            this.checkboxHealBotMode.AutoSize = true;
            this.checkboxHealBotMode.Location = new System.Drawing.Point(115, 19);
            this.checkboxHealBotMode.Name = "checkboxHealBotMode";
            this.checkboxHealBotMode.Size = new System.Drawing.Size(94, 17);
            this.checkboxHealBotMode.TabIndex = 5;
            this.checkboxHealBotMode.Text = "HealBot Mode";
            this.checkboxHealBotMode.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(303, 312);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(127, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "This is the LEADER";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.Cursor = System.Windows.Forms.Cursors.Help;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(3, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(209, 55);
            this.label4.TabIndex = 8;
            this.label4.Text = "This is the distance that the follower will attempt to keep the leader - this als" +
    "o dictates how far away it will go to get Quests and loot.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbFollowName);
            this.groupBox1.Controls.Add(this.boolFollowByName);
            this.groupBox1.Controls.Add(this.tbFollowDistance);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(23, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 162);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Following";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Follow Name:";
            // 
            // tbFollowName
            // 
            this.tbFollowName.Enabled = false;
            this.tbFollowName.Location = new System.Drawing.Point(100, 133);
            this.tbFollowName.Name = "tbFollowName";
            this.tbFollowName.Size = new System.Drawing.Size(100, 20);
            this.tbFollowName.TabIndex = 11;
            // 
            // boolFollowByName
            // 
            this.boolFollowByName.AutoSize = true;
            this.boolFollowByName.Location = new System.Drawing.Point(13, 110);
            this.boolFollowByName.Name = "boolFollowByName";
            this.boolFollowByName.Size = new System.Drawing.Size(102, 17);
            this.boolFollowByName.TabIndex = 2;
            this.boolFollowByName.Text = "Follow By Name";
            this.boolFollowByName.UseVisualStyleBackColor = true;
            this.boolFollowByName.CheckedChanged += new System.EventHandler(this.boolFollowByName_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.boolSkinMobs);
            this.groupBox2.Controls.Add(this.boolLootMobs);
            this.groupBox2.Controls.Add(this.boolGetQuests);
            this.groupBox2.Controls.Add(this.boolAssistLeader);
            this.groupBox2.Controls.Add(this.checkboxHealBotMode);
            this.groupBox2.Location = new System.Drawing.Point(23, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 105);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // boolSkinMobs
            // 
            this.boolSkinMobs.AutoSize = true;
            this.boolSkinMobs.Checked = true;
            this.boolSkinMobs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boolSkinMobs.Location = new System.Drawing.Point(115, 65);
            this.boolSkinMobs.Name = "boolSkinMobs";
            this.boolSkinMobs.Size = new System.Drawing.Size(76, 17);
            this.boolSkinMobs.TabIndex = 2;
            this.boolSkinMobs.Text = "Skin Mobs";
            this.boolSkinMobs.UseVisualStyleBackColor = true;
            // 
            // boolLootMobs
            // 
            this.boolLootMobs.AutoSize = true;
            this.boolLootMobs.Checked = true;
            this.boolLootMobs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boolLootMobs.Location = new System.Drawing.Point(9, 65);
            this.boolLootMobs.Name = "boolLootMobs";
            this.boolLootMobs.Size = new System.Drawing.Size(76, 17);
            this.boolLootMobs.TabIndex = 2;
            this.boolLootMobs.Text = "Loot Mobs";
            this.boolLootMobs.UseVisualStyleBackColor = true;
            // 
            // ShadowBotConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 402);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lblTarget);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "ShadowBotConfig";
            this.Text = "Eclipse - ShadowBot Settings";
            this.Load += new System.EventHandler(this.ShadowBotConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox boolGetQuests;
        private System.Windows.Forms.CheckBox boolAssistLeader;
        private System.Windows.Forms.TextBox tbFollowDistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkboxHealBotMode;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox boolLootMobs;
        private System.Windows.Forms.CheckBox boolSkinMobs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFollowName;
        private System.Windows.Forms.CheckBox boolFollowByName;
    }
}

