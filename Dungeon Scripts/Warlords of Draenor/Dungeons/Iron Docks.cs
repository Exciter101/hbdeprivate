using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bots.DungeonBuddy.Attributes;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

// ReSharper disable CheckNamespace
namespace Bots.DungeonBuddy.DungeonScripts.WarlordsOfDraenor
// ReSharper restore CheckNamespace
{
	#region Normal Difficulty

	public class IronDocks : Dungeon
	{
		#region Overrides of Dungeon

	
		public override uint DungeonId
		{
			get { return 821; }
		}

	    public override WoWPoint Entrance
	    {
            get { return new WoWPoint(8849.521, 1352.264, 98.26431); }
	    }

	    public override WoWPoint ExitLocation
	    {
            get { return new WoWPoint(6749.356, -538.567, 4.925448); }
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

	    [EncounterHandler(81305, "Fleshrender Nok'gar")]
	    public Func<WoWUnit, Task<bool>> FleshrenderNokgarEncounter()
	    {
	        return async boss =>
	        {
	            return false;
	        };
	    } 
    }

	#endregion

	#region Heroic Difficulty

    public class IronDocksHeroic : IronDocks
	{
		#region Overrides of Dungeon

		public override uint DungeonId
		{
			get { return 857; }
		}

		#endregion
	}

	#endregion
}