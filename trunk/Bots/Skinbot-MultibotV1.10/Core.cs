using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.EclipsePlugins.Models;
using Eclipse.WoWDatabase.Models;
using Styx;
using Styx.Common;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Color = System.Windows.Media.Color;
namespace Eclipse.WoWDatabase
{
    public class Core
    {
        #region Variables
        public static string DataPath = "EclipseWoWDB.edb";
        public static LocalPlayer Me { get; set; }
        public static WoWUnit Target { get; set; }
        public static bool AddTargetOnly = true;
        public static float DistanceToPoll { get; set; }
        public static bool init { get; private set; }
        public static string ilog = string.Empty;
        private static WoWUnit lastTarget = null;
        private static int inventoryCount = 0;
        private static int questCount = 0;
        public static List<Quest> CurrentQuests = new List<Quest>();
        public static bool NinjaSkin = false;
        public static bool PassiveMode = false;
        public static bool ForceNav = false;
        public static Location ForceNavLocation = null;
        public static bool SkinMode = false;
        public static bool KillThese = false;
        public static bool BagsFull = false;
        public static Quest ActiveQuest = null;
        public static uint QuestContextId = 0;
        #endregion

        #region Caches
        public static List<Quest> Quests = new List<Quest>();
        public static List<NPC> NPCs = new List<NPC>();
        public static List<Mob> MOBs = new List<Mob>();
        public static List<Location> Locations = new List<Location>();
        public static List<Faction> Factions = new List<Faction>();
        public static List<Location> RecentlyVisitedLocations = new List<Location>();
        public static List<Location> FavoritePlaces = new List<Location>();
        public static List<Mob> KillList = new List<Mob>();
        public static List<Mob> SkinList = new List<Mob>();
        public static List<Mob> IgnoreList = new List<Mob>();
        public static List<Location> KillLocations = new List<Location>();
        public static List<uint> NewQuests = new List<uint>();
        #endregion

        #region Cache Helpers
        internal static void AddNpc(WoWUnit Target)
        {
            if (NPCs.Where(m => m.Entry == Target.Entry).Count() == 0)
            {
                var npc = new NPC { Name = Target.Name };
                npc.isVendor = Target.IsVendor;
                npc.Entry = Target.Entry;
                npc.Zone = StyxWoW.Me.ZoneId;
                npc.FactionId = Target.FactionId;
                npc.isQuestGiver = Target.IsQuestGiver;
                npc.X = Target.X;
                npc.Y = Target.Y;
                npc.Z = Target.Z;
                npc.Level = Target.Level;
                DAL.ExecuteSL3Query(DAL.Insert(npc, "NPC", "", false, DAL.getTableStructure("NPC")));
                NPCs.Add(npc);
                iLog(string.Format("Added NPC {0} ({1})", npc.Name, npc.Entry));
            }
        }
        internal static void AddMob(WoWUnit Target)
        {
            var _mob = MOBs.Where(m => m.Entry == Target.Entry).FirstOrDefault();
            if (_mob == null && Target != null)
            {
                try
                {
                    var mob = new Mob { Name = Target.Name };
                    mob.Entry = Target.Entry;
                    mob.Zone = StyxWoW.Me.ZoneId;
                    if (Target.FactionId != 0) mob.FactionId = Target.FactionId;
                    mob.isSkinnable = Target.Skinnable;
                    mob.Level = Target.Level;

                    var sql = DAL.Insert(mob, "MOB", "", false, DAL.getTableStructure("MOB"));
                    DAL.ExecuteSL3Query(sql);
                    MOBs.Add(mob);
                    iLog(string.Format("Added Mob {0} ({1})", mob.Name, mob.Entry));
                }
                catch (Exception err)
                {
                    iLog(string.Format("Could not DB mob {0} ({1}) probably a serialization thing...\r\n{2}", Target.Name, Target.Entry, err.ToString()));
                }
            }
            else
            {
                log("Mob is already in the database.");
                if (Target.Skinnable || Target.CanSkin)
                {
                    if (!_mob.isSkinnable)
                    {
                        log("Mob is not marked skinnable in the db but it is - updating.");
                        if (SkinMode && !KillList.Contains(_mob))
                        {
                            log("Since we are in skinmode and this mob is skinnable we are adding it to the Kill List");
                            KillList.Add(_mob);
                        }
                        _mob.isSkinnable = true;
                        MOBs.Where(m => m.Entry == Target.Entry).FirstOrDefault().isSkinnable = true; //make sure its updated in teh list not just this instance.
                        UpdateMob(_mob);
                    }
                }
            }
        }
        internal static void UpdateMob(Mob mob)
        {
            var sql = ORM.Update(mob, "MOB", "", DAL.getTableStructure("MOB"));
            DAL.ExecuteSL3Query(sql);
        }
        internal static void AddUnit(WoWUnit unit)
        {
            if (!unit.IsPlayer)
            {
                if (unit.IsFriendly) Core.AddNpc(unit);
                else Core.AddMob(unit);
            }
        }
        #endregion

