namespace Eclipse.Bots.MultiBot
{
    partial class EclipseConfigForm
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
            this.btnData = new System.Windows.Forms.Button();
            this.pbEclipse = new System.Windows.Forms.PictureBox();
            this.btnTravel = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.chQuestMode = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.btnSell = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbEclipse)).BeginInit();
            this.SuspendLayout();
            // 
            // btnData
            // 
            this.btnData.Location = new System.Drawing.Point(7, 284);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(122, 23);
            this.btnData.TabIndex = 0;
            this.btnData.Text = "Data Management";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // pbEclipse
            // 
            this.pbEclipse.Location = new System.Drawing.Point(0, 0);
            this.pbEclipse.Name = "pbEclipse";
            this.pbEclipse.Size = new System.Drawing.Size(554, 212);
            this.pbEclipse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEclipse.TabIndex = 1;
            this.pbEclipse.TabStop = false;
            // 
            // btnTravel
            // 
            this.btnTravel.Location = new System.Drawing.Point(572, 44);
            this.btnTravel.Name = "btnTravel";
            this.btnTravel.Size = new System.Drawing.Size(193, 23);
            this.btnTravel.TabIndex = 2;
            this.btnTravel.Text = "Travel";
            this.btnTravel.UseVisualStyleBackColor = true;
            this.btnTravel.Click += new System.EventHandler(this.btnTravel_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.Color.DarkViolet;
            this.checkBox1.Location = new System.Drawing.Point(13, 218);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Learning Only";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 39);
            this.label1.TabIndex = 4;
            this.label1.Text = "the bot will do nothing only record what YOU do.";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.ForeColor = System.Drawing.Color.DarkViolet;
            this.checkBox2.Location = new System.Drawing.Point(171, 218);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(70, 17);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "SkinBot";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(152, 238);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 39);
            this.label2.TabIndex = 4;
            this.label2.Text = "the bot will run around and skin mobs in an area";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox3.ForeColor = System.Drawing.Color.DarkViolet;
            this.checkBox3.Location = new System.Drawing.Point(289, 218);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(78, 17);
            this.checkBox3.TabIndex = 3;
            this.checkBox3.Text = "KillThese";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(274, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 39);
            this.label3.TabIndex = 4;
            this.label3.Text = "the bot will kill specific mobs until it runs out, then go look for more";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(152, 284);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Choose Mobs";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(274, 284);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Choose Mobs";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(569, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 30);
            this.label4.TabIndex = 4;
            this.label4.Text = "Go to a Mob an NPC or any Saved location on the continent! (Searchable!)";
            // 
            // chQuestMode
            // 
            this.chQuestMode.AutoSize = true;
            this.chQuestMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chQuestMode.ForeColor = System.Drawing.Color.DarkViolet;
            this.chQuestMode.Location = new System.Drawing.Point(417, 218);
            this.chQuestMode.Name = "chQuestMode";
            this.chQuestMode.Size = new System.Drawing.Size(90, 17);
            this.chQuestMode.TabIndex = 3;
            this.chQuestMode.Text = "QuestMode";
            this.chQuestMode.UseVisualStyleBackColor = true;
            this.chQuestMode.CheckedChanged += new System.EventHandler(this.chQuestMode_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(405, 238);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 39);
            this.label5.TabIndex = 4;
            this.label5.Text = "Experimental - Do Not Use yet";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(405, 284);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(113, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Configure Questing";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnSell
            // 
            this.btnSell.Location = new System.Drawing.Point(572, 116);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(193, 23);
            this.btnSell.TabIndex = 2;
            this.btnSell.Text = "Sell";
            this.btnSell.UseVisualStyleBackColor = true;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(569, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(196, 30);
            this.label6.TabIndex = 4;
            this.label6.Text = "Find the nearest vendor and go sell";
            // 
            // EclipseConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 319);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chQuestMode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnSell);
            this.Controls.Add(this.btnTravel);
            this.Controls.Add(this.pbEclipse);
            this.Controls.Add(this.btnData);
            this.Name = "EclipseConfigForm";
            this.Text = "Eclipse - MultiBot Configuration";
            this.Load += new System.EventHandler(this.EclipseConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbEclipse)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.PictureBox pbEclipse;
        private System.Windows.Forms.Button btnTravel;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chQuestMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Label label6;
    }
}

