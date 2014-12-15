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
using Styx.CommonBot.Coroutines;

namespace Kitty
{
    public partial class KittyMain : CombatRoutine
    {
        public static uint ptMembers { get { return Me.GroupInfo.PartySize; } }

        public static async Task<bool> rotationSelector()
        {
            if (MeIsGuardian && await BearRotationCoroutine()) return true;
            if (MeIsFeral && SpellManager.HasSpell(SAVAGE_ROAR_GLYPH) && await SavageRoarGlyphedCoroutine()) return true;
            if (MeIsFeral && !SpellManager.HasSpell(SAVAGE_ROAR_GLYPH) && await FeralRotationCoroutine()) return true;
            if (MeIsBoomkin && await BoomkinRotationCoroutine()) return true;
            if (MeIsResto && await HealingRotationCoroutine()) return true;
            if (MeIsLowbie && await LowbieRotationCoroutine()) return true;
            return false;
        }

        #region BearRotation

        public static async Task<bool> BearRotationCoroutine()
        {
            if (!AutoBot && Me.Mounted) return false;
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (pullTimer.IsRunning) { pullTimer.Stop(); }
            if (await RemoveRooted(BEAR_FORM, MeIsRooted && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(DASH, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now > snareTimer)) return true;
            if (await CastBuff(STAMPEDING_ROAR, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now > snareTimer)) return true;
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
            if (await CastBuff(HEALING_TOUCH, Me.HealthPercent <= 90 && IsOverlayed(5185))) return true;
            if (await CastBuff(FRENZIED_REGENERATION, BearFrenziedRegenerationConditions)) return true;
            if (await CastBuff(SURVIVAL_INSTINCTS, !spellOnCooldown(SURVIVAL_INSTINCTS) && Me.HealthPercent <= P.myPrefs.PercentSurvivalInstincts)) return true;
            if (await CastBuff(SAVAGE_DEFENSE, BearSavageDefenseConditions)) return true;
            if (await Cast(WILD_CHARGE, gotTarget && WildChargeConditions(8, 25))) return true;
            if (await CastBuff(BERSERK, gotTarget && !spellOnCooldown(BERSERK) && BerserkBearConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(INCARNATION_BEAR, gotTarget && !spellOnCooldown(INCARNATION_BEAR) && IncarnationBearConditions && Me.CurrentTarget.IsWithinMeleeRange))
            if (await Cast(FORCE_OF_NATURE, gotTarget && DateTime.Now >= fonTimer && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await NeedTrinket1(UseTrinket1 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket1Use)) return true;
            if (await NeedTrinket2(UseTrinket2 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket2Use)) return true;
            if (await CastGroundSpellTrinket(1, gotTarget && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, gotTarget && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await Cast(PULVERIZE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(MANGLE, gotTarget && !spellOnCooldown(MANGLE) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(THRASH, gotTarget && BearThrashConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(MAUL, gotTarget && BearMaulConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(LACERATE, gotTarget && BearLacerateConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            return false;
        }

        #endregion

        #region FeralRotation
        public static async Task<bool> SavageRoarGlyphedCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn || (!AutoBot && Me.Mounted)) return false;
            if (pullTimer.IsRunning && AutoBot)
            {
                pullTimer.Stop();
                Logging.Write(Colors.CornflowerBlue, "Stopping PullTimer => Combat");
                lastGuid = Me.CurrentTarget.Guid;
                fightTimer.Restart();
            }
            if (await RemoveRooted(FERALFORM, MeIsRooted && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(DASH, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now > snareTimer)) return true;
            if (await CastBuff(STAMPEDING_ROAR, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now > snareTimer)) return true;
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
            if (await CastBuff(TIGERS_FURY, gotTarget && TigersFuryConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(WILD_CHARGE, gotTarget && WildChargeConditions(8, 25))) return true;
            if (await CastBuff(BERSERK, gotTarget 
                && (Targets.IsWoWBoss(Me.CurrentTarget) || HKM.cooldownsOn)
                && !spellOnCooldown(BERSERK)
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(INCARNATION_CAT, gotTarget 
                && !spellOnCooldown(INCARNATION_CAT)
                && buffExists(BERSERK, Me)
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(FORCE_OF_NATURE, gotTarget
                && ((Targets.IsWoWBoss(Me.CurrentTarget) || HKM.cooldownsOn)
                || (!Targets.IsWoWBoss(Me.CurrentTarget)
                && !HKM.cooldownsOn
                && DateTime.Now >= fonTimer))
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(FEROCIUOS_BITE, gotTarget 
                && (debuffExists(RIP, Me.CurrentTarget) 
                && debuffTimeLeft(RIP, Me.CurrentTarget) > 6000 
                && Me.EnergyPercent >= 25 
                && Me.ComboPoints >= 5
                && Me.CurrentTarget.IsWithinMeleeRange)
                || (gotTarget && Me.CurrentTarget.MaxHealth < Me.MaxHealth * 1.5
                && Me.EnergyPercent >= 25
                && Me.ComboPoints >= 3
                && Me.CurrentTarget.IsWithinMeleeRange))
                || (gotTarget && Me.CurrentTarget.HealthPercent < 25)
                && debuffExists(RIP, Me.CurrentTarget)
                && debuffTimeLeft(RIP, Me.CurrentTarget) < 6000
                && Me.EnergyPercent >= 25
                && Me.ComboPoints >= 1
                && Me.CurrentTarget.IsWithinMeleeRange) return true;
            if (await NeedTrinket1(UseTrinket1 
                && nextTrinketTimeAllowed <= DateTime.Now)
                && Me.CurrentTarget.IsWithinMeleeRange) return true;
            if (await NeedTrinket2(UseTrinket2 
                && nextTrinketTimeAllowed <= DateTime.Now)
                && Me.CurrentTarget.IsWithinMeleeRange) return true;
            if (await CastGroundSpellTrinket(1, Me.CurrentTarget != null 
                && P.myPrefs.Trinket1Use 
                && nextTrinketTimeAllowed <= DateTime.Now)
                && Me.CurrentTarget.IsWithinMeleeRange) return true;
            if (await CastGroundSpellTrinket(2, Me.CurrentTarget != null 
                && P.myPrefs.Trinket2Use 
                && nextTrinketTimeAllowed <= DateTime.Now)
                && Me.CurrentTarget.IsWithinMeleeRange) return true;
            if (await Cast(RIP, gotTarget 
                && Me.CurrentTarget.MaxHealth > Me.MaxHealth * 1.5
                && (!debuffExists(RIP, Me.CurrentTarget)
                || (debuffExists(RIP, Me.CurrentTarget) && debuffTimeLeft(RIP, Me.CurrentTarget) <= 4500))
                && Me.ComboPoints >= 5
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(RAKE, gotTarget 
                && (!debuffExists(RAKE, Me.CurrentTarget)
                || (debuffExists(RAKE, Me.CurrentTarget) && debuffTimeLeft(RAKE, Me.CurrentTarget) <= 4500))
                && Me.EnergyPercent >= 35
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(MOONFIRE, gotTarget && SpellManager.HasSpell(LUNAR_INSPIRATION) && !debuffExists(MOONFIRE, Me.CurrentTarget) && Me.CurrentTarget.Distance <= 35)) return true;
            if (await Cast(THRASH, gotTarget 
                && ThrashConditions 
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(SHRED, gotTarget && ShredConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(SWIPE, gotTarget 
                && Me.EnergyPercent >= 50
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CannotContinueFight(Me.CurrentTarget, Me.CurrentTarget != null
                && AutoBot
                && Me.CurrentTarget.HealthPercent >= 95
                && !Me.CurrentTarget.IsPlayer
                && lastGuid == Me.CurrentTarget.Guid
                && fightTimer.ElapsedMilliseconds >= 30 * 1000)) return true;
            return false;
        }


        public static async Task<bool> FeralRotationCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn || (!AutoBot && Me.Mounted)) return false;
            if (pullTimer.IsRunning && AutoBot) 
            { 
                pullTimer.Stop(); 
                Logging.Write(Colors.CornflowerBlue, "Stopping PullTimer => Combat"); 
                lastGuid = Me.CurrentTarget.Guid; 
                fightTimer.Restart(); 
            }
            if (await RemoveRooted(FERALFORM, MeIsRooted && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(DASH, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now > snareTimer)) return true;
            if (await CastBuff(STAMPEDING_ROAR, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now > snareTimer)) return true;
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
            if (await Cast(WILD_CHARGE, gotTarget && WildChargeConditions(8, 25))) return true;
            if (await CastBuff(BERSERK, gotTarget && BerserkConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(INCARNATION_CAT, gotTarget && IncarnationCatConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(FORCE_OF_NATURE, gotTarget && DateTime.Now >= fonTimer && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(FEROCIUOS_BITE, gotTarget && FerociousBiteConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await NeedTrinket1(UseTrinket1 && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await NeedTrinket2(UseTrinket2 && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(1, Me.CurrentTarget != null && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, Me.CurrentTarget != null && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await Cast(RIP, gotTarget && RipConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(RAKE, gotTarget && RakeConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(MOONFIRE, gotTarget && SpellManager.HasSpell(LUNAR_INSPIRATION) && !debuffExists(MOONFIRE, Me.CurrentTarget) && Me.CurrentTarget.Distance <= 35)) return true;
            if (await Cast(THRASH, gotTarget && ThrashConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(SHRED, gotTarget && ShredConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(SWIPE, gotTarget && SwipeConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CannotContinueFight(Me.CurrentTarget, Me.CurrentTarget != null 
                && AutoBot
                && Me.CurrentTarget.HealthPercent >= 95 
                && !Me.CurrentTarget.IsPlayer
                && lastGuid == Me.CurrentTarget.Guid
                && fightTimer.ElapsedMilliseconds >= 30 * 1000)) return true;
            return false;
        }

        #endregion

        #region BoomkinRotation

        public static async Task<bool> BoomkinRotationCoroutine()
        {
            if (pullTimer.IsRunning) { pullTimer.Stop(); }
            if (await CastBuff(MOONKIN_FORM, Me.Shapeshift != ShapeshiftForm.Moonkin)) return true;

            return false;
        }

        #endregion

        #region LowbieRotation

        public static async Task<bool> LowbieRotationCoroutine()
        {
            if (pullTimer.IsRunning) { pullTimer.Stop(); }
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
            if (await CastHeal(REGROWTH, regrowthProcPlayer, regrowthProcPlayer != null && IsOverlayed(REGROWTH_INT))) return true;
            if (await CastHeal(NATURES_CURE, dispelTargets, dispelTargets != null)) return true;
            if (await CastBuff(BARKSKIN, !spellOnCooldown(BARKSKIN) && Me.HealthPercent <= P.myPrefs.PercentBarkskin)) return true;
            if (await CastHeal(IRONBARK, LifebloomPlayer, LifebloomPlayer != null && !spellOnCooldown(IRONBARK) && LifebloomPlayer.HealthPercent <= 45)) return true;
            if (await CastBuff(NATURES_VIGIL, gotTarget && !spellOnCooldown(NATURES_VIGIL) && naturesVigil)) return true;
            if (await CastHeal(LIFEBLOOM, LifebloomPlayer, LifebloomPlayer != null && !buffExists(LIFEBLOOM, LifebloomPlayer))) return true;
            if (await CastMushroom(WILD_MUSHROOM, mushRoomPlayer, mushRoomPlayer != null && needMushroom && !Me.IsMoving)) return true;
            if (await CastHeal(SWIFTMEND, SwiftmendPlayer, SwiftmendPlayer != null)) return true;
            if (await NeedTrinket1(UseTrinket1 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket1Use)) return true;
            if (await NeedTrinket2(UseTrinket2 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket2Use)) return true;
            if (await CastGroundSpellTrinket(1, gotTarget && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, gotTarget && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastHeal(GENESIS, GenesisPlayers, GenesisPlayers != null)) return true;
            if (await CastHeal(FORCE_OF_NATURE, fonTarget, fonTarget != null && (fonTarget.Guid != lastFonGuid || DateTime.Now > healfonTimer))) return true;
            if (await CastHeal(WILD_GROWTH, WildGrowthPlayer, WildGrowthPlayer != null && !spellOnCooldown(WILD_GROWTH))) return true;
            if (await CastHeal(REJUVENATION, RejuvenationPlayer, RejuvenationPlayer != null)) return true;
            if (await CastHeal(HEALING_TOUCH, HealingTouchPlayer, HealingTouchPlayer != null)) return true;
            if (await CastHeal(REGROWTH, RegrowthPlayer, RegrowthPlayer != null)) return true;
            if (await CastDmgSpell(MOONFIRE, lifebloomCurrentTarget, lifebloomCurrentTarget != null
                && MeIsSolo
                && Me.ManaPercent > 90
                && !debuffExists(MOONFIRE, lifebloomCurrentTarget))) return true;
            if (await CastDmgSpell(WRATH, lifebloomCurrentTarget, lifebloomCurrentTarget != null
                && MeIsSolo
                && Me.ManaPercent > 90)) return true;
            Thread.Sleep(10);
            return false;
        }

        #endregion

        #region healing variables
        public static int partyCount 
        { 
            get 
            {
                return Me.GroupInfo.NumRaidMembers == 0 ? Me.GroupInfo.NumPartyMembers : Me.GroupInfo.NumRaidMembers;
            } 
        }
        public static bool naturesVigil
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= 85 && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).ToList();
                if (members.Count() >= 1)
                {
                    if (partyCount > 5 && partyCount <= 10 && members.Count() >= 3) return true;
                    if (partyCount > 10 && members.Count() >= 4) return true;
                    if (partyCount <= 5 && members.Count() >= 2) return true;
                }
                return false;
            }
        }
        public static int WildGrowthCount
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.WildGrowthPlayers510;
                if (partyCount > 10) return P.myPrefs.WildGrowthPlayers50;
                return P.myPrefs.WildGrowthPlayers5;
            }
        }
        public static int Wildgrowthhealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.WildGrowth510;
                if (partyCount > 10) return P.myPrefs.WildGrowth50;
                return P.myPrefs.WildGrowth5;
            }
        }
        public static int TranquilityCount
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.TranquilityPlayers510;
                if (partyCount > 10) return P.myPrefs.TranquilityPlayers50;
                return P.myPrefs.TranquilityPlayers5;
            }
        }
        public static int TranquilityHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.Tranquility510;
                if (partyCount > 10) return P.myPrefs.Tranquility50;
                return P.myPrefs.Tranquility5;
            }
        }
        public static int RejuvenationHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.Rejuvenation510;
                if (partyCount > 10) return P.myPrefs.Rejuvenation50;
                return P.myPrefs.Rejuvenation5;
            }
        }
        public static int RegrowthHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.Regrowth510;
                if (partyCount > 10) return P.myPrefs.Regrowth50;
                return P.myPrefs.Regrowth5;
            }
        }
        public static int HaelingTouchHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.HealingTouch510;
                if (partyCount > 10) return P.myPrefs.HealingTouch50;
                return P.myPrefs.HealingTouch5;
            }
        }
        public static int GenesisCount
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.GenesisPlayers510;
                if (partyCount > 10) return P.myPrefs.GenesisPlayers50;
                return P.myPrefs.GenesisPlayers5;
            }
        }
        public static int GenesisHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.Genesis510;
                if (partyCount > 10) return P.myPrefs.Genesis50;
                return P.myPrefs.Genesis5;
            }
        }
        public static int SwiftmendHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.Swiftmend510;
                if (partyCount > 10) return P.myPrefs.Swiftmend50;
                return P.myPrefs.Swiftmend5;
            }
        }
        public static int fonHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.ForceOfNature510;
                if (partyCount > 10) return P.myPrefs.ForceOfNature50;
                return P.myPrefs.ForceOfNature5;
            }
        }
        public static int mushroomID = 145205;
        public static WoWGuid lastFonGuid;
        public static DateTime healfonTimer;
        public static void SetNextForceOfNatureAllowed()
        {
            healfonTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 5000);
        }
        public static WoWPoint mushroomLocation { get; set; }
        private static DateTime mushroomTimer;
        private static WoWPlayer mushroomTarget { get; set; }

        public static IEnumerable<WoWUnit> Mushrooms
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.Name == "Wild Mushroom" && o.CreatedByUnitGuid == StyxWoW.Me.Guid && o.Distance <= 40);

            }
        }
        public static int MushroomCount
        {
            get { return Mushrooms.Count(); }
        }

        public static bool needMushroom
        {
            get
            {
                if (MushroomCount == 0) return true;
                if (MushroomCount != 0
                    && mushroomTarget != null
                    && mushroomTarget.HealthPercent <= 85
                    && mushroomTarget.Location.Distance(mushroomLocation) > 15) return true;
                return false;
            }
        }
        public static List<WoWPlayer> getPartyMembers()
        {
            List<WoWPlayer> targets = new List<WoWPlayer>();
            targets = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p => p != null
                && p.IsAlive
                && p.Distance <= 100
                && p.IsInMyPartyOrRaid).ToList();
            return targets;
        }

