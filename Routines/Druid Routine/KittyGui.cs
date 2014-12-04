using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Styx.Common;
using Styx.WoWInternals;

using HKM = Kitty.KittyHotkeyManagers;
using P = Kitty.KittySettings;

namespace Kitty
{
    public partial class KittyGui : Form
    {
        private static string comboSelectPause;
        private static string comboSelectAoe;
        private static string comboSelectManual;
        private static string comboSelectCooldown;

        public KittyGui()
        {
            InitializeComponent();
            checkBox1.Checked = P.myPrefs.AutoMovement;
            checkBox3.Checked = P.myPrefs.AutoTargeting;
            checkBox5.Checked = P.myPrefs.AutoFacing;
            checkBox2.Checked = P.myPrefs.AutoMovementDisable;
            checkBox4.Checked = P.myPrefs.AutoTargetingDisable;
            checkBox6.Checked = P.myPrefs.AutoFacingDisable;
            checkBox7.Checked = P.myPrefs.FlaskOraliusCrystal;
            checkBox8.Checked = P.myPrefs.FlaskCrystal;
            checkBox9.Checked = P.myPrefs.FlaskAlchemy;
            checkBox10.Checked = P.myPrefs.PrintRaidstyleMsg;
            checkBox11.Checked = P.myPrefs.AutoInterrupt;
            checkBox12.Checked = P.myPrefs.PullProwlAndRake;
            checkBox13.Checked = P.myPrefs.PullProwlAndShred;
            checkBox14.Checked = P.myPrefs.Trinket1Use;
            checkBox15.Checked = P.myPrefs.Trinket2Use;

            numericUpDown1.Value = new decimal(P.myPrefs.PercentBarkskin);
            numericUpDown2.Value = new decimal(P.myPrefs.PercentFrenziedRegeneration);
            numericUpDown3.Value = new decimal(P.myPrefs.PercentSurvivalInstincts);
            numericUpDown4.Value = new decimal(P.myPrefs.PercentSavageDefense);
            numericUpDown5.Value = new decimal(P.myPrefs.PercentHealthstone);
            numericUpDown6.Value = new decimal(P.myPrefs.PercentTrinket1HP);
            numericUpDown7.Value = new decimal(P.myPrefs.PercentTrinket2HP);
            numericUpDown8.Value = new decimal(P.myPrefs.PercentRejuCombat);
            numericUpDown9.Value = new decimal(P.myPrefs.PercentSwitchBearForm);
            numericUpDown10.Value = new decimal(P.myPrefs.PercentTrinket1Energy);
            numericUpDown11.Value = new decimal(P.myPrefs.PercentTrinket1Mana);
            numericUpDown12.Value = new decimal(P.myPrefs.PercentTrinket2Energy);
            numericUpDown13.Value = new decimal(P.myPrefs.PercentTrinket2Mana);


            #region modifier keys
            comboSelectCooldown = P.myPrefs.ModifkeyCooldowns;
            if (comboSelectCooldown == "Alt") cmbUseCooldownsModifier.SelectedIndex = 0;
            if (comboSelectCooldown == "Ctrl") cmbUseCooldownsModifier.SelectedIndex = 1;
            if (comboSelectCooldown == "Shift") cmbUseCooldownsModifier.SelectedIndex = 2;
            if (comboSelectCooldown == "Windows") cmbUseCooldownsModifier.SelectedIndex = 3;

            comboSelectPause = P.myPrefs.ModifkeyPause;
            if (comboSelectPause == "Alt") cmbPauseCRModifier.SelectedIndex = 0;
            if (comboSelectPause == "Ctrl") cmbPauseCRModifier.SelectedIndex = 1;
            if (comboSelectPause == "Shift") cmbPauseCRModifier.SelectedIndex = 2;
            if (comboSelectPause == "Windows") cmbPauseCRModifier.SelectedIndex = 3;

            comboSelectAoe = P.myPrefs.ModifkeyStopAoe;
            if (comboSelectAoe == "Alt") cmbStopAoeModifier.SelectedIndex = 0;
            if (comboSelectAoe == "Ctrl") cmbStopAoeModifier.SelectedIndex = 1;
            if (comboSelectAoe == "Shift") cmbStopAoeModifier.SelectedIndex = 2;
            if (comboSelectAoe == "Windows") cmbStopAoeModifier.SelectedIndex = 3;

            comboSelectManual = P.myPrefs.ModifkeyPlayManual;
            if (comboSelectManual == "Alt") cmbPlayManualModifier.SelectedIndex = 0;
            if (comboSelectManual == "Ctrl") cmbPlayManualModifier.SelectedIndex = 1;
            if (comboSelectManual == "Shift") cmbPlayManualModifier.SelectedIndex = 2;
            if (comboSelectManual == "Windows") cmbPlayManualModifier.SelectedIndex = 3;
            #endregion

            #region combobox HOTKEYS
            //cooldowns
            if (P.myPrefs.KeyUseCooldowns == Keys.None) cmbUseCooldowns.SelectedIndex = 0;
            if (P.myPrefs.KeyUseCooldowns == Keys.A) cmbUseCooldowns.SelectedIndex = 1;
            if (P.myPrefs.KeyUseCooldowns == Keys.B) cmbUseCooldowns.SelectedIndex = 2;
            if (P.myPrefs.KeyUseCooldowns == Keys.C) cmbUseCooldowns.SelectedIndex = 3;
            if (P.myPrefs.KeyUseCooldowns == Keys.D) cmbUseCooldowns.SelectedIndex = 4;
            if (P.myPrefs.KeyUseCooldowns == Keys.E) cmbUseCooldowns.SelectedIndex = 5;
            if (P.myPrefs.KeyUseCooldowns == Keys.F) cmbUseCooldowns.SelectedIndex = 6;
            if (P.myPrefs.KeyUseCooldowns == Keys.G) cmbUseCooldowns.SelectedIndex = 7;
            if (P.myPrefs.KeyUseCooldowns == Keys.H) cmbUseCooldowns.SelectedIndex = 8;
            if (P.myPrefs.KeyUseCooldowns == Keys.I) cmbUseCooldowns.SelectedIndex = 9;
            if (P.myPrefs.KeyUseCooldowns == Keys.J) cmbUseCooldowns.SelectedIndex = 10;
            if (P.myPrefs.KeyUseCooldowns == Keys.K) cmbUseCooldowns.SelectedIndex = 11;
            if (P.myPrefs.KeyUseCooldowns == Keys.L) cmbUseCooldowns.SelectedIndex = 12;
            if (P.myPrefs.KeyUseCooldowns == Keys.M) cmbUseCooldowns.SelectedIndex = 13;
            if (P.myPrefs.KeyUseCooldowns == Keys.N) cmbUseCooldowns.SelectedIndex = 14;
            if (P.myPrefs.KeyUseCooldowns == Keys.O) cmbUseCooldowns.SelectedIndex = 15;
            if (P.myPrefs.KeyUseCooldowns == Keys.P) cmbUseCooldowns.SelectedIndex = 16;
            if (P.myPrefs.KeyUseCooldowns == Keys.Q) cmbUseCooldowns.SelectedIndex = 17;
            if (P.myPrefs.KeyUseCooldowns == Keys.R) cmbUseCooldowns.SelectedIndex = 18;
            if (P.myPrefs.KeyUseCooldowns == Keys.S) cmbUseCooldowns.SelectedIndex = 19;
            if (P.myPrefs.KeyUseCooldowns == Keys.T) cmbUseCooldowns.SelectedIndex = 20;
            if (P.myPrefs.KeyUseCooldowns == Keys.U) cmbUseCooldowns.SelectedIndex = 21;
            if (P.myPrefs.KeyUseCooldowns == Keys.V) cmbUseCooldowns.SelectedIndex = 22;
            if (P.myPrefs.KeyUseCooldowns == Keys.W) cmbUseCooldowns.SelectedIndex = 23;
            if (P.myPrefs.KeyUseCooldowns == Keys.X) cmbUseCooldowns.SelectedIndex = 24;
            if (P.myPrefs.KeyUseCooldowns == Keys.Y) cmbUseCooldowns.SelectedIndex = 25;
            if (P.myPrefs.KeyUseCooldowns == Keys.Z) cmbUseCooldowns.SelectedIndex = 26;
            //pause
            if (P.myPrefs.KeyPauseCR == Keys.None) cmbPauseCR.SelectedIndex = 0;
            if (P.myPrefs.KeyPauseCR == Keys.A) cmbPauseCR.SelectedIndex = 1;
            if (P.myPrefs.KeyPauseCR == Keys.B) cmbPauseCR.SelectedIndex = 2;
            if (P.myPrefs.KeyPauseCR == Keys.C) cmbPauseCR.SelectedIndex = 3;
            if (P.myPrefs.KeyPauseCR == Keys.D) cmbPauseCR.SelectedIndex = 4;
            if (P.myPrefs.KeyPauseCR == Keys.E) cmbPauseCR.SelectedIndex = 5;
            if (P.myPrefs.KeyPauseCR == Keys.F) cmbPauseCR.SelectedIndex = 6;
            if (P.myPrefs.KeyPauseCR == Keys.G) cmbPauseCR.SelectedIndex = 7;
            if (P.myPrefs.KeyPauseCR == Keys.H) cmbPauseCR.SelectedIndex = 8;
            if (P.myPrefs.KeyPauseCR == Keys.I) cmbPauseCR.SelectedIndex = 9;
            if (P.myPrefs.KeyPauseCR == Keys.J) cmbPauseCR.SelectedIndex = 10;
            if (P.myPrefs.KeyPauseCR == Keys.K) cmbPauseCR.SelectedIndex = 11;
            if (P.myPrefs.KeyPauseCR == Keys.L) cmbPauseCR.SelectedIndex = 12;
            if (P.myPrefs.KeyPauseCR == Keys.M) cmbPauseCR.SelectedIndex = 13;
            if (P.myPrefs.KeyPauseCR == Keys.N) cmbPauseCR.SelectedIndex = 14;
            if (P.myPrefs.KeyPauseCR == Keys.O) cmbPauseCR.SelectedIndex = 15;
            if (P.myPrefs.KeyPauseCR == Keys.P) cmbPauseCR.SelectedIndex = 16;
            if (P.myPrefs.KeyPauseCR == Keys.Q) cmbPauseCR.SelectedIndex = 17;
            if (P.myPrefs.KeyPauseCR == Keys.R) cmbPauseCR.SelectedIndex = 18;
            if (P.myPrefs.KeyPauseCR == Keys.S) cmbPauseCR.SelectedIndex = 19;
            if (P.myPrefs.KeyPauseCR == Keys.T) cmbPauseCR.SelectedIndex = 20;
            if (P.myPrefs.KeyPauseCR == Keys.U) cmbPauseCR.SelectedIndex = 21;
            if (P.myPrefs.KeyPauseCR == Keys.V) cmbPauseCR.SelectedIndex = 22;
            if (P.myPrefs.KeyPauseCR == Keys.W) cmbPauseCR.SelectedIndex = 23;
            if (P.myPrefs.KeyPauseCR == Keys.X) cmbPauseCR.SelectedIndex = 24;
            if (P.myPrefs.KeyPauseCR == Keys.Y) cmbPauseCR.SelectedIndex = 25;
            if (P.myPrefs.KeyPauseCR == Keys.Z) cmbPauseCR.SelectedIndex = 26;
            //stopAoe
            if (P.myPrefs.KeyStopAoe == Keys.None) cmbStopAoe.SelectedIndex = 0;
            if (P.myPrefs.KeyStopAoe == Keys.A) cmbStopAoe.SelectedIndex = 1;
            if (P.myPrefs.KeyStopAoe == Keys.B) cmbStopAoe.SelectedIndex = 2;
            if (P.myPrefs.KeyStopAoe == Keys.C) cmbStopAoe.SelectedIndex = 3;
            if (P.myPrefs.KeyStopAoe == Keys.D) cmbStopAoe.SelectedIndex = 4;
            if (P.myPrefs.KeyStopAoe == Keys.E) cmbStopAoe.SelectedIndex = 5;
            if (P.myPrefs.KeyStopAoe == Keys.F) cmbStopAoe.SelectedIndex = 6;
            if (P.myPrefs.KeyStopAoe == Keys.G) cmbStopAoe.SelectedIndex = 7;
            if (P.myPrefs.KeyStopAoe == Keys.H) cmbStopAoe.SelectedIndex = 8;
            if (P.myPrefs.KeyStopAoe == Keys.I) cmbStopAoe.SelectedIndex = 9;
            if (P.myPrefs.KeyStopAoe == Keys.J) cmbStopAoe.SelectedIndex = 10;
            if (P.myPrefs.KeyStopAoe == Keys.K) cmbStopAoe.SelectedIndex = 11;
            if (P.myPrefs.KeyStopAoe == Keys.L) cmbStopAoe.SelectedIndex = 12;
            if (P.myPrefs.KeyStopAoe == Keys.M) cmbStopAoe.SelectedIndex = 13;
            if (P.myPrefs.KeyStopAoe == Keys.N) cmbStopAoe.SelectedIndex = 14;
            if (P.myPrefs.KeyStopAoe == Keys.O) cmbStopAoe.SelectedIndex = 15;
            if (P.myPrefs.KeyStopAoe == Keys.P) cmbStopAoe.SelectedIndex = 16;
            if (P.myPrefs.KeyStopAoe == Keys.Q) cmbStopAoe.SelectedIndex = 17;
            if (P.myPrefs.KeyStopAoe == Keys.R) cmbStopAoe.SelectedIndex = 18;
            if (P.myPrefs.KeyStopAoe == Keys.S) cmbStopAoe.SelectedIndex = 19;
            if (P.myPrefs.KeyStopAoe == Keys.T) cmbStopAoe.SelectedIndex = 20;
            if (P.myPrefs.KeyStopAoe == Keys.U) cmbStopAoe.SelectedIndex = 21;
            if (P.myPrefs.KeyStopAoe == Keys.V) cmbStopAoe.SelectedIndex = 22;
            if (P.myPrefs.KeyStopAoe == Keys.W) cmbStopAoe.SelectedIndex = 23;
            if (P.myPrefs.KeyStopAoe == Keys.X) cmbStopAoe.SelectedIndex = 24;
            if (P.myPrefs.KeyStopAoe == Keys.Y) cmbStopAoe.SelectedIndex = 25;
            if (P.myPrefs.KeyStopAoe == Keys.Z) cmbStopAoe.SelectedIndex = 26;
            //play manual
            if (P.myPrefs.KeyPlayManual == Keys.None) cmbPlayManual.SelectedIndex = 0;
            if (P.myPrefs.KeyPlayManual == Keys.A) cmbPlayManual.SelectedIndex = 1;
            if (P.myPrefs.KeyPlayManual == Keys.B) cmbPlayManual.SelectedIndex = 2;
            if (P.myPrefs.KeyPlayManual == Keys.C) cmbPlayManual.SelectedIndex = 3;
            if (P.myPrefs.KeyPlayManual == Keys.D) cmbPlayManual.SelectedIndex = 4;
            if (P.myPrefs.KeyPlayManual == Keys.E) cmbPlayManual.SelectedIndex = 5;
            if (P.myPrefs.KeyPlayManual == Keys.F) cmbPlayManual.SelectedIndex = 6;
            if (P.myPrefs.KeyPlayManual == Keys.G) cmbPlayManual.SelectedIndex = 7;
            if (P.myPrefs.KeyPlayManual == Keys.H) cmbPlayManual.SelectedIndex = 8;
            if (P.myPrefs.KeyPlayManual == Keys.I) cmbPlayManual.SelectedIndex = 9;
            if (P.myPrefs.KeyPlayManual == Keys.J) cmbPlayManual.SelectedIndex = 10;
            if (P.myPrefs.KeyPlayManual == Keys.K) cmbPlayManual.SelectedIndex = 11;
            if (P.myPrefs.KeyPlayManual == Keys.L) cmbPlayManual.SelectedIndex = 12;
            if (P.myPrefs.KeyPlayManual == Keys.M) cmbPlayManual.SelectedIndex = 13;
            if (P.myPrefs.KeyPlayManual == Keys.N) cmbPlayManual.SelectedIndex = 14;
            if (P.myPrefs.KeyPlayManual == Keys.O) cmbPlayManual.SelectedIndex = 15;
            if (P.myPrefs.KeyPlayManual == Keys.P) cmbPlayManual.SelectedIndex = 16;
            if (P.myPrefs.KeyPlayManual == Keys.Q) cmbPlayManual.SelectedIndex = 17;
            if (P.myPrefs.KeyPlayManual == Keys.R) cmbPlayManual.SelectedIndex = 18;
            if (P.myPrefs.KeyPlayManual == Keys.S) cmbPlayManual.SelectedIndex = 19;
            if (P.myPrefs.KeyPlayManual == Keys.T) cmbPlayManual.SelectedIndex = 20;
            if (P.myPrefs.KeyPlayManual == Keys.U) cmbPlayManual.SelectedIndex = 21;
            if (P.myPrefs.KeyPlayManual == Keys.V) cmbPlayManual.SelectedIndex = 22;
            if (P.myPrefs.KeyPlayManual == Keys.W) cmbPlayManual.SelectedIndex = 23;
            if (P.myPrefs.KeyPlayManual == Keys.X) cmbPlayManual.SelectedIndex = 24;
            if (P.myPrefs.KeyPlayManual == Keys.Y) cmbPlayManual.SelectedIndex = 25;
            if (P.myPrefs.KeyPlayManual == Keys.Z) cmbPlayManual.SelectedIndex = 26;
            #endregion

            #region trinkets
            if (P.myPrefs.Trinket1 == 1) radioButton1.Checked = true;
            if (P.myPrefs.Trinket1 == 2) radioButton2.Checked = true;
            if (P.myPrefs.Trinket1 == 3) radioButton3.Checked = true;
            if (P.myPrefs.Trinket1 == 4) radioButton4.Checked = true;
            if (P.myPrefs.Trinket1 == 5) radioButton5.Checked = true;
            if (P.myPrefs.Trinket1 == 6) radioButton6.Checked = true;
            if (P.myPrefs.Trinket2 == 1) radioButton7.Checked = true;
            if (P.myPrefs.Trinket2 == 2) radioButton8.Checked = true;
            if (P.myPrefs.Trinket2 == 3) radioButton9.Checked = true;
            if (P.myPrefs.Trinket2 == 4) radioButton10.Checked = true;
            if (P.myPrefs.Trinket2 == 5) radioButton11.Checked = true;
            if (P.myPrefs.Trinket2 == 6) radioButton12.Checked = true;
            #endregion

        }

