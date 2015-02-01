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

using HKM = Kitty.KittyHotkeyManagers;
using EH = Kitty.EventHandlers;
using CL = Kitty.CombatLogEventArgs;
using P = Kitty.KittySettings;

namespace Kitty
{
    public partial class KittyMain : CombatRoutine
    {
        public override string Name { get { return "Druid Routine by Pasterke"; } }
        public override WoWClass Class { get { return WoWClass.Druid; } }
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override Composite CombatBehavior 
        { 
            get 
            {
                    return new ActionRunCoroutine(ctx => rotationSelector());
            } 
        }
        public override Composite PreCombatBuffBehavior { get { return new ActionRunCoroutine(ctx => PreCombatBuffCoroutine()); } }
        public override Composite CombatBuffBehavior { get { return new ActionRunCoroutine(ctx => CombatBuffCoroutine()); } }
        public override Composite PullBehavior { get { return new ActionRunCoroutine(ctx => PullCoroutine()); } }
        public override Composite PullBuffBehavior { get { return new ActionRunCoroutine(ctx => PullBuffCoroutine()); } }

        public static WoWGuid lastGuid;
        public static bool checkInCombat { get; set; }
        public static DateTime nextCheckTimer;
        public static DateTime combatTimer;
        public static void setNextCombatTimer()
        {
            combatTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 30 * 1000);
        }
        public static void setNextCheckTimer()
        {
            nextCheckTimer = DateTime.Now + new TimeSpan(0, 0, 0, 30, 0);
        }
        public bool checkTarget { get; set; }
        public static bool _init = false;
        public override bool WantButton { get { return true; } }
        public override void OnButtonPress()
        {
            new KittyGui().ShowDialog();
        }

        public override void Initialize()
        {
            Logging.Write("\r\n" + "-- Hello {0}", Me.Name);
            Logging.Write("-- Thanks for using");
            Logging.Write("-- The Druid Combat Routine");
            Logging.Write("-- by Pasterke" + "\r\n");
            HKM.registerHotKeys();
            Lua.Events.AttachEvent("UI_ERROR_MESSAGE", CL.CombatLogErrorHandler);
            EH.AttachCombatLogEvent();
            Lua.Events.AttachEvent("MODIFIER_STATE_CHANGED", HKM.HandleModifierStateChanged);
            _init = true;
        }

        public override void ShutDown()
        {
            HKM.removeHotkeys();
            EH.DetachCombatLogEvent();
            Lua.Events.DetachEvent("UI_ERROR_MESSAGE", CL.CombatLogErrorHandler);
            Lua.Events.DetachEvent("MODIFIER_STATE_CHANGED", HKM.HandleModifierStateChanged);
        }

        public static int lastPTSize { get; set; }
        public override void Pulse()
        {
            try
            {
                
                if (Me.IsDead
                    && AutoBot)
                {
                    Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/script RepopMe()"));
                }
                return;
            }
            catch (Exception e) { Logging.Write(Colors.Red, "Pulse: " + e); }
            return;
        }
        public override bool NeedRest
        {
            get
            {
                if (HKM.pauseRoutineOn || HKM.manualOn) return false;
                if (Me.HealthPercent <= 50 && !Me.IsSwimming) return true;
                if (Me.HealthPercent <= 85 && !buffExists(REJUVENATION, Me)) return true;
                if (Me.HealthPercent <= 75) return true;
                return false;
            }
        }
        public override void Rest()
        {
            if (Me.HealthPercent <= 50 && !Me.IsSwimming && Canbuff) { Styx.CommonBot.Rest.Feed(); }
            if (Me.HealthPercent <= 75 && !Me.HasAura("Food") && SpellManager.HasSpell(HEALING_TOUCH) && Canbuff) { SpellManager.Cast(HEALING_TOUCH, Me); }
            if (Me.HealthPercent <= 85 && !buffExists(REJUVENATION, Me) && !Me.HasAura("Food") && Canbuff) { SpellManager.Cast(REJUVENATION, Me); }
            base.Rest();
        }

