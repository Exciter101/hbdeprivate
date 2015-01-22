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

namespace DK
{
    class DKSettings : Settings
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static readonly DKSettings myPrefs = new DKSettings();
        
        public DKSettings() 
            :base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Routines/Settings/DK/{0}-DKSettings-{1}.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name)))
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
        public string ModifkeyResDPS { get; set; }

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
        public Keys KeyResHealers { get; set; }

        [Setting, DefaultValue(KeyPress.None)]
        public Keys KeyResDps { get; set; }

        [Setting, DefaultValue(true)]
        public bool PrintRaidstyleMsg { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoInterrupt { get; set; }

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

        [Setting, DefaultValue(90)]
        public int PercentPowerWordShield { get; set; }

        [Setting, DefaultValue(3)]
        public int Racial { get; set; }

        [Setting, DefaultValue(1)]
        public int RaidFlask { get; set; }

        [Setting, DefaultValue(5)]
        public int MindSearAdds { get; set; }

        [Setting, DefaultValue(76084)]
        public int RaidFlaskKind { get; set; }

        [Setting, DefaultValue(95)]
        public int PercentRenewCombat { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentRenewOoC { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentFlashHealOoC { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentFlashHealCombat { get; set; }

        [Setting, DefaultValue(0)]
        public int PercentFiendMana { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentHealthstone { get; set; }

        [Setting, DefaultValue(50)]
        public int PercentNaaru { get; set; }

        [Setting, DefaultValue(false)]
        public bool FlaskCrystal { get; set; }

        [Setting, DefaultValue(false)]
        public bool FlaskAlchemy { get; set; }

        [Setting, DefaultValue(false)]
        public bool FlaskOraliusCrystal { get; set; }

        [Setting, DefaultValue(1)]
        public int CDBerserking { get; set; }

        [Setting, DefaultValue(false)]
        public bool MsgBoxShown { get; set; }

        [Setting, DefaultValue(1)]
        public int Presence { get; set; }

        [Setting, DefaultValue(2)]
        public int AddsDeathAndDecay { get; set; }

        [Setting, DefaultValue(2)]
        public int DeathAndDecayRunes { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseDeathAndDecayRunes { get; set; }

        [Setting, DefaultValue(2)]
        public int AddsDefile { get; set; }

        [Setting, DefaultValue(2)]
        public int DefileRunes { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseDefileRunes { get; set; }

    }
}
