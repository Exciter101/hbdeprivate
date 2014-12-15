using Styx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.ShadowBot.Comms
{
    public class WowCharacter
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public WowLocation Location { get; set; }
        public int ZoneId { get; set; }
    }
}
