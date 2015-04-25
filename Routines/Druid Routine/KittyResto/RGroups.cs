using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Action = Styx.TreeSharp.Action;

namespace RestoDruid
{
    public partial class RMain
    {
        #region proving grounds
        public static bool InProvingGrounds
        {
            get { return Me.MinimapZoneText == "Proving Grounds"; }
        }
        #endregion

        #region partycount
        public static int partyCount
        {
            get
            {
                if (Me.CurrentMap.IsBattleground || Me.CurrentMap.IsPveInstance) { return Me.GroupInfo.NumRaidMembers; }
                return Me.GroupInfo.NumRaidMembers == 0 ? Me.GroupInfo.NumPartyMembers : Me.GroupInfo.NumRaidMembers;
            }
        }
        #endregion

        public static IEnumerable<WoWPartyMember> WoWPartyMembers
        {
            get
            {
                return StyxWoW.Me.GroupInfo.RaidMembers.Union(StyxWoW.Me.GroupInfo.PartyMembers).Distinct().ToList();
            }
        }

        public static IEnumerable<WoWUnit> Tanks
        {
            get
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                list = WoWPartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank)).Select(p => p.ToPlayer()).ToList();
                return list;
            }
        }

        public static IEnumerable<WoWUnit> Healers
        {
            get
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                list = WoWPartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Healer)).Select(p => p.ToPlayer()).ToList();
                return list;
            }
        }

        public static IEnumerable<WoWUnit> Damage
        {
            get
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                list = WoWPartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Damage)).Select(p => p.ToPlayer()).ToList();
                return list;
            }
        }

        public static IEnumerable<WoWUnit> PartyMembers
        {
            get
            {
                 var t = new List<WoWUnit>();

                try
                {

                    if (partyCount > 0 && InProvingGrounds)
                    {
                        t = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(p => p != null
                            && p.CanSelect
                            && p.IsFriendly).ToList();
                        return t;
                    }
                    if (partyCount > 0 && !InProvingGrounds)
                    {
                        t = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(p => p.ToPlayer() != null
                            && p.ToPlayer().IsInMyPartyOrRaid).ToList();
                        return t;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "PartyMembers: " + e); return null; }
                return t;
            }
        }

        public static IEnumerable<WoWPlayer> SearchAreaPlayers()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p => p != null && p.IsInMyPartyOrRaid);
        }

        public static IEnumerable<WoWUnit> SearchAreaUnits()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWUnit>();
        }

        public static bool IsValidObject(WoWObject wowObject)
        {
            return (wowObject != null) && wowObject.IsValid;
        }
    }
}
