namespace Eclipse.Bots.MultiBot.Views
{
    partial class MobSelectionList
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
            this.btnSearchMobs = new System.Windows.Forms.Button();
            this.tbSearchMobs = new System.Windows.Forms.TextBox();
            this.lblMobs = new System.Windows.Forms.Label();
            this.lbMobs = new System.Windows.Forms.ListBox();
            this.lbAMobs = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.lbKillList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lbIgnoreList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSearchMobs
            // 
            this.btnSearchMobs.Location = new System.Drawing.Point(128, 26);
            this.btnSearchMobs.Name = "btnSearchMobs";
            this.btnSearchMobs.Size = new System.Drawing.Size(75, 23);
            this.btnSearchMobs.TabIndex = 9;
            this.btnSearchMobs.Text = "Search";
            this.btnSearchMobs.UseVisualStyleBackColor = true;
            this.btnSearchMobs.Click += new System.EventHandler(this.btnSearchMobs_Click);
            // 
            // tbSearchMobs
            // 
            this.tbSearchMobs.Location = new System.Drawing.Point(12, 28);
            this.tbSearchMobs.Name = "tbSearchMobs";
            this.tbSearchMobs.Size = new System.Drawing.Size(110, 20);
            this.tbSearchMobs.TabIndex = 8;
            // 
            // lblMobs
            // 
            this.lblMobs.AutoSize = true;
            this.lblMobs.Location = new System.Drawing.Point(9, 11);
            this.lblMobs.Name = "lblMobs";
            this.lblMobs.Size = new System.Drawing.Size(62, 13);
            this.lblMobs.TabIndex = 7;
            this.lblMobs.Text = "Mobs in DB";
            // 
            // lbMobs
            // 
            this.lbMobs.FormattingEnabled = true;
            this.lbMobs.Location = new System.Drawing.Point(12, 54);
            this.lbMobs.Name = "lbMobs";
            this.lbMobs.Size = new System.Drawing.Size(191, 225);
            this.lbMobs.TabIndex = 6;
            // 
            // lbAMobs
            // 
            this.lbAMobs.FormattingEnabled = true;
            this.lbAMobs.Location = new System.Drawing.Point(222, 54);
            this.lbAMobs.Name = "lbAMobs";
            this.lbAMobs.Size = new System.Drawing.Size(213, 225);
            this.lbAMobs.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(222, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Add to Kill List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(219, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Mobs in Area";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 283);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Add to KillList";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbKillList
            // 
            this.lbKillList.BackColor = System.Drawing.Color.Black;
            this.lbKillList.ForeColor = System.Drawing.Color.White;
            this.lbKillList.FormattingEnabled = true;
            this.lbKillList.Location = new System.Drawing.Point(441, 28);
            this.lbKillList.Name = "lbKillList";
            this.lbKillList.Size = new System.Drawing.Size(214, 251);
            this.lbKillList.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(438, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Kill List";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(520, 283);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Remove";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(360, 29);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 20);
            this.button4.TabIndex = 11;
            this.button4.Text = "Get/Refresh";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lbIgnoreList
            // 
            this.lbIgnoreList.BackColor = System.Drawing.Color.DarkGray;
            this.lbIgnoreList.ForeColor = System.Drawing.Color.Black;
            this.lbIgnoreList.FormattingEnabled = true;
            this.lbIgnoreList.Location = new System.Drawing.Point(669, 28);
            this.lbIgnoreList.Name = "lbIgnoreList";
            this.lbIgnoreList.Size = new System.Drawing.Size(214, 251);
            this.lbIgnoreList.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(666, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Ignore List";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(750, 283);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "Remove";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button3_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(102, 283);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(101, 23);
            this.button6.TabIndex = 10;
            this.button6.Text = "Add to IgnoreList";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(334, 283);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(101, 23);
            this.button7.TabIndex = 10;
            this.button7.Text = "Add to IgnoreList";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // MobSelectionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 310);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSearchMobs);
            this.Controls.Add(this.tbSearchMobs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMobs);
            this.Controls.Add(this.lbIgnoreList);
            this.Controls.Add(this.lbKillList);
            this.Controls.Add(this.lbAMobs);
            this.Controls.Add(this.lbMobs);
            this.Name = "MobSelectionList";
            this.Text = "MobSelectionList";
            this.Load += new System.EventHandler(this.MobSelectionList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearchMobs;
        private System.Windows.Forms.TextBox tbSearchMobs;
        private System.Windows.Forms.Label lblMobs;
        private System.Windows.Forms.ListBox lbMobs;
        private System.Windows.Forms.ListBox lbAMobs;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lbKillList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox lbIgnoreList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}