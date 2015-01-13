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

using HKM = DK.DKHotkeyManagers;
using EH = DK.EventHandlers;
using CL = DK.CombatLogEventArgs;
using P = DK.DKSettings;

namespace DK
{
    public partial class DKMain : CombatRoutine
    {
        public override string Name { get { return "DK Routine by Pasterke"; } }
        public override WoWClass Class { get { return WoWClass.DeathKnight; } }
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override Composite CombatBehavior { get { return new ActionRunCoroutine(ctx => rotationSelector()); } }
        public override Composite PreCombatBuffBehavior { get { return new ActionRunCoroutine(ctx => PreCombatBuffCoroutine()); } }
        public override Composite CombatBuffBehavior { get { return new ActionRunCoroutine(ctx => CombatBuffCoroutine()); } }
        public override Composite PullBehavior { get { return new ActionRunCoroutine(ctx => PullCoroutine()); } }

        public static WoWGuid lastGuid;
        public static bool checkInCombat { get; set; }
        public static DateTime nextCheckTimer;
        public static DateTime combatTimer;
        public static DateTime moveBackTimer;
        public static void setNextCombatTimer()
        {
            combatTimer = DateTime.Now + new TimeSpan(0, 0, 0, 0, 30 * 1000);
        }
        public static void setNextCheckTimer()
        {
            nextCheckTimer = DateTime.Now + new TimeSpan(0, 0, 0, 30, 0);
        }
        public bool checkTarget { get; set; }

        public override bool WantButton { get { return true; } }
        public override void OnButtonPress()
        {
            new DKGui().ShowDialog();
        }

        public override void Initialize()
        {
            Logging.Write("\r\n" + "-- Hello {0}", Me.Name);
            Logging.Write("-- Thanks for using");
            Logging.Write("-- The DK Combat Routine");
            Logging.Write("-- by Pasterke" + "\r\n");
            HKM.registerHotKeys();
            Lua.Events.AttachEvent("UI_ERROR_MESSAGE", CL.CombatLogErrorHandler);
            EH.AttachCombatLogEvent();
            //svnUpdates.CheckForUpdate();
            if (!P.myPrefs.MsgBoxShown)
            {
                MessageBox.Show("Only Blood DeathKnight Supported !\r\nThis Message won't be shown again.", "Spec Warning");
                P.myPrefs.MsgBoxShown = true;
                P.myPrefs.Save();
            }
        }

        public override void ShutDown()
        {
            HKM.removeHotkeys();
            EH.DetachCombatLogEvent();
            Lua.Events.DetachEvent("UI_ERROR_MESSAGE", CL.CombatLogErrorHandler);
        }
        public static List<WoWUnit> partyMembers = new List<WoWUnit>();
        public override void Pulse()
        {
            try
            {
                if (Me.IsDead
                    && AutoBot)
                {
                    Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/script RepopMe()"));
                }
                return;
            }
            catch (Exception e) { Logging.Write(Colors.Red, "Pulse: " + e); }
            return;
        }
        public override bool NeedRest
        {
            get
            {
                if (Me.HealthPercent <= 50 && !Me.IsSwimming && Canbuff) return true;
                return false;
            }
        }
        public override void Rest()
        {
            base.Rest();
            if (Me.HealthPercent <= 50 && !Me.IsSwimming && Canbuff && AutoBot) { Styx.CommonBot.Rest.Feed(); }
        }

        private static async Task<bool> PreCombatBuffCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await UseItem(CRYSTAL_OF_ORALIUS_ITEM, CrystalOfOraliusConditions && Canbuff)) return true;
            if (await UseItem(CRYSTAL_OF_INSANITY_ITEM, CrystalOfInsanityConditions && Canbuff)) return true;
            if (await UseItem(ALCHEMYFLASK_ITEM, AlchemyFlaskConditions && Canbuff)) return true;
            if (await CastBuff(PRESENCE, P.myPrefs.Presence != 0 && needPresence && Canbuff)) return true;
            return false;
        }

        private static async Task<bool> CombatBuffCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await UseItem(CRYSTAL_OF_ORALIUS_ITEM, CrystalOfOraliusConditions && Canbuff)) return true;
            if (await UseItem(CRYSTAL_OF_INSANITY_ITEM, CrystalOfInsanityConditions && Canbuff)) return true;
            if (await UseItem(ALCHEMYFLASK_ITEM, AlchemyFlaskConditions && Canbuff)) return true;
            if (await UseItem(HEALTHSTONE_ITEM, Me.HealthPercent <= 45 && Canbuff)) return true;
            if (await CastBuff(PRESENCE, P.myPrefs.Presence != 0 && needPresence && Canbuff)) return true;
            return false;
        }

        private static DateTime pullingTimer;
        private static async Task<bool> PullCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (!pullTimer.IsRunning && AutoBot)
            {
                pullTimer.Restart();
                lastGuid = Me.CurrentTarget.Guid;
                Logging.Write(Colors.CornflowerBlue, "Starting PullTimer");
            }
            if (await CannotPull(Me.CurrentTarget, Me.CurrentTarget != null
                && pullTimer.ElapsedMilliseconds >= 30 * 1000
                && lastGuid == Me.CurrentTarget.Guid)) return true;
            if (await clearTarget(Me.CurrentTarget == null && AllowTargeting && (Me.CurrentTarget.IsDead || Me.CurrentTarget.IsFriendly) && !Me.CurrentTarget.Lootable)) return true;
            if (await MoveToTarget(Me.CurrentTarget != null && AllowMovement && Me.CurrentTarget.Distance > 4.5f)) return true;
            if (await StopMovement(Me.CurrentTarget != null && AllowMovement && Me.CurrentTarget.Distance <= 4.5f && Me.IsMoving)) return true;
            if (await FaceMyTarget(Me.CurrentTarget != null && AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget) && !Me.IsMoving)) return true;

            if (await CastPull(DARK_COMMAND, gotTarget && Range30 && !spellOnCooldown(DARK_COMMAND) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(DEATH_GRIP, gotTarget && Range30 && !spellOnCooldown(DEATH_GRIP) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(OUTBREAK, gotTarget && Range30 && !spellOnCooldown(OUTBREAK) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(DEATH_COIL, gotTarget && Range30 && !spellOnCooldown(DEATH_COIL) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(ICY_TOUCH, gotTarget && Range30 && !spellOnCooldown(ICY_TOUCH) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(PLAGUE_STRIKE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now >= pullingTimer)) return true;

            return false;
        }

        #region rest
        private static async Task<bool> EatFood(bool req)
        {
            if (!req) return false;
            Styx.CommonBot.Rest.Feed();
            await CommonCoroutines.SleepForLagDuration();
            return Canbuff;
        }
        #endregion
    }
}
