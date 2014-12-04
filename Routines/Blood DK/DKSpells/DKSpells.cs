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


#region methods
using Form1 = DeathKnight.GUI.Form1;
using HKM = DeathKnight.Helpers.HotkeyManager;
using S = DeathKnight.DKSpells.DKSpells;
using CL = DeathKnight.Handlers.CombatLogEventArgs;
using EH = DeathKnight.Handlers.EventHandlers;
using L = DeathKnight.Helpers.Logs;
using T = DeathKnight.Helpers.targets;
using U = DeathKnight.Helpers.Unit;
using UI = DeathKnight.Helpers.UseItems;
using P = DeathKnight.DKSettings.DKPrefs;
using M = DeathKnight.Helpers.Movement;
using I = DeathKnight.Helpers.Interrupts;
#endregion

namespace DeathKnight.DKSpells
{
    class DKSpells
    {
        private static  LocalPlayer Me { get { return StyxWoW.Me; } }
        private static int DRcount { get { return Me.DeathRuneCount; } }
        private static int BRcount { get { return Me.BloodRuneCount; } }
        private static int FRcount { get { return Me.FrostRuneCount; } }
        private static int URcount { get { return Me.UnholyRuneCount; } }
        private static uint RPcount { get { return Me.GetCurrentPower(WoWPowerType.RunicPower); } }
        private static int LastSpell = 0;

        //flask id's
        public static int CRYSTAL_OF_INSANITY_ITEM = 86569;
        public static int FLASK_OF_THE_WARM_SUN = 76085;
        public static int FLASK_OF_WINTERS_BITE = 76088;
        public static int FLASK_OF_THE_EARTH = 76087;
        public static int FLASK_OF_SPRING_BLOSSOMS = 76084;
        public static int FLASK_OF_FALING_LEAVES = 76086;

        //flaskbuff id's
        public static int CRYSTAL_OF_INSANITY_BUFF = 127230;
        public static int FLASK_OF_THE_EARTH_BUFF = 105694;
        public static int FLASK_OF_THE_WARM_SUN_BUFF = 105691;
        public static int FLASK_OF_WINTERS_BITE_BUFF = 105696;
        public static int FLASK_OF_FALLING_LEAVES_BUFF = 105693;
        public static int FLASK_OF_SPRING_BLOSSOMS_BUFF = 105689;
        public static int ALCHEMY_FLASK = 75525;
        public static int HEALTHSTONE = 5512;

        public static bool NotHaveFlaskBuff
        {
            get
            {
                return !Me.HasAura(FLASK_OF_FALLING_LEAVES_BUFF)
                    && !Me.HasAura(FLASK_OF_SPRING_BLOSSOMS_BUFF)
                    && !Me.HasAura(FLASK_OF_THE_EARTH_BUFF)
                    && !Me.HasAura(FLASK_OF_THE_WARM_SUN_BUFF)
                    && !Me.HasAura(FLASK_OF_WINTERS_BITE_BUFF);
            }
        }

        //alchemy buffs
        public static bool NotHaveAlchemyBuff
        {
            get
            {
                return !Me.HasAura("Enhanced Agility")
                    && !Me.HasAura("Enhanced Inteleect")
                    && !Me.HasAura("Enhanced Strenght");
            }
        }

        #region spell int's
        public const int
            //talents
            PLAGUE_LEECH = 123693,
            UNHOLY_BLIGHT = 115989,
            LICHBORNE = 49039,
            ANTI_MAGIC_ZONE = 51052,
            DEATHS_ADVANCE = 96268,
            ASPHYXIATE = 108194,
            BLOOD_TAP = 45529,
            DEATH_PACT = 48743,
            DEATH_SIPHON = 108196,
            CONVERSION = 119975,
            GOREFIENDS_GRASP = 108199,
            REMORSELESS_WINTER = 108200,
            DESECRATED_GROUND = 108201,
            DEFILE = 152280,
            BREATH_OF_SINDRAGOSA = 152279,

