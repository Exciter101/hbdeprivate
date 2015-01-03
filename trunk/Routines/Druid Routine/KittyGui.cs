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
        private static string comboSelectResTanks;
        private static string comboSelectResHealers;
        private static string comboSelectResAll;

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
            checkBox16.Checked = P.myPrefs.PullWildCharge;
            checkBox17.Checked = P.myPrefs.AutoDispel;
            checkBox18.Checked = P.myPrefs.Trinket1UseResto;
            checkBox19.Checked = P.myPrefs.Trinket2UseResto;

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
            numericUpDown14.Value = new decimal(P.myPrefs.Rejuvenation5);
            numericUpDown15.Value = new decimal(P.myPrefs.Regrowth5);
            numericUpDown16.Value = new decimal(P.myPrefs.HealingTouch5);
            numericUpDown17.Value = new decimal(P.myPrefs.WildGrowth5);
            numericUpDown18.Value = new decimal(P.myPrefs.WildGrowthPlayers5);
            numericUpDown19.Value = new decimal(P.myPrefs.Tranquility5);
            numericUpDown20.Value = new decimal(P.myPrefs.TranquilityPlayers5);
            numericUpDown21.Value = new decimal(P.myPrefs.Genesis5);
            numericUpDown22.Value = new decimal(P.myPrefs.GenesisPlayers5);
            numericUpDown23.Value = new decimal(P.myPrefs.GenesisPlayers510);
            numericUpDown24.Value = new decimal(P.myPrefs.Genesis510);
            numericUpDown25.Value = new decimal(P.myPrefs.TranquilityPlayers510);
            numericUpDown26.Value = new decimal(P.myPrefs.Tranquility510);
            numericUpDown27.Value = new decimal(P.myPrefs.WildGrowthPlayers510);
            numericUpDown28.Value = new decimal(P.myPrefs.WildGrowth510);
            numericUpDown29.Value = new decimal(P.myPrefs.HealingTouch510);
            numericUpDown30.Value = new decimal(P.myPrefs.Regrowth510);
            numericUpDown31.Value = new decimal(P.myPrefs.Rejuvenation510);
            numericUpDown32.Value = new decimal(P.myPrefs.GenesisPlayers50);
            numericUpDown33.Value = new decimal(P.myPrefs.Genesis50);
            numericUpDown34.Value = new decimal(P.myPrefs.TranquilityPlayers50);
            numericUpDown35.Value = new decimal(P.myPrefs.Tranquility50);
            numericUpDown36.Value = new decimal(P.myPrefs.WildGrowthPlayers50);
            numericUpDown37.Value = new decimal(P.myPrefs.WildGrowth50);
            numericUpDown38.Value = new decimal(P.myPrefs.HealingTouch50);
            numericUpDown39.Value = new decimal(P.myPrefs.Regrowth50);
            numericUpDown40.Value = new decimal(P.myPrefs.Rejuvenation50);
            numericUpDown41.Value = new decimal(P.myPrefs.ForceOfNature5);
            numericUpDown42.Value = new decimal(P.myPrefs.ForceOfNature510);
            numericUpDown43.Value = new decimal(P.myPrefs.ForceOfNature50);
            numericUpDown44.Value = new decimal(P.myPrefs.Swiftmend5);
            numericUpDown45.Value = new decimal(P.myPrefs.Swiftmend510);
            numericUpDown46.Value = new decimal(P.myPrefs.Swiftmend50);
            numericUpDown47.Value = new decimal(P.myPrefs.PercentTrinket1ManaResto);
            numericUpDown48.Value = new decimal(P.myPrefs.PercentTrinket1EnergyResto);
            numericUpDown49.Value = new decimal(P.myPrefs.PercentTrinket1HPResto);
            numericUpDown50.Value = new decimal(P.myPrefs.PercentTrinket2ManaResto);
            numericUpDown51.Value = new decimal(P.myPrefs.PercentTrinket2EnergyResto);
            numericUpDown52.Value = new decimal(P.myPrefs.PercentTrinket2HPResto);
            numericUpDown53.Value = new decimal(P.myPrefs.PercentHealingTouchCombat);
            numericUpDown54.Value = new decimal(P.myPrefs.AoeCat);
            numericUpDown55.Value = new decimal(P.myPrefs.AoeMoonkin);


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

            comboSelectResTanks = P.myPrefs.ModifkeyResTanks;
            if (comboSelectResTanks == "Alt") cmbModifierResTanks.SelectedIndex = 0;
            if (comboSelectResTanks == "Ctrl") cmbModifierResTanks.SelectedIndex = 1;
            if (comboSelectResTanks == "Shift") cmbModifierResTanks.SelectedIndex = 2;
            if (comboSelectResTanks == "Windows") cmbModifierResTanks.SelectedIndex = 3;

            comboSelectResHealers = P.myPrefs.ModifkeyResHealers;
            if (comboSelectResHealers == "Alt") cmbModifierResHealers.SelectedIndex = 0;
            if (comboSelectResHealers == "Ctrl") cmbModifierResHealers.SelectedIndex = 1;
            if (comboSelectResHealers == "Shift") cmbModifierResHealers.SelectedIndex = 2;
            if (comboSelectResHealers == "Windows") cmbModifierResHealers.SelectedIndex = 3;

            comboSelectResAll = P.myPrefs.ModifkeyResAll;
            if (comboSelectResAll == "Alt") cmbModifierResAll.SelectedIndex = 0;
            if (comboSelectResAll == "Ctrl") cmbModifierResAll.SelectedIndex = 1;
            if (comboSelectResAll == "Shift") cmbModifierResAll.SelectedIndex = 2;
            if (comboSelectResAll == "Windows") cmbModifierResAll.SelectedIndex = 3;
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
            //res tanks
            if (P.myPrefs.KeyResTanks == Keys.None) cmbHotkeyResTanks.SelectedIndex = 0;
            if (P.myPrefs.KeyResTanks == Keys.A) cmbHotkeyResTanks.SelectedIndex = 1;
            if (P.myPrefs.KeyResTanks == Keys.B) cmbHotkeyResTanks.SelectedIndex = 2;
            if (P.myPrefs.KeyResTanks == Keys.C) cmbHotkeyResTanks.SelectedIndex = 3;
            if (P.myPrefs.KeyResTanks == Keys.D) cmbHotkeyResTanks.SelectedIndex = 4;
            if (P.myPrefs.KeyResTanks == Keys.E) cmbHotkeyResTanks.SelectedIndex = 5;
            if (P.myPrefs.KeyResTanks == Keys.F) cmbHotkeyResTanks.SelectedIndex = 6;
            if (P.myPrefs.KeyResTanks == Keys.G) cmbHotkeyResTanks.SelectedIndex = 7;
            if (P.myPrefs.KeyResTanks == Keys.H) cmbHotkeyResTanks.SelectedIndex = 8;
            if (P.myPrefs.KeyResTanks == Keys.I) cmbHotkeyResTanks.SelectedIndex = 9;
            if (P.myPrefs.KeyResTanks == Keys.J) cmbHotkeyResTanks.SelectedIndex = 10;
            if (P.myPrefs.KeyResTanks == Keys.K) cmbHotkeyResTanks.SelectedIndex = 11;
            if (P.myPrefs.KeyResTanks == Keys.L) cmbHotkeyResTanks.SelectedIndex = 12;
            if (P.myPrefs.KeyResTanks == Keys.M) cmbHotkeyResTanks.SelectedIndex = 13;
            if (P.myPrefs.KeyResTanks == Keys.N) cmbHotkeyResTanks.SelectedIndex = 14;
            if (P.myPrefs.KeyResTanks == Keys.O) cmbHotkeyResTanks.SelectedIndex = 15;
            if (P.myPrefs.KeyResTanks == Keys.P) cmbHotkeyResTanks.SelectedIndex = 16;
            if (P.myPrefs.KeyResTanks == Keys.Q) cmbHotkeyResTanks.SelectedIndex = 17;
            if (P.myPrefs.KeyResTanks == Keys.R) cmbHotkeyResTanks.SelectedIndex = 18;
            if (P.myPrefs.KeyResTanks == Keys.S) cmbHotkeyResTanks.SelectedIndex = 19;
            if (P.myPrefs.KeyResTanks == Keys.T) cmbHotkeyResTanks.SelectedIndex = 20;
            if (P.myPrefs.KeyResTanks == Keys.U) cmbHotkeyResTanks.SelectedIndex = 21;
            if (P.myPrefs.KeyResTanks == Keys.V) cmbHotkeyResTanks.SelectedIndex = 22;
            if (P.myPrefs.KeyResTanks == Keys.W) cmbHotkeyResTanks.SelectedIndex = 23;
            if (P.myPrefs.KeyResTanks == Keys.X) cmbHotkeyResTanks.SelectedIndex = 24;
            if (P.myPrefs.KeyResTanks == Keys.Y) cmbHotkeyResTanks.SelectedIndex = 25;
            if (P.myPrefs.KeyResTanks == Keys.Z) cmbHotkeyResTanks.SelectedIndex = 26;
            //res healers
            if (P.myPrefs.KeyReshealers == Keys.None) cmbHotkeyResHealers.SelectedIndex = 0;
            if (P.myPrefs.KeyReshealers == Keys.A) cmbHotkeyResHealers.SelectedIndex = 1;
            if (P.myPrefs.KeyReshealers == Keys.B) cmbHotkeyResHealers.SelectedIndex = 2;
            if (P.myPrefs.KeyReshealers == Keys.C) cmbHotkeyResHealers.SelectedIndex = 3;
            if (P.myPrefs.KeyReshealers == Keys.D) cmbHotkeyResHealers.SelectedIndex = 4;
            if (P.myPrefs.KeyReshealers == Keys.E) cmbHotkeyResHealers.SelectedIndex = 5;
            if (P.myPrefs.KeyReshealers == Keys.F) cmbHotkeyResHealers.SelectedIndex = 6;
            if (P.myPrefs.KeyReshealers == Keys.G) cmbHotkeyResHealers.SelectedIndex = 7;
            if (P.myPrefs.KeyReshealers == Keys.H) cmbHotkeyResHealers.SelectedIndex = 8;
            if (P.myPrefs.KeyReshealers == Keys.I) cmbHotkeyResHealers.SelectedIndex = 9;
            if (P.myPrefs.KeyReshealers == Keys.J) cmbHotkeyResHealers.SelectedIndex = 10;
            if (P.myPrefs.KeyReshealers == Keys.K) cmbHotkeyResHealers.SelectedIndex = 11;
            if (P.myPrefs.KeyReshealers == Keys.L) cmbHotkeyResHealers.SelectedIndex = 12;
            if (P.myPrefs.KeyReshealers == Keys.M) cmbHotkeyResHealers.SelectedIndex = 13;
            if (P.myPrefs.KeyReshealers == Keys.N) cmbHotkeyResHealers.SelectedIndex = 14;
            if (P.myPrefs.KeyReshealers == Keys.O) cmbHotkeyResHealers.SelectedIndex = 15;
            if (P.myPrefs.KeyReshealers == Keys.P) cmbHotkeyResHealers.SelectedIndex = 16;
            if (P.myPrefs.KeyReshealers == Keys.Q) cmbHotkeyResHealers.SelectedIndex = 17;
            if (P.myPrefs.KeyReshealers == Keys.R) cmbHotkeyResHealers.SelectedIndex = 18;
            if (P.myPrefs.KeyReshealers == Keys.S) cmbHotkeyResHealers.SelectedIndex = 19;
            if (P.myPrefs.KeyReshealers == Keys.T) cmbHotkeyResHealers.SelectedIndex = 20;
            if (P.myPrefs.KeyReshealers == Keys.U) cmbHotkeyResHealers.SelectedIndex = 21;
            if (P.myPrefs.KeyReshealers == Keys.V) cmbHotkeyResHealers.SelectedIndex = 22;
            if (P.myPrefs.KeyReshealers == Keys.W) cmbHotkeyResHealers.SelectedIndex = 23;
            if (P.myPrefs.KeyReshealers == Keys.X) cmbHotkeyResHealers.SelectedIndex = 24;
            if (P.myPrefs.KeyReshealers == Keys.Y) cmbHotkeyResHealers.SelectedIndex = 25;
            if (P.myPrefs.KeyReshealers == Keys.Z) cmbHotkeyResHealers.SelectedIndex = 26;
            //res all
            if (P.myPrefs.KeyResAll == Keys.None) cmbHotkeyResAll.SelectedIndex = 0;
            if (P.myPrefs.KeyResAll == Keys.A) cmbHotkeyResAll.SelectedIndex = 1;
            if (P.myPrefs.KeyResAll == Keys.B) cmbHotkeyResAll.SelectedIndex = 2;
            if (P.myPrefs.KeyResAll == Keys.C) cmbHotkeyResAll.SelectedIndex = 3;
            if (P.myPrefs.KeyResAll == Keys.D) cmbHotkeyResAll.SelectedIndex = 4;
            if (P.myPrefs.KeyResAll == Keys.E) cmbHotkeyResAll.SelectedIndex = 5;
            if (P.myPrefs.KeyResAll == Keys.F) cmbHotkeyResAll.SelectedIndex = 6;
            if (P.myPrefs.KeyResAll == Keys.G) cmbHotkeyResAll.SelectedIndex = 7;
            if (P.myPrefs.KeyResAll == Keys.H) cmbHotkeyResAll.SelectedIndex = 8;
            if (P.myPrefs.KeyResAll == Keys.I) cmbHotkeyResAll.SelectedIndex = 9;
            if (P.myPrefs.KeyResAll == Keys.J) cmbHotkeyResAll.SelectedIndex = 10;
            if (P.myPrefs.KeyResAll == Keys.K) cmbHotkeyResAll.SelectedIndex = 11;
            if (P.myPrefs.KeyResAll == Keys.L) cmbHotkeyResAll.SelectedIndex = 12;
            if (P.myPrefs.KeyResAll == Keys.M) cmbHotkeyResAll.SelectedIndex = 13;
            if (P.myPrefs.KeyResAll == Keys.N) cmbHotkeyResAll.SelectedIndex = 14;
            if (P.myPrefs.KeyResAll == Keys.O) cmbHotkeyResAll.SelectedIndex = 15;
            if (P.myPrefs.KeyResAll == Keys.P) cmbHotkeyResAll.SelectedIndex = 16;
            if (P.myPrefs.KeyResAll == Keys.Q) cmbHotkeyResAll.SelectedIndex = 17;
            if (P.myPrefs.KeyResAll == Keys.R) cmbHotkeyResAll.SelectedIndex = 18;
            if (P.myPrefs.KeyResAll == Keys.S) cmbHotkeyResAll.SelectedIndex = 19;
            if (P.myPrefs.KeyResAll == Keys.T) cmbHotkeyResAll.SelectedIndex = 20;
            if (P.myPrefs.KeyResAll == Keys.U) cmbHotkeyResAll.SelectedIndex = 21;
            if (P.myPrefs.KeyResAll == Keys.V) cmbHotkeyResAll.SelectedIndex = 22;
            if (P.myPrefs.KeyResAll == Keys.W) cmbHotkeyResAll.SelectedIndex = 23;
            if (P.myPrefs.KeyResAll == Keys.X) cmbHotkeyResAll.SelectedIndex = 24;
            if (P.myPrefs.KeyResAll == Keys.Y) cmbHotkeyResAll.SelectedIndex = 25;
            if (P.myPrefs.KeyResAll == Keys.Z) cmbHotkeyResAll.SelectedIndex = 26;
            //bearform
            if (P.myPrefs.SwitchBearKey == P.PressBearFormKey.None) comboBox1.SelectedIndex = 0;
            if (P.myPrefs.SwitchBearKey == P.PressBearFormKey.RSHIFT) comboBox1.SelectedIndex = 1;
            if (P.myPrefs.SwitchBearKey == P.PressBearFormKey.LSHIFT) comboBox1.SelectedIndex = 2;
            if (P.myPrefs.SwitchBearKey == P.PressBearFormKey.LCTRL) comboBox1.SelectedIndex = 3;
            if (P.myPrefs.SwitchBearKey == P.PressBearFormKey.RCTRL) comboBox1.SelectedIndex = 4;
            if (P.myPrefs.SwitchBearKey == P.PressBearFormKey.LALT) comboBox1.SelectedIndex = 5;
            if (P.myPrefs.SwitchBearKey == P.PressBearFormKey.RALT) comboBox1.SelectedIndex = 6;
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

            //resto
            if (P.myPrefs.Trinket1Resto == 1) radioButton18.Checked = true;
            if (P.myPrefs.Trinket1Resto == 2) radioButton17.Checked = true;
            if (P.myPrefs.Trinket1Resto == 3) radioButton16.Checked = true;
            if (P.myPrefs.Trinket1Resto == 4) radioButton15.Checked = true;
            if (P.myPrefs.Trinket1Resto == 5) radioButton14.Checked = true;
            if (P.myPrefs.Trinket1Resto == 6) radioButton13.Checked = true;
            if (P.myPrefs.Trinket2Resto == 1) radioButton24.Checked = true;
            if (P.myPrefs.Trinket2Resto == 2) radioButton23.Checked = true;
            if (P.myPrefs.Trinket2Resto == 3) radioButton22.Checked = true;
            if (P.myPrefs.Trinket2Resto == 4) radioButton21.Checked = true;
            if (P.myPrefs.Trinket2Resto == 5) radioButton20.Checked = true;
            if (P.myPrefs.Trinket2Resto == 6) radioButton19.Checked = true;
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
        private void cmbModifierResTanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            int keyselect = cmbModifierResTanks.SelectedIndex;
            switch (keyselect)
            {
                case 0: P.myPrefs.ModifkeyResTanks = "Alt"; break;
                case 1: P.myPrefs.ModifkeyResTanks = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyResTanks = "Shift"; break;
                case 3: P.myPrefs.ModifkeyResTanks = "Windows"; break;
            }
        }

        private void cmbModifierResHealers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int keyselect = cmbModifierResHealers.SelectedIndex;
            switch (keyselect)
            {
                case 0: P.myPrefs.ModifkeyResHealers = "Alt"; break;
                case 1: P.myPrefs.ModifkeyResHealers = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyResHealers = "Shift"; break;
                case 3: P.myPrefs.ModifkeyResHealers = "Windows"; break;
            }
        }

        private void cmbModifierResAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            int keyselect = cmbModifierResAll.SelectedIndex;
            switch (keyselect)
            {
                case 0: P.myPrefs.ModifkeyResAll = "Alt"; break;
                case 1: P.myPrefs.ModifkeyResAll = "Ctrl"; break;
                case 2: P.myPrefs.ModifkeyResAll = "Shift"; break;
                case 3: P.myPrefs.ModifkeyResAll = "Windows"; break;
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
        private void cmbHotkeyResTanks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHotkeyResTanks.SelectedIndex == 0) P.myPrefs.KeyResTanks = Keys.None;
            if (cmbHotkeyResTanks.SelectedIndex == 1) P.myPrefs.KeyResTanks = Keys.A;
            if (cmbHotkeyResTanks.SelectedIndex == 2) P.myPrefs.KeyResTanks = Keys.B;
            if (cmbHotkeyResTanks.SelectedIndex == 3) P.myPrefs.KeyResTanks = Keys.C;
            if (cmbHotkeyResTanks.SelectedIndex == 4) P.myPrefs.KeyResTanks = Keys.D;
            if (cmbHotkeyResTanks.SelectedIndex == 5) P.myPrefs.KeyResTanks = Keys.E;
            if (cmbHotkeyResTanks.SelectedIndex == 6) P.myPrefs.KeyResTanks = Keys.F;
            if (cmbHotkeyResTanks.SelectedIndex == 7) P.myPrefs.KeyResTanks = Keys.G;
            if (cmbHotkeyResTanks.SelectedIndex == 8) P.myPrefs.KeyResTanks = Keys.H;
            if (cmbHotkeyResTanks.SelectedIndex == 9) P.myPrefs.KeyResTanks = Keys.I;
            if (cmbHotkeyResTanks.SelectedIndex == 10) P.myPrefs.KeyResTanks = Keys.J;
            if (cmbHotkeyResTanks.SelectedIndex == 11) P.myPrefs.KeyResTanks = Keys.K;
            if (cmbHotkeyResTanks.SelectedIndex == 12) P.myPrefs.KeyResTanks = Keys.L;
            if (cmbHotkeyResTanks.SelectedIndex == 13) P.myPrefs.KeyResTanks = Keys.M;
            if (cmbHotkeyResTanks.SelectedIndex == 14) P.myPrefs.KeyResTanks = Keys.N;
            if (cmbHotkeyResTanks.SelectedIndex == 15) P.myPrefs.KeyResTanks = Keys.O;
            if (cmbHotkeyResTanks.SelectedIndex == 16) P.myPrefs.KeyResTanks = Keys.P;
            if (cmbHotkeyResTanks.SelectedIndex == 17) P.myPrefs.KeyResTanks = Keys.Q;
            if (cmbHotkeyResTanks.SelectedIndex == 18) P.myPrefs.KeyResTanks = Keys.R;
            if (cmbHotkeyResTanks.SelectedIndex == 19) P.myPrefs.KeyResTanks = Keys.S;
            if (cmbHotkeyResTanks.SelectedIndex == 20) P.myPrefs.KeyResTanks = Keys.T;
            if (cmbHotkeyResTanks.SelectedIndex == 21) P.myPrefs.KeyResTanks = Keys.U;
            if (cmbHotkeyResTanks.SelectedIndex == 22) P.myPrefs.KeyResTanks = Keys.V;
            if (cmbHotkeyResTanks.SelectedIndex == 23) P.myPrefs.KeyResTanks = Keys.W;
            if (cmbHotkeyResTanks.SelectedIndex == 24) P.myPrefs.KeyResTanks = Keys.X;
            if (cmbHotkeyResTanks.SelectedIndex == 25) P.myPrefs.KeyResTanks = Keys.Y;
            if (cmbHotkeyResTanks.SelectedIndex == 26) P.myPrefs.KeyResTanks = Keys.Z;
        }

        private void cmbHotkeyResHealers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHotkeyResHealers.SelectedIndex == 0) P.myPrefs.KeyReshealers = Keys.None;
            if (cmbHotkeyResHealers.SelectedIndex == 1) P.myPrefs.KeyReshealers = Keys.A;
            if (cmbHotkeyResHealers.SelectedIndex == 2) P.myPrefs.KeyReshealers = Keys.B;
            if (cmbHotkeyResHealers.SelectedIndex == 3) P.myPrefs.KeyReshealers = Keys.C;
            if (cmbHotkeyResHealers.SelectedIndex == 4) P.myPrefs.KeyReshealers = Keys.D;
            if (cmbHotkeyResHealers.SelectedIndex == 5) P.myPrefs.KeyReshealers = Keys.E;
            if (cmbHotkeyResHealers.SelectedIndex == 6) P.myPrefs.KeyReshealers = Keys.F;
            if (cmbHotkeyResHealers.SelectedIndex == 7) P.myPrefs.KeyReshealers = Keys.G;
            if (cmbHotkeyResHealers.SelectedIndex == 8) P.myPrefs.KeyReshealers = Keys.H;
            if (cmbHotkeyResHealers.SelectedIndex == 9) P.myPrefs.KeyReshealers = Keys.I;
            if (cmbHotkeyResHealers.SelectedIndex == 10) P.myPrefs.KeyReshealers = Keys.J;
            if (cmbHotkeyResHealers.SelectedIndex == 11) P.myPrefs.KeyReshealers = Keys.K;
            if (cmbHotkeyResHealers.SelectedIndex == 12) P.myPrefs.KeyReshealers = Keys.L;
            if (cmbHotkeyResHealers.SelectedIndex == 13) P.myPrefs.KeyReshealers = Keys.M;
            if (cmbHotkeyResHealers.SelectedIndex == 14) P.myPrefs.KeyReshealers = Keys.N;
            if (cmbHotkeyResHealers.SelectedIndex == 15) P.myPrefs.KeyReshealers = Keys.O;
            if (cmbHotkeyResHealers.SelectedIndex == 16) P.myPrefs.KeyReshealers = Keys.P;
            if (cmbHotkeyResHealers.SelectedIndex == 17) P.myPrefs.KeyReshealers = Keys.Q;
            if (cmbHotkeyResHealers.SelectedIndex == 18) P.myPrefs.KeyReshealers = Keys.R;
            if (cmbHotkeyResHealers.SelectedIndex == 19) P.myPrefs.KeyReshealers = Keys.S;
            if (cmbHotkeyResHealers.SelectedIndex == 20) P.myPrefs.KeyReshealers = Keys.T;
            if (cmbHotkeyResHealers.SelectedIndex == 21) P.myPrefs.KeyReshealers = Keys.U;
            if (cmbHotkeyResHealers.SelectedIndex == 22) P.myPrefs.KeyReshealers = Keys.V;
            if (cmbHotkeyResHealers.SelectedIndex == 23) P.myPrefs.KeyReshealers = Keys.W;
            if (cmbHotkeyResHealers.SelectedIndex == 24) P.myPrefs.KeyReshealers = Keys.X;
            if (cmbHotkeyResHealers.SelectedIndex == 25) P.myPrefs.KeyReshealers = Keys.Y;
            if (cmbHotkeyResHealers.SelectedIndex == 26) P.myPrefs.KeyReshealers = Keys.Z;
        }

        private void cmbHotkeyResAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbHotkeyResAll.SelectedIndex == 0) P.myPrefs.KeyResAll = Keys.None;
            if (cmbHotkeyResAll.SelectedIndex == 1) P.myPrefs.KeyResAll = Keys.A;
            if (cmbHotkeyResAll.SelectedIndex == 2) P.myPrefs.KeyResAll = Keys.B;
            if (cmbHotkeyResAll.SelectedIndex == 3) P.myPrefs.KeyResAll = Keys.C;
            if (cmbHotkeyResAll.SelectedIndex == 4) P.myPrefs.KeyResAll = Keys.D;
            if (cmbHotkeyResAll.SelectedIndex == 5) P.myPrefs.KeyResAll = Keys.E;
            if (cmbHotkeyResAll.SelectedIndex == 6) P.myPrefs.KeyResAll = Keys.F;
            if (cmbHotkeyResAll.SelectedIndex == 7) P.myPrefs.KeyResAll = Keys.G;
            if (cmbHotkeyResAll.SelectedIndex == 8) P.myPrefs.KeyResAll = Keys.H;
            if (cmbHotkeyResAll.SelectedIndex == 9) P.myPrefs.KeyResAll = Keys.I;
            if (cmbHotkeyResAll.SelectedIndex == 10) P.myPrefs.KeyResAll = Keys.J;
            if (cmbHotkeyResAll.SelectedIndex == 11) P.myPrefs.KeyResAll = Keys.K;
            if (cmbHotkeyResAll.SelectedIndex == 12) P.myPrefs.KeyResAll = Keys.L;
            if (cmbHotkeyResAll.SelectedIndex == 13) P.myPrefs.KeyResAll = Keys.M;
            if (cmbHotkeyResAll.SelectedIndex == 14) P.myPrefs.KeyResAll = Keys.N;
            if (cmbHotkeyResAll.SelectedIndex == 15) P.myPrefs.KeyResAll = Keys.O;
            if (cmbHotkeyResAll.SelectedIndex == 16) P.myPrefs.KeyResAll = Keys.P;
            if (cmbHotkeyResAll.SelectedIndex == 17) P.myPrefs.KeyResAll = Keys.Q;
            if (cmbHotkeyResAll.SelectedIndex == 18) P.myPrefs.KeyResAll = Keys.R;
            if (cmbHotkeyResAll.SelectedIndex == 19) P.myPrefs.KeyResAll = Keys.S;
            if (cmbHotkeyResAll.SelectedIndex == 20) P.myPrefs.KeyResAll = Keys.T;
            if (cmbHotkeyResAll.SelectedIndex == 21) P.myPrefs.KeyResAll = Keys.U;
            if (cmbHotkeyResAll.SelectedIndex == 22) P.myPrefs.KeyResAll = Keys.V;
            if (cmbHotkeyResAll.SelectedIndex == 23) P.myPrefs.KeyResAll = Keys.W;
            if (cmbHotkeyResAll.SelectedIndex == 24) P.myPrefs.KeyResAll = Keys.X;
            if (cmbHotkeyResAll.SelectedIndex == 25) P.myPrefs.KeyResAll = Keys.Y;
            if (cmbHotkeyResAll.SelectedIndex == 26) P.myPrefs.KeyResAll = Keys.Z;
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

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.PullWildCharge = checkBox16.Checked;
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Rejuvenation5 = (int)numericUpDown14.Value;
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Regrowth5 = (int)numericUpDown15.Value;
        }

        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.HealingTouch5 = (int)numericUpDown16.Value;
        }

        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.WildGrowth5 = (int)numericUpDown17.Value;
        }

        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.WildGrowthPlayers5 = (int)numericUpDown18.Value;
        }

        private void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Tranquility5 = (int)numericUpDown19.Value;
        }

        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.TranquilityPlayers5 = (int)numericUpDown20.Value;
        }

        private void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Genesis5 = (int)numericUpDown21.Value;
        }

        private void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.GenesisPlayers5 = (int)numericUpDown22.Value;
        }

        private void numericUpDown41_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.ForceOfNature5 = (int)numericUpDown41.Value;
        }

        private void numericUpDown44_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Swiftmend5 = (int)numericUpDown44.Value;
        }

        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Rejuvenation510 = (int)numericUpDown31.Value;
        }

        private void numericUpDown30_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Regrowth510 = (int)numericUpDown30.Value;
        }

        private void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.HealingTouch510 = (int)numericUpDown29.Value;
        }

        private void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.WildGrowth510 = (int)numericUpDown28.Value;
        }

        private void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.WildGrowthPlayers510 = (int)numericUpDown27.Value;
        }

        private void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Tranquility510 = (int)numericUpDown26.Value;
        }

        private void numericUpDown25_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.TranquilityPlayers510 = (int)numericUpDown25.Value;
        }

        private void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Genesis510 = (int)numericUpDown24.Value;
        }

        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.GenesisPlayers510 = (int)numericUpDown23.Value;
        }

        private void numericUpDown42_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.ForceOfNature510 = (int)numericUpDown42.Value;
        }

        private void numericUpDown45_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Swiftmend510 = (int)numericUpDown45.Value;
        }

        private void numericUpDown40_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Rejuvenation50 = (int)numericUpDown40.Value;
        }

        private void numericUpDown39_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Regrowth50 = (int)numericUpDown39.Value;
        }

        private void numericUpDown38_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.HealingTouch50 = (int)numericUpDown38.Value;
        }

        private void numericUpDown37_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.WildGrowth50 = (int)numericUpDown37.Value;
        }

        private void numericUpDown36_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.WildGrowthPlayers50 = (int)numericUpDown36.Value;
        }

        private void numericUpDown35_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Tranquility50 = (int)numericUpDown35.Value;
        }

        private void numericUpDown34_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.TranquilityPlayers50 = (int)numericUpDown34.Value;
        }

        private void numericUpDown33_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Genesis50 = (int)numericUpDown33.Value;
        }

        private void numericUpDown32_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.GenesisPlayers50 = (int)numericUpDown32.Value;
        }

        private void numericUpDown43_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.ForceOfNature50 = (int)numericUpDown43.Value;
        }

        private void numericUpDown46_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.Swiftmend50 = (int)numericUpDown46.Value;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoDispel = checkBox17.Checked;
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton18.Checked) P.myPrefs.Trinket1Resto = 1;
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton17.Checked) P.myPrefs.Trinket1Resto = 2;
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton16.Checked) P.myPrefs.Trinket1Resto = 3;
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton15.Checked) P.myPrefs.Trinket1Resto = 4;
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton14.Checked) P.myPrefs.Trinket1Resto = 5;
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton13.Checked) P.myPrefs.Trinket1Resto = 6;
        }

        private void numericUpDown48_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket1EnergyResto = (int)numericUpDown48.Value;
        }

        private void numericUpDown47_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket1ManaResto = (int)numericUpDown47.Value;
        }

        private void numericUpDown49_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket1HPResto = (int)numericUpDown49.Value;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.Trinket1UseResto = checkBox18.Checked;
        }

        private void radioButton24_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton24.Checked) P.myPrefs.Trinket2Resto = 1;
        }

        private void radioButton23_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton23.Checked) P.myPrefs.Trinket2Resto = 2;
        }

        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton22.Checked) P.myPrefs.Trinket2Resto = 3;
        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton21.Checked) P.myPrefs.Trinket2Resto = 4;
        }

        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton20.Checked) P.myPrefs.Trinket2Resto = 5;
        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton19.Checked) P.myPrefs.Trinket2Resto = 6;
        }

        private void numericUpDown51_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket2EnergyResto = (int)numericUpDown51.Value;
        }

        private void numericUpDown50_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket2ManaResto = (int)numericUpDown50.Value;
        }

        private void numericUpDown52_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket2HPResto = (int)numericUpDown52.Value;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.Trinket2UseResto = checkBox19.Checked;
        }

        private void numericUpDown53_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentHealingTouchCombat = (int)numericUpDown53.Value;
        }

        private void numericUpDown54_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.AoeCat = (int)numericUpDown54.Value;
        }

        private void numericUpDown55_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.AoeMoonkin = (int)numericUpDown55.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) P.myPrefs.SwitchBearKey = P.PressBearFormKey.None;
            if (comboBox1.SelectedIndex == 1) P.myPrefs.SwitchBearKey = P.PressBearFormKey.RSHIFT;
            if (comboBox1.SelectedIndex == 2) P.myPrefs.SwitchBearKey = P.PressBearFormKey.LSHIFT;
            if (comboBox1.SelectedIndex == 3) P.myPrefs.SwitchBearKey = P.PressBearFormKey.LCTRL;
            if (comboBox1.SelectedIndex == 4) P.myPrefs.SwitchBearKey = P.PressBearFormKey.RCTRL;
            if (comboBox1.SelectedIndex == 5) P.myPrefs.SwitchBearKey = P.PressBearFormKey.LALT;
            if (comboBox1.SelectedIndex == 6) P.myPrefs.SwitchBearKey = P.PressBearFormKey.RALT;

        }

        

        

    }
}
