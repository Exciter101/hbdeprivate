using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

// HonorBuddy Includes
using Styx;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using Styx.Common;
using Styx.Common.Helpers;


namespace MasterBaiter
{
    public class MasterBaiter : HBPlugin
    {
        #region Globals

        public override string Name { get { return "MasterBaiter - Bait your pole!"; } }
        private static string LogName { get { return "MasterBaiter"; } }
        public override string Author { get { return "FaceRollFTW"; } }
        public override Version Version { get { return _version; } }
        private readonly Version _version = new Version(6, 1, 0, 0);
        private static LocalPlayer Me { get { return StyxWoW.Me; } }


        #region Config Form

        public override string ButtonText { get { return "Bait Me"; } }
        public override bool WantButton { get { return true; } }

        public static MasterBaiter_Settings Master_Settings = new MasterBaiter_Settings();

        public override void OnButtonPress()
        {
            MasterBaiter_Settings.ChangeType Changed;

            MasterBaiter_Settings tmpMB_Settings = new MasterBaiter_Settings();

            //Master_Settings.DumpSettings("OnButtonPress: Dump Master Settings");

            tmpMB_Settings.Copy(Master_Settings);

            //tmpMB_Settings.DumpSettings("OnButtonPress: Dump Tmp Settings");

            MB_Config ConfigForm = new MB_Config(tmpMB_Settings);
            ConfigForm.ShowDialog();

            MyDebugging("OnButtonPress After ShowDialog");

            //tmpMB_Settings.DumpSettings("OnButtonPress: Dump Tmp Settings");

            Changed = tmpMB_Settings.WhatChanged(Master_Settings);

            if (Changed == MasterBaiter_Settings.ChangeType.None)
            {
                MyDebugging("OnButtonPress: No Settings Change");
                return;
            }

            //
            // Settings changes so copy the settings to the Master.
            //
            Master_Settings.Copy(tmpMB_Settings);
            Master_Settings.Save();
            Master_Settings.DumpSettings();

            if ((Changed & MasterBaiter_Settings.ChangeType.Baits) == MasterBaiter_Settings.ChangeType.Baits)
            {
                MyDebugging("OnButtonPress: Baits Changed");
                BuildBaitsToUse();
                CurrentBait = GetNextUsableBait(false);  // Set the current bait to use.
            }
            
            if ((Changed & MasterBaiter_Settings.ChangeType.Debug) == MasterBaiter_Settings.ChangeType.Debug)
            {
                MyDebugging("OnButtonPress: Debugging Changed");
            }

            if ((Changed & MasterBaiter_Settings.ChangeType.Rotate) == MasterBaiter_Settings.ChangeType.Rotate)
            {
                MyDebugging("OnButtonPress: Rotate Baits Changed");
            }
        }

        #endregion


        private static Bait CurrentBait = null;

        private static Queue<Bait> Baits;

        private TimeSpan BaitCheckTime = new TimeSpan(0, 0, 30);  // 30 seconds
        private TimeSpan ReBaitTime = new TimeSpan(0, 10, 5);  // 10 minutes, 5 seconds - Changed to 10 minutes in 6.1
        private MyWaitTimer ReBait_Timer = null;
        private MyWaitTimer CheckForBaits_Timer = null;   // Used to throttle the check for having a bait in our bag.

        #endregion


        public override void OnEnable()
        {
            base.OnEnable();

            MyDebugging("OnEnable: Settings file name = " + Master_Settings.SettingsPath);

            BuildBaitsToUse();

            CurrentBait = GetNextUsableBait(false);  // Set the current bait to use.

            Logging.Write(LogLevel.Normal, Colors.Orange, "[{0} {1}]: Enabled", LogName, Version);

        }

        public override void OnDisable()
        {
            base.OnDisable();

            Baits = null;
            ReBait_Timer = null;
            CheckForBaits_Timer = null;

            Logging.Write(LogLevel.Normal, Colors.Orange, "[{0} {1}]: Disabled", LogName, Version);
        }


