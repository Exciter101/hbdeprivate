using System;
using System.Collections.Generic;
using System.Linq;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Threading;

namespace BattlePetCollector
{
    public class Plugin : HBPlugin
    {
        private IPluginLogger _logger;
        private IPluginSettings _pluginSettings;
        private IPluginProperties _pluginProperties;
        private IPetLua _petLua;
        private IPetJournal _petJournal;
        private IPetDatabase _petDatabase;
        private IZoneTimeout _zoneTimeout;

        public IPetDatabase PetDatabase
        {
            get { return _petDatabase; }
        }
        private IProfileLoader _profileLoader;

        private int _lastPulse = int.MinValue;
        private int _lastPulseZoneCheck = int.MinValue;
        private bool _testMode = true;

        public Plugin()
        {
            _logger = new PluginLogger();
            _pluginSettings = new PluginSettings(_logger, Styx.Helpers.Settings.SettingsDirectory);
            _pluginProperties = _pluginSettings as IPluginProperties;
            _petLua = new PetLua(_logger);
            _petJournal = new PetJournal(_logger, _pluginProperties,_petLua);
            _profileLoader = new ProfileLoader(_logger, Styx.Plugins.PluginManager.PluginsDirectory);
            _zoneTimeout = new ZoneTimeout(_logger, _pluginProperties);
        }

