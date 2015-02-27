using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HKM = DK.DKHotkeyManagers;

namespace DK
{
    public partial class DKGui2 : Form
    {
        public DKGui2()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = DKSettings.myPrefs;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HKM.keysRegistered = false;
            HKM.registerHotKeys();
            DKSettings.myPrefs.Save();
            Close();
        }

        private void DKGui2_Load(object sender, EventArgs e)
        {

        }
    }
}