        #region  Core Methods
        internal static void Initialize()
        {
            iLog("Running init...");
            FindDB();
            DataTable dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table' and Name='Quests';");
            if (dt == null)
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL(new Quest(), "Quests"));
            }

            dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table' and Name='MOB';");
            if (dt == null)
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL(new Mob(), "MOB"));
            }

            dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table' and Name='NPC';");
            if (dt == null)
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL(new Mob(), "NPC"));
            }

            dt = DAL.LoadSL3Data("SELECT * FROM sqlite_master WHERE type='table' and Name='Locations';");
            if (dt == null)
            {
                DAL.ExecuteSL3Query(DAL.generateCreateSQL(Core.Target, "Locations"));
            }

            DataLoader.loadMobs();
            DataLoader.loadNPCs();
            //DataLoader.loadQuests();
            DataLoader.loadLocations();
            DataLoader.loadFavorites();
            init = true;
            iLog("Finished init.");
        }
        internal static void FindDB()
        {
            var path = Application.StartupPath;
            if (!File.Exists(DataPath))
            {
                var results = Directory.GetFiles(path, "*.edb", SearchOption.AllDirectories).ToList();
                if (results.Count > 0)
                {
                    DataPath = results[0];
                    log(string.Format("--------------------------------Found {0}------------------------------------", results[0]));
                }
            }
        }
        public static bool Pulse()
        {
            if (init == true)
            {
                try
                {
                    UpdateQuests();
                    InventoryChangeCheck();
                    if (StyxWoW.Me.CurrentTarget != null)
                    {
                        if (lastTarget != StyxWoW.Me.CurrentTarget || Target == null)
                        {
                            Target = StyxWoW.Me.CurrentTarget;
                            iLog(string.Format("Target changed to {0} ({1})", Target.Name, Target.Entry));
                            ProccessUnits();
                            ProccessFactions();
                            lastTarget = StyxWoW.Me.CurrentTarget;
                        }
                    }
                    else lastTarget = null;
                }
                catch (Exception err)
                {
                    log("Error at root level: " + err.ToString());
                    return false;
                }
            }
            else
            {
                log("Not Initialized - Calling now:");
                Initialize();
            }
            return true;
        }

        private static void UpdateQuests()
        {
            var quests = StyxWoW.Me.QuestLog.GetAllQuests();
            foreach (var q in quests)
            {
                if (CurrentQuests.Where(qe => qe.Id == q.Id).Count() == 0) 
                {
                    Quest quest = new Quest() { Id = q.Id, Description = q.Description, Level = q.Level, Name = q.Name, RecievedFrom = QuestContextId, Money = q.RewardMoney };
                    List<QuestOrder> questorders = ORM.convertDataTabletoObject<QuestOrder>(DAL.LoadSL3Data(string.Format("select * from questorders where questid = '{0}'", quest.Id)), "");
                    quest.QuestOrders = questorders;
                    EC.log(String.Format("Loaded {0} for Quest named {1}", quest.QuestOrders.Count(), quest.Name));
                    CurrentQuests.Add(quest);
                    QuestContextId = 0;
                }
            }
        }
        #endregion

        #region HandlerMethods

        public static void ProccessUnits()
        {

            if (AddTargetOnly == true) AddUnit(Target);
            else AoeDataMine(DistanceToPoll);

            if (!Target.IsPlayer)
            {
                var loc = new Location { Entry = Target.Entry, X = Target.X, Y = Target.Y, Z = Target.Z, Zone = StyxWoW.Me.ZoneId };
                var savedloc = Locations.Where(l => l.Z == loc.Z && loc.Y == l.Y && loc.Z == l.Z && loc.Entry == l.Entry && loc.Zone == l.Zone).FirstOrDefault();
                if (savedloc == null)
                {
                    //ToDo: see if these are close to another hotspot (with 20 yards) and dont add them - probably best on a new thread...
                    log(string.Format("Adding {4}({0}) to database at Hotspot ({1}, {2}, {3})", loc.Entry, loc.X, loc.Y, loc.Z, Target.Name));
                    DAL.ExecuteSL3Query(ORM.Insert(loc, "Locations", "", false, DAL.getTableStructure("Locations")));
                    Locations.Add(loc);
                }

                var mob = MOBs.Where(e => e.Entry == Target.Entry).FirstOrDefault();
                var npc = NPCs.Where(e => e.Entry == Target.Entry).FirstOrDefault();
                if (npc != null)
                {
                    var needsupdate = false;
                    if (npc.Level == 0) needsupdate = true;
                    if (!npc.isVendor && Target.IsVendor) needsupdate = true;

                    if (needsupdate)
                    {
                        Core.log("Updating npc.");
                        NPCs.Remove(npc);
                        DAL.ExecuteSL3Query(ORM.Update(npc, "NPC", "", DAL.getTableStructure("NPC")));
                        npc.Level = Target.Level;
                        npc.isVendor = Target.IsVendor;
                        NPCs.Add(npc);
                    }
                }
                if (mob != null)
                {
                    if (mob.Level == 0)
                    {
                        Core.UpdateMob(mob);
                        MOBs.Where(e => e.Entry == Target.Entry).FirstOrDefault().Level = Target.Level;
                    }
                    if (Target.IsDead && !Target.Lootable && Target.Skinnable)
                    {
                        iLog("Found skinnable mob - updating mob entry.");
                        MOBs.Remove(mob);
                        mob.isSkinnable = true;
                        DAL.ExecuteSL3Query(ORM.Update(mob, "MOB", "", DAL.getTableStructure("MOB")));
                        MOBs.Add(mob);
                    }
                }
            }
        }
        public static void ProccessFactions()
        {
            var faction = Factions.Where(f => f.FactionId == StyxWoW.Me.CurrentTarget.FactionId && f.Zone == StyxWoW.Me.ZoneId).FirstOrDefault();
            if (faction == null)
            {
                var skinnable = 0;
                if (StyxWoW.Me.CurrentTarget.CanSkin || StyxWoW.Me.CurrentTarget.Skinnable) skinnable = 1;
                DAL.ExecuteSL3Query(String.Format("INSERT OR IGNORE INTO factions (factionid, name, isskinnable, zone) VALUES ({0}, '{1}', '{2}', {3});", Target.FactionId, Target.Faction, skinnable, StyxWoW.Me.ZoneId));
                var f = new Faction { IsSkinnable = StyxWoW.Me.CurrentTarget.Skinnable, Zone = StyxWoW.Me.ZoneId };
                f.FactionId = StyxWoW.Me.CurrentTarget.FactionId;
                if (StyxWoW.Me.CurrentTarget.Faction != null) f.Name = StyxWoW.Me.CurrentTarget.Faction.Name;
                Factions.Add(f);
            }
            else if (faction != null && !faction.IsSkinnable && StyxWoW.Me.CurrentTarget.Skinnable)
            {
                iLog(string.Format("updated faction {0} as skinnable.", Target.FactionId));
                DAL.ExecuteSL3Query(string.Format("update factions set isskinnable = 1 where factionid = '{0}' and zone = '{1}'", Target.FactionId, StyxWoW.Me.ZoneId));
            }


        }
        public static void AoeDataMine(float distance)
        {
            if (Styx.WoWInternals.ObjectManager.ObjectList != null)
            {
                var units = Styx.WoWInternals.ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p => p.IsValid && p.DistanceSqr <= distance * distance).ToList();
                Core.iLog(string.Format("Getting nearby WowUnits."));
                foreach (var unit in units)
                {
                    AddUnit(unit);
                }
            }
        }
        public static List<WoWObject> GetNearbyNPCSWithQuests()
        {
            return Styx.WoWInternals.ObjectManager.ObjectList.Where(w => w.Type == WoWObjectType.Unit).ToList().Where(m => ((WoWUnit)m).IsFriendly && ((WoWUnit)m).QuestGiverStatus == QuestGiverStatus.Available).ToList();
        }
        #endregion

        #region Event Hooks
        //ToDo: Have these methods raise events.
        public static void InventoryChangeCheck()
        {
            try
            {
                //Core.log(StyxWoW.Me.Inventory.FreeSlots.ToString());
                if (StyxWoW.Me.Inventory.FreeSlots < 2)
                {
                    Core.BagsFull = true;
                }
                var count = 0;
                foreach (var item in StyxWoW.Me.BagItems)
                {
                    count += (int)item.StackCount;
                }
                if (count != inventoryCount)
                {
                    inventoryCount = count;
                    Core.log("Detected change in inventory.");
                    //return true;
                }
            }
            catch (Exception err)
            {
                log(err.ToString());
            }
            //return false;
        }
        #endregion

        #region Helper Methods
        public static void log(string text)
        {
            Logging.Write(Color.FromRgb(144, 0, 255), "Eclipse=>" + text);
        }
        public static void iLog(string text)
        {
            ilog += (string.Format("Eclipse | {0:MM-dd-yy hh:mm:ss} => {1} \r\n", DateTime.Now, text));
            log(text);
        }
        public static double Distance(float[] loc1, float[] loc2)
        {
            return Math.Sqrt(loc1.Zip(loc2, (a, b) => (a - b) * (a - b)).Sum());
        }
        public static string WebRequest(string url)
        {
            HttpWebRequest webRequest = null;
            string responseData = string.Empty;

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.Timeout = 200000;
            responseData = WebResponseGet(webRequest);
            webRequest = null;
            return responseData;
        }
        public static string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = string.Empty;
            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }
            return responseData;
        }
        #endregion

        public static bool QuestingMode { get; set; }
    }
}
