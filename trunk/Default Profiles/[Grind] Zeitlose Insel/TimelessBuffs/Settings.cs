using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Helpers;
using System.IO;
using Styx;
using Styx.Common;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.TreeSharp;

namespace TimelessBuffs
{
    public class TimelessSettings : Settings
    {
        public static readonly TimelessSettings myPrefs = new TimelessSettings();

        public TimelessSettings()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Plugins/PluginSettings/Pasterke/TimelessSettings.xml")))
        {
        }

        [Setting]
        [DefaultValue(true)]
        [Category("Crystal of Insanity")]
        [DisplayName("Crystal of Insanity")]
        [Description("Auto Buff Crystal of Insanity Everywhere")]
        public bool AutoBuffCrystal { get; set; }

        [Setting]
        [DefaultValue(95)]
        [Category("Health Buffs")]
        [DisplayName("Glowing Mushroom HP %")]
        [Description("Use Glowing Mushroom if HP % <= this Value")]
        public int AutoBuffGlowingMushroom { get; set; }
    }
}
