namespace MasterBaiter
{
    partial class MB_Config
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
            this.prpg_Settings = new System.Windows.Forms.PropertyGrid();
            this.btn_Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // prpg_Settings
            // 
            this.prpg_Settings.Location = new System.Drawing.Point(12, 12);
            this.prpg_Settings.Name = "prpg_Settings";
            this.prpg_Settings.Size = new System.Drawing.Size(397, 298);
            this.prpg_Settings.TabIndex = 0;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(175, 316);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Save.TabIndex = 1;
            this.btn_Save.Text = "&Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // MB_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 340);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.prpg_Settings);
            this.Name = "MB_Config";
            this.Text = "Master Baiter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid prpg_Settings;
        private System.Windows.Forms.Button btn_Save;
    }
}