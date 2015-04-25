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

using P = DK.DKSettings;

namespace DK
{
    public partial class DKMain : CombatRoutine
    {
        private static async Task<bool> CastGroundSpell(string spell, bool reqs, WoWUnit target)
        {
            if (!reqs) return false;
            if (!SpellManager.CanCast(spell)) return false;
            if (!SpellManager.Cast(spell)) return false;

            SpellManager.ClickRemoteLocation(target.Location);
            Logging.Write(Colors.OrangeRed, "Casting:" + spell + " on: " + Me.CurrentTarget.SafeName);
            lstSpell = spell;
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
            }
            SpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
            SetNextNextTrinketTimeAllowed();
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> Cast(string Spell, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (spellOnCooldown(Spell)) return false;
            if (!SpellManager.CanCast(Spell, Me.CurrentTarget)) return false;
            if (!SpellManager.Cast(Spell, Me.CurrentTarget)) return false;
            Logging.Write(Colors.Yellow, "Casting: " + Spell + " on: " + Me.CurrentTarget.SafeName);
            lstSpell = Spell;
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastRes(string Spell, bool reqs, WoWPlayer myTarget)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (spellOnCooldown(Spell)) return false;
            if (!SpellManager.CanCast(Spell, Me.CurrentTarget)) return false;
            if (!SpellManager.Cast(Spell, Me.CurrentTarget)) return false;
            Logging.Write(Colors.Yellow, "Casting: " + Spell + " on: " + myTarget.SafeName);
            lstSpell = Spell;
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastPull(string Spell, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (spellOnCooldown(Spell)) return false;
            if (!SpellManager.CanCast(Spell, Me.CurrentTarget)) return false;
            if (!SpellManager.Cast(Spell, Me.CurrentTarget)) return false;
            pullingTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2500);
            Logging.Write(Colors.LightBlue, "Casting: " + Spell + " on: " + Me.CurrentTarget.SafeName);
            lstSpell = Spell;
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }

        public static async Task<bool> CastBuff(string Spell, bool reqs)
        {
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!reqs) return false;
            if (spellOnCooldown(Spell)) return false;
            if (!SpellManager.CanCast(Spell, Me)) return false;
            if (!SpellManager.Cast(Spell, Me)) return false;
            Logging.Write(Colors.LightSeaGreen, "Casting: " + Spell + " on: " + Me.SafeName);
            lstSpell = Spell;
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
