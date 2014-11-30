namespace Eclipse.Bots.MultiBot.Views
{
    partial class TravelForm
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
            this.lbFavoritePlaces = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbNpcLocations = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSearchFavs = new System.Windows.Forms.TextBox();
            this.btnSearchFavs = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tbSearchNPC = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnAddFavorite = new System.Windows.Forms.Button();
            this.tbFavName = new System.Windows.Forms.TextBox();
            this.btnGOTOFav = new System.Windows.Forms.Button();
            this.btnGOTONpc = new System.Windows.Forms.Button();
            this.lbMobs = new System.Windows.Forms.ListBox();
            this.lblMobs = new System.Windows.Forms.Label();
            this.tbSearchMobs = new System.Windows.Forms.TextBox();
            this.btnSearchMobs = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbFavoritePlaces
            // 
            this.lbFavoritePlaces.FormattingEnabled = true;
            this.lbFavoritePlaces.Location = new System.Drawing.Point(12, 57);
            this.lbFavoritePlaces.Name = "lbFavoritePlaces";
            this.lbFavoritePlaces.Size = new System.Drawing.Size(163, 147);
            this.lbFavoritePlaces.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Favorite Places";
            // 
            // lbNpcLocations
            // 
            this.lbNpcLocations.FormattingEnabled = true;
            this.lbNpcLocations.Location = new System.Drawing.Point(12, 318);
            this.lbNpcLocations.Name = "lbNpcLocations";
            this.lbNpcLocations.Size = new System.Drawing.Size(163, 173);
            this.lbNpcLocations.TabIndex = 0;
            this.lbNpcLocations.SelectedValueChanged += new System.EventHandler(this.lbNpcLocations_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "NPCs";
            // 
            // tbSearchFavs
            // 
            this.tbSearchFavs.Location = new System.Drawing.Point(12, 32);
            this.tbSearchFavs.Name = "tbSearchFavs";
            this.tbSearchFavs.Size = new System.Drawing.Size(81, 20);
            this.tbSearchFavs.TabIndex = 2;
            // 
            // btnSearchFavs
            // 
            this.btnSearchFavs.Location = new System.Drawing.Point(100, 30);
            this.btnSearchFavs.Name = "btnSearchFavs";
            this.btnSearchFavs.Size = new System.Drawing.Size(75, 23);
            this.btnSearchFavs.TabIndex = 3;
            this.btnSearchFavs.Text = "Search";
            this.btnSearchFavs.UseVisualStyleBackColor = true;
            this.btnSearchFavs.Click += new System.EventHandler(this.btnSearchFavs_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(100, 292);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbSearchNPC
            // 
            this.tbSearchNPC.Location = new System.Drawing.Point(12, 294);
            this.tbSearchNPC.Name = "tbSearchNPC";
            this.tbSearchNPC.Size = new System.Drawing.Size(81, 20);
            this.tbSearchNPC.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(192, 292);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(163, 217);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // btnAddFavorite
            // 
            this.btnAddFavorite.Location = new System.Drawing.Point(100, 207);
            this.btnAddFavorite.Name = "btnAddFavorite";
            this.btnAddFavorite.Size = new System.Drawing.Size(75, 23);
            this.btnAddFavorite.TabIndex = 7;
            this.btnAddFavorite.Text = "Add Fav";
            this.btnAddFavorite.UseVisualStyleBackColor = true;
            this.btnAddFavorite.Click += new System.EventHandler(this.btnAddFavorite_Click);
            // 
            // tbFavName
            // 
            this.tbFavName.Location = new System.Drawing.Point(13, 208);
            this.tbFavName.Name = "tbFavName";
            this.tbFavName.Size = new System.Drawing.Size(81, 20);
            this.tbFavName.TabIndex = 2;
            // 
            // btnGOTOFav
            // 
            this.btnGOTOFav.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGOTOFav.ForeColor = System.Drawing.Color.Green;
            this.btnGOTOFav.Location = new System.Drawing.Point(43, 234);
            this.btnGOTOFav.Name = "btnGOTOFav";
            this.btnGOTOFav.Size = new System.Drawing.Size(75, 23);
            this.btnGOTOFav.TabIndex = 8;
            this.btnGOTOFav.Text = "GO";
            this.btnGOTOFav.UseVisualStyleBackColor = true;
            this.btnGOTOFav.Click += new System.EventHandler(this.btnGOTOFav_Click);
            // 
            // btnGOTONpc
            // 
            this.btnGOTONpc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGOTONpc.ForeColor = System.Drawing.Color.Green;
            this.btnGOTONpc.Location = new System.Drawing.Point(55, 497);
            this.btnGOTONpc.Name = "btnGOTONpc";
            this.btnGOTONpc.Size = new System.Drawing.Size(75, 23);
            this.btnGOTONpc.TabIndex = 8;
            this.btnGOTONpc.Text = "GO";
            this.btnGOTONpc.UseVisualStyleBackColor = true;
            this.btnGOTONpc.Click += new System.EventHandler(this.btnGOTONpc_Click);
            // 
            // lbMobs
            // 
            this.lbMobs.FormattingEnabled = true;
            this.lbMobs.Location = new System.Drawing.Point(192, 57);
            this.lbMobs.Name = "lbMobs";
            this.lbMobs.Size = new System.Drawing.Size(163, 199);
            this.lbMobs.TabIndex = 0;
            // 
            // lblMobs
            // 
            this.lblMobs.AutoSize = true;
            this.lblMobs.Location = new System.Drawing.Point(189, 16);
            this.lblMobs.Name = "lblMobs";
            this.lblMobs.Size = new System.Drawing.Size(33, 13);
            this.lblMobs.TabIndex = 1;
            this.lblMobs.Text = "Mobs";
            // 
            // tbSearchMobs
            // 
            this.tbSearchMobs.Location = new System.Drawing.Point(192, 33);
            this.tbSearchMobs.Name = "tbSearchMobs";
            this.tbSearchMobs.Size = new System.Drawing.Size(81, 20);
            this.tbSearchMobs.TabIndex = 4;
            // 
            // btnSearchMobs
            // 
            this.btnSearchMobs.Location = new System.Drawing.Point(280, 31);
            this.btnSearchMobs.Name = "btnSearchMobs";
            this.btnSearchMobs.Size = new System.Drawing.Size(75, 23);
            this.btnSearchMobs.TabIndex = 5;
            this.btnSearchMobs.Text = "Search";
            this.btnSearchMobs.UseVisualStyleBackColor = true;
            this.btnSearchMobs.Click += new System.EventHandler(this.btnSearchMobs_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Green;
            this.button2.Location = new System.Drawing.Point(239, 262);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "GO";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnGOTOMob);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.Color.Red;
            this.btnStop.Location = new System.Drawing.Point(100, 545);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(189, 23);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "STOP NAV";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // TravelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 580);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnGOTONpc);
            this.Controls.Add(this.btnGOTOFav);
            this.Controls.Add(this.btnAddFavorite);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnSearchMobs);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbSearchMobs);
            this.Controls.Add(this.tbSearchNPC);
            this.Controls.Add(this.btnSearchFavs);
            this.Controls.Add(this.tbFavName);
            this.Controls.Add(this.tbSearchFavs);
            this.Controls.Add(this.lblMobs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbMobs);
            this.Controls.Add(this.lbNpcLocations);
            this.Controls.Add(this.lbFavoritePlaces);
            this.Name = "TravelForm";
            this.Text = "TravelForm";
            this.Load += new System.EventHandler(this.TravelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbFavoritePlaces;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbNpcLocations;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSearchFavs;
        private System.Windows.Forms.Button btnSearchFavs;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbSearchNPC;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnAddFavorite;
        private System.Windows.Forms.TextBox tbFavName;
        private System.Windows.Forms.Button btnGOTOFav;
        private System.Windows.Forms.Button btnGOTONpc;
        private System.Windows.Forms.ListBox lbMobs;
        private System.Windows.Forms.Label lblMobs;
        private System.Windows.Forms.TextBox tbSearchMobs;
        private System.Windows.Forms.Button btnSearchMobs;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnStop;
    }
}