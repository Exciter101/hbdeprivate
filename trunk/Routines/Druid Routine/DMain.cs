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
using PR = Druid.DSettings.RestoSettings;
using M = Druid.Helpers.Movement;
using I = Druid.Helpers.Interrupts;
using RF = Druid.DRotations.Feral;
using FRB = Druid.DRotations.FeralBear;
using FRG = Druid.DRotations.Guardian;
using BAL = Druid.DRotations.Balance;
using SVN = Druid.DHelpers.svnCheck;
using RESTO = Druid.DRotations.Resto;
#endregion

namespace Druid
{
    class DMain
    {
        public class Main : CombatRoutine
        {
            public override string Name { get { return "Druid Routine by Pasterke"; } }
            public override WoWClass Class { get { return WoWClass.Druid; } }

            public static LocalPlayer Me { get { return StyxWoW.Me; } }

            public override Composite CombatBehavior { get { return Combat(); } }
            public override Composite PreCombatBuffBehavior { get { return PreCombatBuffs(); } }
            public override Composite CombatBuffBehavior { get { return CombatBuffs(); } }
            public override Composite PullBehavior { get { return Pull(); } }
            public override Composite PullBuffBehavior { get { return PullBuffs(); } }

            public string usedBot { get { return BotManager.Current.Name.ToUpper(); } }

            private static bool IsInGame
            {
                get { return StyxWoW.IsInGame; }
            }
            public Stopwatch mountTimer = new Stopwatch();
            public override bool WantButton { get { return true; } }
            public override void OnButtonPress()
            {
                new Form1().ShowDialog();
            }
            public override void Initialize()
            {
                
                Logging.Write("\r\n" + "-- Hello {0}", Me.Name);
                Logging.Write("-- Thanks for using");
                Logging.Write("-- The Druid Combat Routine");
                Logging.Write("-- by Pasterke" + "\r\n");
                HKM.registerHotKeys();
                if (P.myPrefs.AutoSVN)
                {
                    SVN.CheckForUpdate();
                }
                Lua.Events.AttachEvent("UI_ERROR_MESSAGE", CL.CombatLogErrorHandler);
                EH.AttachCombatLogEvent();

            }
            public override void ShutDown()
            {
                HKM.removeHotkeys();
                EH.DetachCombatLogEvent();
                Lua.Events.DetachEvent("UI_ERROR_MESSAGE", CL.CombatLogErrorHandler);
            }

            #region Pulse
            public override void Pulse()
            {
                if (!StyxWoW.IsInWorld || Me == null || !Me.IsValid || Me.IsGhost)
                {
                    return;
                }
                if (Me.IsDead
                    && AutoBot)
                {
                    Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/script RepopMe()"));
                }

                if (AutoBot && S.gotTarget && Me.CurrentMap.IsBattleground && !Me.Combat) { T.CheckBGTarget(); }
                if (AutoBot && S.gotTarget && !Me.CurrentMap.IsBattleground && !Me.Combat) { T.CheckTarget(); }
            }
            #endregion

            public override bool NeedRest
            {
                get
                {
                    return CanBuff
                        && !Me.Combat
                        && !HKM.pauseRoutineOn
                        && !HKM.manualOn
                        && !Me.GroupInfo.IsInRaid
                        && (Me.HealthPercent <= P.myPrefs.FoodHPOoC
                        || Me.ManaPercent <= P.myPrefs.FoodManaOoC
                        || Me.HealthPercent <= P.myPrefs.PercentHealingTouchOoC
                        || Me.HealthPercent <= P.myPrefs.PercentRejuOoC);
                }
            }

            public override void Rest()
            {
                if (CanBuff
                    && !Me.IsSwimming
                    && Me.HealthPercent <= P.myPrefs.FoodHPOoC)
                {
                    Styx.CommonBot.Rest.Feed();
                }
                if (CanBuff
                    && !Me.IsSwimming
                    && Me.ManaPercent <= P.myPrefs.FoodManaOoC)
                {
                    Styx.CommonBot.Rest.Feed();
                }
                if (CanBuff
                    && ((Me.HealthPercent > P.myPrefs.FoodHPOoC && Me.HealthPercent > P.myPrefs.PercentHealingTouchOoC) && Me.HealthPercent <= P.myPrefs.PercentRejuOoC)
                    && !Me.HasAura("Rejuvenation")
                    && !Me.HasAura("Food")
                    && !Me.HasAura("Drink")
                    && SpellManager.CanCast(S.REJUVENATION))
                {
                    SpellManager.Cast(S.REJUVENATION, Me);
                    Logging.Write(Colors.BlanchedAlmond, "Rejuvenation OoC");
                    SetNextHealAllowed();
                }
                if (CanBuff
                    && (Me.HealthPercent > P.myPrefs.FoodHPOoC && Me.HealthPercent < P.myPrefs.PercentHealingTouchOoC)
                    && !Me.HasAura("Food")
                    && !Me.HasAura("Drink")
                    && SpellManager.CanCast(S.HEALING_TOUCH)
                    && healingTouchTime <= DateTime.Now)
                {
                    SpellManager.Cast(S.HEALING_TOUCH, Me);
                    Logging.Write(Colors.BlanchedAlmond, "Healing Touch OoC");
                    healingTouchTime.AddMilliseconds(2000);
                }
            }

