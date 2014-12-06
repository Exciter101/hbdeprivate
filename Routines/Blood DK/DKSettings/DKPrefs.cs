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
using Form1 = DeathKnight.GUI.Form1;
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
#endregion

namespace DeathKnight.DKSettings
{
    class DKPrefs : Settings
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static readonly DKPrefs myPrefs = new DKPrefs();

        public DKPrefs()
            : base(
                Path.Combine(Utilities.AssemblyDirectory,
                    string.Format(@"Routines/Settings/DeathKnight/{0}-SVSettings-{1}.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name))
                )
        {
        }

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

        [Setting, DefaultValue(true)]
        public bool PrintRaidstyleMsg { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoInterrupt { get; set; }

        [Setting, DefaultValue(50)]
        public int FoodHPOoC { get; set; }

        [Setting, DefaultValue(1)]
        public int Trinket1 { get; set; }

        [Setting, DefaultValue(1)]
        public int Trinket2 { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentTrinket1HP { get; set; }
        [Setting, DefaultValue(45)]
        public int PercentTrinket2HP { get; set; }

        [Setting, DefaultValue(1)]
        public int Gloves { get; set; }

        [Setting, DefaultValue(3)]
        public int Racial { get; set; }

        [Setting, DefaultValue(1)]
        public int RaidFlask { get; set; }

        [Setting, DefaultValue(1)]
        public int RaidFlaskKind { get; set; }

        [Setting, DefaultValue(45)]
        public int PercentHealthstone { get; set; }

        [Setting, DefaultValue(50)]
        public int PercentNaaru { get; set; }

        [Setting, DefaultValue(60)]
        public int PercentFortitude { get; set; }

        [Setting, DefaultValue(70)]
        public int PercentVampiric { get; set; }

        [Setting, DefaultValue(50)]
        public int PercentRuneTap { get; set; }

        [Setting, DefaultValue(85)]
        public int PercentConversion { get; set; }

        [Setting, DefaultValue(90)]
        public int PercentDancing { get; set; }

        [Setting, DefaultValue(false)]
        public bool FlaskCrystal { get; set; }

        [Setting, DefaultValue(false)]
        public bool FlaskAlchemy { get; set; }

        [Setting, DefaultValue(1)]
        public int Presence { get; set; }
    }
}
