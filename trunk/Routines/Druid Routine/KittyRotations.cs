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

namespace Kitty
{
    public partial class KittyMain : CombatRoutine
    {
        public static async Task<bool> rotationSelector()
        {
            if (MeIsGuardian && await BearRotationCoroutine()) return true;
            if (MeIsFeral && await FeralRotationCoroutine()) return true;
            if (MeIsBoomkin && await BoomkinRotationCoroutine()) return true;
            if (MeIsResto && await HealingRotationCoroutine()) return true;
            if (MeIsLowbie && !SpellManager.HasSpell(CAT_FORM) && await LowbieRotationCoroutine()) return true;
            return false;
        }

        #region BearRotation

        public static async Task<bool> BearRotationCoroutine()
        {
            if (!AutoBot && Me.Mounted) return false;
            if (await RemoveRooted(BEAR_FORM, MeIsRooted && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(BEAR_FORM, Me.Shapeshift != ShapeshiftForm.Bear)) return true;
            if (await findTargets(Me.CurrentTarget == null && AllowTargeting && FindTargetsCount >= 1)) return true;
            if (await clearTarget(Me.CurrentTarget != null && AutoBot && Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly)) return true;
            if (await findMeleeAttackers(gotTarget && AutoBot && AllowTargeting && Me.CurrentTarget.Distance > 10 && MeleeAttackersCount >= 1)) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 4.5f)) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 4.5f)) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;

            if (await CastBuff(BARKSKIN, gotTarget && Me.HealthPercent <= P.myPrefs.PercentBarkskin && !spellOnCooldown(BARKSKIN))) return true;
            if (await Cast(SKULL_BASH, gotTarget && SkullBashConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(INCAPACITATING_ROAR, gotTarget && IncapacitatingRoarConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(TYPHOON, gotTarget && TyphoonConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(MIGHTY_BASH, gotTarget && MightyBashConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(WAR_STOMP, gotTarget && WarStompConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(HEALING_TOUCH, Me.HealthPercent <= 90 && IsOverlayed(HEALING_TOUCH_INT))) return true;
            if (await CastBuff(FRENZIED_REGENERATION, BearFrenziedRegenerationConditions)) return true;
            if (await CastBuff(SURVIVAL_INSTINCTS, !spellOnCooldown(SURVIVAL_INSTINCTS) && Me.HealthPercent <= P.myPrefs.PercentSurvivalInstincts)) return true;
            if (await CastBuff(SAVAGE_DEFENSE, BearSavageDefenseConditions)) return true;
            if (await CastBuff(BERSERK, gotTarget && BerserkBearConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(INCARNATION_BEAR, gotTarget && IncarnationBearConditions && Me.CurrentTarget.IsWithinMeleeRange))
            if (await Cast(FORCE_OF_NATURE, gotTarget && ForceOfNatureConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await NeedTrinket1(UseTrinket1 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket1Use)) return true;
            if (await NeedTrinket2(UseTrinket2 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket2Use)) return true;
            if (await CastGroundSpellTrinket(1, Me.CurrentTarget.Location, gotTarget && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, Me.CurrentTarget.Location, gotTarget && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await Cast(MANGLE, gotTarget && !spellOnCooldown(MANGLE) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(THRASH, gotTarget && BearThrashConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(MAUL, gotTarget && BearMaulConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(LACERATE, gotTarget && BearLacerateConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(PULVERIZE, gotTarget && !spellOnCooldown(PULVERIZE) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            return false;
        }
        
        #endregion

        #region FeralRotation

        public static async Task<bool> FeralRotationCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (!AutoBot && Me.Mounted) return false;
            if (await RemoveRooted(FERALFORM, MeIsRooted && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(CAT_FORM, Me.Shapeshift != ShapeshiftForm.Cat)) return true;
            if (await findTargets(Me.CurrentTarget == null && AutoBot && AllowTargeting && FindTargetsCount >= 1)) return true;
            if (await findMeleeAttackers(gotTarget && AllowTargeting && Me.CurrentTarget.Distance > 10 && MeleeAttackersCount >= 1)) return true;
            if (await clearTarget(Me.CurrentTarget != null && (Me.CurrentTarget.IsDead || (AutoBot && Me.CurrentTarget.IsFriendly)))) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 4.5f)) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 4.5f)) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;

            if (await CastBuff(REJUVENATION, Me.HealthPercent <= P.myPrefs.PercentRejuCombat && !buffExists(REJUVENATION, Me))) return true;
            if (await CastBuff(SURVIVAL_INSTINCTS, !spellOnCooldown(SURVIVAL_INSTINCTS) && Me.HealthPercent <= P.myPrefs.PercentSurvivalInstincts)) return true;
            if (await CastBuff(HEALING_TOUCH, IsOverlayed(HEALING_TOUCH_INT))) return true;
            if (await Cast(SKULL_BASH, gotTarget && SkullBashConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(INCAPACITATING_ROAR, gotTarget && IncapacitatingRoarConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(TYPHOON, gotTarget && TyphoonConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(MIGHTY_BASH, gotTarget && MightyBashConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(WAR_STOMP, gotTarget && WarStompConditions(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(SAVAGE_ROAR, gotTarget && SavageRoarConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(TIGERS_FURY, gotTarget && TigersFuryConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(BERSERK, gotTarget && BerserkConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(INCARNATION_CAT, gotTarget && IncarnationCatConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(FORCE_OF_NATURE, gotTarget && ForceOfNatureConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(FEROCIUOS_BITE, gotTarget && FerociousBiteConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await NeedTrinket1(UseTrinket1 && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await NeedTrinket2(UseTrinket2 && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(1, Me.CurrentTarget.Location, gotTarget && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, Me.CurrentTarget.Location, gotTarget && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await Cast(RIP, gotTarget && RipConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(RAKE, gotTarget && RakeConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(THRASH, gotTarget && ThrashConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(SHRED, gotTarget && ShredConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(SWIPE, gotTarget && SwipeConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            return false;
        }

        #endregion

        #region BoomkinRotation

        public static async Task<bool> BoomkinRotationCoroutine()
        {
            if (await CastBuff(MOONKIN_FORM, Me.Shapeshift != ShapeshiftForm.Moonkin)) return true;

            return false;
        }

        #endregion

        #region LowbieRotation

        public static async Task<bool> LowbieRotationCoroutine()
        {
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 39f)) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 39f)) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;
            if (await Cast(MOONFIRE, gotTarget && !debuffExists(MOONFIRE, Me.CurrentTarget)) && Me.CurrentTarget.Distance <= 39) return true;
            if (await Cast(WRATH, gotTarget) && Me.CurrentTarget.Distance <= 39) return true;

            return false;
        }

        #endregion

        #region HealingRotation

        public static async Task<bool> HealingRotationCoroutine()
        {
            if (await CastBuff(MOONKIN_FORM, Me.Shapeshift != ShapeshiftForm.Moonkin)) return true;

            return false;
        }

        #endregion
    }
}