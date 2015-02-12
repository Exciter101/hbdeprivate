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
            RAMPANT_GROWTH = "Rampant Growth",
            GERMINATION = "Germination",

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
            BERSERKING = "Berserking",
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

        public static WoWUnit playerToRes = null;
        public static bool needResPeople
        {
            get
            {
                if (spellOnCooldown(REBIRTH)) return false;
                if (HKM.resTanks)
                {
                    WoWUnit target = Tanks().Where(p => p.IsDead
                        && p.Location.Distance(Me.Location) <= 40).FirstOrDefault();
                    if (target != null) playerToRes = target; return true;
                }
                if (HKM.resHealers)
                {
                    WoWUnit target = Healers().Where(p => p.IsDead
                        && p.Location.Distance(Me.Location) <= 40).FirstOrDefault();
                    if (target != null) playerToRes = target; return true;
                }
                if (HKM.resAll)
                {
                    WoWUnit target = Damage().Where(p => p.IsDead
                        && p.Location.Distance(Me.Location) <= 40).FirstOrDefault();
                    if (target != null) playerToRes = target; return true;
                }
                return false;
            }
        }

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
                if (Me.RagePercent >= 80) return true;
                if (!SpellManager.HasSpell(THRASH) && Me.RagePercent >= 30) return true;
                if (!SpellManager.HasSpell(LACERATE) && Me.RagePercent >= 30) return true;
                if (!SpellManager.HasSpell(TOOTH_AND_CLAW) && Me.RagePercent >= 65) return true;
                if (SpellManager.HasSpell(TOOTH_AND_CLAW) && buffExists(TOOTH_AND_CLAW, Me) && Me.RagePercent >= 65) return true;
                return false;
            }
        }
        public static bool BearThrashConditions
        {
            get
            {
                if (!SpellManager.HasSpell(LACERATE)) return true;
                if (noBearThrashCount > 0) return true;
                if (!debuffExists(THRASH, Me.CurrentTarget)) return true;
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
                    && Me.RagePercent >= 60)
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
        public static bool needSavageRoar
        {
            get
            {
                if (!P.myPrefs.UseSavageRoar) return false;
                if (buffExists(SAVAGE_ROAR, Me)) return false;
                if (Me.EnergyPercent < 25) return false;
                if (Me.ComboPoints < 1) return false;
                if (buffExists(SAVAGE_ROAR, Me) && buffTimeLeft(SAVAGE_ROAR, Me) <= 4000) return true;
                return true;
            }
        }
        public static uint _addsMaxHealth
        {
            get
            {
                uint maxHealth = 0;
                if (Me.CurrentTarget != null)
                {
                    var result = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(p => p != null
                        && p.IsAlive
                        && (p.Combat
                        && (p.IsTargetingMeOrPet || p.IsTargetingMyPartyMember || p.IsTargetingMyRaidMember))
                        && p.DistanceSqr <= 10 * 10).OrderByDescending(p => p.MaxHealth);
                    if (result.Count() > 0) maxHealth = result.FirstOrDefault().MaxHealth;
                }
                return maxHealth;
            }
        }
        public static bool needBossRotation(WoWUnit unit)
        {
            if (unit.IsPlayer) return true;
            if (unit.Name.Contains("Dummy")) return true;
            if (unit.Classification == WoWUnitClassificationType.WorldBoss) return true;
            if (unit.Classification == WoWUnitClassificationType.RareElite && partyCount == 0 && unit.MaxHealth > Me.MaxHealth * 1.5) return true;
            if (unit.Classification == WoWUnitClassificationType.Rare && partyCount == 0 && unit.MaxHealth > Me.MaxHealth * 1.5) return true;
            if (unit.Classification == WoWUnitClassificationType.Elite && partyCount == 0 && unit.MaxHealth > Me.MaxHealth * 1.5) return true;
            if (unit.IsBoss) return true;
            if (unit.HealthPercent >= Me.MaxHealth * 3 && partyCount == 0) return true;
            if (HKM.cooldownsOn) return true;
            return false;
        }
        public static bool validTarget(WoWUnit unit)
        {
            if (Blacklist.Contains(unit, BlacklistFlags.All)) return false;
            if (unit.Name.Contains("Dummy")) return true;
            if (!unit.CanSelect) return false;
            if (unit.IsCritter) return false;
            if (unit.IsNonCombatPet) return false;
            if (unit.IsPet && unit.OwnedByRoot.IsPlayer) return false;
            if (unit.IsDead) return false;
            return true;
        }
        public static bool needRake(WoWUnit unit)
        {
            try
            {
                if (debuffExists(RAKE, unit) && debuffTimeLeft(RAKE, unit) > 4000) return false;
                if (Me.EnergyPercent < 35) return false;
                if (addCount >= 9) return false;
                if (addCount >= 3 && addCount < 9 && _addsMaxHealth < Me.MaxHealth * 1.5) return false;
                if (!Me.IsSafelyFacing(unit)) return false;
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Rake => " + e); }
            return true;
        }
        public static WoWUnit _moonfireTarget
        {
            get
            {
                if (!SpellManager.HasSpell(LUNAR_INSPIRATION)) return null;
                if (Me.CurrentTarget != null && !debuffExists(MOONFIRE, Me.CurrentTarget) && Me.CurrentTarget.Name.Contains("Dummy")) return Me.CurrentTarget;
                if (addCount >= 9) return null;
                var result = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(p => p != null
                            && p.IsAlive
                            && (p.Combat
                            && (p.IsTargetingMeOrPet || p.IsTargetingMyPartyMember || p.IsTargetingMyRaidMember))
                            && !buffExists(MOONFIRE, p)
                            && p.DistanceSqr <= 20 * 20).OrderBy(p => p.Distance).ToList();

                return result.Count() > 0 ? result.FirstOrDefault() : null;
            }
        }
        public static bool needShred(WoWUnit unit)
        {
            try
            {
                if (addCount > 2) return false;
                if (!unit.InLineOfSight) return false;
                if (!unit.IsWithinMeleeRange) return false;
                if (Me.EnergyPercent < 55) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Shred => " + e); }
            return true;
        }
        public static bool needSwipe(WoWUnit unit)
        {
            try
            {
                if (addCount < 3) return false;
                if (!unit.InLineOfSight) return false;
                if (!unit.IsWithinMeleeRange) return false;
                if (Me.EnergyPercent < 45) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Swipe => " + e); }
            return true;
        }
        public static bool needRip(WoWUnit unit)
        {
            try
            {
                if (debuffExists(RIP, unit) && debuffTimeLeft(RIP, unit) > 6000) return false;
                if (Me.ComboPoints < 5) return false;
                if (Me.EnergyPercent < 30) return false;
                if (!unit.InLineOfSight) return false;
                if (!unit.IsWithinMeleeRange) return false;
                if (Me.EnergyPercent < 30) return false;
                if (unit.MaxHealth <= Me.MaxHealth) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Rip => " + e); }
            return true;
        }
        public static bool needFerociousBite(WoWUnit unit)
        {
            try
            {
                if (needRip(unit) && !debuffExists(RIP, unit)) return false;
                if (debuffExists(RIP, unit) && debuffTimeLeft(RIP, unit) <= 6000) return false;
                if (!unit.InLineOfSight) return false;
                if (!unit.IsWithinMeleeRange) return false;
                if (!Me.IsSafelyFacing(unit)) return false;
                if (unit.HealthPercent <= 25 && debuffExists(RIP, unit) && debuffTimeLeft(RIP, unit) <= 5000 && Me.ComboPoints >= 1 && Me.EnergyPercent >= 25) return true;
                if (P.myPrefs.UseSavageRoar && buffExists(SAVAGE_ROAR, Me) && buffTimeLeft(SAVAGE_ROAR, Me) <= 5000) return false;
                if (Me.EnergyPercent < 50) return false;
                if (Me.ComboPoints < 5) return false;

            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Ferocious Bite => " + e); }
            return true;
        }
        public static bool needThrash
        {
            get
            {
                try
                {
                    var result = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(p => p != null
                        && (p.Combat
                        && (p.IsTargetingMeOrPet || p.IsTargetingMyRaidMember || p.IsTargetingMyPartyMember))
                        && !debuffExists(THRASH, p)
                        && p.DistanceSqr <= 10 * 10).ToList();
                    if (!SpellManager.HasSpell(THRASH)) return false;
                    if (spellOnCooldown(THRASH)) return false;
                    if (!SpellManager.CanCast(THRASH)) return false;
                    if (addCount < 3) return false;
                    if (Me.EnergyPercent < 50) return false;
                    if (result.Count() >= 3) return true;

                }
                catch (Exception e) { Logging.Write(Colors.Violet, "Need Thrash => " + e); }
                return true;
            }
        }
        public static WoWUnit _feralHealingTouchUnit
        {
            get
            {
                if (!IsOverlayed(HEALING_TOUCH_INT)) return null;
                if (Me.HealthPercent < 90) return Me;
                if (partyCount > 0)
                {
                    var result = newPartyMembers.Where(p => p != null
                        && p.IsAlive
                        && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();

                    return result.Count() > 0 ? result.FirstOrDefault() : null;
                }
                return null;
            }
        }
        public static bool needSkullBash(WoWUnit unit)
        {
            try
            {
                if (!Me.IsSafelyFacing(unit)) return false;
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
                if (DateTime.Now < _interruptTimer) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Skull Bash => " + e); }
            return Me.CurrentTarget.IsCasting && Me.CanInterruptCurrentSpellCast;
        }
        public static bool needIncapacitatingRoar(WoWUnit unit)
        {
            try
            {
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
                if (DateTime.Now < _interruptTimer) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Incapacitating Roar => " + e); }
            return Me.CurrentTarget.IsCasting && Me.CanInterruptCurrentSpellCast;
        }
        public static bool needTyphoon(WoWUnit unit)
        {
            try
            {
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
                if (DateTime.Now < _interruptTimer) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Typhoon => " + e); }
            return Me.CurrentTarget.IsCasting && Me.CanInterruptCurrentSpellCast;
        }
        public static bool needMightyBash(WoWUnit unit)
        {
            try
            {
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
                if (DateTime.Now < stunTimer) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Mighty Bash => " + e); }
            return Me.CurrentTarget.IsCasting && !Me.CanInterruptCurrentSpellCast;
        }
        public static bool needWarStomp(WoWUnit unit)
        {
            try
            {
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
                if (DateTime.Now < stunTimer) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need War Stomp => " + e); }
            return Me.CurrentTarget.IsCasting && !Me.CanInterruptCurrentSpellCast;
        }
        public static bool needIncarnation(WoWUnit unit)
        {
            try
            {
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
                if (HKM.cooldownsOn) return true;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need incarnation => " + e); }
            return true;
        }
        public static bool needBerserk(WoWUnit unit)
        {
            try
            {
                if (!needBossRotation(unit)) return false;
                if (HaveHasteBuff) return false;
                if (!Me.IsSafelyFacing(unit)) return false;
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Berserk => " + e); }
            return true;
        }
        public static bool needBerserking(WoWUnit unit)
        {
            try
            {
                if (!needBossRotation(unit)) return false;
                if (HaveHasteBuff) return false;
                if (buffExists(BERSERK, Me)) return false;
                if (!Me.IsSafelyFacing(unit)) return false;
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Need Berserking => " + e); }
            return true;
        }
        public static bool needForceOfNature(WoWUnit unit)
        {
            try
            {
                if (!unit.IsWithinMeleeRange) return false;
                if (!unit.InLineOfSight) return false;
                if (!needBossRotation(unit) && DateTime.Now < fonTimer) return false;
            }
            catch (Exception e) { Logging.Write(Colors.Violet, "Force of Nature => " + e); }
            return true;
        }
        public static bool needTigersFury
        {
            get
            {
                try
                {
                    if (!Me.CurrentTarget.IsWithinMeleeRange) return false;
                    if (!Me.CurrentTarget.InLineOfSight) return false;
                    if (Me.EnergyPercent > 30) return false;
                }
                catch (Exception e) { Logging.Write(Colors.Violet, "Tiger's Fury => " + e); }
                return true;
            }
        }
        public static bool WildChargeConditions(float min, float max)
        {
            return Me.CurrentTarget != null && (Me.CurrentTarget.Distance >= min && Me.CurrentTarget.Distance <= max);
        }
        #endregion

        #region interrupts
        public static DateTime interruptTimer;
        public static DateTime stunTimer;

        public static bool SkullBashConditions(WoWUnit unit)
        {
            if (P.myPrefs.AutoInterrupt
                && unit.IsCasting
                && Me.CanInterruptCurrentSpellCast
                && !spellOnCooldown(SKULL_BASH)
                && DateTime.Now >= interruptTimer)
            {
                interruptTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2500);
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
                && DateTime.Now >= interruptTimer)
            {
                interruptTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2500);
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
                && DateTime.Now >= interruptTimer)
            {
                interruptTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2500);
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
                && DateTime.Now >= stunTimer)
            {
                stunTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2500);
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
                && DateTime.Now >= stunTimer)
            {
                stunTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2500);
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