        //
        // Executed every time the Plugin is pulsed.
        //
        public override void Pulse()
        {

            if (Me.Combat || !Me.IsAlive || Me.IsGhost || Me.IsOnTransport || Me.OnTaxi || Me.Stunned || Me.Mounted)
            {
                MyDebugging("Pulse: Combat Alive Ghost Transport Taxi Stunned Flying Mounted");

                return;
            }

            if (CurrentBait == null)
            {
                MyDebugging("Pulse: CurrentBait = null");

                if (CheckForBaits_Timer == null)
                {  
                    // This should never happen but you never say never. Or the addon was enabled with no baits set to true.
                    MyDebugging("Pulse: CurrentBait = null & CheckForBaits_Timer = null - You may not have any baits enabled in the settings");
                    return;
                }

                if (CheckForBaits_Timer.IsFinished == true)
                {
                    MyDebugging("Pulse: CheckForBaits_Timer expired");
                    CurrentBait = GetNextUsableBait(false);
                }

                return;
            }


            //
            // Check the rebait timer is expired
            //
            if (ReBait_Timer != null)
            {
                if (ReBait_Timer.IsFinished == false)
                {
                    //MyDebugging(string.Format("ReBait_Timer IsFinished is false TimeLeft value is {0}", ReBait_Timer.TimeLeft.ToString()));
                    MyDebugging("Pulse: ReBait_Timer waiting");
                    return;
                }
                else
                {
                    MyDebugging(string.Format("Pulse: ReBait_Timer IsFinished is true TimeLeftvalue is {0}", ReBait_Timer.TimeLeft.ToString()));
                }

            }
            else
            {
                MyDebugging("Pulse: ReBait_Timer is null");
            }

            //
            // Check to see if we have a fishing pole equiped and we are near water.
            //
            if (IsFishingPoleEquipped() == false || IsNearWater() == false || IsFishing() == false)
            {
                // we dont so return.
                MyDebugging(string.Format("Pulse: IsFishingPoleEquipped = {0} IsNearWater = {1} IsFishing = {2}",
                            IsFishingPoleEquipped().ToString(), IsNearWater().ToString(), IsFishing().ToString()));
                return;
            }


            if (!Me.Combat) // Just in case
            {
                if (!Me.HasAura(CurrentBait.AuraId) && IsNearWater() == true)
                {
                    MyDebugging("Pulse: CurrentBait Aura not active and we are near water");

                    if (Master_Settings.RotateBaits == true || CurrentBait.IsInBags == false)
                    {
                        MyDebugging(string.Format("Pulse: Switching baits, RotateBaits = {0} CurrentBait {1} IsInBags = {2}",
                                                  Master_Settings.RotateBaits.ToString(), CurrentBait.Name, CurrentBait.IsInBags.ToString()));

                        if ((CurrentBait = GetNextUsableBait(true)) == null)
                        {
                            MyDebugging("Pulse: Switching baits, no selected bait in our bag.");
                            return;
                        }
                    }

                    useItem(CurrentBait.Item);
                }
            }
        }

        private void useItem(WoWItem item)
        {
            MyLogging(string.Format("Using {0}", item.Name));

            item.UseContainerItem();

            ReBait_Timer = new MyWaitTimer(ReBaitTime);

            MyDebugging(string.Format("Use Item: ReBaitTimer value is {0}", ReBait_Timer.TimeLeft.ToString()));
 
        }

