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
    class HotkeyManager
    {
        public static bool aoeStop { get; set; }
        public static bool cooldownsOn { get; set; }
        public static bool manualOn { get; set; }
        public static bool keysRegistered { get; set; }
        public static bool pauseRoutineOn { get; set; }
        

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
            keysRegistered = true;
            Lua.DoString(@"print('Hotkeys: \124cFF15E61C Registered!')" + "\r\n");
            Logging.Write(Colors.Bisque, "Stop Aoe Key: Alt + " + P.myPrefs.KeyStopAoe);
            Logging.Write(Colors.Bisque, "Play Manual Key: Alt + " + P.myPrefs.KeyPlayManual);
            Logging.Write(Colors.Bisque, "Pause Routine: Key Alt + " + P.myPrefs.KeyPauseCR);
            Logging.Write(Colors.Bisque, "Cooldowns Key: Alt + " + P.myPrefs.KeyUseCooldowns + "\r\n");
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
            HotkeysManager.Unregister("manualOn");
            aoeStop = false;
            cooldownsOn = false;
            manualOn = false;
            keysRegistered = false;
            Lua.DoString(@"print('Hotkeys: \124cFFE61515 Removed!')");
            Logging.Write(Colors.OrangeRed, "Hotkeys: Removed!");
        }
        #endregion
    }
}
