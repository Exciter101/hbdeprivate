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

using P = Kitty.KittySettings;
using HKM = Kitty.KittyHotkeyManagers;
using Styx.Common.Helpers;

namespace Kitty
{
    public partial class KittyMain : CombatRoutine
    {
        public const string
            MANGLE = "Mangle",
            LACERATE = "Lacerate",
            MAUL = "Maul",
            THRASH = "Thrash",
            PULVERIZE = "Pulverize",
            HEALING_TOUCH = "Healing Touch",
            REJUVENATION = "Rejuvenation",
            RAKE = "Rake",
            RIP = "Rip",
            WILD_CHARGE = "Wild Charge",
            FEROCIUOS_BITE = "Ferocious Bite",
            SAVAGE_ROAR = "Savage Roar",
            PREDATORY_SWIFTNESS = "Predatory Swiftness",
            CLEARCASTING = "Clearcasting",
            SKULL_BASH = "Skull Bash",
            MIGHTY_BASH = "Mighty Bash",
            TYPHOON = "Typhoon",
            INCAPACITATING_ROAR = "Incapacitating Roar",
            SHRED = "Shred",
            SWIPE = "Swipe",
            MARK_OF_THE_WILD = "Mark of the Wild",
            LEGACY_OF_THE_EMPEROR = "Legacy of the Emperor",
            BLESSING_OF_KINGS = "Blessing of Kings",
            BLOODLUST = "Bloodlust",
            HEROISM = "Heroism",
            TIME_WARP = "Time Warp",
            ANCIENT_HYSTERIA = "Ancient Hysteria",
            MOONFIRE = "Moonfire",
            WRATH = "Wrath",
            PROWL = "Prowl",
            DASH = "Dash",
            STAMPEDING_ROAR = "Stampeding Roar",
            REBIRTH = "Rebirth",
            FAERIE_FIRE = "Faerie Fire",
            FAERIE_SWARM = "Faerie Swarm",
            BEAR_FORM = "Bear Form",
            CAT_FORM = "Cat Form",
            CLAWS_OF_SHIRVALLAH = "Claws of Shirvallah",
            MOONKIN_FORM = "Moonkin Form",
            TOOTH_AND_CLAW = "Tooth and Claw",
            FRENZIED_REGENERATION = "Frenzied Regeneration",
            SAVAGE_DEFENSE = "Savage Defense",
            BERSERK = "Berserk",
            ENHANCED_AGILITY = "Enhanced Agility",
            ENHANCED_STRENGHT = "Enhanced Strenght",
            ENHANCED_INTELLECT = "Enhanced Intellect",
            DREAM_OF_CENARIUS = "Dream of Cenarius",
            TIGERS_FURY = "Tiger's Fury",
            INCARNATION_CAT = "Incarnation: King of the Jungle",
            INCARNATION_BEAR = "Incarnation: Son of Ursoc",
            FORCE_OF_NATURE = "Force of Nature",
            WAR_STOMP = "War Stomp",
            BARKSKIN = "Barkskin",
            SURVIVAL_INSTINCTS = "Survival Instincts",
            GROWL = "Growl",
            TRAVEL_FORM = "Travel Form",
            //healing
            REGROWTH = "Regrowth",
            WILD_GROWTH = "Wild Growth",
            WILD_MUSHROOM = "Wild Mushroom",
            GENESIS = "Genesis",
            SWIFTMEND = "Swiftmend",
            NATURES_VIGIL = "Nature's Vigil",
            NATURES_SWIFTNESS = "Nature's Swiftness",
            TRANQUILITY = "Tranquility",
            LIFEBLOOM = "Lifebloom",
            NATURES_CURE = "Nature's Cure",
            LUNAR_INSPIRATION = "Lunar Inspiration",
            IRONBARK = "Ironbark",

            STARFIRE = "Starfire",
            STARSURGE = "Starsurge",
            ASTRAL_COMMUNION = "Astral Communion",
            CELESTIAL_ALIGNMENT = "Celestial Alignment",
            SOLAR_BEAM = "Solar Beam",
            SOLAR_PEAK = "Solar Peak",
            LUNAR_PEAK = "Lunar Peak",
            STARFALL = "Starfall",
            SUNFIRE = "Sunfire",
            HURRICANE = "Hurricane",

