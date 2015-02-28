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
        public static string lstSpell = "";
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
            PLAGUE_STRIKE = "Plague Strike",
            REMORSELESS_WINTER = "Remorseless Winter",
            MIND_FREEZE = "Mind Freeze",
            CHAINS_OF_ICE = "Chains of Ice",
            ASPHYXIATE = "Asphyxiate",

            RAISE_ALLY = "Raise Ally",

            GOREFIEND_GRASP = "Gorefiend's Grasp",
            STRANGULATE = "Strangulate",
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
        
        public static WoWPlayer TankToRes
        {
            get
            {
                var list = new List<WoWPlayer>();
                list = WoWPartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank)
                    && p.ToPlayer().IsDead
                    && p.ToPlayer().Distance <= 40).Select(p => p.ToPlayer()).ToList();
                return list.Count() > 0 ? list.FirstOrDefault() : null;
            }
        }
        public static WoWPlayer HealerToRes
        {
            get
            {
                var list = new List<WoWPlayer>();
                list = WoWPartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Healer)
                    && p.ToPlayer().IsDead
                    && p.ToPlayer().Distance <= 40).Select(p => p.ToPlayer()).ToList();
                return list.Count() > 0 ? list.FirstOrDefault() : null;
            }
        }
        public static WoWPlayer DpsToRes
        {
            get
            {
                var list = new List<WoWPlayer>();
                list = WoWPartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Damage)
                    && p.ToPlayer().IsDead
                    && p.ToPlayer().Distance <= 40).Select(p => p.ToPlayer()).ToList();
                return list.Count() > 0 ? list.FirstOrDefault() : null;
            }
        }

        public static bool needDeathAndDecay
        {
            get
            {
                if (SpellManager.HasSpell(DEFILE)) return false;
                if (addCountMelee < P.myPrefs.AddsDeathAndDecay) return false;
                if (UnholyRuneCount >= 1) return true;
                return false;
            }
        }
        public static bool needDefile
        {
            get
            {
                if (addCountMelee < P.myPrefs.AddsDefile) return false;
                if (UnholyRuneCount >= 1) return true;
                return false;
            }
        }
        public static string PRESENCE 
        {
            get
            {
                return P.myPrefs.Presence.ToString().Trim() + " Presence";
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
                if (BloodRuneCount >= 1 && Me.CurrentTarget.HealthPercent >= 35 && Me.CurrentTarget.IsWithinMeleeRange) return true;
                if (IsOverlayed(BLOOD_BOIL_INT) && Me.CurrentTarget.IsWithinMeleeRange) return true;
                return false;
            }
        }

        public static bool needEmpoweredRuneWeapon
        {
            get
            {
                if ((HKM.cooldownsOn || Me.CurrentTarget.IsBoss) && ZeroRunes) return true;
                return false;
            }
        }
        public static bool needBloodTap
        {
            get
            {
                if (buffExists(BLOOD_CHARGE_INT, Me) && buffStackCount(BLOOD_CHARGE_INT, Me) >= 12) { return true; }
                if (Me.CurrentTarget != null
                    && DeathRuneCount < 2
                    && DepletedCount < 3
                    && Me.HealthPercent < P.myPrefs.BloodTapHP
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
                if (Me.HealthPercent < P.myPrefs.Conversion && Me.RunicPowerPercent > 30) { return true; }
                return false;
            }
        }
        public static bool Range30
        {
            get { return Me.CurrentTarget != null && Me.CurrentTarget.Distance <= 29; }
        }
        #endregion


        
        private static bool CanUseEquippedItem(WoWItem item)
        {
            string itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;
            return item.Usable && item.Cooldown <= 0;
        }

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

        #region use items

        #endregion


    }
}
