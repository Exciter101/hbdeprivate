using Styx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


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
    class Logs
    {
        #region [Method] - Combat Log
        public static string lastCombatMSG;
        public static void combatLog(string Message, params object[] args)
        {
            if (Message == lastCombatMSG)
                return;
            Logging.Write(Colors.Yellow, "{0}", String.Format(Message, args));
            lastCombatMSG = Message;
        }
        #endregion

        #region [Method] - Diagnostics Log
        public static void diagnosticLog(string Message, params object[] args)
        {
            if (Message == null)
                return;
            Logging.WriteDiagnostic(Colors.Firebrick, "{0}", String.Format(Message, args));
        }
        #endregion
    }
}
