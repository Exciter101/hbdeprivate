using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePetCollector
{
    public class EnemyPet
    {
        public string Name { get; set; }
        public string Rarity { get; set; }
        public bool IsRare { get { return Rarity == "4" || Rarity == "Rare"; } }
    }
}
