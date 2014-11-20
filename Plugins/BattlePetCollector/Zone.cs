using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePetCollector
{
    public class Zone
    {
        public string Continent
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            private set;
        }

        public Zone(string continent, string zone)
        {
            Continent = RemoveApostrophe(continent);
            Name = RemoveApostrophe(zone);
            Continent = TranslateContinentName(Continent, Name);
        }

        private string RemoveApostrophe(string value)
        {
            return value.Replace("'", "");
        }


        private string TranslateContinentName(string continent, string zone)
        {
            if (zone == "Azuremyst Isle" || zone == "Bloodmyst Isle" || zone == "Exodar")
            {
                return "Expansion01B";
            }

            if (zone == "Ghostlands" || zone == "Eversong Woods")
            {
                return "Expansion01C";
            }
            return continent;
        }
        public override string ToString()
        {
            return Continent + "-" + Name;
        }
    }
}
