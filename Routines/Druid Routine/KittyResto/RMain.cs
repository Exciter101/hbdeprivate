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

namespace RestoDruid
{
    public partial class RMain
    {
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        

        public static bool _init = false;
        public static int lastPartySize = 0;
        public static uint lastMapID = 0;
        public static uint lastZoneID = 0;
        public static string lastSpell = string.Empty;

        

        public static async Task<bool> PreCombatBuffCoroutine()
        {
            if (await CastBuff(_markofthewild, needStatsBuff, Me)) return true;
            if (usedBot.Contains("GATHER") && !Me.Mounted && Me.IsMoving && Me.Shapeshift != ShapeshiftForm.Travel) { SpellManager.Cast("Travel Form"); }

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }



        public static async Task<bool> HealingRoutine()
        {
            if (Me.Mounted && !AutoBot) { return false; }
            if (Me.IsCasting || Me.IsChanneling) return false;

            if (Me.CurrentTarget != null && AutoBot && (Me.CurrentTarget.IsDead || !units.ValidUnit(Me.CurrentTarget))) { Me.ClearTarget(); }
            if (Me.CurrentTarget != null && needMovement && Me.CurrentTarget.Distance > 39) { Navigator.PlayerMover.MoveTowards(Me.CurrentTarget.Location); }
            if (Me.CurrentTarget != null && needMovement && Me.CurrentTarget.Distance <= 39) { Navigator.PlayerMover.MoveStop(); }
            if (Me.CurrentTarget != null && needFacing && !Me.IsSafelyFacing(Me.CurrentTarget)) { Me.CurrentTarget.Face(); }

            if (await CastBuff(_markofthewild, needStatsBuff, Me)) return true;
            if (await UseItem(healthStone, needHealthstone())) return true;

            if (await CastHeal(_healingtouch, HarmonyTarget != null && !Me.HasAura("Harmony"), HarmonyTarget)) return true;
            if (await CastHeal(_ironbark, IronBarkTarget != null && !units.spellOnCooldown(_ironbark), IronBarkTarget)) return true;

            if (await CastTrinket(needTrinket1 && DateTime.Now >= _trinketTimer)) return true;
            if (await CastTrinket(needTrinket2 && DateTime.Now >= _trinketTimer)) return true;

            if (await CastHeal(_naturescure, dispelTarget != null && !units.spellOnCooldown(_naturescure), dispelTarget)) return true;

            if (await CastBuff(_naturesvigil, needNaturesVigil && !units.spellOnCooldown(_naturesvigil), Me)) return true;
            if (await CastBuff(_incarnationresto, needIncarnation, Me)) return true;

            if (await CastHeal(_regrowth, RegrowthProcTarget != null, RegrowthProcTarget)) return true;

            if (await CastHeal(_lifebloom, lifebloomTarget != null, lifebloomTarget)) return true;
            if (await CastHeal(_rejuvenation, KeepRejuOnTankUnit != null, KeepRejuOnTankUnit)) return true;
            if (await CastHeal(_rejuvenation, KeepGerminationOnTankUnit != null, KeepGerminationOnTankUnit)) return true;

            if (await CastNonTargetSpell(_tranquility, TranquilityUnit != null && !Me.IsMoving)) return true;
            if (await CastHeal(_wildgrowth, wildgrowthTarget != null, wildgrowthTarget)) return true;
            if (await CastGroundSpell(_wildmushroom, MushroomTarget != null && (DateTime.Now >= lastMushroomCast || resetMushroomTimer), MushroomTarget)) return true;
            if (await CastHeal(_genesis, genesisTarget != null, genesisTarget)) return true;
            if (await CastHeal(_swiftmend, SwiftMendTarget != null, SwiftMendTarget)) return true;

            //force of nature
            if (await CastHeal(_forceofnature, FoNTarget != null && DateTime.Now >= _fonTimer, FoNTarget)) return true;

            //regrowth
            if (await CastHeal(_regrowth, RegrowthTarget != null, RegrowthTarget)) return true;

            //healing touch
            if (await CastHeal(_healingtouch, HealingTouchTarget != null, HealingTouchTarget)) return true;

            //rejuvenation
            if (await CastHeal(_rejuvenation, RejuvenationTarget != null, RejuvenationTarget)) return true;

            //germination
            if (await CastHeal(_rejuvenation, GerminationTarget != null, GerminationTarget)) return true;

            //dps
            if (await CastHeal(_moonfire, needDpsTarget() && dpsTarget != null && units.ValidUnit(dpsTarget) && !units.debuffExists(_moonfire, dpsTarget), dpsTarget)) return true;
            if (await CastHeal(_wrath, needDpsTarget() && dpsTarget != null && units.ValidUnit(dpsTarget), dpsTarget)) return true;



            //if (await CastHeal(_moonfire, Me.CurrentTarget != null && units.ValidUnit(Me.CurrentTarget) && !units.debuffExists(_moonfire, Me.CurrentTarget) && partyCount == 0, Me.CurrentTarget)) return true;
            //if (await CastHeal(_wrath, Me.CurrentTarget != null && units.ValidUnit(Me.CurrentTarget) && partyCount == 0, Me.CurrentTarget)) return true;

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        public static async Task<bool> CastBuff(string Spell, bool reqs, WoWUnit myTarget)
        {
            if (!reqs) return false;
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            Logging.Write("Casting: " + Spell + " on: " + myTarget.Name);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastHeal(string Spell, bool reqs, WoWUnit myTarget)
        {
            if (!reqs) return false;
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            
            Logging.Write("Casting: " + Spell + " on: " + myTarget.Name);
            if (Spell == _forceofnature) { _fonTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 1500); }
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastNonTargetSpell(string Spell, bool reqs)
        {
            if (!reqs) return false;
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!SpellManager.CanCast(Spell)) return false;
            if (!SpellManager.Cast(Spell)) return false;
            Logging.Write("Casting: " + Spell);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastGroundSpell(string Spell, bool reqs, WoWUnit myTarget)
        {
            if (!reqs) return false;
            if (!SpellManager.HasSpell(Spell)) return false;
            if (!SpellManager.CanCast(Spell, myTarget)) return false;
            if (!SpellManager.Cast(Spell, myTarget)) return false;
            if (!await Coroutine.Wait(1000, () => StyxWoW.Me.CurrentPendingCursorSpell != null))
            {
                Logging.Write("Cursor didn't turn into the spell!");
                return false;
            }
            SpellManager.ClickRemoteLocation(myTarget.Location);
            Logging.Write("Casting: " + Spell + " on: " + myTarget.SafeName);
            if (Spell == _wildmushroom) { lastMushroomCast = DateTime.Now + new TimeSpan(0, 0, 0, 25, 0); }
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> CastTrinket(bool reqs)
        {
            if (!reqs) return false;
            _trinketTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 5000);
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> UseItem(int itemID, bool reqs)
        {
            if (!reqs) return false;
            WoWItem potion = StyxWoW.Me.BagItems.FirstOrDefault(h => h.Entry == itemID);
            if (potion == null || potion.CooldownTimeLeft.TotalMilliseconds > 0)
            {
                return false;
            }
            potion.Use();
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }

        #region timers
        public static DateTime _fonTimer;
        #endregion

        #region autobot
        public static string usedBot 
        { 
            get { return BotManager.Current.Name.ToUpper(); } 
        }
        public static bool AutoBot
        {
            get
            {
                return usedBot.Contains("QUEST") || usedBot.Contains("GRIND") || usedBot.Contains("GATHER") || usedBot.Contains("ANGLER") || usedBot.Contains("ARCHEO");

            }
        }
        #endregion
    }
}
