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

using PD = RestoDruid.RSettings;

namespace RestoDruid
{
    public partial class RMain
    {
        public const string
            _barkskin = "Barkskin",
            _blessingofkings = "Blessing of Kings",
            _cenarionward = "Cenarion Ward",
            _clearcasting = "Clearcasting",
            _cyclone = "Cyclone",
            _entanglingroots = "Entangling Roots",
            _dreamofcenarius = "Dream of Cenarius",
            _faerieswarm = "Faerie Swarm",
            _forceofnature = "Force of Nature",
            _genesis = "Genesis",
            _healingtouch = "Healing Touch",
            _heartofthewild = "Heart of the Wild",
            _incapacitatingroar = "Incapacitating Roar",
            _incarnationresto = "Incarnation: Tree of Life",
            _ironbark = "Ironbark",
            _legacyoftheemperor = "Legacy of the Emperor",
            _lifebloom = "Lifebloom",
            _markofthewild = "Mark of the Wild",
            _massentanglement = "Mass Entanglement",
            _masteryharmony = "Harmony",
            _mightybash = "Mighty Bash",
            _momentofclarity = "Moment of Clarity",
            _moonfire = "Moonfire",
            _naturescure = "Nature's Cure",
            _naturesswiftness = "Nature's Swiftness",
            _naturesvigil = "Nature's Vigil",
            _rampantgrowth = "Rampant Growth",
            _rebirth = "Rebirth",
            _rejuvenation = "Rejuvenation",
            _regrowth = "Regrowth",
            _renewal = "Renewal",
            _revive = "Revive",
            _souloftheforest = "Soul of the Forest",
            _stampedingroar = "Stampeding Roar",
            _swiftmend = "Swiftmend",
            _tranquility = "Tranquility",
            _typhoon = "Typhoon",
            _ursolsvortex = "Ursol's Vortex",
            _wildgrowth = "Wild Growth",
            _wildmushroom = "Wild Mushroom",
            _wrath = "Wrath";

        public const int
            _forceofnaturehealth = 75,
            _germinationhealthtanks = 85,
            _germinationhealthhealers = 90,
            _germinationhhealthdamage = 85,
            //_healingtouchhealth = 75,
            _rejuvenatiohealth = 90,
            _regrowthhealth = 65,
            _swiftmendhealth = 45,
            _genesisplayercount = 3,
            _genesishealth = 30,
            _germinationbuff = 155777,
            _tranquilityplayercount = 4,
            _tranquilityhealth = 45,
            _wildgrowthplayercount = 2,
            _wildgrowthhealth = 89,
            _wildmushroomint = 144,
            _wildmushroomplayercount = 3,
            _wildmushroomhealth = 85;

        
        public static WoWGuid lastGuid;

        public static bool needMovement
        {
            get
            {
                if (partyCount > 0 && PD.myPrefs.MovementAutoDisable) return false;
                if (partyCount == 0 && !PD.myPrefs.MovementAuto) return false;
                return true;
            }
        }
        public static bool needFacing
        {
            get
            {
                if (partyCount > 0 && PD.myPrefs.MovementFacingDisable) return false;
                if (partyCount == 0 && !PD.myPrefs.MovementFacing) return false;
                return true;
            }
        }
        public static bool needStatsBuff
        {
            get
            {
                if (PD.myPrefs.buffMarkOfTheWild == PD.MotW.Manual) return false;
                if (PD.myPrefs.buffMarkOfTheWild == PD.MotW.All_Group_Members && !Me.HasAura(_markofthewild) && !Me.HasAura(_blessingofkings) && !Me.HasAura(_legacyoftheemperor)) return true; 
                if (PD.myPrefs.buffMarkOfTheWild == PD.MotW.Me_Only && (Me.HasAura(_markofthewild) || Me.HasAura(_blessingofkings) || Me.HasAura(_legacyoftheemperor))) return false;
                if (PD.myPrefs.buffMarkOfTheWild == PD.MotW.All_Group_Members)
                {
                    var unit = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p => p != null
                        && p.IsInMyPartyOrRaid
                        && !p.HasAura(_markofthewild)
                        && !p.HasAura(_blessingofkings)
                        && !p.HasAura(_legacyoftheemperor));
                    if (unit.Count() == 0) return false;
                }
                return true;
            }
        }