        private void button1_Click(object sender, EventArgs e)
        {
            HKM.keysRegistered = false;
            HKM.registerHotKeys();
            KittySettings.myPrefs.Save();
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoMovement = checkBox1.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoTargeting = checkBox3.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoFacing = checkBox5.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoMovementDisable = checkBox2.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoTargetingDisable = checkBox4.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoFacingDisable = checkBox6.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.FlaskOraliusCrystal = checkBox7.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.FlaskCrystal = checkBox8.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.FlaskAlchemy = checkBox9.Checked;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.PrintRaidstyleMsg = checkBox10.Checked;
        }

        #region modifiers key
        private void cmbUseCooldownsModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            int keySelect = cmbUseCooldownsModifier.SelectedIndex;
            switch (keySelect)
            {
                case 0: P.myPrefs.ModifkeyCooldowns = "Alt"; break;
                case 1: P.myPrefs.ModifkeyCooldowns = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyCooldowns = "Shift"; break;
                case 3: P.myPrefs.ModifkeyCooldowns = "Windows"; break;
            }
        }

        private void cmbPauseCRModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            int keyselect = cmbPauseCRModifier.SelectedIndex;
            switch (keyselect)
            {
                case 0: P.myPrefs.ModifkeyPause = "Alt"; break;
                case 1: P.myPrefs.ModifkeyPause = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyPause = "Shift"; break;
                case 3: P.myPrefs.ModifkeyPause = "Windows"; break;
            }
        }

        private void cmbStopAoeModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            int keyselect = cmbStopAoeModifier.SelectedIndex;
            switch (keyselect)
            {
                case 0: P.myPrefs.ModifkeyStopAoe = "Alt"; break;
                case 1: P.myPrefs.ModifkeyStopAoe = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyStopAoe = "Shift"; break;
                case 3: P.myPrefs.ModifkeyStopAoe = "Windows"; break;
            }
        }

        private void cmbPlayManualModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            int keyselect = cmbPlayManualModifier.SelectedIndex;
            switch (keyselect)
            {
                case 0: P.myPrefs.ModifkeyPlayManual = "Alt"; break;
                case 1: P.myPrefs.ModifkeyPlayManual = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyPlayManual = "Shift"; break;
                case 3: P.myPrefs.ModifkeyPlayManual = "Windows"; break;
            }
        }
        #endregion

        #region hotkeys
        private void cmbUseCooldowns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUseCooldowns.SelectedIndex == 0) P.myPrefs.KeyUseCooldowns = Keys.None;
            if (cmbUseCooldowns.SelectedIndex == 1) P.myPrefs.KeyUseCooldowns = Keys.A;
            if (cmbUseCooldowns.SelectedIndex == 2) P.myPrefs.KeyUseCooldowns = Keys.B;
            if (cmbUseCooldowns.SelectedIndex == 3) P.myPrefs.KeyUseCooldowns = Keys.C;
            if (cmbUseCooldowns.SelectedIndex == 4) P.myPrefs.KeyUseCooldowns = Keys.D;
            if (cmbUseCooldowns.SelectedIndex == 5) P.myPrefs.KeyUseCooldowns = Keys.E;
            if (cmbUseCooldowns.SelectedIndex == 6) P.myPrefs.KeyUseCooldowns = Keys.F;
            if (cmbUseCooldowns.SelectedIndex == 7) P.myPrefs.KeyUseCooldowns = Keys.G;
            if (cmbUseCooldowns.SelectedIndex == 8) P.myPrefs.KeyUseCooldowns = Keys.H;
            if (cmbUseCooldowns.SelectedIndex == 9) P.myPrefs.KeyUseCooldowns = Keys.I;
            if (cmbUseCooldowns.SelectedIndex == 10) P.myPrefs.KeyUseCooldowns = Keys.J;
            if (cmbUseCooldowns.SelectedIndex == 11) P.myPrefs.KeyUseCooldowns = Keys.K;
            if (cmbUseCooldowns.SelectedIndex == 12) P.myPrefs.KeyUseCooldowns = Keys.L;
            if (cmbUseCooldowns.SelectedIndex == 13) P.myPrefs.KeyUseCooldowns = Keys.M;
            if (cmbUseCooldowns.SelectedIndex == 14) P.myPrefs.KeyUseCooldowns = Keys.N;
            if (cmbUseCooldowns.SelectedIndex == 15) P.myPrefs.KeyUseCooldowns = Keys.O;
            if (cmbUseCooldowns.SelectedIndex == 16) P.myPrefs.KeyUseCooldowns = Keys.P;
            if (cmbUseCooldowns.SelectedIndex == 17) P.myPrefs.KeyUseCooldowns = Keys.Q;
            if (cmbUseCooldowns.SelectedIndex == 18) P.myPrefs.KeyUseCooldowns = Keys.R;
            if (cmbUseCooldowns.SelectedIndex == 19) P.myPrefs.KeyUseCooldowns = Keys.S;
            if (cmbUseCooldowns.SelectedIndex == 20) P.myPrefs.KeyUseCooldowns = Keys.T;
            if (cmbUseCooldowns.SelectedIndex == 21) P.myPrefs.KeyUseCooldowns = Keys.U;
            if (cmbUseCooldowns.SelectedIndex == 22) P.myPrefs.KeyUseCooldowns = Keys.V;
            if (cmbUseCooldowns.SelectedIndex == 23) P.myPrefs.KeyUseCooldowns = Keys.W;
            if (cmbUseCooldowns.SelectedIndex == 24) P.myPrefs.KeyUseCooldowns = Keys.X;
            if (cmbUseCooldowns.SelectedIndex == 25) P.myPrefs.KeyUseCooldowns = Keys.Y;
            if (cmbUseCooldowns.SelectedIndex == 26) P.myPrefs.KeyUseCooldowns = Keys.Z;
        }

        private void cmbPauseCR_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPauseCR.SelectedIndex == 0) P.myPrefs.KeyPauseCR = Keys.None;
            if (cmbPauseCR.SelectedIndex == 1) P.myPrefs.KeyPauseCR = Keys.A;
            if (cmbPauseCR.SelectedIndex == 2) P.myPrefs.KeyPauseCR = Keys.B;
            if (cmbPauseCR.SelectedIndex == 3) P.myPrefs.KeyPauseCR = Keys.C;
            if (cmbPauseCR.SelectedIndex == 4) P.myPrefs.KeyPauseCR = Keys.D;
            if (cmbPauseCR.SelectedIndex == 5) P.myPrefs.KeyPauseCR = Keys.E;
            if (cmbPauseCR.SelectedIndex == 6) P.myPrefs.KeyPauseCR = Keys.F;
            if (cmbPauseCR.SelectedIndex == 7) P.myPrefs.KeyPauseCR = Keys.G;
            if (cmbPauseCR.SelectedIndex == 8) P.myPrefs.KeyPauseCR = Keys.H;
            if (cmbPauseCR.SelectedIndex == 9) P.myPrefs.KeyPauseCR = Keys.I;
            if (cmbPauseCR.SelectedIndex == 10) P.myPrefs.KeyPauseCR = Keys.J;
            if (cmbPauseCR.SelectedIndex == 11) P.myPrefs.KeyPauseCR = Keys.K;
            if (cmbPauseCR.SelectedIndex == 12) P.myPrefs.KeyPauseCR = Keys.L;
            if (cmbPauseCR.SelectedIndex == 13) P.myPrefs.KeyPauseCR = Keys.M;
            if (cmbPauseCR.SelectedIndex == 14) P.myPrefs.KeyPauseCR = Keys.N;
            if (cmbPauseCR.SelectedIndex == 15) P.myPrefs.KeyPauseCR = Keys.O;
            if (cmbPauseCR.SelectedIndex == 16) P.myPrefs.KeyPauseCR = Keys.P;
            if (cmbPauseCR.SelectedIndex == 17) P.myPrefs.KeyPauseCR = Keys.Q;
            if (cmbPauseCR.SelectedIndex == 18) P.myPrefs.KeyPauseCR = Keys.R;
            if (cmbPauseCR.SelectedIndex == 19) P.myPrefs.KeyPauseCR = Keys.S;
            if (cmbPauseCR.SelectedIndex == 20) P.myPrefs.KeyPauseCR = Keys.T;
            if (cmbPauseCR.SelectedIndex == 21) P.myPrefs.KeyPauseCR = Keys.U;
            if (cmbPauseCR.SelectedIndex == 22) P.myPrefs.KeyPauseCR = Keys.V;
            if (cmbPauseCR.SelectedIndex == 23) P.myPrefs.KeyPauseCR = Keys.W;
            if (cmbPauseCR.SelectedIndex == 24) P.myPrefs.KeyPauseCR = Keys.X;
            if (cmbPauseCR.SelectedIndex == 25) P.myPrefs.KeyPauseCR = Keys.Y;
            if (cmbPauseCR.SelectedIndex == 26) P.myPrefs.KeyPauseCR = Keys.Z;
        }

        private void cmbStopAoe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStopAoe.SelectedIndex == 0) P.myPrefs.KeyStopAoe = Keys.None;
            if (cmbStopAoe.SelectedIndex == 1) P.myPrefs.KeyStopAoe = Keys.A;
            if (cmbStopAoe.SelectedIndex == 2) P.myPrefs.KeyStopAoe = Keys.B;
            if (cmbStopAoe.SelectedIndex == 3) P.myPrefs.KeyStopAoe = Keys.C;
            if (cmbStopAoe.SelectedIndex == 4) P.myPrefs.KeyStopAoe = Keys.D;
            if (cmbStopAoe.SelectedIndex == 5) P.myPrefs.KeyStopAoe = Keys.E;
            if (cmbStopAoe.SelectedIndex == 6) P.myPrefs.KeyStopAoe = Keys.F;
            if (cmbStopAoe.SelectedIndex == 7) P.myPrefs.KeyStopAoe = Keys.G;
            if (cmbStopAoe.SelectedIndex == 8) P.myPrefs.KeyStopAoe = Keys.H;
            if (cmbStopAoe.SelectedIndex == 9) P.myPrefs.KeyStopAoe = Keys.I;
            if (cmbStopAoe.SelectedIndex == 10) P.myPrefs.KeyStopAoe = Keys.J;
            if (cmbStopAoe.SelectedIndex == 11) P.myPrefs.KeyStopAoe = Keys.K;
            if (cmbStopAoe.SelectedIndex == 12) P.myPrefs.KeyStopAoe = Keys.L;
            if (cmbStopAoe.SelectedIndex == 13) P.myPrefs.KeyStopAoe = Keys.M;
            if (cmbStopAoe.SelectedIndex == 14) P.myPrefs.KeyStopAoe = Keys.N;
            if (cmbStopAoe.SelectedIndex == 15) P.myPrefs.KeyStopAoe = Keys.O;
            if (cmbStopAoe.SelectedIndex == 16) P.myPrefs.KeyStopAoe = Keys.P;
            if (cmbStopAoe.SelectedIndex == 17) P.myPrefs.KeyStopAoe = Keys.Q;
            if (cmbStopAoe.SelectedIndex == 18) P.myPrefs.KeyStopAoe = Keys.R;
            if (cmbStopAoe.SelectedIndex == 19) P.myPrefs.KeyStopAoe = Keys.S;
            if (cmbStopAoe.SelectedIndex == 20) P.myPrefs.KeyStopAoe = Keys.T;
            if (cmbStopAoe.SelectedIndex == 21) P.myPrefs.KeyStopAoe = Keys.U;
            if (cmbStopAoe.SelectedIndex == 22) P.myPrefs.KeyStopAoe = Keys.V;
            if (cmbStopAoe.SelectedIndex == 23) P.myPrefs.KeyStopAoe = Keys.W;
            if (cmbStopAoe.SelectedIndex == 24) P.myPrefs.KeyStopAoe = Keys.X;
            if (cmbStopAoe.SelectedIndex == 25) P.myPrefs.KeyStopAoe = Keys.Y;
            if (cmbStopAoe.SelectedIndex == 26) P.myPrefs.KeyStopAoe = Keys.Z;
        }

        private void cmbPlayManual_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPlayManual.SelectedIndex == 0) P.myPrefs.KeyPlayManual = Keys.None;
            if (cmbPlayManual.SelectedIndex == 1) P.myPrefs.KeyPlayManual = Keys.A;
            if (cmbPlayManual.SelectedIndex == 2) P.myPrefs.KeyPlayManual = Keys.B;
            if (cmbPlayManual.SelectedIndex == 3) P.myPrefs.KeyPlayManual = Keys.C;
            if (cmbPlayManual.SelectedIndex == 4) P.myPrefs.KeyPlayManual = Keys.D;
            if (cmbPlayManual.SelectedIndex == 5) P.myPrefs.KeyPlayManual = Keys.E;
            if (cmbPlayManual.SelectedIndex == 6) P.myPrefs.KeyPlayManual = Keys.F;
            if (cmbPlayManual.SelectedIndex == 7) P.myPrefs.KeyPlayManual = Keys.G;
            if (cmbPlayManual.SelectedIndex == 8) P.myPrefs.KeyPlayManual = Keys.H;
            if (cmbPlayManual.SelectedIndex == 9) P.myPrefs.KeyPlayManual = Keys.I;
            if (cmbPlayManual.SelectedIndex == 10) P.myPrefs.KeyPlayManual = Keys.J;
            if (cmbPlayManual.SelectedIndex == 11) P.myPrefs.KeyPlayManual = Keys.K;
            if (cmbPlayManual.SelectedIndex == 12) P.myPrefs.KeyPlayManual = Keys.L;
            if (cmbPlayManual.SelectedIndex == 13) P.myPrefs.KeyPlayManual = Keys.M;
            if (cmbPlayManual.SelectedIndex == 14) P.myPrefs.KeyPlayManual = Keys.N;
            if (cmbPlayManual.SelectedIndex == 15) P.myPrefs.KeyPlayManual = Keys.O;
            if (cmbPlayManual.SelectedIndex == 16) P.myPrefs.KeyPlayManual = Keys.P;
            if (cmbPlayManual.SelectedIndex == 17) P.myPrefs.KeyPlayManual = Keys.Q;
            if (cmbPlayManual.SelectedIndex == 18) P.myPrefs.KeyPlayManual = Keys.R;
            if (cmbPlayManual.SelectedIndex == 19) P.myPrefs.KeyPlayManual = Keys.S;
            if (cmbPlayManual.SelectedIndex == 20) P.myPrefs.KeyPlayManual = Keys.T;
            if (cmbPlayManual.SelectedIndex == 21) P.myPrefs.KeyPlayManual = Keys.U;
            if (cmbPlayManual.SelectedIndex == 22) P.myPrefs.KeyPlayManual = Keys.V;
            if (cmbPlayManual.SelectedIndex == 23) P.myPrefs.KeyPlayManual = Keys.W;
            if (cmbPlayManual.SelectedIndex == 24) P.myPrefs.KeyPlayManual = Keys.X;
            if (cmbPlayManual.SelectedIndex == 25) P.myPrefs.KeyPlayManual = Keys.Y;
            if (cmbPlayManual.SelectedIndex == 26) P.myPrefs.KeyPlayManual = Keys.Z;
        }
        #endregion hotkeys

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoInterrupt = checkBox11.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.PullProwlAndRake = checkBox12.Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.PullProwlAndShred = checkBox13.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentBarkskin = (int)numericUpDown1.Value;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentFrenziedRegeneration = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentSurvivalInstincts = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentSavageDefense = (int)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentHealthstone = (int)numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket1HP = (int)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket2HP = (int)numericUpDown7.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentRejuCombat = (int)numericUpDown8.Value;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentSwitchBearForm = (int)numericUpDown9.Value;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) P.myPrefs.Trinket1 = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) P.myPrefs.Trinket1 = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked) P.myPrefs.Trinket1 = 3;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked) P.myPrefs.Trinket1 = 4;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked) P.myPrefs.Trinket1 = 5;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked) P.myPrefs.Trinket1 = 6;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked) P.myPrefs.Trinket2 = 1;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked) P.myPrefs.Trinket2 = 2;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked) P.myPrefs.Trinket2 = 3;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked) P.myPrefs.Trinket2 = 4;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked) P.myPrefs.Trinket2 = 5;
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton12.Checked) P.myPrefs.Trinket2 = 6;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket1Energy = (int)numericUpDown10.Value;
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket2Mana = (int)numericUpDown13.Value;
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket2Energy = (int)numericUpDown12.Value;
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket1Mana = (int)numericUpDown11.Value;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.Trinket1Use = checkBox14.Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.Trinket2Use = checkBox15.Checked;
        }
    }
}
