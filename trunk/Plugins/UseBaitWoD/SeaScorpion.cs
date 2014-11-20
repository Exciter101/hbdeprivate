// Apoc (Penguin) helped Kickazz006 develop this plugin
// This Plugin drinks HP/Mana pots when low
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;

// HB Stuff
using Styx;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;


namespace Use_Bait___Sea_Scorpion_Bait
{
    public class Sea_Scorpion_Bait : HBPlugin
    {
        #region Globals

        public override string Name { get { return "Use Bait WoD - Sea Scorpion Bait - Open Sea"; } }
        public override string Author { get { return "Zenmate"; } }
        public override Version Version { get { return new Version(1, 0, 0); } }
        public override string ButtonText { get { return "magic!"; } }
        public override bool WantButton { get { return false; } }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #endregion

        public static WoWItem FindFirstUsableItemBySpell(params string[] spellNames)
		{
			List<WoWItem> carried = StyxWoW.Me.CarriedItems;
			// Yes, this is a bit of a hack. But the cost of creating an object each call, is negated by the speed of the Contains from a hash set.
			// So take your optimization bitching elsewhere.
			var spellNameHashes = new HashSet<string>(spellNames);

			return (from i in carried
					let spells = i.Effects
					where i.ItemInfo != null && spells != null && spells.Count != 0 &&
						  i.Usable &&
						  i.Cooldown == 0 &&
						  i.ItemInfo.RequiredLevel <= StyxWoW.Me.Level &&
						  spells.Any(s => s.Spell != null && spellNameHashes.Contains(s.Spell.Name))
					orderby i.ItemInfo.Level descending
					select i).FirstOrDefault();
		}

        public WoWItem FireAmmoniteBait()
        {
            return FindFirstUsableItemBySpell("Sea Scorpion Bait");
        }


        public override void Pulse()
        {
            if (Me.Combat || !Me.IsAlive || Me.IsGhost || Me.IsOnTransport || Me.OnTaxi || Me.Stunned || (Me.Mounted && Me.IsFlying)) // Chillax
            {
                return;
            }

            if (!Me.Combat) // Pay Attn!
            {
                if (!Me.HasAura("Sea Scorpion Bait"))
                {
                    WoWItem gPoL = FireAmmoniteBait();
                    if (gPoL != null)
                    {
                        gPoL.UseContainerItem();
                        Styx.Common.Logging.Write(System.Windows.Media.Color.FromRgb(255, 0, 255), "Used " + gPoL.Name + "!");
                    }
                    
                }
            }
        }
    }
}