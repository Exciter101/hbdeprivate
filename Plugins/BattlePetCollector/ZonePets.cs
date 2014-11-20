using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePetCollector
{
    public class ZonePets
    {
        List<PetInfo> _zonePets;
        IPluginProperties _properties;

        public ZonePets(List<PetInfo> zonePets, IPluginProperties properties)
        {
            _zonePets = zonePets;
            _properties = properties;
        }

        public List<PetInfo> NotOwned(List<Pet> ownedPets)
        {
            List<PetInfo> zonePetsNotOwned = new List<PetInfo>();

            foreach (PetInfo zonePet in _zonePets)
            {
                List<Pet> ownedZonePets = ownedPets.FindAll((ownedPet) =>
                {
                    if (!ownedPet.CreatureID.Equals(zonePet.Id)) { return false; } // different pet id?
                    if (!_properties.OnlyConsiderRarePetsAsCaught || ownedPet.IsRare) { return true; } // is rare, or don't care
                    return ownedPets.FindAll((o) => { return o.CreatureID.Equals(zonePet.Id); }).Count == 3; // not owning 3 already
                });
                if (ownedZonePets.Count == 0)
                {
                    zonePetsNotOwned.Add(zonePet);
                }
            }
            return zonePetsNotOwned;
        }
    }
}
