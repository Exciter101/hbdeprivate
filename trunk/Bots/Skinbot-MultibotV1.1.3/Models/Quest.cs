using Eclipse.EclipsePlugins.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.WoWDatabase.Models
{
    public class Quest
    {
        public uint Id { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public bool IsDaily { get; set; }
        public bool IsWeekly { get; set; }
        public bool IsShareable { get; set; }
        public string Description { get; set; }
        public int RequiredLevel { get; set; }
        public string ObjectiveText { get; set; }
        public uint RecievedFrom { get; set; }
        public uint TurnInTo { get; set; }
        public uint Money { get; set; }
        public int Side { get; set; }
        public uint Reputation { get; set; }
        public uint Faction { get; set; }
        public List<QuestOrder> QuestOrders { get; set; }
    }
}
