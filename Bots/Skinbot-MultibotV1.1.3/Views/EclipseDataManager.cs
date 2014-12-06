using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.EclipsePlugins.Models;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eclipse.Bots.MultiBot.Views
{
    public partial class EclipseDataManager : Form
    {
        public EclipseDataManager()
        {
            InitializeComponent();
        }

        private void tbRefreshUI_Click(object sender, EventArgs e)
        {

        }

        private void EclipseDataManager_Load(object sender, EventArgs e)
        {
            
        }
        private void btnGetTargetInfo_Click(object sender, EventArgs e)
        {
            //if (Core.Target != null) tbSql.Text = DAL.generateCreateSQL(Core.Target, "MOBs");
            tbSql.Text = DAL.generateCreateSQL(new Eclipse.WoWDatabase.Models.Quest(), "Quests");

        }

        private void btnRunSql_Click(object sender, EventArgs e)
        {
            DAL dal = new DAL();
            var dt = DAL.LoadSL3Data(tbSql.Text);
            dataGridView1.DataSource = dt;
        }

        private void btnGetTables_Click(object sender, EventArgs e)
        {
            DAL dal = new DAL();
            DataTable dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table';");
            lbTables.DataSource = dt;
            lbTables.DisplayMember = "Name";

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            tbDBFile.Text = ofd.FileName;
        }

        public void RefreshUI()
        {
            lbMobs.DataSource = null;
            lbMobs.DataSource = Core.MOBs;
            lbMobs.DisplayMember = "Name";

            lbNpcs.DataSource = null;
            lbNpcs.DataSource = Core.NPCs;
            lbNpcs.DisplayMember = "Name";

            lbLocations.DataSource = null;
            lbLocations.DataSource = Core.Quests;
            lbLocations.DisplayMember = "Name";

            //lblTimer.Text = timerTicks.ToString();
            tbLog.Text = Core.ilog;
            if (StyxWoW.Me != null)
            {
                if (StyxWoW.Me.CurrentTarget != null) propertyGrid1.SelectedObject = StyxWoW.Me.CurrentTarget;
            }
        }

        private void btnSetDistance_Click(object sender, EventArgs e)
        {
            Core.AddTargetOnly = false;
            if (cbDistance.Text == "Target") Core.AddTargetOnly = true;
            else Core.DistanceToPoll = float.Parse(cbDistance.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //var qs = StyxWoW.Me.QuestLog.GetAllQuests().ToList();
            //var q = StyxWoW.Me.QuestLog.GetQuestById(qs.FirstOrDefault().Id);
            //tbSql.Text = DAL.generateCreateSQL(q, "Quests");

            tbSql.Text =
            DAL.Insert(StyxWoW.Me.CurrentTarget, "NPC", "", false, DAL.getTableStructure("NPC"));
            //NPCs.Add(Target);
            //iLog(string.Format("Added NPC {0} ({1})", Target.Name, Target.Entry));

        }
        private void button7_Click(object sender, EventArgs e)
        {
            var props = typeof(Eclipse.WoWDatabase.Models.Quest).GetProperties().ToList();
            foreach (var prop in props)
            {
                tbSql.AppendText(prop.PropertyType.ToString() + "\r\n");
            }

        }

        private void lbTables_DoubleClick(object sender, EventArgs e)
        {
            var table = (DataRowView)lbTables.SelectedItem;
            tbSql.Text = string.Format("Select *  from {0} limit 1000", table["Name"]);

        }

        private void tbLog_TextChanged(object sender, EventArgs e)
        {
            this.AutoScroll = true;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            FileInfo file;
            if (File.Exists(tbDBFile.Text))
            {
                file = new FileInfo(tbDBFile.Text);
                DAL.DBFile = file.FullName;
            }
            else MessageBox.Show("Couldnt find that file..");

        }

        private void btnMobs_Click(object sender, EventArgs e)
        {
            lbMobs.DataSource = null;
            lbMobs.DataSource = Core.MOBs;
            lbMobs.DisplayMember = "Name";
        }

        private void btnLocations_Click(object sender, EventArgs e)
        {
            lbLocations.DataSource = null;
            lbLocations.DataSource = Core.Locations;
            lbLocations.DisplayMember = "Entry";
        }

        private void btnNPCs_Click(object sender, EventArgs e)
        {

            lbNpcs.DataSource = null;
            lbNpcs.DataSource = Core.NPCs;
            lbNpcs.DisplayMember = "Name";
        }

        private void lbLocations_DoubleClick(object sender, EventArgs e)
        {
            var loc = (Location)lbLocations.SelectedItem;
            if (loc != null)
            {
                Core.ForceNav = true;
                Core.ForceNavLocation = loc;
            }
        }

        private void lbLocations_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var mobs = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit).OrderBy(m => m.Distance).ToList();
                foreach (var mob in mobs)
                {
                    Core.AddUnit((WoWUnit)mob);
                }
            }
            catch (Exception err)
            {
                Core.log(err.ToString());
            }
        }

        private void lbMobs_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (Mob)lbMobs.SelectedItem;
        }

        private void lbLocations_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (Location)lbLocations.SelectedItem;
        }

        private void lbNpcs_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (NPC)lbNpcs.SelectedItem;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbSql.AppendText(ORM.generateCreateSQL(new Blackspot(), "BlackSpots"));
            tbSql.AppendText(ORM.generateCreateSQL(new MailBox(), "MailBoxes"));
            tbSql.AppendText(ORM.generateCreateSQL(new Eclipse.WoWDatabase.Models.Quest(), "Quests"));
            tbSql.AppendText(ORM.generateCreateSQL(new QuestObjective(), "QuestObjectives"));
            tbSql.AppendText(ORM.generateCreateSQL(new QuestOrder(), "QuestOrders"));
            tbSql.AppendText(ORM.generateCreateSQL(new QuestOrder(), "QuestOrderLogic"));
            tbSql.AppendText(ORM.generateCreateSQL(new EclipseVendor(), "Vendors"));
        }
    }
}
