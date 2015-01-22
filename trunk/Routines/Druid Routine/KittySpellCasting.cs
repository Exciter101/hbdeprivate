using Buddy.Coroutines;
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
using P = Kitty.KittySettings;
using HKM = Kitty.KittyHotkeyManagers;
namespace Kitty
{
    public partial class KittyMain : CombatRoutine
    {
        public static int rndInterrupt
        {
            get
            {
                Random random = new Random();
                int randomNumber = random.Next(1000, 1501);
                return randomNumber;
            }
        }
        public static DateTime _interruptTimer;

        public static WoWGuid lastInterruptCheck;

        public static DateTime snareTimer;
        private static async Task<bool> CastGroundSpell(string spell, bool reqs)
        {
            if (!reqs) return false;
            // If we cannot cast the spell, obviously
            // we're not going to take any actions, so just return false
            if (!SpellManager.CanCast(spell))
                return false;

            // If the spell cast fails for whatever other reason
            // then we can't do anything either.
            if (!SpellManager.Cast(spell))
                return false;

            // 'Wait' waits until the condition becomes true or the timeout period has elapsed.
            // False is returned if the waiting condition is false for the entire
            // timeout period (here 1000 milliseconds).
            if (!await Coroutine.Wait(1000, () => StyxWoW.Me.CurrentPendingCursorSpell != null))
            {
                Logging.WriteDiagnostic("Cursor didn't turn into the spell!");
                return false;
            }

            SpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);

            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        private static async Task<bool> CastGroundSpellTrinket(int trinket, bool reqs)
        {
            if (!reqs) return false;
            if (trinket == 1)
            {
                var Trinket1 = StyxWoW.Me.Inventory.Equipped.Trinket1;
                if (Trinket1 != null
                    && CanUseEquippedItem(Trinket1))
                {
                    Trinket1.Use();
                    Logging.Write(Colors.OrangeRed, "Using 1st Trinket");
                }
                if (!await Coroutine.Wait(1000, () => StyxWoW.Me.CurrentPendingCursorSpell != null))
                {
                    Logging.WriteDiagnostic("Cursor didn't turn into the spell!");
                    return false;
                }
            }
            if (trinket == 2)
            {
                var Trinket2 = StyxWoW.Me.Inventory.Equipped.Trinket2;
                if (Trinket2 != null
                    && CanUseEquippedItem(Trinket2))
                {
                    Trinket2.Use();
                    Logging.Write(Colors.OrangeRed, "Using 2nd Trinket");
                }
                if (!await Coroutine.Wait(1000, () => StyxWoW.Me.CurrentPendingCursorSpell != null))
                {
                    Logging.WriteDiagnostic("Cursor didn't turn into the spell!");
                    return false;
                }
            }
            SpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
            SetNextNextTrinketTimeAllowed();
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> Cast(string Spell, bool reqs, WoWUnit target)
        {
            if (!reqs) return false;
            if (!SpellManager.HasSpell(Spell)) return false;
            if (spellOnCooldown(Spell)) return false;
            if (!SpellManager.CanCast(Spell, target)) return false;
            if (!SpellManager.Cast(Spell, target)) return false;

            if (Spell == FORCE_OF_NATURE) { fonTimer = DateTime.Now + new TimeSpan(0, 0, 0, 16, 0); }
            if (Spell == MIGHTY_BASH) { stunTimer = DateTime.Now + new TimeSpan(0, 0, 0, 3, 0); }
            if (Spell == WAR_STOMP) { stunTimer = DateTime.Now + new TimeSpan(0, 0, 0, 3, 0); }

            Logging.Write(Colors.Yellow, "Casting: " + Spell + " on: " + target.SafeName);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastBuff(string Spell, bool reqs)
        {
            if (!reqs) return false;
            if (!SpellManager.HasSpell(Spell)) return false;
            if (spellOnCooldown(Spell)) return false;
            if (!SpellManager.CanCast(Spell, Me)) return false;
            if (!SpellManager.Cast(Spell, Me)) return false;

            if (Spell == DASH || Spell == STAMPEDING_ROAR) { snareTimer = DateTime.Now + new TimeSpan(0, 0, 0, 5, 0); }

            SpellManager.Cast(Spell, Me);
            Logging.Write(Colors.LightSeaGreen, "Casting: " + Spell + " on: " + Me.SafeName);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastMultiDot(string Spell, WoWUnit target, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (!SpellManager.CanCast(Spell, target)) return false;
            if (!SpellManager.Cast(Spell, target)) return false;
            Logging.Write(Colors.Yellow, "Multi-Dot: " + Spell + " on: " + target.SafeName);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> GetRandomInterruptTimer(int rnd, bool reqs)
        {
            if (!reqs) return false;
            if (lastGuid == Me.CurrentTarget.Guid) return false;
            _interruptTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, rnd);
            lastGuid = guid;
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> stopPullTimer(bool reqs)
        {
            if (!reqs) return false;
            pullTimer.Stop();
            Logging.Write(Colors.CornflowerBlue, "Stopping PullTimer => We Are In Combat");
            fightTimer.Restart();
            Logging.Write(Colors.CornflowerBlue, "FightTimer started !");
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> blackListingUnit(bool reqs, WoWUnit unit)
        {
            if (!reqs) return false;
            if (unit.IsPlayer) return false;
            if (unit.HealthPercent < 95) return false;
            Blacklist.Add(unit, BlacklistFlags.Combat, new TimeSpan(0, 0, 0, 30, 0));
            Logging.Write(Colors.Red, "This is a bugged mob, Blacklisting for 30 sec !");
            fightTimer.Stop();
            pullTimer.Stop();
            Me.ClearTarget();
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }

        public static async Task<bool> UseItem(int itemID, bool reqs)
        {
            if (!reqs) return false;
            WoWItem potion = Me.BagItems.FirstOrDefault(h => h.Entry == itemID);
            if (potion == null || potion.CooldownTimeLeft.TotalMilliseconds > 0)
            {
                return false;
            }
            potion.Use();
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }

        public static async Task<bool> RemoveRooted(string myForm, bool reqs)
        {
            if (!reqs) return false;
            Logging.Write(Colors.LightPink, "Shapeshifting Cause Rooted");
            Lua.DoString("RunMacroText(\"/cancelaura " + myForm + "\")");
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        private static DateTime nextTrinketTimeAllowed;
        public static void SetNextNextTrinketTimeAllowed()
        {
            nextTrinketTimeAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 10000);
        }
        public static async Task<bool> NeedTrinket1(bool reqs)
        {
            if (!reqs) return false;
            var Trinket1 = StyxWoW.Me.Inventory.Equipped.Trinket1;

            if (Trinket1 != null
                && CanUseEquippedItem(Trinket1))
            {
                Trinket1.Use();
                Logging.Write(Colors.OrangeRed, "Using 1st Trinket");
                SetNextNextTrinketTimeAllowed();
            }
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> NeedTrinket2(bool reqs)
        {
            if (!reqs) return false;
            var Trinket2 = StyxWoW.Me.Inventory.Equipped.Trinket2;

            if (Trinket2 != null
                && CanUseEquippedItem(Trinket2))
            {
                Trinket2.Use();
                Logging.Write(Colors.OrangeRed, "Using 2nd Trinket");
                SetNextNextTrinketTimeAllowed();
            }
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
    }
}
