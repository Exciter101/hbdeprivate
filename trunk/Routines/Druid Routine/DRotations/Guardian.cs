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
    class Guardian
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Composite GuardianRot()
        {
            return new S.FrameLockSelector(
                T.LookToRes(),
                new Decorator(ret => P.myPrefs.Trinket1 == 4 && Me.HealthPercent <= P.myPrefs.PercentTrinket1HP, UI.useTrinket1()),
                new Decorator(ret => P.myPrefs.Trinket2 == 4 && Me.HealthPercent <= P.myPrefs.PercentTrinket2HP, UI.useTrinket2()),
                new Decorator(ret => P.myPrefs.Trinket1 == 5 && Me.ManaPercent <= P.myPrefs.PercentTrinket1Mana, UI.useTrinket1()),
                new Decorator(ret => P.myPrefs.Trinket2 == 5 && Me.ManaPercent <= P.myPrefs.PercentTrinket2Mana, UI.useTrinket2()),
                new Decorator(ret => P.myPrefs.Trinket1 == 2 && HKM.cooldownsOn, UI.useTrinket1()),
                new Decorator(ret => P.myPrefs.Trinket2 == 2 && HKM.cooldownsOn, UI.useTrinket2()),
                new Decorator(ret => P.myPrefs.Trinket1 == 3 && T.IsWoWBoss(Me.CurrentTarget), UI.useTrinket1()),
                new Decorator(ret => P.myPrefs.Trinket2 == 3 && T.IsWoWBoss(Me.CurrentTarget), UI.useTrinket2()),
                S.castBearForm(),
                S.castHealingTouchGuardian(),
                S.castBerserkGuardian(),
                S.castIncarnationGuardian(),
                S.castHotWGuardian(),
                S.castMaul(),
                S.castMangleGuardian(),
                S.castLacerate(),
                S.castThrashBear()
                );
        }
        
    }
}
