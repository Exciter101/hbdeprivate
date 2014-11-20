using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace BattlePetCollector
{
    public class PetDatabase : IPetDatabase
    {
        const string ContinentsFilename = @"..\..\Plugins\BattlePetCollector\Continents.xml";
        const string PetZonePetsXPath = "//Continent[@name='{0}']/Zone[@name='{1}']/Pet";
        const string PetZoneXPath = "//Continent[@name='{0}']/Zone[@name='{1}']";
        const string PetZonesXPath = "//Continent[@name='{0}']/Zone[@profile]";
        const string PetContinentPetsXPath = "//Continent[@name='{0}']/Zone/Pet";
        const string ProfilesXPath = "//*[@profile]";

        IPluginLogger _logger;
        XmlDocument _xdoPets;

        bool _isLoaded = false;

        public bool IsLoaded
        {
            get { return _isLoaded; }
        }

        public PetDatabase(IPluginLogger logger)
        {
            _logger = logger;
        }

        public void Load()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location + @"\" + ContinentsFilename;
            Load(path);
        }

        public void Load(string path)
        {
            _xdoPets = new XmlDocument();
            try
            {
                _xdoPets.Load(path);
                _logger.WriteVerbose("Loaded pet database from " + path);
                _isLoaded = true;
            }
            catch (Exception e)
            {
                _logger.WriteError("Failed to load from " + path + ". " ,e);
            }
        }

                
        public List<PetInfo> GetPetsForZone(Zone zone)
        {
            XmlNodeList nodes = GetPetNodesForZone(zone);
            string defaultProfile = GetDefaultProfileForZone(zone);

            return CreatePets(nodes, defaultProfile);
        }

        private List<PetInfo> CreatePets(XmlNodeList nodes, string defaultProfile)
        {
            List<PetInfo> pets = new List<PetInfo>();
            List<string> petNames = new List<string>();

            foreach (XmlNode node in nodes)
            {
                PetInfo pet = new PetInfo(node, defaultProfile, _logger);
                if (!petNames.Contains(pet.Name))
                {
                    petNames.Add(pet.Name);
                    pets.Add(pet);
                }
            }
            return pets;
        }

        public List<PetInfo> GetPetsForContinent(Zone zone)
        {
            XmlNodeList nodes = GetPetNodesForContinent(zone);
            return CreatePets(nodes, "");
        }

        public List<string> GetAllProfiles()
        {
            XmlNodeList nodes = _xdoPets.SelectNodes(ProfilesXPath);
            List<string> profiles = new List<string>();
            foreach (XmlNode node in nodes)
            {
                profiles.Add(GetProfile(node));
            }
            return profiles;
        }

        public List<Zone> GetZones(Zone zone)
        {
            CheckPetDatabaseHasLoaded();

            string xpath = string.Format(PetZonesXPath, zone.Continent);
            System.Diagnostics.Debug.WriteLine(xpath);

            XmlNodeList nodes = _xdoPets.SelectNodes(xpath);

            List<Zone> zones = new List<Zone>();

            foreach (XmlNode node in nodes)
            {
                zones.Add(new Zone(zone.Continent, GetAttributValue(node, "name")));
            }
            return zones;
        }

        public string GetDefaultProfileForZone(Zone zone)
        {
            CheckPetDatabaseHasLoaded();

            string xpath = string.Format(PetZoneXPath, zone.Continent, zone.Name);
            System.Diagnostics.Debug.WriteLine(xpath);

            XmlNode node = _xdoPets.SelectSingleNode(xpath);
            if (node == null)
            {
                _logger.WriteVerbose("No zone node found for " + zone);
                return "";
            }

            return GetProfile(node);
        }

        private string GetProfile(XmlNode node)
        {
            return GetAttributValue(node, "profile");
        }

        private string GetAttributValue(XmlNode node, string name)
        {
            bool found = false;
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name == name) { found = true; }
            }
            if (!found)
            {
                _logger.WriteVerbose("Attribute " + name + " not found in node " + node.OuterXml);
            }

            try
            {
                return node.Attributes[name].Value;
            }
            catch (Exception ex)
            {
                if (node == null) { _logger.WriteError("node null when trying to get attribute: " + name, ex); throw; }
                else
                {
                    _logger.WriteError(node.Name + " node attribute '" + name + "'. ", ex);
                }
                return "";
            }
        }


        private XmlNodeList GetPetNodesForZone(Zone zone)
        {
            CheckPetDatabaseHasLoaded();

            string xpath = string.Format(PetZonePetsXPath, zone.Continent, zone.Name);
            System.Diagnostics.Debug.WriteLine(xpath);

            XmlNodeList nodes = _xdoPets.SelectNodes(xpath);
            return nodes;
        }

        private XmlNodeList GetPetNodesForContinent(Zone zone)
        {
            CheckPetDatabaseHasLoaded();

            string xpath = string.Format(PetContinentPetsXPath, zone.Continent);
            System.Diagnostics.Debug.WriteLine(xpath);

            XmlNodeList nodes = _xdoPets.SelectNodes(xpath);
            return nodes;
        }

        private bool CheckPetDatabaseHasLoaded()
        {
            if (_xdoPets == null)
            {
                _logger.WriteError(new Exception("Pet database has not been loaded."));
                return false;
            }
            return true;
        }
    }
}
