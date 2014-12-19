using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region methods
using Form1 = DeathKnight.GUI.Form1;
using HKM = DeathKnight.Helpers.HotkeyManager;
using S = DeathKnight.DKSpells.DKSpells;
using CL = DeathKnight.Handlers.CombatLogEventArgs;
using EH = DeathKnight.Handlers.EventHandlers;
using L = DeathKnight.Helpers.Logs;
using T = DeathKnight.Helpers.targets;
using U = DeathKnight.Helpers.Unit;
using UI = DeathKnight.Helpers.UseItems;
using P = DeathKnight.DKSettings.DKPrefs;
using M = DeathKnight.Helpers.Movement;
using I = DeathKnight.Helpers.Interrupts;
#endregion

namespace DeathKnight.Helpers
{
    class Unit
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region ValidUnits
        
        
        public static bool ValidUnit(WoWUnit p, bool showReason = false)
        {
            if (p == null || !p.IsValid)
                return false;

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
    }
}
