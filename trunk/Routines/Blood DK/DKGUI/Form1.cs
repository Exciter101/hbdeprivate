using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


#region methods

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
using System.IO;
using Styx.Common;
#endregion

namespace DeathKnight.GUI
{
    public partial class Form1 : Form
    {
        private static int pauseSelect;
        private static int stopAoeSelect;
        private static int manualSelect;
        private static int cooldownSelect;
        private static string comboSelectPause;
        private static string comboSelectAoe;
        private static string comboSelectManual;
        private static string comboSelectCooldown;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Utilities.AssemblyDirectory +
                            @"\Routines\DeathKnight\DKImages\bg.png"))
            {
                pictureBox1.ImageLocation = Utilities.AssemblyDirectory +
                                            @"\Routines\DeathKnight\DKImages\bg.png";
            }
            if (File.Exists(Utilities.AssemblyDirectory +
                            @"\Routines\DeathKnight\DKImages\Document.rtf"))
            {
                richTextBox1.LoadFile(Utilities.AssemblyDirectory +
                                            @"\Routines\DeathKnight\DKImages\Document.rtf");
            }

            //comboboxes
            #region hotkeys
            comboSelectPause = P.myPrefs.KeyPauseCR.ToString();
            switch (comboSelectPause)
            {
                case "None":
                    cmbPause.SelectedIndex = 0;
                    break;
                case "A":
                    cmbPause.SelectedIndex = 1;
                    break;
                case "B":
                    cmbPause.SelectedIndex = 2;
                    break;
                case "C":
                    cmbPause.SelectedIndex = 3;
                    break;
                case "D":
                    cmbPause.SelectedIndex = 4;
                    break;
                case "E":
                    cmbPause.SelectedIndex = 5;
                    break;
                case "F":
                    cmbPause.SelectedIndex = 6;
                    break;
                case "G":
                    cmbPause.SelectedIndex = 7;
                    break;
                case "H":
                    cmbPause.SelectedIndex = 8;
                    break;
                case "I":
                    cmbPause.SelectedIndex = 9;
                    break;
                case "J":
                    cmbPause.SelectedIndex = 10;
                    break;
                case "K":
                    cmbPause.SelectedIndex = 11;
                    break;
                case "L":
                    cmbPause.SelectedIndex = 12;
                    break;
                case "M":
                    cmbPause.SelectedIndex = 13;
                    break;
                case "N":
                    cmbPause.SelectedIndex = 14;
                    break;
                case "O":
                    cmbPause.SelectedIndex = 15;
                    break;
                case "P":
                    cmbPause.SelectedIndex = 16;
                    break;
                case "Q":
                    cmbPause.SelectedIndex = 17;
                    break;
                case "R":
                    cmbPause.SelectedIndex = 18;
                    break;
                case "S":
                    cmbPause.SelectedIndex = 19;
                    break;
                case "T":
                    cmbPause.SelectedIndex = 20;
                    break;
                case "U":
                    cmbPause.SelectedIndex = 21;
                    break;
                case "V":
                    cmbPause.SelectedIndex = 22;
                    break;
                case "W":
                    cmbPause.SelectedIndex = 23;
                    break;
                case "X":
                    cmbPause.SelectedIndex = 24;
                    break;
                case "Y":
                    cmbPause.SelectedIndex = 25;
                    break;
                case "Z":
                    cmbPause.SelectedIndex = 26;
                    break;
            }
            comboSelectAoe = P.myPrefs.KeyStopAoe.ToString();
            switch (comboSelectAoe)
            {
                case "None":
                    cmbAoeStop.SelectedIndex = 0;
                    break;
                case "A":
                    cmbAoeStop.SelectedIndex = 1;
                    break;
                case "B":
                    cmbAoeStop.SelectedIndex = 2;
                    break;
                case "C":
                    cmbAoeStop.SelectedIndex = 3;
                    break;
                case "D":
                    cmbAoeStop.SelectedIndex = 4;
                    break;
                case "E":
                    cmbAoeStop.SelectedIndex = 5;
                    break;
                case "F":
                    cmbAoeStop.SelectedIndex = 6;
                    break;
                case "G":
                    cmbAoeStop.SelectedIndex = 7;
                    break;
                case "H":
                    cmbAoeStop.SelectedIndex = 8;
                    break;
                case "I":
                    cmbAoeStop.SelectedIndex = 9;
                    break;
                case "J":
                    cmbAoeStop.SelectedIndex = 10;
                    break;
                case "K":
                    cmbAoeStop.SelectedIndex = 11;
                    break;
                case "L":
                    cmbAoeStop.SelectedIndex = 12;
                    break;
                case "M":
                    cmbAoeStop.SelectedIndex = 13;
                    break;
                case "N":
                    cmbAoeStop.SelectedIndex = 14;
                    break;
                case "O":
                    cmbAoeStop.SelectedIndex = 15;
                    break;
                case "P":
                    cmbAoeStop.SelectedIndex = 16;
                    break;
                case "Q":
                    cmbAoeStop.SelectedIndex = 17;
                    break;
                case "R":
                    cmbAoeStop.SelectedIndex = 18;
                    break;
                case "S":
                    cmbAoeStop.SelectedIndex = 19;
                    break;
                case "T":
                    cmbAoeStop.SelectedIndex = 20;
                    break;
                case "U":
                    cmbAoeStop.SelectedIndex = 21;
                    break;
                case "V":
                    cmbAoeStop.SelectedIndex = 22;
                    break;
                case "W":
                    cmbAoeStop.SelectedIndex = 23;
                    break;
                case "X":
                    cmbAoeStop.SelectedIndex = 24;
                    break;
                case "Y":
                    cmbAoeStop.SelectedIndex = 25;
                    break;
                case "Z":
                    cmbAoeStop.SelectedIndex = 26;
                    break;
            }
            comboSelectManual = P.myPrefs.KeyPlayManual.ToString();
            switch (comboSelectManual)
            {
                case "None":
                    cmbPlayManual.SelectedIndex = 0;
                    break;
                case "A":
                    cmbPlayManual.SelectedIndex = 1;
                    break;
                case "B":
                    cmbPlayManual.SelectedIndex = 2;
                    break;
                case "C":
                    cmbPlayManual.SelectedIndex = 3;
                    break;
                case "D":
                    cmbPlayManual.SelectedIndex = 4;
                    break;
                case "E":
                    cmbPlayManual.SelectedIndex = 5;
                    break;
                case "F":
                    cmbPlayManual.SelectedIndex = 6;
                    break;
                case "G":
                    cmbPlayManual.SelectedIndex = 7;
                    break;
                case "H":
                    cmbPlayManual.SelectedIndex = 8;
                    break;
                case "I":
                    cmbPlayManual.SelectedIndex = 9;
                    break;
                case "J":
                    cmbPlayManual.SelectedIndex = 10;
                    break;
                case "K":
                    cmbPlayManual.SelectedIndex = 11;
                    break;
                case "L":
                    cmbPlayManual.SelectedIndex = 12;
                    break;
                case "M":
                    cmbPlayManual.SelectedIndex = 13;
                    break;
                case "N":
                    cmbPlayManual.SelectedIndex = 14;
                    break;
                case "O":
                    cmbPlayManual.SelectedIndex = 15;
                    break;
                case "P":
                    cmbPlayManual.SelectedIndex = 16;
                    break;
                case "Q":
                    cmbPlayManual.SelectedIndex = 17;
                    break;
                case "R":
                    cmbPlayManual.SelectedIndex = 18;
                    break;
                case "S":
                    cmbPlayManual.SelectedIndex = 19;
                    break;
                case "T":
                    cmbPlayManual.SelectedIndex = 20;
                    break;
                case "U":
                    cmbPlayManual.SelectedIndex = 21;
                    break;
                case "V":
                    cmbPlayManual.SelectedIndex = 22;
                    break;
                case "W":
                    cmbPlayManual.SelectedIndex = 23;
                    break;
                case "X":
                    cmbPlayManual.SelectedIndex = 24;
                    break;
                case "Y":
                    cmbPlayManual.SelectedIndex = 25;
                    break;
                case "Z":
                    cmbPlayManual.SelectedIndex = 26;
                    break;
            }
            comboSelectCooldown = P.myPrefs.KeyUseCooldowns.ToString();
            switch (comboSelectCooldown)
            {
                case "None":
                    cmbCooldowns.SelectedIndex = 0;
                    break;
                case "A":
                    cmbCooldowns.SelectedIndex = 1;
                    break;
                case "B":
                    cmbCooldowns.SelectedIndex = 2;
                    break;
                case "C":
                    cmbCooldowns.SelectedIndex = 3;
                    break;
                case "D":
                    cmbCooldowns.SelectedIndex = 4;
                    break;
                case "E":
                    cmbCooldowns.SelectedIndex = 5;
                    break;
                case "F":
                    cmbCooldowns.SelectedIndex = 6;
                    break;
                case "G":
                    cmbCooldowns.SelectedIndex = 7;
                    break;
                case "H":
                    cmbCooldowns.SelectedIndex = 8;
                    break;
                case "I":
                    cmbCooldowns.SelectedIndex = 9;
                    break;
                case "J":
                    cmbCooldowns.SelectedIndex = 10;
                    break;
                case "K":
                    cmbCooldowns.SelectedIndex = 11;
                    break;
                case "L":
                    cmbCooldowns.SelectedIndex = 12;
                    break;
                case "M":
                    cmbCooldowns.SelectedIndex = 13;
                    break;
                case "N":
                    cmbCooldowns.SelectedIndex = 14;
                    break;
                case "O":
                    cmbCooldowns.SelectedIndex = 15;
                    break;
                case "P":
                    cmbCooldowns.SelectedIndex = 16;
                    break;
                case "Q":
                    cmbCooldowns.SelectedIndex = 17;
                    break;
                case "R":
                    cmbCooldowns.SelectedIndex = 18;
                    break;
                case "S":
                    cmbCooldowns.SelectedIndex = 19;
                    break;
                case "T":
                    cmbCooldowns.SelectedIndex = 20;
                    break;
                case "U":
                    cmbCooldowns.SelectedIndex = 21;
                    break;
                case "V":
                    cmbCooldowns.SelectedIndex = 22;
                    break;
                case "W":
                    cmbCooldowns.SelectedIndex = 23;
                    break;
                case "X":
                    cmbCooldowns.SelectedIndex = 24;
                    break;
                case "Y":
                    cmbCooldowns.SelectedIndex = 25;
                    break;
                case "Z":
                    cmbCooldowns.SelectedIndex = 26;
                    break;
            }
            #endregion

            //radiobuttons
            if (P.myPrefs.Presence == 1) { btnBloodPresence.Checked = true; }
            if (P.myPrefs.Presence == 2) { btnFrostPresence.Checked = true; }
            if (P.myPrefs.Presence == 3) { btnUnholyPresence.Checked = true; }

            if (P.myPrefs.RaidFlaskKind == 76087) { btnRaidFlaskEarth.Checked = true; }
            if (P.myPrefs.RaidFlaskKind == 76088) { btnFlaskWinter.Checked = true; }
            if (P.myPrefs.RaidFlask == 1) { btnRaidFlaskManual.Checked = true; }
            if (P.myPrefs.RaidFlask == 2) { btnRaidFlaskRaid.Checked = true; }
            if (P.myPrefs.RaidFlask == 3) { btnRaidFlaskNotLFR.Checked = true; }
            if (P.myPrefs.RaidFlask == 4) { btnRaidFlaskDungeons.Checked = true; }

            if (P.myPrefs.Trinket1 == 1) { btnTrinket1Manual.Checked = true; }
            if (P.myPrefs.Trinket1 == 2) { btnTrinket1HP.Checked = true; }
            if (P.myPrefs.Trinket1 == 3) { btnTrinket1CD.Checked = true; }

            if (P.myPrefs.Trinket2 == 1) { btnTrinket2Manual.Checked = true; }
            if (P.myPrefs.Trinket2 == 2) { btnTrinket2HP.Checked = true; }
            if (P.myPrefs.Trinket2 == 3) { btnTrinket2CD.Checked = true; }

            if (P.myPrefs.Racial == 1) { radioButton1.Checked = true; }
            if (P.myPrefs.Racial == 2) { radioButton2.Checked = true; }
            if (P.myPrefs.Racial == 3) { btnRacialNone.Checked = true; }

            if (P.myPrefs.Gloves == 1) { radioButton4.Checked = true; }
            if (P.myPrefs.Gloves == 2) { radioButton3.Checked = true; }


            //chekboxes
            checkBox1.Checked = P.myPrefs.AutoMovement;
            checkBox2.Checked = P.myPrefs.AutoTargeting;
            checkBox3.Checked = P.myPrefs.AutoFacing;
            checkBox4.Checked = P.myPrefs.AutoMovementDisable;
            checkBox5.Checked = P.myPrefs.AutoTargetingDisable;
            checkBox6.Checked = P.myPrefs.AutoFacingDisable;
            checkBox7.Checked = P.myPrefs.PrintRaidstyleMsg;
            checkBox8.Checked = P.myPrefs.FlaskCrystal;
            checkBox9.Checked = P.myPrefs.FlaskAlchemy;
            checkBox10.Checked = P.myPrefs.AutoInterrupt;

            //numupdown
            numericUpDown1.Value = new decimal(P.myPrefs.PercentFortitude);
            numericUpDown2.Value = new decimal(P.myPrefs.PercentRuneTap);
            numericUpDown3.Value = new decimal(P.myPrefs.PercentVampiric);
            numericUpDown4.Value = new decimal(P.myPrefs.PercentHealthstone);
            numericUpDown5.Value = new decimal(P.myPrefs.PercentNaaru);
            nupTrinket1HPpercent.Value = new decimal(P.myPrefs.PercentTrinket1HP);
            nupTrinket2HPpercent.Value = new decimal(P.myPrefs.PercentTrinket2HP);
            numericUpDown6.Value = new decimal(P.myPrefs.PercentConversion);
            numericUpDown7.Value = new decimal(P.myPrefs.PercentDancing);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            HKM.keysRegistered = false;
            HKM.registerHotKeys();
            P.myPrefs.Save();
            Close();
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

        private void btnBloodPresence_CheckedChanged(object sender, EventArgs e)
        {
            if (btnBloodPresence.Checked) { P.myPrefs.Presence = 1; }
        }

        private void btnFrostPresence_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFrostPresence.Checked) { P.myPrefs.Presence = 2; }
        }

        private void btnUnholyPresence_CheckedChanged(object sender, EventArgs e)
        {
            if (btnUnholyPresence.Checked) { P.myPrefs.Presence = 3; }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.PrintRaidstyleMsg = checkBox7.Checked;
        }

        #region hotkeys
        private void cmbPause_SelectedIndexChanged(object sender, EventArgs e)
        {
            pauseSelect = cmbPause.SelectedIndex;
            switch (pauseSelect)
            {
                case 0:
                    P.myPrefs.KeyPauseCR = Keys.None;
                    break;
                case 1:
                    P.myPrefs.KeyPauseCR = Keys.A;
                    break;
                case 2:
                    P.myPrefs.KeyPauseCR = Keys.B;
                    break;
                case 3:
                    P.myPrefs.KeyPauseCR = Keys.C;
                    break;
                case 4:
                    P.myPrefs.KeyPauseCR = Keys.D;
                    break;
                case 5:
                    P.myPrefs.KeyPauseCR = Keys.E;
                    break;
                case 6:
                    P.myPrefs.KeyPauseCR = Keys.F;
                    break;
                case 7:
                    P.myPrefs.KeyPauseCR = Keys.G;
                    break;
                case 8:
                    P.myPrefs.KeyPauseCR = Keys.H;
                    break;
                case 9:
                    P.myPrefs.KeyPauseCR = Keys.I;
                    break;
                case 10:
                    P.myPrefs.KeyPauseCR = Keys.J;
                    break;
                case 11:
                    P.myPrefs.KeyPauseCR = Keys.K;
                    break;
                case 12:
                    P.myPrefs.KeyPauseCR = Keys.L;
                    break;
                case 13:
                    P.myPrefs.KeyPauseCR = Keys.M;
                    break;
                case 14:
                    P.myPrefs.KeyPauseCR = Keys.N;
                    break;
                case 15:
                    P.myPrefs.KeyPauseCR = Keys.O;
                    break;
                case 16:
                    P.myPrefs.KeyPauseCR = Keys.P;
                    break;
                case 17:
                    P.myPrefs.KeyPauseCR = Keys.Q;
                    break;
                case 18:
                    P.myPrefs.KeyPauseCR = Keys.R;
                    break;
                case 19:
                    P.myPrefs.KeyPauseCR = Keys.S;
                    break;
                case 20:
                    P.myPrefs.KeyPauseCR = Keys.T;
                    break;
                case 21:
                    P.myPrefs.KeyPauseCR = Keys.U;
                    break;
                case 22:
                    P.myPrefs.KeyPauseCR = Keys.V;
                    break;
                case 23:
                    P.myPrefs.KeyPauseCR = Keys.W;
                    break;
                case 24:
                    P.myPrefs.KeyPauseCR = Keys.X;
                    break;
                case 25:
                    P.myPrefs.KeyPauseCR = Keys.Y;
                    break;
                case 26:
                    P.myPrefs.KeyPauseCR = Keys.Z;
                    break;
            }
        }

        private void cmbAoeStop_SelectedIndexChanged(object sender, EventArgs e)
        {
            stopAoeSelect = cmbAoeStop.SelectedIndex;
            switch (stopAoeSelect)
            {
                case 0:
                    P.myPrefs.KeyStopAoe = Keys.None;
                    break;
                case 1:
                    P.myPrefs.KeyStopAoe = Keys.A;
                    break;
                case 2:
                    P.myPrefs.KeyStopAoe = Keys.B;
                    break;
                case 3:
                    P.myPrefs.KeyStopAoe = Keys.C;
                    break;
                case 4:
                    P.myPrefs.KeyStopAoe = Keys.D;
                    break;
                case 5:
                    P.myPrefs.KeyStopAoe = Keys.E;
                    break;
                case 6:
                    P.myPrefs.KeyStopAoe = Keys.F;
                    break;
                case 7:
                    P.myPrefs.KeyStopAoe = Keys.G;
                    break;
                case 8:
                    P.myPrefs.KeyStopAoe = Keys.H;
                    break;
                case 9:
                    P.myPrefs.KeyStopAoe = Keys.I;
                    break;
                case 10:
                    P.myPrefs.KeyStopAoe = Keys.J;
                    break;
                case 11:
                    P.myPrefs.KeyStopAoe = Keys.K;
                    break;
                case 12:
                    P.myPrefs.KeyStopAoe = Keys.L;
                    break;
                case 13:
                    P.myPrefs.KeyStopAoe = Keys.M;
                    break;
                case 14:
                    P.myPrefs.KeyStopAoe = Keys.N;
                    break;
                case 15:
                    P.myPrefs.KeyStopAoe = Keys.O;
                    break;
                case 16:
                    P.myPrefs.KeyStopAoe = Keys.P;
                    break;
                case 17:
                    P.myPrefs.KeyStopAoe = Keys.Q;
                    break;
                case 18:
                    P.myPrefs.KeyStopAoe = Keys.R;
                    break;
                case 19:
                    P.myPrefs.KeyStopAoe = Keys.S;
                    break;
                case 20:
                    P.myPrefs.KeyStopAoe = Keys.T;
                    break;
                case 21:
                    P.myPrefs.KeyStopAoe = Keys.U;
                    break;
                case 22:
                    P.myPrefs.KeyStopAoe = Keys.V;
                    break;
                case 23:
                    P.myPrefs.KeyStopAoe = Keys.W;
                    break;
                case 24:
                    P.myPrefs.KeyStopAoe = Keys.X;
                    break;
                case 25:
                    P.myPrefs.KeyStopAoe = Keys.Y;
                    break;
                case 26:
                    P.myPrefs.KeyStopAoe = Keys.Z;
                    break;
            }
        }

        private void cmbPlayManual_SelectedIndexChanged(object sender, EventArgs e)
        {
            manualSelect = cmbPlayManual.SelectedIndex;
            switch (manualSelect)
            {
                case 0:
                    P.myPrefs.KeyPlayManual = Keys.None;
                    break;
                case 1:
                    P.myPrefs.KeyPlayManual = Keys.A;
                    break;
                case 2:
                    P.myPrefs.KeyPlayManual = Keys.B;
                    break;
                case 3:
                    P.myPrefs.KeyPlayManual = Keys.C;
                    break;
                case 4:
                    P.myPrefs.KeyPlayManual = Keys.D;
                    break;
                case 5:
                    P.myPrefs.KeyPlayManual = Keys.E;
                    break;
                case 6:
                    P.myPrefs.KeyPlayManual = Keys.F;
                    break;
                case 7:
                    P.myPrefs.KeyPlayManual = Keys.G;
                    break;
                case 8:
                    P.myPrefs.KeyPlayManual = Keys.H;
                    break;
                case 9:
                    P.myPrefs.KeyPlayManual = Keys.I;
                    break;
                case 10:
                    P.myPrefs.KeyPlayManual = Keys.J;
                    break;
                case 11:
                    P.myPrefs.KeyPlayManual = Keys.K;
                    break;
                case 12:
                    P.myPrefs.KeyPlayManual = Keys.L;
                    break;
                case 13:
                    P.myPrefs.KeyPlayManual = Keys.M;
                    break;
                case 14:
                    P.myPrefs.KeyPlayManual = Keys.N;
                    break;
                case 15:
                    P.myPrefs.KeyPlayManual = Keys.O;
                    break;
                case 16:
                    P.myPrefs.KeyPlayManual = Keys.P;
                    break;
                case 17:
                    P.myPrefs.KeyPlayManual = Keys.Q;
                    break;
                case 18:
                    P.myPrefs.KeyPlayManual = Keys.R;
                    break;
                case 19:
                    P.myPrefs.KeyPlayManual = Keys.S;
                    break;
                case 20:
                    P.myPrefs.KeyPlayManual = Keys.T;
                    break;
                case 21:
                    P.myPrefs.KeyPlayManual = Keys.U;
                    break;
                case 22:
                    P.myPrefs.KeyPlayManual = Keys.V;
                    break;
                case 23:
                    P.myPrefs.KeyPlayManual = Keys.W;
                    break;
                case 24:
                    P.myPrefs.KeyPlayManual = Keys.X;
                    break;
                case 25:
                    P.myPrefs.KeyPlayManual = Keys.Y;
                    break;
                case 26:
                    P.myPrefs.KeyPlayManual = Keys.Z;
                    break;
            }
        }
        private void cmbCooldowns_SelectedIndexChanged(object sender, EventArgs e)
        {
            cooldownSelect = cmbCooldowns.SelectedIndex;
            switch (cooldownSelect)
            {
                case 0:
                    P.myPrefs.KeyUseCooldowns = Keys.None;
                    break;
                case 1:
                    P.myPrefs.KeyUseCooldowns = Keys.A;
                    break;
                case 2:
                    P.myPrefs.KeyUseCooldowns = Keys.B;
                    break;
                case 3:
                    P.myPrefs.KeyUseCooldowns = Keys.C;
                    break;
                case 4:
                    P.myPrefs.KeyUseCooldowns = Keys.D;
                    break;
                case 5:
                    P.myPrefs.KeyUseCooldowns = Keys.E;
                    break;
                case 6:
                    P.myPrefs.KeyUseCooldowns = Keys.F;
                    break;
                case 7:
                    P.myPrefs.KeyUseCooldowns = Keys.G;
                    break;
                case 8:
                    P.myPrefs.KeyUseCooldowns = Keys.H;
                    break;
                case 9:
                    P.myPrefs.KeyUseCooldowns = Keys.I;
                    break;
                case 10:
                    P.myPrefs.KeyUseCooldowns = Keys.J;
                    break;
                case 11:
                    P.myPrefs.KeyUseCooldowns = Keys.K;
                    break;
                case 12:
                    P.myPrefs.KeyUseCooldowns = Keys.L;
                    break;
                case 13:
                    P.myPrefs.KeyUseCooldowns = Keys.M;
                    break;
                case 14:
                    P.myPrefs.KeyUseCooldowns = Keys.N;
                    break;
                case 15:
                    P.myPrefs.KeyUseCooldowns = Keys.O;
                    break;
                case 16:
                    P.myPrefs.KeyUseCooldowns = Keys.P;
                    break;
                case 17:
                    P.myPrefs.KeyUseCooldowns = Keys.Q;
                    break;
                case 18:
                    P.myPrefs.KeyUseCooldowns = Keys.R;
                    break;
                case 19:
                    P.myPrefs.KeyUseCooldowns = Keys.S;
                    break;
                case 20:
                    P.myPrefs.KeyUseCooldowns = Keys.T;
                    break;
                case 21:
                    P.myPrefs.KeyUseCooldowns = Keys.U;
                    break;
                case 22:
                    P.myPrefs.KeyUseCooldowns = Keys.V;
                    break;
                case 23:
                    P.myPrefs.KeyUseCooldowns = Keys.W;
                    break;
                case 24:
                    P.myPrefs.KeyUseCooldowns = Keys.X;
                    break;
                case 25:
                    P.myPrefs.KeyUseCooldowns = Keys.Y;
                    break;
                case 26:
                    P.myPrefs.KeyUseCooldowns = Keys.Z;
                    break;
            }
        }
        #endregion

        private void btnRaidFlaskEarth_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRaidFlaskEarth.Checked) { P.myPrefs.RaidFlaskKind = 76087; }
        }

        private void btnFlaskWinter_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFlaskWinter.Checked) { P.myPrefs.RaidFlaskKind = 76088; }
        }

        private void btnRaidFlaskManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRaidFlaskManual.Checked) { P.myPrefs.RaidFlask = 1; }
        }

        private void btnRaidFlaskRaid_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRaidFlaskRaid.Checked) { P.myPrefs.RaidFlask = 2; }
        }

        private void btnRaidFlaskNotLFR_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRaidFlaskNotLFR.Checked) { P.myPrefs.RaidFlask = 3; }
        }

        private void btnRaidFlaskDungeons_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRaidFlaskDungeons.Checked) { P.myPrefs.RaidFlask = 4; }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.FlaskCrystal = checkBox8.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.FlaskAlchemy = checkBox9.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentFortitude = (int)numericUpDown1.Value;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentRuneTap = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentVampiric = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentHealthstone = (int)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentNaaru = (int)numericUpDown5.Value;
        }

        private void btnTrinket1Manual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket1Manual.Checked) { P.myPrefs.Trinket1 = 1; }
        }

        private void btnTrinket1HP_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket1HP.Checked) { P.myPrefs.Trinket1 = 2; }
        }

        private void btnTrinket1CD_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket1CD.Checked) { P.myPrefs.Trinket1 = 3; }
        }

        private void btnTrinket2Manual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket2Manual.Checked) { P.myPrefs.Trinket2 = 1; }
        }

        private void btnTrinket2HP_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket2HP.Checked) { P.myPrefs.Trinket2 = 2; }
        }

        private void btnTrinket2CD_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket2CD.Checked) { P.myPrefs.Trinket2 = 3; }
        }

        private void nupTrinket1HPpercent_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket1HP = (int)nupTrinket1HPpercent.Value;
        }

        private void nupTrinket2HPpercent_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket2HP = (int)nupTrinket2HPpercent.Value;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) { P.myPrefs.Racial = 1; }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) { P.myPrefs.Racial = 2; }
        }

        private void btnRacialNone_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRacialNone.Checked) { P.myPrefs.Racial = 3; }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked) { P.myPrefs.Gloves = 1; }
        }

        

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentConversion = (int)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentDancing = (int)numericUpDown7.Value;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoInterrupt = checkBox10.Checked;
        }

        private void btnRacialNone_CheckedChanged_1(object sender, EventArgs e)
        {
            if (btnRacialNone.Checked) { P.myPrefs.Racial = 3; }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked) { P.myPrefs.Gloves = 2; }
        }

        
    }
}