        #region healthstone
        public static int healthstonePercent = PD.myPrefs.HealthstonePercent;
        public static int healthStone = 5512;
        public static bool needHealthstone()
        {
            return Me.HealthPercent <= healthstonePercent;
        }
        #endregion

        #region trinkets
        public static DateTime _trinketTimer;
        private static bool CanUseEquippedItem(WoWItem item)
        {
            string itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;
            return item.Usable && item.Cooldown <= 0;
        }
        public static bool needTrinket1
        {
            get
            {
                if (PD.myPrefs.Trinket1 == PD.TrinketUse.Manual) return false;
                if (PD.myPrefs.Trinket1 == PD.TrinketUse.OnCoolDown) //cooldown
                {
                    var Trinket1 = StyxWoW.Me.Inventory.Equipped.Trinket1;
                    if (Trinket1 != null
                        && CanUseEquippedItem(Trinket1))
                    {
                        Trinket1.Use();
                        Logging.Write(Colors.OrangeRed, "Using 1st Trinket");
                        return true;
                    }
                    return false;
                }
                if (PD.myPrefs.Trinket1 == PD.TrinketUse.LowMana) //low mana
                {
                    var Trinket1 = StyxWoW.Me.Inventory.Equipped.Trinket1;
                    if (Trinket1 != null
                        && CanUseEquippedItem(Trinket1)
                        && Me.ManaPercent <= PD.myPrefs.Trinket1LowResources)
                    {
                        Trinket1.Use();
                        Logging.Write(Colors.OrangeRed, "Using 1st Trinket");
                        return true;
                    }
                    return false;
                }
                if (PD.myPrefs.Trinket1 == PD.TrinketUse.LowHP) //low hp
                {
                    var Trinket1 = StyxWoW.Me.Inventory.Equipped.Trinket1;
                    if (Trinket1 != null
                        && CanUseEquippedItem(Trinket1)
                        && Me.HealthPercent <= PD.myPrefs.Trinket2LowResources)
                    {
                        Trinket1.Use();
                        Logging.Write(Colors.OrangeRed, "Using 1st Trinket");
                        return true;
                    }
                    return false;
                }
                if (PD.myPrefs.Trinket1 == PD.TrinketUse.CrowdControlled) //crowdcontrolled
                {
                    var Trinket1 = StyxWoW.Me.Inventory.Equipped.Trinket1;
                    if (Trinket1 != null
                        && CanUseEquippedItem(Trinket1)
                        && units.IsCrowdControlledPlayer(Me))
                    {
                        Trinket1.Use();
                        Logging.Write(Colors.OrangeRed, "Using 1st Trinket");
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }
        public static bool needTrinket2
        {
            get
            {
                if (PD.myPrefs.Trinket2 == PD.TrinketUse.Manual) return false;
                if (PD.myPrefs.Trinket2 == PD.TrinketUse.OnCoolDown) //cooldown
                {
                    var Trinket2 = StyxWoW.Me.Inventory.Equipped.Trinket2;
                    if (Trinket2 != null
                        && CanUseEquippedItem(Trinket2))
                    {
                        Trinket2.Use();
                        Logging.Write(Colors.OrangeRed, "Using 2nd Trinket");
                        return true;
                    }
                    return false;
                }
                if (PD.myPrefs.Trinket2 == PD.TrinketUse.LowMana) //low mana
                {
                    var Trinket2 = StyxWoW.Me.Inventory.Equipped.Trinket2;
                    if (Trinket2 != null
                        && CanUseEquippedItem(Trinket2)
                        && Me.ManaPercent <= PD.myPrefs.Trinket2LowResources)
                    {
                        Trinket2.Use();
                        Logging.Write(Colors.OrangeRed, "Using 2nd Trinket");
                        return true;
                    }
                    return false;
                }
                if (PD.myPrefs.Trinket2 == PD.TrinketUse.LowHP) //low hp
                {
                    var Trinket2 = StyxWoW.Me.Inventory.Equipped.Trinket2;
                    if (Trinket2 != null
                        && CanUseEquippedItem(Trinket2)
                        && Me.HealthPercent <= PD.myPrefs.Trinket2LowResources)
                    {
                        Trinket2.Use();
                        Logging.Write(Colors.OrangeRed, "Using 2nd Trinket");
                        return true;
                    }
                    return false;
                }
                if (PD.myPrefs.Trinket2 == PD.TrinketUse.CrowdControlled) //crowdcontrolled
                {
                    var Trinket2 = StyxWoW.Me.Inventory.Equipped.Trinket2;
                    if (Trinket2 != null
                        && CanUseEquippedItem(Trinket2)
                        && units.IsCrowdControlledPlayer(Me))
                    {
                        Trinket2.Use();
                        Logging.Write(Colors.OrangeRed, "Using 2nd Trinket");
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion

        #region regrowth clearcasting

        public static WoWUnit RegrowthProcTarget
        {
            get
            {
                WoWUnit t = null;
                try
                {
                    if (!Me.HasAura(_clearcasting)) return null;
                    if (partyCount == 0 && Me.HasAura(_clearcasting)) return Me;

                    if (InProvingGrounds && Me.HasAura(_clearcasting))
                    {
                         t = PartyMembers.Where(p => p != null
                            && p.InLineOfSight
                            && p.Distance <= 40).OrderBy(p => p.HealthPercent).FirstOrDefault();
                         return t != null ? t : null;
                    }
                    if (!InProvingGrounds && partyCount > 0 && Me.HasAura(_clearcasting))
                    {
                        t = PartyMembers.Where(p => p != null
                             && p.InLineOfSight
                             && p.Distance <= 40).OrderBy(p => p.HealthPercent).FirstOrDefault();
                        return t != null ? t : null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "RegrowthProcTarget: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region force of nature
        public static WoWUnit FoNTarget
        {
            get
            {
                WoWUnit t = null;
                try
                {
                    int health = PD.myPrefs.FoND;
                    if (partyCount > 5) health = PD.myPrefs.FoNR;

                    if (partyCount == 0 && Me.HealthPercent <= health) { return Me; }

                    if (partyCount < 6 && !InProvingGrounds)
                    {
                         t = PartyMembers.Where(p => p.IsAlive
                            && p.Distance <= 40
                            && p.InLineOfSight
                            && p.HealthPercent <= health).OrderBy(p => p.HealthPercent).FirstOrDefault();
                        return t != null ? t : null;
                    }
                    if (partyCount > 5 && !InProvingGrounds)
                    {
                        t = PartyMembers.Where(p => p.IsAlive
                           && p.Distance <= 40
                           && p.InLineOfSight
                           && p.HealthPercent <= health).OrderBy(p => p.HealthPercent).FirstOrDefault();
                        return t != null ? t : null;
                    }
                    if (partyCount > 0 && InProvingGrounds)
                    {
                        t = PartyMembers.Where(p => p != null
                            && p.Distance <= 40
                            && p.InLineOfSight
                            && p.HealthPercent <= health).FirstOrDefault();
                        return t != null ? t : null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "FoNTarget: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region healing touch
        public static WoWUnit HealingTouchTarget
        {
            get
            {
                WoWUnit t = null;
                try
                {
                    int health = PD.myPrefs.HealingTouchD;
                    if (partyCount > 5) health = PD.myPrefs.HealingTouchR;
                    if (partyCount == 0 && Me.HealthPercent <= health) { return Me; }
                    if (partyCount > 0 && !InProvingGrounds)
                    {
                        t = PartyMembers.Where(p => p.IsAlive
                            && p.Distance <= 40
                            && p.InLineOfSight
                            && p.HealthPercent <= health).OrderBy(p => p.HealthPercent).FirstOrDefault();
                        return t != null ? t : null;
                    }
                    if (partyCount > 0 && InProvingGrounds)
                    {
                        t = PartyMembers.Where(p => p.Name == "Oto the Protector"
                            && p.Distance <= 40
                            && p.InLineOfSight
                            && p.HealthPercent <= health).FirstOrDefault();
                        return t != null ? t : null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "HealingTouchTarget: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region swiftmend
        public static WoWUnit SwiftMendTarget 
        {
            get
            {
                WoWUnit t = null;
                try
                {
                    int health = PD.myPrefs.SwiftmendHealthPercentD;
                    if (partyCount > 5) health = PD.myPrefs.SwiftmendHealthPercentR;

                    if (partyCount == 0
                        && (Me.HasAura(_rejuvenation) || Me.HasAura(_regrowth))
                        && Me.HealthPercent <= health) { return Me; }
                    if (partyCount > 0)
                    {
                         t = PartyMembers.Where(p => p.HealthPercent <= health
                            && p.Distance <= 40
                            && p.InLineOfSight
                            && (p.HasAura(_rejuvenation) || p.HasAura(_regrowth))).OrderBy(p => p.HealthPercent).FirstOrDefault();
                        return t != null ? t : null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "SwiftMendTarget: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region regrowth
        public static WoWUnit RegrowthTarget
        {
            get
            {
                WoWUnit t = null;
                try
                {
                    int health = PD.myPrefs.RegrowthD;
                    if (partyCount > 5) health = PD.myPrefs.RegrowthR;

                    if (partyCount == 0 && Me.HealthPercent <= health) { return Me; }

                    if (partyCount > 0)
                    {
                         t = PartyMembers.Where(p => p.IsAlive
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && p.HealthPercent <= health).OrderBy(p => p.HealthPercent).FirstOrDefault();
                        return t != null ? t : null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "RegrowthTarget: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region harmony
        public static WoWUnit HarmonyTarget
        {
            get
            {
                WoWUnit t = null;
                try
                {
                    if (partyCount == 0) return null;
                    if (partyCount > 0)
                    {
                        t = PartyMembers.Where(p => p != null
                            && p.IsAlive
                            && p.InLineOfSight
                            && p.Distance <= 40).OrderBy(p => p.HealthPercent).FirstOrDefault();
                        return t != null ? t : null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "HarmonyTarget: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region Ironbark
        public static WoWUnit IronBarkTarget
        {
            get
            {
                WoWUnit t = null;
                try
                {
                    int health = PD.myPrefs.IronBarkHealthPercentD;
                    if (partyCount > 5) health = PD.myPrefs.IronBarkHealthPercentR;

                    if (partyCount == 0 && Me.HealthPercent <= PD.myPrefs.IronBarkHealthPercentD) return Me;

                    if (partyCount > 0 && InProvingGrounds)
                    {
                        t = PartyMembers.Where(p => p != null
                            && p.Name == "Oto the Protector"
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && p.HealthPercent <= health).FirstOrDefault();
                        return t != null ? t : null;
                    }
                    if (partyCount > 0 && !InProvingGrounds)
                    {
                        t = Tanks.Where(p => p != null
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && p.HealthPercent <= health).FirstOrDefault();
                        return t != null ? t : null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "IronBark: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region rejuvenation
        public static WoWUnit KeepRejuOnTankUnit
        {
            get
            {
                var t = new List<WoWUnit>();
                try
                {
                    
                    if (partyCount == 0 && !Me.HasAura(_rejuvenation)) return Me;

                    if (partyCount > 0 && InProvingGrounds && PD.myPrefs.RejuvenationDOnTank)
                    {
                         t = PartyMembers.Where(p => p != null
                            && p.Name == "Oto the Protector"
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && !p.HasAura(_rejuvenation)
                            && p.HealthPercent > 0).ToList();
                         return t.FirstOrDefault() != null ? t.FirstOrDefault() : null;
                    }
                    if (partyCount < 6 && !InProvingGrounds)
                    {
                        t = Tanks.Where(p => p != null
                            && p.IsAlive
                            && p.Distance <= 40
                            && p.InLineOfSight).ToList();
                        foreach (WoWUnit unit in t)
                        {
                            if (!unit.HasAura(_rejuvenation)) return unit;
                        }
                    }
                    if (partyCount > 5 && !InProvingGrounds)
                    {
                        t = Tanks.Where(p => p != null
                            && p.IsAlive
                            && p.Distance <= 40
                            && p.InLineOfSight).ToList();
                        foreach (WoWUnit unit in t)
                        {
                            if (!units.buffExists("Rejuvenation", unit)) return unit;
                        }
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "KeepRejuOnTank: " + e); return null; }
                return null;
            }
        }
        public static WoWUnit RejuvenationTarget
        {
            get
            {
                try
                {
                    int health = PD.myPrefs.RejuvenationD;
                    if (partyCount > 5) health = PD.myPrefs.RejuvenationR;

                    if (partyCount > 0)
                    {
                        var t = PartyMembers.Where(p => p != null
                            && p.IsAlive
                            && p.Distance <= 40
                            && p.InLineOfSight
                            && p.HealthPercent <= health
                            && !p.HasAura(_rejuvenation)).FirstOrDefault();
                        return t != null ? t : null; ;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "RejuvenationTarget: " + e); return null; }
                return null;

            }
        }
        #endregion

        #region germination
        public static WoWUnit KeepGerminationOnTankUnit
        {
            get
            {

                var t = new List<WoWUnit>();
                try
                {
                    if (partyCount == 0 && Me.HasAura(_rejuvenation) && !Me.HasAura(_germinationbuff) && PD.myPrefs.GerminationEnableD) return Me;

                    if (partyCount > 0 && InProvingGrounds && PD.myPrefs.GerminationDOnTank && PD.myPrefs.GerminationEnableD)
                    {
                        t = PartyMembers.Where(p => p != null
                            && p.Name == "Oto the Protector"
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && p.HasAura(_rejuvenation)
                            && !p.HasAura(_germinationbuff)).ToList();
                        return t.FirstOrDefault() != null ? t.FirstOrDefault() : null;
                    }
                    if (partyCount < 6 && !InProvingGrounds && PD.myPrefs.GerminationDOnTank && PD.myPrefs.GerminationEnableD)
                    {
                        t = Tanks.Where(p => p != null
                            && p.IsAlive
                            && p.Distance <= 40
                            && p.InLineOfSight
                            && p.HasAura(_rejuvenation)
                            && !p.HasAura(_germinationbuff)).ToList();
                        return t.FirstOrDefault() != null ? t.FirstOrDefault() : null;
                    }
                    if (partyCount > 5 && !InProvingGrounds && PD.myPrefs.GerminationROnTank && PD.myPrefs.GerminationEnableR)
                    {
                        t = Tanks.Where(p => p != null
                            && p.IsAlive
                            && p.Distance <= 40
                            && p.InLineOfSight
                            && p.HasAura(_rejuvenation)
                            && !p.HasAura(_germinationbuff)).ToList();
                        return t.FirstOrDefault() != null ? t.FirstOrDefault() : null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "KeepGerminationOnTank: " + e); return null; }
                return null;
                
            }
        }
        public static WoWUnit GerminationTarget
        {
            get
            {
                WoWUnit t = null;
                try
                {
                    int health = PD.myPrefs.GerminationD;
                    if (partyCount > 5) health = PD.myPrefs.GerminationR;

                    if (partyCount > 0)
                    {
                        t = PartyMembers.Where(p => p != null
                            && p.IsAlive
                            && p.Distance <= 40
                            && p.InLineOfSight
                            && p.HealthPercent <= health
                            && p.HasAura(_rejuvenation)
                            && !p.HasAura(_germinationbuff)).OrderBy(p => p.HealthPercent).FirstOrDefault();
                        return t != null ? t : null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "GerminationTarget: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region lifebloom
        public static WoWUnit lifebloomTarget
        {
            get
            {
                var t = new List<WoWUnit>();
                try
                {
                    if (partyCount == 0 && !Me.HasAura(_lifebloom)) { return Me; }

                    if (partyCount > 0 && InProvingGrounds)
                    {
                        t = PartyMembers.Where(p => p.Name == "Oto the Protector"
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && !p.HasAura(_lifebloom)).ToList();
                        return t.FirstOrDefault();
                    }
                    if (partyCount > 0 && !InProvingGrounds)
                    {
                        t = Tanks.Where(p => p.IsAlive
                            && p.InLineOfSight
                            && p.Distance <= 40).ToList();
                        foreach (WoWUnit unit in t)
                        {
                            if (units.buffExists(_lifebloom, unit) && units.MyBuffExists(_lifebloom, unit)) return null;
                            if (!units.buffExists(_lifebloom, unit)) return unit;
                        }
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "LifebloomTarget: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region genesis
        public static WoWUnit genesisTarget
        {
            get
            {
                List<WoWUnit> t = new List<WoWUnit>();
                try
                {
                    if (partyCount == 0) return null;

                    if (!PD.myPrefs.GenesisUseD && partyCount < 6) return null;
                    if (!PD.myPrefs.GenesisUseR && partyCount > 5) return null;

                    int health = PD.myPrefs.GenesisHealthPercentD;
                    if (partyCount > 5) health = PD.myPrefs.GenesisHealthPercentR;

                    int playerCount = PD.myPrefs.GenesisPlayerCountD;
                    if (partyCount > 5) playerCount = PD.myPrefs.GenesisPlayerCountR;

                    if (partyCount > 0)
                    {
                        t = PartyMembers.Where(p => p.HealthPercent <= health
                               && p.InLineOfSight
                               && p.Distance <= 40
                               && p.HasAura(_rejuvenation)).OrderBy(p => p.HealthPercent).ToList();
                        if (t.Count() >= playerCount) { return t.FirstOrDefault(); }
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "KeepRejuOnTank: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region wildgrowth
        public static WoWUnit wildgrowthTarget
        {
            get
            {
                List<WoWUnit> t = new List<WoWUnit>();
                try
                {
                    if (partyCount == 0) return null;

                    int health = PD.myPrefs.WildGrowthHealthPercentD;
                    if (partyCount > 5) health = PD.myPrefs.WildGrowthHealthPercentR;

                    int playerCount = PD.myPrefs.WildGrowthPlayerCountD;
                    if (partyCount > 5) playerCount = PD.myPrefs.WildGrowthPlayerCountR;

                    if (partyCount > 0)
                    {
                        t = PartyMembers.Where(p => p.HealthPercent <= health
                                && p.InLineOfSight
                                && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();
                        if (t.Count() >= playerCount) { return t.FirstOrDefault(); }
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "Wild Growth Target: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region tranquility
        public static WoWUnit TranquilityUnit
        {
            get
            {
                List<WoWUnit> t = new List<WoWUnit>();
                try
                {
                    if (partyCount < 6 && !PD.myPrefs.TranquilityUseD) return null;
                    if (partyCount > 5 && !PD.myPrefs.TranquilityUseR) return null;

                    if (partyCount == 0) return null;

                    int health = PD.myPrefs.TranquilityHealthPercentD;
                    if (partyCount > 5) health = PD.myPrefs.TranquilityHealthPercentR;

                    int playerCount = PD.myPrefs.TranquilityPlayerCountD;
                    if (partyCount > 5) playerCount = PD.myPrefs.TranquilityPlayerCountR;

                    if (partyCount > 0)
                    {
                        t = PartyMembers.Where(p => p.HealthPercent <= health
                            && p.InLineOfSight
                            && p.Distance <= 40).ToList();
                        if (t.Count() >= playerCount) return Me;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "Tranquility: " + e); return null; }
                return null;
            }
        }
        #endregion

        #region wild mushroom
        public static int MUSHROOM_ID = 47649;
        public static DateTime lastMushroomCast;
        public static int mushroomCheck
        {
            get
            {
                var t = ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.Entry == MUSHROOM_ID && o.CreatedByUnitGuid == StyxWoW.Me.Guid && o.Distance <= 40);
                return t.Count();
            }
        }
        public static WoWUnit MushroomTarget
        {
            get
            {
                WoWPoint mushPlacement;
                var list = new List<WoWUnit>();
                try
                {
                    if (partyCount == 0) return null;

                    int health = PD.myPrefs.WildMushroomHealthPercentD;
                    if (partyCount > 5) health = PD.myPrefs.WildMushroomHealthPercentR;

                    int playerCount = PD.myPrefs.WildMushroomPlayerCountD;
                    if (partyCount > 5) playerCount = PD.myPrefs.WildMushroomPlayerCountR;

                    if (mushroomCheck == 0)
                    {
                        var t = PartyMembers.Where(p => p != null
                            && p.IsAlive
                            && p.HealthPercent <= health
                            && p.Distance <= 40).OrderBy(p => p.HealthPercent).FirstOrDefault();
                        if (t != null)
                        {
                            mushPlacement = t.Location;
                            list = PartyMembers.Where(p => p != null
                                && p.IsAlive
                                && p.HealthPercent <= health
                                && p.Location.Distance(mushPlacement) <= 15).ToList();
                            if (list.Count() >= playerCount) 
                            { 
                                return list.FirstOrDefault(); 
                            }
                        }
                        return null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "Wild Mushroom " + e); return null; }
                return null;
            }
        }
        #endregion

        #region dpstargets

        public static WoWUnit dpsTarget
        {
            get
            {
                if (partyCount == 0 && Me.CurrentTarget != null && units.ValidUnit(Me.CurrentTarget)) return Me.CurrentTarget;
                else
                {
                    var t = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(p => p.IsAlive
                        && (p.Combat
                        && (p.IsTargetingMeOrPet || p.IsTargetingMyPartyMember || p.IsTargetingMyRaidMember))
                        && p.InLineOfSight
                        && p.Distance <= 40
                        && Me.IsSafelyFacing(p)).OrderBy(p => p.Distance).ToList();
                    return t.Count() > 0 ? t.FirstOrDefault() : null;
                }
            }
        }
        public static bool needDpsTarget()
        {
            return SpellManager.HasSpell(_dreamofcenarius) && partyCount > 0 && Me.ManaPercent >= 60;
        }
        #endregion

        #region nature's cure
        public static WoWUnit dispelTarget
        {
            get
            {
                WoWUnit t = null;
                try
                {
                    if (!PD.myPrefs.AutoDispel) return null;

                    if (partyCount == 0
                        && units.HasDebuff(Me))
                    {
                        foreach (var debuff in Me.Debuffs.Values)
                        {
                            if (debuff.Spell.DispelType == WoWDispelType.Magic
                                || debuff.Spell.DispelType == WoWDispelType.Poison
                                || debuff.Spell.DispelType == WoWDispelType.Curse)
                            {
                                return Me;
                            }
                        }
                        return null;
                    }
                    if (partyCount > 0)
                    {
                        t = PartyMembers.Where(p => p != null
                            && p.IsAlive
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && units.HasDebuff(p)).FirstOrDefault();
                        if (t != null)
                        {
                            foreach (var debuff in t.Debuffs.Values)
                            {
                                if (debuff.Spell.DispelType == WoWDispelType.Magic
                                    || debuff.Spell.DispelType == WoWDispelType.Poison
                                    || debuff.Spell.DispelType == WoWDispelType.Curse)
                                {
                                    return t;
                                }
                            }
                        }
                        return null;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "Auto Dispel " + e); return null; }
                return null;
            }
        }
        #endregion

        #region incarnation
        public static bool needIncarnation
        {
            get
            {
                var t = new List<WoWUnit>();
                try
                {
                    if (!SpellManager.HasSpell(_incarnationresto)) return false;
                    if (units.spellOnCooldown(_incarnationresto)) return false;
                    if (partyCount == 0) return false;
                    if (PD.myPrefs.IncarnationD == PD.UseIncarnation.OnCoolDown && !units.spellOnCooldown(_incarnationresto) && partyCount < 6) return true;
                    if (PD.myPrefs.IncarnationR == PD.UseIncarnation.OnCoolDown && !units.spellOnCooldown(_incarnationresto) && partyCount > 5) return true;

                    if (PD.myPrefs.IncarnationD == PD.UseIncarnation.OnCoolDownBosses && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss && partyCount < 6) return true;
                    if (PD.myPrefs.IncarnationR == PD.UseIncarnation.OnCoolDownBosses && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss && partyCount > 5) return true;

                    if (PD.myPrefs.IncarnationD == PD.UseIncarnation.Manual && partyCount < 6) return false;
                    if (PD.myPrefs.IncarnationR == PD.UseIncarnation.Manual && partyCount > 5) return false;
                    if (PD.myPrefs.IncarnationD == PD.UseIncarnation.OnLowHealthPlayers && partyCount < 6)
                    {
                        t = PartyMembers.Where(p => p != null
                            && p.IsAlive
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && p.HealthPercent <= PD.myPrefs.IncarnationPercentD).ToList();
                        if (t.Count() >= PD.myPrefs.IncarnationPlayerCountD) return true;
                    }
                    if (PD.myPrefs.IncarnationR == PD.UseIncarnation.OnLowHealthPlayers && partyCount > 5)
                    {
                        t = PartyMembers.Where(p => p != null
                            && p.IsAlive
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && p.HealthPercent <= PD.myPrefs.IncarnationPercentR).ToList();
                        if (t.Count() >= PD.myPrefs.IncarnationPlayerCountR) return true;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "Incarnation " + e); return false; }
                return false;
            }
        }
        #endregion

        #region nature's vigil
        public static bool needNaturesVigil
        {
            get
            {
                if (!SpellManager.HasSpell(_naturesvigil)) return false;
                var t = new List<WoWUnit>();
                try
                {
                    if (partyCount == 0) return false;
                    if (!units.spellOnCooldown(_naturesvigil)) return false;
                    if (partyCount > 0)
                    {
                        t = PartyMembers.Where(p => p != null
                            && p.IsAlive
                            && p.InLineOfSight
                            && p.Distance <= 40
                            && p.HealthPercent <= PD.myPrefs.NaturesVigilPercent).ToList();
                        if (t.Count() >= PD.myPrefs.NaturesVigilPlayersCount) return true;
                    }
                }
                catch (Exception e) { Logging.Write(Colors.Bisque, "Nature's Vigil " + e); return false; }
                return false;
            }
        }
        #endregion

    }
}
