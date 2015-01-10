using JetBrains.Annotations;
using Styx;
using Styx.Common;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using GroupRole = Styx.WoWInternals.WoWObjects.WoWPartyMember.GroupRole;

namespace Kitty
{
    [UsedImplicitly]
    internal static class Group
    {
        public static bool IsValidObject(WoWObject wowObject)
        {
            return (wowObject != null) && wowObject.IsValid;
        }

        public static IEnumerable<WoWPlayer> SearchAreaPlayers()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p => p != null && p.IsInMyPartyOrRaid);
        }

        public static IEnumerable<WoWUnit> SearchAreaUnits()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWUnit>();
        }


        public static List<WoWUnit> PulsePartyMembersPG()
        {
            var results = new List<WoWUnit>();
            foreach (var p in SearchAreaUnits())
            {
                if (!IsValidObject(p)) continue;
                if (p.Name == "Proving Grounds") continue;
                if (p.Name == "Xuen") continue;
                if (p.Name == "Trial Master Rotun") continue;
                if (p.Name == "Nadaga Soulweaver") continue;
                if (p.Name == "Furnisher Echoroot") continue;
                results.Add(p);
            }
            return results;
        }
        public static List<WoWUnit> PulsePartyMembers()
        {
            var results = new List<WoWUnit>();
            foreach (var p in SearchAreaPlayers())
            {
                if (!IsValidObject(p)) continue;
                results.Add(p);
            }
            return results;
        }
    }
}
