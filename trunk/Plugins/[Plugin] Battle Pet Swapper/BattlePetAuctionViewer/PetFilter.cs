using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattlePetAuctionViewer;

namespace BattlePetAuctionViewer
{

    public class FilterArgs
    {
        public List<string> Sources { get; set; }
        public string NameOrId { get; set; }

        public bool OwnedPets { get; set; }

        public bool NotOwned { get; set; }
    }

    public class PetFilter
    {

        public List<Pet> Filter(List<Pet> pets,FilterArgs args)
        {
            List<Pet> result = new List<Pet>();
            foreach (Pet pet in pets)
            {
                bool add = true;
                
                //filter source
                add = add && (args.Sources.Contains(pet.Source) || string.IsNullOrEmpty(pet.Source));

                //filter name or Id
                string text = args.NameOrId == null ? "" : args.NameOrId.Trim().ToLower(); ;
                add = add && (string.IsNullOrEmpty(text) || pet.Name.Trim().ToLower().Contains(text) || pet.Id.ToString() == text);

                if (!args.OwnedPets || !args.NotOwned)
                {
                    if (!args.OwnedPets)
                    {
                        add = add && pet.OwnedCount == 0;
                    }

                    if (!args.NotOwned)
                    {
                        add = add && pet.OwnedCount > 0;
                    }
                }

                if (add)
                {
                    result.Add(pet);
                }
            }
            return result;
        }
    }
}
