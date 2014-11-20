using System;
using System.Collections.Generic;
using Styx.Helpers;

namespace BattlePetAuctionViewer
{
    public class Pet
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Source { get; set; }
        public string Info { get; set; }
        public long Buyout { get; set; }
        public long OwnedCount { get; set; }
        public double Popularity { get; set; }
        public string Auction { get; set; }

        public long BuyoutGold
        {
            get
            {
                if (Buyout <= 0) { return 0; }
                return (long)(Buyout / 10000);
            }
        }

        public Pet(string name, int id, string source, string info)
        {
            Name = name;
            Id = id;
            Source = source;
            Info = info;
        }
        public Pet(string name, long buyout)
        {
            Name = name;
            Buyout = buyout;
        }

        public override string ToString()
        {
            return Name;
        }

        public static Pet CreatePet(string name, int id, string infoText)
        {
            string[] infoFields = infoText.Split('|');

            string source = infoFields[1].Substring(9).Trim();
            string info = infoFields[2].Substring(1).Trim();

            if (source.EndsWith(":")) { source = source.Substring(0, source.Length - 1); }
            System.Diagnostics.Debug.WriteLine(source + "-" + info);

            return new Pet(name, id, source, info);
        }

        public static void Dump(string filename, List<Pet> pets, IPluginLogger logger)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                foreach (Pet pet in pets)
                {
                    file.WriteLine(pet.Name + "|" + pet.Id + "|" + pet.Source + "|" + pet.Info + "|" + pet.Buyout + "|" + pet.OwnedCount + "|" + pet.Auction);
                }
            }
            if (logger != null)
            {
                logger.Write("Pets dumped to " + filename);
            }
        }

      
    }
}