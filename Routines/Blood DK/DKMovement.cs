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
    class Movement
    {
        public static bool MeInParty
        {
            get
            {
                var t = new List<WoWPartyMember>();
                t = StyxWoW.Me.GroupInfo.RaidMembers.Union(StyxWoW.Me.GroupInfo.PartyMembers).Distinct().ToList();
                return t.Count() > 0 ? true : false;
            }
        }



        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        /// <summary>
        /// (Non-Blocking) Attempts to move to the player's current target.
        /// </summary>
        /// <returns>Returns true if we are able to move towards the target.</returns>
        /// 
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        public static async Task MoveToCombatTarget()
        {
            if (Me.CurrentTarget == null) return;

            if (MeInParty && DKSettings.myPrefs.AutoMovementDisable) return;

            if ((GetAsyncKeyState(Keys.LButton) != 0
                    && GetAsyncKeyState(Keys.RButton) != 0) ||
                    GetAsyncKeyState(Keys.W) != 0 ||
                    GetAsyncKeyState(Keys.S) != 0 ||
                    GetAsyncKeyState(Keys.D) != 0 ||
                    GetAsyncKeyState(Keys.A) != 0) return;
            
            try
            {
                await MoveToTarget(Me.CurrentTarget,
                    () =>
                        Me.CurrentTarget != null
                        && DKSettings.myPrefs.AutoMovement
                        && !Me.CurrentTarget.IsWithinMeleeRange);

                await MoveStop(
                    () =>
                        Me.CurrentTarget != null
                        && DKSettings.myPrefs.AutoMovement
                        && Me.CurrentTarget.IsWithinMeleeRange);
            }
            catch (Exception ex)
            {
                Logging.Write(string.Format("Exception caught while trying to move: {0}", ex.Message));
            }
        }

        /// <summary>
        /// (Non-Blocking) Attempts to move to the specified target.
        /// </summary>
        /// <returns>Returns true if we are able to move towards the target.</returns>
        public static async Task MoveToTarget(WoWUnit target, Func<bool> conditionCheck = null)
        {
            if (conditionCheck != null && !conditionCheck())
                return;

            if (target == null)
                return;

            if (target.IsDead)
                return;

            await CommonCoroutines.MoveTo(target.Location);
        }

        /// <summary>
        /// (Non-Blocking) Stop the player from moving.
        /// </summary>
        /// <returns>Returns true if we are able to stop moving.</returns>
        public static async Task MoveStop(Func<bool> conditionCheck = null)
        {
            if (conditionCheck != null && !conditionCheck())
                return;

            await CommonCoroutines.StopMoving();
        }

        /// <summary>
        /// (Non-Blocking) Attempts to face the player's current target.
        /// </summary>
        /// <returns>Returns true if we are able to safely face the target</returns>
        public static async Task FaceMyCurrentTarget()
        {
            if (Me.CurrentTarget == null)
                return;

            if (MeInParty && DKSettings.myPrefs.AutoFacingDisable) return;

            await FaceTarget(Me.CurrentTarget,
                () =>
                    Me.CurrentTarget != null 
                    && DKSettings.myPrefs.AutoFacing
                    && !Me.IsMoving
                    && !Me.IsSafelyFacing(Me.CurrentTarget));
        }

        /// <summary>
        /// (Non-Blocking) Attempts to face the specified target.
        /// </summary>
        /// <returns>Returns true if we are able to safely face the target</returns>
        public static async Task FaceTarget(WoWUnit target, Func<bool> conditionCheck = null)
        {
            if (conditionCheck != null && !conditionCheck())
                return;

            target.Face();
            await CommonCoroutines.SleepForLagDuration();
        }

        /// <summary>
        /// (Non-Blocking) Attempts to clear the player's current target.
        /// </summary>
        /// <returns>Returns true if we are able to clear the target</returns>
        public static async Task<bool> ClearMyDeadTarget()
        {
            if (Me.CurrentTarget != null && Me.CurrentTarget.IsDead)
            {
                Me.ClearTarget();
                await CommonCoroutines.SleepForLagDuration();
                return false;
            }
            return false;
        }
    }
}
