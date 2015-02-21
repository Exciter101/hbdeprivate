using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using P = DK.DKSettings;
using HKM = DK.DKHotkeyManagers;

namespace DK
{
    public partial class DKGui : Form
    {
        public DKGui()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HKM.keysRegistered = false;
            HKM.registerHotKeys();
            P.myPrefs.Save();
            Close();
        }

        private void DKgui_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = P.myPrefs.AutoMovement;
            checkBox2.Checked = P.myPrefs.AutoTargeting;
            checkBox3.Checked = P.myPrefs.AutoFacing;
            checkBox4.Checked = P.myPrefs.AutoMovementDisable;
            checkBox5.Checked = P.myPrefs.AutoTargetingDisable;
            checkBox6.Checked = P.myPrefs.AutoFacingDisable;
            checkBox7.Checked = P.myPrefs.UseDeathAndDecayRunes;
            checkBox8.Checked = P.myPrefs.UseDefileRunes;
            checkBox9.Checked = P.myPrefs.PrintRaidstyleMsg;


            #region numUpDown
            numericUpDown1.Value = new decimal(P.myPrefs.AddsDeathAndDecay);

            numericUpDown3.Value = new decimal(P.myPrefs.AddsDefile);
            numericUpDown5.Value = new decimal(P.myPrefs.IceBoundFortitude);
            numericUpDown6.Value = new decimal(P.myPrefs.DancingRuneWeapon);
            numericUpDown7.Value = new decimal(P.myPrefs.VampiricBlood);
            numericUpDown8.Value = new decimal(P.myPrefs.DeathPact);
            numericUpDown9.Value = new decimal(P.myPrefs.Conversion);
            numericUpDown10.Value = new decimal(P.myPrefs.PercentHealthstone);
            numericUpDown11.Value = new decimal(P.myPrefs.Trinket1HP);
            numericUpDown12.Value = new decimal(P.myPrefs.Trinket2HP);
            numericUpDown13.Value = new decimal(P.myPrefs.PercentNaaru);
            numericUpDown14.Value = new decimal(P.myPrefs.RuneTap);
            numericUpDown15.Value = new decimal(P.myPrefs.Gorefiend);

            #endregion

            #region radiobuttons
            if (P.myPrefs.Presence == 0) btnPresenceManual.Checked = true;
            if (P.myPrefs.Presence == 1) btnPresenceBlood.Checked = true;
            if (P.myPrefs.Presence == 2) btnPresenceFrost.Checked = true;
            if (P.myPrefs.Presence == 3) btnPresenceUnholy.Checked = true;
            #endregion