        private static async Task<bool> PreCombatBuffCoroutine()
        {
            if (HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await CastBuff(MARK_OF_THE_WILD, MarkOfTheWildConditions && Canbuff)) return true;
            if (await UseItem(CRYSTAL_OF_ORALIUS_ITEM, CrystalOfOraliusConditions && Canbuff)) return true;
            if (await UseItem(CRYSTAL_OF_INSANITY_ITEM, CrystalOfInsanityConditions && Canbuff)) return true;
            if (await UseItem(ALCHEMYFLASK_ITEM, AlchemyFlaskConditions && Canbuff)) return true;
            if (await CastBuff(TRAVEL_FORM, !Me.Combat && !Me.Mounted && Me.IsSwimming && !Me.HasAura(TRAVEL_FORM))) return true;
            if (await CastBuff(MOONKIN_FORM, MeIsBoomkin 
                && Me.Shapeshift != ShapeshiftForm.Moonkin 
                && Me.Shapeshift != ShapeshiftForm.Travel 
                && !buffExists(PROWL, Me) && Canbuff)) return true;
            return false;
        }

        private static async Task<bool> CombatBuffCoroutine()
        {
            if (HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await CastBuff(MARK_OF_THE_WILD, MarkOfTheWildConditions && Canbuff)) return true;
            if (await UseItem(CRYSTAL_OF_ORALIUS_ITEM, CrystalOfOraliusConditions && Canbuff)) return true;
            if (await UseItem(CRYSTAL_OF_INSANITY_ITEM, CrystalOfInsanityConditions && Canbuff)) return true;
            if (await UseItem(ALCHEMYFLASK_ITEM, AlchemyFlaskConditions && Canbuff)) return true;
            if (await UseItem(HEALTHSTONE_ITEM, Me.HealthPercent <= 45 && Canbuff)) return true;
            if (await CastHeal(REBIRTH, playerToRes, needResPeople && playerToRes != null)) return true;
            if (await CastBuff(BARKSKIN, Me.HealthPercent <= P.myPrefs.PercentBarkskin)) return true;
            if (await CastBuff(REJUVENATION, Me.HealthPercent <= P.myPrefs.PercentRejuCombat && !buffExists(REJUVENATION, Me))) return true;
            if (await CastBuff(HEALING_TOUCH, MeIsBoomkin && Me.HealthPercent <= P.myPrefs.PercentHealingTouchCombat)) return true;
            return false;
        }

        private static async Task<bool> PullBuffCoroutine()
        {
            if (!AutoBot) return false;
            if (HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await CastBuff(TRAVEL_FORM, !Me.Combat && Me.IsSwimming && !buffExists(TRAVEL_FORM, Me) && !buffExists(PROWL, Me) && Canbuff)) return true;
            if (await CastBuff(PROWL, gotTarget && MeIsFeral && (P.myPrefs.PullProwlAndRake || P.myPrefs.PullProwlAndShred) && Canbuff && !buffExists(PROWL, Me))) return true;
            return false;
        }

