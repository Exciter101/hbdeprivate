using Styx.Common;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using P = DK.DKSettings;

namespace DK
{
    class DKHotkeyManagers
    {
        public static bool aoeStop { get; set; }
        public static bool cooldownsOn { get; set; }
        public static bool manualOn { get; set; }
        public static bool keysRegistered { get; set; }
        public static bool pauseRoutineOn { get; set; }

        private static ModifierKeys getPauseKey()
        {
            string usekey = P.myPrefs.ModifkeyPause;
            switch (usekey)
            {
                case "Alt": return ModifierKeys.Alt;
                case "Ctrl": return ModifierKeys.Control;
                case "Shift": return ModifierKeys.Shift;
                case "Windows": return ModifierKeys.Win;
                default: return ModifierKeys.Alt;
            }
        }
        private static ModifierKeys getCooldownsKey()
        {
            string usekey = P.myPrefs.ModifkeyCooldowns;
            switch (usekey)
            {
                case "Alt": return ModifierKeys.Alt;
                case "Ctrl": return ModifierKeys.Control;
                case "Shift": return ModifierKeys.Shift;
                case "Windows": return ModifierKeys.Win;
                default: return ModifierKeys.Alt;
            }
        }
        private static ModifierKeys getStopAoeKey()
        {
            string usekey = P.myPrefs.ModifkeyStopAoe;
            switch (usekey)
            {
                case "Alt": return ModifierKeys.Alt;
                case "Ctrl": return ModifierKeys.Control;
                case "Shift": return ModifierKeys.Shift;
                case "Windows": return ModifierKeys.Win;
                default: return ModifierKeys.Alt;
            }
        }
        private static ModifierKeys getManualKey()
        {
            string usekey = P.myPrefs.ModifkeyPlayManual;
            switch (usekey)
            {
                case "Alt": return ModifierKeys.Alt;
                case "Ctrl": return ModifierKeys.Control;
                case "Shift": return ModifierKeys.Shift;
                case "Windows": return ModifierKeys.Win;
                default: return ModifierKeys.Alt;
            }
        }


        #region [Method] - Hotkey Registration
        public static void registerHotKeys()
        {
            if (keysRegistered)
                return;

            if (P.myPrefs.KeyStopAoe != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("aoeStop", P.myPrefs.KeyStopAoe, getStopAoeKey(), ret =>
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
            }

            if (P.myPrefs.KeyUseCooldowns != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("cooldownsOn", P.myPrefs.KeyUseCooldowns, getCooldownsKey(), ret =>
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
            }
            if (P.myPrefs.KeyPlayManual != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("manualOn", P.myPrefs.KeyPlayManual, getManualKey(), ret =>
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
            }
            if (P.myPrefs.KeyPauseCR != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("pauseRoutineOn", P.myPrefs.KeyPauseCR, getPauseKey(), ret =>
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
            }
            keysRegistered = true;
            Logging.Write(" " + "\r\n");
            Lua.DoString(@"print('Hotkeys: \124cFF15E61C Registered!')" + "\r\n");
            Logging.Write(Colors.Bisque, "Stop Aoe Key: " + P.myPrefs.ModifkeyStopAoe + "+ " + P.myPrefs.KeyStopAoe);
            Logging.Write(Colors.Bisque, "Play Manual Key:  " + P.myPrefs.ModifkeyPlayManual + "+ " + P.myPrefs.KeyPlayManual);
            Logging.Write(Colors.Bisque, "Pause CR Key: " + P.myPrefs.ModifkeyPause + "+ " + P.myPrefs.KeyPauseCR);
            Logging.Write(Colors.Bisque, "Use Cooldowns Key: " + P.myPrefs.ModifkeyCooldowns + "+ " + P.myPrefs.KeyUseCooldowns);
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
            aoeStop = false;
            cooldownsOn = false;
            manualOn = false;
            pauseRoutineOn = false;
            keysRegistered = false;
            Lua.DoString(@"print('Hotkeys: \124cFFE61515 Removed!')");
            Logging.Write(Colors.OrangeRed, "Hotkeys: Removed!");
        }
        #endregion
    }
}