        private void BuildBaitsToUse()
        {
            MyDebugging("BuildBaitsToUse Called");

            //
            // Build the list of Baits to be used.
            //
            Baits = new Queue<Bait>();

            //<WoWItem Name="Abyssal Gulper Eel Bait" Entry="110293" />
            //<Aura Name="Abyssal Gulper Eel Bait" SpellId="158038" />
            if (Master_Settings.useAbyssalGulperEel)
            {
                MyDebugging("BuildBaitsToUse: Adding Abyssal Gulper Eel Bait to Baits");
                Baits.Enqueue(new Bait("Abyssal Gulper Eel Bait", "Abyssal Gulper Eel Bait", 110293, 158038, 6722, "Spires of Arak"));
            }

            //<WoWItem Name="Blackwater Whiptail Bait" Entry="110294" />
            //<Aura Name="Blackwater Whiptail Bait" SpellId="158039" " />
            if (Master_Settings.useBlackwaterWhiptail)
            {
                MyDebugging("BuildBaitsToUse: Adding Blackwater Whiptail Bait to Baits");
                Baits.Enqueue(new Bait("Blackwater Whiptail Bait", "Blackwater Whiptail Bait", 110294, 158039, 6662, "Talador"));
            }

            //<WoWItem Name="Blind Lake Sturgeon Bait" Entry="110290" />
            //<Aura Name="Blind Lake Sturgeon Bait" SpellId="158035" />
            if (Master_Settings.useBlindLakeSturgeon)
            {
                MyDebugging("BuildBaitsToUse: Adding Blind Lake Sturgeon Bait to Baits");
                Baits.Enqueue(new Bait("Blind Lake Sturgeon Bait", "Blind Lake Sturgeon Bait", 110290, 158035, 6719, "Shadowmoon Valley"));
            }

            //<WoWItem Name="Fat Sleeper Bait" Entry="110289" />
            //<Aura Name="Fat Sleeper Bait" SpellId="158034" />
            if (Master_Settings.useFatSleeper)
            {
                MyDebugging("BuildBaitsToUse: Adding Fat Sleeper Bait to Baits");
                Baits.Enqueue(new Bait("Fat Sleeper Bait", "Fat Sleeper Bait", 110289, 158034, 6755, "Nagrand"));
            }

            //<WoWItem Name="Fire Ammonite Bait" Entry="110291" />
            //<Aura Name="Fire Ammonite Bait" SpellId="158036"  />
            if (Master_Settings.useFireAmmonite)
            {
                MyDebugging("BuildBaitsToUse: Adding Fire Ammonite Bait to Baits");
                Baits.Enqueue(new Bait("Fire Ammonite Bait", "Fire Ammonite Bait", 110291, 158036, 6720, "Frostfire Ridge"));
            }

            //<WoWItem Name="Jawless Skulker Bait" Entry="110274" />
            //<Aura Name="Jawless Skulker Bait" SpellId="158031" />
            if (Master_Settings.useJawlessSkulker)
            {
                MyDebugging("BuildBaitsToUse: Adding Jawless Skulker Bait to Baits");
                Baits.Enqueue(new Bait("Jawless Skulker Bait", "Jawless Skulker Bait", 110274, 158031, 6721, "Gorgrond"));
            }

            //<WoWItem Name="Sea Scorpion Bait" Entry="110292" />
            //<Aura Name="Sea Scorpion Bait" SpellId="158037" />
            if (Master_Settings.useSeaScorpion)
            {
                MyDebugging("BuildBaitsToUse: Adding Sea Scorpion Bait to Baits");
                Baits.Enqueue(new Bait("Sea Scorpion Bait", "Sea Scorpion Bait", 110292, 158037, 9999, "Draenor Coast"));
            }

            if (Baits.Count == 0)
            {
                MyLogging("No Baits Enabled");
                Master_Settings.DumpSettings("No Baits Enabled");
            }

            return;
        }

        private Bait GetNextUsableBait(bool SwitchBaits)
        {
            Bait retBait = null;
            Bait firstBait = null;

            MyDebugging("GetNextUsableBait: Baits.Count = " + Baits.Count.ToString());

            if (Baits.Count == 0)
            {
                return null;
            }

            for (int iCntr = 0; iCntr < Baits.Count; iCntr++)
            {
                MyDebugging("GetNextUsableBait: Baits = " + Baits.ElementAt(iCntr).Name);
            }

            retBait = firstBait = Baits.Peek();

            if (SwitchBaits == true)
            {
                Bait tmpBait = Baits.Dequeue();  // Get the Current.
                Baits.Enqueue(tmpBait);
                retBait = Baits.Peek();  // Get the next.
            }

            // See if we have this bait in our bags to use.
            do
            {
                if (retBait.IsInBags)
                {
                    MyDebugging("GetNextUsableBait: Returning = " + retBait.Name);
                    return retBait;
                }
                else
                {
                    Bait tmpBait = Baits.Dequeue();  // Get the Current.
                    Baits.Enqueue(tmpBait);
                    retBait = Baits.Peek();  // Get the next.
                }
            } while (firstBait != retBait);

            //
            // Just in case the first bait in the queue is the only one in your bags and SwitchBaits is set to true.
            //
            if (retBait.IsInBags)
            {
                MyDebugging("GetNextUsableBait: Returning = " + retBait.Name);
                return retBait;
            }


            MyLogging("No selected baits are in your bags.");

            CheckForBaits_Timer = new MyWaitTimer(BaitCheckTime);

            return null;
        }

        private const int FishingAuraId = 131490;

