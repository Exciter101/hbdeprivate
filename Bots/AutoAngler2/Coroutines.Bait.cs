using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace HighVoltz.AutoAngler
{
	static partial class Coroutines
	{
		private readonly static WaitTimer BaitRecastTimer = WaitTimer.TenSeconds;

        private static readonly Dictionary<uint, string> Baits = new Dictionary<uint, string>();
																/*{
																	{110293, "Abyssal Gulper Eel Bait"},
																	{110291, "Fire Ammonite Bait"},
																	{110290, "Blind Lake Sturgeon Bait"},
																	{110292, "Sea Scorpion Bait"},
																	{110274, "Jawless Skulker Bait"},
																	{110289, "Fat Sleeper Bait"},
																	{110294, "Blackwater Whiptail Bait"},
																	{114628, "Icespine Stinger Bait"},
																	{114874, "Moonshell Claw Bait"},
																};*/
        
        
        
		// does nothing if no Baits are in bag
		public async static Task<bool> ApplyBait()
		{
			if (StyxWoW.Me.IsCasting || IsBaitBuffed)
				return false;

			if (!BaitRecastTimer.IsFinished)
				return false;
			
			BaitRecastTimer.Reset();

            Baits.Clear();

            if (AutoAnglerSettings.Instance.AbyssalGulperEelBait)
                Baits.Add(110293, "Abyssal Gulper Eel Bait");
            if (AutoAnglerSettings.Instance.FireAmmoniteBait)
                Baits.Add(110291, "Fire Ammonite Bait");
            if (AutoAnglerSettings.Instance.BlindLakeSturgeonBait)
                Baits.Add(110290, "Blind Lake Sturgeon Bait");
            if (AutoAnglerSettings.Instance.SeaScorpionBait)
                Baits.Add(110292, "Sea Scorpion Bait");
            if (AutoAnglerSettings.Instance.JawlessSkulkerBait)
                Baits.Add(110274, "Jawless Skulker Bait");
            if (AutoAnglerSettings.Instance.FatSleeperBait)
                Baits.Add(110289, "Fat Sleeper Bait");
            if (AutoAnglerSettings.Instance.BlackwaterWhiptailBait)
                Baits.Add(110294, "Blackwater Whiptail Bait");
            if (AutoAnglerSettings.Instance.IcespineStingerBait)
                Baits.Add(114628, "Icespine Stinger Bait");
            if (AutoAnglerSettings.Instance.MoonshellClawBait)
                Baits.Add(114874, "Moonshell Claw Bait");
                        


			OrderedDictionary BaitsInBag = new OrderedDictionary();

			foreach (var kv in Baits)
			{
				WoWItem BaitInBag = Utility.GetItemInBag(kv.Key);
				if (BaitInBag != null)
				{
                  BaitsInBag.Add(kv.Key, kv.Value);
				}
			}
          
		    if (BaitsInBag.Count > 0) {

			 
			var rnd = new Random();
                        int rndidx = rnd.Next(0, BaitsInBag.Count);

                        uint[] keys = new uint[BaitsInBag.Count];
                        BaitsInBag.Keys.CopyTo(keys, 0);
		
			WoWItem BaitInBag = Utility.GetItemInBag(keys[rndidx]);

            			
			if (BaitInBag != null && BaitInBag.Use())
				{
					AutoAnglerBot.Log("Appling {0}", BaitsInBag[keys[rndidx]]);
					await CommonCoroutines.SleepForLagDuration();
					return true;
				}
			}
			return false;
		}

		public static bool IsBaitBuffed
		{
			get
			{
				foreach (var kv in Baits)
			    {
				  if (Me.HasAura(kv.Value)) { return true; }				
				}
			  return false;	
			}             			
		}
	}
}
