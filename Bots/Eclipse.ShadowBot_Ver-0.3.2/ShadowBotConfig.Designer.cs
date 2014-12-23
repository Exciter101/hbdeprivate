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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(78, 312);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Follow Target";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 282);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Following:";
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(111, 282);
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
            this.boolAssistLeader.Location = new System.Drawing.Point(115, 42);
            this.boolAssistLeader.Name = "boolAssistLeader";
            this.boolAssistLeader.Size = new System.Drawing.Size(89, 17);
            this.boolAssistLeader.TabIndex = 2;
            this.boolAssistLeader.Text = "Assist Leader";
            this.boolAssistLeader.UseVisualStyleBackColor = true;
            // 
            // tbFollowDistance
            // 
            this.tbFollowDistance.Location = new System.Drawing.Point(109, 85);
            this.tbFollowDistance.Name = "tbFollowDistance";
            this.tbFollowDistance.Size = new System.Drawing.Size(61, 20);
            this.tbFollowDistance.TabIndex = 3;
            this.tbFollowDistance.Text = "12";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 88);
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
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbFollowDistance);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(23, 97);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 109);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ranges";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.boolGetQuests);
            this.groupBox2.Controls.Add(this.boolAssistLeader);
            this.groupBox2.Controls.Add(this.checkboxHealBotMode);
            this.groupBox2.Location = new System.Drawing.Point(23, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 79);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(312, 296);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Not yet Implemented";
            // 
            // ShadowBotConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 345);
            this.Controls.Add(this.label3);
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
        private System.Windows.Forms.Label label3;
    }
}

