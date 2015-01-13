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

using P = DK.DKSettings;
using HKM = DK.DKHotkeyManagers;

namespace DK
{
    public partial class DKMain : CombatRoutine
    {
        public const string
            //common
            BLOODLUST = "Bloodlust",
            HEROISM = "Heroism",
            TIME_WARP = "Time Warp",
            ANCIENT_HYSTERIA = "Ancient Hysteria",
            ENHANCED_AGILITY = "Enhanced Agility",
            ENHANCED_STRENGHT = "Enhanced Strenght",
            ENHANCED_INTELLECT = "Enhanced Intellect",
            GIFT_OF_THE_NAARU = "Gift of the Naaru",

            //DK
            OUTBREAK = "Outbreak",
            BLOOD_BOIL = "Blood Boil",
            PLAGUE_LEECH = "Plague Leech",
            DEATH_STRIKE = "Death Strike",
            DEATH_COIL = "Death Coil",
            SOUL_REAPER = "Soul Reaper",
            BLOOD_TAP = "Blood Tap",
            DEATH_AND_DECAY = "Death and Decay",
            DARK_COMMAND = "Dark Command",
            DEATH_GRIP = "Death Grip",
            BLOOD_PRESENCE = "Blood Presence",
            UNHOLY_PRESENCE = "Unholy Presence",
            FROST_PRESENCE = "Frost Presence",
            ANTI_MAGIC_SHELL = "Anti-Magic Shell",
            ICEBOUND_FORTITUDE = "Icebound Fortitude",
            DANCING_RUNE_WEAPON ="Dancing Rune Weapon",
            ARMY_OF_THE_DEAD = "Army of the Dead",
            BONE_SHIELD = "Bone Shield",
            VAMPIRIC_BLOOD = "Vampiric Blood",
            EMPOWER_RUNE_WEAPON = "Empower Rune Weapon",
            DARK_SIMULACRUM = "Dark Simulacrum",
            RUNE_TAP = "Rune Tap",
            BLOOD_PLAGUE = "Blood Plague",
            FROST_FEVER = "Frost Fever",
            UNHOLY_BLIGHT = "Unholy Blight",
            DEATH_PACT = "Death Pact",
            DEFILE = "Defile",
            CONVERSION = "Conversion",
            HORN_OF_WINTER = "Horn of Winter",
            ICY_TOUCH = "Icy Touch",
            PLAGUE_STRIKE = "Plague Strik",
            REMORSELESS_WINTER = "Remorseless Winter",
            MIND_FREEZE = "Mind Freeze",
            CHAINS_OF_ICE = "Chains of Ice",
            ASPHYXIATE = "Asphyxiate",
            RAISE_ALLY = "Raise Ally",
            EINDE = "The End";


        public const int
            ALCHEMYFLASK_ITEM = 75525,
            CRYSTAL_OF_INSANITY_ITEM = 86569,
            CRYSTAL_OF_INSANITY_BUFF = 127230,
            HEALTHSTONE_ITEM = 5512,
            CRYSTAL_OF_ORALIUS_BUFF = 176151,
            CRYSTAL_OF_ORALIUS_ITEM = 118922,
            MIND_SPIKE_INT = 73510,
            TELAARI_TALBUK_INT = 165803,

            BLOOD_BOIL_INT = 50842,
            BLOOD_CHARGE_INT = 114851,
            GLYPHED_OUTBREAK = 0,
            EIND = 0;

        public static string LSPELLCAST = string.Empty;

        #region runecount
        public static int DeathRuneCount { get { return Me.DeathRuneCount; } }
        public static int BloodRuneCount { get { return Me.BloodRuneCount; } }
        public static int FrostRuneCount { get { return Me.FrostRuneCount; } }
        public static int UnholyRuneCount { get { return Me.UnholyRuneCount; } }
        public static bool ZeroRunes { get { return (Me.BloodRuneCount + Me.FrostRuneCount + Me.UnholyRuneCount + Me.DeathRuneCount) == 0; } }
        #endregion

        #region spellConditions
        public static IEnumerable<WoWPartyMember> WoWPartyMembers { get { return StyxWoW.Me.GroupInfo.RaidMembers.Union(StyxWoW.Me.GroupInfo.PartyMembers).Distinct(); } }
        public static WoWPlayer playerToRes = null;

