using System.Collections.Generic;
using Bots.DungeonBuddy.Enums;
using Bots.DungeonBuddy.Helpers;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

// ReSharper disable CheckNamespace
namespace Bots.DungeonBuddy.DungeonScripts.WarlordsOfDraenor
// ReSharper restore CheckNamespace
{
	#region Normal Difficulty

	public class TheEverbloom : Dungeon
	{
		#region Overrides of Dungeon

	    public override WoWPoint Entrance
	    {
            get { return new WoWPoint(7111.703, 195.8656, 144.6757); }
	    }

	    public override WoWPoint ExitLocation
	    {
            get { return new WoWPoint(425.5091, 1319.822, 125.0202); }
	    }

	    public override uint DungeonId
		{
			get { return 824; }
		}

		public override void RemoveTargetsFilter(List<WoWObject> units)
		{
			units.RemoveAll(
				ret =>
				{
					return false;
				});
		}

	    public override void IncludeTargetsFilter(List<WoWObject> incomingunits, HashSet<WoWObject> outgoingunits)
	    {
            foreach (var obj in incomingunits)
            {
                var unit = obj as WoWUnit;
                if (unit != null)
                {
                }
            }
	    }

	    public override void WeighTargetsFilter(List<Targeting.TargetPriority> units)
		{
			foreach (var priority in units)
			{
				var unit = priority.Object as WoWUnit;
				if (unit != null)
				{

				}
			}
		}

        public override void OnEnter()
        {
            // Followers will automatically leave when leader does so no need to show more than one popup.
            if (DungeonBuddySettings.Instance.PartyMode != PartyMode.Follower)
            {
                Alert.Show(
                    "Dungeon Not Supported",
                    string.Format(
                        "The {0} dungeon is not supported. If you wish to stay in group and play manually then press 'Cancel'. Otherwise Dungeonbuddy will automatically leave group.",
                        Name),
                    30,
                    true,
                    true,
                    () => Lua.DoString("LeaveParty()"),
                    null,
                    "Leave",
                    "Cancel");
            }
        }

		#endregion
		
		private static LocalPlayer Me
		{
			get { return StyxWoW.Me; }
		}

		#region Root


		#endregion

	}

	#endregion

	#region Heroic Difficulty

    public class TheEverbloomHeroic : TheEverbloom
	{
		#region Overrides of Dungeon

		public override uint DungeonId
		{
			get { return 866; }
		}

		#endregion
	}

	#endregion
}