using Styx.Common;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    class HotkeyManager
    {
        public static bool aoeStop { get; set; }
        public static bool cooldownsOn { get; set; }
        public static bool manualOn { get; set; }
        public static bool keysRegistered { get; set; }
        public static bool pauseRoutineOn { get; set; }
        public static bool switchBearform { get; set; }
        

        #region [Method] - Hotkey Registration
        public static void registerHotKeys()
        {
            if (keysRegistered)
                return;
            HotkeysManager.Register("aoeStop", P.myPrefs.KeyStopAoe, ModifierKeys.Alt, ret =>
            {
                aoeStop = !aoeStop;
                Lua.DoString(aoeStop ? @"print('AoE Mode: \124cFF15E61C Disabled!')" : @"print('AoE Mode: \124cFFE61515 Enabled!')");
                string msgStopAoe = "Aoe Disabled !, press Alt + " + P.myPrefs.KeyStopAoe.ToString() + " in WOW to enable Aoe again";
                string msgAoeBackOn = "Aoe Enabled !, press Alt + " + P.myPrefs.KeyStopAoe.ToString() + " in WOW to disable Aoe again";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        aoeStop ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStopAoe + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgAoeBackOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });
            HotkeysManager.Register("cooldownsOn", P.myPrefs.KeyUseCooldowns, ModifierKeys.Alt, ret =>
            {
                cooldownsOn = !cooldownsOn;
                Lua.DoString(cooldownsOn ? @"print('Cooldowns: \124cFF15E61C Enabled!')" : @"print('Cooldowns: \124cFFE61515 Disabled!')");
                string msgStop = "Burst Mode Disabled !, press Alt + " + P.myPrefs.KeyUseCooldowns.ToString() + " in WOW to enable Burst Mode again";
                string msgOn = "Burst Mode Enabled !, press Alt + " + P.myPrefs.KeyUseCooldowns.ToString() + " in WOW to disable Burst Mode again";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        cooldownsOn ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });
            HotkeysManager.Register("manualOn", P.myPrefs.KeyPlayManual, ModifierKeys.Alt, ret =>
            {
                manualOn = !manualOn;
                Lua.DoString(manualOn ? @"print('Manual Mode: \124cFF15E61C Enabled!')" : @"print('Manual Mode: \124cFFE61515 Disabled!')");
                string msgStop = "Manual Mode Disabled !, press Alt + " + P.myPrefs.KeyPlayManual.ToString() + " in WOW to enable Manual Mode again";
                string msgOn = "Manual Mode Enabled !, press Alt + " + P.myPrefs.KeyPlayManual.ToString() + " in WOW to disable Manual Mode again";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        manualOn ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });
            HotkeysManager.Register("pauseRoutineOn", P.myPrefs.KeyPauseCR, ModifierKeys.Alt, ret =>
            {
                pauseRoutineOn = !pauseRoutineOn;
                Lua.DoString(pauseRoutineOn ? @"print('Routine Paused: \124cFF15E61C Enabled!')" : @"print('Routine Paused: \124cFFE61515 Disabled!')");
                string msgStop = "Routine Running !, press Alt + " + P.myPrefs.KeyPauseCR.ToString() + " in WOW to Pause Routine again";
                string msgOn = "Routine Paused !, press Alt + " + P.myPrefs.KeyPauseCR.ToString() + " in WOW to enable Routine";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        pauseRoutineOn ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });
            HotkeysManager.Register("switchBearform", P.myPrefs.KeySwitchBearform, ModifierKeys.Alt, ret =>
            {
                switchBearform = !switchBearform;
                Lua.DoString(switchBearform ? @"print('Switching to Bear Form: \124cFF15E61C Enabled!')" : @"print('Switching Bear Form: \124cFFE61515 Canceled!')");
                string msgStop = "Switching to Cat Form !, press Alt + " + P.myPrefs.KeySwitchBearform.ToString() + " in WOW to switch back to Bear Form";
                string msgOn = "Switching to Bear Form !, press Alt + " + P.myPrefs.KeySwitchBearform.ToString() + " in WOW to switch back to Cat Form";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        switchBearform ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });
            keysRegistered = true;
            Logging.Write(" " + "\r\n");
            Lua.DoString(@"print('Hotkeys: \124cFF15E61C Registered!')" + "\r\n");
            Logging.Write(Colors.Bisque, "Stop Aoe Key: Alt + " + P.myPrefs.KeyStopAoe);
            Logging.Write(Colors.Bisque, "Play Manual Key: Alt + " + P.myPrefs.KeyPlayManual);
            Logging.Write(Colors.Bisque, "Pause Routine: Key Alt + " + P.myPrefs.KeyPauseCR);
            Logging.Write(Colors.Bisque, "Cooldowns Key: Alt + " + P.myPrefs.KeyUseCooldowns);
            Logging.Write(Colors.Bisque, "Switching Bear Form Key: Alt + " + P.myPrefs.KeySwitchBearform + "\r\n");
            Logging.Write(Colors.OrangeRed, "Hotkeys: Registered!");
        }
        #endregion

        #region [Method] - Hotkey Removal
        public static void removeHotkeys()
        {
            if (!keysRegistered)
                return;
            HotkeysManager.Unregister("aoeStop");
            HotkeysManager.Unregister("cooldownsOn");
            HotkeysManager.Unregister("pauseRoutineOn");
            HotkeysManager.Unregister("manualOn");
            HotkeysManager.Unregister("switchBearform");
            aoeStop = false;
            cooldownsOn = false;
            manualOn = false;
            pauseRoutineOn = false;
            keysRegistered = false;
            switchBearform = false;
            Lua.DoString(@"print('Hotkeys: \124cFFE61515 Removed!')");
            Logging.Write(Colors.OrangeRed, "Hotkeys: Removed!");
        }
        #endregion
    }
}
