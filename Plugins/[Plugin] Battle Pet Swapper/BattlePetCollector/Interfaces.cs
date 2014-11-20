using System.Collections.Generic;
using BattlePetCollector;
using System;

namespace BattlePetCollector
{
    public interface IPluginLogger
    {
        void Write(string message);
        void WriteError(string message,Exception e);
        void WriteError(Exception e);
        void WriteVerbose(string message);
        void StatusUpdate(string message);
    }

    public interface IPetLua
    {
        void SetFilterAllCollectedPets();
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
        List<EnemyPet> GetEnemyPetInfo();
    }

    public interface IPluginSettings
    {
        void ConvertSettingsToProperties();
        void ConvertsPropertiesToSettings();
        void Save();
    }

    public interface IPetChooser
    {
        List<Pet> SelectPetsForLevel(List<Pet> ownedPetsList, int Level);
    }

    public interface IPetJournal
    {
        bool IsLoaded { get; }
        void PopulatePetJournal();
        void Clear();
        List<Pet> OwnedPetsList { get; }
        List<Pet> FavouritePetsList { get; }
        List<string> DistinctPetNames { get; }
    }

    public interface IPetLoader
    {
        void Load(List<Pet> selectedpets);
    }

    public interface IProfileLoader
    {
        bool IsOnProfile(string firstProfile);
        bool SwitchToProfile(string firstProfile);
        DateTime ProfileLoadTime
        {
            get;
        }

        void CheckFileExists(string profile);
    }

    public interface IPetDatabase
    {
        List<PetInfo> GetPetsForZone(Zone zone);
        void Load();
        void Load(string path);
        List<Zone> GetZones(Zone zone);
        string GetDefaultProfileForZone(Zone zone);
        bool IsLoaded { get; }
        List<PetInfo> GetPetsForContinent(Zone zone);
        List<string> GetAllProfiles();
    }

    interface IZoneTimeout
    {
        void SetTargetZone(Zone zone);
        bool ZoneTimeoutReached(Zone currentZone);
        bool HasVistedZone(Zone zone);
        bool HasVistedZones { get; }
        void ClearVisitedZones();
        void LogTimeInZone(Zone currentZone);
        void UpdateCurrentZoneEntry(Zone currentZone);

        void ResetZoneTimeout();
    }

    public interface IPluginProperties
    {
        bool StopWhenAllPetsCaught { get; set; }
        int MaxMinsPerZone { get; set; }
        bool BlackListUnrequiredPets { get; set; }
        bool ForfeitWhenOwned { get; set; }
        bool ForfeitWhenNotBlue { get; set; }
        bool OnlyConsiderRarePetsAsCaught { get; set; }
    }
}