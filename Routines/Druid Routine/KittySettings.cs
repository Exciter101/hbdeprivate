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

namespace Kitty
{
    class KittySettings : Settings
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static readonly KittySettings myPrefs = new KittySettings();
        
        public KittySettings() 
            :base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Routines/Settings/Druid/{0}-KittySettings-{1}.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name)))
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

        public enum KeyModifier
        {
            Alt,
            Control,
            Shift,
            Windows
        }

        [Setting, DefaultValue(KeyModifier.Alt)]
        public Keys Modkey { get; set; }

        [Setting, DefaultValue("Alt")]
        public string ModifkeyPause { get; set; }

        [Setting, DefaultValue("Alt")]
        public string ModifkeyCooldowns { get; set; }

        [Setting, DefaultValue("Alt")]
        public string ModifkeyStopAoe { get; set; }

        [Setting, DefaultValue("Alt")]
        public string ModifkeyPlayManual { get; set; }

        [Setting, DefaultValue("Shift")]
        public string ModifkeyResTanks { get; set; }

        [Setting, DefaultValue("Shift")]
        public string ModifkeyResHealers { get; set; }

        [Setting, DefaultValue("Shift")]
        public string ModifkeyResAll { get; set; }

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

        [Setting, DefaultValue(KeyPress.None)]
        public Keys KeyResTanks { get; set; }

        [Setting, DefaultValue(KeyPress.None)]
        public Keys KeyReshealers { get; set; }

        [Setting, DefaultValue(KeyPress.None)]
        public Keys KeyResAll { get; set; }

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

        [Setting, DefaultValue(false)]
        public bool Trinket1Use { get; set; }

        [Setting, DefaultValue(1)]
        public int Trinket2 { get; set; }

        [Setting, DefaultValue(false)]
        public bool Trinket2Use { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket1HP { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket1Mana { get; set; }

        [Setting, DefaultValue(25)]
        public int PercentTrinket1Energy { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket2HP { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket2Mana { get; set; }

        [Setting, DefaultValue(25)]
        public int PercentTrinket2Energy { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentCenarionWard { get; set; }

        [Setting, DefaultValue(35)]
        public int PercentSwitchBearForm { get; set; }

        [Setting, DefaultValue(3)]
        public int Racial { get; set; }

        [Setting, DefaultValue(1)]
        public int RaidFlask { get; set; }

        [Setting, DefaultValue(76084)]
        public int RaidFlaskKind { get; set; }

        [Setting, DefaultValue(95)]
        public int PercentRejuCombat { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentRejuOoC { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentHealingTouchOoC { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentHealingTouchCombat { get; set; }

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

        [Setting, DefaultValue(40)]
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
        public bool FlaskOraliusCrystal { get; set; }

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

        [Setting, DefaultValue(false)]
        public bool PullProwlAndRake { get; set; }

        [Setting, DefaultValue(false)]
        public bool PullProwlAndShred { get; set; }

        [Setting, DefaultValue(false)]
        public bool PullWildCharge { get; set; }

        //restoration 2-5
        [Setting, DefaultValue(90)]
        public int Rejuvenation5 { get; set; }

        [Setting, DefaultValue(80)]
        public int Regrowth5 { get; set; }

        [Setting, DefaultValue(50)]
        public int HealingTouch5 { get; set; }

        [Setting, DefaultValue(65)]
        public int ForceOfNature5 { get; set; }

        [Setting, DefaultValue(85)]
        public int WildGrowth5 { get; set; }

        [Setting, DefaultValue(45)]
        public int Tranquility5 { get; set; }

        [Setting, DefaultValue(85)]
        public int Genesis5 { get; set; }

        [Setting, DefaultValue(2)]
        public int WildGrowthPlayers5 { get; set; }

        [Setting, DefaultValue(3)]
        public int TranquilityPlayers5 { get; set; }

        [Setting, DefaultValue(2)]
        public int GenesisPlayers5 { get; set; }

        [Setting, DefaultValue(75)]
        public int Swiftmend5 { get; set; }

        //restoration 5-10
        [Setting, DefaultValue(85)]
        public int Rejuvenation510 { get; set; }

        [Setting, DefaultValue(60)]
        public int Regrowth510 { get; set; }

        [Setting, DefaultValue(40)]
        public int HealingTouch510 { get; set; }

        [Setting, DefaultValue(65)]
        public int ForceOfNature510 { get; set; }

        [Setting, DefaultValue(80)]
        public int WildGrowth510 { get; set; }

        [Setting, DefaultValue(45)]
        public int Tranquility510 { get; set; }

        [Setting, DefaultValue(65)]
        public int Genesis510 { get; set; }

        [Setting, DefaultValue(3)]
        public int WildGrowthPlayers510 { get; set; }

        [Setting, DefaultValue(5)]
        public int TranquilityPlayers510 { get; set; }

        [Setting, DefaultValue(3)]
        public int GenesisPlayers510 { get; set; }

        [Setting, DefaultValue(75)]
        public int Swiftmend510 { get; set; }

        //restoration 10+
        [Setting, DefaultValue(85)]
        public int Rejuvenation50 { get; set; }

        [Setting, DefaultValue(60)]
        public int Regrowth50 { get; set; }

        [Setting, DefaultValue(40)]
        public int HealingTouch50 { get; set; }

        [Setting, DefaultValue(65)]
        public int ForceOfNature50 { get; set; }

        [Setting, DefaultValue(75)]
        public int WildGrowth50 { get; set; }

        [Setting, DefaultValue(45)]
        public int Tranquility50 { get; set; }

        [Setting, DefaultValue(65)]
        public int Genesis50 { get; set; }

        [Setting, DefaultValue(4)]
        public int WildGrowthPlayers50 { get; set; }

        [Setting, DefaultValue(6)]
        public int TranquilityPlayers50 { get; set; }

        [Setting, DefaultValue(3)]
        public int GenesisPlayers50 { get; set; }

        [Setting, DefaultValue(75)]
        public int Swiftmend50 { get; set; }

        //resto trinkets
        [Setting, DefaultValue(1)]
        public int Trinket1Resto { get; set; }

        [Setting, DefaultValue(false)]
        public bool Trinket1UseResto { get; set; }

        [Setting, DefaultValue(1)]
        public int Trinket2Resto { get; set; }

        [Setting, DefaultValue(false)]
        public bool Trinket2UseResto { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket1HPResto { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket1ManaResto { get; set; }

        [Setting, DefaultValue(25)]
        public int PercentTrinket1EnergyResto { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket2HPResto { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket2ManaResto { get; set; }

        [Setting, DefaultValue(25)]
        public int PercentTrinket2EnergyResto { get; set; }

        [Setting, DefaultValue(4)]
        public int AoeCat { get; set; }

        [Setting, DefaultValue(4)]
        public int AoeMoonkin { get; set; }
    }
}
