using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
using PR = Druid.DSettings.RestoSettings;
using M = Druid.Helpers.Movement;
using I = Druid.Helpers.Interrupts;
using Styx.Common;
using System.IO;
#endregion

namespace Druid.DGUI
{
    public partial class Form1 : Form
    {
        private static int pauseSelect;
        private static int stopAoeSelect;
        private static int manualSelect;
        private static int cooldownSelect;
        private static int bearformSelect;
        private static string comboSelectPause;
        private static string comboSelectAoe;
        private static string comboSelectManual;
        private static string comboSelectCooldown;
        private static string comboselectBearform;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HKM.keysRegistered = false;
            HKM.registerHotKeys();
            P.myPrefs.Save();
            PR.myPrefs.Save();
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Utilities.AssemblyDirectory +
                            @"\Routines\Druid\DImages\Document.rtf"))
            {
                richTextBox1.LoadFile(Utilities.AssemblyDirectory +
                                            @"\Routines\Druid\DImages\Document.rtf");
            }
            if (File.Exists(Utilities.AssemblyDirectory +
                            @"\Routines\Druid\DImages\Document2.rtf"))
            {
                richTextBox2.LoadFile(Utilities.AssemblyDirectory +
                                            @"\Routines\Druid\DImages\Document2.rtf");
            }
            propertyGrid1.SelectedObject = PR.myPrefs;

            #region movement
            checkBox1.Checked = P.myPrefs.AutoMovement;
            checkBox2.Checked = P.myPrefs.AutoTargeting;
            checkBox3.Checked = P.myPrefs.AutoFacing;
            checkBox4.Checked = P.myPrefs.AutoMovementDisable;
            checkBox5.Checked = P.myPrefs.AutoTargetingDisable;
            checkBox6.Checked = P.myPrefs.AutoFacingDisable;
            #endregion

            #region trinket1
            if (P.myPrefs.Trinket1 == 1) { btnTrinket1Manual.Checked = true; }
            if (P.myPrefs.Trinket1 == 2) { btnTrinket1CD.Checked = true; }
            if (P.myPrefs.Trinket1 == 3) { btnTrinket1Boss.Checked = true; }
            if (P.myPrefs.Trinket1 == 4) { btnTrinket1HP.Checked = true; }
            if (P.myPrefs.Trinket1 == 5) { btnTrinket1Mana.Checked = true; }
            nupTrinket1HP.Value = new decimal(P.myPrefs.PercentTrinket1HP);
            nupTrinket1Mana.Value = new decimal(P.myPrefs.PercentTrinket1Mana);
            #endregion

            #region trinket2
            if (P.myPrefs.Trinket2 == 1) { btnTrinket2Manual.Checked = true; }
            if (P.myPrefs.Trinket2 == 2) { btnTrinket2CD.Checked = true; }
            if (P.myPrefs.Trinket2 == 3) { btnTrinket2Boss.Checked = true; }
            if (P.myPrefs.Trinket2 == 4) { btnTrinket2HP.Checked = true; }
            if (P.myPrefs.Trinket2 == 5) { btnTrinket2Mana.Checked = true; }
            nupTrinket2HP.Value = new decimal(P.myPrefs.PercentTrinket2HP);
            nupTrinket2Mana.Value = new decimal(P.myPrefs.PercentTrinket2Mana);
            #endregion

            #region gloves
            if (P.myPrefs.Gloves == 1) { btnGlovesManual.Checked = true; }
            if (P.myPrefs.Gloves == 2) { btnGlovesCD.Checked = true; }
            if (P.myPrefs.Gloves == 3) { btnGlovesBoss.Checked = true; }
            #endregion

            #region berserk
            if (P.myPrefs.CDBerserk == 1) { btnBerserkManual.Checked = true; }
            if (P.myPrefs.CDBerserk == 2) { btnBerserkCD.Checked = true; }
            if (P.myPrefs.CDBerserk == 3) { btnBerserkBoss.Checked = true; }
            #endregion

            #region Berserking
            if (P.myPrefs.CDBerserking == 1) { btnBerserkingManual.Checked = true; }
            if (P.myPrefs.CDBerserking == 2) { btnBerserkingCD.Checked = true; }
            if (P.myPrefs.CDBerserking == 3) { btnBerserkingBoss.Checked = true; }
            #endregion

            #region incarnation
            if (P.myPrefs.CDIncarnation == 1) { btnIncarnationManual.Checked = true; }
            if (P.myPrefs.CDIncarnation == 2) { btnIncarnationCD.Checked = true; }
            if (P.myPrefs.CDIncarnation == 3) { btnIncarnationBoss.Checked = true; }
            #endregion incarnation

            #region heart of the wild
            if (P.myPrefs.CDHeartOfTheWild == 1) { btnHoftWManual.Checked = true; }
            if (P.myPrefs.CDHeartOfTheWild == 2) { btnHoftWCD.Checked = true; }
            if (P.myPrefs.CDHeartOfTheWild == 3) { btnHoftWBoss.Checked = true; }
            #endregion

            #region misc
            checkBox7.Checked = P.myPrefs.AutoInterrupt;
            checkBox8.Checked = P.myPrefs.AutoShape;
            checkBox9.Checked = P.myPrefs.PredatoryHealOthers;
            numericUpDown1.Value = new decimal(P.myPrefs.PercentPredatoryHealOthers);
            checkBox11.Checked = P.myPrefs.FlaskAlchemy;
            checkBox12.Checked = P.myPrefs.FlaskCrystal;
            #endregion

            #region protection
            nupBarkskin.Value = new decimal(P.myPrefs.PercentBarkskin);
            numSurvIntstincts.Value = new decimal(P.myPrefs.PercentSurvivalInstincts);
            nupFrenzied.Value = new decimal(P.myPrefs.PercentFrenziedRegeneration);
            nupHealthstone.Value = new decimal(P.myPrefs.PercentHealthstone);
            numericUpDown2.Value = new decimal(P.myPrefs.PercentRejuCombat);
            numericUpDown7.Value = new decimal(P.myPrefs.PercentSavageDefense);
            numericUpDown8.Value = new decimal(P.myPrefs.PercentSwitchBearForm);
            numericUpDown9.Value = new decimal(P.myPrefs.PercentCenarionWard);
            #endregion

            #region hotkey pause
            checkBox10.Checked = P.myPrefs.PrintRaidstyleMsg;

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
            #endregion

            #region hotkey stopaoe
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
            #endregion

            #region hotkey playmanual
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
            #endregion

            #region hotkey use cooldowns
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

            #region hotkey switch bearform
            comboselectBearform = P.myPrefs.KeySwitchBearform.ToString();
            switch (comboselectBearform)
            {
                case "None":
                    cmbSwitchBear.SelectedIndex = 0;
                    break;
                case "A":
                    cmbSwitchBear.SelectedIndex = 1;
                    break;
                case "B":
                    cmbSwitchBear.SelectedIndex = 2;
                    break;
                case "C":
                    cmbSwitchBear.SelectedIndex = 3;
                    break;
                case "D":
                    cmbSwitchBear.SelectedIndex = 4;
                    break;
                case "E":
                    cmbSwitchBear.SelectedIndex = 5;
                    break;
                case "F":
                    cmbSwitchBear.SelectedIndex = 6;
                    break;
                case "G":
                    cmbSwitchBear.SelectedIndex = 7;
                    break;
                case "H":
                    cmbSwitchBear.SelectedIndex = 8;
                    break;
                case "I":
                    cmbSwitchBear.SelectedIndex = 9;
                    break;
                case "J":
                    cmbSwitchBear.SelectedIndex = 10;
                    break;
                case "K":
                    cmbSwitchBear.SelectedIndex = 11;
                    break;
                case "L":
                    cmbSwitchBear.SelectedIndex = 12;
                    break;
                case "M":
                    cmbSwitchBear.SelectedIndex = 13;
                    break;
                case "N":
                    cmbSwitchBear.SelectedIndex = 14;
                    break;
                case "O":
                    cmbSwitchBear.SelectedIndex = 15;
                    break;
                case "P":
                    cmbSwitchBear.SelectedIndex = 16;
                    break;
                case "Q":
                    cmbSwitchBear.SelectedIndex = 17;
                    break;
                case "R":
                    cmbSwitchBear.SelectedIndex = 18;
                    break;
                case "S":
                    cmbSwitchBear.SelectedIndex = 19;
                    break;
                case "T":
                    cmbSwitchBear.SelectedIndex = 20;
                    break;
                case "U":
                    cmbSwitchBear.SelectedIndex = 21;
                    break;
                case "V":
                    cmbSwitchBear.SelectedIndex = 22;
                    break;
                case "W":
                    cmbSwitchBear.SelectedIndex = 23;
                    break;
                case "X":
                    cmbSwitchBear.SelectedIndex = 24;
                    break;
                case "Y":
                    cmbSwitchBear.SelectedIndex = 25;
                    break;
                case "Z":
                    cmbSwitchBear.SelectedIndex = 26;
                    break;
            }
            #endregion

            #region Out of Combat
            numericUpDown3.Value = new decimal(P.myPrefs.PercentRejuOoC);
            numericUpDown4.Value = new decimal(P.myPrefs.PercentHealingTouchOoC);
            numericUpDown5.Value = new decimal(P.myPrefs.FoodHPOoC);
            numericUpDown6.Value = new decimal(P.myPrefs.FoodManaOoC);
            #endregion

            #region flasks
            if (P.myPrefs.RaidFlaskKind == 76084) { btnAgilityFlask.Checked = true; }
            if (P.myPrefs.RaidFlaskKind == 76087) { btnStaminaFlask.Checked = true; }
            if (P.myPrefs.RaidFlaskKind == 76085) { btnIntellectFlask.Checked = true; }
            if (P.myPrefs.RaidFlask == 1) { btnRaidFlaskManual.Checked = true; }
            if (P.myPrefs.RaidFlask == 2) { btnRaidFlaskRaids.Checked = true; }
            if (P.myPrefs.RaidFlask == 3) { btnRaidFlaskNotLFR.Checked = true; }
            if (P.myPrefs.RaidFlask == 4) { btnRaidFlaskInstances.Checked = true; }
            #endregion

            #region pull
            
            #endregion

            #region res people
            if (P.myPrefs.ResPeople == 1) { btnResManual.Checked = true; }
            if (P.myPrefs.ResPeople == 2) { btnResTank.Checked = true; }
            if (P.myPrefs.ResPeople == 3) { btnResTankHeal.Checked = true; }
            if (P.myPrefs.ResPeople == 4) { btnResFocus.Checked = true; }
            if (P.myPrefs.ResPeople == 5) { btnResAll.Checked = true; }
            #endregion

            checkBox13.Checked = P.myPrefs.AutoSVN;
            checkBox14.Checked = P.myPrefs.GoLowbieCat;
            checkBox15.Checked = P.myPrefs.PullPref;
            numericUpDown10.Value = new decimal(P.myPrefs.ShredEnergy);

        }

        #region movement
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
        #endregion

        #region trinket1
        private void btnTrinket1Manual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket1Manual.Checked) { P.myPrefs.Trinket1 = 1; }
        }

        private void btnTrinket1CD_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket1CD.Checked) { P.myPrefs.Trinket1 = 2; }
        }

        private void btnTrinket1Boss_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket1Boss.Checked) { P.myPrefs.Trinket1 = 3; }
        }

        private void btnTrinket1HP_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket1HP.Checked) { P.myPrefs.Trinket1 = 4; }
        }

        private void btnTrinket1Mana_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket1Mana.Checked) { P.myPrefs.Trinket1 = 5; }
        }
        private void nupTrinket1HP_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket1HP = (int)nupTrinket1HP.Value;
        }

        private void nupTrinket1Mana_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket1Mana = (int)nupTrinket1Mana.Value;
        }
        #endregion

        #region trinket 2
        private void btnTrinket2Manual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket2Manual.Checked) { P.myPrefs.Trinket2 = 1; }
        }

        private void btnTrinket2CD_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket2CD.Checked) { P.myPrefs.Trinket2 = 2; }
        }

        private void btnTrinket2Boss_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket2Boss.Checked) { P.myPrefs.Trinket2 = 3; }
        }

        private void btnTrinket2HP_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket2HP.Checked) { P.myPrefs.Trinket2 = 4; }
        }

        private void btnTrinket2Mana_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTrinket2Mana.Checked) { P.myPrefs.Trinket2 = 5; }
        }
        private void nupTrinket2HP_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket2HP = (int)nupTrinket2HP.Value;
        }

        private void nupTrinket2Mana_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentTrinket2Mana = (int)nupTrinket2Mana.Value;
        }
        #endregion

        #region gloves
        private void btnGlovesManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnGlovesManual.Checked) { P.myPrefs.Gloves = 1; }
        }

        private void btnGlovesCD_CheckedChanged(object sender, EventArgs e)
        {
            if (btnGlovesCD.Checked) { P.myPrefs.Gloves = 2; }
        }

        private void btnGlovesBoss_CheckedChanged(object sender, EventArgs e)
        {
            if (btnGlovesBoss.Checked) { P.myPrefs.Gloves = 3; }
        }
        #endregion

        #region berserk
        private void btnBerserkManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnBerserkManual.Checked) { P.myPrefs.CDBerserk = 1; }
        }

        private void btnBerserkCD_CheckedChanged(object sender, EventArgs e)
        {
            if (btnBerserkCD.Checked) { P.myPrefs.CDBerserk = 2; }
        }

        private void btnBerserkBoss_CheckedChanged(object sender, EventArgs e)
        {
            if (btnBerserkBoss.Checked) { P.myPrefs.CDBerserk = 3; }
        }
        #endregion

        #region incarnation
        private void btnIncarnationManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnIncarnationManual.Checked) { P.myPrefs.CDIncarnation = 1; }
        }

        private void btnIncarnationCD_CheckedChanged(object sender, EventArgs e)
        {
            if (btnIncarnationCD.Checked) { P.myPrefs.CDIncarnation = 2; }
        }

        private void btnIncarnationBoss_CheckedChanged(object sender, EventArgs e)
        {
            if (btnIncarnationBoss.Checked) { P.myPrefs.CDIncarnation = 3; }
        }
        #endregion

        #region misc
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoInterrupt = checkBox7.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoShape = checkBox8.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.PredatoryHealOthers = checkBox9.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentPredatoryHealOthers = (int)numericUpDown1.Value;
        }
        #endregion

        #region protection
        private void nupBarkskin_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentBarkskin = (int)nupBarkskin.Value;
        }

        private void numSurvIntstincts_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentSurvivalInstincts = (int)numSurvIntstincts.Value;
        }

        private void nupFrenzied_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentFrenziedRegeneration = (int)nupFrenzied.Value;
        }

        private void nupHealthstone_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentHealthstone = (int)nupHealthstone.Value;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentRejuCombat = (int)numericUpDown2.Value;
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentSavageDefense = (int)numericUpDown7.Value; 
        }
        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentSwitchBearForm = (int)numericUpDown8.Value;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentCenarionWard = (int)numericUpDown9.Value;
        }
        #endregion

        #region hotkey pause
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.PrintRaidstyleMsg = checkBox10.Checked;
        }

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
        #endregion

        #region hotkey stopaoe
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
        #endregion

        #region hotkey play manual
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
        #endregion

        #region hotkey cooldowns
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

        #region hotkey switch bearform
        private void cmbSwitchBear_SelectedIndexChanged(object sender, EventArgs e)
        {
            bearformSelect = cmbSwitchBear.SelectedIndex;
            switch (bearformSelect)
            {
                case 0:
                    P.myPrefs.KeySwitchBearform = Keys.None;
                    break;
                case 1:
                    P.myPrefs.KeySwitchBearform = Keys.A;
                    break;
                case 2:
                    P.myPrefs.KeySwitchBearform = Keys.B;
                    break;
                case 3:
                    P.myPrefs.KeySwitchBearform = Keys.C;
                    break;
                case 4:
                    P.myPrefs.KeySwitchBearform = Keys.D;
                    break;
                case 5:
                    P.myPrefs.KeySwitchBearform = Keys.E;
                    break;
                case 6:
                    P.myPrefs.KeySwitchBearform = Keys.F;
                    break;
                case 7:
                    P.myPrefs.KeySwitchBearform = Keys.G;
                    break;
                case 8:
                    P.myPrefs.KeySwitchBearform = Keys.H;
                    break;
                case 9:
                    P.myPrefs.KeySwitchBearform = Keys.I;
                    break;
                case 10:
                    P.myPrefs.KeySwitchBearform = Keys.J;
                    break;
                case 11:
                    P.myPrefs.KeySwitchBearform = Keys.K;
                    break;
                case 12:
                    P.myPrefs.KeySwitchBearform = Keys.L;
                    break;
                case 13:
                    P.myPrefs.KeySwitchBearform = Keys.M;
                    break;
                case 14:
                    P.myPrefs.KeySwitchBearform = Keys.N;
                    break;
                case 15:
                    P.myPrefs.KeySwitchBearform = Keys.O;
                    break;
                case 16:
                    P.myPrefs.KeySwitchBearform = Keys.P;
                    break;
                case 17:
                    P.myPrefs.KeySwitchBearform = Keys.Q;
                    break;
                case 18:
                    P.myPrefs.KeySwitchBearform = Keys.R;
                    break;
                case 19:
                    P.myPrefs.KeySwitchBearform = Keys.S;
                    break;
                case 20:
                    P.myPrefs.KeySwitchBearform = Keys.T;
                    break;
                case 21:
                    P.myPrefs.KeySwitchBearform = Keys.U;
                    break;
                case 22:
                    P.myPrefs.KeySwitchBearform = Keys.V;
                    break;
                case 23:
                    P.myPrefs.KeySwitchBearform = Keys.W;
                    break;
                case 24:
                    P.myPrefs.KeySwitchBearform = Keys.X;
                    break;
                case 25:
                    P.myPrefs.KeySwitchBearform = Keys.Y;
                    break;
                case 26:
                    P.myPrefs.KeySwitchBearform = Keys.Z;
                    break;
            }
        }
        #endregion

        #region Out of Combat
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentRejuOoC = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.PercentHealingTouchOoC = (int)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.FoodHPOoC = (int)numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.FoodManaOoC = (int)numericUpDown6.Value;
        }
        #endregion

        #region Berserking
        private void btnBerserkingManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnBerserkingManual.Checked) { P.myPrefs.CDBerserking = 1; }
        }

        private void btnBerserkingCD_CheckedChanged(object sender, EventArgs e)
        {
            if (btnBerserkingCD.Checked) { P.myPrefs.CDBerserking = 2; }
        }

        private void btnBerserkingBoss_CheckedChanged(object sender, EventArgs e)
        {
            if (btnBerserkingBoss.Checked) { P.myPrefs.CDBerserking = 3; }
        }
        #endregion

        #region heart of the wild
        private void btnHoftWManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnHoftWManual.Checked) { P.myPrefs.CDHeartOfTheWild = 1; }
        }

        private void btnHoftWCD_CheckedChanged(object sender, EventArgs e)
        {
            if (btnHoftWCD.Checked) { P.myPrefs.CDHeartOfTheWild = 2; }
        }

        private void btnHoftWBoss_CheckedChanged(object sender, EventArgs e)
        {
            if (btnHoftWBoss.Checked) { P.myPrefs.CDHeartOfTheWild = 3; }
        }
        #endregion

        #region flasks
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.FlaskAlchemy = checkBox11.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.FlaskCrystal = checkBox12.Checked;
        }

        private void btnAgilityFlask_CheckedChanged(object sender, EventArgs e)
        {
            if (btnAgilityFlask.Checked) { P.myPrefs.RaidFlaskKind = 76084; }
        }

        private void btnIntellectFlask_CheckedChanged(object sender, EventArgs e)
        {
            if (btnIntellectFlask.Checked) { P.myPrefs.RaidFlaskKind = 76085; }
        }

        private void btnStaminaFlask_CheckedChanged(object sender, EventArgs e)
        {
            if (btnStaminaFlask.Checked) { P.myPrefs.RaidFlaskKind = 76087; }
        }

        private void btnRaidFlaskManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRaidFlaskManual.Checked) { P.myPrefs.RaidFlask = 1; }
        }

        private void btnRaidFlaskRaids_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRaidFlaskRaids.Checked) { P.myPrefs.RaidFlask = 2; }
        }

        private void btnRaidFlaskNotLFR_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRaidFlaskNotLFR.Checked) { P.myPrefs.RaidFlask = 3; }
        }

        private void btnRaidFlaskInstances_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRaidFlaskInstances.Checked) { P.myPrefs.RaidFlask = 4; }
        }
        #endregion

        #region res people
        private void btnResManual_CheckedChanged(object sender, EventArgs e)
        {
            if (btnResManual.Checked) { P.myPrefs.ResPeople = 1; }
        }

        private void btnResTank_CheckedChanged(object sender, EventArgs e)
        {
            if (btnResTank.Checked) { P.myPrefs.ResPeople = 2; }
        }

        private void btnResTankHeal_CheckedChanged(object sender, EventArgs e)
        {
            if (btnResTankHeal.Checked) { P.myPrefs.ResPeople = 3; }
        }

        private void btnResFocus_CheckedChanged(object sender, EventArgs e)
        {
            if (btnResFocus.Checked) { P.myPrefs.ResPeople = 4; }
        }

        private void btnResAll_CheckedChanged(object sender, EventArgs e)
        {
            if (btnResAll.Checked) { P.myPrefs.ResPeople = 5; }
        }
        #endregion

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.AutoSVN = checkBox13.Checked;
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            P.myPrefs.ShredEnergy = (int)numericUpDown10.Value;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.GoLowbieCat = checkBox14.Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            P.myPrefs.PullPref = checkBox15.Checked;
        }

    }
}
