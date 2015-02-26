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
using P = DK.DKSettings;

namespace DK
{
    public partial class DKMain : CombatRoutine
    {
        public override string Name { get { return "DeathKnight Routine by Pasterke"; } }
        public override WoWClass Class { get { return WoWClass.DeathKnight; } }
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override Composite CombatBehavior { get { return new ActionRunCoroutine(ctx => rotationSelector()); } }
        public override Composite PreCombatBuffBehavior { get { return new ActionRunCoroutine(ctx => PreCombatBuffCoroutine()); } }
        public override Composite CombatBuffBehavior { get { return new ActionRunCoroutine(ctx => CombatBuffCoroutine()); } }
        public override Composite PullBehavior { get { return new ActionRunCoroutine(ctx => PullCoroutine()); } }
        public override Composite RestBehavior { get { return new ActionRunCoroutine(ctx => RestCoroutine()); } }


        public static WoWGuid lastGuid;
        public static bool checkInCombat { get; set; }
        public static DateTime nextCheckTimer;
        public static DateTime moveBackTimer;
        public static void setNextCheckTimer()
        {
            nextCheckTimer = DateTime.Now + new TimeSpan(0, 0, 0, 30, 0);
        }
        public bool checkTarget { get; set; }

        public override bool WantButton { get { return true; } }
        public override void OnButtonPress()
        {
            new DKGui2().ShowDialog();
        }

        public override void Initialize()
        {
            Logging.Write("\r\n" + "-- Hello {0}", Me.Name);
            Logging.Write("-- Thanks for using");
            Logging.Write("-- The DK Combat Routine");
            Logging.Write("-- by Pasterke" + "\r\n");

            HKM.registerHotKeys();
        }

        public override void ShutDown()
        {
            HKM.removeHotkeys();
        }

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

        private static async Task<bool> RestCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (Me.Mounted || Me.IsSwimming || Me.HasAura("Food")) return false;
            if (Me.HealthPercent <= P.myPrefs.FoodHPOoC && !Me.IsSwimming && Canbuff && AutoBot) { Styx.CommonBot.Rest.Feed(); }

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        private static async Task<bool> PreCombatBuffCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await CastBuff(PRESENCE, P.myPrefs.Presence != P.presence.Manual && !Me.HasAura(PRESENCE) && Canbuff)) return true;

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        private static async Task<bool> CombatBuffCoroutine()
        {
            if (Me.IsCasting || HKM.pauseRoutineOn || HKM.manualOn) return false;
            if (await CastBuff(PRESENCE, P.myPrefs.Presence != P.presence.Manual && !Me.HasAura(PRESENCE) && Canbuff)) return true;

            await CommonCoroutines.SleepForLagDuration();
            return false;
        }

        public static DateTime pullingTimer;
        public static DateTime pullingTimerCheck;
        public static DateTime combatTimer;
        public static bool pullTimerIsRunning = false;
        public static bool combatTimerIsRunning = false;

        private static async Task<bool> PullCoroutine()
        {
            if (DKSettings.myPrefs.AutoMovement) await Movement.MoveToCombatTarget();
            if (DKSettings.myPrefs.AutoFacing) await Movement.FaceMyCurrentTarget();
            if (Me.CurrentTarget != null && !pullTimerIsRunning) await StartPullTimer();
            if (Me.CurrentTarget != null && pullTimerIsRunning && lastGuid == Me.CurrentTargetGuid && DateTime.Now >= pullingTimerCheck) await BlacklistingPullMob();
            if (await CastPull(DARK_COMMAND, gotTarget && Range30 && !spellOnCooldown(DARK_COMMAND) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(DEATH_GRIP, gotTarget && Range30 && !spellOnCooldown(DEATH_GRIP) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(OUTBREAK, gotTarget && Range30 && !spellOnCooldown(OUTBREAK) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(DEATH_COIL, gotTarget && Range30 && !spellOnCooldown(DEATH_COIL) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(ICY_TOUCH, gotTarget && Range30 && !spellOnCooldown(ICY_TOUCH) && DateTime.Now >= pullingTimer)) return true;
            if (await CastPull(PLAGUE_STRIKE, gotTarget && Me.CurrentTarget.IsWithinMeleeRange && DateTime.Now >= pullingTimer)) return true;



            await CommonCoroutines.SleepForLagDuration();
            return false;
        }
        public static async Task<bool> StartPullTimer()
        {
            if (Me.CurrentTarget != null && ValidUnit(Me.CurrentTarget))
            {
                combatTimerIsRunning = false;
                pullTimerIsRunning = true;
                pullingTimerCheck = DateTime.Now + new TimeSpan(0, 0, 0, 30, 0);
                lastGuid = Me.CurrentTargetGuid;
                Logging.Write(Colors.CornflowerBlue, "Starting PullTimer");
                return false;
            }
            await CommonCoroutines.SleepForLagDuration();
            return false;
        }
        public static async Task<bool> BlacklistingPullMob()
        {
            if (Me.CurrentTarget != null && ValidUnit(Me.CurrentTarget))
            {
                pullTimerIsRunning = false;
                combatTimerIsRunning = false;
                Blacklist.Add(Me.CurrentTargetGuid, BlacklistFlags.All, new TimeSpan(0, 0, 0, 10, 0));

                Logging.Write(Colors.CornflowerBlue, "Current Target " + Me.CurrentTarget.SafeName + " is a bugged mob ! Blacklisting it for 10 seconds !");
                Me.ClearTarget();
                return false;
            }
            await CommonCoroutines.SleepForLagDuration();
            return false;
        }
        public static async Task<bool> StartCombatTimer()
        {
            if (Me.CurrentTarget != null && ValidUnit(Me.CurrentTarget))
            {
                pullTimerIsRunning = false;
                combatTimerIsRunning = true;
                combatTimer = DateTime.Now + new TimeSpan(0, 0, 0, 30, 0);
                lastGuid = Me.CurrentTargetGuid;
                Logging.Write(Colors.CornflowerBlue, "Starting CombatTimer");
                return false;
            }
            await CommonCoroutines.SleepForLagDuration();
            return false;
        }
        public static async Task<bool> BlacklistingCombatMob()
        {
            if (Me.CurrentTarget != null && ValidUnit(Me.CurrentTarget) && !Me.CurrentTarget.IsPlayer)
            {
                combatTimerIsRunning = false;
                pullTimerIsRunning = false;
                Blacklist.Add(Me.CurrentTargetGuid, BlacklistFlags.All, new TimeSpan(0, 0, 0, 10, 0));

                Logging.Write(Colors.CornflowerBlue, "Current Target " + Me.CurrentTarget.SafeName + " is a bugged mob ! Blacklisting it for 10 seconds !");
                Me.ClearTarget();
                return false;
            }
            await CommonCoroutines.SleepForLagDuration();
            return false;
        }
        

    }
}