        private static async Task<bool> PullCoroutine()
        {
            if (!AutoBot) return false;
            if (HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (!pullTimer.IsRunning && AutoBot && Me.CurrentTarget != null) 
            { 
                pullTimer.Restart();
                lastGuid = Me.CurrentTarget.Guid;
                Logging.Write(Colors.CornflowerBlue, "Starting PullTimer"); 
            }
            if (await RemoveRooted(BEAR_FORM, MeIsRooted && (MeIsGuardian || MeIsFeralBear) && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await RemoveRooted(CAT_FORM, MeIsFeral && !MeIsFeralBear && MeIsRooted && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(CAT_FORM, Me.Shapeshift != ShapeshiftForm.Cat && MeIsFeral && !MeIsFeralBear)) return true;
            if (await CastBuff(BEAR_FORM, Me.Shapeshift != ShapeshiftForm.Bear && (MeIsGuardian || MeIsFeralBear))) return true;
            if (await clearTarget(Me.CurrentTarget == null && AllowTargeting && (Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly))) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 4.5f && (MeIsFeral || MeIsGuardian))) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 4.5f && (MeIsFeral || MeIsGuardian))) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 39f && (MeIsBoomkin || MeIsLowbie || MeIsResto))) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 39f && (MeIsBoomkin || MeIsLowbie || MeIsResto))) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget))) return true;
            //feral
            if (await Cast(WILD_CHARGE, gotTarget && P.myPrefs.PullWildCharge && !spellOnCooldown(WILD_CHARGE) && WildChargeConditions(8, 25) && MeIsFeral, Me.CurrentTarget)) return true;
            if (await Cast(MOONFIRE, gotTarget && SpellManager.HasSpell(LUNAR_INSPIRATION) && Me.CurrentTarget.Distance <= 40, Me.CurrentTarget)) return true;
            if (await Cast(MOONFIRE, gotTarget 
                && !SpellManager.HasSpell(FAERIE_FIRE) 
                && !SpellManager.HasSpell(FAERIE_SWARM)
                && !P.myPrefs.PullProwlAndShred 
                && !P.myPrefs.PullProwlAndRake 
                && Me.CurrentTarget.Distance < 35 
                && MeIsFeral, Me.CurrentTarget)) return true;
            if (await Cast(FF, gotTarget && !spellOnCooldown(FF) && !P.myPrefs.PullProwlAndShred && !P.myPrefs.PullProwlAndRake && Me.CurrentTarget.Distance < 35 && MeIsFeral, Me.CurrentTarget)) return true;
            if (await Cast(RAKE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsFeral && P.myPrefs.PullProwlAndRake, Me.CurrentTarget)) return true;
            if (await Cast(SHRED, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsFeral && P.myPrefs.PullProwlAndShred, Me.CurrentTarget)) return true;
            if (await Cast(RAKE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsFeral && !P.myPrefs.PullProwlAndRake && spellOnCooldown(FF), Me.CurrentTarget)) return true;
            if (await Cast(SHRED, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsFeral && !P.myPrefs.PullProwlAndShred && spellOnCooldown(FF), Me.CurrentTarget)) return true;
            //guardian
            if (await Cast(WILD_CHARGE, gotTarget && P.myPrefs.PullWildCharge && !spellOnCooldown(WILD_CHARGE) && WildChargeConditions(8, 25) && MeIsGuardian, Me.CurrentTarget)) return true;
            if (await Cast(FF, gotTarget && !spellOnCooldown(FF) && Me.CurrentTarget.Distance <= 30 && MeIsGuardian, Me.CurrentTarget)) return true;
            if (await Cast(GROWL, gotTarget && spellOnCooldown(FF) && Me.CurrentTarget.Distance <= 30 && MeIsGuardian, Me.CurrentTarget)) return true;
            if (await Cast(LACERATE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsGuardian, Me.CurrentTarget)) return true;
            //moonkin - lowbie
            if (await Cast(MOONFIRE, gotTarget && Me.CurrentTarget.Distance <= 39 && (MeIsBoomkin || MeIsLowbie || MeIsResto) && !debuffExists(MOONFIRE, Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(WRATH, gotTarget && Me.CurrentTarget.Distance <= 39 && (MeIsBoomkin || MeIsLowbie || MeIsResto), Me.CurrentTarget)) return true;

            return false;
        }

        #region rest
        private static async Task<bool> EatFood(bool req)
        {
            if (!req) return false;
            Styx.CommonBot.Rest.Feed();
            await CommonCoroutines.SleepForLagDuration();
            return Canbuff;
        }
        #endregion

        #region spec
        public static bool MeIsFeralBear { get { return Me.Specialization == WoWSpec.DruidFeral && (HKM.SwitchBearForm || Me.HealthPercent <= P.myPrefs.PercentSwitchBearForm); } }
        public static bool MeIsFeral { get { return (Me.Specialization == WoWSpec.DruidFeral && !MeIsFeralBear) || (Me.Level < 10 && SpellManager.HasSpell(CAT_FORM)); } }
        public static bool MeIsGuardian { get { return Me.Specialization == WoWSpec.DruidGuardian || MeIsFeralBear; } }
        public static bool MeIsBoomkin { get { return Me.Specialization == WoWSpec.DruidBalance; } }
        public static bool MeIsResto { get { return Me.Specialization == WoWSpec.DruidRestoration; } }
        public static bool MeIsLowbie { get { return Me.Level < 10 && !SpellManager.HasSpell(CAT_FORM); } }
        #endregion
    }
}
