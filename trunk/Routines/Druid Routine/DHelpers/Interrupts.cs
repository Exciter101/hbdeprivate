  using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
using M = Druid.Helpers.Movement;
using I = Druid.Helpers.Interrupts;
#endregion

namespace Druid.Helpers
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
                    castInfo = Lua.GetReturnVal<string>(
                        "local name, subText, text, texture, startTime, endTime, isTradeSkill, DcastID, notInterruptible = UnitCastingInfo(\"target\"); return notInterruptible", 0);
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

        #region dispels
        public static bool IsDispellable
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
