using Styx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


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
