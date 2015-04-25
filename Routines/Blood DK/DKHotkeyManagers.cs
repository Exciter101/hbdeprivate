using Styx;
using Styx.Common;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        private static string ManualHotKey()
        {
            string w = "";
            w = P.myPrefs.KeyPlayManual.ToString();
            string[] ww = w.Split(',');
            return ww[0].Trim();
        }
        private static string ManualModifKey()
        {
            string w = "";
            w = P.myPrefs.KeyPlayManual.ToString();
            string[] ww = w.Split(',');
            return ww[1].Trim();
        }

        private static string PauseHotKey()
        {
            string w = "";
            w = P.myPrefs.KeyPauseCR.ToString();
            string[] ww = w.Split(',');
            return ww[0].Trim();
        }
        private static string PauseModifKey()
        {
            string w = "";
            w = P.myPrefs.KeyPauseCR.ToString();
            string[] ww = w.Split(',');
            return ww[1].Trim();
        }

        private static string CooldownsHotKey()
        {
            string w = "";
            w = P.myPrefs.KeyUseCooldowns.ToString();
            string[] ww = w.Split(',');
            return ww[0].Trim();
        }
        private static string CooldownsModifKey()
        {
            string w = "";
            w = P.myPrefs.KeyUseCooldowns.ToString();
            string[] ww = w.Split(',');
            return ww[1].Trim();
        }


        private static string StopAoeKey()
        {
            string w = "";
            w = P.myPrefs.KeyStopAoe.ToString();
            string[] ww = w.Split(',');
            return ww[0].Trim();
        }
        private static string StopAoeModifKey()
        {
            string w = "";
            w = P.myPrefs.KeyStopAoe.ToString();
            string[] ww = w.Split(',');
            return ww[1].Trim();
        }

        private static string TankHotKey()
        {
            string w = "";
            w = P.myPrefs.KeyResTanks.ToString();
            string[] ww = w.Split(',');
            return ww[0].Trim();
        }
        private static string TankModifKey()
        {
            string w = "";
            w = P.myPrefs.KeyResTanks.ToString();
            string[] ww = w.Split(',');
            return ww[1].Trim();
        }
        private static string HealerHotKey()
        {
            string w = "";
            w = P.myPrefs.KeyResHealers.ToString();
            string[] ww = w.Split(',');
            return ww[0].Trim();
        }
        private static string HealerModifKey()
        {
            string w = "";
            w = P.myPrefs.KeyResHealers.ToString();
            string[] ww = w.Split(',');
            return ww[1].Trim();
        }
        private static string AllHotKey()
        {
            string w = "";
            w = P.myPrefs.KeyResDps.ToString();
            string[] ww = w.Split(',');
            return ww[0].Trim();
        }
        private static string AllModifKey()
        {
            string w = "";
            w = P.myPrefs.KeyResDps.ToString();
            string[] ww = w.Split(',');
            return ww[1].Trim();
        }


        private static Keys keyStopAoe { get { return DKSettings.myPrefs.KeyStopAoe == Keys.None ? (Keys)Enum.Parse(typeof(Keys), "A") : (Keys)Enum.Parse(typeof(Keys), StopAoeKey()); } }
        private static Keys keyCooldowns { get { return DKSettings.myPrefs.KeyUseCooldowns == Keys.None ? (Keys)Enum.Parse(typeof(Keys), "C") : (Keys)Enum.Parse(typeof(Keys), CooldownsHotKey()); } }
        private static Keys keyPause { get { return DKSettings.myPrefs.KeyPauseCR == Keys.None ? (Keys)Enum.Parse(typeof(Keys), "P") : (Keys)Enum.Parse(typeof(Keys), PauseHotKey()); } }
        private static Keys keyManual { get { return DKSettings.myPrefs.KeyPlayManual == Keys.None ? (Keys)Enum.Parse(typeof(Keys), "M") : (Keys)Enum.Parse(typeof(Keys), ManualHotKey()); } }
        private static Keys keyResTank { get { return DKSettings.myPrefs.KeyResTanks == Keys.None ? (Keys)Enum.Parse(typeof(Keys), "T") : (Keys)Enum.Parse(typeof(Keys), TankHotKey()); } }
        private static Keys keyResHealers { get { return DKSettings.myPrefs.KeyResHealers == Keys.None ? (Keys)Enum.Parse(typeof(Keys), "H") : (Keys)Enum.Parse(typeof(Keys), HealerHotKey()); } }
        private static Keys keyResAll { get { return DKSettings.myPrefs.KeyResDps == Keys.None ? (Keys)Enum.Parse(typeof(Keys), "A") : (Keys)Enum.Parse(typeof(Keys), AllHotKey()); } }




        private static ModifierKeys modifKeyStopAoe { get { return DKSettings.myPrefs.KeyStopAoe == Keys.None ? (ModifierKeys)Enum.Parse(typeof(ModifierKeys), "Alt") : (ModifierKeys)Enum.Parse(typeof(ModifierKeys), StopAoeModifKey()); } }
        private static ModifierKeys modifKeyCooldowns { get { return DKSettings.myPrefs.KeyUseCooldowns == Keys.None ? (ModifierKeys)Enum.Parse(typeof(ModifierKeys), "Alt") : (ModifierKeys)Enum.Parse(typeof(ModifierKeys), CooldownsModifKey()); } }
        private static ModifierKeys modifKeyPause { get { return DKSettings.myPrefs.KeyPauseCR == Keys.None ? (ModifierKeys)Enum.Parse(typeof(ModifierKeys), "Alt") : (ModifierKeys)Enum.Parse(typeof(ModifierKeys), PauseModifKey()); } }
        private static ModifierKeys modifKeyManual { get { return DKSettings.myPrefs.KeyPlayManual == Keys.None ? (ModifierKeys)Enum.Parse(typeof(ModifierKeys), "Alt") : (ModifierKeys)Enum.Parse(typeof(ModifierKeys), ManualModifKey()); } }
        private static ModifierKeys modifKeyTanks { get { return DKSettings.myPrefs.KeyResTanks == Keys.None ? (ModifierKeys)Enum.Parse(typeof(ModifierKeys), "Shift") : (ModifierKeys)Enum.Parse(typeof(ModifierKeys), TankModifKey()); } }
        private static ModifierKeys modifKeyhealers { get { return DKSettings.myPrefs.KeyResHealers == Keys.None ? (ModifierKeys)Enum.Parse(typeof(ModifierKeys), "Shift") : (ModifierKeys)Enum.Parse(typeof(ModifierKeys), HealerModifKey()); } }
        private static ModifierKeys modifKeyAll { get { return DKSettings.myPrefs.KeyResDps == Keys.None ? (ModifierKeys)Enum.Parse(typeof(ModifierKeys), "Shift") : (ModifierKeys)Enum.Parse(typeof(ModifierKeys), AllModifKey()); } }



        #region Hotkey Registration
        public static void registerHotKeys()
        {
            if (keysRegistered)
                return;


            HotkeysManager.Register("aoeStop", keyStopAoe, modifKeyStopAoe, ret =>
            {
                aoeStop = !aoeStop;
                Lua.DoString(aoeStop ? @"print('AoE Mode: \124cFF15E61C Disabled!')" : @"print('AoE Mode: \124cFFE61515 Enabled!')");
                string msgStopAoe = "Aoe Disabled !, press " + modifKeyStopAoe + " + " + keyStopAoe + " in WOW to enable Aoe again";
                string msgAoeBackOn = "Aoe Enabled !, press " + modifKeyStopAoe + " + " + keyStopAoe + " in WOW to disable Aoe again";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        aoeStop ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStopAoe + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgAoeBackOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });


            HotkeysManager.Register("cooldownsOn", keyCooldowns, modifKeyCooldowns, ret =>
            {
                cooldownsOn = !cooldownsOn;
                Lua.DoString(cooldownsOn ? @"print('Cooldowns: \124cFF15E61C Enabled!')" : @"print('Cooldowns: \124cFFE61515 Disabled!')");
                string msgStop = "Burst Mode Disabled !, press " + modifKeyCooldowns + " + " + keyCooldowns + " in WOW to enable Burst Mode again";
                string msgOn = "Burst Mode Enabled !, press " + modifKeyCooldowns + " + " + keyCooldowns + " in WOW to disable Burst Mode again";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        cooldownsOn ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });

            HotkeysManager.Register("manualOn", keyManual, modifKeyManual, ret =>
            {
                manualOn = !manualOn;
                Lua.DoString(manualOn ? @"print('Manual Mode: \124cFF15E61C Enabled!')" : @"print('Manual Mode: \124cFFE61515 Disabled!')");
                string msgStop = "Manual Mode Disabled !, press " + modifKeyManual + " + " + keyManual + " in WOW to enable Manual Mode again";
                string msgOn = "Manual Mode Enabled !, press " + modifKeyManual + " + " + keyManual + " in WOW to disable Manual Mode again";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        manualOn ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });
            HotkeysManager.Register("pauseRoutineOn", keyPause, modifKeyPause, ret =>
                {
                    pauseRoutineOn = !pauseRoutineOn;
                    Lua.DoString(pauseRoutineOn ? @"print('Routine Paused: \124cFF15E61C Enabled!')" : @"print('Routine Paused: \124cFFE61515 Disabled!')");
                    string msgStop = "Routine Running !, press " + modifKeyPause + " + " + keyPause + " in WOW to Pause Routine again";
                    string msgOn = "Routine Paused !, press " + modifKeyPause + " + " + keyPause + " in WOW to enable Routine";
                    if (P.myPrefs.PrintRaidstyleMsg)
                        Lua.DoString(
                            pauseRoutineOn ?
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                            :
                            "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
                });
            HotkeysManager.Register("resTanks", keyResTank, modifKeyTanks, ret =>
            {
                resTanks = !resTanks;
                Lua.DoString(resTanks ? @"print('Ressing Tanks Activated: \124cFF15E61C Enabled!')" : @"print('Ressing Tanks Disabled: \124cFFE61515 Disabled!')");
                string msgStop = "Ressing Tanks Disabled !, press " + modifKeyTanks + " + " + keyResTank + " in WOW to enable Res Tanks again";
                string msgOn = "Ressing Tanks Enabled !, press " + modifKeyTanks + " + " + keyResTank + " in WOW to disable Res Tanks again";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        resTanks ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });
            HotkeysManager.Register("resHealers", keyResHealers, modifKeyhealers, ret =>
            {
                resHealers = !resHealers;
                Lua.DoString(resHealers ? @"print('Ressing Healers Activated: \124cFF15E61C Enabled!')" : @"print('Ressing Healers Disabled: \124cFFE61515 Disabled!')");
                string msgStop = "Ressing Healers Disabled !, press " + modifKeyhealers + " + " + keyResHealers + " in WOW to enable Res Healers again";
                string msgOn = "Ressing Healers Enabled !, press " + modifKeyhealers + " + " + keyResHealers + " in WOW to disable Res Healers again";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        resHealers ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });
            HotkeysManager.Register("resDPS", keyResAll, modifKeyAll, ret =>
            {
                resDPS = !resDPS;
                Lua.DoString(resDPS ? @"print('Ressing DPS Activated: \124cFF15E61C Enabled!')" : @"print('Ressing DPS Disabled: \124cFFE61515 Disabled!')");
                string msgStop = "Ressing DPS Disabled !, press " + modifKeyAll + " + " + keyResAll + " in WOW to enable Res DPS again";
                string msgOn = "Ressing DPS Enabled !, press " + modifKeyAll + " + " + keyResAll + " in WOW to disable DPS Healers again";
                if (P.myPrefs.PrintRaidstyleMsg)
                    Lua.DoString(
                        resDPS ?
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgOn + "\", ChatTypeInfo[\"RAID_WARNING\"]);"
                        :
                        "RaidNotice_AddMessage(RaidWarningFrame, \"" + msgStop + "\", ChatTypeInfo[\"RAID_WARNING\"]);");
            });

            keysRegistered = true;
            Logging.Write(" " + "\r\n");
            Lua.DoString(@"print('Hotkeys: \124cFF15E61C Registered!')" + "\r\n");
            Logging.Write(Colors.Bisque, "Stop Aoe Key: " + modifKeyStopAoe + "+ " + keyStopAoe);
            Logging.Write(Colors.Bisque, "Play Manual Key:  " + modifKeyManual + "+ " + keyManual);
            Logging.Write(Colors.Bisque, "Pause CR Key: " + modifKeyPause + "+ " + keyPause);
            Logging.Write(Colors.Bisque, "Use Cooldowns Key: " + modifKeyCooldowns + "+ " + keyCooldowns);
            Logging.Write(Colors.Bisque, "Res DPS Key: " + modifKeyAll + "+ " + keyResAll);
            Logging.Write(Colors.Bisque, "Res Tanks Key: " + modifKeyTanks + "+ " + keyResTank);
            Logging.Write(Colors.Bisque, "Res Healers Key: " + modifKeyhealers + "+ " + keyResHealers);
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
