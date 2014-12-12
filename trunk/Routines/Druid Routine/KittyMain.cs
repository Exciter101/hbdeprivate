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

        public override Composite CombatBehavior { get { return new ActionRunCoroutine(ctx => rotationSelector()); } }
        public override Composite PreCombatBuffBehavior { get { return new ActionRunCoroutine(ctx => PreCombatBuffCoroutine()); } }
        public override Composite CombatBuffBehavior { get { return new ActionRunCoroutine(ctx => CombatBuffCoroutine()); } }
        public override Composite PullBehavior { get { return new ActionRunCoroutine(ctx => PullCoroutine()); } }
        public override Composite PullBuffBehavior { get { return new ActionRunCoroutine(ctx => PullBuffCoroutine()); } }
        public override Composite RestBehavior { get { return new ActionRunCoroutine(ctx => RestCoroutine()); } }

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
        }

        public override void ShutDown()
        {
            HKM.removeHotkeys();
            EH.DetachCombatLogEvent();
            Lua.Events.DetachEvent("UI_ERROR_MESSAGE", CL.CombatLogErrorHandler);
        }

        public static int lastPTSize { get; set; }
        public override void Pulse()
        {
            try
            {
                if (partyCount != lastPTSize)
                {
                    Logging.Write(Colors.DarkTurquoise, "Current PartySize: " + partyCount);
                    lastPTSize = partyCount;
                }
                if (Me.IsDead
                    && AutoBot)
                {
                    Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/script RepopMe()"));
                }
                if (AutoBot)
                {
                    CheckMyCurrentTarget();
                }
                return;
            }
            catch (Exception e) { Logging.Write(Colors.Red, "Pulse: " + e); }
            return;
        }

        private static async Task<bool> RestCoroutine()
        {
            if (!AutoBot) return false;
            if (HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await EatFood(Me.HealthPercent <= 50 && !Me.IsSwimming)) return true;
            if (await CastBuff(HEALING_TOUCH, Me.HealthPercent <= 75)) return true;
            if (await CastBuff(REJUVENATION, Me.HealthPercent <= 85 && !buffExists(REJUVENATION, Me))) return true;
            return false;
        }

        private static async Task<bool> PreCombatBuffCoroutine()
        {
            if (HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await CastBuff(MARK_OF_THE_WILD, MarkOfTheWildConditions && Canbuff)) return true;
            if (await UseItem(CRYSTAL_OF_ORALIUS_ITEM, CrystalOfOraliusConditions && Canbuff)) return true;
            if (await UseItem(CRYSTAL_OF_INSANITY_ITEM, CrystalOfInsanityConditions && Canbuff)) return true;
            if (await UseItem(ALCHEMYFLASK_ITEM, AlchemyFlaskConditions && Canbuff)) return true;
            if (await CastBuff(SAVAGE_ROAR, SavageRoarConditions && Me.Shapeshift == ShapeshiftForm.Cat) && Canbuff) return true;
            if (await CastBuff(TRAVEL_FORM, !Me.Combat && Me.IsSwimming && !Me.HasAura(TRAVEL_FORM))) return true;
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
            if (await CastHeal(REBIRTH, TankPlayerToRes, TankPlayerToRes != null && HKM.resTanks && !spellOnCooldown(REBIRTH))) return true;
            if (await CastHeal(REBIRTH, HealerPlayerToRes, HealerPlayerToRes != null && HKM.resHealers && !spellOnCooldown(REBIRTH))) return true;
            if (await CastHeal(REBIRTH, AllPlayerToRes, AllPlayerToRes != null && HKM.resAll && !spellOnCooldown(REBIRTH))) return true;
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
            if (HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await RemoveRooted(BEAR_FORM, MeIsRooted && MeIsGuardian && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await RemoveRooted(FERALFORM, MeIsFeral && MeIsRooted && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(CAT_FORM, Me.Shapeshift != ShapeshiftForm.Cat && MeIsFeral)) return true;
            if (await CastBuff(BEAR_FORM, Me.Shapeshift != ShapeshiftForm.Bear && MeIsGuardian)) return true;
            if (await clearTarget(Me.CurrentTarget == null && AutoBot && (Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly))) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 4.5f && (MeIsFeral || MeIsGuardian))) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 4.5f && (MeIsFeral || MeIsGuardian))) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 39f && (MeIsBoomkin || MeIsLowbie || MeIsResto))) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 39f && (MeIsBoomkin || MeIsLowbie || MeIsResto))) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget))) return true;
            //feral
            if (await Cast(WILD_CHARGE, gotTarget && P.myPrefs.PullWildCharge && !spellOnCooldown(WILD_CHARGE) && WildChargeConditions(8, 25) && MeIsFeral)) return true;
            if (await Cast(MOONFIRE, gotTarget && SpellManager.HasSpell(LUNAR_INSPIRATION) && Me.CurrentTarget.Distance <= 35)) return true;
            if (await Cast(MOONFIRE, gotTarget 
                && !SpellManager.HasSpell(FAERIE_FIRE) 
                && !SpellManager.HasSpell(FAERIE_SWARM)
                && !P.myPrefs.PullProwlAndShred 
                && !P.myPrefs.PullProwlAndRake 
                && Me.CurrentTarget.Distance < 35 
                && MeIsFeral)) return true;
            if (await Cast(FF, gotTarget && !spellOnCooldown(FF) && !P.myPrefs.PullProwlAndShred && !P.myPrefs.PullProwlAndRake && Me.CurrentTarget.Distance < 35 && MeIsFeral)) return true;
            if (await Cast(RAKE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsFeral && P.myPrefs.PullProwlAndRake)) return true;
            if (await Cast(SHRED, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsFeral && P.myPrefs.PullProwlAndShred)) return true;
            if (await Cast(RAKE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsFeral && !P.myPrefs.PullProwlAndRake && spellOnCooldown(FF))) return true;
            if (await Cast(SHRED, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsFeral && !P.myPrefs.PullProwlAndShred && spellOnCooldown(FF))) return true;
            //guardian
            if (await Cast(WILD_CHARGE, gotTarget && P.myPrefs.PullWildCharge && !spellOnCooldown(WILD_CHARGE) && WildChargeConditions(8, 25) && MeIsGuardian)) return true;
            if (await Cast(FF, gotTarget && !spellOnCooldown(FF) && Me.CurrentTarget.Distance <= 30 && MeIsGuardian)) return true;
            if (await Cast(GROWL, gotTarget && spellOnCooldown(FF) && Me.CurrentTarget.Distance <= 30 && MeIsGuardian)) return true;
            if (await Cast(LACERATE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && MeIsGuardian)) return true;
            //moonkin - lowbie
            if (await Cast(MOONFIRE, gotTarget && Me.CurrentTarget.Distance <= 39 && (MeIsBoomkin || MeIsLowbie || MeIsResto) && !debuffExists(MOONFIRE, Me.CurrentTarget))) return true;
            if (await Cast(WRATH, gotTarget && Me.CurrentTarget.Distance <= 39 && (MeIsBoomkin || MeIsLowbie || MeIsResto))) return true;

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
        public static bool MeIsFeral { get { return Me.Specialization == WoWSpec.DruidFeral || (Me.Level < 10 && SpellManager.HasSpell(CAT_FORM)); } }
        public static bool MeIsGuardian { get { return Me.Specialization == WoWSpec.DruidGuardian; } }
        public static bool MeIsBoomkin { get { return Me.Specialization == WoWSpec.DruidBalance; } }
        public static bool MeIsResto { get { return Me.Specialization == WoWSpec.DruidRestoration; } }
        public static bool MeIsLowbie { get { return Me.Level < 10 && !SpellManager.HasSpell(CAT_FORM); } }
        #endregion
    }
}
