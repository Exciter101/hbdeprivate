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
using Form1 = Druid.DGUI.Form1;
using HKM = Druid.Helpers.HotkeyManager;
using S = Druid.DSpells.SpellCasts;
using CL = Druid.Handlers.CombatLogEventArgs;
using EH = Druid.Handlers.EventHandlers;
using L = Druid.Helpers.Logs;
using T = Druid.Helpers.targets;
using U = Druid.Helpers.Unit;
using UI = Druid.Helpers.UseItems;
using P = Druid.DSettings.DruidPrefs;
using PR = Druid.DSettings.RestoSettings;
using M = Druid.Helpers.Movement;
using I = Druid.Helpers.Interrupts;
#endregion

namespace Druid.Helpers
{
    public class Movement
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
        public static Composite CreateMeleeMovement()
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
        public static Composite CreateRangeMovement()
        {
            return new Decorator(ret => S.gotTarget && M.AllowMovement && Me.CurrentTarget.Distance <= 39,
                new Action(ret =>
                {
                    if (CL.IsNotInLineOfSight)
                    {
                        Navigator.MoveTo(Me.CurrentTarget.Location);
                    }

                    Navigator.MoveTo(Me.CurrentTarget.Location);
                    return RunStatus.Failure;
            }));
        }
        public static Composite CreateStopMeleeMovement()
        {
            return new Action(ret =>
            {
                Navigator.PlayerMover.MoveStop();
                return RunStatus.Failure;
            });
        }
        public static Composite CreateStopRangeMovement()
        {
            return new Decorator(ret => S.gotTarget && M.AllowMovement && Me.CurrentTarget.Distance <= 39,
                new Action(ret =>
                {
                    Navigator.PlayerMover.MoveStop();
                    return RunStatus.Failure;
                }));
        }
        public static Composite FacingTarget()
        {
            return new Decorator(ret => S.gotTarget && M.AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget),
                new Action(ret =>
                {
                    Me.CurrentTarget.Face();
                    return RunStatus.Failure;
                }));
        }
        
        #endregion

        /*public static bool IsAboveTheGround(WoWUnit u)
        {
            float height = HeightOffTheGround(u);
            if (height == float.MaxValue)
                return false; // make this true if better to assume aerial 

            return height > MeleeRange(u);
        }*/
        public static float HeightOffTheGround(WoWUnit u)
        {
            var unitLoc = new WoWPoint(u.Location.X, u.Location.Y, u.Location.Z);
            IEnumerable<float> listMeshZ = Navigator.FindHeights(unitLoc.X, unitLoc.Y).Where(h => h <= unitLoc.Z + 2f);
            if (listMeshZ.Any())
                return unitLoc.Z - listMeshZ.Max();

            return float.MaxValue;
        }
        public static bool IsInMeleeRange(WoWUnit unit)
        {
            if (unit.IsPlayer && unit.Distance <= 3.5f) { return true; }
            return unit.IsWithinMeleeRange;
        }
    }
}

