using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

// HonorBuddy Includes
using Styx.Helpers;
using Styx;
using Styx.Common;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.WoWInternals.WoWObjects;

namespace MasterBaiter
{
    public class MasterBaiter_Settings : Settings
    {
        //public static  MasterBaiter_Settings Preferences = new MasterBaiter_Settings();
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public MasterBaiter_Settings()
            : base(Utilities.AssemblyDirectory + @"\Settings\" + Me.RealmName + @"\" + Me.Name + @"\MasterBaiter_Settings.xml")
        {
        }

        [Flags]
        public enum ChangeType
        {
            None = 0,
            Rotate = 1,
            Baits = 2,
            Debug = 4
        }

        public void Copy(MasterBaiter_Settings MB_Settings)
        {
            useAbyssalGulperEel = MB_Settings.useAbyssalGulperEel;
            useBlackwaterWhiptail = MB_Settings.useBlackwaterWhiptail;
            useBlindLakeSturgeon = MB_Settings.useBlindLakeSturgeon;
            useFatSleeper = MB_Settings.useFatSleeper;
            useFireAmmonite = MB_Settings.useFireAmmonite;
            useJawlessSkulker = MB_Settings.useJawlessSkulker;
            useSeaScorpion = MB_Settings.useSeaScorpion;
            RotateBaits = MB_Settings.RotateBaits;

            DebugLog = MB_Settings.DebugLog;
        }

        public ChangeType WhatChanged(MasterBaiter_Settings MB_Settings)
        {
            ChangeType retChanges = ChangeType.None;

            if (useAbyssalGulperEel != MB_Settings.useAbyssalGulperEel ||
                useBlackwaterWhiptail != MB_Settings.useBlackwaterWhiptail ||
                useBlindLakeSturgeon != MB_Settings.useBlindLakeSturgeon ||
                useFatSleeper != MB_Settings.useFatSleeper ||
                useFireAmmonite != MB_Settings.useFireAmmonite ||
                useJawlessSkulker != MB_Settings.useJawlessSkulker ||
                useSeaScorpion != MB_Settings.useSeaScorpion)
            {
                retChanges |= ChangeType.Baits;
            }

            if (RotateBaits != MB_Settings.RotateBaits)
            {
                retChanges |= ChangeType.Rotate;
            }

            if (DebugLog != MB_Settings.DebugLog)
            {
                retChanges |= ChangeType.Debug;
            }

            return retChanges;
        }

        public void DumpSettings(string Message = null)
        {
            MyLogging("MasterBaiter: Settings " + Message);
            MyLogging("------------------------------------------");
            MyLogging(string.Format("   Use Abyssal Gulper Eel Bait  = {0}", useAbyssalGulperEel));
            MyLogging(string.Format("   Use Blackwater Whiptail Bait = {0}", useBlackwaterWhiptail));
            MyLogging(string.Format("   Use Blind Lake Sturgeon Bait = {0}", useBlindLakeSturgeon));
            MyLogging(string.Format("   Use Fat Sleeper Bait         = {0}", useFatSleeper));
            MyLogging(string.Format("   Use Fire Ammonite Bait       = {0}", useFireAmmonite));
            MyLogging(string.Format("   Use Jawless Skulker Bait     = {0}", useJawlessSkulker));
            MyLogging(string.Format("   Use Sea Scorpion Bait        = {0}", useSeaScorpion));
            MyLogging(string.Format("   RotateBaits                  = {0}", RotateBaits));
            MyLogging(string.Format("   DebugLog                     = {0}", DebugLog));
            MyLogging("------------------------------------------");

            return;
        }

        private static void MyLogging(string Message)
        {
            Logging.Write(LogLevel.Normal, Colors.Orange, Message);
        }


        [Setting]
        [DefaultValue(false)]
        [Category("Baits")]
        [DisplayName("Abyssal Gulper Eel - Spires of Arak")]
        [Description("Use Abyssal Gulper Eel Bait")]
        public bool useAbyssalGulperEel { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Baits")]
        [DisplayName("Blackwater Whiptail - Talador")]
        [Description("Use Blackwater Whiptail Bait")]
        public bool useBlackwaterWhiptail { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Baits")]
        [DisplayName("Blind Lake Sturgeon - Shadowmoon Valley")]
        [Description("Use Blind Lake Sturgeon Bait")]
        public bool useBlindLakeSturgeon { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Baits")]
        [DisplayName("Fat Sleeper - Nagrand")]
        [Description("Use Fat Sleeper Bait")]
        public bool useFatSleeper { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Baits")]
        [DisplayName("Fire Ammonite - Frostfire Ridge")]
        [Description("Use Fire Ammonite Bait")]
        public bool useFireAmmonite { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Baits")]
        [DisplayName("Jawless Skulker - Gorgrond")]
        [Description("Use Jawless Skulker Bait")]
        public bool useJawlessSkulker { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Baits")]
        [DisplayName("Sea Scorpion - Dreaenor Coast")]
        [Description("Use Sea Scorpion Bait")]
        public bool useSeaScorpion { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("AutoRotate")]
        [DisplayName("Rotate Baits")]
        [Description("Rotate the baits, using all enabled baits")]
        public bool RotateBaits { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Debug")]
        [DisplayName("Debug Logging")]
        [Description("Turn on for more detailed logging")]
        public bool DebugLog { get; set; }

    }
}
