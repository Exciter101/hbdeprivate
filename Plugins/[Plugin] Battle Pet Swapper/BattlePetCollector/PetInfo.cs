using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePetCollector
{
    public class PetInfo
    {
        IPluginLogger _logger;

        public PetInfo(XmlNode node,string defaultProfile,IPluginLogger logger)
        {
            _logger = logger;
            Name = GetAttributValue(node,"name");
            string id = GetAttributValue(node, "id");
            Id = int.Parse(id);

            if (HasAttribute(node,"profile"))
            {
                Profile=GetAttributValue(node,"profile");
            }
            else
            {
                Profile=defaultProfile;
            }
        }

        private bool HasAttribute(XmlNode node, string name)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name == name) { return true; }
            }
            return false;
        }

        private string GetAttributValue(XmlNode node,string name)
        {
            try
            {
                return node.Attributes[name].Value;
            }
            catch (Exception ex)
            {
                _logger.WriteError("Pet node attribute '" + name + "'. " , ex);
                return "";
            }
        }

        public PetInfo(string name,int id,string profile)
        {
            Name = name;
            Id = id;
            Profile = profile;
        }

        public static bool ContainsPetWithId(List<PetInfo> pets, int id)
        {
            foreach (PetInfo pet in pets)
            {
                if (pet.Id == id) { return true; }
            }
            return false;
        }

        public string Name { get; private set; }

        public int Id { get; private set; }

        public static string PetNames(List<PetInfo> pets)
        {
            bool firstPet = true;
            StringBuilder sb = new StringBuilder();
            foreach (PetInfo pet in pets)
            {
                sb.Append((firstPet ? "" : ", ") + (pet.Name+"-"+pet.Id));
                firstPet = false;
            }
            return sb.ToString();
        }

        public string Profile { get; private set; }
    }
}