        public override void Pulse()
        {
            Zone currentZone = new Zone(Me.MapName, Me.ZoneText);

            try
            {
                if (_lastPulse + 3000 < Environment.TickCount || !_petJournal.IsLoaded)
                {
                    _zoneTimeout.UpdateCurrentZoneEntry(currentZone);

                    if (!_petDatabase.IsLoaded) { _petDatabase.Load(); }

                    if (_testMode) { ValidateProfilePaths(); }

                    if (_pluginProperties.BlackListUnrequiredPets)
                    {
                        BlacklistUnrequiredUnits();
                    }

                    if (!_petLua.IsInBattle())
                    {
                        if (_lastPulseZoneCheck + 60000 < Environment.TickCount || !_petJournal.IsLoaded)
                        {
                            _zoneTimeout.LogTimeInZone(currentZone);

                            CheckPetsAgainstZone();
                            _lastPulseZoneCheck = Environment.TickCount;
                        }
                    }
                    else
                    {
                        ForfeitIfNoRequiredPets();
                    }
                    _lastPulse = Environment.TickCount;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError("Pulse() ",ex);
            }
        }

        private void ValidateProfilePaths()
        {
            _testMode=false;
            foreach(string profile in _petDatabase.GetAllProfiles())
            {
                _profileLoader.CheckFileExists(profile);
            }
        }

        private void ForfeitIfNoRequiredPets()
        {
            if (!_pluginProperties.ForfeitWhenOwned && !_pluginProperties.ForfeitWhenNotBlue) { return; }

            List<EnemyPet> enemyPets = _petLua.GetEnemyPetInfo();

            if (_pluginProperties.ForfeitWhenOwned)
            {
                ForfeitWhenOwned(enemyPets);
            }

            if (_pluginProperties.ForfeitWhenNotBlue)
            {
                ForfeitWhenNotBlue(enemyPets);
            }
            
        }
        private void ForfeitWhenOwned(List<EnemyPet> enemyPets)
        {
            bool needed = false;
            foreach (EnemyPet pet in enemyPets)
            {
                if (string.IsNullOrEmpty(pet.Name))
                {
                    _logger.Write("Failed to read enemy pet name");
                    needed = true;
                }
                else
                {
                    List<Pet> ownedZonePets = _petJournal.OwnedPetsList.FindAll((ownedPet) => { return ownedPet.Name.Equals(pet.Name); });
                    bool needThisPet = ownedZonePets.Count == 0 || (pet.IsRare && ownedZonePets.Count < 3);
                    string rarity = pet.IsRare ? "rare" : "not rare";
                    _logger.WriteVerbose((needThisPet ? "Need" : "Don't need") + " to trap pet. It is " + rarity + " I have " + ownedZonePets.Count + " of them: ");
                    if (needThisPet) { needed = true; }
                }
            }
            if (!needed)
            {
                _logger.Write("Don't need any of these pets, forfeiting !");
                _petLua.ForfeitGame();
            }
        }

        private void ForfeitWhenNotBlue(List<EnemyPet> enemyPets)
        {
            bool needed = false;
            foreach (EnemyPet pet in enemyPets)
            {
                if (string.IsNullOrEmpty(pet.Name))
                {
                    _logger.Write("Failed to read enemy pet name");
                    needed = true;
                }
                else
                {
                    List<Pet> ownedZonePets = _petJournal.OwnedPetsList.FindAll((ownedPet) => { return ownedPet.Name.Equals(pet.Name); });
                    bool needThisPet = ownedZonePets.Count < 3 && pet.IsRare;
                    needed = needed || needThisPet;
                    _logger.WriteVerbose((needThisPet ? "Rare, need" : "Not rare or have 3, don't need") + " to trap: " + pet.Name);
                }
            }
            if (!needed)
            {
                _logger.Write("Don't need any of these pets, forfeiting !!!");
                _petLua.ForfeitGame();
            }
        }

        private void CheckPetsAgainstZone()
        {
            Zone currentZone = new Zone(Me.MapName, Me.ZoneText);

            if (!_petJournal.IsLoaded || _petLua.GetNumPetsOwned() != _petJournal.OwnedPetsList.Count)
            {
                long distinctPetsCount=0;
                distinctPetsCount=_petJournal.DistinctPetNames.Count;
                _petJournal.PopulatePetJournal();
                if (distinctPetsCount != _petJournal.DistinctPetNames.Count) { _zoneTimeout.ResetZoneTimeout(); }
            }

            LogContinentPetsToCatch();

            List<PetInfo> zonePetsNotOwned = GetZonePetsToCatch(currentZone);

            if (zonePetsNotOwned.Count == 0 || _zoneTimeout.ZoneTimeoutReached(currentZone) || _zoneTimeout.HasVistedZone(currentZone))
            {
                if (zonePetsNotOwned.Count == 0)
                {
                    _logger.Write("No pets to trap in " + currentZone);
                }
                else if (_zoneTimeout.ZoneTimeoutReached(currentZone))
                {
                    _logger.Write("Zone has just timed out " + currentZone);
                }
                else
                {
                    _logger.Write("Zone has already been visited " + currentZone);
                }
                if (!SwitchZone(currentZone))
                {
                    if (_zoneTimeout.HasVistedZones)
                    {
                        _logger.Write("Reset blacklisted zones.");
                        _zoneTimeout.ClearVisitedZones();
                    }
                    else
                    {
                        if (this._pluginProperties.StopWhenAllPetsCaught)
                        {
                            TreeRoot.Stop("No pets to catch");
                        }
                        else
                        {
                            _logger.Write("No pets to catch on this continent " + Me.MapName);
                        }
                    }
                }
            }
            else
            {
                _logger.Write("Pets to trap in " + Me.ZoneText + " >> " + PetInfo.PetNames(zonePetsNotOwned));
                SwitchProfile(zonePetsNotOwned,currentZone);
            }
        }

        private List<PetInfo> GetZonePetsToCatch(Zone zone)
        {
            List<PetInfo> zonePetsList = _petDatabase.GetPetsForZone(zone);
            ZonePets zonePets = new ZonePets(zonePetsList, _pluginProperties);
            List<PetInfo> zonePetsNotOwned = zonePets.NotOwned(_petJournal.OwnedPetsList);
            return zonePetsNotOwned;
        }

        private void LogContinentPetsToCatch()
        {
            Zone currentZone = new Zone(Me.MapName, Me.ZoneText);
            List<PetInfo> continentPetsList = _petDatabase.GetPetsForContinent(currentZone);
            ZonePets continentPets = new ZonePets(continentPetsList, _pluginProperties);
            List<PetInfo> continentPetsNotOwned = continentPets.NotOwned(_petJournal.OwnedPetsList);
            int percentageCaught = (int)(((double)continentPetsList.Count - continentPetsNotOwned.Count) / continentPetsList.Count * 100);
            _logger.Write(percentageCaught.ToString() + "% of pets on the continent caught (" + continentPetsNotOwned.Count + " to trap)");
        }

        private void SwitchProfile(List<PetInfo> zonePetsNotOwned, Zone zone)
        {
            string firstProfile = zonePetsNotOwned[0].Profile;
            SwitchProfile(firstProfile, zone);
        }

        private void SwitchProfile(string firstProfile,Zone zone)
        {
            if (!_profileLoader.IsOnProfile(firstProfile))
            {
                _zoneTimeout.SetTargetZone(zone);
                _profileLoader.SwitchToProfile(firstProfile);
            }
        }

        private bool SwitchZone(Zone currentZone)
        {
            Zone newZone = NextZone(_petJournal.OwnedPetsList, currentZone);
            if (newZone != null)
            {
                _logger.Write("Moving to zone " + newZone+".");
                string profile = _petDatabase.GetDefaultProfileForZone(newZone);
                SwitchProfile(profile,newZone);
                return true;
            }
            else
            {
                _logger.Write("Can't find any zones with pets left to trap in " + Me.MapName+"!");
                return false;
            }
        }

        public Zone NextZone(List<Pet> ownedPets,Zone currentZone)
        {
            List<Zone> zones = _petDatabase.GetZones(currentZone);

            foreach (Zone zone in zones)
            {
                if (_zoneTimeout.HasVistedZone(zone)) { continue; }
                List<PetInfo> zonePetsList = _petDatabase.GetPetsForZone(zone);
                ZonePets zonePets = new ZonePets(zonePetsList, _pluginProperties);
                List<PetInfo> zonePetsNotOwned = zonePets.NotOwned(ownedPets);
                if (zonePetsNotOwned.Count > 0)
                {
                    _logger.Write("Need pets in " + zone + ". >> " + PetInfo.PetNames(zonePetsNotOwned));
                    return zone;
                }
            }
            return null;
        }

        public void BlacklistUnrequiredUnits()
        {
            foreach (WoWUnit unit in (from unit in ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                                      where unit.IsPetBattleCritter && !unit.IsDead && !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                                      select unit))
            {
                if (_petJournal.OwnedPetsList.FindAll((ownedPet) => 
                {
                    return ownedPet.Name.Equals(unit.Name) && (!_pluginProperties.OnlyConsiderRarePetsAsCaught || ownedPet.IsRare);
                }).Count > 0)
                {
                    _logger.WriteVerbose("Blacklisting " + unit.Name+" as not required.");
                    try
                    {
                        Blacklist.Add(unit.Guid, BlacklistFlags.All, TimeSpan.FromMinutes(120));
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteError(ex);
                    }
                }
                else
                {
                    _logger.WriteVerbose("Need to battle " + unit.Name);
                }
            }
        }

        #region Plugin Properties / Settings Button

        private static LocalPlayer Me { get { return Styx.StyxWoW.Me; } }
        public override string Name { get { return "Battle Pet Collector"; } }
        public override string Author { get { return "Andy West"; } }
        public override Version Version { get { return new Version(1, 0, 2, 1); } }
        public override string ButtonText { get { return "Configuration"; } }
        public override bool WantButton { get { return true; } }

        public override void OnButtonPress()
        {
            new PluginSettingsForm(_pluginSettings, _logger).Show();
        }

        #endregion

        #region Bot Events

        public override void Initialize()
        {
            BotEvents.OnBotStarted += BotEvents_OnBotStarted;
            BotEvents.OnBotStopped += BotEvents_OnBotStopped;
            _logger.Write(Name + " loaded (V" + Version.ToString() + ")");
            _logger.WriteVerbose(_pluginSettings.ToString());
        }

        void BotEvents_OnBotStarted(EventArgs args)
        {
            if (Me.IsDead || Me.IsGhost || !Styx.StyxWoW.IsInGame) return;
            _petJournal.Clear();
            _petDatabase = new PetDatabase(_logger);
            _logger.Write("Continent: " + Me.MapName);
        }

        void BotEvents_OnBotStopped(EventArgs args)
        {
        }

        #endregion
    }
}
