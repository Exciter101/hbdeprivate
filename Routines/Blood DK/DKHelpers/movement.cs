using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Styx.Pathing;
using Styx.Common;
using System.Windows.Media;
using Styx.CommonBot.Frames;
using System.Diagnostics;
using NewMixedMode;

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
    class Movement
    {
        #region movement targeting facing

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static bool AllowFacing
        {
            get
            {
                if (HKM.manualOn) { return false; }
                else if (P.myPrefs.AutoFacingDisable
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
                if (HKM.manualOn) { return false; }
                else if (P.myPrefs.AutoTargetingDisable
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
                if (HKM.manualOn)
                {
                    return false;
                }
                else if (P.myPrefs.AutoMovementDisable
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
        public static Composite CreateMovement()
        {
            return new Action(ret =>
            {
                if (CL.IsNotInLineOfSight)
                {
                    Navigator.MoveTo(Me.CurrentTarget.Location);
                }

                Navigator.MoveTo(Me.CurrentTarget.Location);
                return RunStatus.Failure;
            });
        }
        public static Composite CreateStopMovement()
        {
            return new Action(ret =>
            {
                Navigator.PlayerMover.MoveStop();
                return RunStatus.Failure;
            });
        }
        public static Composite FacingTarget()
        {
            return new Action(ret =>
            {
                if (!Me.IsSafelyFacing(Me.CurrentTarget))
                {
                    Me.CurrentTarget.Face();
                }
                return RunStatus.Failure;
            });
        }
        #endregion

        public static float MeleeRange
        {
            get { return StyxWoW.Me.CurrentTarget == null ? 0 : MeleeDistance(Me.CurrentTarget); }
        }

        public static float MeleeDistance(WoWUnit mob)
        {
            return mob.IsPlayer ? 3.5f : Math.Max(5f, StyxWoW.Me.CombatReach + 1.3333334f + mob.CombatReach);
        }

        public static float HeightOffTheGround(WoWUnit u)
        {
            var unitLoc = new WoWPoint(u.Location.X, u.Location.Y, u.Location.Z);
            IEnumerable<float> listMeshZ = Navigator.FindHeights(unitLoc.X, unitLoc.Y).Where(h => h <= unitLoc.Z + 2f);
            if (listMeshZ.Any())
                return unitLoc.Z - listMeshZ.Max();

            return float.MaxValue;
        }
    }
}

