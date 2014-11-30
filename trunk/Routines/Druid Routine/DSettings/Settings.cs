using System.Text;
using Styx.Helpers;
using System.IO;
using Styx;
using Styx.Common;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
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
using M = Druid.Helpers.Movement;
using I = Druid.Helpers.Interrupts;
#endregion

namespace Druid.DSettings
{
    class DruidPrefs : Styx.Helpers.Settings
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static readonly DruidPrefs myPrefs = new DruidPrefs();
        
        public DruidPrefs() 
            :base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Routines/Settings/Druid/{0}-DruidSettings-{1}.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name)))
        {
        }

        [Setting, DefaultValue(true)]
        public bool AutoSVN { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoDispel { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoMovement { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoTargeting { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoFacing { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoMovementDisable { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoTargetingDisable { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoFacingDisable { get; set; }

        public enum KeyPress
        {
            None,
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z
        }

        [Setting, DefaultValue(KeyPress.None)]
        public Keys KeyStopAoe { get; set; }

        [Setting, DefaultValue(KeyPress.None)]
        public Keys KeyUseCooldowns { get; set; }

        [Setting, DefaultValue(KeyPress.None)]
        public Keys KeyPauseCR { get; set; }

        [Setting, DefaultValue(KeyPress.None)]
        public Keys KeyPlayManual { get; set; }

        [Setting, DefaultValue(KeyPress.None)]
        public Keys KeySwitchBearform { get; set; }

        [Setting, DefaultValue(true)]
        public bool PrintRaidstyleMsg { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoInterrupt { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoShape { get; set; }

        [Setting, DefaultValue(50)]
        public int FoodHPOoC { get; set; }

        [Setting, DefaultValue(50)]
        public int FoodManaOoC { get; set; }

        [Setting, DefaultValue(1)]
        public int Trinket1 { get; set; }

        [Setting, DefaultValue(1)]
        public int Trinket2 { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket1HP { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket1Mana { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket2HP { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket2Mana { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentCenarionWard { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentSwitchBearForm { get; set; }

        [Setting, DefaultValue(1)]
        public int Gloves { get; set; }

        [Setting, DefaultValue(3)]
        public int Racial { get; set; }

        [Setting, DefaultValue(1)]
        public int RaidFlask { get; set; }

        [Setting, DefaultValue(76084)]
        public int RaidFlaskKind { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentRejuCombat { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentRejuOoC { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentHealingTouchOoC { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentHealthstone { get; set; }

        [Setting, DefaultValue(50)]
        public int PercentNaaru { get; set; }

        [Setting, DefaultValue(60)]
        public int PercentSurvivalInstincts { get; set; }

        [Setting, DefaultValue(70)]
        public int PercentBarkskin { get; set; }

        [Setting, DefaultValue(50)]
        public int PercentDavageDefense { get; set; }

        [Setting, DefaultValue(85)]
        public int PercentFrenziedRegeneration { get; set; }

        [Setting, DefaultValue(false)]
        public bool PredatoryHealOthers { get; set; }

        [Setting, DefaultValue(85)]
        public int PercentPredatoryHealOthers { get; set; }

        [Setting, DefaultValue(90)]
        public int PercentSavageDefense { get; set; }

        [Setting, DefaultValue(false)]
        public bool FlaskCrystal { get; set; }

        [Setting, DefaultValue(false)]
        public bool FlaskAlchemy { get; set; }

        [Setting, DefaultValue(false)]
        public bool GoLowbieCat { get; set; }

        [Setting, DefaultValue(1)]
        public int CDBerserk { get; set; }

        [Setting, DefaultValue(1)]
        public int CDIncarnation { get; set; }

        [Setting, DefaultValue(1)]
        public int CDHeartOfTheWild { get; set; }

        [Setting, DefaultValue(1)]
        public int CDBerserking { get; set; }

        [Setting, DefaultValue(false)]
        public bool PullPref { get; set; }

        [Setting, DefaultValue(1)]
        public int ResPeople { get; set; }

        [Setting, DefaultValue(60)]
        public int ShredEnergy { get; set; }


    }
}
