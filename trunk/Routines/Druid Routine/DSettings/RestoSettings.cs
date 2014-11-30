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

namespace Druid.DSettings
{
    class RestoSettings : Settings
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static readonly RestoSettings myPrefs = new RestoSettings();

        public RestoSettings()
            : base(
                Path.Combine(Utilities.AssemblyDirectory,
                    string.Format(@"Routines/Settings/Druid/Restoration/{0}-RestoDruidSettings-{1}.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name))
                )
        {
        }

        //healing
        [Setting, DefaultValue(90)]
        [Category("Dungeon")]
        [DisplayName("Rejuvenation HP %")]
        [Description("Cast Rejuvenation on targets below this Value")]
        public int HealRejuD { get; set; }

        [Setting, DefaultValue(80)]
        [Category("Dungeon")]
        [DisplayName("Wild Growth")]
        [Description("Cast Wild Growth Players HP <= Value")]
        public int HealWildGrowthD { get; set; }

        [Setting, DefaultValue(2)]
        [Category("Dungeon")]
        [DisplayName("Wild Growth Players")]
        [Description("Cast Wild Growth Number Players >= Value")]
        public int HealWildGrowth5 { get; set; }

        [Setting, DefaultValue(75)]
        [Category("Dungeon")]
        [DisplayName("Force of Nature")]
        [Description("Cast FoN on players HP  <= Value")]
        public int HealWithFonD { get; set; }

        [Setting, DefaultValue(70)]
        [Category("Dungeon")]
        [DisplayName("Regrowth")]
        [Description("Cast Regrowth Players HP <= Value")]
        public int HealRegrowthD { get; set; }

        [Setting, DefaultValue(60)]
        [Category("Dungeon")]
        [DisplayName("Tranquility")]
        [Description("Cast Tranquility Players HP <= Value. 0 = disable")]
        public int HealTranquilityD { get; set; }

        [Setting, DefaultValue(4)]
        [Category("Dungeon")]
        [DisplayName("Tranquility Players")]
        [Description("Cast Tranquility Number Players >= Value")]
        public int HealTranquility5 { get; set; }

        [Setting, DefaultValue(60)]
        [Category("Dungeon")]
        [DisplayName("Healing Touch")]
        [Description("Cast Healing Touch Players HP <= Value")]
        public int HealTouchD { get; set; }

        //raid
        [Setting, DefaultValue(90)]
        [Category("Raid")]
        [DisplayName("Rejuvenation HP %")]
        [Description("Cast Rejuvenation on targets below this Value")]
        public int HealRejuR { get; set; }

        [Setting, DefaultValue(80)]
        [Category("Raid")]
        [DisplayName("Wild Growth")]
        [Description("Cast Wild Growth Players HP <= Value")]
        public int HealWildGrowthR { get; set; }

        [Setting, DefaultValue(75)]
        [Category("Raid")]
        [DisplayName("Force of Nature")]
        [Description("Cast FoN on players HP  <= Value")]
        public int HealWithFonR { get; set; }

        [Setting, DefaultValue(70)]
        [Category("Raid")]
        [DisplayName("Regrowth")]
        [Description("Cast Regrowth Players HP <= Value")]
        public int HealRegrowthR { get; set; }

        [Setting, DefaultValue(60)]
        [Category("Raid")]
        [DisplayName("Tranquility")]
        [Description("Cast Tranquility Players HP <= Value. 0 = disable")]
        public int HealTranquilityR { get; set; }

        [Setting, DefaultValue(6)]
        [Category("Raid")]
        [DisplayName("Tranquility Players")]
        [Description("Cast Tranquility Number Players >= Value")]
        public int HealTranquility10 { get; set; }

        [Setting, DefaultValue(60)]
        [Category("Raid")]
        [DisplayName("Healing Touch")]
        [Description("Cast Healing Touch Players HP w= Value")]
        public int HealTouchR { get; set; }

        [Setting, DefaultValue(3)]
        [Category("Raid")]
        [DisplayName("Wild Growth Players")]
        [Description("Cast Wild Growth number Players >= Value")]
        public int HealWildGrowth10 { get; set; }


    }
}
