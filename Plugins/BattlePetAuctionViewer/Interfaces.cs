using System.Collections.Generic;
using System;

namespace BattlePetAuctionViewer
{
    public interface IPluginLogger
    {
        void Write(string message);
        void WriteError(string message,Exception e);
        void WriteError(Exception e);
        void WriteVerbose(string message);
    }

    public interface IProgress
    {
        void Step(int step, int maxSteps, string description);
        void SubStep(int step, int maxSteps);
    }

    public interface IPetLua
    {
        void SetFilterOwnedPets();
        void SetFilterNonOwnedPets();
        void SetFavouritesFlag();
        int GetLevelBySlotID_Enemy(int slotID);
        int GetNumPets();
        int GetNumPetsOwned();
        System.Collections.Generic.List<string> GetPetInfoByIndex(int partsize, uint k, int currentportionsize);
        System.Collections.Generic.List<string> GetPetStats(string PetID);
        int GetTargetLevel();
        bool IsInBattle();
        bool IsSummonable(string PetID);
        void LoadPet(int slot, string petID);
        string GetNameBySlotID_Enemy(int slotID);
        void ForfeitGame();
        void ResurrectPets();
    }
    public interface IPetJournal
    {
        bool IsLoaded { get; }
        void PopulatePetJournal();
        List<Pet> PetsOwned { get; }
        List<Pet> PetsNotOwned { get; }
    }
}