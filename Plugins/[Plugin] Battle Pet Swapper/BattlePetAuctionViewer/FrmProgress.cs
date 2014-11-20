using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BattlePetAuctionViewer
{
    public partial class FrmProgress : Form, IProgress
    {
        public FrmProgress()
        {
            InitializeComponent();
            this.ShowInTaskbar = true;
            //this.TopMost = true;
        }

        public void Step(int step, int maxSteps, string description)
        {
            try
            {
                if (this.progressBar1.Maximum != maxSteps)
                {
                    this.progressBar1.Value = 0;
                    this.progressBar1.Maximum = maxSteps;
                }
                if (this.progressBar1.Value <= this.progressBar1.Maximum)
                {
                    this.progressBar1.Value = step;
                }
                this.labStep.Text = "Step " + step + " of " + maxSteps;

                this.labStepDescription.Text = description;
                lastDescripton = description;
                this.progressBar2.Value = 0;
            }
            catch (Exception ex)
            {
            }

            this.Refresh();
            Application.DoEvents();
        }

        private string lastDescripton = "";

        public void SubStep(int step, int maxSteps)
        {
            try
            {
                if (this.progressBar2.Maximum != maxSteps)
                {
                    this.progressBar2.Value = 0;
                    this.progressBar2.Maximum = maxSteps;
                }
                if (this.progressBar2.Value <= this.progressBar2.Maximum)
                {
                    this.progressBar2.Value = step;
                }
                this.labStepDescription.Text = lastDescripton + " part " + step + " of " + maxSteps;

            }
            catch (Exception ex)
            {
            }
            this.Refresh();
            Application.DoEvents();
        }
    }
}