        public static bool canCastDeathStrike
        {
            get
            {
                if (DeathRuneCount >= 2
                || (UnholyRuneCount >= 1 && FrostRuneCount >= 1)
                || (DeathRuneCount == 1 && UnholyRuneCount >= 1)
                || (DeathRuneCount == 1 && FrostRuneCount >= 1)) return true;
                return false;
            }
        }
        public static bool needResPeople
        {
            get
            {
                if (Me.RunicPowerPercent < 30) return false;
                if (spellOnCooldown(RAISE_ALLY)) return false;
                if (HKM.resTanks)
                {
                    WoWPlayer target = WoWPartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank)
                        && p.ToPlayer().IsDead
                        && p.ToPlayer().Location.Distance(Me.Location) <= 40).Select(p => p.ToPlayer()).FirstOrDefault();
                    if (target != null) playerToRes = target; return true;
                }
                if (HKM.resHealers)
                {
                    WoWPlayer target = WoWPartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Healer)
                        && p.ToPlayer().IsDead
                        && p.ToPlayer().Location.Distance(Me.Location) <= 40).Select(p => p.ToPlayer()).FirstOrDefault();
                    if (target != null) playerToRes = target; return true;
                }
                if (HKM.resDPS)
                {
                    WoWPlayer target = WoWPartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Damage)
                        && p.ToPlayer().IsDead
                        && p.ToPlayer().Location.Distance(Me.Location) <= 40).Select(p => p.ToPlayer()).FirstOrDefault();
                    if (target != null) playerToRes = target; return true;
                }
                return false;
            }
        }

        public static bool needDeathrunesForDeathAndDecay
        {
            get
            {
                if (P.myPrefs.UseDeathAndDecayRunes && DeathRuneCount > P.myPrefs.DeathAndDecayRunes) return true;
                return false;
            }
        }
        public static bool needDeathrunesForDefile
        {
            get
            {
                if (P.myPrefs.UseDefileRunes && DeathRuneCount > P.myPrefs.DefileRunes) return true;
                return false;
            }
        }
        public static bool needDeathAndDecay
        {
            get
            {
                if (SpellManager.HasSpell(DEFILE)) return false;
                if (spellOnCooldown(DEATH_AND_DECAY)) return false;
                if (addCountMelee < P.myPrefs.AddsDeathAndDecay) return false;
                if (UnholyRuneCount >= 1) return true;
                if (needDeathrunesForDeathAndDecay) return true;
                return !spellOnCooldown(DEATH_AND_DECAY) ? true : false;
            }
        }
        public static bool needDefile
        {
            get
            {
                if (!SpellManager.HasSpell(DEFILE)) return false;
                if (spellOnCooldown(DEFILE)) return false;
                if (addCountMelee < P.myPrefs.AddsDefile) return false;
                if (UnholyRuneCount >= 1) return true;
                if (needDeathrunesForDefile) return true;
                return !spellOnCooldown(DEFILE) ? true : false;
            }
        }
        public static bool needPresence
        {
            get
            {
                if (checkPresence == "none") return false;
                PRESENCE = checkPresence;
                return true;
            }
        }
        public static string PRESENCE { get; set; }
        public static string checkPresence
        {
            get
            {
                if (P.myPrefs.Presence == 1 && !Me.HasAura(BLOOD_PRESENCE)) return BLOOD_PRESENCE;
                if (P.myPrefs.Presence == 2 && !Me.HasAura(FROST_PRESENCE)) return FROST_PRESENCE;
                if (P.myPrefs.Presence == 3 && !Me.HasAura(UNHOLY_PRESENCE)) return UNHOLY_PRESENCE;
                return "none";
            }
        }
        public static bool needSoulReaper
        {
            get
            {
                if (Me.CurrentTarget.HealthPercent > 35) return false;
                if (spellOnCooldown(SOUL_REAPER)) return false;
                if (Me.CurrentTarget.HealthPercent < 35 && Me.CurrentTarget.IsWithinMeleeRange) return true;
                return false;
            }
        }
        public static bool needBloodBoil
        {
            get
            {
                if (Me.CurrentTarget.Location.Distance(Me.Location) > 5f) return false;
                if (BloodRuneCount >= 1 && Me.CurrentTarget.HealthPercent >= 35 && Me.CurrentTarget.IsWithinMeleeRange) return true;
                if (IsOverlayed(BLOOD_BOIL_INT) && Me.CurrentTarget.IsWithinMeleeRange) return true;
                return false;
            }
        }

        public static bool needEmpoweredRuneWeapon
        {
            get
            {
                if ((HKM.cooldownsOn || Targets.IsWoWBoss(Me.CurrentTarget)) && ZeroRunes) return true;
                return false;
            }
        }
        public static bool needBloodTap
        {
            get
            {
                if (Me.CurrentTarget != null
                    && DeathRuneCount < 2
                    && DepletedCount < 3
                    && buffExists(BLOOD_CHARGE_INT, Me) && buffStackCount(BLOOD_CHARGE_INT, Me) >= 5) { return true; }
                return false;
            }
        }
        public static bool needPlagueLeech
        {
            get
            {
                if (gotTarget
                && SpellManager.HasSpell(PLAGUE_LEECH)
                && SpellManager.CanCast(PLAGUE_LEECH)
                && Me.CurrentTarget.IsWithinMeleeRange
                && !spellOnCooldown(PLAGUE_LEECH)
                && debuffExists(FROST_FEVER, Me.CurrentTarget)
                && debuffExists(BLOOD_PLAGUE, Me.CurrentTarget)
                && (DeathRuneCount + UnholyRuneCount + FrostRuneCount + BloodRuneCount) < 2)
                {
                    return true;
                }
                return false;
            }
        }
        public static int DepletedCount
        {
            get
            {
                int tel = 0;
                if (BloodRuneCount == 1) tel++;
                if (FrostRuneCount == 1) tel++;
                if (UnholyRuneCount == 1) tel++;
                return tel;
            }
        }
        public static bool needOutbreak
        {
            get
            {
                if(Me.CurrentTarget != null
                    && !debuffExists(BLOOD_PLAGUE, Me.CurrentTarget)
                    && !debuffExists(FROST_FEVER, Me.CurrentTarget)
                    && !spellOnCooldown(OUTBREAK)) { return true; }
                return false;
            }
        }
        public static bool needUnholyBlight
        {
            get
            {
                if (Me.CurrentTarget != null
                    && noDisease.Count() > 1
                    && Me.BloodRuneCount == 0
                    && !spellOnCooldown(UNHOLY_BLIGHT)) return true;
                return false;
            }
        }
        public static bool needConversion
        {
            get
            {
                if (Me.HealthPercent >= 90 && buffExists(CONVERSION, Me))
                {
                    Lua.DoString("RunMacroText(\"/cancelaura Conversion\")");
                    return false;
                }
                if (Me.HealthPercent < 85 && Me.RunicPowerPercent > 30) { return true; }
                return false;
            }
        }
        public static bool Range30
        {
            get { return Me.CurrentTarget != null && Me.CurrentTarget.Distance <= 29; }
        }
        #endregion


        #region trinkets
        public static bool UseTrinket1
        {
            get
            {
                if (P.myPrefs.Trinket1Use) return false;
                if (P.myPrefs.Trinket1 == 1) return false;
                if (P.myPrefs.Trinket1 == 2 && (HKM.cooldownsOn || (Targets.IsWoWBoss(Me.CurrentTarget) && AutoBot))) return true;
                if (P.myPrefs.Trinket1 == 3 && IsCrowdControlledPlayer(Me)) return true;
                if (P.myPrefs.Trinket1 == 4 && Me.EnergyPercent <= P.myPrefs.PercentTrinket1Energy) return true;
                if (P.myPrefs.Trinket1 == 5 && Me.ManaPercent >= P.myPrefs.PercentTrinket1Mana) return true;
                if (P.myPrefs.Trinket1 == 6 && Me.HealthPercent <= P.myPrefs.PercentTrinket1HP) return true;
                return false;
            }
        }
        public static bool UseTrinket2
        {
            get
            {
                if (P.myPrefs.Trinket2Use) return false;
                if (P.myPrefs.Trinket2 == 1) return false;
                if (P.myPrefs.Trinket2 == 2 && (HKM.cooldownsOn || (Targets.IsWoWBoss(Me.CurrentTarget) && AutoBot)))
                if (P.myPrefs.Trinket2 == 3 && IsCrowdControlledPlayer(Me)) return true;
                if (P.myPrefs.Trinket2 == 4 && Me.EnergyPercent <= P.myPrefs.PercentTrinket2Energy) return true;
                if (P.myPrefs.Trinket2 == 5 && Me.ManaPercent >= P.myPrefs.PercentTrinket2Mana) return true;
                if (P.myPrefs.Trinket2 == 6 && Me.HealthPercent <= P.myPrefs.PercentTrinket2HP) return true;
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

        #region buff condtions
       
        public static bool AlchemyFlaskConditions
        {
            get
            {
                return !Me.HasAura(ENHANCED_AGILITY) 
                    && !Me.HasAura(ENHANCED_INTELLECT) 
                    && !Me.HasAura(ENHANCED_STRENGHT) 
                    && !Me.HasAura(CRYSTAL_OF_INSANITY_BUFF)
                    && !Me.HasAura(CRYSTAL_OF_ORALIUS_BUFF);
            }
        }
        public static bool CrystalOfOraliusConditions
        {
            get { return !Me.HasAura(CRYSTAL_OF_ORALIUS_BUFF); }
        }
        public static bool CrystalOfInsanityConditions
        {
            get { return !Me.HasAura(CRYSTAL_OF_ORALIUS_BUFF) && !Me.HasAura(CRYSTAL_OF_INSANITY_BUFF); }
        }
        
        #endregion

        #region interrupts
        public static DateTime interruptTimer;
        
        #endregion

        #region overlayed
        public static bool IsOverlayed(int spellID)
        {
            return Lua.GetReturnVal<bool>("return IsSpellOverlayed(" + spellID + ")", 0);
        }
        #endregion

        
    }
}