            EINDE = "The End";


        public const int
            ALCHEMYFLASK_ITEM = 75525,
            CRYSTAL_OF_INSANITY_ITEM = 86569,
            CRYSTAL_OF_INSANITY_BUFF = 127230,
            HEALTHSTONE_ITEM = 5512,
            CRYSTAL_OF_ORALIUS_BUFF = 176151,
            CRYSTAL_OF_ORALIUS_ITEM = 118922,
            DREAM_OF_CENARIUS_INT = 145162,
            HEALING_TOUCH_INT = 5185,
            REGROWTH_INT = 8936,
            SAVAGE_ROAR_GLYPH = 155836,

            STARFIRE_INT = 2912,
            WRATH_INT = 5176,
            MOONFIRE_INT = 8921,
            STARSURGE_INT = 78674,
            SUNFIRE_INT = 93402,
            EIND = 0;

        public static string LSPELLCAST = string.Empty;
        public static string FF { get { return !SpellManager.HasSpell(FAERIE_SWARM) ? FAERIE_FIRE : FAERIE_SWARM; } }
        public static string FERALFORM { get { return !SpellManager.HasSpell(CLAWS_OF_SHIRVALLAH) ? CAT_FORM : CLAWS_OF_SHIRVALLAH; } }

        public static DateTime fonTimer;

        #region trinkets
        public static bool UseTrinket1
        {
            get
            {
                if (MeIsResto)
                {
                    if (P.myPrefs.Trinket1UseResto) return false;
                    if (P.myPrefs.Trinket1Resto == 1) return false;
                    if (P.myPrefs.Trinket1Resto == 2 && (HKM.cooldownsOn || (Targets.IsWoWBoss(Me.CurrentTarget) && AutoBot))) return true;
                    if (P.myPrefs.Trinket1Resto == 3 && IsCrowdControlledPlayer(Me)) return true;
                    if (P.myPrefs.Trinket1Resto == 4 && Me.EnergyPercent <= P.myPrefs.PercentTrinket1EnergyResto) return true;
                    if (P.myPrefs.Trinket1Resto == 5 && Me.ManaPercent >= P.myPrefs.PercentTrinket1ManaResto) return true;
                    if (P.myPrefs.Trinket1Resto == 6 && Me.HealthPercent <= P.myPrefs.PercentTrinket1HPResto) return true;
                }
                else
                {
                    if (P.myPrefs.Trinket1Use) return false;
                    if (P.myPrefs.Trinket1 == 1) return false;
                    if (P.myPrefs.Trinket1 == 2 && (HKM.cooldownsOn || (Targets.IsWoWBoss(Me.CurrentTarget) && AutoBot))) return true;
                    if (P.myPrefs.Trinket1 == 3 && IsCrowdControlledPlayer(Me)) return true;
                    if (P.myPrefs.Trinket1 == 4 && Me.EnergyPercent <= P.myPrefs.PercentTrinket1Energy) return true;
                    if (P.myPrefs.Trinket1 == 5 && Me.ManaPercent >= P.myPrefs.PercentTrinket1Mana) return true;
                    if (P.myPrefs.Trinket1 == 6 && Me.HealthPercent <= P.myPrefs.PercentTrinket1HP) return true;
                }
                return false;
            }
        }
        public static bool UseTrinket2
        {
            get
            {
                if (MeIsResto)
                {
                    if (P.myPrefs.Trinket2UseResto) return false;
                    if (P.myPrefs.Trinket2Resto == 1) return false;
                    if (P.myPrefs.Trinket2Resto == 2 && (HKM.cooldownsOn || (Targets.IsWoWBoss(Me.CurrentTarget) && AutoBot)))
                    if (P.myPrefs.Trinket2Resto == 3 && IsCrowdControlledPlayer(Me)) return true;
                    if (P.myPrefs.Trinket2Resto == 4 && Me.EnergyPercent <= P.myPrefs.PercentTrinket2EnergyResto) return true;
                    if (P.myPrefs.Trinket2Resto == 5 && Me.ManaPercent >= P.myPrefs.PercentTrinket2ManaResto) return true;
                    if (P.myPrefs.Trinket2Resto == 6 && Me.HealthPercent <= P.myPrefs.PercentTrinket2HPResto) return true;
                }
                else
                {
                    if (P.myPrefs.Trinket2Use) return false;
                    if (P.myPrefs.Trinket2 == 1) return false;
                    if (P.myPrefs.Trinket2 == 2 && (HKM.cooldownsOn || (Targets.IsWoWBoss(Me.CurrentTarget) && AutoBot)))
                    if (P.myPrefs.Trinket2 == 3 && IsCrowdControlledPlayer(Me)) return true;
                    if (P.myPrefs.Trinket2 == 4 && Me.EnergyPercent <= P.myPrefs.PercentTrinket2Energy) return true;
                    if (P.myPrefs.Trinket2 == 5 && Me.ManaPercent >= P.myPrefs.PercentTrinket2Mana) return true;
                    if (P.myPrefs.Trinket2 == 6 && Me.HealthPercent <= P.myPrefs.PercentTrinket2HP) return true;
                }
                return false;
            }
        }
        private static bool CanUseEquippedItem(WoWItem item)
        {
            string itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;
            return item.Usable && item.Cooldown <= 0;
        }
        #endregion

