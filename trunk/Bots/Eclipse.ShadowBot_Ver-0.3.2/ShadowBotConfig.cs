using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.ShadowBot.Data;
using Styx;
using Styx.CommonBot;
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

namespace Eclipse.ShadowBot
{
    public partial class ShadowBotConfig : Form
    {
        public ShadowBotConfig()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EclipseShadowBot.settings = new ShadowBotSettings();
            if (Styx.CommonBot.TreeRoot.IsRunning)
            {
                EclipseShadowBot.Leader = (WoWPlayer)EclipseShadowBot.Me.CurrentTarget;
                if (EclipseShadowBot.Me.Role != WoWPartyMember.GroupRole.Healer && checkboxHealBotMode.Checked) MessageBox.Show("You have chosen the heal only mode - but your group role is not set to healer. Your bot will not heal or dps if you dont set your role.");
                if (EclipseShadowBot.Leader != null)
                {
                    lblTarget.Text = EclipseShadowBot.Leader.Name;
                    EclipseShadowBot.AssistLeader = boolAssistLeader.Checked;
                    EclipseShadowBot.PickUpQuests = boolGetQuests.Checked;
                    EclipseShadowBot.FollowDistance = int.Parse(tbFollowDistance.Text);
                    EclipseShadowBot.HealBotMode = checkboxHealBotMode.Checked;
                    if (boolGetQuests.Checked) Questing.AttachQuestingEvents();

                    EclipseShadowBot.settings.AssistLeader = EclipseShadowBot.AssistLeader;
                    EclipseShadowBot.settings.PickUpQuests = EclipseShadowBot.PickUpQuests;
                    EclipseShadowBot.settings.FollowDistance = EclipseShadowBot.FollowDistance;
                    EclipseShadowBot.settings.HealBotMode = EclipseShadowBot.HealBotMode;
                    EclipseShadowBot.settings.CharacterName = EclipseShadowBot.Me.Name;
                    ShadowBotSettings.SaveOrCreate(EclipseShadowBot.settings);
                }
                
            }
            else MessageBox.Show("The bot must be running in order to choose a leader.");

        }

        private void ShadowBotConfig_Load(object sender, EventArgs e)
        {
            DataTable dt = DAL.LoadSL3Data(string.Format("Select * from ShadowBotSettings where CharacterName = '{0}'", StyxWoW.Me.Name));
            if (dt != null){
                EclipseShadowBot.log("Loading Settings...");
                EclipseShadowBot.settings = (ShadowBotSettings)ORM.convertDataRowtoObject(new ShadowBotSettings(), dt.Rows[0]);
                boolAssistLeader.Checked = EclipseShadowBot.settings.AssistLeader;
                boolGetQuests.Checked = EclipseShadowBot.settings.PickUpQuests;
                checkboxHealBotMode.Checked = EclipseShadowBot.settings.HealBotMode;
                tbFollowDistance.Text = EclipseShadowBot.settings.FollowDistance.ToString();

                EclipseShadowBot.log("Finished loading settings...");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
