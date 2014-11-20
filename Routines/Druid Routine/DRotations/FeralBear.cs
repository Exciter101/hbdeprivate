using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
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

namespace Druid.DRotations
{
    class FeralBear
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Composite FeralBearRot()
        {
            return new S.FrameLockSelector(
                S.castCatForm(),
                T.LookToRes(),
                S.castBearForm(),
                S.castMangle(),
                S.castThrash()
                );
        }
    }
}