        private bool IsFishing()
        {
            //
            // Check to see if we are actively fishing
            //
            return Me.HasAura(FishingAuraId);
        }


        private bool IsFishingPoleEquipped()
        {
            //
            // Check to see if we have a fishing pole equipped.
            //
            WoWItem mainHand = StyxWoW.Me.Inventory.Equipped.MainHand;
            if (mainHand == null || mainHand.ItemInfo.WeaponClass != WoWItemWeaponClass.FishingPole)
            {
                // we don't so return.
                return false;
            }

            return true;
        }

        private bool IsNearWater()
        {
            //
            // This code was taken from AutoAngler2
            // Thank you HighVoltz
            //
            const float PIx2 = 3.14159f * 2f;
            const int TraceStep = 20;

            WoWPoint playerLoc = Me.Location;
            var tracelines = new WorldLine[TraceStep * 3];
            bool[] tracelineWaterVals, traceLineTerrainVals;
            WoWPoint[] waterHitPoints, terrainHitpoints;

            for (int i = 0; i < TraceStep; i++)
            {
                // scans 10,15 and 20 yards from player for water at every 18 degress 
                for (int n = 0; n < 3; n++)
                {
                    WoWPoint p = (playerLoc.RayCast((i * PIx2) / TraceStep, 10 + (n * 5)));
                    WoWPoint highPoint = p;
                    highPoint.Z += 5;
                    WoWPoint lowPoint = p;
                    lowPoint.Z -= 55;
                    tracelines[(i * 3) + n].Start = highPoint;
                    tracelines[(i * 3) + n].End = lowPoint;
                }
            }

            GameWorld.MassTraceLine(tracelines, TraceLineHitFlags.LiquidAll, out tracelineWaterVals, out waterHitPoints);

            GameWorld.MassTraceLine(tracelines, TraceLineHitFlags.Collision, out traceLineTerrainVals, out terrainHitpoints);

            for (int i = 0; i < TraceStep; i++)
            {
                for (int n = 0; n < 3; n++)
                {
                    var idx = i * 3 + n;
                    if (tracelineWaterVals[idx]
                        && (!traceLineTerrainVals[idx] || terrainHitpoints[idx].Z < waterHitPoints[idx].Z))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void MyLogging(string Message)
        {
            Logging.Write(LogLevel.Normal, Colors.Orange, "{0}: {1}", LogName, Message);
        }

        private static void MyDebugging(string Message)
        {
            if (Master_Settings.DebugLog == true)
                Logging.Write(LogLevel.Normal, Colors.Aqua, "{0} Debug: {1}", LogName, Message);
        }
    }

    public class Bait
    {
        private string _Name;
        private string _Aura;
        private int _EntryId;
        private int _AuraId;
        private uint _ZoneId;
        private string _ZoneName;
        private WoWItem _Item = null;

        public string Name { get { return _Name; } }
        public string Aura { get { return _Aura; } }
        public int EntryId { get { return _EntryId; } }
        public int AuraId { get { return _AuraId; } }
        public uint ZoneId { get { return _ZoneId; } }
        public string ZoneName { get { return _ZoneName; } }
        public WoWItem Item { get { return _Item; } }

        public bool IsInBags
        {
            get
            {
                var baitItem = StyxWoW.Me.BagItems.FirstOrDefault(r => r.Entry == _EntryId);
                if (baitItem != null)
                {
                    _Item = baitItem;
                    return true;
                }
                else
                {
                    _Item = null;
                    return false;
                }
            }
        }

        public Bait()
        {
        }

        public Bait(string Name, string Aura)
        {
            _Name = Name;
            _Aura = Aura;
        }

        public Bait(string Name, string Aura, int EntryId)
        {
            _Name = Name;
            _Aura = Aura;
            _EntryId = EntryId;
        }

        public Bait(string Name, string Aura, int EntryId, int AuraId)
        {
            _Name = Name;
            _Aura = Aura;
            _EntryId = EntryId;
            _AuraId = AuraId;
        }

        public Bait(string Name, string Aura, int EntryId, int AuraId, uint ZoneId, string ZoneName)
        {
            _Name = Name;
            _Aura = Aura;
            _EntryId = EntryId;
            _AuraId = AuraId;
            _ZoneId = ZoneId;
            _ZoneName = ZoneName;
        }

        public override string ToString()
        {
 	         return _Name;
        }
    };

}
