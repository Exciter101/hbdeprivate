using System.Collections.Generic;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals.WoWObjects;

// ReSharper disable CheckNamespace
namespace Bots.DungeonBuddy.DungeonScripts.WarlordsOfDraenor
// ReSharper restore CheckNamespace
{
	#region Normal Difficulty

    public class Auchindoun : Dungeon
	{
		#region Overrides of Dungeon

	
		public override uint DungeonId
		{
			get { return 820; }
		}

        public override WoWPoint Entrance
        {
            get { return new WoWPoint(1491.402, 3071.209, 110.1424); }
        }

        public override WoWPoint ExitLocation
        {
            get { return new WoWPoint(1491.654, 2952.643, 35.23913); }
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

    public class AuchindounHeroic : Auchindoun
	{
		#region Overrides of Dungeon

		public override uint DungeonId
		{
			get { return 845; }
		}

		#endregion
	}

	#endregion
}