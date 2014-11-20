using System.Collections.Generic;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals.WoWObjects;

// ReSharper disable CheckNamespace
namespace Bots.DungeonBuddy.DungeonScripts.WarlordsOfDraenor
// ReSharper restore CheckNamespace
{
	#region Normal Difficulty

	public class Skyreach : Dungeon
	{
		#region Overrides of Dungeon

	
		public override uint DungeonId
		{
			get { return 779; }
		}

	    public override WoWPoint Entrance
	    {
            get { return new WoWPoint(28.13581, 2526.396, 103.606); }
	    }

        // Must talk to Shadow-Sage Iskar (Id: 82376) and click the confirmation popup to exit. 
        // Call lua func SelectGossipOption(1) to confirm
	    public override WoWPoint ExitLocation
	    {
            get { return new WoWPoint(1235.361, 1734.913, 177.1658); }
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

    public class SkyreachHeroic : Skyreach
	{
		#region Overrides of Dungeon

		public override uint DungeonId
		{
			get { return 780; }
		}

		#endregion
	}

	#endregion
}