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
using Form1 = Druid.DGUI.Form1;
using HKM = Druid.Helpers.HotkeyManager;
using S = Druid.DSpells.SpellCasts;
using CL = Druid.Handlers.CombatLogEventArgs;
using EH = Druid.Handlers.EventHandlers;
using L = Druid.Helpers.Logs;
using T = Druid.Helpers.targets;
using U = Druid.Helpers.Unit;
using UI = Druid.Helpers.UseItems;
using P = Druid.DSettings.DruidPrefs;
using M = Druid.Helpers.Movement;
using I = Druid.Helpers.Interrupts;
#endregion




namespace Druid.DSpells
{
    public partial class SpellCasts
    {

        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static int LastSpell { get; set; }
        public static bool needAoe
        {
            get
            {
                if (HKM.aoeStop) { return false; }
                if (SpellManager.HasSpell(SWIPE) && T.MeleeAddCount >= 3) { return true; }
                return false;
            }
        }

        #region need rip
        public static bool needRip(WoWUnit unit)
        {
            if(unit.Name.Contains("Dummy")) return true;
            if (unit.MaxHealth > (Me.MaxHealth * 1.5)) return true;
            return false;
        }
        #endregion

        #region balance
        public static Composite castMoonfire()
        {
            return new Decorator(ret => gotTarget 
                && SpellManager.HasSpell("Moonfire")
                && !U.debuffExists("Moonfire", Me.CurrentTarget)
                && Me.CurrentTarget.Distance <= 40
                && LastSpell != MOONFIRE
                && SpellManager.CanCast("Moonfire"),
                new Action(ret =>
                {
                    SpellManager.Cast("Moonfire");
                    Logging.Write(Colors.DodgerBlue, WoWSpell.FromId(MOONFIRE).Name);
                    LastSpell = MOONFIRE;
                }
            ));
        }
        public static Composite castWrath()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(WRATH)
                && Me.CurrentTarget.Distance <= 40
                && SpellManager.CanCast(WRATH),
                new Action(ret =>
                {
                    SpellManager.Cast(WRATH);
                    Logging.Write(Colors.DodgerBlue, WoWSpell.FromId(WRATH).Name);
                    LastSpell = MOONFIRE;
                }
            ));
        }
        #endregion







        #region spells Feral
        public static int spell = 0;
        public static Composite castIncapacitatingRoar()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.AutoInterrupt
                && SpellManager.HasSpell(INCAPACITATING_ROAR)
                && !U.spellOnCooldown(INCAPACITATING_ROAR)
                && (Me.CurrentTarget.IsCasting && I.ItsTimeToInterrupt)
                && SpellManager.CanCast(INCAPACITATING_ROAR)
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextInterruptAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(INCAPACITATING_ROAR);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(INCAPACITATING_ROAR).Name);
                    LastSpell = WAR_STOMP;
                    SetNextInterruptAllowed();
                }));
        }
        public static Composite castTyphoon()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.AutoInterrupt
                && SpellManager.HasSpell(TYPHOON)
                && !U.spellOnCooldown(TYPHOON)
                && (Me.CurrentTarget.IsCasting && I.ItsTimeToInterrupt)
                && SpellManager.CanCast(TYPHOON)
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextInterruptAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(TYPHOON);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(TYPHOON).Name);
                    LastSpell = TYPHOON;
                    SetNextInterruptAllowed();
                }));
        }
        public static Composite castSkullBash()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.AutoInterrupt
                && SpellManager.HasSpell(SKULL_BASH)
                && !U.spellOnCooldown(SKULL_BASH)
                && (Me.CurrentTarget.IsCasting && I.ItsTimeToInterrupt)
                && SpellManager.CanCast(SKULL_BASH)
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextInterruptAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(SKULL_BASH);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(SKULL_BASH).Name);
                    LastSpell = SKULL_BASH;
                    SetNextInterruptAllowed();
                }));
        }
        public static Composite castMightyBash()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.AutoInterrupt
                && SpellManager.HasSpell(MIGHTY_BASH)
                && !U.spellOnCooldown(MIGHTY_BASH)
                && (Me.CurrentTarget.IsCasting && !I.ItsTimeToInterrupt)
                && SpellManager.CanCast(MIGHTY_BASH)
                && Me.CurrentTarget.IsWithinMeleeRange
                && nextInterruptAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(MIGHTY_BASH);
                    Logging.Write(Colors.Pink, WoWSpell.FromId(MIGHTY_BASH).Name);
                    LastSpell = WAR_STOMP;
                    SetNextInterruptAllowed();
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
        public static Composite castMotW()
        {
            return new Decorator(ret => CanBuff
                && SpellManager.HasSpell(MARK_OF_THE_WILD)
                && !Me.HasAura("Prowl")
                && !U.buffExists(MARK_OF_THE_WILD, Me)
                && !Me.HasAura("Blessing of Kings")
                && !Me.HasAura("Legacy of the Emperor")
                && LastSpell != MARK_OF_THE_WILD
                && SpellManager.CanCast(MARK_OF_THE_WILD),
                new Action(ret =>
                {
                    SpellManager.Cast(MARK_OF_THE_WILD, Me);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(MARK_OF_THE_WILD).Name);
                    LastSpell = MARK_OF_THE_WILD;
                }));
        }
        public static Composite castShapeShift()
        {
            return new Decorator(ret => gotTarget && P.myPrefs.AutoShape && U.IsRooted(Me),
                new Action(ret =>
                {
                    if (Me.Shapeshift == ShapeshiftForm.Cat)
                    {
                        Lua.DoString("RunmacroText(\"/cancelaura Cat Form\")");
                        return RunStatus.Success;
                    }
                    if (Me.Shapeshift == ShapeshiftForm.Bear)
                    {
                        Lua.DoString("RunmacroText(\"/cancelaura Bear Form\")");
                        return RunStatus.Success;
                    }
                    if (Me.Shapeshift == ShapeshiftForm.Moonkin)
                    {
                        Lua.DoString("RunmacroText(\"/cancelaura Moonkin Form\")");
                        return RunStatus.Success;
                    }
                    return RunStatus.Failure;
                }));
        }
        public static Composite castProwl()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(PROWL)
                && Me.Specialization == WoWSpec.DruidFeral
                && !U.buffExists(PROWL, Me)
                && !U.spellOnCooldown(PROWL)
                && P.myPrefs.PullPref
                && LastSpell != PROWL
                && SpellManager.CanCast(PROWL),
                new Action(ret =>
                {
                    SpellManager.Cast(PROWL);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(PROWL).Name);
                    LastSpell = spell;
                }));
        }
        public static Composite castFoNBurst()
        {
            return new Decorator(ret => gotTarget
                && SR_UP
                && (T.IsWoWBoss(Me.CurrentTarget)
                || HKM.cooldownsOn)
                && SpellManager.HasSpell(FORCE_OF_NATURE)
                && !U.spellOnCooldown(FORCE_OF_NATURE)
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != FORCE_OF_NATURE
                && SpellManager.CanCast(FORCE_OF_NATURE),
                new Action(ret =>
                {
                    SpellManager.Cast(FORCE_OF_NATURE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(FORCE_OF_NATURE).Name);
                    LastSpell = FORCE_OF_NATURE;
                }));
        }
        public static Composite castFoN()
        {
            return new Decorator(ret => gotTarget
                && SR_UP
                && !T.IsWoWBoss(Me.CurrentTarget)
                && !HKM.cooldownsOn
                && SpellManager.HasSpell(FORCE_OF_NATURE)
                && !U.spellOnCooldown(FORCE_OF_NATURE)
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != FORCE_OF_NATURE
                && SpellManager.CanCast(FORCE_OF_NATURE)
                && nextFoNAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(FORCE_OF_NATURE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(FORCE_OF_NATURE).Name);
                    SetNextFoNAllowed();
                    LastSpell = FORCE_OF_NATURE;
                }));
        }
        public static Composite castIncarnation()
        {
            return new Decorator(ret => gotTarget
                && SR_UP
                && U.buffExists(INCARNATION, Me)
                && SpellManager.HasSpell(INCARNATION)
                && ((T.IsWoWBoss(Me.CurrentTarget) && P.myPrefs.CDIncarnation == 3)
                || (P.myPrefs.CDIncarnation == 2 && HKM.cooldownsOn))
                && !U.spellOnCooldown(INCARNATION)
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != INCARNATION
                && SpellManager.CanCast(INCARNATION),
                new Action(ret =>
                {
                    SpellManager.Cast(INCARNATION);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(INCARNATION).Name);
                    LastSpell = INCARNATION;
                }));
        }
        public static Composite castBerserk()
        {
            return new Decorator(ret => gotTarget
                && SR_UP
                && (U.buffExists(TIGERS_FURY, Me) && U.buffTimeLeft(TIGERS_FURY, Me) > 5000)
                && SpellManager.HasSpell(BERSERK)
                && ((T.IsWoWBoss(Me.CurrentTarget) && P.myPrefs.CDBerserk == 3)
                || (P.myPrefs.CDBerserk == 2 && HKM.cooldownsOn))
                && !U.spellOnCooldown(BERSERK)
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != BERSERK
                && SpellManager.CanCast(BERSERK),
                new Action(ret =>
                {
                    SpellManager.Cast(BERSERK);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BERSERK).Name);
                    LastSpell = BERSERK;
                }));
        }
        public static Composite castBerserking()
        {
            return new Decorator(ret => gotTarget
                && SR_UP
                && SpellManager.HasSpell(BERSERKING)
                && ((T.IsWoWBoss(Me.CurrentTarget) && P.myPrefs.CDBerserking == 3)
                || (P.myPrefs.CDBerserking == 2 && HKM.cooldownsOn))
                && !U.spellOnCooldown(BERSERKING)
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != BERSERKING
                && SpellManager.CanCast(BERSERKING),
                new Action(ret =>
                {
                    SpellManager.Cast(BERSERKING);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BERSERKING).Name);
                    LastSpell = BERSERKING;
                }));
        }
        public static Composite castHotW()
        {
            return new Decorator(ret => gotTarget
                && SR_UP
                && SpellManager.HasSpell(HEART_OF_THE_WILD)
                && ((T.IsWoWBoss(Me.CurrentTarget) && P.myPrefs.CDHeartOfTheWild == 3)
                || (P.myPrefs.CDHeartOfTheWild == 2 && HKM.cooldownsOn))
                && !U.spellOnCooldown(HEART_OF_THE_WILD)
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != HEART_OF_THE_WILD
                && SpellManager.CanCast(HEART_OF_THE_WILD),
                new Action(ret =>
                {
                    SpellManager.Cast(HEART_OF_THE_WILD);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(HEART_OF_THE_WILD).Name);
                    LastSpell = HEART_OF_THE_WILD;
                }));
        }
        public static Composite castCatForm()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(CAT_FORM)
                && Me.Specialization == WoWSpec.DruidFeral
                && Me.Shapeshift != ShapeshiftForm.Cat
                && Me.HealthPercent > P.myPrefs.PercentSwitchBearForm
                && SpellManager.CanCast(CAT_FORM),
                new Action(ret =>
                {
                    SpellManager.Cast(CAT_FORM);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(CAT_FORM).Name);
                    LastSpell = CAT_FORM;
                }));
        }
        public static Composite castThrash()
        {
            return new Decorator(ret => SR_UP
                    && gotTarget
                    && SpellManager.HasSpell(THRASH)
                    && U.buffExists("Clearcasting", Me)
                    && (!SpellManager.HasSpell("Soul of the Forest") || (T.MeleeAddCount > 1 && (Me.EnergyPercent >= 60 || U.buffExists(CLEARCASTING, Me))))
                    && Me.CurrentTarget.IsWithinMeleeRange
                    && LastSpell != THRASH
                    && SpellManager.CanCast(THRASH),
                    new Action(ret =>
                    {
                        SpellManager.Cast(THRASH);
                        Logging.Write(Colors.Yellow, WoWSpell.FromId(THRASH).Name);
                        LastSpell = THRASH;
                    }));
        }
        public static Composite castThrashAoe()
        {
            return new Decorator(ret => SR_UP
                    && gotTarget
                    && SpellManager.HasSpell(THRASH)
                    && (U.buffExists("Clearcasting", Me) || Me.EnergyPercent >= 50)
                    && Me.CurrentTarget.IsWithinMeleeRange
                    && !U.debuffExists(THRASH, Me.CurrentTarget)
                    && SpellManager.CanCast(THRASH),
                    new Action(ret =>
                    {
                        SpellManager.Cast(THRASH);
                        Logging.Write(Colors.Yellow, WoWSpell.FromId(THRASH).Name);
                        LastSpell = THRASH;
                    }));
        }
        public static Composite castSwipe()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(SWIPE)
                && Me.EnergyPercent >= 45
                && Me.CurrentTarget.IsWithinMeleeRange
                && SpellManager.CanCast(SWIPE),
                new Action(ret =>
                {
                    SpellManager.Cast(SWIPE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(SWIPE).Name);
                    LastSpell = SWIPE;
                }));
        }
        public static Composite castTigersFury()
        {
            return new Decorator(ret => gotTarget
                && SR_UP
                && Me.Shapeshift == ShapeshiftForm.Cat
                && SpellManager.HasSpell(TIGERS_FURY)
                && !U.buffExists(TIGERS_FURY, Me)
                && !U.spellOnCooldown(TIGERS_FURY)
                && Me.GetCurrentPower(WoWPowerType.Energy) < 30
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != spell
                && SpellManager.CanCast(TIGERS_FURY),
                new Action(ret =>
                {
                    SpellManager.Cast(TIGERS_FURY);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(TIGERS_FURY).Name);
                    LastSpell = TIGERS_FURY;
                }));
        }
        public static bool SR_UP { get { return U.buffExists("Savage Roar", Me); } }
        public static Composite castSavageRoar()
        {
            return new Decorator(ret => gotTarget
                && Me.Shapeshift == ShapeshiftForm.Cat
                && SpellManager.HasSpell(SAVAGE_ROAR)
                && !Me.HasAura("Savage Roar")
                && Me.EnergyPercent >= 25
                && Me.ComboPoints > 1
                && LastSpell != SAVAGE_ROAR
                && SpellManager.CanCast(SAVAGE_ROAR),
                new Action(ret =>
                {
                    SpellManager.Cast(SAVAGE_ROAR);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(SAVAGE_ROAR).Name);
                    LastSpell = SAVAGE_ROAR;
                }));
        }
        public static Composite castRakePull()
        {
            return new Decorator(ret => gotTarget
                && P.myPrefs.PullPref
                && Me.CurrentTarget.IsWithinMeleeRange
                && SpellManager.HasSpell(RAKE)
                && Me.GetCurrentPower(WoWPowerType.Energy) >= 35
                && LastSpell != RAKE
                && SpellManager.CanCast(RAKE),
                new Action(ret =>
                {
                    SpellManager.Cast(RAKE);
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(RAKE).Name);
                    LastSpell = RAKE;
                }));
        }
        public static Composite castRake()
        {
            return new Decorator(ret => gotTarget
                && SR_UP
                && SpellManager.HasSpell(RAKE)
                && (!U.debuffExists("Rake", Me.CurrentTarget) || (U.debuffExists("Rake", Me.CurrentTarget) && U.debuffTimeLeft("Rake", Me.CurrentTarget) <= 5000))
                && Me.GetCurrentPower(WoWPowerType.Energy) >= 35
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != RAKE
                && SpellManager.CanCast(RAKE),
                new Action(ret =>
                {
                    SpellManager.Cast(RAKE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(RAKE).Name);
                    LastSpell = RAKE;
                }));
        }
        public static Composite castRakeLow()
        {
            return new Decorator(ret => gotTarget
                && !SpellManager.HasSpell(SAVAGE_ROAR)
                && SpellManager.HasSpell(RAKE)
                && (!U.debuffExists("Rake", Me.CurrentTarget) || (U.debuffExists("Rake", Me.CurrentTarget) && U.debuffTimeLeft("Rake", Me.CurrentTarget) <= 5000))
                && Me.GetCurrentPower(WoWPowerType.Energy) >= 35
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != RAKE
                && SpellManager.CanCast(RAKE),
                new Action(ret =>
                {
                    SpellManager.Cast(RAKE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(RAKE).Name);
                    LastSpell = RAKE;
                }));
        }
        public static Composite castShred()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(SHRED)
                && Me.EnergyPercent >= 40
                && Me.CurrentTarget.IsWithinMeleeRange
                && SpellManager.CanCast(SHRED),
                new Action(ret =>
                {
                    SpellManager.Cast(SHRED);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(SHRED).Name);
                    LastSpell = SHRED;
                }));
        }
        public static Composite castRipLow()
        {
            return new Decorator(ret => SpellManager.HasSpell(RIP)
                && !SpellManager.HasSpell(SAVAGE_ROAR)
                    && gotTarget
                    && needRip(Me.CurrentTarget)
                    && (!U.debuffExists(RIP, Me.CurrentTarget)
                    || (U.debuffExists(RIP, Me.CurrentTarget) && U.debuffTimeLeft(RIP, Me.CurrentTarget) <= 5000))
                    && Me.ComboPoints >= 5
                    && Me.EnergyPercent >= 30
                    && Me.CurrentTarget.IsWithinMeleeRange
                    && SpellManager.CanCast(RIP),
                    new Action(ret =>
                    {
                        SpellManager.Cast(RIP);
                        Logging.Write(Colors.Yellow, WoWSpell.FromId(RIP).Name);
                        LastSpell = RIP;
                    }));
        }
        public static Composite castRip()
        {
            return new Decorator(ret => SpellManager.HasSpell(RIP)
                    && gotTarget
                    && needRip(Me.CurrentTarget)
                    && SR_UP
                    && (!U.debuffExists(RIP, Me.CurrentTarget)
                    || (U.debuffExists(RIP, Me.CurrentTarget) && U.debuffTimeLeft(RIP, Me.CurrentTarget) <= 5000))
                    && Me.ComboPoints >= 5
                    && Me.EnergyPercent >= 30
                    && Me.CurrentTarget.IsWithinMeleeRange
                    && SpellManager.CanCast(RIP),
                    new Action(ret =>
                    {
                        SpellManager.Cast(RIP);
                        Logging.Write(Colors.Yellow, WoWSpell.FromId(RIP).Name);
                        LastSpell = RIP;
                    }));
        }
        public static Composite castFBR()
        {
            return new Decorator(ret => gotTarget
                && (SR_UP
                && U.buffTimeLeft("Savage Roar", Me) > 6000)
                && Me.CurrentTarget.IsWithinMeleeRange
                && SpellManager.HasSpell(FEROCIUOS_BITE)
                && ((U.debuffExists("Rip", Me.CurrentTarget) && U.debuffTimeLeft("Rip", Me.CurrentTarget) >= 6000 && Me.ComboPoints >= 5 && Me.EnergyPercent >= 50)
                || (Me.CurrentTarget.HealthPercent < 25
                && (U.debuffExists("Rip", Me.CurrentTarget)
                && U.debuffTimeLeft("Rip", Me.CurrentTarget) <= 5000)
                && Me.ComboPoints >= 1 && Me.EnergyPercent >= 25))
                && SpellManager.CanCast(FEROCIUOS_BITE),
                new Action(ret =>
                {
                    SpellManager.Cast(FEROCIUOS_BITE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(FEROCIUOS_BITE).Name);
                    LastSpell = FEROCIUOS_BITE;
                }));
        }
        public static Composite castFBLow()
        {
            return new Decorator(ret => gotTarget
                && !SpellManager.HasSpell(RIP)
                && SpellManager.HasSpell(FEROCIUOS_BITE)
                && Me.EnergyPercent >= 30
                && Me.ComboPoints >= 3
                && Me.CurrentTarget.IsWithinMeleeRange
                && SpellManager.CanCast(FEROCIUOS_BITE),
                new Action(ret =>
                {
                    SpellManager.Cast(FEROCIUOS_BITE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(FEROCIUOS_BITE).Name);
                    LastSpell = FEROCIUOS_BITE;
                }));
        }
        public static Composite castFB()
        {
            return new Decorator(ret => gotTarget
                && SR_UP
                && !needRip(Me.CurrentTarget)
                && SpellManager.HasSpell(FEROCIUOS_BITE)
                && Me.EnergyPercent >= 30
                && Me.ComboPoints >= 3
                && Me.CurrentTarget.IsWithinMeleeRange
                && SpellManager.CanCast(FEROCIUOS_BITE),
                new Action(ret =>
                {
                    SpellManager.Cast(FEROCIUOS_BITE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(FEROCIUOS_BITE).Name);
                    LastSpell = FEROCIUOS_BITE;
                }));
        }
        #endregion

        #region spells feral bear
        public static Composite castBearForm()
        {
            return new Decorator(ret => gotTarget
                && Me.Specialization == WoWSpec.DruidGuardian
                && SpellManager.HasSpell(BEAR_FORM)
                && Me.Shapeshift != ShapeshiftForm.Bear
                && SpellManager.CanCast(BEAR_FORM),
                new Action(ret =>
                {
                    SpellManager.Cast(BEAR_FORM);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BEAR_FORM).Name);
                    LastSpell = BEAR_FORM;
                }));
        }
        public static Composite castMangle()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(MANGLE)
                && !U.spellOnCooldown(MANGLE)
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != MANGLE
                && SpellManager.CanCast(MANGLE),
                new Action(ret =>
                {
                    SpellManager.Cast(MANGLE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(MANGLE).Name);
                    LastSpell = MANGLE;
                }));
        }
        #endregion

        #region spells guardian

        public static Composite castFaerieSwarm()
        {
            return new Decorator(ret => gotTarget
                && !P.myPrefs.PullPref
                && Me.CurrentTarget.Distance <= 35
                && SpellManager.HasSpell("Faerie Swarm")
                && !U.spellOnCooldown("Faerie Swarm")
                && SpellManager.CanCast("Faerie Swarm")
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast("Faerie Swarm");
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(FAERIE_SWARM).Name);
                    SetNextPullSpellAllowed();
                    LastSpell = FAERIE_SWARM;

                }));
        }
        public static Composite castFaerieSwarmAbove()
        {
            return new Decorator(ret => gotTarget
                && Me.CurrentTarget.Distance <= 35
                && SpellManager.HasSpell("Faerie Swarm")
                && !U.spellOnCooldown("Faerie Swarm")
                && SpellManager.CanCast("Faerie Swarm")
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast("Faerie Swarm");
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(FAERIE_SWARM).Name);
                    SetNextPullSpellAllowed();
                    LastSpell = FAERIE_SWARM;

                }));
        }
        public static Composite castFaerieSwarmBear()
        {
            return new Decorator(ret => gotTarget
                && Me.CurrentTarget.Distance <= 29
                && SpellManager.HasSpell("Faerie Swarm")
                && !U.spellOnCooldown("Faerie Swarm")
                && SpellManager.CanCast("Faerie Swarm")
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast("Faerie Swarm");
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(FAERIE_SWARM).Name);
                    SetNextPullSpellAllowed();
                    LastSpell = FAERIE_SWARM;

                }));
        }
        public static Composite castFaerieFire()
        {
            return new Decorator(ret => gotTarget
                && !P.myPrefs.PullPref
                && Me.CurrentTarget.Distance <= 29
                && SpellManager.HasSpell("Faerie Fire")
                && !SpellManager.HasSpell("Faerie Swarm")
                && !U.spellOnCooldown("Faerie Fire")
                && SpellManager.CanCast("Faerie Fire")
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast("Faerie Fire");
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(FAERIE_FIRE).Name);
                    SetNextPullSpellAllowed();
                    LastSpell = FAERIE_FIRE;
                }));
        }
        public static Composite castFaerieFireAbove()
        {
            return new Decorator(ret => gotTarget
                && Me.CurrentTarget.Distance <= 29
                && SpellManager.HasSpell("Faerie Fire")
                && !SpellManager.HasSpell("Faerie Swarm")
                && !U.spellOnCooldown("Faerie Fire")
                && SpellManager.CanCast("Faerie Fire")
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast("Faerie Fire");
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(FAERIE_FIRE).Name);
                    SetNextPullSpellAllowed();
                    LastSpell = FAERIE_FIRE;
                }));
        }
        public static Composite castFaerieFireBear()
        {
            return new Decorator(ret => gotTarget
                && Me.CurrentTarget.Distance <= 35
                && SpellManager.HasSpell("Faerie Fire")
                && !SpellManager.HasSpell("Faerie Swarm")
                && !U.spellOnCooldown("Faerie Fire")
                && SpellManager.CanCast("Faerie Fire")
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast("Faerie Fire");
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(FAERIE_FIRE).Name);
                    SetNextPullSpellAllowed();
                    LastSpell = FAERIE_FIRE;
                }));
        }
        public static Composite castGrowl()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(GROWL)
                && !U.spellOnCooldown(GROWL)
                && Me.CurrentTarget.Distance <= 29
                && LastSpell != GROWL
                && SpellManager.CanCast(GROWL)
                && nextPullSpellAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(GROWL);
                    Logging.Write(Colors.LightBlue, WoWSpell.FromId(GROWL).Name);
                    SetNextPullSpellAllowed();
                    LastSpell = GROWL;
                }));
        }
        public static Composite castHealingTouchGuardian()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(HEALING_TOUCH)
                && IsOverlayed(HEALING_TOUCH)
                && Me.HealthPercent <= 95
                && LastSpell != HEALING_TOUCH
                && SpellManager.CanCast(HEALING_TOUCH),
                new Action(ret =>
                {
                    SpellManager.Cast(HEALING_TOUCH);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(HEALING_TOUCH).Name);
                    LastSpell = HEALING_TOUCH;
                }));
        }
        public static Composite castBerserkGuardian()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell("Berserk")
                && !U.spellOnCooldown("Berserk")
                && Me.CurrentTarget.IsWithinMeleeRange
                && ((P.myPrefs.CDBerserk == 3 && T.IsWoWBoss(Me.CurrentTarget)) || (P.myPrefs.CDBerserk == 2 && HKM.cooldownsOn))
                && LastSpell != BBERSERK
                && SpellManager.CanCast("Berserk"),
                new Action(ret =>
                {
                    SpellManager.Cast(MAUL);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BBERSERK).Name);
                    LastSpell = BBERSERK;

                }));
        }
        public static Composite castIncarnationGuardian()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(BINCARNATION)
                && !U.spellOnCooldown(BINCARNATION)
                && Me.CurrentTarget.IsWithinMeleeRange
                && ((P.myPrefs.CDIncarnation == 3 && T.IsWoWBoss(Me.CurrentTarget)) || (P.myPrefs.CDIncarnation == 2 && HKM.cooldownsOn))
                && LastSpell != BINCARNATION
                && SpellManager.CanCast(BINCARNATION),
                new Action(ret =>
                {
                    SpellManager.Cast(BINCARNATION);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BINCARNATION).Name);
                    LastSpell = BINCARNATION;

                }));
        }
        public static Composite castHotWGuardian()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(HEART_OF_THE_WILD)
                && !U.spellOnCooldown(HEART_OF_THE_WILD)
                && Me.CurrentTarget.IsWithinMeleeRange
                && ((P.myPrefs.CDHeartOfTheWild == 3 && T.IsWoWBoss(Me.CurrentTarget)) || (P.myPrefs.CDHeartOfTheWild == 2 && HKM.cooldownsOn))
                && LastSpell != HEART_OF_THE_WILD
                && SpellManager.CanCast(HEART_OF_THE_WILD),
                new Action(ret =>
                {
                    SpellManager.Cast(HEART_OF_THE_WILD);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(HEART_OF_THE_WILD).Name);
                    LastSpell = HEART_OF_THE_WILD;

                }));
        }
        public static Composite castMangleGuardian()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(MANGLE)
                && !U.spellOnCooldown(MANGLE)
                && Me.CurrentTarget.IsWithinMeleeRange
                && LastSpell != MANGLE
                && SpellManager.CanCast(MANGLE),
                new Action(ret =>
                {
                    SpellManager.Cast(MANGLE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(MANGLE).Name);
                    LastSpell = MANGLE;
                }));
        }
        public static Composite castMaul()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(MAUL)
                && !U.spellOnCooldown(MAUL)
                && Me.CurrentTarget.IsWithinMeleeRange
                && (Me.GetCurrentPower(WoWPowerType.Rage) >= 35 && U.buffExists("Tooth and Claw", Me))
                && LastSpell != MAUL
                && SpellManager.CanCast(MAUL),
                new Action(ret =>
                {
                    SpellManager.Cast(MAUL);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(MAUL).Name);
                    LastSpell = MAUL;
                }));
        }
        public static Composite castThrashBear()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(BTHRASH)
                && Me.CurrentTarget.IsWithinMeleeRange
                && SpellManager.CanCast(BTHRASH),
                new Action(ret =>
                {
                    SpellManager.Cast(BTHRASH);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BTHRASH).Name);
                    LastSpell = BTHRASH;
                }));
        }
        public static Composite castLacerate()
        {
            return new Decorator(ret => gotTarget
                && SpellManager.HasSpell(LACERATE)
                && Me.CurrentTarget.IsWithinMeleeRange
                && U.debuffStackCount("Lacerate", Me.CurrentTarget) < 3
                && SpellManager.CanCast(LACERATE),
                new Action(ret =>
                {
                    SpellManager.Cast(LACERATE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(LACERATE).Name);
                    LastSpell = LACERATE;
                }));
        }
        #endregion

        #region protection
        public static Composite castSwitchBearform()
        {
            return new Decorator(ret => gotTarget
                && CanBuff
                && SpellManager.HasSpell(BEAR_FORM)
                && Me.Specialization == WoWSpec.DruidFeral
                && Me.Shapeshift != ShapeshiftForm.Bear
                && (Me.HealthPercent <= P.myPrefs.PercentSwitchBearForm || HKM.switchBearform)
                && LastSpell != BEAR_FORM
                && SpellManager.CanCast(BEAR_FORM),
                new Action(ret =>
                {
                    SpellManager.Cast(BEAR_FORM);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BEAR_FORM).Name);
                    LastSpell = BEAR_FORM;

                }));
        }
        public static Composite castCenarionWard()
        {
            return new Decorator(ret => gotTarget
                && CanBuff
                && SpellManager.HasSpell(CENARION_WARD)
                && !U.spellOnCooldown(CENARION_WARD)
                && Me.HealthPercent <= P.myPrefs.PercentCenarionWard
                && LastSpell != CENARION_WARD
                && SpellManager.CanCast(CENARION_WARD)
                && nextBuffAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(CENARION_WARD);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(CENARION_WARD).Name);
                    SetNextBuffAllowed();
                    LastSpell = CENARION_WARD;
                }));
        }
        public static Composite castSurvInt()
        {
            return new Decorator(ret => gotTarget
                && CanBuff
                && SpellManager.HasSpell(SURVIVAL_INSTINCTS)
                && !U.spellOnCooldown(SURVIVAL_INSTINCTS)
                && Me.HealthPercent <= P.myPrefs.PercentSurvivalInstincts
                && LastSpell != SURVIVAL_INSTINCTS
                && SpellManager.CanCast(SURVIVAL_INSTINCTS)
                && nextBuffAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(SURVIVAL_INSTINCTS);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(SURVIVAL_INSTINCTS).Name);
                    SetNextBuffAllowed();
                    LastSpell = SURVIVAL_INSTINCTS;
                }));
        }
        public static Composite castBarkskin()
        {
            return new Decorator(ret => gotTarget
                && CanBuff
                && SpellManager.HasSpell(BARKSKIN)
                && !U.spellOnCooldown(BARKSKIN)
                && Me.HealthPercent <= P.myPrefs.PercentBarkskin
                && LastSpell != BARKSKIN
                && SpellManager.CanCast(BARKSKIN)
                && nextBuffAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(BARKSKIN);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(BARKSKIN).Name);
                    SetNextBuffAllowed();
                    LastSpell = BARKSKIN;
                }));
        }
        public static Composite castFrenziedReg()
        {
            return new Decorator(ret => gotTarget
                && CanBuff
                && SpellManager.HasSpell(FRENZIED_REGENERATION)
                && !U.spellOnCooldown(FRENZIED_REGENERATION)
                && Me.GetCurrentPower(WoWPowerType.Rage) >= 60
                && Me.HealthPercent <= P.myPrefs.PercentFrenziedRegeneration
                && LastSpell != FRENZIED_REGENERATION
                && SpellManager.CanCast(FRENZIED_REGENERATION)
                && nextBuffAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(FRENZIED_REGENERATION);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(FRENZIED_REGENERATION).Name);
                    SetNextBuffAllowed();
                    LastSpell = FRENZIED_REGENERATION;
                }));
        }
        public static Composite castSavageDefense()
        {
            return new Decorator(ret => gotTarget
                && CanBuff
                && SpellManager.HasSpell(SAVAGE_DEFENSE)
                && !U.spellOnCooldown(SAVAGE_DEFENSE)
                && Me.HealthPercent <= P.myPrefs.PercentSavageDefense
                && Me.GetCurrentPower(WoWPowerType.Rage) >= 60
                && LastSpell != SAVAGE_DEFENSE
                && SpellManager.CanCast(SAVAGE_DEFENSE)
                && nextSaDefenseAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(SAVAGE_DEFENSE);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(SAVAGE_DEFENSE).Name);
                    SetNextSaDefenseAllowed();
                    LastSpell = SAVAGE_DEFENSE;
                }));
        }
        public static Composite castRejuvenation()
        {
            return new Decorator(ret => gotTarget
                && CanBuff
                && SpellManager.HasSpell(REJUVENATION)
                && !U.buffExists(REJUVENATION, Me)
                && Me.HealthPercent <= P.myPrefs.PercentRejuCombat
                && LastSpell != REJUVENATION
                && SpellManager.CanCast(REJUVENATION)
                && nextBuffAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(REJUVENATION);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(REJUVENATION).Name);
                    SetNextBuffAllowed();
                    LastSpell = REJUVENATION;
                }));
        }
        public static Composite castHealingTouchMe()
        {
            return new Decorator(ret => gotTarget
                && CanBuff
                && SpellManager.HasSpell(HEALING_TOUCH)
                && (!MeInGroup || !P.myPrefs.PredatoryHealOthers)
                && Me.HealthPercent <= P.myPrefs.PercentPredatoryHealOthers
                && U.buffExists("Predatory Swiftness", Me)
                && LastSpell != HEALING_TOUCH
                && SpellManager.CanCast(HEALING_TOUCH)
                && nextBuffAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(HEALING_TOUCH, Me);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(HEALING_TOUCH).Name);
                    SetNextBuffAllowed();
                    LastSpell = HEALING_TOUCH;
                }));
        }
        public static Composite castHealingTouchOthers()
        {
            return new Decorator(ret => gotTarget
                && CanBuff
                && P.myPrefs.PredatoryHealOthers
                && MeInGroup
                && SpellManager.HasSpell(HEALING_TOUCH)
                && Me.HealthPercent > P.myPrefs.PercentPredatoryHealOthers
                && T.HealTarget != null
                && U.buffExists("Predatory Swiftness", Me)
                && LastSpell != HEALING_TOUCH
                && SpellManager.CanCast(HEALING_TOUCH)
                && nextBuffAllowed <= DateTime.Now,
                new Action(ret =>
                {
                    SpellManager.Cast(HEALING_TOUCH, T.HealTarget);
                    Logging.Write(Colors.Yellow, WoWSpell.FromId(HEALING_TOUCH).Name);
                    SetNextBuffAllowed();
                    LastSpell = HEALING_TOUCH;
                }));
        }
        #endregion

        #region flasks
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
        #endregion

        #region timers
        public static DateTime SlowDown;
        public static void SetSlowDown()
        {
            SlowDown = DateTime.Now + new TimeSpan(0, 0, 0, 0, 750);
        }

        public static DateTime nextFoNAllowed;
        public static void SetNextFoNAllowed()
        {
            nextFoNAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 15000);
        }

        public static DateTime nextPullSpellAllowed;
        public static void SetNextPullSpellAllowed()
        {
            nextPullSpellAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2000);
        }

        public static DateTime nextBuffAllowed;
        public static void SetNextBuffAllowed()
        {
            nextBuffAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2000);
        }

        public static DateTime nextSaDefenseAllowed;
        public static void SetNextSaDefenseAllowed()
        {
            nextSaDefenseAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 6000);
        }
        public static DateTime nextInterruptAllowed;
        public static void SetNextInterruptAllowed()
        {
            nextInterruptAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2500);
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

        #region framelock
        public class FrameLockSelector : PrioritySelector
        {
            public FrameLockSelector(params Composite[] children)
                : base(children)
            {
                /*empty*/
            }


            public FrameLockSelector(ContextChangeHandler contextChange, params Composite[] children)
                : base(contextChange, children)
            {
                /*empty*/
            }


            public override RunStatus Tick(object context)
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    return base.Tick(context);
                }
            }
        }
        #endregion

        #region IsDummy
        public static bool IsDummy
        {
            get { return Me.CurrentTarget.Name.Contains("Dummy"); }
        }
        #endregion

        #region can buff
        public static bool CanBuff
        {
            get
            {
                return !Me.Mounted && !Me.IsFlying && !Me.OnTaxi && !Me.IsDead && !Me.IsGhost && !Me.IsCasting;
            }
        }
        #endregion

        #region solo
        public static bool MeInGroup
        {
            get
            {
                return Me.GroupInfo.IsInParty;
            }
        }
        #endregion

        #region autoattack
        public static Composite AutoAttack()
        {
            return new Action(ret =>
            {
                Lua.DoString("StartAttack()");
                return RunStatus.Failure;
            });
        }
        #endregion
    }
}
