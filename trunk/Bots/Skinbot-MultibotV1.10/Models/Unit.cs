using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.WoWDatabase.Models
{
    public class Unit
    {
        public uint id { get { return Entry; } set { Entry = value; } }
        public string Name { get; set; }
        public uint Zone { get; set; }
        public uint Entry { get; set; }
        public uint FactionId { get; set; }
        public int Level { get; set; }

    }
}
