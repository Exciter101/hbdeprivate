using Styx.Common;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using P = Kitty.KittySettings;

namespace Kitty
{
    class KittyHotkeyManagers
    {
        public static bool aoeStop { get; set; }
        public static bool cooldownsOn { get; set; }
        public static bool manualOn { get; set; }
        public static bool keysRegistered { get; set; }
        public static bool pauseRoutineOn { get; set; }
        public static bool resTanks { get; set; }
        public static bool resHealers { get; set; }
        public static bool resAll { get; set; }

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
        private static ModifierKeys getResTankKey()
        {
            string usekey = P.myPrefs.ModifkeyResTanks;
            switch (usekey)
            {
                case "Alt": return ModifierKeys.Alt;
                case "Ctrl": return ModifierKeys.Control;
                case "Shift": return ModifierKeys.Shift;
                case "Windows": return ModifierKeys.Win;
                default: return ModifierKeys.Alt;
            }
        }
        private static ModifierKeys getResHealersKey()
        {
            string usekey = P.myPrefs.ModifkeyResHealers;
            switch (usekey)
            {
                case "Alt": return ModifierKeys.Alt;
                case "Ctrl": return ModifierKeys.Control;
                case "Shift": return ModifierKeys.Shift;
                case "Windows": return ModifierKeys.Win;
                default: return ModifierKeys.Alt;
            }
        }
        private static ModifierKeys getResAllKey()
        {
            string usekey = P.myPrefs.ModifkeyResAll;
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
            if (P.myPrefs.KeyResTanks != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("resTanks", P.myPrefs.KeyResTanks, getResTankKey(), ret =>
                {
                    resTanks = !resTanks;
                    Lua.DoString(resTanks ? @"print('Ressing Tanks: \124cFF15E61C Enabled!')" : @"print('Ressing Tanks: \124cFFE61515 Disabled!')");
                    string msgResTanksOn = "Ressing Tanks Enabled !, press Alt + " + P.myPrefs.KeyResTanks.ToString() + " in WOW to disable Ressing Tanks again";
                    string msgResTanksOff = "Ressing Tanks Disabled !, press Alt + " + P.myPrefs.KeyResTanks.ToString() + " in WOW to enable Ressing Tanks again";
                    if (P.myPrefs.PrintRaidstyleMsg)
                        Lua.DoString(
                            resTanks ?
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgResTanksOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                            :
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgResTanksOff + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
                });
            }
            if (P.myPrefs.KeyReshealers != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("resHealers", P.myPrefs.KeyReshealers, getResHealersKey(), ret =>
                {
                    resHealers = !resHealers;
                    Lua.DoString(resHealers ? @"print('Ressing Healers: \124cFF15E61C Enabled!')" : @"print('Ressing Healers: \124cFFE61515 Disabled!')");
                    string msgResHealersOn = "Ressing Healers Enabled !, press Alt + " + P.myPrefs.KeyReshealers.ToString() + " in WOW to disable Ressing Healers again";
                    string msgResHealersOff = "Ressing Healers Disabled !, press Alt + " + P.myPrefs.KeyReshealers.ToString() + " in WOW to enable Ressing Healers again";
                    if (P.myPrefs.PrintRaidstyleMsg)
                        Lua.DoString(
                            resHealers ?
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgResHealersOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                            :
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgResHealersOff + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
                });
            }
            if (P.myPrefs.KeyResAll != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("resAll", P.myPrefs.KeyResAll, getResAllKey(), ret =>
                {
                    resAll = !resAll;
                    Lua.DoString(resAll ? @"print('Ressing All: \124cFF15E61C Enabled!')" : @"print('Ressing All: \124cFFE61515 Disabled!')");
                    string msgResAllOn = "Ressing All Enabled !, press Alt + " + P.myPrefs.KeyResAll.ToString() + " in WOW to disable Ressing All again";
                    string msgResAllOff = "Ressing All Disabled !, press Alt + " + P.myPrefs.KeyResAll.ToString() + " in WOW to enable Ressing All again";
                    if (P.myPrefs.PrintRaidstyleMsg)
                        Lua.DoString(
                            resAll ?
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgResAllOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                            :
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgResAllOff + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
                });
            }
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
            Logging.Write(Colors.Bisque, "\r\n" + "Hotkeys to Res People:");
            Logging.Write(Colors.Bisque, "Res Tanks Key: " + P.myPrefs.ModifkeyResTanks + "+ " + P.myPrefs.KeyResTanks);
            Logging.Write(Colors.Bisque, "Res Healers Key: " + P.myPrefs.ModifkeyResHealers + "+ " + P.myPrefs.KeyReshealers);
            Logging.Write(Colors.Bisque, "Res All Key: " + P.myPrefs.ModifkeyResAll + "+ " + P.myPrefs.KeyResAll);
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
            HotkeysManager.Unregister("resTanks");
            HotkeysManager.Unregister("reshealers");
            HotkeysManager.Unregister("resAll");
            aoeStop = false;
            cooldownsOn = false;
            manualOn = false;
            pauseRoutineOn = false;
            resTanks = false;
            resHealers = false;
            resAll = false;
            keysRegistered = false;
            Lua.DoString(@"print('Hotkeys: \124cFFE61515 Removed!')");
            Logging.Write(Colors.OrangeRed, "Hotkeys: Removed!");
        }
        #endregion
    }
}
