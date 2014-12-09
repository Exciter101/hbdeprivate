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

namespace Kitty
{
    public partial class KittyMain : CombatRoutine
    {
        public static string usedBot { get { return BotManager.Current.Name.ToUpper(); } }

        #region solo
        public static bool MeIsSolo
        {
            get { return !Me.GroupInfo.IsInParty && !Me.GroupInfo.IsInRaid && !Me.GroupInfo.IsInLfgParty && !Me.GroupInfo.IsInBattlegroundParty; }
        }
        #endregion

        #region gotTarget
        public static bool gotTarget
        {
            get
            {
                return Me.CurrentTarget != null && Me.CurrentTarget.IsAlive && Me.CurrentTarget.Attackable && Me.CurrentTarget.CanSelect;
            }
        }
        #endregion

        #region can buff and eat
        public static bool Canbuff
        {
            get
            {
                return !Me.Mounted && !Me.IsFlying && !Me.OnTaxi && !Me.IsDead && !Me.IsGhost;
            }
        }
        #endregion

        #region AutoBot
        public static bool AutoBot
        {
            get
            {
                return usedBot.Contains("QUEST") || usedBot.Contains("GRIND") || usedBot.Contains("GATHER") || usedBot.Contains("ANGLER") || usedBot.Contains("ARCHEO");

            }
        }
        #endregion

        #region addcount
        public static List<WoWUnit> UnfriendlyTargets()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(u => u.IsAlive
                && u.Combat
                && (u.IsTargetingMeOrPet || u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember)
                && u.Distance <= 8 * 8).ToList();
        }

        public static int addCount
        {
            get { return UnfriendlyTargets().Count(); }
        }
        #endregion

        #region findTarget

