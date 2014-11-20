using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Helpers;

namespace BattlePetAuctionViewer
{
    public class PetJournal : IPetJournal
    {
        private IPluginLogger _logger;
        private IPetLua _petLua;
        private IProgress _progress;

        public PetJournal(IPluginLogger logger, IPetLua petLua,IProgress progress)
        {
            _logger = logger;
            _petLua = petLua;
            _progress = progress;
        }

        List<Pet> _petsOwned = new List<Pet>();
        public List<Pet> PetsOwned
        {
            get { return _petsOwned; }
        }

        List<Pet> _petsNotOwned = new List<Pet>();
        public List<Pet> PetsNotOwned
        {
            get { return _petsNotOwned; }
        }

        public bool IsLoaded { get { return _petsOwned.Count > 0 || _petsNotOwned.Count > 0; } }

        public void PopulatePetJournal()
        {
            try
            {
                _progress.Step(1, 4,"Reading owned pets:");
                _logger.Write("Reading owned pets...");
                _petLua.SetFilterOwnedPets();
                _petsOwned = LoadFromJournal();
                _logger.Write(string.Format("Read {0} distinct pets owned.", _petsOwned.Count));
                //Pet.Dump(@"c:\OwnedPets.csv", _petsOwned, _logger);

                _progress.Step(2, 4, "Reading pets not owned:");
                _logger.Write("Reading pets not owned...");
                _petLua.SetFilterNonOwnedPets();
                _petsNotOwned = LoadFromJournal();
                _logger.Write(string.Format("Read {0} pets not yet owned.", _petsNotOwned.Count));
                //Pet.Dump(@"c:\PetsNotOwned.csv", _petsNotOwned, _logger);
            }
            catch
            {
                _logger.Write("Journal init query fail!!! ");
                try
                {
                    int PetCount = _petLua.GetNumPets();
                    int PetsOwned = _petLua.GetNumPetsOwned();
                    _logger.Write("Query too large?? " + PetsOwned + "," + PetCount);
                }
                catch
                {
                    _logger.Write("simple C_PetJournal.GetNumPets function failed. Try in WoW: \n/run local numPets, numOwned = C_PetJournal.GetNumPets(false); print('Journal: Pet count:' .. tostring(numOwned) .. ' total:' .. tostring(numPets));");
                }
            }
        }

        private List<Pet> LoadFromJournal()
        {
            List<Pet> pets = new List<Pet>();
            List<string> names = new List<string>();

            int partsize = 10;
            int PetsOwned = _petLua.GetNumPets();
            

            if (PetsOwned == 0) { _logger.Write("0 pets to read."); return new List<Pet>(); }

            int remaining = (PetsOwned - 1) % partsize;
            int maxportions = ((PetsOwned - 1) / partsize) + 1;

            for (uint k = 0; k < maxportions; k++)
            {
                _progress.SubStep((int)k, maxportions-1);

                int currentportionsize = ((k == maxportions - 1) ? remaining : partsize - 1);
                List<string> petData = _petLua.GetPetInfoByIndex(partsize, k, currentportionsize);

                for (int i = 0; i < petData.Count(); i += 3)
                {
                    string name = petData[i];
                    int id = int.Parse(petData[i + 1]);
                    string info = petData[i + 2];

                    pets.Add(Pet.CreatePet(name, id, info));
                    names.Add(name);
                };
            }
            return pets;
        }
    }
}