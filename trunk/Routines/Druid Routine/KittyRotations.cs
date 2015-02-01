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
            if (MeIsFeralBear && await BearRotationCoroutine()) return true;
            if (MeIsGuardian && await BearRotationCoroutine()) return true;
            if (MeIsFeral && !MeIsFeralBear && await FeralRotationCoroutine()) return true;
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
            if (await stopPullTimer(pullTimer.IsRunning && AutoBot)) return true;
            if (await RemoveRooted(BEAR_FORM, MeIsRooted && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(DASH, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now > snareTimer)) return true;
            if (await CastBuff(STAMPEDING_ROAR, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now > snareTimer)) return true;
            if (await CastBuff(BEAR_FORM, Me.Shapeshift != ShapeshiftForm.Bear)) return true;
            if (await findTargets(Me.CurrentTarget == null && AllowTargeting && FindTargetsCount >= 1)) return true;
            if (await clearTarget(Me.CurrentTarget != null && AllowTargeting && (Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly))) return true;
            if (await findMeleeAttackers(gotTarget && AutoBot && AllowTargeting && Me.CurrentTarget.Distance > 10 && MeleeAttackersCount >= 1)) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 4.5f)) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 4.5f)) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;

            if (await CastBuff(BARKSKIN, gotTarget && Me.HealthPercent <= P.myPrefs.PercentBarkskin && !spellOnCooldown(BARKSKIN))) return true;

            //interuupts
            if (await Cast(SKULL_BASH, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needSkullBash(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(INCAPACITATING_ROAR, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needIncapacitatingRoar(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(TYPHOON, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needTyphoon(Me.CurrentTarget), Me.CurrentTarget)) return true;

            //stun targets
            if (await Cast(MIGHTY_BASH, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needMightyBash(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(WAR_STOMP, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needWarStomp(Me.CurrentTarget), Me.CurrentTarget)) return true;

            if (await CastBuff(HEALING_TOUCH, Me.HealthPercent <= 90 && IsOverlayed(5185))) return true;
            if (await CastBuff(FRENZIED_REGENERATION, BearFrenziedRegenerationConditions)) return true;
            if (await CastBuff(SURVIVAL_INSTINCTS, !spellOnCooldown(SURVIVAL_INSTINCTS) && Me.HealthPercent <= P.myPrefs.PercentSurvivalInstincts)) return true;
            if (await CastBuff(SAVAGE_DEFENSE, BearSavageDefenseConditions)) return true;


            if (await Cast(WILD_CHARGE, gotTarget && WildChargeConditions(8, 25), Me.CurrentTarget)) return true;
            if (await CastBuff(BERSERK, gotTarget && !spellOnCooldown(BERSERK) && BerserkBearConditions && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(INCARNATION_BEAR, gotTarget && !spellOnCooldown(INCARNATION_BEAR) && IncarnationBearConditions && Me.CurrentTarget.IsWithinMeleeRange))
                if (await Cast(FORCE_OF_NATURE, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needForceOfNature(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await NeedTrinket1(UseTrinket1 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket1Use)) return true;
            if (await NeedTrinket2(UseTrinket2 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket2Use)) return true;
            if (await CastGroundSpellTrinket(1, gotTarget && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, gotTarget && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await Cast(PULVERIZE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange, Me.CurrentTarget)) return true;
            if (await Cast(MANGLE, gotTarget && !spellOnCooldown(MANGLE) && Me.CurrentTarget.IsWithinMeleeRange, Me.CurrentTarget)) return true;
            if (await Cast(THRASH, gotTarget && BearThrashConditions && Me.CurrentTarget.IsWithinMeleeRange, Me.CurrentTarget)) return true;
            if (await Cast(MAUL, gotTarget && BearMaulConditions && Me.CurrentTarget.IsWithinMeleeRange, Me.CurrentTarget)) return true;
            if (await Cast(LACERATE, gotTarget && BearLacerateConditions && Me.CurrentTarget.IsWithinMeleeRange, Me.CurrentTarget)) return true;
            //if (await blackListingUnit(Me.CurrentTarget != null && AutoBot && lastGuid == Me.CurrentTarget.Guid && fightTimer.ElapsedMilliseconds >= 30, Me.CurrentTarget)) return true;
            
            return false;
        }

        #endregion

        #region FeralRotation
        


        public static async Task<bool> FeralRotationCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn || (!AutoBot && Me.Mounted)) return false;
            if (await stopPullTimer(pullTimer.IsRunning && AutoBot)) return true;
            if (await RemoveRooted(FERALFORM, MeIsRooted && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(DASH, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now >= snareTimer)) return true;
            if (await CastBuff(STAMPEDING_ROAR, MeIsSnared && gotTarget && !Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now >= snareTimer)) return true;
            if (await CastBuff(CAT_FORM, Me.Shapeshift != ShapeshiftForm.Cat)) return true;
            if (await findTargets(Me.CurrentTarget == null && AllowTargeting && FindTargetsCount >= 1)) return true;
            if (await findMeleeAttackers(gotTarget && AllowTargeting && Me.CurrentTarget.Distance > 10 && MeleeAttackersCount >= 1)) return true;
            if (await clearTarget(Me.CurrentTarget != null && AllowTargeting && (Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly))) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 4.5f)) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 4.5f)) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;

            
            if (await CastBuff(SURVIVAL_INSTINCTS, !spellOnCooldown(SURVIVAL_INSTINCTS) && Me.HealthPercent <= P.myPrefs.PercentSurvivalInstincts)) return true;

            if (await GetRandomInterruptTimer(rndInterrupt, Me.CurrentTarget != null && Me.CurrentTarget.IsCasting && Me.CanInterruptCurrentSpellCast)) return true;
            if (await CastBuff(REJUVENATION, Me.HealthPercent <= P.myPrefs.PercentRejuCombat && !buffExists(REJUVENATION, Me))) return true;
            if (await Cast(HEALING_TOUCH, _feralHealingTouchUnit != null, _feralHealingTouchUnit)) return true;

            //interuupts
            if (await Cast(SKULL_BASH, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needSkullBash(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(INCAPACITATING_ROAR, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needIncapacitatingRoar(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(TYPHOON, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needTyphoon(Me.CurrentTarget), Me.CurrentTarget)) return true;

            //stun targets
            if (await Cast(MIGHTY_BASH, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needMightyBash(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(WAR_STOMP, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needWarStomp(Me.CurrentTarget), Me.CurrentTarget)) return true;

            if (await CastBuff(SAVAGE_ROAR, needSavageRoar)) return true;
            if (await CastBuff(TIGERS_FURY, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needTigersFury)) return true;

            //cooldowns
            if (await CastBuff(INCARNATION_CAT, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needIncarnation(Me.CurrentTarget))) return true;
            if (await CastBuff(BERSERK, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needBerserk(Me.CurrentTarget))) return true;
            if (await CastBuff(BERSERKING, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needBerserking(Me.CurrentTarget))) return true;
            if (await NeedTrinket1(UseTrinket1 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket1Use)) return true;
            if (await NeedTrinket2(UseTrinket2 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket2Use)) return true;
            if (await CastGroundSpellTrinket(1, gotTarget && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, gotTarget && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await Cast(FORCE_OF_NATURE, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needForceOfNature(Me.CurrentTarget), Me.CurrentTarget)) return true;

            if (await Cast(FEROCIUOS_BITE, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needFerociousBite(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(RIP, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needRip(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(RAKE, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needRake(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(MOONFIRE, _moonfireTarget != null, _moonfireTarget)) return true;
            if (await Cast(THRASH, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needThrash, Me.CurrentTarget)) return true;
            if (await Cast(SHRED, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needShred(Me.CurrentTarget), Me.CurrentTarget)) return true;
            if (await Cast(SWIPE, Me.CurrentTarget != null && validTarget(Me.CurrentTarget) && needSwipe(Me.CurrentTarget), Me.CurrentTarget)) return true;
            //if (await blackListingUnit(Me.CurrentTarget != null && AutoBot && lastGuid == Me.CurrentTarget.Guid && fightTimer.ElapsedMilliseconds >= 30, Me.CurrentTarget)) return true;
            
            return false;
        }

        #endregion

        #region BoomkinRotation

        public static async Task<bool> BoomkinRotationCoroutine()
        {
            if (await stopPullTimer(pullTimer.IsRunning && AutoBot)) return true;
            if (await CastBuff(MOONKIN_FORM, Me.Shapeshift != ShapeshiftForm.Moonkin)) return true;
            if (await findTargets(Me.CurrentTarget == null && AllowTargeting && FindTargetsCount >= 1)) return true;
            if (await clearTarget(Me.CurrentTarget != null && AllowTargeting && (Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly))) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 39f)) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 39f)) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;
            if (await Cast(TYPHOON, gotTarget && MeleeAttackersCount > P.myPrefs.AoeMoonkin, Me.CurrentTarget)) return true;
            if (await CastMultiDot(MOONFIRE, dotTargets(MOONFIRE), dotTargets(MOONFIRE) != null
                && !debuffExists(MOONFIRE, Me.CurrentTarget)
                && addCount <= P.myPrefs.AoeMoonkin
                && SpellManager.HasSpell(MOONFIRE))) return true;
            if (await CastMultiDot(SUNFIRE, dotTargets(SUNFIRE), dotTargets(SUNFIRE) != null
                && !debuffExists(SUNFIRE, Me.CurrentTarget)
                && addCount <= P.myPrefs.AoeMoonkin
                && SpellManager.HasSpell(SUNFIRE))) return true;
            if (await Cast(STARFALL, gotTarget && !spellOnCooldown(STARFALL) && addCount > P.myPrefs.AoeMoonkin, Me.CurrentTarget)) return true;
            if (await CastGroundSpell(HURRICANE, gotTarget && !Me.IsChanneling && addCount > P.myPrefs.AoeMoonkin)) return true;
            if (await Cast(STARSURGE, gotTarget && IsOverlayed(STARSURGE_INT), Me.CurrentTarget)) return true;
            if (await Cast(MOONFIRE, gotTarget && IsOverlayed(MOONFIRE_INT), Me.CurrentTarget)) return true;
            if (await Cast(SUNFIRE, gotTarget && IsOverlayed(SUNFIRE_INT), Me.CurrentTarget)) return true;
            if (await Cast(STARFIRE, gotTarget && IsOverlayed(STARFIRE_INT), Me.CurrentTarget)) return true;
            if (await Cast(WRATH, gotTarget && IsOverlayed(WRATH_INT), Me.CurrentTarget)) return true;
            if (await Cast(STARFIRE, gotTarget && !IsOverlayed(WRATH_INT) && !IsOverlayed(STARFIRE_INT), Me.CurrentTarget)) return true;
            //if (await blackListingUnit(Me.CurrentTarget != null && AutoBot && lastGuid == Me.CurrentTarget.Guid && fightTimer.ElapsedMilliseconds >= 30, Me.CurrentTarget)) return true;
            
            return false;
        }

        #endregion

        #region LowbieRotation

        public static async Task<bool> LowbieRotationCoroutine()
        {
            if (await stopPullTimer(pullTimer.IsRunning && AutoBot)) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 39f)) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 39f)) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;
            if (await Cast(MOONFIRE, gotTarget && !debuffExists(MOONFIRE, Me.CurrentTarget) && Me.CurrentTarget.Distance <= 39, Me.CurrentTarget)) return true;
            if (await Cast(WRATH, gotTarget && Me.CurrentTarget.Distance <= 39, Me.CurrentTarget)) return true;
            //if (await blackListingUnit(Me.CurrentTarget != null && AutoBot && lastGuid == Me.CurrentTarget.Guid && fightTimer.ElapsedMilliseconds >= 30, Me.CurrentTarget)) return true;
            
            return false;
        }

        #endregion

        #region HealingRotation

        public static async Task<bool> HealingRotationCoroutine()
        {
            
            if (await findTargets(Me.CurrentTarget == null && AllowTargeting && FindTargetsCount >= 1)) return true;
            if (await findMeleeAttackers(gotTarget && AllowTargeting && Me.CurrentTarget.Distance > 40 && MeleeAttackersCount >= 1)) return true;
            if (await clearTarget(Me.CurrentTarget != null && AllowTargeting && (Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly))) return true;
            if (await MoveToTarget(gotTarget && AllowMovement && Me.CurrentTarget.Distance > 40f)) return true;
            if (await StopMovement(gotTarget && AllowMovement && Me.CurrentTarget.Distance <= 40f)) return true;
            if (await FaceMyTarget(gotTarget && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;
            if (await CastHeal(REGROWTH, _regrowthProcPlayer, _regrowthProcPlayer != null && IsOverlayed(REGROWTH_INT))) return true;
            if (await CastHeal(REGROWTH, _regrowthPlayerCritical, _regrowthPlayerCritical != null)) return true;
            if (await CastHeal(NATURES_CURE, dispelTargets, dispelTargets != null)) return true;
            if (await CastBuff(BARKSKIN, !spellOnCooldown(BARKSKIN) && Me.HealthPercent <= P.myPrefs.PercentBarkskin)) return true;
            if (await CastHeal(IRONBARK, _ironbarkPlayer, _ironbarkPlayer != null && !spellOnCooldown(IRONBARK))) return true;
            if (await CastBuff(NATURES_VIGIL, gotTarget && !spellOnCooldown(NATURES_VIGIL) && needNaturesVigil)) return true;
            if (await CastHeal(LIFEBLOOM, _lifebloomPlayer, _lifebloomPlayer != null && !buffExists(LIFEBLOOM, _lifebloomPlayer))) return true;
            if (await CastMushroom(WILD_MUSHROOM, _mushroomPlayer, _mushroomPlayer != null && needMushroom && !Me.IsMoving)) return true;
            if (await CastHeal(SWIFTMEND, _swiftmendPlayer, _swiftmendPlayer != null)) return true;
            if (await NeedTrinket1(UseTrinket1 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket1Use)) return true;
            if (await NeedTrinket2(UseTrinket2 && nextTrinketTimeAllowed <= DateTime.Now && !P.myPrefs.Trinket2Use)) return true;
            if (await CastGroundSpellTrinket(1, gotTarget && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, gotTarget && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastHeal(FORCE_OF_NATURE, _forceofnaturePlayer, _forceofnaturePlayer != null && (_forceofnaturePlayer.Guid != lastFonGuid || DateTime.Now > healfonTimer))) return true;
            if (await CastHeal(WILD_GROWTH, _wildgrowthPlayer, _wildgrowthPlayer != null && !spellOnCooldown(WILD_GROWTH))) return true;
            if (await CastHeal(REGROWTH, _regrowthPlayer, _regrowthPlayer != null)) return true;
            if (await CastHeal(HEALING_TOUCH, _healingtouchPlayer, _healingtouchPlayer != null)) return true;
            if (await CastHeal(GERMINATION, _germinationPlayer, _germinationPlayer != null)) return true;
            if (await CastHeal(REJUVENATION, _rejuvenationPlayer, _rejuvenationPlayer != null)) return true;
            if (await CastHeal(WRATH, _wrathTarget, _wrathTarget != null && Me.IsSafelyFacing(_wrathTarget))) return true;
            if (await CastHeal(GENESIS, _genesisPlayer, _genesisPlayer != null)) return true;
            if (await CastDmgSpell(MOONFIRE, gotTarget && !debuffExists(MOONFIRE, Me.CurrentTarget) && partyCount == 0, Me.CurrentTarget)) return true;
            if (await CastDmgSpell(WRATH, gotTarget && partyCount == 0, Me.CurrentTarget)) return true;

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        #endregion

        #region healing variables

        #region fill party

        #region proving grounds
        public static bool InProvingGrounds { get { return Me.MinimapZoneText == "Proving Grounds"; } }
        #endregion

        public static WoWUnit healTarget;
        public static WoWUnit aoeTarget;
        public static IEnumerable<WoWUnit> getPartyMembers()
        {
            if (InProvingGrounds)
            {
                return Group.PulsePartyMembersPG().ToList();
            }
            else
            {
                return StyxWoW.Me.GroupInfo.RaidMembers.Union(StyxWoW.Me.GroupInfo.PartyMembers).Select(p => p.ToPlayer()).Distinct();
            }
        }
        public static IEnumerable<WoWUnit> newPartyMembers
        {
            get
            {
                return getPartyMembers();
            }
        }
        #endregion

        #region canCast
        public static bool canCast
        {
            get { return !Me.IsCasting && !Me.IsChanneling; }
        }
        #endregion

        #region partycount
        public static int partyCount
        {
            get
            {
                if (Me.CurrentMap.IsBattleground || Me.CurrentMap.IsPveInstance) { return Me.GroupInfo.NumRaidMembers; }
                return Me.GroupInfo.NumRaidMembers == 0 ? Me.GroupInfo.NumPartyMembers : Me.GroupInfo.NumRaidMembers;
            }
        }
        #endregion

        #region _wrathTarget
        public static WoWUnit _wrathTarget
        {
            get
            {
                if (!SpellManager.HasSpell(DREAM_OF_CENARIUS)) { return null; }

                if (partyCount == 0) return null;

                var myTanks = Tanks().Where(p => p != null && p.InLineOfSight && p.InLineOfSpellSight && p.Distance <= 40).FirstOrDefault();

                if (myTanks != null && myTanks.IsDead && gotTarget) return Me.CurrentTarget;

                return myTanks != null ? myTanks.CurrentTarget : null;
            }
        }
        public static bool IsAttackingTank(WoWUnit unit)
        {
            return unit.Combat;
        }
        #endregion

        #region ironbark
        public static WoWUnit _ironbarkPlayer
        {
            get
            {
                if (partyCount == 0 && Me.HealthPercent <= 45) return Me;

                var result = Tanks().Where(p => p != null
                && p.IsAlive
                && p.InLineOfSight
                && p.InLineOfSpellSight
                && p.HealthPercent <= 45
                && p.Distance <= 40);

                return result != null ? result.FirstOrDefault() : null;
            }
        }
        #endregion

        #region natures vigil
        public static int naturesVigilCount
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return 3;
                    if (partyCount > 10) return 4;
                    return 2;
            }
        }
        public static int naturesVigilHealth = 85;
        public static bool needNaturesVigil
        {
            get
            {
                var result = newPartyMembers.Where(p => p != null
                && p.IsAlive
                && p.InLineOfSight
                && p.InLineOfSpellSight
                && p.HealthPercent <= naturesVigilHealth
                && p.Distance <= 40);

                return result.Count() >= naturesVigilCount ? true : false;
            }
        }
        #endregion

        #region wild growth
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
        public static WoWUnit _wildgrowthPlayer
        {
            get
            {
                var result = newPartyMembers.Where(p => p != null
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40
                    && p.HealthPercent <= Wildgrowthhealth);
                return result.Count() >= WildGrowthCount ? result.FirstOrDefault() : null;
            }
        }
        #endregion

        #region tranquility
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
        public static WoWUnit _tranquilityPlayer
        {
            get
            {
                var result = newPartyMembers.Where(p => p != null
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40
                    && p.HealthPercent <= TranquilityHealth);
                return result.Count() >= TranquilityCount ? result.FirstOrDefault() : null;
            }
        }
        #endregion

        #region rejuvenation
        public static int RejuvenationHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.Rejuvenation510;
                if (partyCount > 10) return P.myPrefs.Rejuvenation50;
                return P.myPrefs.Rejuvenation5;
            }
        }
        public static WoWUnit _germinationPlayer 
        {
            get
            {
                if (partyCount == 0 && buffExists(REJUVENATION, Me) && !buffExists(GERMINATION, Me) && Me.HealthPercent <= RejuvenationHealth) return Me;
                var results = newPartyMembers.Where(p => p != null
                        && p.IsAlive
                        && p.InLineOfSight
                        && p.InLineOfSpellSight
                        && buffExists(REJUVENATION, p)
                        && !buffExists(GERMINATION, p)
                        && p.Distance <= 40
                        && p.HealthPercent <= RejuvenationHealth).FirstOrDefault();

                return results != null ? results : null;
            }
        }
        public static WoWUnit _rejuvenationPlayer
        {
            get
            {
                if(partyCount == 0 && !buffExists(REJUVENATION, Me) && Me.HealthPercent <= RejuvenationHealth) return Me;

                WoWUnit target = null;

                if (SpellManager.HasSpell(RAMPANT_GROWTH) && !buffExists(REJUVENATION, Me))
                {
                    return Me;
                }
                else
                {
                    target = newPartyMembers.Where(p => p != null
                        && p.IsAlive
                        && p.InLineOfSight
                        && p.InLineOfSpellSight
                        && !buffExists(REJUVENATION, p)
                        && p.Distance <= 40
                        && p.HealthPercent <= RejuvenationHealth).FirstOrDefault();
                }
                return target != null ? target : null;
            }
        }
        #endregion

        #region regrowth
        public static int RegrowthHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.Regrowth510;
                if (partyCount > 10) return P.myPrefs.Regrowth50;
                return P.myPrefs.Regrowth5;
            }
        }
        public static WoWUnit _regrowthPlayerCritical
        {
            get
            {
                if (partyCount == 0 && Me.HealthPercent <= RegrowthHealth) return Me;
                var result = newPartyMembers.Where(p => p != null
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40
                    && p.HealthPercent <= 40);
                return result != null ? result.FirstOrDefault() : null;
            }
        }
        public static WoWUnit _regrowthProcPlayer
        {
            get
            {
                if (partyCount == 0 && (Me.HealthPercent <= RegrowthHealth || IsOverlayed(REGROWTH_INT))) return Me;
                var result = newPartyMembers.Where(p => p != null
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40).OrderBy(p => p.HealthPercent).FirstOrDefault();
                return result != null ? result : null;
            }
        }
        public static WoWUnit _regrowthPlayer
        {
            get
            {
                if (partyCount == 0 && Me.HealthPercent <= RegrowthHealth) return Me;
                var result = newPartyMembers.Where(p => p != null
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40
                    && p.HealthPercent <= RegrowthHealth);
                return result != null ? result.FirstOrDefault() : null;
            }
        }
        #endregion

        #region healing touch
        public static int HaelingTouchHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.HealingTouch510;
                if (partyCount > 10) return P.myPrefs.HealingTouch50;
                return P.myPrefs.HealingTouch5;
            }
        }
        public static WoWUnit _healingtouchPlayer
        {
            get
            {
                if (partyCount == 0 && Me.HealthPercent <= HaelingTouchHealth) return Me;
                var result = newPartyMembers.Where(p => p != null
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40
                    && p.HealthPercent <= HaelingTouchHealth);
                return result != null ? result.FirstOrDefault() : null;
            }
        }
        #endregion

        #region genesis
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
        public static WoWUnit _genesisPlayer
        {
            get
            {
                var result = newPartyMembers.Where(p => p != null
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40
                    && p.HealthPercent <= GenesisHealth);
                return result.Count() >= GenesisCount ? result.FirstOrDefault() : null;
            }
        }
        #endregion

        #region swiftmend
        public static WoWUnit _swiftmendPlayer
        {
            get
            {
                WoWUnit target = null;
                if (SpellManager.HasSpell(RAMPANT_GROWTH) && buffExists(REJUVENATION, Me))
                {
                    target = newPartyMembers.Where(p => p != null
                           && p.IsAlive
                           && p.InLineOfSight
                           && p.InLineOfSpellSight
                           && p.Distance <= 40
                           && p.HealthPercent <= SwiftmendHealth).OrderBy(p => p.HealthPercent).FirstOrDefault();
                }
                else
                {
                    target = newPartyMembers.Where(p => p != null
                           && p.IsAlive
                           && p.InLineOfSight
                           && p.InLineOfSpellSight
                           && p.Distance <= 40
                           && (buffExists(REJUVENATION, p) || buffExists(REGROWTH, p))
                           && p.HealthPercent <= SwiftmendHealth).OrderBy(p => p.HealthPercent).FirstOrDefault();
                }
                return target != null ? target : null;
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
        #endregion

        #region force of nature
        public static WoWGuid lastFonGuid;
        public static DateTime healfonTimer;
        public static int fonHealth
        {
            get
            {
                if (partyCount > 5 && partyCount <= 10) return P.myPrefs.ForceOfNature510;
                if (partyCount > 10) return P.myPrefs.ForceOfNature50;
                return P.myPrefs.ForceOfNature5;
            }
        }
        public static WoWUnit _forceofnaturePlayer
        {
            get
            {
                var result = newPartyMembers.Where(p => p != null
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40
                    && p.HealthPercent <= fonHealth);
                return result != null ? result.FirstOrDefault() : null;
            }
        }
        #endregion

        #region mushroom
        public static int mushroomID = 145205;
        public static int mushroomHealth = 85;
        public static DateTime mushroomTimer;
        public static WoWPoint mushroomLocation { get; set; }
        private static WoWUnit mushroomTarget { get; set; }

        public static bool needMushroom
        {
            get
            {
                if (Me.IsMoving) return false;
                if (DateTime.Now >= mushroomTimer) return true;
                if (mushroomTarget != null && mushroomTarget.Location.Distance(mushroomTarget.Location) >= 15) return true;
                return false;
            }
        }
        public static WoWUnit _mushroomPlayer
        {
            get
            {
                var result = newPartyMembers.Where(p => p != null
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40
                    && p.HealthPercent <= mushroomHealth);
                return result.Count() >= 3 ? result.FirstOrDefault() : null;
            }
        }
        #endregion

        #region lifebloom
        public static WoWUnit _lifebloomPlayer
        {
            get
            {
                if (partyCount == 0 && !buffExists(LIFEBLOOM, Me)) return Me;
                if (Me.MinimapZoneText == "Proving Grounds") return newPartyMembers.Where(p => p.Name == "Oto the Protector").FirstOrDefault(); //.Select(p => p.ToUnit()).FirstOrDefault();
                var results = Tanks().Where(p => p != null 
                    && p.IsAlive 
                    && p.Distance <= 40 
                    && p.InLineOfSight 
                    && p.InLineOfSpellSight).OrderBy(p => p.Distance).ToList();
                if (results.Count() > 0)
                {
                    foreach (WoWUnit unit in results)
                    {
                        if (buffExists(LIFEBLOOM, unit)) return null;
                    }
                }
                return results.Count() > 0 ? results.FirstOrDefault() : null;

            }
        }
        #endregion

        public static List<WoWPlayer> Tanks()
        {
            return StyxWoW.Me.GroupInfo.RaidMembers.Union(StyxWoW.Me.GroupInfo.PartyMembers)
                    .Where(p => p != null && p.HasRole(WoWPartyMember.GroupRole.Tank)).Select(p => p.ToPlayer()).ToList();
        }
        public static List<WoWPlayer> Healers()
        {
            return StyxWoW.Me.GroupInfo.RaidMembers.Union(StyxWoW.Me.GroupInfo.PartyMembers)
                    .Where(p => p != null && p.HasRole(WoWPartyMember.GroupRole.Healer)).Select(p => p.ToPlayer()).ToList();
        }
        public static List<WoWPlayer> Damage()
        {
            return StyxWoW.Me.GroupInfo.RaidMembers.Union(StyxWoW.Me.GroupInfo.PartyMembers)
                    .Where(p => p != null && p.HasRole(WoWPartyMember.GroupRole.Damage)).Select(p => p.ToPlayer()).ToList();
        }
        public static WoWPlayer ResTanks()
        {
            List<WoWPlayer> members = new List<WoWPlayer>();
            members = Tanks().Where(p => p != null
                && !p.IsMe
                && p.IsDead
                && p.InLineOfSight
                && p.InLineOfSpellSight
                && p.Distance <= 40).OrderBy(p => p.Distance).ToList();
            if (members.Count() >= 0)
            {
                return members.FirstOrDefault();
            }
            return null;
        }
        public static WoWPlayer ResHealers()
        {
            List<WoWPlayer> members = new List<WoWPlayer>();
            members = Healers().Where(p => p != null
                && !p.IsMe
                && p.IsDead
                && p.InLineOfSight
                && p.InLineOfSpellSight
                && p.Distance <= 40).OrderBy(p => p.Distance).ToList();
            if (members.Count() >= 0)
            {
                return members.FirstOrDefault();
            }
            return null;
        }
        public static WoWPlayer ResDamage()
        {
            List<WoWPlayer> members = new List<WoWPlayer>();
            members = Damage().Where(p => p != null
                && !p.IsMe
                && p.IsDead
                && p.InLineOfSight
                && p.InLineOfSpellSight
                && p.Distance <= 40).OrderBy(p => p.Distance).ToList();
            if (members.Count() >= 0)
            {
                return members.FirstOrDefault();
            }
            return null;
        }
        public static WoWUnit dispelTargets
        {
            get
            {
                if (!P.myPrefs.AutoDispel) return null;
                var result = newPartyMembers.Where(p => p != null
                    && CanDispelTarget(p)
                    && p.IsAlive
                    && p.InLineOfSight
                    && p.InLineOfSpellSight
                    && p.Distance <= 40).FirstOrDefault();

                return result != null ? result : null;

            }
        }
        #endregion

        #region healing spellcasts
        public static async Task<bool> CastHeal(string Spell, WoWUnit myTarget, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (myTarget.IsDead) return false;
            if (spellOnCooldown(Spell)) return false;
            if (!myTarget.InLineOfSpellSight && !myTarget.InLineOfSight) return false;
            if (myTarget.Distance > 40) return false;
            if (Spell == FORCE_OF_NATURE) { healfonTimer = DateTime.Now + new TimeSpan(0, 0, 0, 30, 0); lastFonGuid = myTarget.Guid; }
            if (Spell == HEALING_TOUCH && !spellOnCooldown(NATURES_SWIFTNESS)) { SpellManager.Cast(NATURES_SWIFTNESS); }
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            Logging.Write(Colors.Yellow, "Casting: " + Spell + " on: " + myTarget.SafeName);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastDmgSpell(string Spell, bool reqs, WoWUnit myTarget)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (myTarget.IsDead) return false;
            if (spellOnCooldown(Spell)) return false;
            if (!myTarget.InLineOfSpellSight && !myTarget.InLineOfSight) return false;
            if (myTarget.Distance > 40) return false;
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            Logging.Write(Colors.Yellow, "Casting: " + Spell + " on: " + myTarget.SafeName);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastMushroom(string Spell, WoWUnit myTarget, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (myTarget.IsDead) return false;
            if (spellOnCooldown(Spell)) return false;
            if (!myTarget.InLineOfSpellSight && !myTarget.InLineOfSight) return false;
            if (myTarget.Distance > 40) return false;
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            Logging.Write(Colors.Yellow, "Casting: " + Spell + " on: " + myTarget.SafeName);
            mushroomTimer = DateTime.Now + new TimeSpan(0, 0, 0, 30, 0);
            mushroomLocation = myTarget.Location;
            mushroomTarget = myTarget;
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        
        #endregion

        #region proving grounds
        public static HashSet<string> pNPC = new HashSet<string>()
        {
            "Ki the Assassin",
            "Sooli the Survivalist",
            "Oto the Protector",
            "Kavan the Arcanist"
        };
        public static IEnumerable<WoWPlayer> SearchAreaPlayers()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWPlayer>();
        }

        public static List<WoWPlayer> SearchAreaUnits()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(p => pNPC.Contains(p.Name)).Select(p => p.ToPlayer()).ToList();
        }
        #endregion
    }
}