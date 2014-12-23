using Eclipse.WoWDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class Boss : Mob
    {
        public string isFinal  {get;set;}
        public string Optional { get; set; }
        public string KillOrder { get; set; }
        public DBPath Path { get; set; }
    }
}