            //Racials
            WAR_STOMP = 20549,
            BLOOD_FURY = 20572,
            BERSERKING = 26297,
            GIFT_OF_THE_NAARU = 59544,

            //diseases
            FROST_FEVER = 55095,
            BLOOD_PLAGUE = 55078,

            //buffs
            CRIMSON_SCOURGE= 81141,
            BLOOD_CHARGE = 114851,

            //spellbook
            ANTI_MAGIC_SHELL = 48707,
            ARMY_OF_THE_DEATH = 42650,
            BLOODBOIL = 50842,
            BLOOD_PRESENCE = 48263,
            BONE_SHIELD = 49222,
            CHAINS_OF_ICE = 45524,
            CONTROL_UNDEAD = 111673,
            DANCING_RUNEWEAPON = 49028,
            DARK_COMMAND = 56222,
            DARK_SIMULACRUM = 77606,
            DEATH_AND_DECAY = 43265,
            DEATH_COIL = 47541,
            DEATH_GRIP = 49576,
            DEATH_STRIKE = 49998,
            EMPOWER_RUNE_WEAPON = 47568,
            FROST_PRESENCE = 48266,
            HORN_OF_WINTER = 57330,
            ICEBOUND_FORTITUDE = 48792,
            ICY_TOUCH = 45477,
            MIND_FREEZE = 47528,
            OUTBREAK = 77575,
            PATH_OF_FROST = 3714,
            PLAGUE_STRIKE = 45462,
            RAISE_ALLY = 61999,
            RUNE_TAP = 48982,
            SOUL_REAPER = 114866,
            STRANGULATE = 47476,
            UNHOLY_PRESENCE = 48265,
            VAMPIRIC_BLOOD = 55233;
        #endregion

        #region interrupt cd
        public static DateTime nextInterruptAllowed;

        public static void SetNextInterruptAllowed()
        {
            nextInterruptAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2500);
        }
        #endregion

