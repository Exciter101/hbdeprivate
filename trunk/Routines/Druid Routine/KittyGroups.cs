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
            results = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(p => p != null
                && p.IsFriendly).ToList();
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
