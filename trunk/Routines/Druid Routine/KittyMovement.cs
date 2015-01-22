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

namespace Kitty
{
    public partial class KittyMain : CombatRoutine
    {
        #region movement targeting facing

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public static bool AllowFacing
        {
            get
            {
                /*if (HKM.manualOn) { return false; }*/
                if (P.myPrefs.AutoFacingDisable
                    && (Me.CurrentMap.IsDungeon || Me.CurrentMap.IsInstance || Me.CurrentMap.IsRaid || Me.CurrentMap.IsScenario || Me.GroupInfo.IsInRaid))
                {
                    return false;
                }
                else if ((GetAsyncKeyState(Keys.LButton) != 0
                    && GetAsyncKeyState(Keys.RButton) != 0) ||
                    GetAsyncKeyState(Keys.W) != 0 ||
                    GetAsyncKeyState(Keys.S) != 0 ||
                    GetAsyncKeyState(Keys.D) != 0 ||
                    GetAsyncKeyState(Keys.A) != 0)
                {
                    return false;
                }
                return P.myPrefs.AutoFacing;

            }
        }
        public static bool AllowTargeting
        {
            get
            {
                /*if (HKM.manualOn) { return false; }*/
                if (P.myPrefs.AutoTargetingDisable
                    && (Me.CurrentMap.IsDungeon || Me.CurrentMap.IsInstance || Me.CurrentMap.IsRaid || Me.CurrentMap.IsScenario || Me.GroupInfo.IsInRaid))
                {
                    return false;
                }
                else if ((GetAsyncKeyState(Keys.LButton) != 0
                    && GetAsyncKeyState(Keys.RButton) != 0) ||
                    GetAsyncKeyState(Keys.W) != 0 ||
                    GetAsyncKeyState(Keys.S) != 0 ||
                    GetAsyncKeyState(Keys.D) != 0 ||
                    GetAsyncKeyState(Keys.A) != 0)
                {
                    return false;
                }
                return P.myPrefs.AutoTargeting;
            }
        }
        public static bool AllowMovement
        {
            get
            {
                /*if (HKM.manualOn)
                {
                    return false;
                }*/
                if (P.myPrefs.AutoMovementDisable
                    && (Me.CurrentMap.IsDungeon || Me.CurrentMap.IsInstance || Me.CurrentMap.IsRaid || Me.CurrentMap.IsScenario || Me.GroupInfo.IsInRaid))
                {
                    return false;
                }
                else if ((GetAsyncKeyState(Keys.LButton) != 0
                    && GetAsyncKeyState(Keys.RButton) != 0) ||
                    GetAsyncKeyState(Keys.W) != 0 ||
                    GetAsyncKeyState(Keys.S) != 0 ||
                    GetAsyncKeyState(Keys.D) != 0 ||
                    GetAsyncKeyState(Keys.A) != 0)
                {
                    return false;
                }


                return P.myPrefs.AutoMovement;
            }
        }
        #endregion

        #region facing
        public static async Task<bool> FaceMyTarget(bool reqs)
        {
            if (!reqs) return false;
            Me.CurrentTarget.Face();
            await CommonCoroutines.SleepForLagDuration();
            return false;
        }
        #endregion

        #region move to and stop movement
        public static async Task<bool> MoveToTarget(bool reqs)
        {
            if (!reqs) return false;
            Navigator.MoveTo(Me.CurrentTarget.Location);
            await CommonCoroutines.SleepForLagDuration();
            return false;
        }
        public static async Task<bool> StopMovement(bool reqs)
        {
            if (!reqs) return false;
            Navigator.PlayerMover.MoveStop();
            await CommonCoroutines.SleepForLagDuration();
            return false;
        }
        #endregion
    }
}
