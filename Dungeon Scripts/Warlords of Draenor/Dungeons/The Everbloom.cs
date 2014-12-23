using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Navigation;
using Bots.DungeonBuddy.Attributes;
using Bots.DungeonBuddy.Enums;
using Bots.DungeonBuddy.Helpers;
using Buddy.Coroutines;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.CommonBot.POI;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
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
				obj =>
				{
					var unit = obj as WoWUnit;
					if (unit == null)
						return false;

					return false;
				});
		}

	    public override void IncludeTargetsFilter(List<WoWObject> incomingunits, HashSet<WoWObject> outgoingunits)
	    {
		    var isDps = Me.IsDps();
            foreach (var obj in incomingunits)
            {
                var unit = obj as WoWUnit;
                if (unit != null)
                {
					if (isDps && unit.Entry == MobId_Entanglement )
		                outgoingunits.Add(obj);
					else if (unit.Entry == MobId_AqueousGlobule)
						outgoingunits.Add(obj);
                }
            }
	    }

	    public override void WeighTargetsFilter(List<Targeting.TargetPriority> units)
	    {
		    var isDps = Me.IsDps();
		    var isRangedDps = isDps && Me.IsRange();

			foreach (var priority in units)
			{
				var unit = priority.Object as WoWUnit;
				if (unit != null)
				{
					if (unit.Entry == MobId_LifeWardenGola || unit.Entry == MobId_Dulhu || unit.Entry == MobId_EarthshaperTelu)
					{
						if (unit.HasAura("Briarskin"))
						{
							priority.Score += -5000;
						}
						else
						{
							if (unit.Entry == MobId_LifeWardenGola)
								priority.Score += isDps ? 4500 : -4000;
							else if (unit.Entry == MobId_EarthshaperTelu)
								priority.Score += isDps ? 3500 : -4000;
							else if (isDps)
								priority.Score += 4000;
						}
						continue;
					}

					switch (unit.Entry)
					{
						case MobId_VenomCrazedPaleOne:
							if (isDps)
								priority.Score += unit.HasAura("Toxic Blood") ?  5500: 3500;
							break;
						case MobId_GorgedBusters:
							priority.Score += 5000;
							break;
						case MobId_ToxicSpiderling:
						case MobId_AqueousGlobule:
							if (isDps)
								priority.Score += 4500;
							break;
						case MobId_VenomSprayer:
							if (isDps)
								priority.Score += 4000;
							break;
					}

					if (isRangedDps && unit.Entry == MobId_Entanglement)
						priority.Score += 4500;

					// DPS should priorite the adds that spawn during Yalknu encounter
					if (isDps && _yalknuAdds.Contains(unit.Entry))
					{
						priority.Score += 4000;
						continue;
					}

				}
			}
		}

        public override void OnEnter()
        {
			// Because of Wall climbing we will have script leave group when not in a full group of bots.
			var numberOfFollowersInGroup = DungeonBuddySettings.Instance.PartyMembers.Count(s => GroupMember.GroupMembers.Any(g => g.NameEquals(s)));
			// need to include self when comparing to maxInstanceSize
			var inFullGroupOfBots = GroupMember.GroupMembers.Count == 5 && (numberOfFollowersInGroup + 1) == 5;

	        var shouldLeave = DungeonBuddySettings.Instance.PartyMode == PartyMode.Off
							|| (DungeonBuddySettings.Instance.PartyMode == PartyMode.Leader && !inFullGroupOfBots);

			// Followrs will leave if leader leaves.
			if (shouldLeave)
			{

				Alert.Show(
					"Dungeonbuddy: Script only works when in a full group of bots.",
					string.Format(
						"The {0} dungeon cannot be AFKed while not in a full group of your own bots because of wall climbing ATM. If you wish to stay in group and play manually then press 'Cancel'. Otherwise Dungeonbuddy will automatically leave group.",
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

		public override async Task<bool> HandleMovement(WoWPoint location)
		{
			// move through portal to final boss.
			if (AtYalnuEncounter(location) && !AtYalnuEncounter(Me.Location))
				return (await CommonCoroutines.MoveTo(_portalToValnuLoc)).IsSuccessful();
			return false;
		}

		#endregion
		
		private static LocalPlayer Me
		{
			get { return StyxWoW.Me; }
		}

		#region Root


		#endregion

		// guide http://www.wowhead.com/guide=2665/everbloom-dungeon-strategy-guide#witherbark
		#region Witherbark

		#region Trash

		private const uint MobId_Dreadpetal = 81864;
		private const uint MobId_Gnarlroot = 81984;

		private const uint AreaTriggerId_LivingLeaves = 7607;

		[EncounterHandler((int) MobId_Dreadpetal, "Dreadpetal")]
		public Func<WoWUnit, Task<bool>> DreadpetalEncounter()
		{
			return async boss => Me.PartyMembers.Any(p => p.HasAura("Dread", aura => aura.StackCount >= 3))
					&& await ScriptHelpers.DispelGroup("Dreadpetal Toxin", ScriptHelpers.PartyDispelType.Poison);
		}

		[EncounterHandler((int)MobId_Gnarlroot, "Gnarlroot")]
		public Func<WoWUnit, Task<bool>> GnarlrootEncounter()
		{
			AddAvoidObject(4, AreaTriggerId_LivingLeaves);
			return async boss => false;
		}

		#endregion

		private const int SpellId_ParchedGasp = 164357;

		private const uint MobId_Witherbark = 81522;
		private const uint MobId_AqueousGlobule = 81638;
		private const uint AreaTriggerId_NoxiousVines = 7200;

		[EncounterHandler((int)MobId_Witherbark, "Witherbark")]
		public Func<WoWUnit, Task<bool>> WitherbarkEncounter()
		{
			var tankLoc = new WoWPoint(417.7986, 1615.377, 89.29382);

			var uncheckedGrowthDropStart = new WoWPoint(455.7579, 1590.944, 86.28169);
			var uncheckedGrowthDropEnd = new WoWPoint(401.3669, 1572.452, 87.6643);

			// Left by Unchecked Growth
			AddAvoidObject(5, AreaTriggerId_NoxiousVines);

			// Deadly frontal 60deg cone AOE attack
			AddAvoidObject(
				ctx => !Me.IsTank(),
				12,
				o => o.Entry == MobId_Witherbark && o.ToUnit().CastingSpellId == SpellId_ParchedGasp,
				o => o.Location.RayCast(o.Rotation, 10));


			return async boss =>
			{
				if (Me.HasAura("Noxious Vines"))
				{
					var stayAtLoc = Me.Location.GetNearestPointOnSegment(uncheckedGrowthDropStart, uncheckedGrowthDropEnd);
					return await ScriptHelpers.StayAtLocationWhile(() => Me.HasAura("Noxious Vines"), stayAtLoc, "Unchecked Growth dropoff");
				}

				if (await ScriptHelpers.TankUnitAtLocation(tankLoc, 15))
					return true;

				return false;
			};
		}

		#endregion

		// guide http://www.wowhead.com/guide=2665/everbloom-dungeon-strategy-guide#ancient-protectors
		#region Ancient Protectors

		#region Trash

		private const int SpellId_TripleAttack = 169418;
		private const int SpellId_NoxiousEruption_Trash = 169445;

		private const uint MobId_TwistedAbomination = 84767;

		[EncounterHandler((int)MobId_TwistedAbomination, "Twisted Abomination")]
		public Func<WoWUnit, Task<bool>> TwistedAbominationEncounter()
		{
			AddAvoidObject(
				ctx => !Me.IsTank(),
				7,
				o => o.Entry == MobId_TwistedAbomination && o.ToUnit().CastingSpellId == SpellId_TripleAttack,
				o => o.Location.RayCast(o.Rotation, 6));

			AddAvoidObject(8, o => o.Entry == MobId_TwistedAbomination && o.ToUnit().CastingSpellId == SpellId_NoxiousEruption_Trash);

			return async boss => false;
		}

		#endregion

		private const int SpellId_NoxiousEruption = 175997;
		private const int SpellId_Slash = 168383;
		private const int SpellId_RevitalizingWaters = 168082;
		private const int SpellId_RapidTides = 168105;
		private const int SpellId_BramblePatch = 167966;
		private const int SpellId_Briarskin = 168041;

		private const uint MobId_Dulhu = 83894;
		private const uint MobId_LifeWardenGola = 83892;
		private const uint MobId_EarthshaperTelu = 83893;

		private const uint AreaTriggerId_BramblePatch = 7499;

		private WoWUnit _gola, _telu;

		[EncounterHandler((int) MobId_Dulhu, "Dulhu")]
		public Func<WoWUnit, Task<bool>> DulhuEncounter()
		{
			// Noxious Eruption - 10yd PBAOE centered on boss
			AddAvoidObject(
				ctx => true, 
				10, 
				o => o.Entry == MobId_Dulhu && o.ToUnit().CastingSpellId == SpellId_NoxiousEruption);

			// Slash - Ability does damage to players in front of boss within 8 yds
			AddAvoidObject(
				ctx => true,
				9,
				o => o.Entry == MobId_Dulhu && o.ToUnit().CastingSpellId == SpellId_Slash,
				o => o.Location.RayCast(o.Rotation, 8));

			var golaIsALive = new PerFrameCachedValue<bool>(() => ScriptHelpers.IsViable(_gola) && _gola.IsAlive);
			var teluIsAlive = new PerFrameCachedValue<bool>(
					() => ScriptHelpers.IsViable(_telu) && _telu.IsAlive && (!ScriptHelpers.IsViable(_gola) || !_gola.IsAlive));

			var disableCapabilityHandler = ScriptHelpers.CombatRoutineCapabilityManager.CreateNewHandle();

			return async boss =>
			{
				ScriptHelpers.CombatRoutineCapabilityManager.Update(
					disableCapabilityHandler,
					CapabilityFlags.GapCloser,
					() => ScriptHelpers.IsViable(boss) && boss.CastingSpellId == SpellId_NoxiousEruption,
					"Dulhu is casting Noxious Eruption");

				// cast hero at start if available. 
				if (boss.HealthPercent <= 97 && teluIsAlive && await ScriptHelpers.CastHeroism())
					return true;

				// Casted by Telu, reduces damage taken by 75% and inflicts damage to attacker
				if (await ScriptHelpers.DispelEnemy("Briarskin", ScriptHelpers.EnemyDispelType.Magic, boss))
					return true;

				// Casted by Gola, removes all cooldowns from abilites
				if (await ScriptHelpers.DispelEnemy("Rapid Tides", ScriptHelpers.EnemyDispelType.Magic, boss))
					return true;

				// tank the Gola by the current kill target.
				if (Me.IsTank())
				{
					if (golaIsALive)
						return await ScriptHelpers.StayAtLocationWhile(() => golaIsALive, _gola.Location, "Dulhu");
					if (teluIsAlive)
						return await ScriptHelpers.StayAtLocationWhile(() => teluIsAlive, _telu.Location, "Telu");
				}
				return false;
			};
		}


		[EncounterHandler((int) MobId_LifeWardenGola, "Life Warden Gola")]
		public Func<WoWUnit, Task<bool>> LifeWardenGolaEncounter()
		{
			return async boss =>
			{
				_gola = boss;

				if (await ScriptHelpers.InterruptCast(boss, SpellId_RevitalizingWaters, SpellId_RapidTides))
					return true;

				// Casted by Telu, reduces damage taken by 75% and inflicts damage to attacker
				if (await ScriptHelpers.DispelEnemy("Briarskin", ScriptHelpers.EnemyDispelType.Magic, boss))
					return true;

				// Casted by Gola, removes all cooldowns from abilites
				if (await ScriptHelpers.DispelEnemy("Rapid Tides", ScriptHelpers.EnemyDispelType.Magic, boss))
					return true;

				return false;
			};
		}

		[EncounterHandler((int) MobId_EarthshaperTelu, "Earthshaper Telu")]
		public Func<WoWUnit, Task<bool>> EarthshaperTeluEncounter()
		{
			AddAvoidObject(ctx => true, 4.5f, AreaTriggerId_BramblePatch);
			return async boss =>
			{
				_telu = boss;
				if (await ScriptHelpers.InterruptCast(boss, SpellId_BramblePatch, SpellId_Briarskin))
					return true;

				// Casted by Telu, reduces damage taken by 75% and inflicts damage to attacker
				if (await ScriptHelpers.DispelEnemy("Briarskin", ScriptHelpers.EnemyDispelType.Magic, boss))
					return true;

				// Casted by Gola, removes all cooldowns from abilites
				if (await ScriptHelpers.DispelEnemy("Rapid Tides", ScriptHelpers.EnemyDispelType.Magic, boss))
					return true;

				return false;
			};
		}

		#endregion

		// guide http://www.wowhead.com/guide=2665/everbloom-dungeon-strategy-guide#xeritac-optional
		#region Xeri'tac


		#region Trash
		private const int SpellId_SporeBreath = 170411;

		private const uint MobId_InfestedVenomfang = 85232;
		[EncounterHandler((int)MobId_InfestedVenomfang, "Infested Venomfang")]
		public Func<WoWUnit, Task<bool>> InfestedVenomfangEncounter()
		{
			AddAvoidObject(
				3,
				o => o.Entry == MobId_InfestedVenomfang && o.ToUnit().CastingSpellId == SpellId_SporeBreath,
				o => Me.Location.GetNearestPointOnSegment(o.Location, o.Location.RayCast(o.Rotation, 25)));

			return async boss => false;
		}

		#endregion

		private const int SpellId_Swipe = 169371;
		private const int SpellId_ToxicBolt = 169375;
		private const int MissileSpellId_GaseousVolley = 169383;

		private const uint MobId_Xeritac = 84550;
		private const uint MobId_GorgedBusters = 86552;
		private const uint MobId_ToxicSpiderling = 84552;
		private const uint MobId_VenomCrazedPaleOne = 84554;
		private const uint MobId_VenomSprayer = 86547;

		private const uint AreaTriggerId_ToxicGas = 7582;
		private const uint DynamicObjectId_Descend = 169322;
		private const uint GameObjectId_ToxicEggs = 234113;

		[ObjectHandler((int)GameObjectId_ToxicEggs, "Toxic Eggs", ObjectRange = 40)]
		public async Task<bool> ToxicEggsHandler(WoWGameObject eggs)
		{
			if (!Me.IsTank() || !Targeting.Instance.IsEmpty() || !LootTargeting.Instance.IsEmpty() )
				return false;

			return await ScriptHelpers.InteractWithObject(eggs, 0, true);
		}

		[EncounterHandler((int)MobId_Xeritac, "Xeri'tac")]
		public Func<WoWUnit, Task<bool>> XeritacEncounter()
		{
			WoWUnit boss = null;

			AddAvoidLocation(
				ctx => ScriptHelpers.IsViable(boss) && boss.Combat,
				2,
				o => ((WoWMissile) o).ImpactPosition,
				() => WoWMissile.InFlightMissiles.Where(m => m.SpellId == MissileSpellId_GaseousVolley));

			AddAvoidObject(ctx => true, 2, AreaTriggerId_ToxicGas);

			// These mobs exploded when they reach their target.
			AddAvoidObject(ctx => true, 10, o => o.Entry == MobId_GorgedBusters && o.ToUnit().CurrentTargetGuid == Me.Guid);

			AddAvoidObject(ctx => true, 5, DynamicObjectId_Descend);

			// Swipe does AOE damage in front of the caster
			AddAvoidObject(
				ctx => Me.IsFollower() ,
				6,
				o => o.Entry == MobId_VenomCrazedPaleOne && o.ToUnit().HasAura("Toxic Blood"),
				o => o.Location.RayCast(o.Rotation, 5));

			return async npc =>
			{
				boss = npc;

				if (await ScriptHelpers.InterruptCast(boss, SpellId_ToxicBolt))
					return true;

				if (await ScriptHelpers.DispelGroup("Venomous Sting", ScriptHelpers.PartyDispelType.Poison))
					return true;

				return false;
			};
		}

		#endregion

		// guide http://www.wowhead.com/guide=2665/everbloom-dungeon-strategy-guide#archmage-sol
		#region Archmage Sol

		#region Trash

		private const int SpellId_DragonsBreath = 169843;
		private const int MissileSpellId_FrozenSnap = 169848;
		private const uint MobId_PutridPyromancer = 84957;
		private const uint MobId_InfestedIcecaller = 84989;

		[EncounterHandler((int)MobId_PutridPyromancer, "Putrid Pyromancer")]
		public async Task<bool> PutridPyromancerEncounter(WoWUnit npc)
		{
			return await ScriptHelpers.InterruptCast(npc, SpellId_DragonsBreath);
		}


		[EncounterHandler((int)MobId_InfestedIcecaller, "Infested Icecaller")]
		public Func<WoWUnit, Task<bool>> InfestedIcecallerEncounter()
		{
			AddAvoidLocation(
				ctx => true,
				3.5f,
				o => ((WoWMissile) o).ImpactPosition,
				() => WoWMissile.InFlightMissiles.Where(m => m.SpellId == MissileSpellId_FrozenSnap));

			return async boss => false;
		}

		#endregion

		private const int SpellId_ParasiticGrowth = 168885;
		private const int SpellId_Frostbolt = 166465;
		private const int SpellId_Fireball = 166464;
		private const int SpellId_ArcaneBurst = 166466;
		private const int AreaTriggerSpellId_Firebloom = 166560;
		private const int MissileSpellId_Firebloom = 166562;

		private const uint MobId_ArchmageSol = 82682;
		private const uint AreaTriggerId_Firebloom = 7368;

		private const uint AreaTriggerId_FrozenRain = 7388;

		[EncounterHandler((int)MobId_ArchmageSol, "Archmage Sol")]
		public Func<WoWUnit, Task<bool>> ArchmageSolEncounter()
		{
			AddAvoidObject(8, AreaTriggerId_FrozenRain);

			// Firebloom that tiggers the ring of fire. 
			AddAvoidLocation(
				ctx => true,
				2,
				o => ((WoWMissile)o).ImpactPosition,
				() => WoWMissile.InFlightMissiles.Where(m => m.SpellId == MissileSpellId_Firebloom));

			AddAvoidObject(2, o => o is WoWAreaTrigger && ((WoWAreaTrigger)o).SpellId == AreaTriggerSpellId_Firebloom);

			return async boss =>
			{
				var fireBloom = ObjectManager.GetObjectsOfType<WoWAreaTrigger>().FirstOrDefault();

				// jump to avoid getting hit by the fire bloom ring
				if (fireBloom != null)
				{
					var radius = 30 *  (6000-fireBloom.TimeLeft.TotalMilliseconds) / 6000;

					var dist = fireBloom.Distance;
					if (dist < radius && radius - dist < 3)
					{
						try
						{
							WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
							await Coroutine.Sleep(120);
						}
						finally
						{
							WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
						}
					}

				}
				

				return await ScriptHelpers.InterruptCast(
					boss,
					SpellId_ParasiticGrowth,
					SpellId_Frostbolt,
					SpellId_Fireball,
					SpellId_ArcaneBurst);
			};
		}

		#endregion

		// guide http://www.wowhead.com/guide=2665/everbloom-dungeon-strategy-guide#yalnu
		#region Yalnu

		private const int SpellId_ColossalBlow = 169179;

		private const uint MobId_Yalnu = 83846;
		private const uint MobId_ColossalBlow = 84964;
		private const uint MobId_Entanglement = 84499;

		private const uint MobId_ViciousMandragora = 84399;
		private const uint MobId_GnarledAncient = 84400;
		private const uint MobId_SwiftSproutling = 84401;

		private const uint MobId_FeralLasher = 86684;

		private readonly uint[] _yalknuAdds = {MobId_ViciousMandragora, MobId_GnarledAncient, MobId_SwiftSproutling, MobId_FeralLasher};

		private readonly WoWPoint _portalToValnuLoc = new WoWPoint(623.4323, 1734.328, 144.1603);
		private readonly WoWPoint _valnuFinalLoc = new WoWPoint(924.4457, -1220.136, 183.9173);

		[EncounterHandler((int) MobId_Yalnu, "Yalnu")]
		public Func<WoWUnit, Task<bool>> YalnuEncounter()
		{
			// AOE stun + damage
			AddAvoidObject(18, MobId_ColossalBlow);

			var hasAddAggro = new TimeCachedValue<bool>(
				TimeSpan.FromMilliseconds(500),
				() => !Me.IsMelee()
					&& ObjectManager.GetObjectsOfType<WoWUnit>().Any(u => _yalknuAdds.Contains(u.Entry) && u.Aggro));

			return async boss =>
			{
				if (boss.HealthPercent <= 97 && boss.HealthPercent > 20 && await ScriptHelpers.CastHeroism())
					return true;

				if (hasAddAggro && ScriptHelpers.Tank != null)
				{
					return await ScriptHelpers.StayAtLocationWhile(
						() => hasAddAggro && ScriptHelpers.Tank != null,
						ScriptHelpers.Tank.Location,
						"tank location");
				}

				// aquire a feral lasher trample target 
				if (Me.IsDps() && boss.HasAura("Genesis"))
				{
					var partyMemberLocs = Me.PartyMembers.Select(p => p.Location).ToList();
					var submergedFeralLasher = ObjectManager.GetObjectsOfType<WoWUnit>()
						.Where(
							u =>
							{
								if (u.Entry != MobId_FeralLasher || !u.HasAura("Genesis"))
									return false;

								if (Blacklist.Contains(u, BlacklistFlags.Interact))
									return false;

								if (partyMemberLocs.Any(l => ScriptHelpers.AtLocation(l, u.Location, 1)))
								{
									Blacklist.Add(u, BlacklistFlags.Interact, TimeSpan.FromSeconds(5));
									return false;
								}
								return true;
							})
						.OrderBy(u => u.DistanceSqr)
						.FirstOrDefault();

					// trample a feral lasher
					if (ScriptHelpers.IsViable(submergedFeralLasher))
					{
						return await ScriptHelpers.StayAtLocationWhile(
							() => ScriptHelpers.IsViable(submergedFeralLasher)
								&& submergedFeralLasher.HasAura("Submerged")
								&& !Blacklist.Contains(submergedFeralLasher, BlacklistFlags.Interact),
							submergedFeralLasher.Location,
							"Feral Lasher",
							1f);
					}
				}

				return false;
			};
		}

		private bool AtYalnuEncounter(WoWPoint loc)
		{
			return loc.DistanceSqr(_valnuFinalLoc) <= 100*100;
		}

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
	}

	#endregion
}