            Composite PreCombatBuffs()
            {
                return new PrioritySelector(
                    new Decorator(ret => HKM.pauseRoutineOn, Hold()),
                    S.castMotW(),
                    new Decorator(ret => CanUseCrystal, UI.useMyItems(crystal)),
                    new Decorator(ret => CanUseAlchemyFlask, UI.useMyItems(AlchemyFlask)),
                    new Decorator(ret => !Me.HasAura("Prowl"), S.castSavageRoar())
                    );
            }

            Composite PullBuffs()
            {
                return new PrioritySelector(
                    new Decorator(ret => HKM.pauseRoutineOn || NeedRest, Hold()),
                    S.castCatForm(),
                    S.castBearForm(),
                    S.castProwl()
                    );
            }

            Composite Pull()
            {
                return new PrioritySelector(
                    new Decorator(ret => HKM.pauseRoutineOn || HKM.manualOn, Hold()),
                    new Decorator(ret => Me.CurrentTarget == null && M.AllowTargeting, T.FindTarget()),
                    new Decorator(ret => S.gotTarget
                        && M.AllowTargeting
                        && Me.CurrentTarget.Distance > 40
                        && T.RangeAddCount >= 1, T.FindTarget()),
                    new Decorator(ret => S.gotTarget
                        && M.AllowTargeting
                        && Me.CurrentTarget.Distance > 10
                        && T.MeleeAttackersCount >= 1, T.FindTarget()),
                    new Decorator(ret => Me.CurrentTarget != null
                        && AutoBot
                        && (Me.CurrentTarget.IsFriendly || Me.CurrentTarget.IsDead), new Action(ret => { Me.ClearTarget(); return RunStatus.Failure; })),
                    new Decorator(ret => Me.GotTarget && IsBalance && M.AllowMovement && Me.CurrentTarget.Distance > 39, M.CreateRangeMovement()),
                    new Decorator(ret => Me.GotTarget && IsBalance && M.AllowMovement && Me.CurrentTarget.Distance <= 39, M.CreateStopRangeMovement()),
                    new Decorator(ret => Me.GotTarget && (IsFeral || IsGuardian) && M.AllowMovement && Me.CurrentTarget.Distance > 4f, M.CreateMeleeMovement()),
                    new Decorator(ret => Me.GotTarget && (IsFeral || IsGuardian) && M.AllowMovement && Me.CurrentTarget.Distance <= 4f, M.CreateStopMeleeMovement()),
                    new Decorator(ret => Me.GotTarget && M.AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving, M.FacingTarget()),
                    new Decorator(ret => S.gotTarget && IsGuardian,
                        new S.FrameLockSelector(
                            S.castBearForm(),
                            S.castGrowl(),
                            S.castFaerieFireBear(),
                            S.castFaerieSwarmBear())),
                    new Decorator(ret => S.gotTarget && IsFeral, RF.feralPull()),
                    new Decorator(ret => S.gotTarget && IsBalance,
                        new PrioritySelector(
                            S.castMoonfire(),
                            S.castWrath()
                        ))
                 );
            }
            Composite CombatBuffs()
            {
                return new PrioritySelector(
                    new Decorator(ret => HKM.pauseRoutineOn, Hold()),
                    new Decorator(ret => CanBuff
                        && S.NotHaveFlaskBuff
                        && canUseRaidFlask
                        && UI.HasItem(myRaidFlask), UI.useMyItems(myRaidFlask)),
                    new Decorator(ret => CanUseCrystal, UI.useMyItems(crystal)),
                    new Decorator(ret => CanUseAlchemyFlask, UI.useMyItems(AlchemyFlask)),
                    new Decorator(ret => CanBuff && Me.HealthPercent <= P.myPrefs.PercentHealthstone && UI.HasItem(healthstone), UI.useMyItems(healthstone))
                    );
            }
            Composite Combat()
            {
                return new PrioritySelector(
                    new Decorator(ret => ((Me.Mounted && !AutoBot) || HKM.pauseRoutineOn || HKM.manualOn), Hold()),
                    new Decorator(ret => Me.CurrentTarget == null && M.AllowTargeting, T.FindTarget()),
                    new Decorator(ret => S.gotTarget
                        && M.AllowTargeting
                        && Me.CurrentTarget.Distance > 10
                        && T.MeleeAttackersCount >= 1, T.FindTarget()),
                    new Decorator(ret => Me.CurrentTarget != null
                        && AutoBot
                        && (Me.CurrentTarget.IsFriendly || Me.CurrentTarget.IsDead), new Action(ret => { Me.ClearTarget(); return RunStatus.Failure; })),
                    new Decorator(ret => Me.GotTarget && IsBalance && M.AllowMovement && Me.CurrentTarget.Distance > 39, M.CreateRangeMovement()),
                    new Decorator(ret => Me.GotTarget && IsBalance && M.AllowMovement && Me.CurrentTarget.Distance <= 39, M.CreateStopRangeMovement()),
                    new Decorator(ret => Me.GotTarget && (IsFeral || IsGuardian) && M.AllowMovement && Me.CurrentTarget.Distance > 3.5f, M.CreateMeleeMovement()),
                    new Decorator(ret => Me.GotTarget && (IsFeral || IsGuardian) && M.AllowMovement && Me.CurrentTarget.Distance <= 3.5f, M.CreateStopMeleeMovement()),
                    new Decorator(ret => Me.GotTarget && M.AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving, M.FacingTarget()),
                    //S.castSurvInt(),
                    //S.castBarkskin(),
                    //S.castFrenziedReg(),
                    //S.castSavageDefense(),
                    //S.castCenarionWard(),
                    new Decorator(ret => Me.Specialization == WoWSpec.DruidRestoration, RESTO.HealRotation()),
                    new Decorator(ret => S.gotTarget && IsBalance, BAL.BalanceRot()),
                    new Decorator(ret => S.gotTarget && IsGuardian, FRG.GuardianRot()),
                    new Decorator(ret => S.gotTarget && IsFeral && Me.Shapeshift != ShapeshiftForm.Bear, RF.feral2rot()),
                    new Decorator(ret => S.gotTarget && IsFeral && Me.Shapeshift == ShapeshiftForm.Bear, FRB.FeralBearRot())
                    );
            }







