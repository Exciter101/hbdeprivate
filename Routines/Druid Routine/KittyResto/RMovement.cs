using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.CommonBot.Routines;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PD = RestoDruid.RSettings;

namespace RestoDruid
{
    public class Movement
    {

        public static async Task<bool> AutoMove(bool reqs, WoWUnit unit, int distance)
        {
            if (!reqs) return false;
            if (RMain.partyCount == 0 && !PD.myPrefs.MovementAuto) return false;
            if (RMain.partyCount > 0 && PD.myPrefs.MovementAutoDisable) return false;
            if (unit != null && unit.Distance > distance)
            {
                Navigator.MoveTo(unit.Location);
            }
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> StopMove(bool reqs, WoWUnit unit, int distance)
        {
            if (!reqs) return false;
            if (RMain.partyCount == 0 && !PD.myPrefs.MovementAuto) return false;
            if (RMain.partyCount > 0 && PD.myPrefs.MovementAutoDisable) return false;
            if (unit != null && unit.Distance <= distance)
            {
                Navigator.PlayerMover.MoveStop();
            }
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> AutoTarget(bool reqs, WoWUnit unit)
        {
            if (!reqs) return false;
            if (RMain.partyCount == 0 && !PD.myPrefs.MovementTargeting) return false;
            if (RMain.partyCount > 0 && PD.myPrefs.MovementTargetingDisable) return false;
            if (unit == null)
            {
                unit.Target();
            }
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> AutoFace(bool reqs, WoWUnit unit)
        {
            if (!reqs) return false;
            if (RMain.partyCount == 0 && !PD.myPrefs.MovementFacing) return false;
            if (RMain.partyCount > 0 && PD.myPrefs.MovementFacingDisable) return false;
            if (unit != null && units.ValidUnit(unit))
            {
                unit.CurrentTarget.Face();
            }
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
        public static async Task<bool> ClearTarget(bool reqs)
        {
            if (!reqs) return false;
            StyxWoW.Me.ClearTarget();
            await CommonCoroutines.SleepForLagDuration();
            return true;
        }
    }
}
