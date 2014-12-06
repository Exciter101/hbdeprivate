using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.WoWDatabase.Models
{
    public class Faction
    {
        public uint FactionId { get; set; }
        public string Name { get; set; }
        public bool IsSkinnable { get; set; }
        public uint Zone { get; set; }

    }
}
