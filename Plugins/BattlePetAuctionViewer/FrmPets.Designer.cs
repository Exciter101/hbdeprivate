namespace BattlePetAuctionViewer
{
    partial class FrmPets
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Auction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Popularity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceCheckedList = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Filter = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cbAll = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.radAllPets = new System.Windows.Forms.RadioButton();
            this.radAuctionHousePets = new System.Windows.Forms.RadioButton();
            this.cbNotOwned = new System.Windows.Forms.CheckBox();
            this.cbOwned = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nameOrId = new System.Windows.Forms.TextBox();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.infoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyoutDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OwnedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.petBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.Filter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.petBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridView1.CausesValidation = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.idDataGridViewTextBoxColumn,
            this.sourceDataGridViewTextBoxColumn,
            this.infoDataGridViewTextBoxColumn,
            this.Auction,
            this.buyoutDataGridViewTextBoxColumn,
            this.OwnedCount,
            this.Popularity});
            this.dataGridView1.DataSource = this.petBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView1.Location = new System.Drawing.Point(12, 140);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Yellow;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(780, 319);
            this.dataGridView1.TabIndex = 0;
            // 
            // Auction
            // 
            this.Auction.DataPropertyName = "Auction";
            this.Auction.HeaderText = "Auction";
            this.Auction.Name = "Auction";
            this.Auction.Width = 120;
            // 
            // Popularity
            // 
            this.Popularity.DataPropertyName = "Popularity";
            this.Popularity.HeaderText = "Popularity(%)";
            this.Popularity.Name = "Popularity";
            // 
            // sourceCheckedList
            // 
            this.sourceCheckedList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.sourceCheckedList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceCheckedList.ForeColor = System.Drawing.Color.Yellow;
            this.sourceCheckedList.FormattingEnabled = true;
            this.sourceCheckedList.Items.AddRange(new object[] {
            "One",
            "Two",
            "Three",
            "Four"});
            this.sourceCheckedList.Location = new System.Drawing.Point(85, 15);
            this.sourceCheckedList.Name = "sourceCheckedList";
            this.sourceCheckedList.Size = new System.Drawing.Size(171, 109);
            this.sourceCheckedList.TabIndex = 1;
            this.sourceCheckedList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.sourceList_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(265, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name or Id:";
            // 
            // Filter
            // 
            this.Filter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Filter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Filter.Controls.Add(this.button1);
            this.Filter.Controls.Add(this.cbAll);
            this.Filter.Controls.Add(this.label4);
            this.Filter.Controls.Add(this.radAllPets);
            this.Filter.Controls.Add(this.radAuctionHousePets);
            this.Filter.Controls.Add(this.cbNotOwned);
            this.Filter.Controls.Add(this.cbOwned);
            this.Filter.Controls.Add(this.label3);
            this.Filter.Controls.Add(this.label2);
            this.Filter.Controls.Add(this.nameOrId);
            this.Filter.Controls.Add(this.sourceCheckedList);
            this.Filter.Controls.Add(this.label1);
            this.Filter.Location = new System.Drawing.Point(12, 2);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(780, 132);
            this.Filter.TabIndex = 3;
            this.Filter.TabStop = false;
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(6, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbAll
            // 
            this.cbAll.AutoSize = true;
            this.cbAll.Checked = true;
            this.cbAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAll.Location = new System.Drawing.Point(64, 19);
            this.cbAll.Name = "cbAll";
            this.cbAll.Size = new System.Drawing.Size(15, 14);
            this.cbAll.TabIndex = 12;
            this.cbAll.UseVisualStyleBackColor = true;
            this.cbAll.CheckedChanged += new System.EventHandler(this.cbAll_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(265, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 14);
            this.label4.TabIndex = 11;
            this.label4.Text = "Displayed Pets:";
            // 
            // radAllPets
            // 
            this.radAllPets.AutoSize = true;
            this.radAllPets.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radAllPets.Location = new System.Drawing.Point(467, 93);
            this.radAllPets.Name = "radAllPets";
            this.radAllPets.Size = new System.Drawing.Size(74, 18);
            this.radAllPets.TabIndex = 10;
            this.radAllPets.Text = "My Pets";
            this.radAllPets.UseVisualStyleBackColor = true;
            this.radAllPets.CheckedChanged += new System.EventHandler(this.radAllPets_CheckedChanged);
            // 
            // radAuctionHousePets
            // 
            this.radAuctionHousePets.AutoSize = true;
            this.radAuctionHousePets.Checked = true;
            this.radAuctionHousePets.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radAuctionHousePets.Location = new System.Drawing.Point(383, 93);
            this.radAuctionHousePets.Name = "radAuctionHousePets";
            this.radAuctionHousePets.Size = new System.Drawing.Size(81, 18);
            this.radAuctionHousePets.TabIndex = 9;
            this.radAuctionHousePets.TabStop = true;
            this.radAuctionHousePets.Text = "Auctions";
            this.radAuctionHousePets.UseVisualStyleBackColor = true;
            this.radAuctionHousePets.CheckedChanged += new System.EventHandler(this.radAuctionHousePets_CheckedChanged);
            // 
            // cbNotOwned
            // 
            this.cbNotOwned.AutoSize = true;
            this.cbNotOwned.Checked = true;
            this.cbNotOwned.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNotOwned.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbNotOwned.Location = new System.Drawing.Point(459, 67);
            this.cbNotOwned.Name = "cbNotOwned";
            this.cbNotOwned.Size = new System.Drawing.Size(89, 18);
            this.cbNotOwned.TabIndex = 8;
            this.cbNotOwned.Text = "Not Owned";
            this.cbNotOwned.UseVisualStyleBackColor = true;
            this.cbNotOwned.CheckedChanged += new System.EventHandler(this.cbNotOwned_CheckedChanged);
            // 
            // cbOwned
            // 
            this.cbOwned.AutoSize = true;
            this.cbOwned.Checked = true;
            this.cbOwned.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOwned.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOwned.Location = new System.Drawing.Point(383, 67);
            this.cbOwned.Name = "cbOwned";
            this.cbOwned.Size = new System.Drawing.Size(61, 18);
            this.cbOwned.TabIndex = 7;
            this.cbOwned.Text = "Owned";
            this.cbOwned.UseVisualStyleBackColor = true;
            this.cbOwned.CheckedChanged += new System.EventHandler(this.cbOwned_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(265, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ownership:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "Source:";
            // 
            // nameOrId
            // 
            this.nameOrId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nameOrId.ForeColor = System.Drawing.Color.Yellow;
            this.nameOrId.Location = new System.Drawing.Point(265, 33);
            this.nameOrId.Name = "nameOrId";
            this.nameOrId.Size = new System.Drawing.Size(283, 20);
            this.nameOrId.TabIndex = 3;
            this.nameOrId.TextChanged += new System.EventHandler(this.nameOrId_TextChanged);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 180;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Width = 83;
            // 
            // sourceDataGridViewTextBoxColumn
            // 
            this.sourceDataGridViewTextBoxColumn.DataPropertyName = "Source";
            this.sourceDataGridViewTextBoxColumn.HeaderText = "Source";
            this.sourceDataGridViewTextBoxColumn.Name = "sourceDataGridViewTextBoxColumn";
            this.sourceDataGridViewTextBoxColumn.ReadOnly = true;
            this.sourceDataGridViewTextBoxColumn.Width = 150;
            // 
            // infoDataGridViewTextBoxColumn
            // 
            this.infoDataGridViewTextBoxColumn.DataPropertyName = "Info";
            this.infoDataGridViewTextBoxColumn.HeaderText = "Info";
            this.infoDataGridViewTextBoxColumn.Name = "infoDataGridViewTextBoxColumn";
            this.infoDataGridViewTextBoxColumn.ReadOnly = true;
            this.infoDataGridViewTextBoxColumn.Width = 250;
            // 
            // buyoutDataGridViewTextBoxColumn
            // 
            this.buyoutDataGridViewTextBoxColumn.DataPropertyName = "BuyoutGold";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Transparent;
            this.buyoutDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.buyoutDataGridViewTextBoxColumn.HeaderText = "Buyout(g)";
            this.buyoutDataGridViewTextBoxColumn.Name = "buyoutDataGridViewTextBoxColumn";
            this.buyoutDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // OwnedCount
            // 
            this.OwnedCount.DataPropertyName = "OwnedCount";
            this.OwnedCount.HeaderText = "Owned Count";
            this.OwnedCount.Name = "OwnedCount";
            this.OwnedCount.ReadOnly = true;
            this.OwnedCount.Width = 120;
            // 
            // petBindingSource
            // 
            this.petBindingSource.DataSource = typeof(BattlePetAuctionViewer.Pet);
            // 
            // FrmPets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(804, 462);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.dataGridView1);
            this.ForeColor = System.Drawing.Color.Yellow;
            this.Name = "FrmPets";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Battle Pet Auction Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.Filter.ResumeLayout(false);
            this.Filter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.petBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource petBindingSource;
        private System.Windows.Forms.CheckedListBox sourceCheckedList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox Filter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nameOrId;
        private System.Windows.Forms.RadioButton radAllPets;
        private System.Windows.Forms.RadioButton radAuctionHousePets;
        private System.Windows.Forms.CheckBox cbNotOwned;
        private System.Windows.Forms.CheckBox cbOwned;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbAll;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn infoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Auction;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyoutDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OwnedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Popularity;
    }
}