        #region hastebuffs
        public static bool HaveHasteBuff
        {
            get
            {
                return Me.HasAura(BLOODLUST)
                    || Me.HasAura(HEROISM)
                    || Me.HasAura(TIME_WARP)
                    || Me.HasAura("Haste")
                    || Me.HasAura("Berserking")
                    || Me.HasAura(ANCIENT_HYSTERIA);
            }
        }
        #endregion

        #region bear conditions
        public static bool IncarnationBearConditions
        {
            get
            {
                if (!spellOnCooldown(INCARNATION_BEAR)
                    && !buffExists(INCARNATION_BEAR, Me)
                    && buffExists(BERSERK, Me)) return true;
                return false;
            }
        }
        public static bool BearMaulConditions
        {
            get
            {
                if (spellOnCooldown(MAUL)) return false;
                if (!SpellManager.HasSpell(TOOTH_AND_CLAW) && Me.RagePercent >= 20) return true;
                if (SpellManager.HasSpell(TOOTH_AND_CLAW) && buffExists(TOOTH_AND_CLAW, Me) && Me.RagePercent >= 20) return true;
                return false;
            }
        }
        public static bool BearThrashConditions
        {
            get
            {
                if (!spellOnCooldown(THRASH)
                && !debuffExists(THRASH, Me.CurrentTarget) || (debuffExists(THRASH, Me.CurrentTarget) && debuffTimeLeft(THRASH, Me.CurrentTarget) <= 4000))
                {
                    return true;
                }
                return false;
            }
        }
        public static bool BearLacerateConditions
        {
            get
            {
                if ((!debuffExists(LACERATE, Me.CurrentTarget)
                    || (debuffExists(LACERATE, Me.CurrentTarget) && debuffStackCount(LACERATE, Me.CurrentTarget) <= 3))
                    || (debuffExists(LACERATE, Me.CurrentTarget)
                    && debuffStackCount(LACERATE, Me.CurrentTarget) >= 3
                    && debuffTimeLeft(LACERATE, Me.CurrentTarget) <= 4500))
                {
                    return true;
                }
                return false;
            }
        }
        public static bool BearPulverizeConditions
        {
            get
            {
                return Me.CurrentTarget != null && debuffExists(LACERATE, Me.CurrentTarget) && debuffStackCount(LACERATE, Me.CurrentTarget) >= 3;
            }
        }
        public static bool BearFrenziedRegenerationConditions
        {
            get
            {
                if (!spellOnCooldown(FRENZIED_REGENERATION)
                    && Me.HealthPercent <= P.myPrefs.PercentFrenziedRegeneration
                    && Me.RagePercent > 80)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool BearSavageDefenseConditions
        {
            get
            {
                if (!spellOnCooldown(SAVAGE_DEFENSE)
                    && Me.RagePercent >= 60
                    && Me.HealthPercent <= P.myPrefs.PercentDavageDefense)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region buff condtions
        public static bool MarkOfTheWildConditions
        {
            get
            {
                return !Me.HasAura(MARK_OF_THE_WILD) && !Me.HasAura(LEGACY_OF_THE_EMPEROR) && !Me.HasAura(BLESSING_OF_KINGS);
            }
        }
        public static bool AlchemyFlaskConditions
        {
            get
            {
                return !Me.HasAura(ENHANCED_AGILITY) 
                    && !Me.HasAura(ENHANCED_INTELLECT) 
                    && !Me.HasAura(ENHANCED_STRENGHT) 
                    && !Me.HasAura(CRYSTAL_OF_INSANITY_BUFF)
                    && !Me.HasAura(CRYSTAL_OF_ORALIUS_BUFF)
                    && P.myPrefs.FlaskAlchemy;
            }
        }
        public static bool CrystalOfOraliusConditions
        {
            get { return !Me.HasAura(CRYSTAL_OF_ORALIUS_BUFF) && P.myPrefs.FlaskOraliusCrystal; }
        }
        public static bool CrystalOfInsanityConditions
        {
            get { return !Me.HasAura(CRYSTAL_OF_ORALIUS_BUFF) && !Me.HasAura(CRYSTAL_OF_INSANITY_BUFF) && P.myPrefs.FlaskCrystal; }
        }
        public static bool BerserkBearConditions
        {
            get
            {
                if (!spellOnCooldown(BERSERK)
                    && !buffExists(BERSERK, Me)
                    && (Targets.IsWoWBoss(Me.CurrentTarget) && AutoBot) || HKM.cooldownsOn) return true;
                return false;
            }
        }
        #endregion

        #region feral conditions
        public static bool SavageRoarConditions
        {
            get
            {
                if(!buffExists(SAVAGE_ROAR, Me)
                    && !SpellManager.HasSpell(SAVAGE_ROAR_GLYPH)
                    && Me.EnergyPercent >= 25
                    && Me.ComboPoints >= 1) 
                { 
                    return true; 
                }
                return false;
            }
        }
        public static bool SR_BUFF_UP { get { return buffExists(SAVAGE_ROAR, Me); } }

        public static bool TigersFuryConditions { get { return !spellOnCooldown(TIGERS_FURY) && Me.EnergyPercent < 30; } }

        public static bool WildChargeConditions(float min, float max)
        {
            return Me.CurrentTarget != null && (Me.CurrentTarget.Distance >= min && Me.CurrentTarget.Distance <= max);
        }

        public static bool FerociousBiteConditions
        {
            get
            {
                if(debuffExists(RIP, Me.CurrentTarget)
                    && Me.EnergyPercent >= 25
                    && Me.ComboPoints >= 5)
                {
                    return true;
                }
                if (Me.CurrentTarget.HealthPercent < 25
                    && debuffExists(RIP, Me.CurrentTarget)
                    && debuffTimeLeft(RIP, Me.CurrentTarget) <= 4500
                    && Me.EnergyPercent >= 25
                    && Me.ComboPoints >= 1)
                {
                    return true;
                }
                if (debuffExists(RIP, Me.CurrentTarget)
                    && debuffTimeLeft(RIP, Me.CurrentTarget) > 6000
                    && debuffTimeLeft(SAVAGE_ROAR, Me) > 6000
                    && Me.EnergyPercent >= 25
                    && Me.ComboPoints >= 5)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool needRip { get { return SpellManager.HasSpell(RIP) && Me.CurrentTarget.MaxHealth > Me.MaxHealth * 1.5; } }
        public static bool RipConditions
        {
            get
            {
                if((!debuffExists(RIP, Me.CurrentTarget)
                    || (debuffExists(RIP, Me.CurrentTarget) && debuffTimeLeft(RIP, Me.CurrentTarget) <= 6000))
                    && Me.EnergyPercent >= 30
                    && Me.ComboPoints >= 5)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool RakeConditions
        {
            get
            {
                if((!debuffExists(RAKE, Me.CurrentTarget)
                    || (debuffExists(RAKE, Me.CurrentTarget) && debuffTimeLeft(RAKE, Me.CurrentTarget) <= 5000))
                    && Me.EnergyPercent >= 35
                    && (addCount < 2 || !SpellManager.HasSpell(THRASH)))
                {
                    return true;
                }
                return false;
            }
        }
        public static bool ThrashConditions
        {
            get
            {
                if (!debuffExists(THRASH, Me.CurrentTarget)
                    && addCount > 1 
                    && SpellManager.HasSpell(THRASH)
                    && Me.EnergyPercent >= 50 || buffExists(CLEARCASTING, Me))
                {
                    return true;
                }
                return false;
            }
        }
        public static bool ShredConditions
        {
            get
            {
                if (Me.EnergyPercent >= 40 && !buffExists(SAVAGE_ROAR, Me) && (addCount < 4 || !SpellManager.HasSpell(SWIPE) || HKM.aoeStop)) { return true; }
                if (Me.EnergyPercent >= 50 && buffExists(SAVAGE_ROAR, Me) && (addCount < 4 || !SpellManager.HasSpell(SWIPE) || HKM.aoeStop)) { return true; }
                return false;
            }
        }
        public static bool SwipeConditions
        {
            get
            {
                if(SpellManager.HasSpell(SWIPE)
                    && !HKM.aoeStop
                    && addCount >= 4
                    && Me.EnergyPercent >= 45)
                {
                    return true;
                }
                return false;
            }
        }
        public static bool BerserkConditions
        {
            get
            {
                if (!spellOnCooldown(BERSERK)
                    && (Targets.IsWoWBoss(Me.CurrentTarget) && AutoBot) || HKM.cooldownsOn) return true;
                return false;
            }
        }
        public static bool IncarnationCatConditions
        {
            get
            {
                if (!spellOnCooldown(INCARNATION_CAT)
                    && buffExists(BERSERK, Me)) return true;
                return false;
            }
        }
        #endregion

        #region interrupts
        public static DateTime interruptTimer;
        public static DateTime stunTimer;
        public static bool SkullBashConditions(WoWUnit unit)
        {
          if(P.myPrefs.AutoInterrupt
              && unit.IsCasting
              && Me.CanInterruptCurrentSpellCast
              && !spellOnCooldown(SKULL_BASH)
              && interruptTimer <= DateTime.Now)
          {
              interruptTimer.AddSeconds(5);
              return true;
          }
          return false;
        }
        public static bool TyphoonConditions(WoWUnit unit)
        {
            if (P.myPrefs.AutoInterrupt
                && unit.IsCasting
                && Me.CanInterruptCurrentSpellCast
                && !spellOnCooldown(TYPHOON)
                && interruptTimer <= DateTime.Now)
            {
                interruptTimer.AddSeconds(5);
                return true;
            }
            return false;
        }
        public static bool IncapacitatingRoarConditions(WoWUnit unit)
        {
            if (P.myPrefs.AutoInterrupt
                && unit.IsCasting
                && Me.CanInterruptCurrentSpellCast
                && !spellOnCooldown(INCAPACITATING_ROAR)
                && interruptTimer <= DateTime.Now)
            {
                interruptTimer.AddSeconds(5);
                return true;
            }
            return false;
        }
        public static bool MightyBashConditions(WoWUnit unit)
        {
            if (P.myPrefs.AutoInterrupt
                && unit.IsCasting
                && !Me.CanInterruptCurrentSpellCast
                && !spellOnCooldown(MIGHTY_BASH)
                && stunTimer <= DateTime.Now)
            {
                stunTimer.AddSeconds(5);
                return true;
            }
            return false;
        }
        public static bool WarStompConditions(WoWUnit unit)
        {
            if (P.myPrefs.AutoInterrupt
                && unit.IsCasting
                && !Me.CanInterruptCurrentSpellCast
                && !spellOnCooldown(WAR_STOMP)
                && stunTimer <= DateTime.Now)
            {
                stunTimer.AddSeconds(5);
                return true;
            }
            return false;
        }
        #endregion

        #region overlayed
        public static bool IsOverlayed(int spellID)
        {
            return Lua.GetReturnVal<bool>("return IsSpellOverlayed(" + spellID + ")", 0);
        }
        #endregion
    }
}