            #region autoattack
            Composite AutoAttack()
            {
                return new Action(ret =>
                {
                    Lua.DoString("StartAttack()");
                    return RunStatus.Failure;
                });
            }
            #endregion

            #region AutoBot
            bool AutoBot
            {
                get
                {
                    return usedBot.Contains("QUEST") || usedBot.Contains("GRIND") || usedBot.Contains("GATHER") || usedBot.Contains("ANGLER") || usedBot.Contains("ARCHEO");

                }
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

            #region Hold
            Composite Hold()
            {
                return new Action(ret =>
                {
                    return;
                });
            }
            #endregion

            #region use flasks, healthstone and others

            public static int myRaidFlask = P.myPrefs.RaidFlaskKind;

            public const int
                crystal = 86569,
                AlchemyFlask = 75525,
                healthstone = 5512,
                CRYSTAL_OF_INSANITY_BUFF = 127230;

            public bool canUseRaidFlask
            {
                get
                {
                    if (P.myPrefs.RaidFlask == 1)
                    {
                        return false;
                    }
                    if (P.myPrefs.RaidFlask == 2 && (Me.CurrentMap.IsRaid || Me.GroupInfo.IsInRaid))
                    {
                        return true;
                    }
                    if (P.myPrefs.RaidFlask == 3 && (Me.CurrentMap.IsRaid || Me.GroupInfo.IsInRaid) && !Me.GroupInfo.IsInLfgParty)
                    {
                        return true;
                    }
                    if (P.myPrefs.RaidFlask == 4 && (Me.CurrentMap.IsDungeon || Me.CurrentMap.IsInstance))
                    {
                        return true;
                    }
                    return false;
                }
            }
            public static bool CanUseCrystal
            {
                get
                {
                    if (nextBuffAllowed <= DateTime.Now) { return false; }
                    if (CanBuff
                        && !Me.HasAura(S.CRYSTAL_OF_INSANITY_BUFF)
                        && S.NotHaveFlaskBuff
                        && P.myPrefs.FlaskCrystal
                        && UI.HasItem(crystal)) 
                    { 
                        SetNextBuffAllowed(); 
                        return true; 
                    }
                    return false;
                }
            }
            public static bool CanUseAlchemyFlask
            {
                get
                {
                    if (nextBuffAllowed <= DateTime.Now) { return false; }
                    if (CanBuff
                        && !Me.HasAura(S.CRYSTAL_OF_INSANITY_BUFF)
                        && S.NotHaveFlaskBuff
                        && S.NotHaveAlchemyBuff
                        && P.myPrefs.FlaskAlchemy
                        && UI.HasItem(AlchemyFlask))
                    {
                        SetNextBuffAllowed();
                        return true; 
                    }
                    return false;
                }
            }
            #endregion

            public static DateTime nextHealAllowed;
            public static void SetNextHealAllowed()
            {
                nextHealAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2000);
            }

            #region buffwait
            public static DateTime nextBuffAllowed;
            public static DateTime healingTouchTime;

            public static void SetNextBuffAllowed()
            {
                nextBuffAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 5000);
            }
            #endregion

            public static bool IsFeral 
            {
                get { return Me.Specialization == WoWSpec.DruidFeral || (SpellManager.HasSpell(S.CAT_FORM) && P.myPrefs.GoLowbieCat); }
            }
            public static bool IsGuardian
            {
                get { return Me.Specialization == WoWSpec.DruidGuardian; }
            }
            public static bool IsBalance
            {
                get { return Me.Specialization == WoWSpec.DruidBalance || !SpellManager.HasSpell(S.CAT_FORM); }
            }
        }
    }
}
