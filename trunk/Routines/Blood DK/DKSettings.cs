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
            :base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Routines/DeathKnight/{0}/{1}/DKSettings.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name)))
        {
        }

        
        [Setting, DefaultValue(true)]
        [Category("Movement, Facing")]
        [Description("Auto Movement")]
        public bool AutoMovement { get; set; }

        /*[Setting, DefaultValue(true)]
        [Category("Movement, Facing")]
        [Description("Auto Targeting")]
        public bool AutoTargeting { get; set; }*/

        [Setting, DefaultValue(true)]
        [Category("Movement, Facing")]
        [Description("Auto Facing")]
        public bool AutoFacing { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Movement, Facing in Group")]
        [Description("Disable Movement")]
        public bool AutoMovementDisable { get; set; }

        /*[Setting, DefaultValue(true)]
        [Category("Movement, Facing in Group")]
        [Description("Disable Targeting")]
        public bool AutoTargetingDisable { get; set; }*/

        [Setting, DefaultValue(true)]
        [Category("Movement, Facing in Group")]
        [Description("Disable Facing")]
        public bool AutoFacingDisable { get; set; }


        [Setting, DefaultValue(Keys.None)]
        [Category("Hotkeys")]
        [Description("Stop Aoe")]
        public Keys KeyStopAoe { get; set; }

        [Setting, DefaultValue(Keys.None)]
        [Category("Hotkeys")]
        [Description("Use Cooldowns")]
        public Keys KeyUseCooldowns { get; set; }

        [Setting, DefaultValue(Keys.None)]
        [Category("Hotkeys")]
        [Description("Pause CR")]
        public Keys KeyPauseCR { get; set; }

        [Setting, DefaultValue(Keys.None)]
        [Category("Hotkeys")]
        [Description("Play Manual")]
        public Keys KeyPlayManual { get; set; }

        [Setting, DefaultValue(Keys.None)]
        [Category("Hotkeys")]
        [Description("Res Tanks")]
        public Keys KeyResTanks { get; set; }

        [Setting, DefaultValue(Keys.None)]
        [Category("Hotkeys")]
        [Description("Res Healers")]
        public Keys KeyResHealers { get; set; }

        [Setting, DefaultValue(Keys.None)]
        [Category("Hotkeys")]
        [Description("Res Damage")]
        public Keys KeyResDps { get; set; }

        [Setting, DefaultValue(true)]
        [Category("Hotkeys")]
        [Description("Print Raid Style Msgs")]
        public bool PrintRaidstyleMsg { get; set; }

        [Setting, DefaultValue(true)]
        [Category("DeathKnight")]
        [DisplayName("Auto Interrupt")]
        public bool AutoInterrupt { get; set; }

        [Setting, DefaultValue(50)]
        [Category("Rest Behavior")]
        [DisplayName("Eat Food HP%")]
        public int FoodHPOoC { get; set; }

        
        [Setting, DefaultValue(45)]
        [Category("DeathKnight")]
        [DisplayName("Healthstone HP %")]
        public int PercentHealthstone { get; set; }

        [Setting, DefaultValue(50)]
        [Category("DeathKnight")]
        [DisplayName("Gift of the Naaru HP %")]
        public int PercentNaaru { get; set; }

        public enum presence
        {
            Manual,
            Blood,
            Frost,
            Unholy
        }

        [Setting, DefaultValue(presence.Blood)]
        [Category("DeathKnight")]
        [DisplayName("Presence")]
        public presence Presence { get; set; }

        [Setting, DefaultValue(2)]
        [Category("DeathKnight")]
        [DisplayName("Death and Decay Adds")]
        public int AddsDeathAndDecay { get; set; }

        [Setting, DefaultValue(2)]
        [Category("DeathKnight")]
        [DisplayName("Defile Adds")]
        public int AddsDefile { get; set; }

        [Setting, DefaultValue(65)]
        [Category("DeathKnight")]
        [DisplayName("Icebound Fortitude HP %")]
        public int IceBoundFortitude { get; set; }

        [Setting, DefaultValue(80)]
        [Category("DeathKnight")]
        [DisplayName("Dancing Rune Weapon HP %")]
        public int DancingRuneWeapon { get; set; }

        [Setting, DefaultValue(55)]
        [Category("DeathKnight")]
        [DisplayName("Vampiric Blood HP %")]
        public int VampiricBlood { get; set; }

        [Setting, DefaultValue(45)]
        [Category("DeathKnight")]
        [DisplayName("Death Pact HP %")]
        public int DeathPact { get; set; }

        [Setting, DefaultValue(0)]
        [Category("DeathKnight")]
        [DisplayName("Conversion HP %")]
        public int Conversion { get; set; }

        [Setting, DefaultValue(0)]
        [Category("DeathKnight")]
        [DisplayName("Trinket 1 HP %")]
        public int Trinket1HP { get; set; }

        [Setting, DefaultValue(0)]
        [Category("DeathKnight")]
        [DisplayName("Trinket 2 HP %")]
        public int Trinket2HP { get; set; }

        [Setting, DefaultValue(30)]
        [Category("DeathKnight")]
        [DisplayName("Rune Tap HP %")]
        public int RuneTap { get; set; }

        [Setting, DefaultValue(0)]
        [Category("DeathKnight")]
        [DisplayName("Gorefiends's Grasp adds")]
        public int Gorefiend { get; set; }

        [Setting, DefaultValue(0)]
        [Category("DeathKnight")]
        [DisplayName("Death Strike below HP%")]
        public int DeathStrikeHP { get; set; }

        [Setting, DefaultValue(0)]
        [Category("DeathKnight")]
        [DisplayName("Blood Tap below HP%")]
        public int BloodTapHP { get; set; }
    }
}
