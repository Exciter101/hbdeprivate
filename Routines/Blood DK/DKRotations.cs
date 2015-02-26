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
            if (!AutoBot && Me.Mounted && Me.IsMoving) return false;
            if (HKM.pauseRoutineOn || HKM.manualOn) return false;

            if (Me.Combat && !combatTimerIsRunning) await StartCombatTimer();

            if (Me.CurrentTarget != null
                && combatTimerIsRunning
                && lastGuid == Me.CurrentTargetGuid 
                && Me.CurrentTarget.HealthPercent >= 90 
                && DateTime.Now >= combatTimer) await BlacklistingCombatMob();
            
            if (buffExists("Hand of Protection", Me)) { Lua.DoString("RunMacroText(\"/cancelaura Hand Of Protection\")"); }

            if (await CastBuff(GIFT_OF_THE_NAARU, Me.HealthPercent <= P.myPrefs.PercentNaaru && !spellOnCooldown(GIFT_OF_THE_NAARU))) return true;
            if (await UseItem(HEALTHSTONE_ITEM, Me.HealthPercent <= P.myPrefs.PercentHealthstone)) return true;
            if (await NeedTrinket1(P.myPrefs.Trinket1HP > 0 && Me.HealthPercent <= P.myPrefs.Trinket1HP)) return true;
            if (await NeedTrinket2(P.myPrefs.Trinket2HP > 0 && Me.HealthPercent <= P.myPrefs.Trinket2HP)) return true;

            if (DKSettings.myPrefs.AutoMovement) await Movement.MoveToCombatTarget();
            if (DKSettings.myPrefs.AutoFacing) await Movement.FaceMyCurrentTarget();
            if (await Movement.ClearMyDeadTarget()) return true;

            // res people
            if (await CastRes(RAISE_ALLY, lstSpell != RAISE_ALLY
                && !spellOnCooldown(RAISE_ALLY)
                && HKM.resTanks
                && TankToRes != null
                && Me.RunicPowerPercent >= 30, TankToRes)) return true;
            if (await CastRes(RAISE_ALLY, lstSpell != RAISE_ALLY
                && !spellOnCooldown(RAISE_ALLY)
                && HKM.resHealers
                && HealerToRes != null
                && Me.RunicPowerPercent >= 30, HealerToRes)) return true;
            if (await CastRes(RAISE_ALLY, lstSpell != RAISE_ALLY
                && !spellOnCooldown(RAISE_ALLY)
                && HKM.resDPS
                && DpsToRes != null
                && Me.RunicPowerPercent >= 30, DpsToRes)) return true;

            //interrupt
            if (await Cast(MIND_FREEZE, gotTarget 
                && DKSettings.myPrefs.AutoInterrupt
                && Me.CurrentTarget.IsCasting 
                && Me.CanInterruptCurrentSpellCast 
                && !spellOnCooldown(MIND_FREEZE) 
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await Cast(ASPHYXIATE, gotTarget
                && DKSettings.myPrefs.AutoInterrupt
                && lstSpell != ASPHYXIATE
                && Me.CurrentTarget.IsCasting
                && Me.CanInterruptCurrentSpellCast
                && !spellOnCooldown(ASPHYXIATE) && Range30)) return true;

            if (await Cast(STRANGULATE, gotTarget
                && DKSettings.myPrefs.AutoInterrupt
                && lstSpell != STRANGULATE
                && Me.CurrentTarget.IsCasting
                && Me.CanInterruptCurrentSpellCast
                && !spellOnCooldown(STRANGULATE)
                && Range30)) return true;

            //protection
            if (await CastBuff(BONE_SHIELD, lstSpell != BONE_SHIELD 
                && !buffExists(BONE_SHIELD, Me) 
                && !spellOnCooldown(BONE_SHIELD))) return true;

            if (await CastBuff(ANTI_MAGIC_SHELL, gotTarget 
                && lstSpell != ANTI_MAGIC_SHELL 
                && Me.CurrentTarget.IsCasting 
                && !spellOnCooldown(ANTI_MAGIC_SHELL) 
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await CastBuff(ICEBOUND_FORTITUDE, gotTarget 
                && lstSpell != ICEBOUND_FORTITUDE 
                && Me.HealthPercent < P.myPrefs.IceBoundFortitude 
                && !spellOnCooldown(ICEBOUND_FORTITUDE) 
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await Cast(DANCING_RUNE_WEAPON, gotTarget 
                && lstSpell != DANCING_RUNE_WEAPON 
                && Me.HealthPercent < P.myPrefs.DancingRuneWeapon 
                && !spellOnCooldown(DANCING_RUNE_WEAPON) 
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await CastBuff(BLOOD_TAP, needBloodTap
                && lstSpell != BLOOD_TAP)) return true;

            if (await CastBuff(VAMPIRIC_BLOOD, gotTarget 
                && lstSpell != VAMPIRIC_BLOOD 
                && Me.HealthPercent < P.myPrefs.VampiricBlood 
                && !spellOnCooldown(VAMPIRIC_BLOOD) 
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await CastBuff(EMPOWER_RUNE_WEAPON, gotTarget 
                && lstSpell != EMPOWER_RUNE_WEAPON 
                && needEmpoweredRuneWeapon 
                && !spellOnCooldown(EMPOWER_RUNE_WEAPON) 
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;
            
            if (await CastBuff(RUNE_TAP, gotTarget 
                && lstSpell != RUNE_TAP 
                && Me.HealthPercent < P.myPrefs.RuneTap 
                && !spellOnCooldown(RUNE_TAP))) return true;

            if (await CastBuff(DEATH_PACT, gotTarget 
                && lstSpell != DEATH_PACT 
                && Me.HealthPercent < P.myPrefs.DeathPact 
                && !spellOnCooldown(DEATH_PACT))) return true;

            if (await CastBuff(CONVERSION, gotTarget 
                && needConversion)) return true;

            if (await CastBuff(HORN_OF_WINTER, gotTarget 
                && lstSpell != HORN_OF_WINTER 
                && !buffExists(HORN_OF_WINTER, Me))) return true;

            //running away mobs
            if (await Cast(CHAINS_OF_ICE, gotTarget 
                && lstSpell != CHAINS_OF_ICE 
                && Me.IsSafelyBehind(Me.CurrentTarget) 
                && !spellOnCooldown(CHAINS_OF_ICE) 
                && !Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await Cast(DEATH_COIL, gotTarget 
                && Me.IsSafelyBehind(Me.CurrentTarget) 
                && Me.RunicPowerPercent >= 30 
                && Range30)) return true;

            //diseases
            if (await Cast(OUTBREAK, gotTarget
                && needOutbreak
                && Range30
                && lstSpell != OUTBREAK)) return true;

            if (await Cast(UNHOLY_BLIGHT, gotTarget
                && needUnholyBlight
                && lstSpell != UNHOLY_BLIGHT
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await Cast(ICY_TOUCH, gotTarget
                && lstSpell != ICY_TOUCH
                && lstSpell != OUTBREAK
                && lstSpell != UNHOLY_BLIGHT
                && !debuffExists(FROST_FEVER, Me.CurrentTarget)
                && FrostRuneCount >= 1 && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await Cast(PLAGUE_STRIKE, gotTarget
                && lstSpell != PLAGUE_STRIKE
                && lstSpell != OUTBREAK
                && lstSpell != UNHOLY_BLIGHT
                && !debuffExists(PLAGUE_STRIKE, Me.CurrentTarget)
                && UnholyRuneCount >= 1 && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await Cast(REMORSELESS_WINTER, gotTarget 
                && lstSpell != REMORSELESS_WINTER 
                && !spellOnCooldown(REMORSELESS_WINTER) 
                && addCountMelee >= 5 && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await Cast(GOREFIEND_GRASP, gotTarget 
                && lstSpell != GOREFIEND_GRASP 
                && !spellOnCooldown(GOREFIEND_GRASP) 
                && gorefiendCount > P.myPrefs.Gorefiend 
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            if (await CastGroundSpell(DEFILE, gotTarget
                && (UnholyRuneCount >= 1 || DeathRuneCount >= 3)
                && lstSpell != DEFILE 
                && Me.CurrentTarget.IsWithinMeleeRange 
                && !Me.IsMoving, Me.CurrentTarget)) return true;

            if (await CastGroundSpell(DEATH_AND_DECAY, gotTarget 
                && addCountMelee >= P.myPrefs.AddsDeathAndDecay
                && (UnholyRuneCount >= 1 || DeathRuneCount >= 3)
                && lstSpell != DEATH_AND_DECAY 
                && Me.CurrentTarget.IsWithinMeleeRange 
                && !Me.IsMoving, Me.CurrentTarget)) return true;

            if (await Cast(PLAGUE_LEECH, gotTarget 
                && needPlagueLeech 
                && Range30 
                && lstSpell != PLAGUE_LEECH)) return true;

            if (await Cast(SOUL_REAPER, gotTarget 
                && (BloodRuneCount >= 1 || Me.HealthPercent > P.myPrefs.DeathStrikeHP)
                && Me.CurrentTarget.HealthPercent < 35 
                && lstSpell != SOUL_REAPER)) return true;

            if (await Cast(BLOOD_BOIL, gotTarget 
                && (BloodRuneCount >= 1 || IsOverlayed(BLOOD_BOIL_INT))
                && lstSpell != BLOOD_BOIL)) return true;

            if (await Cast(DEATH_COIL, gotTarget 
                && Me.RunicPowerPercent >= 40 
                && Range30 
                && lstSpell != DEATH_COIL)) return true;

            if (await Cast(DEATH_STRIKE, gotTarget
                && SpellManager.CanCast(DEATH_STRIKE)
                && (Me.HealthPercent <= P.myPrefs.DeathStrikeHP || DeathRuneCount >= 3)
                && Me.CurrentTarget.IsWithinMeleeRange)) return true;

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        public static async Task<bool> FrostRoutine()
        {
            if (!AutoBot && Me.Mounted && !buffExists(TELAARI_TALBUK_INT, Me)) return false;
            if (pullTimer.IsRunning) { pullTimer.Stop(); }
            if (await CastBuff(GIFT_OF_THE_NAARU, Me.HealthPercent <= P.myPrefs.PercentNaaru && !spellOnCooldown(GIFT_OF_THE_NAARU))) return true;
            
            // res people

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        public static async Task<bool> UnholyRoutine()
        {
            if (!AutoBot && Me.Mounted && !buffExists(TELAARI_TALBUK_INT, Me)) return false;
            if (pullTimer.IsRunning) { pullTimer.Stop(); }
            if (await CastBuff(GIFT_OF_THE_NAARU, Me.HealthPercent <= P.myPrefs.PercentNaaru && !spellOnCooldown(GIFT_OF_THE_NAARU))) return true;
            
            // res people

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        
    }
}