            #region modifierkeys
            //cooldowns
            if (P.myPrefs.ModifkeyCooldowns == "Alt") cmbModifKeyCooldowns.SelectedIndex = 0;
            if (P.myPrefs.ModifkeyCooldowns == "Ctrl") cmbModifKeyCooldowns.SelectedIndex = 1;
            if (P.myPrefs.ModifkeyCooldowns == "Shift") cmbModifKeyCooldowns.SelectedIndex = 2;
            if (P.myPrefs.ModifkeyCooldowns == "Windows") cmbModifKeyCooldowns.SelectedIndex = 3;
            //pause
            if (P.myPrefs.ModifkeyPause == "Alt") cmbModifKeyPause.SelectedIndex = 0;
            if (P.myPrefs.ModifkeyPause == "Ctrl") cmbModifKeyPause.SelectedIndex = 1;
            if (P.myPrefs.ModifkeyPause == "Shift") cmbModifKeyPause.SelectedIndex = 2;
            if (P.myPrefs.ModifkeyPause == "Windows") cmbModifKeyPause.SelectedIndex = 3;
            //stopaoe
            if (P.myPrefs.ModifkeyStopAoe == "Alt") cmbModifKeyStopAoe.SelectedIndex = 0;
            if (P.myPrefs.ModifkeyStopAoe == "Ctrl") cmbModifKeyStopAoe.SelectedIndex = 1;
            if (P.myPrefs.ModifkeyStopAoe == "Shift") cmbModifKeyStopAoe.SelectedIndex = 2;
            if (P.myPrefs.ModifkeyStopAoe == "Windows") cmbModifKeyStopAoe.SelectedIndex = 3;
            //play manual
            if (P.myPrefs.ModifkeyPlayManual == "Alt") cmbModifKeyPlayManual.SelectedIndex = 0;
            if (P.myPrefs.ModifkeyPlayManual == "Ctrl") cmbModifKeyPlayManual.SelectedIndex = 1;
            if (P.myPrefs.ModifkeyPlayManual == "Shift") cmbModifKeyPlayManual.SelectedIndex = 2;
            if (P.myPrefs.ModifkeyPlayManual == "Windows") cmbModifKeyPlayManual.SelectedIndex = 3;
            //res tanks
            if (P.myPrefs.ModifkeyResTanks == "Alt") cmbModifKeyResTanks.SelectedIndex = 0;
            if (P.myPrefs.ModifkeyResTanks == "Ctrl") cmbModifKeyResTanks.SelectedIndex = 1;
            if (P.myPrefs.ModifkeyResTanks == "Shift") cmbModifKeyResTanks.SelectedIndex = 2;
            if (P.myPrefs.ModifkeyResTanks == "Windows") cmbModifKeyResTanks.SelectedIndex = 3;
            //reshealers
            if (P.myPrefs.ModifkeyResHealers == "Alt") cmbModifKeyResHealers.SelectedIndex = 0;
            if (P.myPrefs.ModifkeyResHealers == "Ctrl") cmbModifKeyResHealers.SelectedIndex = 1;
            if (P.myPrefs.ModifkeyResHealers == "Shift") cmbModifKeyResHealers.SelectedIndex = 2;
            if (P.myPrefs.ModifkeyResHealers == "Windows") cmbModifKeyResHealers.SelectedIndex = 3;
            //resdps
            if (P.myPrefs.ModifkeyResDPS == "Alt") cmbModifKeyResDps.SelectedIndex = 0;
            if (P.myPrefs.ModifkeyResDPS == "Ctrl") cmbModifKeyResDps.SelectedIndex = 1;
            if (P.myPrefs.ModifkeyResDPS  == "Shift") cmbModifKeyResDps.SelectedIndex = 2;
            if (P.myPrefs.ModifkeyResDPS == "Windows") cmbModifKeyResDps.SelectedIndex = 3;
            #endregion

