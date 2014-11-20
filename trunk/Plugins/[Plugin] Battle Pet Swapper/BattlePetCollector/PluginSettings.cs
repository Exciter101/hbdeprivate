using System;
using System.ComponentModel;
using System.Text;
using Styx;
using Styx.Helpers;

namespace BattlePetCollector
{
    [DefaultPropertyAttribute("Mode")]
    public class PluginSettings : Settings, IPluginProperties, IPluginSettings
    {
        IPluginLogger _logger;

        const string NAMESPACE = "BattlePetCollector";

        public PluginSettings(IPluginLogger logger,string settingsDirectory)
            : base(settingsDirectory + "\\" + NAMESPACE + "_" + (StyxWoW.Me != null ? StyxWoW.Me.Name : "") + ".xml")
        {
            _logger = logger;
            _logger.Write("Loading Settings file:" + settingsDirectory + "\\"+NAMESPACE+"_*****.xml");
            
            Load();

            ConvertSettingsToProperties();
        }

        #region Convert settings to properties and back again

        public void ConvertSettingsToProperties()
        {
            MaxMinsPerZone = ToIntValue(MaxMinsPerZoneSetting, 1);
            BlackListUnrequiredPets = ToBoolValue(BlackListUnrequiredPetsSetting, true);
            ForfeitWhenOwned = ToBoolValue(ForfeitWhenOwnedSetting, true);
            ForfeitWhenNotBlue = ToBoolValue(ForfeitWhenNotBlueSetting, false);
            StopWhenAllPetsCaught = ToBoolValue(StopWhenAllPetsCaughtSetting, true);
            OnlyConsiderRarePetsAsCaught = ToBoolValue(OnlyConsiderRarePetsAsCaughtSetting, false);
        }

        public void ConvertsPropertiesToSettings()
        {
            MaxMinsPerZoneSetting = MaxMinsPerZone.ToString();
            BlackListUnrequiredPetsSetting = BlackListUnrequiredPets.ToString();
            ForfeitWhenOwnedSetting = ForfeitWhenOwned.ToString();
            ForfeitWhenNotBlueSetting = ForfeitWhenNotBlue.ToString();
            StopWhenAllPetsCaughtSetting = StopWhenAllPetsCaught.ToString();
            OnlyConsiderRarePetsAsCaughtSetting = OnlyConsiderRarePetsAsCaught.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb=new StringBuilder();
            sb.Append("Settings:");
            if (BlackListUnrequiredPets) { sb.Append(", BlackList unrequired pets=" + BlackListUnrequiredPets.ToString()); }
            sb.Append(", Max mins per zone=" + MaxMinsPerZoneSetting.ToString());
            return sb.ToString();
        }

        private int ToIntValue(string s, int defaultValue)
        {
            if (string.IsNullOrEmpty(s)) { return defaultValue; }
            int value = 0;
            if (!int.TryParse(s, out value)) { return defaultValue; }
            return value;
        }

        private bool ToBoolValue(string s, bool defaultValue)
        {
            if (string.IsNullOrEmpty(s)) { return defaultValue; }
            bool value = false;
            if (!bool.TryParse(s, out value)) { return defaultValue; }
            return value;
        }

        #endregion

        #region Validation

        private static void ValidateIntRange(int value, int min, int max)
        {
            if (value < min || value > max) throw new ArgumentException(string.Format("Value must be between {0} and {1}", min, max));
        }

        #endregion

        #region Property -MaxMinsPerZone

        [Setting, Styx.Helpers.DefaultValue("40")]
        [Browsable(false)]
        public string MaxMinsPerZoneSetting { get; set; }

        int _maxMinsPerZone = 40;
        [CategoryAttribute("Zone")]
        [DescriptionAttribute("The maximum minutes ti spend in a zone, this is reset if a pet is caught.")]
        [DisplayName("Maximum zone Minutes")]
        public int MaxMinsPerZone
        {
            get { return _maxMinsPerZone; }
            set { ValidateIntRange(value, 1, 999); _maxMinsPerZone = value; }
        }

        #endregion

        #region Property -StopWhenAllPetsCaught

        [Setting, Styx.Helpers.DefaultValue("1")]
        [Browsable(false)]
        public string StopWhenAllPetsCaughtSetting { get; set; }

        [CategoryAttribute("Zone")]
        [DescriptionAttribute("When all pets on the current continent have been caught then stop the bot.")]
        [DisplayName("Stop when no pets to catch")]
        public bool StopWhenAllPetsCaught { get; set; }

        #endregion

        #region Property -OnlyConsiderRarePetsAsCaught

        [Setting, Styx.Helpers.DefaultValue("0")]
        [Browsable(false)]
        public string OnlyConsiderRarePetsAsCaughtSetting { get; set; }

        [CategoryAttribute("Zone")]
        [DescriptionAttribute("If you don't have a rare copy of the pet then it is not considered as having been caught.")]
        [DisplayName("Only consider rare pets as caught.")]
        public bool OnlyConsiderRarePetsAsCaught { get; set; }

        #endregion

        #region Property -BlackListUnrequiredPets

        [Setting, Styx.Helpers.DefaultValue("1")]
        [Browsable(false)]
        public string BlackListUnrequiredPetsSetting { get; set; }

        [CategoryAttribute("Battles")]
        [DescriptionAttribute("Blacklists any pets it sees which are already owned.")]
        [DisplayName("BlackList map pets already owned")]
        public bool BlackListUnrequiredPets { get; set; }

        #endregion

        #region Property -Forfeit battles owned

        [Setting, Styx.Helpers.DefaultValue("1")]
        [Browsable(false)]
        public string ForfeitWhenOwnedSetting { get; set; }

        [CategoryAttribute("Battles")]
        [DescriptionAttribute("Forfeit a match when all pets involved are owned.")]
        [DisplayName("Forfeit when no new pet to trap")]
        public bool ForfeitWhenOwned { get; set; }

        #endregion

        #region Property -Forfeit battles blue

        [Setting, Styx.Helpers.DefaultValue("0")]
        [Browsable(false)]
        public string ForfeitWhenNotBlueSetting { get; set; }

        [CategoryAttribute("Battles")]
        [DescriptionAttribute("Forfeit a match when all no blue pets can be caught.")]
        [DisplayName("Forfeit when no blues to trap")]
        public bool ForfeitWhenNotBlue { get; set; }

        #endregion
    }
}