        #region spells
        //pull
        public static Composite castDeathGrip()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(DEATH_GRIP)
                && !U.spellOnCooldown(DEATH_GRIP)
                && SpellManager.CanCast(DEATH_GRIP)
                && LastSpell != DEATH_GRIP
                && Me.CurrentTarget.Distance <= 29
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(DEATH_GRIP);
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(DEATH_GRIP).Name);
                    LastSpell = DEATH_GRIP;
                    SetNextPullSpellAllowed();
                }));
        }
        public static Composite castDarkCommand()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(DARK_COMMAND)
                && !U.spellOnCooldown(DARK_COMMAND)
                && SpellManager.CanCast(DARK_COMMAND)
                && LastSpell != DARK_COMMAND
                && Me.CurrentTarget.Distance <= 29
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(DARK_COMMAND);
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(DARK_COMMAND).Name);
                    LastSpell = DARK_COMMAND;
                    SetNextPullSpellAllowed();
                }));
        }
        public static Composite castDeathCoilPull()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(DEATH_COIL)
                && !U.spellOnCooldown(DEATH_COIL)
                && SpellManager.CanCast(DEATH_COIL)
                && RPcount >= 30
                && LastSpell != DEATH_COIL
                && Me.CurrentTarget.Distance <= 29
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(DEATH_COIL);
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(DEATH_COIL).Name);
                    LastSpell = DEATH_COIL;
                    SetNextPullSpellAllowed();
                }));
        }
        public static Composite castDeathIcyTouchPull()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(ICY_TOUCH)
                && !U.spellOnCooldown(ICY_TOUCH)
                && SpellManager.CanCast(ICY_TOUCH)
                && FRcount >= 1
                && LastSpell != ICY_TOUCH
                && Me.CurrentTarget.Distance <= 29
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(ICY_TOUCH);
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(ICY_TOUCH).Name);
                    LastSpell = ICY_TOUCH;
                    SetNextPullSpellAllowed();
                }));
        }
        //end pull

        public static Composite castRacial()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.Racial != 0
                && SpellManager.HasSpell(myRacial)
                && !U.spellOnCooldown(myRacial)
                && SpellManager.CanCast(myRacial)
                && LastSpell != myRacial
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(myRacial);
                    Logging.Write(Colors.Orange, WoWSpell.FromId(myRacial).Name);
                    LastSpell = myRacial;
                    SetNextSpellAllowed();
                }));
        }
        public static int myRacial
        {
            get
            {
                return P.myPrefs.Racial == 1 ? BLOOD_FURY : BERSERKING;
            }
        }
        public static Composite castBoneShield()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(BONE_SHIELD)
                && !U.spellOnCooldown(BONE_SHIELD)
                && !U.buffExists(BONE_SHIELD, Me)
                && SpellManager.CanCast(BONE_SHIELD)
                && LastSpell != BONE_SHIELD
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(BONE_SHIELD);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(BONE_SHIELD).Name);
                    LastSpell = BONE_SHIELD;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite cancelConversion()
        {
            return new Decorator(ret => gotTarget
                && U.buffExists(CONVERSION, Me)
                && Me.HealthPercent >= P.myPrefs.PercentConversion,
                new Action(ret =>
                {
                    Lua.DoString("RunMacroText(\"/cancelaura Conversion\")");
                    Logging.Write(Colors.Pink, "Removing Conversion Aura");
                }));
        }
        public static Composite castConversion()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(CONVERSION)
                && !U.spellOnCooldown(CONVERSION)
                && !U.buffExists(CONVERSION, Me)
                && Me.HealthPercent < P.myPrefs.PercentConversion
                && SpellManager.CanCast(CONVERSION)
                && LastSpell != CONVERSION
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(CONVERSION);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(CONVERSION).Name);
                    LastSpell = CONVERSION;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite castWarStomp()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.AutoInterrupt
                && SpellManager.HasSpell(WAR_STOMP)
                && !U.spellOnCooldown(WAR_STOMP)
                && (Me.CurrentTarget.IsCasting && !I.ItsTimeToInterrupt)
                && SpellManager.CanCast(WAR_STOMP)
                && LastSpell != WAR_STOMP
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextInterruptAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(WAR_STOMP);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(WAR_STOMP).Name);
                    LastSpell = WAR_STOMP;
                    SetNextInterruptAllowed();
                }));
        }
        public static Composite castMindFreeze()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.AutoInterrupt
                && SpellManager.HasSpell(MIND_FREEZE)
                && !U.spellOnCooldown(MIND_FREEZE)
                && (Me.CurrentTarget.IsCasting && I.ItsTimeToInterrupt)
                && SpellManager.CanCast(MIND_FREEZE)
                && LastSpell != MIND_FREEZE
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextInterruptAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(MIND_FREEZE);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(MIND_FREEZE).Name);
                    LastSpell = MIND_FREEZE;
                    SetNextInterruptAllowed();
                }));
        }
        public static Composite castAxphyxiate()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.AutoInterrupt
                && SpellManager.HasSpell(ASPHYXIATE)
                && !U.spellOnCooldown(ASPHYXIATE)
                && (Me.CurrentTarget.IsCasting && !I.ItsTimeToInterrupt)
                && SpellManager.CanCast(ASPHYXIATE)
                && LastSpell != ASPHYXIATE
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextInterruptAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(ASPHYXIATE);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(ASPHYXIATE).Name);
                    LastSpell = ASPHYXIATE;
                    SetNextInterruptAllowed();
                }));
        }
        public static Composite castStrangulate()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.AutoInterrupt
                && SpellManager.HasSpell(STRANGULATE)
                && !U.spellOnCooldown(STRANGULATE)
                && (Me.CurrentTarget.IsCasting && !I.ItsTimeToInterrupt)
                && SpellManager.CanCast(STRANGULATE)
                && LastSpell != STRANGULATE
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextInterruptAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(STRANGULATE);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(STRANGULATE).Name);
                    LastSpell = STRANGULATE;
                    SetNextInterruptAllowed();
                }));
        }
        public static Composite castPresence()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(myPresence)
                && !U.buffExists(myPresence, Me)
                && SpellManager.CanCast(myPresence)
                && LastSpell != myPresence
                && nextPlagueAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(myPresence);
                    Logging.Write(Colors.BlanchedAlmond, WoWSpell.FromId(myPresence).Name);
                    LastSpell = myPresence;
                    SetNextPlagueAllowed();
                }));
        }
        public static int myPresence
        {
            get
            {
                if (P.myPrefs.Presence == 2) return FROST_PRESENCE;
                else if (P.myPrefs.Presence == 3) return UNHOLY_PRESENCE;
                return BLOOD_PRESENCE;
            }
        }
        public static Composite castHornOfWinter()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(HORN_OF_WINTER)
                && !U.spellOnCooldown(HORN_OF_WINTER)
                && !U.buffExists(HORN_OF_WINTER, Me)
                && SpellManager.CanCast(HORN_OF_WINTER)
                && LastSpell != HORN_OF_WINTER
                && nextPlagueAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(HORN_OF_WINTER);
                    Logging.Write(Colors.Fuchsia, WoWSpell.FromId(HORN_OF_WINTER).Name);
                    LastSpell = HORN_OF_WINTER;
                    SetNextPlagueAllowed();
                }));
        }
        public static Composite castGiftOfTheNaaru()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(GIFT_OF_THE_NAARU)
                && !U.spellOnCooldown(GIFT_OF_THE_NAARU)
                && Me.HealthPercent <= P.myPrefs.PercentNaaru
                && SpellManager.CanCast(GIFT_OF_THE_NAARU)
                && LastSpell != GIFT_OF_THE_NAARU
                && nextPlagueAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(GIFT_OF_THE_NAARU);
                    Logging.Write(Colors.Fuchsia, WoWSpell.FromId(GIFT_OF_THE_NAARU).Name);
                    LastSpell = GIFT_OF_THE_NAARU;
                    SetNextPlagueAllowed();
                }));
        }
        public static Composite castVampiricBlood()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(VAMPIRIC_BLOOD)
                && !U.spellOnCooldown(VAMPIRIC_BLOOD)
                && Me.HealthPercent <= P.myPrefs.PercentVampiric
                && SpellManager.CanCast(VAMPIRIC_BLOOD)
                && LastSpell != VAMPIRIC_BLOOD
                && nextPlagueAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(VAMPIRIC_BLOOD);
                    Logging.Write(Colors.Fuchsia, WoWSpell.FromId(VAMPIRIC_BLOOD).Name);
                    LastSpell = VAMPIRIC_BLOOD;
                    SetNextPlagueAllowed();
                }));
        }
        public static Composite castRuneTap()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(RUNE_TAP)
                && !U.spellOnCooldown(RUNE_TAP)
                && Me.HealthPercent <= P.myPrefs.PercentRuneTap
                && SpellManager.CanCast(RUNE_TAP)
                && BRcount >= 1
                && LastSpell != RUNE_TAP
                && nextPlagueAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(RUNE_TAP);
                    Logging.Write(Colors.Fuchsia, WoWSpell.FromId(RUNE_TAP).Name);
                    LastSpell = RUNE_TAP;
                    SetNextPlagueAllowed();
                }));
        }
        public static Composite castIceboundFortitude()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(ICEBOUND_FORTITUDE)
                && !U.spellOnCooldown(ICEBOUND_FORTITUDE)
                && Me.HealthPercent <= P.myPrefs.PercentFortitude
                && SpellManager.CanCast(ICEBOUND_FORTITUDE)
                && LastSpell != ICEBOUND_FORTITUDE
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextPlagueAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(ICEBOUND_FORTITUDE);
                    Logging.Write(Colors.Fuchsia, WoWSpell.FromId(ICEBOUND_FORTITUDE).Name);
                    LastSpell = ICEBOUND_FORTITUDE;
                    SetNextPlagueAllowed();
                }));
        }
        public static Composite castEmpowerRuneWeapon()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(EMPOWER_RUNE_WEAPON)
                && !U.spellOnCooldown(EMPOWER_RUNE_WEAPON)
                && SpellManager.CanCast(EMPOWER_RUNE_WEAPON)
                && T.IsWoWBoss(Me.CurrentTarget)
                && LastSpell != EMPOWER_RUNE_WEAPON
                && (DRcount + FRcount + URcount + BRcount) == 0
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(EMPOWER_RUNE_WEAPON);
                    Logging.Write(Colors.Red, WoWSpell.FromId(EMPOWER_RUNE_WEAPON).Name);
                    LastSpell = EMPOWER_RUNE_WEAPON;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite castDancingRuneWeapon()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(DANCING_RUNEWEAPON)
                && !U.spellOnCooldown(DANCING_RUNEWEAPON)
                && SpellManager.CanCast(DANCING_RUNEWEAPON)
                && T.IsWoWBoss(Me.CurrentTarget)
                && LastSpell != DANCING_RUNEWEAPON
                && Me.HealthPercent <= P.myPrefs.PercentDancing
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(DANCING_RUNEWEAPON);
                    Logging.Write(Colors.Red, WoWSpell.FromId(DANCING_RUNEWEAPON).Name);
                    LastSpell = DANCING_RUNEWEAPON;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite castPlagueStrike()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(PLAGUE_STRIKE)
                && !U.debuffExists(BLOOD_PLAGUE, Me.CurrentTarget)
                && SpellManager.CanCast(PLAGUE_STRIKE)
                && LastSpell != PLAGUE_STRIKE
                && URcount >= 1
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextPlagueAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(PLAGUE_STRIKE);
                    Logging.Write(Colors.ForestGreen, WoWSpell.FromId(PLAGUE_STRIKE).Name);
                    LastSpell = PLAGUE_STRIKE;
                    SetNextPlagueAllowed();
                }));
        }
        public static Composite castIcyTouch()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(ICY_TOUCH)
                && !U.debuffExists(FROST_FEVER, Me.CurrentTarget)
                && SpellManager.CanCast(ICY_TOUCH)
                && LastSpell != OUTBREAK
                && FRcount >= 1
                && Me.CurrentTarget.Distance <= 29
                && nextPlagueAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(ICY_TOUCH);
                    Logging.Write(Colors.ForestGreen, WoWSpell.FromId(ICY_TOUCH).Name);
                    LastSpell = ICY_TOUCH;
                    SetNextPlagueAllowed();
                }));
        }
        public static Composite castOutbreak()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(OUTBREAK) 
                && !U.spellOnCooldown(OUTBREAK)
                && !U.debuffExists(FROST_FEVER, Me.CurrentTarget)
                && !U.debuffExists(BLOOD_PLAGUE, Me.CurrentTarget)
                && SpellManager.CanCast(OUTBREAK) 
                && LastSpell != OUTBREAK
                && Me.CurrentTarget.Distance <= 29
                && nextPlagueAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(OUTBREAK);
                    Logging.Write(Colors.ForestGreen, WoWSpell.FromId(OUTBREAK).Name);
                    LastSpell = OUTBREAK;
                    SetNextPlagueAllowed();
                }));
        }
        public static Composite castDeathAndDecay()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(DEATH_AND_DECAY)
                && SpellManager.CanCast(DEATH_AND_DECAY)
                && !U.spellOnCooldown(DEATH_AND_DECAY)
                && (T.MeleeAddCount > 1 && !HKM.aoeStop)
                && ((URcount >= 1 || DRcount >= 1) || IsOverlayed(DEATH_AND_DECAY))
                && Me.CurrentTarget.IsWithinMeleeRange
                && !Me.IsMoving
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(DEATH_AND_DECAY);
                    SpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(DEATH_AND_DECAY).Name);
                    LastSpell = DEATH_AND_DECAY;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite castRemorselesWinter()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(REMORSELESS_WINTER)
                && SpellManager.CanCast(REMORSELESS_WINTER)
                && !U.spellOnCooldown(REMORSELESS_WINTER)
                && (T.MeleeAddCount >= 5 && !HKM.aoeStop)
                && Me.CurrentTarget.IsWithinMeleeRange
                && !Me.IsMoving
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(REMORSELESS_WINTER);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(REMORSELESS_WINTER).Name);
                    LastSpell = REMORSELESS_WINTER;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite castBloodBoil()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(BLOODBOIL)
                && SpellManager.CanCast(BLOODBOIL)
                && Me.CurrentTarget.IsWithinMeleeRange
                && (BRcount >= 1 || U.buffExists(CRIMSON_SCOURGE, Me))
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(BLOODBOIL);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BLOODBOIL).Name);
                    LastSpell = BLOODBOIL;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite castDeathStrike()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(DEATH_STRIKE)
                && SpellManager.CanCast(DEATH_STRIKE)
                && Me.CurrentTarget.IsWithinMeleeRange
                && (!U.spellOnCooldown(DEATH_STRIKE) || U.buffExists(CRIMSON_SCOURGE, Me))
                && DRcount >= 2 || (FRcount >=1 && URcount >= 1)
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(DEATH_STRIKE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(DEATH_STRIKE).Name);
                    LastSpell = DEATH_STRIKE;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite castSoulReaper()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(SOUL_REAPER)
                && !U.spellOnCooldown(SOUL_REAPER)
                && Me.CurrentTarget.IsWithinMeleeRange
                && SpellManager.CanCast(SOUL_REAPER)
                && Me.CurrentTarget.HealthPercent < 35
                && BRcount >= 1
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(SOUL_REAPER);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(SOUL_REAPER).Name);
                    LastSpell = SOUL_REAPER;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite castDeathCoil()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(DEATH_COIL)
                && SpellManager.CanCast(DEATH_COIL)
                && Me.CurrentTarget.Distance <= 39
                && RPcount >= 35
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(DEATH_COIL);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(DEATH_COIL).Name);
                    LastSpell = DEATH_COIL;
                    SetNextSpellAllowed();
                }));
        }
        public static Composite castBloodTap()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(BLOOD_TAP)
                && SpellManager.CanCast(BLOOD_TAP)
                && Me.CurrentTarget.IsWithinMeleeRange
                && (U.buffExists(BLOOD_CHARGE, Me)
                && U.buffStackCount(BLOOD_CHARGE, Me) >= 5)
                && (DRcount + URcount + FRcount + BRcount) <= 5
                && nextSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(BLOOD_TAP);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BLOOD_TAP).Name);
                    SetNextSpellAllowed();
                    LastSpell = BLOOD_TAP;
                }));
        }
        #endregion


        #region timers
        public static DateTime nextPlagueAllowed;

        public static void SetNextPlagueAllowed()
        {
            nextPlagueAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 1500);
        }
        public static DateTime nextSpellAllowed;

        public static void SetNextSpellAllowed()
        {
            nextSpellAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 1000);
        }
        public static DateTime nextPullSpellAllowed;

        public static void SetNextPullSpellAllowed()
        {
            nextPullSpellAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2000);
        }
        #endregion

        #region overlayed
        public static bool IsOverlayed(int spellID)
        {
            return Lua.GetReturnVal<bool>("return IsSpellOverlayed(" + spellID + ")", 0);
        }
        #endregion overlayed

        #region gotTarget
        public static bool gotTarget
        {
            get
            {
                return Me.CurrentTarget != null && Me.CurrentTarget.IsAlive && Me.CurrentTarget.Attackable;
            }
        }
        #endregion
    }
}