            #region hotkeys
            selectHotkey(P.myPrefs.KeyUseCooldowns, cmbHotkeyCooldowns);
            selectHotkey(P.myPrefs.KeyPauseCR, cmbHotkeyPause);
            selectHotkey(P.myPrefs.KeyStopAoe, cmbHotkeyStopAoe);
            selectHotkey(P.myPrefs.KeyPlayManual, cmbHotkeyPlayManual);
            selectHotkey(P.myPrefs.KeyResTanks, cmbHotkeyResTanks);
            selectHotkey(P.myPrefs.KeyResHealers, cmbHotkeyResHealers);
            selectHotkey(P.myPrefs.KeyResDps, cmbHotkeyResDps);
            #endregion
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoMovement = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoTargeting = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoFacing = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoMovementDisable = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoTargetingDisable = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoFacingDisable = checkBox6.Checked;
        }

        private void btnPresenceManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPresenceManual.Checked) P.myPrefs.Presence = 0;
        }

        private void btnPresenceBlood_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPresenceBlood.Checked) P.myPrefs.Presence = 1;
        }

        private void btnPresenceFrost_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPresenceFrost.Checked) P.myPrefs.Presence = 2;
        }

        private void btnPresenceUnholy_CheckedChanged(object sender, EventArgs e)
        {
            if (btnPresenceUnholy.Checked) P.myPrefs.Presence = 3;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.AddsDeathAndDecay = (int)numericUpDown1.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.AddsDefile = (int)numericUpDown3.Value;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.UseDeathAndDecayRunes = checkBox7.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.UseDefileRunes = checkBox8.Checked;
        }

        #region modifkeys
        private void cmbModifKeyCooldowns_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbModifKeyCooldowns.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.ModifkeyCooldowns = "Alt"; break;
                case 1: P.myPrefs.ModifkeyCooldowns = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyCooldowns = "Shift"; break;
                case 3: P.myPrefs.ModifkeyCooldowns = "Windows"; break;
            }
        }

        private void cmbModifKeyPause_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbModifKeyPause.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.ModifkeyPause = "Alt"; break;
                case 1: P.myPrefs.ModifkeyPause = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyPause = "Shift"; break;
                case 3: P.myPrefs.ModifkeyPause = "Windows"; break;
            }
        }

        private void cmbModifKeyStopAoe_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbModifKeyStopAoe.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.ModifkeyStopAoe = "Alt"; break;
                case 1: P.myPrefs.ModifkeyStopAoe = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyStopAoe = "Shift"; break;
                case 3: P.myPrefs.ModifkeyStopAoe = "Windows"; break;
            }
        }

        private void cmbModifKeyPlayManual_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbModifKeyPlayManual.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.ModifkeyPlayManual = "Alt"; break;
                case 1: P.myPrefs.ModifkeyPlayManual = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyPlayManual = "Shift"; break;
                case 3: P.myPrefs.ModifkeyPlayManual = "Windows"; break;
            }
        }

        private void cmbModifKeyResTanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbModifKeyResTanks.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.ModifkeyResTanks = "Alt"; break;
                case 1: P.myPrefs.ModifkeyResTanks = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyResTanks = "Shift"; break;
                case 3: P.myPrefs.ModifkeyResTanks = "Windows"; break;
            }
        }

        private void cmbModifKeyResHealers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbModifKeyResHealers.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.ModifkeyResHealers = "Alt"; break;
                case 1: P.myPrefs.ModifkeyResHealers = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyResHealers = "Shift"; break;
                case 3: P.myPrefs.ModifkeyResHealers = "Windows"; break;
            }
        }

        private void cmbModifKeyResDps_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbModifKeyResDps.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.ModifkeyResDPS = "Alt"; break;
                case 1: P.myPrefs.ModifkeyResDPS = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyResDPS = "Shift"; break;
                case 3: P.myPrefs.ModifkeyResDPS = "Windows"; break;
            }
        }
        #endregion

        #region hotkeys
        private void cmbHotkeyCooldowns_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbHotkeyCooldowns.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.KeyUseCooldowns = Keys.None; break;
                case 1: P.myPrefs.KeyUseCooldowns = Keys.A; break;
                case 2: P.myPrefs.KeyUseCooldowns = Keys.B; break;
                case 3: P.myPrefs.KeyUseCooldowns = Keys.C; break;
                case 4: P.myPrefs.KeyUseCooldowns = Keys.D; break;
                case 5: P.myPrefs.KeyUseCooldowns = Keys.E; break;
                case 6: P.myPrefs.KeyUseCooldowns = Keys.F; break;
                case 7: P.myPrefs.KeyUseCooldowns = Keys.G; break;
                case 8: P.myPrefs.KeyUseCooldowns = Keys.H; break;
                case 9: P.myPrefs.KeyUseCooldowns = Keys.I; break;
                case 10: P.myPrefs.KeyUseCooldowns = Keys.J; break;
                case 11: P.myPrefs.KeyUseCooldowns = Keys.K; break;
                case 12: P.myPrefs.KeyUseCooldowns = Keys.L; break;
                case 13: P.myPrefs.KeyUseCooldowns = Keys.M; break;
                case 14: P.myPrefs.KeyUseCooldowns = Keys.N; break;
                case 15: P.myPrefs.KeyUseCooldowns = Keys.O; break;
                case 16: P.myPrefs.KeyUseCooldowns = Keys.P; break;
                case 17: P.myPrefs.KeyUseCooldowns = Keys.Q; break;
                case 18: P.myPrefs.KeyUseCooldowns = Keys.R; break;
                case 19: P.myPrefs.KeyUseCooldowns = Keys.S; break;
                case 20: P.myPrefs.KeyUseCooldowns = Keys.T; break;
                case 21: P.myPrefs.KeyUseCooldowns = Keys.U; break;
                case 22: P.myPrefs.KeyUseCooldowns = Keys.V; break;
                case 23: P.myPrefs.KeyUseCooldowns = Keys.W; break;
                case 24: P.myPrefs.KeyUseCooldowns = Keys.X; break;
                case 25: P.myPrefs.KeyUseCooldowns = Keys.Y; break;
                case 26: P.myPrefs.KeyUseCooldowns = Keys.Z; break;
            }
        }

        private void selectHotkey(Keys myKey, ComboBox name)
        {
            switch (myKey.ToString().Trim())
            {
                case "None": name.SelectedIndex = 0; break;
                case "A": name.SelectedIndex = 1; break;
                case "B": name.SelectedIndex = 2; break;
                case "C": name.SelectedIndex = 3; break;
                case "D": name.SelectedIndex = 4; break;
                case "E": name.SelectedIndex = 5; break;
                case "F": name.SelectedIndex = 6; break;
                case "G": name.SelectedIndex = 7; break;
                case "H": name.SelectedIndex = 8; break;
                case "I": name.SelectedIndex = 9; break;
                case "J": name.SelectedIndex = 10; break;
                case "K": name.SelectedIndex = 11; break;
                case "L": name.SelectedIndex = 12; break;
                case "M": name.SelectedIndex = 13; break;
                case "N": name.SelectedIndex = 14; break;
                case "O": name.SelectedIndex = 15; break;
                case "P": name.SelectedIndex = 16; break;
                case "Q": name.SelectedIndex = 17; break;
                case "R": name.SelectedIndex = 18; break;
                case "S": name.SelectedIndex = 19; break;
                case "T": name.SelectedIndex = 20; break;
                case "U": name.SelectedIndex = 21; break;
                case "V": name.SelectedIndex = 22; break;
                case "W": name.SelectedIndex = 23; break;
                case "X": name.SelectedIndex = 24; break;
                case "Y": name.SelectedIndex = 25; break;
                case "Z": name.SelectedIndex = 25; break;
            }
        }

        private void cmbHotkeyPause_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbHotkeyPause.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.KeyPauseCR = Keys.None; break;
                case 1: P.myPrefs.KeyPauseCR = Keys.A; break;
                case 2: P.myPrefs.KeyPauseCR = Keys.B; break;
                case 3: P.myPrefs.KeyPauseCR = Keys.C; break;
                case 4: P.myPrefs.KeyPauseCR = Keys.D; break;
                case 5: P.myPrefs.KeyPauseCR = Keys.E; break;
                case 6: P.myPrefs.KeyPauseCR = Keys.F; break;
                case 7: P.myPrefs.KeyPauseCR = Keys.G; break;
                case 8: P.myPrefs.KeyPauseCR = Keys.H; break;
                case 9: P.myPrefs.KeyPauseCR = Keys.I; break;
                case 10: P.myPrefs.KeyPauseCR = Keys.J; break;
                case 11: P.myPrefs.KeyPauseCR = Keys.K; break;
                case 12: P.myPrefs.KeyPauseCR = Keys.L; break;
                case 13: P.myPrefs.KeyPauseCR = Keys.M; break;
                case 14: P.myPrefs.KeyPauseCR = Keys.N; break;
                case 15: P.myPrefs.KeyPauseCR = Keys.O; break;
                case 16: P.myPrefs.KeyPauseCR = Keys.P; break;
                case 17: P.myPrefs.KeyPauseCR = Keys.Q; break;
                case 18: P.myPrefs.KeyPauseCR = Keys.R; break;
                case 19: P.myPrefs.KeyPauseCR = Keys.S; break;
                case 20: P.myPrefs.KeyPauseCR = Keys.T; break;
                case 21: P.myPrefs.KeyPauseCR = Keys.U; break;
                case 22: P.myPrefs.KeyPauseCR = Keys.V; break;
                case 23: P.myPrefs.KeyPauseCR = Keys.W; break;
                case 24: P.myPrefs.KeyPauseCR = Keys.X; break;
                case 25: P.myPrefs.KeyPauseCR = Keys.Y; break;
                case 26: P.myPrefs.KeyPauseCR = Keys.Z; break;
            }
        }

        private void cmbHotkeyStopAoe_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbHotkeyStopAoe.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.KeyStopAoe = Keys.None; break;
                case 1: P.myPrefs.KeyStopAoe = Keys.A; break;
                case 2: P.myPrefs.KeyStopAoe = Keys.B; break;
                case 3: P.myPrefs.KeyStopAoe = Keys.C; break;
                case 4: P.myPrefs.KeyStopAoe = Keys.D; break;
                case 5: P.myPrefs.KeyStopAoe = Keys.E; break;
                case 6: P.myPrefs.KeyStopAoe = Keys.F; break;
                case 7: P.myPrefs.KeyStopAoe = Keys.G; break;
                case 8: P.myPrefs.KeyStopAoe = Keys.H; break;
                case 9: P.myPrefs.KeyStopAoe = Keys.I; break;
                case 10: P.myPrefs.KeyStopAoe = Keys.J; break;
                case 11: P.myPrefs.KeyStopAoe = Keys.K; break;
                case 12: P.myPrefs.KeyStopAoe = Keys.L; break;
                case 13: P.myPrefs.KeyStopAoe = Keys.M; break;
                case 14: P.myPrefs.KeyStopAoe = Keys.N; break;
                case 15: P.myPrefs.KeyStopAoe = Keys.O; break;
                case 16: P.myPrefs.KeyStopAoe = Keys.P; break;
                case 17: P.myPrefs.KeyStopAoe = Keys.Q; break;
                case 18: P.myPrefs.KeyStopAoe = Keys.R; break;
                case 19: P.myPrefs.KeyStopAoe = Keys.S; break;
                case 20: P.myPrefs.KeyStopAoe = Keys.T; break;
                case 21: P.myPrefs.KeyStopAoe = Keys.U; break;
                case 22: P.myPrefs.KeyStopAoe = Keys.V; break;
                case 23: P.myPrefs.KeyStopAoe = Keys.W; break;
                case 24: P.myPrefs.KeyStopAoe = Keys.X; break;
                case 25: P.myPrefs.KeyStopAoe = Keys.Y; break;
                case 26: P.myPrefs.KeyStopAoe = Keys.Z; break;
            }
        }

        private void cmbHotkeyPlayManual_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbHotkeyPlayManual.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.KeyPlayManual = Keys.None; break;
                case 1: P.myPrefs.KeyPlayManual = Keys.A; break;
                case 2: P.myPrefs.KeyPlayManual = Keys.B; break;
                case 3: P.myPrefs.KeyPlayManual = Keys.C; break;
                case 4: P.myPrefs.KeyPlayManual = Keys.D; break;
                case 5: P.myPrefs.KeyPlayManual = Keys.E; break;
                case 6: P.myPrefs.KeyPlayManual = Keys.F; break;
                case 7: P.myPrefs.KeyPlayManual = Keys.G; break;
                case 8: P.myPrefs.KeyPlayManual = Keys.H; break;
                case 9: P.myPrefs.KeyPlayManual = Keys.I; break;
                case 10: P.myPrefs.KeyPlayManual = Keys.J; break;
                case 11: P.myPrefs.KeyPlayManual = Keys.K; break;
                case 12: P.myPrefs.KeyPlayManual = Keys.L; break;
                case 13: P.myPrefs.KeyPlayManual = Keys.M; break;
                case 14: P.myPrefs.KeyPlayManual = Keys.N; break;
                case 15: P.myPrefs.KeyPlayManual = Keys.O; break;
                case 16: P.myPrefs.KeyPlayManual = Keys.P; break;
                case 17: P.myPrefs.KeyPlayManual = Keys.Q; break;
                case 18: P.myPrefs.KeyPlayManual = Keys.R; break;
                case 19: P.myPrefs.KeyPlayManual = Keys.S; break;
                case 20: P.myPrefs.KeyPlayManual = Keys.T; break;
                case 21: P.myPrefs.KeyPlayManual = Keys.U; break;
                case 22: P.myPrefs.KeyPlayManual = Keys.V; break;
                case 23: P.myPrefs.KeyPlayManual = Keys.W; break;
                case 24: P.myPrefs.KeyPlayManual = Keys.X; break;
                case 25: P.myPrefs.KeyPlayManual = Keys.Y; break;
                case 26: P.myPrefs.KeyPlayManual = Keys.Z; break;
            }
        }

        private void cmbHotkeyResTanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbHotkeyResTanks.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.KeyResTanks = Keys.None; break;
                case 1: P.myPrefs.KeyResTanks = Keys.A; break;
                case 2: P.myPrefs.KeyResTanks = Keys.B; break;
                case 3: P.myPrefs.KeyResTanks = Keys.C; break;
                case 4: P.myPrefs.KeyResTanks = Keys.D; break;
                case 5: P.myPrefs.KeyResTanks = Keys.E; break;
                case 6: P.myPrefs.KeyResTanks = Keys.F; break;
                case 7: P.myPrefs.KeyResTanks = Keys.G; break;
                case 8: P.myPrefs.KeyResTanks = Keys.H; break;
                case 9: P.myPrefs.KeyResTanks = Keys.I; break;
                case 10: P.myPrefs.KeyResTanks = Keys.J; break;
                case 11: P.myPrefs.KeyResTanks = Keys.K; break;
                case 12: P.myPrefs.KeyResTanks = Keys.L; break;
                case 13: P.myPrefs.KeyResTanks = Keys.M; break;
                case 14: P.myPrefs.KeyResTanks = Keys.N; break;
                case 15: P.myPrefs.KeyResTanks = Keys.O; break;
                case 16: P.myPrefs.KeyResTanks = Keys.P; break;
                case 17: P.myPrefs.KeyResTanks = Keys.Q; break;
                case 18: P.myPrefs.KeyResTanks = Keys.R; break;
                case 19: P.myPrefs.KeyResTanks = Keys.S; break;
                case 20: P.myPrefs.KeyResTanks = Keys.T; break;
                case 21: P.myPrefs.KeyResTanks = Keys.U; break;
                case 22: P.myPrefs.KeyResTanks = Keys.V; break;
                case 23: P.myPrefs.KeyResTanks = Keys.W; break;
                case 24: P.myPrefs.KeyResTanks = Keys.X; break;
                case 25: P.myPrefs.KeyResTanks = Keys.Y; break;
                case 26: P.myPrefs.KeyResTanks = Keys.Z; break;
            }
        }

        private void cmbHotkeyResHealers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbHotkeyResHealers.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.KeyResHealers = Keys.None; break;
                case 1: P.myPrefs.KeyResHealers = Keys.A; break;
                case 2: P.myPrefs.KeyResHealers = Keys.B; break;
                case 3: P.myPrefs.KeyResHealers = Keys.C; break;
                case 4: P.myPrefs.KeyResHealers = Keys.D; break;
                case 5: P.myPrefs.KeyResHealers = Keys.E; break;
                case 6: P.myPrefs.KeyResHealers = Keys.F; break;
                case 7: P.myPrefs.KeyResHealers = Keys.G; break;
                case 8: P.myPrefs.KeyResHealers = Keys.H; break;
                case 9: P.myPrefs.KeyResHealers = Keys.I; break;
                case 10: P.myPrefs.KeyResHealers = Keys.J; break;
                case 11: P.myPrefs.KeyResHealers = Keys.K; break;
                case 12: P.myPrefs.KeyResHealers = Keys.L; break;
                case 13: P.myPrefs.KeyResHealers = Keys.M; break;
                case 14: P.myPrefs.KeyResHealers = Keys.N; break;
                case 15: P.myPrefs.KeyResHealers = Keys.O; break;
                case 16: P.myPrefs.KeyResHealers = Keys.P; break;
                case 17: P.myPrefs.KeyResHealers = Keys.Q; break;
                case 18: P.myPrefs.KeyResHealers = Keys.R; break;
                case 19: P.myPrefs.KeyResHealers = Keys.S; break;
                case 20: P.myPrefs.KeyResHealers = Keys.T; break;
                case 21: P.myPrefs.KeyResHealers = Keys.U; break;
                case 22: P.myPrefs.KeyResHealers = Keys.V; break;
                case 23: P.myPrefs.KeyResHealers = Keys.W; break;
                case 24: P.myPrefs.KeyResHealers = Keys.X; break;
                case 25: P.myPrefs.KeyResHealers = Keys.Y; break;
                case 26: P.myPrefs.KeyResHealers = Keys.Z; break;
            }
        }

        private void cmbHotkeyResDps_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectKey = cmbHotkeyResDps.SelectedIndex;
            switch (selectKey)
            {
                case 0: P.myPrefs.KeyResDps = Keys.None; break;
                case 1: P.myPrefs.KeyResDps = Keys.A; break;
                case 2: P.myPrefs.KeyResDps = Keys.B; break;
                case 3: P.myPrefs.KeyResDps = Keys.C; break;
                case 4: P.myPrefs.KeyResDps = Keys.D; break;
                case 5: P.myPrefs.KeyResDps = Keys.E; break;
                case 6: P.myPrefs.KeyResDps = Keys.F; break;
                case 7: P.myPrefs.KeyResDps = Keys.G; break;
                case 8: P.myPrefs.KeyResDps = Keys.H; break;
                case 9: P.myPrefs.KeyResDps = Keys.I; break;
                case 10: P.myPrefs.KeyResDps = Keys.J; break;
                case 11: P.myPrefs.KeyResDps = Keys.K; break;
                case 12: P.myPrefs.KeyResDps = Keys.L; break;
                case 13: P.myPrefs.KeyResDps = Keys.M; break;
                case 14: P.myPrefs.KeyResDps = Keys.N; break;
                case 15: P.myPrefs.KeyResDps = Keys.O; break;
                case 16: P.myPrefs.KeyResDps = Keys.P; break;
                case 17: P.myPrefs.KeyResDps = Keys.Q; break;
                case 18: P.myPrefs.KeyResDps = Keys.R; break;
                case 19: P.myPrefs.KeyResDps = Keys.S; break;
                case 20: P.myPrefs.KeyResDps = Keys.T; break;
                case 21: P.myPrefs.KeyResDps = Keys.U; break;
                case 22: P.myPrefs.KeyResDps = Keys.V; break;
                case 23: P.myPrefs.KeyResDps = Keys.W; break;
                case 24: P.myPrefs.KeyResDps = Keys.X; break;
                case 25: P.myPrefs.KeyResDps = Keys.Y; break;
                case 26: P.myPrefs.KeyResDps = Keys.Z; break;
            }
        }
        #endregion

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.PrintRaidstyleMsg = checkBox9.Checked;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.IceBoundFortitude = (int)numericUpDown5.Value;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.DancingRuneWeapon = (int)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.VampiricBlood = (int)numericUpDown7.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.DeathPact = (int)numericUpDown8.Value;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Conversion = (int)numericUpDown9.Value;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentHealthstone = (int)numericUpDown10.Value;
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Trinket1HP = (int)numericUpDown11.Value;
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Trinket2HP = (int)numericUpDown12.Value;
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentNaaru = (int)numericUpDown13.Value;
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.RuneTap = (int)numericUpDown14.Value;
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Gorefiend = (int)numericUpDown15.Value;
        }

    }
}
