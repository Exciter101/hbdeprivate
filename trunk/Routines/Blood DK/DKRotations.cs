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
using Styx.CommonBot.Coroutines;
using Buddy.Coroutines;

namespace DK
{
    public partial class DKMain : CombatRoutine
    {
        public static async Task<bool> rotationSelector()
        {
            if (Me.Specialization == WoWSpec.DeathKnightBlood && await BloodRoutine()) return true;
            if (Me.Specialization == WoWSpec.DeathKnightFrost && await FrostRoutine()) return true;
            if (Me.Specialization == WoWSpec.DeathKnightUnholy && await UnholyRoutine()) return true;
            return false;
        }
        public static async Task<bool> BloodRoutine()
        {
            if (!AutoBot && Me.Mounted && !buffExists(TELAARI_TALBUK_INT, Me)) return false;

            if (pullTimer.IsRunning) { pullTimer.Stop(); }
            if (buffExists("Hand of Protection", Me)) { Lua.DoString("RunMacroText(\"/cancelaura Hand Of Protection\")"); }

            if (await CastBuff(GIFT_OF_THE_NAARU, Me.HealthPercent <= P.myPrefs.PercentNaaru && !spellOnCooldown(GIFT_OF_THE_NAARU))) return true;
            if (await UseItem(HEALTHSTONE_ITEM, Me.HealthPercent <= P.myPrefs.PercentHealthstone)) return true;
            if (await NeedTrinket1(P.myPrefs.Trinket1HP > 0 && Me.HealthPercent <= P.myPrefs.Trinket1HP)) return true;
            if (await NeedTrinket2(P.myPrefs.Trinket2HP > 0 && Me.HealthPercent <= P.myPrefs.Trinket2HP)) return true;

            if (await findTargets(Me.CurrentTarget == null && AllowTargeting && FindTargetsCount >= 1)) return true;
            if (await clearTarget(Me.CurrentTarget != null && AllowTargeting && (Me.CurrentTarget.IsDead || (AutoBot && Me.CurrentTarget.IsFriendly)))) return true;
            if (await findMeleeAttackers(Me.CurrentTarget != null && AllowTargeting && Me.CurrentTarget.Distance > 10 && MeleeAttackersCount >= 1)) return true;
            if (await MoveToTarget(Me.CurrentTarget != null && AllowMovement && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await StopMovement(Me.CurrentTarget != null && AllowMovement && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await FaceMyTarget(Me.CurrentTarget != null && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;
            // res people
            if (await CastRes(RAISE_ALLY, needResPeople && playerToRes != null, playerToRes)) return true;

            //interrupt
            if (await Cast(MIND_FREEZE, gotTarget && Me.CurrentTarget.IsCasting && Me.CanInterruptCurrentSpellCast && !spellOnCooldown(MIND_FREEZE) && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            //protection
            if (await CastBuff(BONE_SHIELD, !buffExists(BONE_SHIELD, Me) && !spellOnCooldown(BONE_SHIELD))) return true;
            if (await CastBuff(ANTI_MAGIC_SHELL, gotTarget && Me.CurrentTarget.IsCasting && !spellOnCooldown(ANTI_MAGIC_SHELL) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(ICEBOUND_FORTITUDE, gotTarget && Me.HealthPercent < P.myPrefs.IceBoundFortitude && !spellOnCooldown(ICEBOUND_FORTITUDE) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(DANCING_RUNE_WEAPON, gotTarget && Me.HealthPercent < P.myPrefs.DancingRuneWeapon && !spellOnCooldown(DANCING_RUNE_WEAPON) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(VAMPIRIC_BLOOD, gotTarget && Me.HealthPercent < P.myPrefs.VampiricBlood && !spellOnCooldown(VAMPIRIC_BLOOD) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(EMPOWER_RUNE_WEAPON, gotTarget && needEmpoweredRuneWeapon && !spellOnCooldown(EMPOWER_RUNE_WEAPON) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            
            if (await CastBuff(RUNE_TAP, gotTarget && Me.HealthPercent < P.myPrefs.RuneTap && !spellOnCooldown(RUNE_TAP))) return true;
            if (await CastBuff(DEATH_PACT, gotTarget && Me.HealthPercent < P.myPrefs.DeathPact && !spellOnCooldown(DEATH_PACT) && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(CONVERSION, gotTarget && needConversion)) return true;
            if (await CastBuff(HORN_OF_WINTER, gotTarget && !buffExists(HORN_OF_WINTER, Me))) return true;

            //running away mobs
            if (await Cast(CHAINS_OF_ICE, gotTarget && Me.IsSafelyBehind(Me.CurrentTarget) && !spellOnCooldown(CHAINS_OF_ICE) && !Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(DEATH_COIL, gotTarget && Me.IsSafelyBehind(Me.CurrentTarget) && Me.RunicPowerPercent >= 30 && Range30)) return true;

            //dmg
            if (await Cast(ASPHYXIATE, gotTarget && Me.CurrentTarget.IsCasting && !Me.CanInterruptCurrentSpellCast && !spellOnCooldown(ASPHYXIATE) && Range30)) return true;
            if (await Cast(REMORSELESS_WINTER, gotTarget && !spellOnCooldown(REMORSELESS_WINTER) && addCountMelee >= 5 && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(GOREFIEND_GRASP, gotTarget && !spellOnCooldown(GOREFIEND_GRASP) && gorefiendCount > P.myPrefs.Gorefiend && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastGroundSpell(DEFILE, gotTarget && needDefile && Me.CurrentTarget.IsWithinMeleeRange && !Me.IsMoving, Me.CurrentTarget)) return true;
            if (await CastGroundSpell(DEATH_AND_DECAY, gotTarget && needDeathAndDecay && Me.CurrentTarget.IsWithinMeleeRange && !Me.IsMoving, Me.CurrentTarget)) return true;
            if (await Cast(PLAGUE_LEECH, gotTarget && needPlagueLeech && Range30)) return true;
            if (await Cast(OUTBREAK, gotTarget && needOutbreak && Range30)) return true;
            if (await Cast(UNHOLY_BLIGHT, gotTarget && needUnholyBlight && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(SOUL_REAPER, gotTarget && BloodRuneCount >= 1 && needSoulReaper)) return true;
            if (await Cast(BLOOD_BOIL, gotTarget && BloodRuneCount >= 1 && needBloodBoil)) return true;
            if (await Cast(DEATH_COIL, gotTarget && Me.RunicPowerPercent >= 40 && Range30)) return true;
            if (await Cast(DEATH_STRIKE, gotTarget && canCastDeathStrike && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await CastBuff(BLOOD_TAP, needBloodTap)) return true;
            if (await Cast(ICY_TOUCH, gotTarget && !debuffExists(FROST_FEVER, Me.CurrentTarget) && FrostRuneCount >= 1 && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            if (await Cast(PLAGUE_STRIKE, gotTarget && !debuffExists(PLAGUE_STRIKE, Me.CurrentTarget) && UnholyRuneCount >= 1 && Me.CurrentTarget.IsWithinMeleeRange)) return true;


            if (await CannotContinueFight(Me.CurrentTarget, Me.CurrentTarget != null
                && AutoBot
                && Me.CurrentTarget.HealthPercent >= 95
                && !Me.CurrentTarget.IsPlayer
                && lastGuid == Me.CurrentTarget.Guid
                && fightTimer.ElapsedMilliseconds >= 30 * 1000)) return true;

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        public static async Task<bool> FrostRoutine()
        {
            if (!AutoBot && Me.Mounted && !buffExists(TELAARI_TALBUK_INT, Me)) return false;
            if (pullTimer.IsRunning) { pullTimer.Stop(); }
            if (await CastBuff(GIFT_OF_THE_NAARU, Me.HealthPercent <= P.myPrefs.PercentNaaru && !spellOnCooldown(GIFT_OF_THE_NAARU))) return true;
            if (await findTargets(Me.CurrentTarget == null && AllowTargeting && FindTargetsCount >= 1)) return true;
            if (await clearTarget(Me.CurrentTarget != null && AllowTargeting && (Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly))) return true;
            if (await findMeleeAttackers(Me.CurrentTarget != null && AutoBot && AllowTargeting && Me.CurrentTarget.Distance > 10 && MeleeAttackersCount >= 1)) return true;
            if (await MoveToTarget(Me.CurrentTarget != null && AllowMovement && Me.CurrentTarget.Distance > 4.5f)) return true;
            if (await StopMovement(Me.CurrentTarget != null && AllowMovement && Me.CurrentTarget.Distance <= 4.5f && Me.IsMoving)) return true;
            if (await FaceMyTarget(Me.CurrentTarget != null && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;

            // res people
            if (await CastRes(RAISE_ALLY, needResPeople && playerToRes != null, playerToRes)) return true;

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        public static async Task<bool> UnholyRoutine()
        {
            if (!AutoBot && Me.Mounted && !buffExists(TELAARI_TALBUK_INT, Me)) return false;
            if (pullTimer.IsRunning) { pullTimer.Stop(); }
            if (await CastBuff(GIFT_OF_THE_NAARU, Me.HealthPercent <= P.myPrefs.PercentNaaru && !spellOnCooldown(GIFT_OF_THE_NAARU))) return true;
            if (await findTargets(Me.CurrentTarget == null && AllowTargeting && FindTargetsCount >= 1)) return true;
            if (await clearTarget(Me.CurrentTarget != null && AllowTargeting && (Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly))) return true;
            if (await findMeleeAttackers(Me.CurrentTarget != null && AutoBot && AllowTargeting && Me.CurrentTarget.Distance > 10 && MeleeAttackersCount >= 1)) return true;
            if (await MoveToTarget(Me.CurrentTarget != null && AllowMovement && Me.CurrentTarget.Distance > 4.5f)) return true;
            if (await StopMovement(Me.CurrentTarget != null && AllowMovement && Me.CurrentTarget.Distance <= 4.5f && Me.IsMoving)) return true;
            if (await FaceMyTarget(Me.CurrentTarget != null && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;

            // res people
            if (await CastRes(RAISE_ALLY, needResPeople && playerToRes != null, playerToRes)) return true;

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        
    }
}