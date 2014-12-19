using Eclipse.Bots.MultiBot.Views;
using Eclipse.WoWDatabase;
using Eclipse.MultiBot.Views;
using Styx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Eclipse.MultiBot;

namespace Eclipse.Bots.MultiBot
{
    public partial class EclipseConfigForm : Form
    {

        public EclipseConfigForm()
        {
            InitializeComponent();
        }

        private void EclipseConfigForm_Load(object sender, EventArgs e)
        {
            pbEclipse.ImageLocation = "http://hb.acsoft.us/image.aspx?image=SkinBot.jpg";
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            EclipseDataManager edb = new EclipseDataManager();
            edb.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                EC.log("!!!Setting BOT to Passive mode!!! (its not gonna do ANYTHING!)");
                Core.PassiveMode = true;
            }
            else Core.PassiveMode = false;
        }

        private void btnTravel_Click(object sender, EventArgs e)
        {
            TravelForm tf = new TravelForm();
            tf.Show();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                EC.log("!!!Setting BOT to SkinBot mode!!!");
                Core.SkinMode = true;
            }
            else Core.SkinMode = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                EC.log("!!!Setting BOT to 'KillThese' mode!!!");
                Core.KillThese = true;
            }
            else Core.KillThese = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MobSelectionList qm = new MobSelectionList();
            qm.Show();
        }

        private void chQuestMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chQuestMode.Checked)
            {
                EC.log("!!!Setting BOT to 'Questing' mode!!!");
                Core.QuestingMode = true;
                Questing.AttachQuestingEvents();

            }
            else {
                Core.QuestingMode = false;
                Questing.DetatchQuestingEvents();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MobSelectionList qm = new MobSelectionList();
            qm.SkinMode = true;
            qm.Show();
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            Core.BagsFull = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            QuestingMode qm = new QuestingMode();
            qm.Show();
        }
    }
}
