using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestoDruid
{
    public partial class RForm : Form
    {
        public RForm()
        {
            InitializeComponent();

            propertyGrid1.SelectedObject = RSettings.myPrefs;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RSettings.myPrefs.Save();
            Close();
        }
    }
}
