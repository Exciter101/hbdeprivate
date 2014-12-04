using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;

namespace Eclipse
{
    public static partial class EC
    {
        #region ConfigurationOptions
        public static bool PartyMode = false;
        public static float PartyDistance = 40;
        public static WoWUnit FollowTarget = null;
        public static bool FarmMode = true;
        private static List<WoWUnit> BlackList = new List<WoWUnit>();

        #endregion
        public static void log(string text)
        {
            Logging.Write(Color.FromRgb(163, 110, 255), "Eclipse=>" + text);
        }
        public enum FarmType
        {
            Skinning,
            MobByLevel
        }
    }
    //ToDo: Move this to its own party
    /// <summary>
    /// Helpers Class
    /// </summary>
    public static partial class EC
    {
        public static bool IsUnitBlackListed(WoWUnit mob)
        {

            if (BlackList.Where(m => m.Guid == mob.Guid).Count() > 0 || Blacklist.Contains(mob.Guid, BlacklistFlags.All) || BlackList.Where(m => m.Name == mob.Name).Count() > 0)
            {
                log("mob is in blacklist - ignoring" + mob.Name);
                return true;
            }
            else return false;
        }
        public static void AddMobToBlackList(WoWUnit mob)
        {
            EC.log(string.Format("Adding {0} to blacklist", mob.Name));
            BlackList.Add(mob);
        }
    }
}
