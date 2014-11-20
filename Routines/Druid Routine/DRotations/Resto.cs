using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
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

using S = Druid.DSpells.SpellCasts;
using U = Druid.Helpers.Unit;
using PR = Druid.DSettings.RestoSettings;
using T = Druid.Helpers.targets;
using L = Druid.Helpers.Logs;
using Bots.DungeonBuddy;
using Styx.CommonBot.Coroutines;

namespace Druid.DRotations
{
    class Resto
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static int lastSpell { get; set; }
        private static readonly List<WoWPlayer> NearbyFriendlyPlayers = new List<WoWPlayer>();

        public static DateTime lastCheck;

        public static DateTime nextTankCheckAllowed;
        public static void SetNextTankCheckAllowed()
        {
            nextTankCheckAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 5, 0);
        }
        public static DateTime nextPartyCheckAllowed;
        public static void SetNextPartyCheckAllowed()
        {
            nextPartyCheckAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 5, 0);
        }
        public static DateTime nextMushroomAllowed;
        public static void SetNextMushroomAllowed()
        {
            nextMushroomAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 30, 0);
        }
        public static bool _loop;

        public const int
            WILD_GROWTH = 48438,
            REGROWTH = 8936,
            HEALING_TOUCH = 5185,
            REJUVENATION = 774,
            SWIFTMEND = 18562,
            LIFEBLOOM = 33763,
            WILD_MUSHROOM = 145205,
            FORCE_OF_NATURE = 102693,
            TRANQUILITY = 740,
            GENESIS = 145518,
            EINDE = 0;

        public static Composite HealRotation()
        {
            return new PrioritySelector(
                new Decorator(ret => Me.Combat && lastCheck >= DateTime.Now, new Action(ret => { GetpartyMembers(); return RunStatus.Failure; })),
                new Decorator(ret => Me.Combat, loopRotation())
            );
        }

        public static List<WoWPlayer> _swiftMenCount = new List<WoWPlayer>();
        public static WoWGuid lastFonGuid;

        public static Composite loopRotation()
        {
            return new Action(ret =>
            {
                _loop = true;
                while (_loop)
                {
                    try
                    {
                        List<WoWPlayer> members = _partyMembers;
                        foreach (var k in members)
                        {
                            WoWUnit p = k;
                            
                            if (p.Distance2D <= 40 && p.IsAlive && p.InLineOfSpellSight && !Me.IsCasting && !Me.IsChanneling)
                            {

                                WoWPlayer myTarget = members.Where(o => o.Distance2D <= 40 && isTank(o.ToPlayer())).OrderBy(o => o.Distance).FirstOrDefault();
                                if (myTarget == null) { myTarget = Me; }
                                // tranquility

                                else if (SpellManager.HasSpell(TRANQUILITY)
                                    && members.Count(o => o.IsAlive && o.HealthPercent <= PR.myPrefs.HealTranquilityR) >= PR.myPrefs.HealTranquility10
                                    && lastSpell != TRANQUILITY
                                    && SpellManager.CanCast(TRANQUILITY))
                                {
                                    SpellManager.Cast(TRANQUILITY);
                                    Logging.Write(Colors.LawnGreen, WoWSpell.FromId(TRANQUILITY).Name);
                                    lastSpell = TRANQUILITY;
                                }
                                // wildgrowth

                                else if (SpellManager.HasSpell(WILD_GROWTH)
                                    && !U.spellOnCooldown(WILD_GROWTH)
                                    && members.Count(o => o.IsAlive && o.HealthPercent <= PR.myPrefs.HealWildGrowthR) >= PR.myPrefs.HealWildGrowth10
                                    && lastSpell != WILD_GROWTH
                                    && SpellManager.CanCast(WILD_GROWTH))
                                {
                                    SpellManager.Cast(WILD_GROWTH, p);
                                    Logging.Write(Colors.LawnGreen, WoWSpell.FromId(WILD_GROWTH).Name + " on: " + p.Name);
                                    lastSpell = WILD_GROWTH;
                                }
                                
                                if (SpellManager.HasSpell(SWIFTMEND)
                                    && (U.buffExists(REJUVENATION, p) || U.buffExists(REGROWTH, p))
                                    && !_swiftMenCount.Contains(p))
                                {
                                    WoWPlayer v = p.ToPlayer();
                                    _swiftMenCount.Add(v);
                                    Logging.Write("adding : " + p + " to swiftmend count");
                                }
                                if (SpellManager.HasSpell(S.NATURES_VIGIL)
                                    && !U.spellOnCooldown(S.NATURES_VIGIL)
                                    && lastSpell != S.NATURES_VIGIL
                                    && SpellManager.CanCast(S.NATURES_VIGIL))
                                {
                                    SpellManager.Cast(S.NATURES_VIGIL);
                                    Logging.Write(Colors.Red, WoWSpell.FromId(S.NATURES_VIGIL).Name);
                                    lastSpell = S.NATURES_VIGIL;
                                }
                                
                                //Swiftmend
                                if (SpellManager.HasSpell(SWIFTMEND)
                                    && !U.spellOnCooldown(SWIFTMEND)
                                    && lastSpell != SWIFTMEND
                                    && _swiftMenCount.Count(o => o.IsAlive) >= 4
                                    && SpellManager.CanCast(SWIFTMEND))
                                {
                                    WoWPlayer swifty = _swiftMenCount.Where(o => o.IsAlive).OrderBy(o => o.Distance).FirstOrDefault();
                                    SpellManager.Cast(SWIFTMEND, swifty);
                                    Logging.Write(Colors.LawnGreen, "Swiftmend on: " + swifty.Name);
                                    lastSpell = SWIFTMEND;
                                    _swiftMenCount.Clear();
                                }
                                // lifebloom
                                if (SpellManager.HasSpell(LIFEBLOOM)
                                    && (!U.buffExists("Lifebloom", myTarget)
                                    || (U.buffExists("Lifebloom", myTarget) && U.buffTimeLeft("Lifebloom", myTarget) <= 3000))
                                    && SpellManager.CanCast(LIFEBLOOM)
                                    && lastSpell != LIFEBLOOM)
                                {
                                    SpellManager.Cast(LIFEBLOOM, myTarget);
                                    Logging.Write(Colors.LawnGreen, "Lifebloom on: " + myTarget.Name);
                                    lastSpell = LIFEBLOOM;
                                }
                                // mushroom
                                else if (SpellManager.HasSpell(WILD_MUSHROOM)
                                    && Mushrooms == null
                                    && !Me.IsChanneling
                                    && (nextMushroomAllowed <= DateTime.Now || needNewMushroom)
                                    && SpellManager.CanCast(WILD_MUSHROOM)
                                    && lastSpell != WILD_MUSHROOM)
                                {
                                    SpellManager.Cast(WILD_MUSHROOM, myTarget);
                                    SpellManager.ClickRemoteLocation(myTarget.Location);
                                    Logging.Write(Colors.LawnGreen, WoWSpell.FromId(WILD_MUSHROOM).Name + " on: " + myTarget.Name);
                                    mushLocation = myTarget.Location;
                                    mushTarget = myTarget;
                                    lastSpell = WILD_MUSHROOM;
                                    SetNextMushroomAllowed();
                                }
                                
                                //force of nature
                                else if (SpellManager.HasSpell(FORCE_OF_NATURE)
                                    && !U.spellOnCooldown(FORCE_OF_NATURE)
                                    && p.HealthPercent <= PR.myPrefs.HealWithFonR
                                    && lastFonGuid != p.Guid
                                    && SpellManager.CanCast(FORCE_OF_NATURE))
                                {
                                    SpellManager.Cast(FORCE_OF_NATURE, p);
                                    Logging.Write(Colors.LawnGreen, WoWSpell.FromId(FORCE_OF_NATURE).Name + " on: " + p.Name);
                                    lastFonGuid = p.Guid;
                                    lastSpell = FORCE_OF_NATURE;
                                }
                                // regrowth
                                else if (SpellManager.HasSpell(REGROWTH)
                                    && p.HealthPercent <= PR.myPrefs.HealRegrowthR
                                    && SpellManager.CanCast(REGROWTH))
                                {
                                    SpellManager.Cast(REGROWTH, p);
                                    Logging.Write(Colors.LawnGreen, WoWSpell.FromId(REGROWTH).Name + " on: " + p.Name);
                                    lastSpell = REGROWTH;
                                }
                                // rejuvenation
                                else if (SpellManager.HasSpell(REJUVENATION)
                                    && p.HealthPercent <= PR.myPrefs.HealRejuR
                                    && SpellManager.CanCast(REJUVENATION)
                                    && !U.buffExists("Rejuvenation", p))
                                {
                                    SpellManager.Cast(REJUVENATION, p);
                                    Logging.Write(Colors.LawnGreen, WoWSpell.FromId(REJUVENATION).Name + " on: " + p.Name);
                                    lastSpell = REJUVENATION;
                                }
                                // healing touch
                                else if (SpellManager.HasSpell(HEALING_TOUCH)
                                    && p.HealthPercent <= PR.myPrefs.HealTouchR
                                    && lastSpell != HEALING_TOUCH
                                    && SpellManager.CanCast(HEALING_TOUCH))
                                {
                                    SpellManager.Cast(HEALING_TOUCH, p);
                                    Logging.Write(Colors.LawnGreen, WoWSpell.FromId(HEALING_TOUCH).Name + " on: " + p.Name);
                                    lastSpell = HEALING_TOUCH;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.Write("CustomClass > Initialize(): " + e);
                    }
                    _loop = false;
                }
            }
            );
        }
        public void Dispose()
        {
            Logging.Write("Dispose Fight Class Heal");

        }

        #region partymembers
        public static List<WoWPlayer> _partyMembers { get { return GetpartyMembers(); } }

        public static List<WoWPlayer> GetpartyMembers()
        {
            lastCheck.AddSeconds(5);
            List<WoWPlayer> party = new List<WoWPlayer>();
            party = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p => p.IsInMyPartyOrRaid).ToList();
            return party;
        }
        #endregion

        #region tanks
        public static List<WoWPlayer> _tanks { get { return getTanks(); } }

        public static List<WoWPlayer> getTanks()
        {
            SetNextTankCheckAllowed();
            List<WoWPlayer> tanklist = new List<WoWPlayer>();
            tanklist = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p => p.IsInMyPartyOrRaid && isTank(p)).ToList();
            return tanklist;
        }

        public static bool isTank(WoWPlayer unit)
        {
            var resultLua = Lua.GetReturnVal<string>("ret =\"false\", role = UnitGroupRolesAssigned(\"" + unit + "\"); if role == \"TANK\" then ret = \"true\" end, return ret", 0);
            if (resultLua == "true")
            {
                Logging.Write(Colors.Pink, "resultLua : " + resultLua);
                return true;
            }
            Logging.Write(Colors.Pink, "resultLua : " + resultLua);
            return false;
        }
        #endregion

        #region mushrooms
        public static WoWUnit Mushrooms
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.Entry == MUSHROOM_ID
                    && o.CreatedByUnitGuid == StyxWoW.Me.Guid
                    && o.Distance <= 40).FirstOrDefault();
            }
        }
        public static int MUSHROOM_ID = 145205;


        private static WoWPoint mushLocation { get; set; }
        private static WoWUnit mushTarget { get; set; }

        private static bool needNewMushroom
        {
            get
            {
                return mushTarget != null && mushTarget.Location.Distance(mushLocation) > 15;
            }
        }
        #endregion
















    }
}
