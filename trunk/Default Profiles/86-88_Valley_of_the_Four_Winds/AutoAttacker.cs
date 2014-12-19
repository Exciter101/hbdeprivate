using System;

using Styx;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;
using System.Linq;
namespace Attacker
{
    public class Autoattack: HBPlugin
    {
        public override string Name
        {
            get { return "AutoAttacker"; }
        }

        public override string Author
        {
            get { return "Stove"; }
        }

        public override Version Version
        {
            get { return new Version(0, 1); }
        }
        public static LocalPlayer Me = StyxWoW.Me;
        public override void Pulse()
        {
            if (!Me.Combat || Me.IsFlying || Me.Mounted) return;
            if (Me.CurrentTarget == null)
            {
                var mobs = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsHostile).OrderBy(o => o.Distance);
                var mob = mobs.FirstOrDefault();
                if (mob != null)
                {
                    mob.Target();
                }
            }
            if (Me.CurrentTarget != null && !Me.IsFacing(Me.CurrentTarget))
            {
                Logging.Write("Facing target");
                Me.Face();
            }
            if (Me.CurrentTarget != null && Me.CurrentTarget.IsDead)
            {
                Logging.Write("Clearing target");
                Me.ClearTarget();
            }
            if (!Me.IsAutoAttacking)
            {
                Logging.Write("Autoattacking!");
                Me.ToggleAttack();
            }
        }
     }
}
