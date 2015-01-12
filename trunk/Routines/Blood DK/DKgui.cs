using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using P = DK.DKSettings;

namespace DK
{
    public partial class DKGui : Form
    {
        public DKGui()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            P.myPrefs.Save();
            Close();
        }

        private void DKgui_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = P.myPrefs.AutoMovement;
            checkBox2.Checked = P.myPrefs.AutoTargeting;
            checkBox3.Checked = P.myPrefs.AutoFacing;
            checkBox4.Checked = P.myPrefs.AutoMovementDisable;
            checkBox5.Checked = P.myPrefs.AutoTargetingDisable;
            checkBox6.Checked = P.myPrefs.AutoFacingDisable;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoMovement = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoTargeting = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoFacing = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoMovementDisable = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoTargetingDisable = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoFacingDisable = checkBox6.Checked;
        }

        
    }
}
