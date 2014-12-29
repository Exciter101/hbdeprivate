using ArachnidCreations;
using ArachnidCreations.DevTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.ShadowBot.Data
{
    public class ShadowBotSettings
    {
        public bool AssistLeader {get;set;}
        public bool IgnoreAttackers { get; set; }
        public bool PickUpQuests { get; set; }
        public int FollowDistance { get; set; }
        public bool HealBotMode { get; set; }
        public string CharacterName { get; set; }
        public string FollowName { get; set; }
        public bool LootMobs { get; set; }
        public bool SkinMobs { get; set; }
        internal static void SaveOrCreate(ShadowBotSettings settings)
        {
            DataTable dt = DAL.getTableStructure("ShadowBotSettings"); //check to see if the table exists.
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable sDT  = DAL.LoadSL3Data(string.Format("select * from ShadowBotSettings where CharacterName = '{0}'", settings.CharacterName));
                    if (sDT.Rows.Count>0) {
                        DAL.ExecuteSL3Query(ORM.Update(settings, "ShadowBotSettings", "CharacterName", DAL.getTableStructure("ShadowBotSettings")));
                    }
                    else DAL.ExecuteSL3Query(ORM.Insert(settings, "ShadowBotSettings", "", false, DAL.getTableStructure("ShadowBotSettings")));
                }
                else
                {
                    CreateTable();
                }
            }
            else
            {
                CreateTable();
            }
        }
        internal static void CreateTable()
        {
            EclipseShadowBot.log("Creating table to save settings.");
            var sql = ORM.generateCreateSQL(new ShadowBotSettings(), "ShadowBotSettings");
            DAL.ExecuteSL3Query(sql);
        }



        public bool FollowByName { get; set; }
    }
}
