using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace MasterBaiter
{
    public partial class MB_Config : Form
    {
        //private MasterBaiter_Settings _MB_Settings = null;

        public MB_Config()
        {
            InitializeComponent();

        }

        public MB_Config(MasterBaiter_Settings MB_Settings)
        {
            InitializeComponent();

            //_MB_Settings = MB_Settings;

            prpg_Settings.SelectedObject = MB_Settings;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            //_MB_Settings.Save();

            Close();
        }
    }
}