        public static List<WoWUnit> FindTarget()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(u => u != null
                && ValidUnit(u)
                && !Blacklist.Contains(u, BlacklistFlags.All))
                .OrderBy(u => u.HealthPercent)
                .ThenBy(u => u.Distance).ToList();
        }
        public static int FindTargetsCount { get { return FindTarget().Count(); } }

        public static async Task<bool> findTargets(bool reqs)
        {
            if (!reqs) return false;
            if (Me.CurrentTarget != null && Me.CurrentTarget.IsPet && Me.CurrentTarget.OwnedByRoot.IsPlayer)
            {
                Blacklist.Add(Me.CurrentTarget, BlacklistFlags.All, TimeSpan.FromDays(1)); return false;
            }
            WoWUnit unit = FindTarget().FirstOrDefault();
            Logging.Write(Colors.CornflowerBlue, "Found new Target " + unit.SafeName);
            unit.Target();
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> clearTarget(bool reqs)
        {
            if (!reqs) return false;
            Logging.Write(Colors.CornflowerBlue, "Clearing Dead Target " + Me.CurrentTarget.SafeName);
            Me.ClearTarget();
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        #endregion

        #region MeleeAttackers
        public static List<WoWUnit> MeleeAttackers()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(u => u != null
                && (u.Combat
                && (u.IsTargetingMeOrPet || u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember || u.IsTargetingAnyMinion))
                && ValidUnit(u)
                && !Blacklist.Contains(u, BlacklistFlags.All)
                && u.Distance < 10).OrderBy(u => u.Distance).ToList();
        }
        public static int MeleeAttackersCount { get { return MeleeAttackers().Count(); } }

        public static async Task<bool> findMeleeAttackers(bool reqs)
        {
            if (!reqs) return false;
            if (Me.CurrentTarget != null && Me.CurrentTarget.IsPet && Me.CurrentTarget.OwnedByRoot.IsPlayer)
            {
                Blacklist.Add(Me.CurrentTarget, BlacklistFlags.All, TimeSpan.FromDays(1)); return false;
            }
            WoWUnit unit = MeleeAttackers().FirstOrDefault();
            Logging.Write(Colors.CornflowerBlue, "Changing target to closest attacker " + unit.SafeName);
            unit.Target();
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        #endregion

        #region MeleeRange
        public static bool IsInMeleeRange(WoWUnit unit) { return unit != null && unit.Distance <= 4.5f; }
        #endregion

        #region valid unit
        public static bool ValidUnit(WoWUnit p, bool showReason = false)
        {
            if (p == null || !p.IsValid)
                return false;

            if (Blacklist.Contains(p, BlacklistFlags.All) && p.Combat && (p.IsTargetingMeOrPet || p.IsTargetingAnyMinion || p.IsTargetingMyPartyMember
                || p.IsTargetingMyRaidMember)) return true;

            // Ignore shit we can't select
            if (!p.CanSelect)
            {
                return false;
            }

            // Ignore shit we can't attack
            if (!p.Attackable)
            {
                return false;
            }

            // Duh
            if (p.IsDead)
            {
                return false;
            }

            // check for enemy players here as friendly only seems to work on npc's
            if (p.IsPlayer)
            {
                WoWPlayer pp = p.ToPlayer();
                if (pp.IsHorde == StyxWoW.Me.IsHorde)
                {
                    return false;
                }

                if (pp.Guid == StyxWoW.Me.CurrentTargetGuid && !CanAttackCurrentTarget)
                {
                    return false;
                }

                return true;
            }

            // Ignore friendlies!
            if (p.IsFriendly)
            {
                return false;
            }

            // If it is a pet/minion/totem, lets find the root of ownership chain
            WoWPlayer pOwner = GetPlayerParent(p);

            // ignore if owner is player, alive, and not blacklisted then ignore (since killing owner kills it)
            // .. following .IsMe check to prevent treating quest mob summoned by us that we need to kill as invalid 
            if (pOwner != null && pOwner.IsAlive && !pOwner.IsMe)
            {
                if (!ValidUnit(pOwner))
                {
                    return false;
                }
                if (!Blacklist.Contains(pOwner, BlacklistFlags.Combat))
                {
                    return false;
                }
                if (!StyxWoW.Me.IsPvPFlagged)
                {
                    return false;
                }
            }

            // And ignore non-combat pets
            if (p.IsNonCombatPet)
            {
                return false;
            }

            // And ignore critters (except for those ferocious ones or if set as BotPoi)
            if (IsIgnorableCritter(p))
            {
                return false;
            }
            return true;
        }
        public static bool CanAttackCurrentTarget
        {
            get
            {
                if (StyxWoW.Me.CurrentTarget == null)
                    return false;

                return Lua.GetReturnVal<bool>("return UnitCanAttack(\"player\",\"target\")", 0);
            }
        }
        public static bool IsIgnorableCritter(WoWUnit u)
        {
            if (!u.IsCritter)
                return false;

            // good enemy if BotPoi
            if (Styx.CommonBot.POI.BotPoi.Current.Guid == u.Guid && Styx.CommonBot.POI.BotPoi.Current.Type == Styx.CommonBot.POI.PoiType.Kill)
                return false;

            // good enemy if Targeting
            if (Targeting.Instance.TargetList.Contains(u))
                return false;

            // good enemy if Threat towards us
            if (u.ThreatInfo.ThreatValue != 0 && u.IsTargetingMyRaidMember)
                return false;

            // Nah, just a harmless critter
            return true;
        }
        public static WoWPlayer GetPlayerParent(WoWUnit unit)
        {
            // If it is a pet/minion/totem, lets find the root of ownership chain
            WoWUnit pOwner = unit;
            while (true)
            {
                if (pOwner.OwnedByUnit != null)
                    pOwner = pOwner.OwnedByRoot;
                else if (pOwner.CreatedByUnit != null)
                    pOwner = pOwner.CreatedByUnit;
                else if (pOwner.SummonedByUnit != null)
                    pOwner = pOwner.SummonedByUnit;
                else
                    break;
            }

            if (unit != pOwner && pOwner.IsPlayer)
                return pOwner as WoWPlayer;

            return null;
        }
        #endregion

        #region Buff Checks

        public static bool buffExists(int Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraById(Buff);
                if (Results != null)
                    return true;
            }
            return false;
        }

        public static double buffTimeLeft(int Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraById(Buff);
                if (Results != null)
                {
                    if (Results.TimeLeft.TotalMilliseconds > 0)
                        return Results.TimeLeft.TotalMilliseconds;
                }
            }
            return 0;
        }

        public static bool buffExists(string Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraByName(Buff);
                if (Results != null)
                    return true;
            }
            return false;
        }

        public static double buffTimeLeft(string Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraByName(Buff);
                if (Results != null)
                {
                    if (Results.TimeLeft.TotalMilliseconds > 0)
                        return Results.TimeLeft.TotalMilliseconds;
                }
            }
            return 0;
        }



        public static uint buffStackCount(int Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraById(Buff);
                if (Results != null)
                    return Results.StackCount;
            }
            return 0;
        }
        public static uint buffStackCount(string Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraByName(Buff);
                if (Results != null)
                    return Results.StackCount;
            }
            return 0;
        }
        #endregion

        #region Cache Checks
        public static IEnumerable<WoWAura> cachedAuras = new List<WoWAura>();
        public static IEnumerable<WoWAura> cachedTargetAuras = new List<WoWAura>();
        public static void getCachedAuras()
        {
            if (Me.CurrentTarget != null)
                cachedTargetAuras = Me.CurrentTarget.GetAllAuras();
            cachedAuras = Me.GetAllAuras();
        }
        #endregion

        #region Cooldown Checks
        private static Dictionary<WoWSpell, long> Cooldowns = new Dictionary<WoWSpell, long>();
        public static TimeSpan cooldownLeft(int Spell)
        {
            SpellFindResults Results;
            if (SpellManager.FindSpell(Spell, out Results))
            {
                WoWSpell Result = Results.Override ?? Results.Original;
                if (Cooldowns.TryGetValue(Result, out lastUsed))
                {
                    if (DateTime.Now.Ticks < lastUsed)
                        return Result.CooldownTimeLeft;
                    return TimeSpan.MaxValue;
                }
            }
            return TimeSpan.MaxValue;
        }
        public static TimeSpan cooldownLeft(string Spell)
        {
            SpellFindResults Results;
            if (SpellManager.FindSpell(Spell, out Results))
            {
                WoWSpell Result = Results.Override ?? Results.Original;
                if (Cooldowns.TryGetValue(Result, out lastUsed))
                {
                    if (DateTime.Now.Ticks < lastUsed)
                        return Result.CooldownTimeLeft;
                    return TimeSpan.MaxValue;
                }
            }
            return TimeSpan.MaxValue;
        }
        private static long lastUsed;
        public static int lastCast;
        public static bool onCooldown(int Spell)
        {
            SpellFindResults Results;
            if (SpellManager.FindSpell(Spell, out Results))
            {
                WoWSpell Result = Results.Override ?? Results.Original;
                if (Cooldowns.TryGetValue(Result, out lastUsed))
                {
                    if (DateTime.Now.Ticks < lastUsed)
                        return Result.Cooldown;
                    return false;
                }
            }
            return false;
        }
        public static bool onCooldown(string Spell)
        {
            SpellFindResults Results;
            if (SpellManager.FindSpell(Spell, out Results))
            {
                WoWSpell Result = Results.Override ?? Results.Original;
                if (Cooldowns.TryGetValue(Result, out lastUsed))
                {
                    if (DateTime.Now.Ticks < lastUsed)
                        return Result.Cooldown;
                    return false;
                }
            }
            return false;
        }
        public static TimeSpan spellCooldownLeft(int Spell, bool useTracker = false)
        {
            if (useTracker)
                return cooldownLeft(Spell);
            SpellFindResults results;
            if (SpellManager.FindSpell(Spell, out results))
            {
                if (results.Override != null)
                    return results.Override.CooldownTimeLeft;
                return results.Override.CooldownTimeLeft;
            }
            return TimeSpan.MaxValue;
        }
        public static TimeSpan spellCooldownLeft(string Spell, bool useTracker = false)
        {
            if (useTracker)
                return cooldownLeft(Spell);
            SpellFindResults results;
            if (SpellManager.FindSpell(Spell, out results))
            {
                if (results.Override != null)
                    return results.Override.CooldownTimeLeft;
                return results.Override.CooldownTimeLeft;
            }
            return TimeSpan.MaxValue;
        }
        public static bool spellOnCooldown(int Spell, bool useTracker = false)
        {
            if (useTracker)
                return !onCooldown(Spell);
            SpellFindResults results;
            if (SpellManager.FindSpell(Spell, out results))
                return results.Override != null ? results.Override.Cooldown : results.Original.Cooldown;
            return false;
        }
        public static bool spellOnCooldown(string Spell, bool useTracker = false)
        {
            if (useTracker)
                return !onCooldown(Spell);
            SpellFindResults results;
            if (SpellManager.FindSpell(Spell, out results))
                return results.Override != null ? results.Override.Cooldown : results.Original.Cooldown;
            return false;
        }
        #endregion

        #region Debuff Checks

        public static bool debuffExists(int Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a => a.SpellId == Debuff && a.CreatorGuid == Me.Guid);
                if (aura != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static double debuffTimeLeft(int Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a =>
                    a.SpellId == Debuff
                    && a.CreatorGuid == Me.Guid);

                if (aura == null)
                {
                    return 0;
                }
                return aura.TimeLeft.TotalMilliseconds;
            }
            return 0;
        }

        public static uint debuffStackCount(int Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a =>
                    a.SpellId == Debuff
                    && a.CreatorGuid == Me.Guid);

                if (aura == null)
                {
                    return 0;
                }
                return aura.StackCount;
            }
            return 0;
        }
        public static bool debuffExists(string Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a => a.Name == Debuff && a.CreatorGuid == Me.Guid);
                if (aura != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static double debuffTimeLeft(string Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a =>
                    a.Name == Debuff
                    && a.CreatorGuid == Me.Guid);

                if (aura == null)
                {
                    return 0;
                }
                return aura.TimeLeft.TotalMilliseconds;
            }
            return 0;
        }

        public static uint debuffStackCount(string Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a =>
                    a.Name == Debuff
                    && a.CreatorGuid == Me.Guid);

                if (aura == null)
                {
                    return 0;
                }
                return aura.StackCount;
            }
            return 0;
        }
        #endregion

        #region Timer Checks
        public static bool isTargetDummy
        {
            get
            {
                return Me.CurrentTarget.Name.Contains("Dummy");
            }
        }
        private static int conv_Date2Timestam(DateTime _time)
        {
            var date1 = new DateTime(1970, 1, 1);
            DateTime date2 = _time;
            var ts = new TimeSpan(date2.Ticks - date1.Ticks);
            return (Convert.ToInt32(ts.TotalSeconds));
        }
        private static uint current_life;
        private static int current_time;
        private static WoWGuid guid;
        private static uint first_life;
        private static uint first_life_max;
        private static int first_time;
        public static long targetExistence(WoWUnit onTarget)
        {
            if (onTarget == null) return 0;
            if (isTargetDummy) return 9999;
            if (onTarget.CurrentHealth == 0 || onTarget.IsDead || !onTarget.IsValid || !onTarget.IsAlive)
                return 0;
            if (guid != onTarget.Guid)
            {
                guid = onTarget.Guid;
                first_life = onTarget.CurrentHealth;
                first_life_max = onTarget.MaxHealth;
                first_time = conv_Date2Timestam(DateTime.Now);
            }
            current_life = onTarget.CurrentHealth;
            current_time = conv_Date2Timestam(DateTime.Now);
            var time_diff = current_time - first_time;
            var hp_diff = first_life - current_life;
            if (hp_diff > 0)
            {
                var full_time = time_diff * first_life_max / hp_diff;
                var past_first_time = (first_life_max - first_life) * time_diff / hp_diff;
                var calc_time = first_time - past_first_time + full_time - current_time;
                if (calc_time < 1) calc_time = 99;
                var time_to_die = calc_time;
                var fight_length = full_time;
                return time_to_die;
            }
            if (hp_diff < 0)
            {
                guid = onTarget.Guid;
                first_life = onTarget.CurrentHealth;
                first_life_max = onTarget.MaxHealth;
                first_time = conv_Date2Timestam(DateTime.Now);
                return -1;
            }
            if (current_life == first_life_max)
                return 9999;
            return -1;
        }
        #endregion

        #region crowdcontrol

        public static bool IsCrowdControlledTarget(WoWUnit unit)
        {
            Dictionary<string, WoWAura>.ValueCollection auras = unit.Auras.Values;

            return unit.Fleeing
                || HasAuraWithEffect(unit,
                        WoWApplyAuraType.ModConfuse,
                        WoWApplyAuraType.ModCharm,
                        WoWApplyAuraType.ModFear,
                        WoWApplyAuraType.ModPossess);
        }

        public static bool IsCrowdControlledPlayer(WoWUnit unit)
        {
            Dictionary<string, WoWAura>.ValueCollection auras = unit.Auras.Values;

            return unit.Rooted
                || unit.Fleeing
                || HasAuraWithEffect(unit,
                        WoWApplyAuraType.ModConfuse,
                        WoWApplyAuraType.ModCharm,
                        WoWApplyAuraType.ModFear,
                        WoWApplyAuraType.ModDecreaseSpeed,
                        WoWApplyAuraType.ModPacify,
                        WoWApplyAuraType.ModPacifySilence,
                        WoWApplyAuraType.ModPossess,
                        WoWApplyAuraType.ModRoot);
        }
        public static bool IsRooted(WoWUnit unit)
        {
            Dictionary<string, WoWAura>.ValueCollection auras = Me.Auras.Values;

            return unit.Rooted;
        }
        public static bool MeIsRooted
        {
            get
            {
                Dictionary<string, WoWAura>.ValueCollection auras = Me.Auras.Values;
                return HasAuraWithEffect(Me, WoWApplyAuraType.ModRoot);
            }

        }
        // this one optimized for single applytype lookup

        public static bool HasAuraWithEffect(WoWUnit unit, WoWApplyAuraType applyType)
        {
            return unit.Auras.Values.Any(a => a.Spell != null && a.Spell.SpellEffects.Any(se => applyType == se.AuraType));
        }

        public static bool HasAuraWithEffect(WoWUnit unit, params WoWApplyAuraType[] applyType)
        {
            var hashes = new HashSet<WoWApplyAuraType>(applyType);
            return unit.Auras.Values.Any(a => a.Spell != null && a.Spell.SpellEffects.Any(se => hashes.Contains(se.AuraType)));
        }
        #endregion

        #region check targets
        private static Stopwatch fightTimer = new Stopwatch();
        private static Stopwatch pullTimer = new Stopwatch();
        private static Stopwatch moveTimer = new Stopwatch();
        public static WoWPoint lastPoint { get; set; }
        public static void CheckMyCurrentTarget()
        {
            if (Me.CurrentTarget != null && ValidUnit(Me.CurrentTarget) && !pullTimer.IsRunning && Me.CurrentTarget.Guid != lastGuid) 
            { 
                pullTimer.Restart(); 
                lastGuid = Me.CurrentTarget.Guid; 
            }

            if (Me.CurrentTarget != null && ValidUnit(Me.CurrentTarget) && Me.CurrentTarget.Guid == lastGuid && pullTimer.ElapsedMilliseconds > 120 * 1000)
            {
                Logging.Write(Colors.CornflowerBlue, "Cannot pull " + Me.CurrentTarget.Name + " Blacklisting for 3 min.");
                Blacklist.Add(Me.CurrentTarget.Guid, BlacklistFlags.All, TimeSpan.FromMinutes(3.00));
                Me.ClearTarget();
            }

            if (gotTarget && (!fightTimer.IsRunning || Me.CurrentTarget.Guid != lastGuid) && Me.Combat)
            {
                pullTimer.Reset();
                fightTimer.Reset();
                fightTimer.Start();
                lastGuid = Me.CurrentTarget.Guid;
            }
            if (Battlegrounds.IsInsideBattleground)
            {
                if (Me.GotTarget &&
                    Me.CurrentTarget.IsPet)
                {
                    Blacklist.Add(Me.CurrentTarget, BlacklistFlags.All, TimeSpan.FromDays(1));
                    Me.ClearTarget();
                }

                if (Me.GotTarget && Me.CurrentTarget.Mounted)
                {
                    Blacklist.Add(Me.CurrentTarget, BlacklistFlags.All, TimeSpan.FromMinutes(1));
                    Me.ClearTarget();
                }
            }
            if (Me.CurrentTarget != null
                && lastGuid == Me.CurrentTarget.Guid
                && !Me.CurrentTarget.IsPlayer
                && fightTimer.ElapsedMilliseconds > 30 * 1000
                && Me.CurrentTarget.HealthPercent > 95)
            {
                Logging.Write(Colors.CornflowerBlue, " This " + Me.CurrentTarget.Name + " is a bugged mob.  Blacklisting for 30 min.");

                Blacklist.Add(Me.CurrentTarget.Guid, BlacklistFlags.All, TimeSpan.FromMinutes(30));
                Me.ClearTarget();
            }
        }
        #endregion
    }
}
