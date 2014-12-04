using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.EclipsePlugins.Models;
using Eclipse.WoWDatabase;
using Eclipse.WoWDatabase.Models;
using Styx;
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

namespace Eclipse.MultiBot.Views
{
    public partial class QuestingMode : Form
    {
        EclipseProfile dt = new EclipseProfile();
        public QuestingMode()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lbNPCsWithQuests.DataSource = Core.GetNearbyNPCSWithQuests();
            lbNPCsWithQuests.DisplayMember = "name";

        }
        private void btnImportProfile_Click(object sender, EventArgs e)
        {
            //try
            {
                //ToDo: Renable before publish!
                //OpenFileDialog ofd = new OpenFileDialog();
                //ofd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                //if (tbFileName.Text.Length == 0)
                //{
                //    ofd.ShowDialog();
                //    tbFileName.Text = ofd.FileName;
                //}
                var dirs = Directory.GetFiles(@"C:\Users\Twist\Documents\Honorbuddy\Default Profiles\Cava\Scripts\", "*.xml");
                foreach (var dir in dirs)
                {
                    dt = new EclipseProfile(dir);
                    EC.log("Loaded Profile Data.");
                    dt.Save();
                }
            }
        }

        private void QuestingMode_Load(object sender, EventArgs e)
        {
            listBox3.DataSource = Core.CurrentQuests;
            listBox3.DisplayMember = "name";
            if (StyxWoW.Me != null) listBox1.DataSource = StyxWoW.Me.QuestLog.GetAllQuests().ToList();
            listBox1.DisplayMember = "name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dt.Save();
        }

        private void listBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            Quest Quest = (Quest)listBox3.SelectedItem;
            if (Quest != null)
            {
                listBox2.DataSource = Quest.QuestOrders;
                listBox2.DisplayMember = "QuestName";
                tbLog.AppendText(String.Format("Showing Quest: {0}, {1} \r\n", Quest.Name, Quest.QuestOrders.Count()));
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            DataTable dt = DAL.LoadSL3Data(string.Format("select * from  Quests;"));

            foreach (DataRow row in dt.Rows)
            {
                var quest = (Quest)ORM.convertDataRowtoObject(new Quest(), row, "");
                Core.Quests.Add(quest);

            }
            foreach (var q in Styx.StyxWoW.Me.QuestLog.GetAllQuests().ToList())
            {
                Quest quest = new Quest { Description = q.Description, Id = q.Id, IsDaily = q.IsDaily, IsShareable = q.IsShareable, IsWeekly = q.IsWeekly, Level = q.Level, Name = q.Name, ObjectiveText = q.ObjectiveText, RequiredLevel = q.RequiredLevel };

                var result2 = DAL.LoadSL3Data(string.Format("select * from Quests where Id = '{0}'", q.Id));
                if (result2 == null)
                {
                    DAL.ExecuteSL3Query(DAL.Insert(quest, "Quests", "", false, DAL.getTableStructure("Quests")));
                    EC.log("Added Quest to DataBase.");
                }
                else
                {
                    if (result2.Rows.Count == 0)
                    {
                        DAL.ExecuteSL3Query(DAL.Insert(quest, "Quests", "", false, DAL.getTableStructure("Quests")));
                        EC.log("Added Quest to DataBase.");
                    }
                }
                var result = DAL.LoadSL3Data(string.Format("select * from questorders where questid = '{0}'", q.Id));
                if (result != null)
                {
                    if (result.Rows.Count > 0)
                    {
                        var qos = ORM.convertDataTabletoObject<QuestOrder>(result);
                        quest.QuestOrders.AddRange(qos);
                        EC.log(string.Format("Loaded quest orders for {0}", q.Id));
                    }
                    else
                    {
                        EC.log(q.ObjectiveText);
                    }
                }

                Core.CurrentQuests.Add(quest);
            }
            listBox3.DataSource = Core.CurrentQuests;
            listBox3.DisplayMember = "name";
            if (StyxWoW.Me != null) listBox1.DataSource = StyxWoW.Me.QuestLog.GetAllQuests().ToList();
            listBox1.DisplayMember = "name";

        }


    }
}

