using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.CommonBot.Profiles;
using System.Reflection;
using System.IO;

namespace BattlePetCollector
{
    public class ProfileLoader : IProfileLoader
    {
        string _currentProfile = string.Empty;
        DateTime _profileLoadTime = DateTime.MaxValue;

        public DateTime ProfileLoadTime
        {
            get 
            {
                if (_profileLoadTime == DateTime.MaxValue) { return DateTime.Now; }
                return _profileLoadTime; 
            }
        } 

        IPluginLogger _logger;
        string _pluginsFolder;

        public ProfileLoader(IPluginLogger logger,string pluginsFolder)
        {
            _logger = logger;
            _pluginsFolder = pluginsFolder;
        }

        public bool IsOnProfile(string profile)
        {
            return !string.IsNullOrEmpty(_currentProfile) && _currentProfile == profile;
        }

        public bool SwitchToProfile(string profile)
        {
            try
            {
                _logger.Write("Loading profile: " + profile);
                ProfileManager.LoadNew(ProfileFilename(profile));
                _currentProfile = profile;
                _profileLoadTime = DateTime.Now;
                return true;
            }
            catch (Exception e)
            {
                _logger.WriteError("Failed to load profile " + ProfileFilename(profile) + ". " , e);
                return false;
            }
        }

        private string ProfileFilename(string profileName)
        {
            string fullProfileName = _pluginsFolder + profileName;
            return fullProfileName;
        }

        public void CheckFileExists(string profile)
        {
            try
            {
                File.ReadAllLines(ProfileFilename(profile));
                _logger.WriteVerbose("Read: " + ProfileFilename(profile) + ".");
            }
            catch (Exception ex)
            {
                _logger.WriteError("Warning: Failed to read: " + ProfileFilename(profile) + ". " , ex);
            }
        }
    }
}
