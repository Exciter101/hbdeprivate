using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    class Interrupts
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region Interrupts
        private static DateTime intAllowed;
        private static Random random = new Random();
        private static int randomNumber;
        private static void setIntAllowed()
        {
            intAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, randomNumber);
        }

        public static bool ItsTimeToInterrupt
        {
            get
            {
                string castInfo = string.Empty;
                

                if (Me.CurrentTarget != null && Me.CurrentTarget.IsCasting)
                {
                    castInfo = Lua.GetReturnVal<string>("local name, subText, text, texture, startTime, endTime, isTradeSkill, DcastID, notInterruptible = UnitCastingInfo(\"target\"); return notInterruptible", 0);
                    //Logging.Write(Colors.Fuchsia, "castInfo: " + castInfo);

                    if (castInfo != "1")
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion
    }
}
