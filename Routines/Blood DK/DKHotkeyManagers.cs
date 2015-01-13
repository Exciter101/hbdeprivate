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
        public static bool resHealers { get; set; }
        public static bool resTanks { get; set; }
        public static bool resDPS { get; set; }


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
        private static ModifierKeys getResTanksKey()
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
        private static ModifierKeys getResDpsKey()
        {
            string usekey = P.myPrefs.ModifkeyResDPS;
            switch (usekey)
            {
                case "Alt": return ModifierKeys.Alt;
                case "Ctrl": return ModifierKeys.Control;
                case "Shift": return ModifierKeys.Shift;
                case "Windows": return ModifierKeys.Win;
                default: return ModifierKeys.Alt;
            }
        }
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
                    string msgStopAoe = "Aoe Disabled !, press " + P.myPrefs.ModifkeyStopAoe + " + " + P.myPrefs.KeyStopAoe.ToString() + " in WOW to enable Aoe again";
                    string msgAoeBackOn = "Aoe Enabled !, press " + P.myPrefs.ModifkeyStopAoe + " + " + P.myPrefs.KeyStopAoe.ToString() + " in WOW to disable Aoe again";
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
                    string msgStop = "Burst Mode Disabled !, press " + P.myPrefs.ModifkeyCooldowns + " + " + P.myPrefs.KeyUseCooldowns.ToString() + " in WOW to enable Burst Mode again";
                    string msgOn = "Burst Mode Enabled !, press " + P.myPrefs.ModifkeyCooldowns + " + " + P.myPrefs.KeyUseCooldowns.ToString() + " in WOW to disable Burst Mode again";
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
                    string msgStop = "Manual Mode Disabled !, press " + P.myPrefs.ModifkeyPlayManual + " + " + P.myPrefs.KeyPlayManual.ToString() + " in WOW to enable Manual Mode again";
                    string msgOn = "Manual Mode Enabled !, press " + P.myPrefs.ModifkeyPlayManual + " + " + P.myPrefs.KeyPlayManual.ToString() + " in WOW to disable Manual Mode again";
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
                        string msgStop = "Routine Running !, press " + P.myPrefs.ModifkeyPause + " + " +  P.myPrefs.KeyPauseCR.ToString() + " in WOW to Pause Routine again";
                        string msgOn = "Routine Paused !, press " + P.myPrefs.ModifkeyPause + " + " + P.myPrefs.KeyPauseCR.ToString() + " in WOW to enable Routine";
                        if (P.myPrefs.PrintRaidstyleMsg)
                            Lua.DoString(
                                pauseRoutineOn ?
                                "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                                :
                                "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
                    });
            }
            if (P.myPrefs.KeyResTanks != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("resTanks", P.myPrefs.KeyResTanks, getResTanksKey(), ret =>
                {
                    resTanks = !resTanks;
                    Lua.DoString(resTanks ? @"print('Ressing Tanks Activated: \124cFF15E61C Enabled!')" : @"print('Ressing Tanks Disabled: \124cFFE61515 Disabled!')");
                    string msgStop = "Ressing Tanks Disabled !, press " + P.myPrefs.ModifkeyResTanks + " + " + P.myPrefs.KeyResTanks.ToString() + " in WOW to enable Res Tanks again";
                    string msgOn = "Ressing Tanks Enabled !, press " + P.myPrefs.ModifkeyResTanks + " + " + P.myPrefs.KeyResTanks.ToString() + " in WOW to disable Res Tanks again";
                    if (P.myPrefs.PrintRaidstyleMsg)
                        Lua.DoString(
                            resTanks ?
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                            :
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
                });
            }
            if (P.myPrefs.KeyResHealers != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("resHealers", P.myPrefs.KeyResHealers, getResHealersKey(), ret =>
                {
                    resHealers = !resHealers;
                    Lua.DoString(resHealers ? @"print('Ressing Healers Activated: \124cFF15E61C Enabled!')" : @"print('Ressing Healers Disabled: \124cFFE61515 Disabled!')");
                    string msgStop = "Ressing Healers Disabled !, press " + P.myPrefs.ModifkeyResHealers + " + " + P.myPrefs.KeyResHealers.ToString() + " in WOW to enable Res Healers again";
                    string msgOn = "Ressing Healers Enabled !, press " + P.myPrefs.ModifkeyResHealers + " + " + P.myPrefs.KeyResHealers.ToString() + " in WOW to disable Res Healers again";
                    if (P.myPrefs.PrintRaidstyleMsg)
                        Lua.DoString(
                            resHealers ?
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                            :
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
                });
            }
            if (P.myPrefs.KeyResDps != System.Windows.Forms.Keys.None)
            {
                HotkeysManager.Register("resDPS", P.myPrefs.KeyResDps, getResDpsKey(), ret =>
                {
                    resDPS = !resDPS;
                    Lua.DoString(resDPS ? @"print('Ressing DPS Activated: \124cFF15E61C Enabled!')" : @"print('Ressing DPS Disabled: \124cFFE61515 Disabled!')");
                    string msgStop = "Ressing DPS Disabled !, press " + P.myPrefs.ModifkeyResDPS + " + " + P.myPrefs.KeyResDps.ToString() + " in WOW to enable Res DPS again";
                    string msgOn = "Ressing DPS Enabled !, press " + P.myPrefs.ModifkeyResDPS + " + " + P.myPrefs.KeyResDps.ToString() + " in WOW to disable DPS Healers again";
                    if (P.myPrefs.PrintRaidstyleMsg)
                        Lua.DoString(
                            resDPS ?
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
            Logging.Write(Colors.Bisque, "Res DPS Key: " + P.myPrefs.ModifkeyResDPS + "+ " + P.myPrefs.KeyResDps);
            Logging.Write(Colors.Bisque, "Res Tanks Key: " + P.myPrefs.ModifkeyResTanks + "+ " + P.myPrefs.KeyResTanks);
            Logging.Write(Colors.Bisque, "Res Healers Key: " + P.myPrefs.ModifkeyResHealers + "+ " + P.myPrefs.KeyResHealers);
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
