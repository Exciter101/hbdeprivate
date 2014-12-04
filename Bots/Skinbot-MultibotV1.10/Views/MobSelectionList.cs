using Eclipse.WoWDatabase;
using Eclipse.WoWDatabase.Models;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eclipse.Bots.MultiBot.Views
{
    public partial class MobSelectionList : Form
    {
        public bool SkinMode = false;
        public MobSelectionList()
        {

            InitializeComponent();
        }

        private void btnSearchMobs_Click(object sender, EventArgs e)
        {
            lbMobs.DataSource = null;
            var list = Core.MOBs.Where(n => n.Name.ToLower().Contains(tbSearchMobs.Text.ToLower())).ToList();
            lbMobs.DataSource = list;
            lbMobs.DisplayMember = "Name";
        }

        private void MobSelectionList_Load(object sender, EventArgs e)
        {
            lbMobs.DataSource = Core.MOBs;
            lbMobs.DisplayMember = "Name";

            if (SkinMode)
            {
                lbKillList.DataSource = Core.SkinList;
            }
            else lbKillList.DataSource = Core.KillList;
            lbKillList.DisplayMember = "Name";

            lbIgnoreList.DataSource = Core.IgnoreList;
            lbIgnoreList.DisplayMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbKillList.DataSource = null;
            var mob = (WoWUnit)lbAMobs.SelectedItem;
            var dbmob = Core.MOBs.Where(m => m.Entry == mob.Entry).FirstOrDefault();
            if (dbmob != null) Core.KillList.Add(dbmob);
            else
            {
                var newmob = new Mob { Entry = mob.Entry, FactionId = mob.FactionId, Level = mob.Level, Name = mob.Name, Zone = StyxWoW.Me.ZoneId };
                Core.AddUnit(mob);
                if (SkinMode)
                {
                    Core.SkinList.Add(newmob);
                    lbKillList.DataSource = Core.SkinList;
                }
                else
                {
                    Core.KillList.Add(newmob);
                    lbKillList.DataSource = Core.KillList;
                }
            }

            lbKillList.DisplayMember = "Name";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lbKillList.DataSource = null;
            var mob = (Mob)lbMobs.SelectedItem;
            if (SkinMode)
            {
                Core.SkinList.Add(mob);
                lbKillList.DataSource = Core.SkinList;
            }
            else
            {
                Core.KillList.Add(mob);
                lbKillList.DataSource = Core.KillList;
            }
            lbKillList.DisplayMember = "Name";
        }

        private void button4_Click(object sender, EventArgs e)
        {

            lbAMobs.DataSource = ObjectManager.ObjectList.Where(w => w.Type == WoWObjectType.Unit).ToList().Where(m => !((WoWUnit)m).IsFriendly).ToList();
            lbMobs.DisplayMember = "Name";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lbIgnoreList.DataSource = null;
            var mob = (Mob)lbIgnoreList.SelectedItem;
            Core.IgnoreList.Remove(mob);
            lbIgnoreList.DataSource = Core.IgnoreList;
            lbIgnoreList.DisplayMember = "Name";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lbIgnoreList.DataSource = null;
            var mob = (Mob)lbMobs.SelectedItem;
            Core.IgnoreList.Add(mob);
            lbIgnoreList.DataSource = Core.IgnoreList;
            lbIgnoreList.DisplayMember = "Name";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lbIgnoreList.DataSource = null;
            var mob = (WoWUnit)lbAMobs.SelectedItem;
            var dbmob = Core.MOBs.Where(m => m.Entry == mob.Entry).FirstOrDefault();
            if (dbmob != null) Core.IgnoreList.Add(dbmob);
            else
            {
                var newmob = new Mob { Entry = mob.Entry, FactionId = mob.FactionId, Level = mob.Level, Name = mob.Name, Zone = StyxWoW.Me.ZoneId };
                Core.AddUnit(mob);
                Core.IgnoreList.Add(newmob);
            }
            lbIgnoreList.DataSource = Core.IgnoreList;
            lbIgnoreList.DisplayMember = "Name";
        }
    }
}