        public static WoWPlayer regrowthProcPlayer
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                if (members.Count() == 0) { return Me; }
                return null;
            }
        }

        public static WoWPlayer fonTarget
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= fonHealth && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                if (members.Count() == 0 && Me.HealthPercent <= fonHealth) { return Me; }
                return null;
            }
        }

        public static List<WoWPlayer> Tanks()
        {
            return StyxWoW.Me.GroupInfo.RaidMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank))
                    .Select(p => p.ToPlayer())
                    .Where(p => p != null && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).ToList();
        }
        public static List<WoWPlayer> ResTanks()
        {
            return StyxWoW.Me.GroupInfo.RaidMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank))
                    .Select(p => p.ToPlayer())
                    .Where(p => p != null && p.IsDead && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).ToList();
        }
        public static WoWPlayer TankPlayerToRes
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = ResTanks();
                if (members.Count() > 0)
                {
                    return members.FirstOrDefault();
                }
                return null;
            }
        }
        public static List<WoWPlayer> ResHealers()
        {
            return StyxWoW.Me.GroupInfo.RaidMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Healer))
                    .Select(p => p.ToPlayer())
                    .Where(p => p != null && p.IsDead && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).ToList();
        }
        public static WoWPlayer HealerPlayerToRes
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = ResHealers();
                if (members.Count() > 0)
                {
                    return members.FirstOrDefault();
                }
                return null;
            }
        }
        public static WoWPlayer AllPlayerToRes
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.IsDead && p.InLineOfSpellSight && p.InLineOfSight && p.Distance <= 40).ToList();
                if (members.Count() > 0)
                {
                    return members.FirstOrDefault();
                }
                return null;
            }
        }
        public static WoWUnit lifebloomCurrentTarget
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                WoWPlayer myTank = null;
                members = Tanks();
                if (members.Count() > 0)
                {
                    foreach (WoWPlayer unit in members)
                    {
                        if (buffExists(LIFEBLOOM, unit)) return null;
                    }
                    myTank = members.FirstOrDefault();
                    if (myTank.CurrentTarget != null && ValidUnit(myTank.CurrentTarget)) return myTank.CurrentTarget;
                }
                if (members.Count() == 0 && Me.CurrentTarget != null) { return Me.CurrentTarget; }
                return null;
            }
        }
        public static WoWPlayer mushRoomPlayer
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = Tanks();
                if (members.Count() > 0)
                {
                    foreach (var unit in members)
                    {
                        if (unit.HealthPercent <= 85) return unit;
                    }
                    return null;
                }
                if (members.Count() == 0 && Me.HealthPercent <= 85) { return Me; }
                return null;
            }
        }

        public static WoWPlayer LifebloomPlayer
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = Tanks();
                if (members.Count() > 0)
                {
                    foreach (WoWPlayer unit in members)
                    {
                        if (buffExists(LIFEBLOOM, unit)) return null;
                    }
                    return members.FirstOrDefault();
                }
                if (members.Count() == 0 && !buffExists(LIFEBLOOM, Me)) { return Me; }
                return null;
            }
        }
        public static WoWPlayer SwiftmendPlayer
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => (buffExists(REJUVENATION, p) || buffExists(REGROWTH, p)) && p.HealthPercent <= SwiftmendHealth && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                return null;
            }
        }
        public static WoWPlayer GenesisPlayers
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => buffExists(REJUVENATION, p) && p.HealthPercent <= GenesisHealth && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();
                if (members.Count() >= GenesisCount)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                return null;
            }
        }
        public static WoWPlayer WildGrowthPlayer
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= Wildgrowthhealth && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();
                if (members.Count() >= WildGrowthCount)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                return null;
            }
        }
        public static WoWPlayer RejuvenationPlayer
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= RejuvenationHealth && !buffExists(REJUVENATION, p) && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                if (members.Count() == 0 && !buffExists(REJUVENATION, Me) && Me.HealthPercent <= RejuvenationHealth) { return Me; }
                return null;
            }
        }
        public static WoWPlayer RegrowthPlayer
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= RegrowthHealth && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                if (members.Count() == 0 && Me.HealthPercent <= RegrowthHealth) { return Me; }
                return null;
            }
        }
        public static WoWPlayer HealingTouchPlayer
        {
            get
            {
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= HaelingTouchHealth && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).OrderBy(p => p.HealthPercent).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                if (members.Count() == 0 && Me.HealthPercent <= HaelingTouchHealth) { return Me; }
                return null;
            }
        }
        public static WoWPlayer dispelTargets
        {
            get
            {
                if (!P.myPrefs.AutoDispel) return null;
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p != null && CanDispelTarget(p) && p.IsAlive && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                if (members.Count() == 0 && CanDispelTarget(Me)) { return Me; }
                return null;
            }
        }
        #endregion

        #region healing spellcasts
        public static async Task<bool> CastHeal(string Spell, WoWPlayer myTarget, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (Spell == FORCE_OF_NATURE) { healfonTimer = DateTime.Now + new TimeSpan(0,0,0,30,0); lastFonGuid = myTarget.Guid; }
            if (Spell == HEALING_TOUCH && !spellOnCooldown(NATURES_SWIFTNESS)) { SpellManager.Cast(NATURES_SWIFTNESS); }
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            Logging.Write(Colors.Yellow, "Casting: " + Spell + " on: " + myTarget.SafeName);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastDmgSpell(string Spell, WoWUnit myTarget, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            Logging.Write(Colors.Yellow, "Casting: " + Spell + " on: " + myTarget.SafeName);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastMushroom(string Spell, WoWPlayer myTarget, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            Logging.Write(Colors.Yellow, "Casting: " + Spell + " on: " + myTarget.SafeName);
            mushroomLocation = myTarget.Location;
            mushroomTarget = myTarget;
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastCombiHeal(string Spell, WoWUnit myTarget, bool reqs)
        {
            if (!SpellManager.HasSpell(NATURES_SWIFTNESS) || spellOnCooldown(NATURES_SWIFTNESS)) return false;
            if (!SpellManager.HasSpell(HEALING_TOUCH)) return false;
            if (!reqs) return false;
            SpellManager.Cast(NATURES_SWIFTNESS);
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            Logging.Write(Colors.Yellow, "Casting Critical : " + Spell + " on: " + myTarget.SafeName);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        #endregion
    }
}