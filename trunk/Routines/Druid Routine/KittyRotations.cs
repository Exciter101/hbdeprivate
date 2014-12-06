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
        public static async Task<bool> rotationSelector()
        {
            if (MeIsGuardian && await BearRotationCoroutine()) return true;
            if (MeIsFeral && await FeralRotationCoroutine()) return true;
            if (MeIsBoomkin && await BoomkinRotationCoroutine()) return true;
            if (MeIsResto && await HealingRotationCoroutine()) return true;
            if (MeIsLowbie && await LowbieRotationCoroutine()) return true;
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
            if (await CastGroundSpellTrinket(1, gotTarget && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, gotTarget && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
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
            if (await CastGroundSpellTrinket(1, Me.CurrentTarget != null && P.myPrefs.Trinket1Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
            if (await CastGroundSpellTrinket(2, Me.CurrentTarget != null && P.myPrefs.Trinket2Use && nextTrinketTimeAllowed <= DateTime.Now)) return true;
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
            //if (await CastHeal(NATURES_CURE, dispelTargets, dispelTargets != null)) return true;
            if (await CastHeal(LIFEBLOOM, LifebloomPlayer, LifebloomPlayer != null && !buffExists(LIFEBLOOM, LifebloomPlayer))) return true;
            if (await CastMushroom(WILD_MUSHROOM, LifebloomPlayer, LifebloomPlayer != null && needMushroom)) return true;
            if (await CastHeal(SWIFTMEND, swiftmendPlayers, swiftmendPlayers != null)) return true;
            if (await CastHeal(FORCE_OF_NATURE, fonTarget, fonTarget != null && lastFonGuid != null && (lastFonGuid != fonTarget.Guid || fonTimer <= DateTime.Now))) return true;
            if (await CastHeal(WILD_GROWTH, WildGrowthPlayer, WildGrowthPlayer != null && !spellOnCooldown(WILD_GROWTH))) return true;
            if (await CastHeal(REJUVENATION, RejuvenationPlayer, RejuvenationPlayer != null)) return true;
            if (await CastHeal(REGROWTH, RegrowthPlayer, RegrowthPlayer != null)) return true;
            if (await CastHeal(HEALING_TOUCH, HealingTouchPlayer, HealingTouchPlayer != null)) return true;
            if (await CastDmgSpell(MOONFIRE, lifebloomCurrentTarget, lifebloomCurrentTarget != null
                && (!Me.GroupInfo.IsInParty || (Me.GroupInfo.IsInParty && Me.ManaPercent > 60))
                && lifebloomCurrentTarget.Combat
                && !debuffExists(MOONFIRE, lifebloomCurrentTarget))) return true;
            if (await CastDmgSpell(WRATH, lifebloomCurrentTarget, lifebloomCurrentTarget != null
                 && (!Me.GroupInfo.IsInParty || (Me.GroupInfo.IsInParty && Me.ManaPercent > 90))
                && lifebloomCurrentTarget.Combat)) return true;
            Thread.Sleep(10);
            return false;
        }

        #endregion

        #region healing variables
        public static int WildGrowthCount = 3;
        public static int Wildgrowthhealth = 85;
        public static int TranquilityCount = 4;
        public static int TranquilityHealth = 50;
        public static int RejuvenationHealth = 95;
        public static int RegrowthHealth = 85;
        public static int HaelingTouchHealth = 45;
        public static int SwiftmendCount = 3;
        public static int fonHealth = 75;
        public static int GenesisCount = 3;
        public static int mushroomID = 145205;
        public static WoWGuid lastFonGuid;
        public static DateTime healfonTimer;
        public static void SetNextForceOfNatureAllowed()
        {
            healfonTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 15000);
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
                    && mushroomTarget.Location.Distance(mushroomLocation) > 15) return true;
                return false;
            }
        }
        public static List<WoWPlayer> getPartyMembers()
        {
            List<WoWPlayer> targets = new List<WoWPlayer>();
            targets = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p => p != null
                && p.IsAlive
                && p.Distance <= 40
                && p.IsInMyPartyOrRaid
                && p.InLineOfSpellSight
                && p.InLineOfSight).ToList();
            return targets;
        }

        public static WoWPlayer fonTarget
        {
            get
            {
                if (MeIsSolo && Me.HealthPercent <= fonHealth)
                {
                    lastFonGuid = Me.Guid;
                    SetNextForceOfNatureAllowed();
                    return Me;
                }
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= fonHealth).OrderBy(p => p.HealthPercent).ThenBy(p => p.Distance).ToList();
                if (members.Count() >= SwiftmendCount)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    lastFonGuid = healTarget.Guid;
                    SetNextForceOfNatureAllowed();
                    return healTarget;
                }
                return null;
            }
        }

        public static List<WoWPlayer> Tanks()
        {
            return StyxWoW.Me.GroupInfo.RaidMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank))
                    .Select(p => p.ToPlayer())
                    .Union(new[] { RaFHelper.Leader })
                    .Where(p => p != null && p.IsDead)
                    .Distinct()
                    .ToList();
        }
        public static WoWUnit lifebloomCurrentTarget
        {
            get
            {
                if (MeIsSolo && Me.GotTarget && ValidUnit(Me.CurrentTarget)) return Me.CurrentTarget;

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
                return null;
            }
        }
        public static WoWPlayer LifebloomPlayer
        {
            get
            {
                if (MeIsSolo) return Me;
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
                return null;
            }
        }
        public static WoWPlayer swiftmendPlayers
        {
            get
            {
                if (MeIsSolo) return null;
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => buffExists(REJUVENATION, p) || buffExists(REGROWTH, p)).OrderBy(p => p.HealthPercent).ThenBy(p => p.Distance).ToList();
                if (members.Count() >= SwiftmendCount)
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
                if (MeIsSolo) return null;
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= Wildgrowthhealth).OrderBy(p => p.HealthPercent).ThenBy(p => p.Distance).ToList();
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
                if (MeIsSolo && !buffExists(REJUVENATION, Me) && Me.HealthPercent <= RejuvenationHealth) return Me;
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= RejuvenationHealth && !buffExists(REJUVENATION, p)).OrderBy(p => p.HealthPercent).ThenBy(p => p.Distance).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                return null;
            }
        }
        public static WoWPlayer RegrowthPlayer
        {
            get
            {
                if (MeIsSolo && Me.HealthPercent <= RegrowthHealth) return Me;
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= RegrowthHealth).OrderBy(p => p.HealthPercent).ThenBy(p => p.Distance).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                return null;
            }
        }
        public static WoWPlayer HealingTouchPlayer
        {
            get
            {
                if (MeIsSolo && Me.HealthPercent <= HaelingTouchHealth) return Me;
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p.HealthPercent <= HaelingTouchHealth).OrderBy(p => p.HealthPercent).ThenBy(p => p.Distance).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                return null;
            }
        }
        public static WoWPlayer dispelTargets
        {
            get
            {
                if (MeIsSolo && needDispel(Me)) return Me;
                List<WoWPlayer> members = new List<WoWPlayer>();
                members = getPartyMembers().Where(p => p != null && needDispel(p)).OrderBy(p => p.Distance).ToList();
                if (members.Count() > 0)
                {
                    WoWPlayer healTarget = members.FirstOrDefault();
                    return healTarget;
                }
                return null;
            }
        }

        public static bool needDispel(WoWPlayer unit)
        {
            if (debuffExists("Magic", unit)
                || debuffExists("Curse", unit)
                || debuffExists("Poison", unit)) return true;
            return false;

        }
        #endregion

        #region healing spellcasts
        public static async Task<bool> CastHeal(string Spell, WoWPlayer myTarget, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
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
        #endregion
    }
}