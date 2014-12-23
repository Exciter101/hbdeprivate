using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bots.DungeonBuddy.Attributes;
using Bots.DungeonBuddy.Avoidance;
using Bots.DungeonBuddy.Helpers;
using Bots.DungeonBuddy.Enums;
using Buddy.Coroutines;
using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.CommonBot.POI;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Tripper.MeshMisc;
using Extensions = Styx.Helpers.Extensions;

// ReSharper disable CheckNamespace

namespace Bots.DungeonBuddy.DungeonScripts.WarlordsOfDraenor
// ReSharper restore CheckNamespace
{

	#region Normal Difficulty

	public class ShadowmoonBurialGrounds : Dungeon
	{
		#region Overrides of Dungeon

		public override uint DungeonId { get { return 783; } }

		public override WoWPoint ExitLocation { get { return new WoWPoint(1712.285, 254.3997, 328.5056); } }

		public override void RemoveTargetsFilter(List<WoWObject> units)
		{
			var isTank = Me.IsTank();

			units.RemoveAll(
				obj =>
				{
					var unit = obj as WoWUnit;
					if (unit == null)
						return false;

					if (isTank && unit.Entry == MobId_RitualofBones)
						return true;

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
					if (unit.Entry == MobId_DefiledSpirit || unit.Entry == MobId_PossessedSoul)
						outgoingunits.Add(unit);
						// Only dps should be killing these since tank needs to be facing Ner'zhul away from group
					else if (unit.Entry == MobId_RitualofBones && isDps)
						outgoingunits.Add(unit);
				}
			}
		}

		public override void WeighTargetsFilter(List<Targeting.TargetPriority> priorities)
		{
			var isDps = Me.IsDps();

			foreach (var priority in priorities)
			{
				var unit = priority.Object as WoWUnit;
				if (unit != null)
				{
					switch (unit.Entry)
					{
						case MobId_ShadowmoonBoneMender:
							if (isDps)
								priority.Score += 3500;
							break;
						case MobId_DefiledSpirit:
							if (isDps || ScriptHelpers.IsViable(_sadana) && _sadana.Combat)
								priority.Score += 4500;
							break;
						case MobId_PossessedSoul:
							priority.Score += 4500;
							break;
						case MobId_RitualofBones:
							priority.Score = unit == _selectedRitualOfBonesTarget ? 5000 : -5000;
							break;
					}
				}
			}
		}

		public override void OnEnter()
		{
			_dynamicBlackspots = GetEntranceTrashBlackspots()
				.Concat(GetBonemawTrashBlackspots()).ToList();

			DynamicBlackspotManager.AddBlackspots(_dynamicBlackspots);
			Lua.Events.AttachEvent("RAID_BOSS_EMOTE", OnRaidBossEmote);
			_backStepCancelBehavior = new ActionRunCoroutine(ctx => CancelBackstep());
			TreeHooks.Instance.InsertHook("Dungeonbuddy_Main", 0, _backStepCancelBehavior);
		}

		public override void OnExit()
		{
			DynamicBlackspotManager.RemoveBlackspots(_dynamicBlackspots);
			_dynamicBlackspots = null;
			Lua.Events.DetachEvent("RAID_BOSS_EMOTE", OnRaidBossEmote);
			TreeHooks.Instance.RemoveHook("Dungeonbuddy_Main", _backStepCancelBehavior);
			_backStepCancelBehavior = null;
		}

		readonly WoWPoint _bonemawLoc = new WoWPoint(1849.425, -551.4028, 201.3045);
		readonly WoWPoint _bonemawMoveToLoc = new WoWPoint(1843.35, -542.5502, 201.6532);

		public override async Task<bool> HandleMovement(WoWPoint location)
		{
			var myLoc = Me.Location;

			var destinationInNerzhulsRoom = location.DistanceSqr(_nerzhulRoomCenterLoc) <= 75*75;
			// If the portal to Ner'zhul is not up then just move to 'location' to get ported down.
			// The portal that spawns after a wipe is handled in mesh.
			if (destinationInNerzhulsRoom && Targeting.Instance.IsEmpty() && LootTargeting.Instance.IsEmpty()
				&& ObjectManager.GetObjectsOfType<WoWGameObject>().All(a => a.Entry != GameObjectId_EntertheShadowlands))
			{
				return (await CommonCoroutines.MoveTo(_nerzhulAutoPortFromLoc)).IsSuccessful();
			}

			// We can't navigate out of Ner'zhul's room, block any attempt to
			if (myLoc.DistanceSqr(_nerzhulRoomCenterLoc) < 75*75 && !destinationInNerzhulsRoom)
				return true;

			if (location.DistanceSqr(_bonemawLoc) < 3*3)
				return (await CommonCoroutines.MoveTo(_bonemawMoveToLoc)).IsSuccessful();

			return false;
		}

		public override bool CanNavigateFully(WoWPoint @from, WoWPoint to)
		{
			if (to.DistanceSqr(_bonemawLoc) < 3*3)
				return true;

			return base.CanNavigateFully(@from, to);
		}

		#endregion

		#region Root

		private const uint AreaTriggerId_ShadowRune1 = 5992;
		private const uint AreaTriggerId_ShadowRune2 = 5994;
		private const uint AreaTriggerId_ShadowRune3 = 5996;
		private Composite _backStepCancelBehavior;
		private List<DynamicBlackspot> _dynamicBlackspots;

		private static LocalPlayer Me { get { return StyxWoW.Me; } }

		[EncounterHandler(0, "Root")]
		public Func<WoWUnit, Task<bool>> RootHandler()
		{
			AddAvoidObject(
				ctx => true,
				2f,
				o => o.Entry == AreaTriggerId_ShadowRune2 && o.ZDiff < 15 && o.DistanceSqr < 60*60,
				ignoreIfBlocking: true);

			AddAvoidObject(
				ctx => true,
				2.5f,
				o => o.Entry == AreaTriggerId_ShadowRune3 && o.ZDiff < 15 && o.DistanceSqr < 60*60,
				ignoreIfBlocking: true);

			return async boss => await ScriptHelpers.CancelCinematicIfPlaying();
		}

		private async Task<bool> CancelBackstep()
		{
			if (Me.Combat || !_isBacksteping)
				return false;
			
			StopBackstepping();
			return true;
		}

		[LocationHandler(1912.797, -26.04675, 286.9844, 10)]
		public async Task<bool> HandleShortcutAtEntrance(WoWPoint loc)
		{
			if (!Me.GotAlivePet || BotPoi.Current.Type != PoiType.None)
				return false;

			// a bit of a hack. Current navigator doesn't handle dismissing pets so we need to do it.
			var meshNavigator = Navigator.NavigationProvider as MeshNavigator;
			if (meshNavigator == null || meshNavigator.CurrentMovePath == null)
				return false;

			var index = Extensions.IndexOf(meshNavigator.CurrentMovePath.Path.AbilityFlags, AbilityFlags.Jump);
			if (index == -1 || meshNavigator.CurrentMovePath.Index > index
				|| index + 1 >= meshNavigator.CurrentMovePath.Path.Points.Length)
			{
				return false;
			}

			if (meshNavigator.CurrentMovePath.Path.Points[index].DistanceSqr(loc) > 10*10)
				return false;

			var moveTo = meshNavigator.CurrentMovePath.Path.Points[index + 1];
			Logging.Write("Dismissing pet before taking shortcut");
			await WoWPetControl.DismissPet();
			// Note. This will cause a new CurrentMovePath to be generated so no need to set any indexes... 
			await ScriptHelpers.MoveToContinue(() => moveTo);
			return true;
		}

		#endregion

		#region Sadana Bloodfury

		#region Trash

		private const int SpellId_ShadowMend = 152818;
		private const int SpellId_VoidPulse = 152964;

		private const uint MobId_ShadowmoonBoneMender = 75713;
		private const uint MobId_VoidSpawn = 75652;
		private const uint MobId_ShadowmoonLoyalist = 75506;
		private const uint AreaTriggerId_VoidSphere = 6016;

		private static readonly WoWPoint RightEntranceTrashLoc = new WoWPoint(1715.288, 48.13368, 287.0209);
		private static readonly WoWPoint LeftEntranceTrashLoc = new WoWPoint(1881.267, 59.7691, 287.0202);

		private readonly TimeCachedValue<bool> ShouldAvoidLeftEntranceSide = new TimeCachedValue<bool>(
			TimeSpan.FromSeconds(5),
			() => ScriptHelpers.GetUnfriendlyNpsAtLocation(LeftEntranceTrashLoc, 20, unit => unit.IsHostile).Any());

		private readonly TimeCachedValue<bool> ShouldAvoidRightEntranceSide = new TimeCachedValue<bool>(
			TimeSpan.FromSeconds(5),
			() => ScriptHelpers.GetUnfriendlyNpsAtLocation(RightEntranceTrashLoc, 20, unit => unit.IsHostile).Any());

		[EncounterHandler((int) MobId_ShadowmoonLoyalist, "Shadowmoon Loyalist")]
		public Func<WoWUnit, Task<bool>> ShadowmoonLoyalistEncounter()
		{
			AddAvoidObject(
				ctx => true,
				3,
				o => o.Entry == AreaTriggerId_VoidSphere,
				o => Me.Location.GetNearestPointOnSegment(o.Location, o.Location.RayCast(o.Rotation, 15)));

			return async npc => false;
		}

		[EncounterHandler((int) MobId_ShadowmoonBoneMender, "Shadowmoon Bone-Mender")]
		public Func<WoWUnit, Task<bool>> ShadowmoonBoneMenderEncounter()
		{
			return async npc => await ScriptHelpers.InterruptCast(npc, SpellId_ShadowMend);
		}

		[EncounterHandler((int) MobId_VoidSpawn, "Void Spawn")]
		public Func<WoWUnit, Task<bool>> VoidSpawnEncounter()
		{
			return async npc => await ScriptHelpers.InterruptCast(npc, SpellId_VoidPulse);
		}


		private IEnumerable<DynamicBlackspot> GetEntranceTrashBlackspots()
		{
			yield return new DynamicBlackspot(
				() => ShouldAvoidRightEntranceSide,
				() => RightEntranceTrashLoc,
				LfgDungeon.MapId,
				40,
				20,
				"Right Entrance Trash group");

			yield return new DynamicBlackspot(
				() => ShouldAvoidLeftEntranceSide,
				() => LeftEntranceTrashLoc,
				LfgDungeon.MapId,
				40,
				20,
				"Left Entrance Trash group");
		}

		#endregion

		private const int SpellId_DarkEclipse = 164974;
		private const int MissileSpellId_Daggerfall = 153225;

		private const uint MobId_Daggerfall = 75981;
		private const uint MobId_DefiledSpirit = 75966;
		private const uint AreaTriggerId_LunarRune2 = 6975;

		private const uint MobId_SadanaBloodfury = 75509;

		private readonly WoWPoint[] DarkEclipseSafePoints =
		{
			new WoWPoint(1795.755, -12.44243, 261.3086),
			new WoWPoint(1805.134, -16.60828, 261.3086),
			new WoWPoint(1809.745, -26.89476, 261.3086),
			new WoWPoint(1805.725, -37.13824, 261.3086),
			new WoWPoint(1795.753, -40.47881, 261.3086),
			new WoWPoint(1786.224, -36.83677, 261.3086),
			new WoWPoint(1781.868, -26.86782, 261.3086),
			new WoWPoint(1786.319, -16.44705, 261.3086),
		};

		private WoWUnit _sadana;
		// http://www.wowhead.com/guide=2668/shadowmoon-burial-grounds-dungeon-strategy-guide#sadana-bloodfury
		[EncounterHandler((int) MobId_SadanaBloodfury, "Sadana Bloodfury")]
		public Func<WoWUnit, Task<bool>> SadanaBloodfuryEncounter()
		{
			var roomCenterLoc = new WoWPoint(1795.512, -27.01042, 261.3087);

			AddAvoidLocation(
				ctx => true,
				() => roomCenterLoc,
				18,
				3,
				o => ((WoWMissile) o).ImpactPosition,
				() => WoWMissile.InFlightMissiles.Where(m => m.SpellId == MissileSpellId_Daggerfall));

			AddAvoidObject(ctx => true, () => roomCenterLoc, 18, 4, MobId_Daggerfall);

			// ignore these unless inside the circle that boss stands in.. 
			AddAvoidObject(
				ctx => true,
				() => roomCenterLoc,
				18,
				1.5f,
				o => o.Entry == AreaTriggerId_ShadowRune1 && o.ZDiff < 15 && o.DistanceSqr < 60*60,
				ignoreIfBlocking: true);

			var inDarkEclipsePhase =
				new PerFrameCachedValue<bool>(() => ScriptHelpers.IsViable(_sadana) && _sadana.HasAura(SpellId_DarkEclipse));
			Func<bool> handleDarkEclipse = () => inDarkEclipsePhase;

			return async boss =>
						{
							_sadana = boss;
							//if (boss.DistanceSqr > 19 * 19)
							//    return (await CommonCoroutines.MoveTo(boss.Location)).IsSuccessful();

							if (inDarkEclipsePhase)
							{
								var safePoint = DarkEclipseSafePoints.Where(l => !AvoidanceManager.Avoids.Any(a => a.IsPointInAvoid(l)))
									.OrderBy(l => l.DistanceSqr(Me.Location))
									.FirstOrDefault();

								return await ScriptHelpers.StayAtLocationWhile(handleDarkEclipse, safePoint, "Dark Eclipse safe spot", 1f);
							}

							return false;
						};
		}

		#endregion

		#region Nhallish

		#region Trash

		private const int SpellId_RendingVoidlash = 156776;
		private const uint MobId_ShadowmoonEnslaver = 76446;

		[EncounterHandler((int) MobId_ShadowmoonEnslaver, "Shadowmoon Enslaver")]
		public Func<WoWUnit, Task<bool>> ShadowmoonEnslaverEncounter()
		{
			return async npc => await ScriptHelpers.InterruptCast(npc, SpellId_RendingVoidlash);
		}

		#endregion

		private const int SpellId_VoidVortex = 152801;
		private const uint MobId_PossessedSoul = 75899;
		private const uint MobId_Nhallish = 75829;
		private const uint AreaTriggerId_SummonAncestors = 6045;
		private const uint AreaTriggerId_VoidDevastation = 6036;
		private const uint AreaTriggerId_VoidVortex = 6017;

		// http://www.wowhead.com/guide=2668/shadowmoon-burial-grounds-dungeon-strategy-guide#nhallish

		[EncounterHandler((int) MobId_Nhallish, "Nhallish", Mode = CallBehaviorMode.Proximity)]
		public Func<WoWUnit, Task<bool>> NhallishEncounter()
		{
			AddAvoidObject(ctx => true, 4.5f, AreaTriggerId_SummonAncestors);
			AddAvoidObject(ctx => true, 6, AreaTriggerId_VoidDevastation);
			AddAvoidObject(
				ctx => true,
				15.5f,
				o => o.Entry == AreaTriggerId_VoidVortex || o.Entry == MobId_Nhallish && o.ToUnit().CastingSpellId == SpellId_VoidVortex);

			var rightDoorEdge = new WoWPoint(1744.237, -189.2551, 257.9099);
			var leftDoorEdge = new WoWPoint (1744.646, -206.0277, 255.882);
			var randomPointInsideRoom = WoWMathHelper.GetRandomPointInCircle(new WoWPoint(1725.798, -194.8995, 252.0052), 3);

			return async boss =>
			{
				if (await ScriptHelpers.MoveInsideBossRoom(boss, leftDoorEdge, rightDoorEdge, randomPointInsideRoom))
					return true;

				var deadPossessedSoul = ObjectManager.GetObjectsOfType<WoWUnit>()
					.FirstOrDefault(u => u.Entry == MobId_PossessedSoul && u.HasAura("Reclaim Soul"));

				// reclaim the soul to get a 40 % damage/heal buff.
				if (deadPossessedSoul != null)
					return await ScriptHelpers.InteractWithObject(deadPossessedSoul, 3000, true);

				return false;
			};
		}

		#endregion

		#region Bonemaw

		#region Trash

		private const int SpellId_BodySlam_Trash = 153395;
		private const int SpellId_DeathVenom = 156717;
		private const uint MobId_MonstrousCorpseSpider = 76104;
		private const uint MobId_PlaguedBat = 75459;
		private const uint MobId_CarrionWorm = 76057;

		private static readonly WoWPoint RightBonemawTrashLoc = new WoWPoint(1725.447, -282.8209, 251.9631);
		private static readonly WoWPoint LeftBonemawTrashLoc = new WoWPoint(1722.297, -245.576, 251.0521);

		private readonly TimeCachedValue<bool> ShouldAvoidLeftBonemawSide = new TimeCachedValue<bool>(
			TimeSpan.FromSeconds(5),
			() => ScriptHelpers.GetUnfriendlyNpsAtLocation(LeftBonemawTrashLoc, 20, unit => unit.IsHostile).Any());

		private readonly TimeCachedValue<bool> ShouldAvoidRightBonemawSide = new TimeCachedValue<bool>(
			TimeSpan.FromSeconds(5),
			() => ScriptHelpers.GetUnfriendlyNpsAtLocation(RightBonemawTrashLoc, 20, unit => unit.IsHostile).Any());

		private IEnumerable<DynamicBlackspot> GetBonemawTrashBlackspots()
		{
			yield return new DynamicBlackspot(
				() => ShouldAvoidRightBonemawSide,
				() => RightBonemawTrashLoc,
				LfgDungeon.MapId,
				40,
				20,
				"Right Bonemaw Trash group");

			yield return new DynamicBlackspot(
				() => ShouldAvoidLeftBonemawSide,
				() => LeftBonemawTrashLoc,
				LfgDungeon.MapId,
				40,
				20,
				"Left Bonemaw Trash group");
		}


		[EncounterHandler((int) MobId_MonstrousCorpseSpider, "Monstrous Corpse Spider")]
		public Func<WoWUnit, Task<bool>> MonstrousCorpseSpiderEncounter()
		{
			return async npc => await ScriptHelpers.InterruptCast(npc, SpellId_DeathVenom);
		}


		[EncounterHandler((int) MobId_PlaguedBat, "Plagued Bat")]
		public Func<WoWUnit, Task<bool>> PlaguedBatEncounter()
		{
			return async npc =>
						{
							if (Me.PartyMembers.Any(g => g.HasAura("Plague Spit") && g.Auras["Plague Spit"].StackCount >= 5))
							{
								if (await ScriptHelpers.DispelGroup("Plague Spit", ScriptHelpers.PartyDispelType.Disease))
									return true;
							}
							return false;
						};
		}

		[EncounterHandler((int) MobId_CarrionWorm, "Carrion Worm")]
		public Func<WoWUnit, Task<bool>> CarrionWormEncounter()
		{
			WoWUnit worm = null;
			AddAvoidLocation(
				ctx => ScriptHelpers.IsViable(worm) && worm.CastingSpellId == SpellId_BodySlam_Trash,
				6*1.33f,
				o => (WoWPoint) o,
				() => ScriptHelpers.GetPointsAlongLineSegment(
					worm.Location,
					worm.Location.RayCast(worm.Rotation, 60),
					6/2).OfType<object>());

			return async npc =>
						{
							worm = npc;
							return false;
						};
		}

		#endregion
		
		private readonly WaitTimer _inhaleEmoteTimer = new WaitTimer(TimeSpan.FromSeconds(5));

		private readonly WaitTimer _inhaleEmoteResetTimer = new WaitTimer(TimeSpan.FromSeconds(20));

		private const int SpellId_Inhale = 153804;
		private const int SpellId_BodySlam = 154175;
		private const int MissileSpellId_NecroticPitch = 153689;

		private const uint MobId_Bonemaw = 75452;
		private const uint AreaTriggerId_NecroticPitch = 6098;
		private const uint MobId_WaterBurst = 77676;

		private const uint GameObjectId_BonemawEntranceDoor = 233990;

		[EncounterHandler((int) MobId_Bonemaw, "Bonemaw")]
		public Func<WoWUnit, Task<bool>> BonemawEncounter()
		{
			const float doorAvoidLineWidth = 2;
			const float bodySlamAvoidLineWidth = 7;

			WoWUnit boss = null;

			var isDoorClosed = new PerFrameCachedValue<bool>(
				() =>
				{
					if (!ScriptHelpers.IsViable(boss) || !boss.Combat)
						return false;

					var door = ObjectManager.GetObjectsOfType<WoWGameObject>()
						.FirstOrDefault(g => g.Entry == GameObjectId_BonemawEntranceDoor);
					return door != null && ((WoWDoor) door.SubObj).IsClosed;
				});

			var rightDoorEdge = new WoWPoint(1815.843, -495.1395, 201.0704);
			var leftDoorEdge = new WoWPoint(1830.936, -490.8846, 201.521);

			var jumpInWaterStartLoc = new WoWPoint(1815.321, -482.4096, 200.836);
			var jumpInWaterEndLoc = new WoWPoint(1798.076, -484.8755, 194.7952);

			var waterSpoutLocs = new[] {new WoWPoint(1798.934, -523.8507, 197.0144), new WoWPoint(1765.194, -412.3889, 197.0144)};

			AddAvoidLocation(
				ctx => !Me.IsSwimming && ScriptHelpers.IsViable(boss) && boss.CastingSpellId == SpellId_BodySlam,
				bodySlamAvoidLineWidth*1.33f,
				o => (WoWPoint) o,
				() => ScriptHelpers.GetPointsAlongLineSegment(
					boss.Location,
					boss.Location.RayCast(boss.Rotation, 60),
					bodySlamAvoidLineWidth/2).OfType<object>());

			AddAvoidLocation(
				ctx => !ScriptHelpers.IsViable(boss) || !boss.HasAura(SpellId_Inhale) && _inhaleEmoteTimer.IsFinished,
				4.5f,
				o => ((WoWMissile) o).ImpactPosition,
				() => WoWMissile.InFlightMissiles.Where(m => m.SpellId == MissileSpellId_NecroticPitch));

			AddAvoidObject(
				ctx => !ScriptHelpers.IsViable(boss) || !boss.HasAura(SpellId_Inhale) && _inhaleEmoteTimer.IsFinished,
				4.5f,
				AreaTriggerId_NecroticPitch);

			// Avoids running into the barrier at room entrance while running from something else.
			AddAvoidLocation(
				ctx => isDoorClosed,
				doorAvoidLineWidth*1.33f,
				o => (WoWPoint) o,
				() => ScriptHelpers.GetPointsAlongLineSegment(
					leftDoorEdge,
					rightDoorEdge,
					doorAvoidLineWidth/2).OfType<object>());

			var noMovebehind = ScriptHelpers.CombatRoutineCapabilityManager.CreateNewHandle();
			
			return async npc =>
			{
				boss = npc;

				// Melee can't move behind this NPC
				ScriptHelpers.CombatRoutineCapabilityManager.Update(
					noMovebehind,
					CapabilityFlags.MoveBehind,
					() => ScriptHelpers.IsViable(boss) && boss.Combat);

				// handle getting out of the water
				if (Me.IsSwimming)
				{
					// We can only use the 2 water bursts by the boss platform. The other one throws player on the walkway 
					// which gets blocked when encounter starts.
					var waterBurst = ObjectManager.GetObjectsOfType<WoWUnit>()
						.Where(u => u.Entry == MobId_WaterBurst && waterSpoutLocs.Any(l => l.Distance2DSqr(u.Location) < 10*10))
						.OrderBy(u => u.DistanceSqr)
						.FirstOrDefault();
					var moveTo = waterBurst != null ? waterBurst.Location : waterSpoutLocs[0];
					TreeRoot.StatusText = "Getting out of the water";
					return (await CommonCoroutines.MoveTo(moveTo)).IsSuccessful();
				}

				if (!Me.Location.IsPointLeftOfLine(leftDoorEdge, rightDoorEdge) && isDoorClosed)
				{
					// Get in the water if on the walkway and door to boss is closed
					// We'll take a portal from water to get to the boss.
					TreeRoot.StatusText = "Going for a swim";
					if (!Navigator.AtLocation(jumpInWaterStartLoc))
						return (await CommonCoroutines.MoveTo(jumpInWaterStartLoc)).IsSuccessful();
					Navigator.PlayerMover.MoveTowards(jumpInWaterEndLoc);
					await Coroutine.Wait(4000, () => Me.IsSwimming);
					return true;
				}

				if (!_inhaleEmoteTimer.IsFinished || boss.HasAura(SpellId_Inhale))
				{
					TreeRoot.StatusText = "Standing on top of Necrotic Pitch to avoid getting inhaled";
								
					Navigator.NavigationProvider.StuckHandler.Reset();
								
					// Stand in pitch to prevent getting sucked into Bonemaw's mouth during inhale.
					var pitch = ObjectManager.GetObjectsOfType<WoWAreaTrigger>()
						.Where(a => a.Entry == AreaTriggerId_NecroticPitch)
						.OrderBy(a => a.DistanceSqr).FirstOrDefault();

					if (pitch != null)
					{
						return await ScriptHelpers.StayAtLocationWhile(
							() => !_inhaleEmoteTimer.IsFinished  || (ScriptHelpers.IsViable(boss) && boss.HasAura(SpellId_Inhale)),
							pitch.Location,
							"Necrotic Pitch",
							4);
					}
				}
				return false;
			};
		}

		private void OnRaidBossEmote(object sender, LuaEventArgs args)
		{
			if (!_inhaleEmoteResetTimer.IsFinished)
				return;

			_inhaleEmoteTimer.Reset();
			_inhaleEmoteResetTimer.Reset();
		}

		#endregion

		#region Ner'zhul

		// Leave after killing all the fully scripted bosses that way user doesn't get deserter and has a chance at some loot.
		[ScenarioStage(1, "Leave group stage", 4)]
		public Func<ScenarioStage, Task<bool>> LeaveDungeonAtLastBoss()
		{
			var showedAlart = false;
			return async stage =>
			{
				if (showedAlart || DungeonBuddySettings.Instance.PartyMode == PartyMode.Follower)
					return false;

				Alert.Show(
					"Dungeonbuddy: Last boss is not fully functional",
					"The script for the Ner'zhul encounter is not fully functional. If you wish to stay in group then press 'Cancel'. Otherwise Dungeonbuddy will automatically leave group.",
					30,
					true,
					true,
					() => Lua.DoString("LeaveParty()"),
					null,
					"Leave",
					"Cancel");

				showedAlart = true;
				return true;
			};
		}

		private const int SpellId_Malevolence = 154442;
		private const uint MobId_OmenofDeath = 76462;
		private const uint MobId_RitualofBones = 76518;
		private const uint MobId_Nerzhul = 76407;
		private const uint GameObjectId_EntertheShadowlands = 239083;
		private const uint AreaTriggerId_RitualofBones = 6166;
		private readonly WoWPoint _nerzhulAutoPortFromLoc = new WoWPoint(1737.913, -759.2268, 235.9705);
		private readonly WoWPoint _nerzhulRoomCenterLoc = new WoWPoint(1712.156, -820.2639, 73.73562);

		private WoWUnit _selectedRitualOfBonesTarget;
		private WoWGuid _selectedRitualOfBonesTargetGuid;
		// used for calculating the safe zone to stand at/move through when dealing with skeletons.
		private WoWPoint _selectedRitualOfBonesStartLoc, _selectedRitualOfBonesEndLoc;

		// http://www.wowhead.com/guide=2668/shadowmoon-burial-grounds-dungeon-strategy-guide#nerzhul
		[EncounterHandler((int) MobId_Nerzhul, "Ner'zhul")]
		public Func<WoWUnit, Task<bool>> NerzhulEncounter()
		{
			bool skeletonPhase = false;
			bool skeletonKillPhase = false;
			bool skeletonSurvivePhase = false;

			AddAvoidObject(
				ctx => Me.IsFollower(),
				15,
				o => o.Entry == MobId_Nerzhul && !o.ToUnit().Combat && o.ToUnit().IsAlive);

			AddAvoidObject(
				ctx => true,
				o => !skeletonPhase ? 15 : 8,
				MobId_OmenofDeath);

			AddAvoidObject(
				ctx => true,
				5,
				o => o.Entry == AreaTriggerId_RitualofBones || o.Entry == MobId_RitualofBones && !skeletonKillPhase,
				o => WoWMathHelper.CalculatePointAtSide(o.Location, o.Rotation, 6, true));

			AddAvoidObject(
				ctx => true,
				o => skeletonSurvivePhase && Me.IsMoving ? 9 : 5,
				o => o.Entry == AreaTriggerId_RitualofBones || o.Entry == MobId_RitualofBones && !skeletonKillPhase,
				o => WoWMathHelper.CalculatePointAtSide(o.Location, o.Rotation, 3, true));

			AddAvoidObject(
				ctx => true,
				o => skeletonSurvivePhase && Me.IsMoving ? 9 : 5,
				o => o.Entry == AreaTriggerId_RitualofBones || o.Entry == MobId_RitualofBones && !skeletonKillPhase);

			AddAvoidObject(
				ctx => true,
				o => skeletonSurvivePhase && Me.IsMoving ? 9 : 5,
				o => o.Entry == AreaTriggerId_RitualofBones || o.Entry == MobId_RitualofBones && !skeletonKillPhase,
				o => WoWMathHelper.CalculatePointAtSide(o.Location, o.Rotation, 3, false));

			AddAvoidObject(
				ctx => true,
				5,
				o => o.Entry == AreaTriggerId_RitualofBones || o.Entry == MobId_RitualofBones && !skeletonKillPhase,
				o => WoWMathHelper.CalculatePointAtSide(o.Location, o.Rotation, 6, false));

			#region Malevolence Avoidance

			// line up a bunch of avoids to make a lone narrow cone.
			AddAvoidObject(
				ctx =>!skeletonPhase || !Me.IsTank(),
				o => 4,
				o => o.Entry == MobId_Nerzhul && o.ToUnit().CastingSpellId == SpellId_Malevolence,
				o => o.Location.RayCast(o.Rotation, 2));

			AddAvoidObject(
				ctx => !skeletonPhase || !Me.IsTank(),
				o => 5.5f,
				o => o.Entry == MobId_Nerzhul && o.ToUnit().CastingSpellId == SpellId_Malevolence,
				o => o.Location.RayCast(o.Rotation, 6));

			AddAvoidObject(
				ctx => !skeletonPhase || !Me.IsTank(),
				o => 7f,
				o => o.Entry == MobId_Nerzhul && o.ToUnit().CastingSpellId == SpellId_Malevolence,
				o => o.Location.RayCast(o.Rotation, 11));

			AddAvoidObject(
				ctx => !skeletonPhase || !Me.IsTank(),
				o => 8.5f,
				o => o.Entry == MobId_Nerzhul && o.ToUnit().CastingSpellId == SpellId_Malevolence,
				o => o.Location.RayCast(o.Rotation, 17));

			AddAvoidObject(
				ctx => !skeletonPhase || !Me.IsTank(),
				o => 10,
				o => o.Entry == MobId_Nerzhul && o.ToUnit().CastingSpellId == SpellId_Malevolence,
				o => o.Location.RayCast(o.Rotation, 24));

			AddAvoidObject(
				ctx => !skeletonPhase || !Me.IsTank(),
				o => 11.5f,
				o => o.Entry == MobId_Nerzhul && o.ToUnit().CastingSpellId == SpellId_Malevolence,
				o => o.Location.RayCast(o.Rotation, 32));

			#endregion

			var unitPathDistanceSqrToRoomCenter = new Func<WoWUnit, float>(
				u => _nerzhulRoomCenterLoc.GetNearestPointOnLine(u.Location, u.Location.RayCast(u.Rotation, 10))
					.DistanceSqr(_nerzhulRoomCenterLoc));

			return async boss =>
			{
				var ritualOfBones = ObjectManager.GetObjectsOfType<WoWUnit>()
					.Where(u => u.Entry == MobId_RitualofBones && u.IsAlive)
					.OrderBy(u => u.HealthPercent)
					.ToList();

				skeletonPhase = ritualOfBones.Any();
				skeletonKillPhase = skeletonPhase && ritualOfBones.Count == 6;
				skeletonSurvivePhase = skeletonPhase && !skeletonKillPhase;

				// cast heroism at start of fight after 
				if (boss.HealthPercent <= 97 && await ScriptHelpers.CastHeroism())
					return true;

				if (skeletonKillPhase)
				{
					// select the lowest HP skeleton or the one pathing the nearest to room center.
					_selectedRitualOfBonesTarget = ritualOfBones.Any(u => u.Entry == MobId_RitualofBones && u.HealthPercent < 99.9)
						? ritualOfBones.First()
						: ritualOfBones.OrderBy(unitPathDistanceSqrToRoomCenter).First();

					if (_selectedRitualOfBonesTarget.Guid != _selectedRitualOfBonesTargetGuid)
					{
						_selectedRitualOfBonesStartLoc = _selectedRitualOfBonesTarget.Location;
						// Any distance will work, just need a start and end point along a line
						_selectedRitualOfBonesEndLoc = _selectedRitualOfBonesStartLoc.RayCast(_selectedRitualOfBonesTarget.Rotation, 1);
						_selectedRitualOfBonesTargetGuid = _selectedRitualOfBonesTarget.Guid;
					}

					if (Me.IsMeleeDps())
						return await HandleMeleeDpsRitualOfBones(_selectedRitualOfBonesTarget);

					if (Me.IsRange())
						return await HandleRangeRitualOfBones(_selectedRitualOfBonesTarget);

					// Commented out since tank seems to have better luck tanking towards center of room 
					// rather then run towards edge of room opposite of skeltons and take a lot of damage from 
					// getting hit from behind. Might adjust the behavior later to have tank back up towards
					// edge if if it helps any.
					if (Me.IsTank() )
						return await HandleTankRitualOfBones(_selectedRitualOfBonesTarget);
				} 
				else if (_isBacksteping)
				{
					// Make sure any melee DPS isn't still backing up.. 
					StopBackstepping();
					return true;
				}
				else if (skeletonSurvivePhase)
				{
					// if one (or more) of the skeletons is dead then stay along the line cleared by killing a skeleton.
					// Tank will stay at edge of room and continue faceing boss away from group 
					var nearestPoint = Me.Location.GetNearestPointOnLine(_selectedRitualOfBonesStartLoc, _selectedRitualOfBonesEndLoc);
					if (Me.Location.DistanceSqr(nearestPoint) > 6*6)
						return (await CommonCoroutines.MoveTo(nearestPoint)).IsSuccessful();
				}


				if (!skeletonPhase && await ScriptHelpers.TankUnitAtLocation(_nerzhulRoomCenterLoc, 10))
					return true;

				return false;
			};
		}


		#region Tank

		private async Task<bool> HandleTankRitualOfBones(WoWUnit target)
		{
			WoWPoint hitLoc;
			var targetLoc = target.Location;
			var furthestLoc = targetLoc.RayCast(target.Rotation, 120);

			var hitResult = Avoidance.Helpers.MeshTraceline(targetLoc, furthestLoc,out hitLoc);

			if (!hitResult.HasValue)
				return false;

			if (hitResult.Value)
				furthestLoc = hitLoc;
			
			// if the skeletions have crossed the room do nothing. we're fuked
			if (furthestLoc.DistanceSqr(targetLoc) <= 5*5)
				return false;

			foreach (var point in ScriptHelpers.GetPointsAlongLineSegment(hitLoc, targetLoc, 2))
			{
				if (AvoidanceManager.Avoids.Any(a => a.IsPointInAvoid(point)))
					continue;

				MoveToLocationWhileFacingUnit(point, Me.CurrentTarget, () => ScriptHelpers.IsViable(target), "Moving boss to room edge");
				//return await ScriptHelpers.StayAtLocationWhile(() => ScriptHelpers.IsViable(target), point, precision: 6);
				break;
			}

			return false;
		}

		#endregion

		#region Ranged
		private async Task<bool> HandleRangeRitualOfBones(WoWUnit target)
		{
			var myLoc = Me.Location;
			var unitLoc = target.Location;

			WoWPoint hitLoc;
			var start = unitLoc.RayCast(target.Rotation, 10);
			var furthestLoc = unitLoc.RayCast(target.Rotation, 120);

			var hitResult = Avoidance.Helpers.MeshTraceline(start, furthestLoc, out hitLoc);

			if (!hitResult.HasValue)
				return false;

			if (hitResult.Value)
				furthestLoc = hitLoc;

			var nearestPoint = myLoc.GetNearestPointOnSegment(start, furthestLoc);

			if (myLoc.Distance(nearestPoint) > 3*3)
			{
				var moveTo = nearestPoint.DistanceSqr(unitLoc) >= 30*30
					? nearestPoint
					: unitLoc.RayCast(target.Rotation, Math.Min(30, unitLoc.Distance(furthestLoc)));

				var moveTimer = new WaitTimer(TimeSpan.FromMilliseconds(StyxWoW.Random.Next(3000, 4000)));
				moveTimer.Reset();
				await ScriptHelpers.MoveToContinue(() => moveTo, () => !moveTimer.IsFinished);
				
			}
			return false;
		}

		#endregion

		#region Melee DPS

		private async Task<bool> HandleMeleeDpsRitualOfBones(WoWUnit target)
		{
			MoveToLocationWhileFacingUnit(
				target.Location.RayCast(target.Rotation, 10),
				target,
				() => ScriptHelpers.IsViable(target),
				string.Format("Staying in front of skeleton" ));

			return false;
		}

		#endregion

		#region Backstep

		private CapabilityManagerHandle _backstepNoMoveCapabilityHandle;
		private bool _isBacksteping;

		private void StopBackstepping()
		{
			// Using LUA instead of WoWMovement API due to having issues with the later.
			if (StyxWoW.Me.MovementInfo.MovingBackward)
				Lua.DoString("MoveBackwardStop()");
			if (StyxWoW.Me.MovementInfo.MovingStrafeLeft)
				Lua.DoString("StrafeLeftStop()");
			if (StyxWoW.Me.MovementInfo.MovingStrafeRight)
				Lua.DoString("StrafeRightStop()");
			if (StyxWoW.Me.MovementInfo.MovingForward)
				Lua.DoString("MoveForwardStop()");
			_isBacksteping = false;
		}

		private void MoveToLocationWhileFacingUnit(WoWPoint destination, WoWUnit unit, Func<bool> conditon, string reason = null)
		{
			_isBacksteping = true;
			// todo: undo 'true' assignment once issues with facing in Singular are resolved

			var dumbCR = true; // RoutineManager.Current.SupportedCapabilities == CapabilityFlags.None;
			if (dumbCR && ScriptHelpers.MovementEnabled)
			{
				ScriptHelpers.DisableMovement(conditon);
			}
			else if (!dumbCR)
			{
				if (_backstepNoMoveCapabilityHandle == null)
					_backstepNoMoveCapabilityHandle = ScriptHelpers.CombatRoutineCapabilityManager.CreateNewHandle();

				ScriptHelpers.CombatRoutineCapabilityManager.Update(
					_backstepNoMoveCapabilityHandle,
					CapabilityFlags.Movement,
					conditon,
					reason);
			}

			var unitLoc = unit.Location;
			var myLoc = Me.Location;

			var unitToDestinationDistance = unitLoc.Distance(destination);
			var meToDestinationDistance = myLoc.Distance(destination);

			var maxMeleeRange = unit.MeleeRange();
			//float minMeleeRange = maxMeleeRange;
			var me2Unit = myLoc - unit.Location;
			var unit2End = unit.Location - destination;
			me2Unit.Normalize();
			unit2End.Normalize();

			if (!WoWMovement.IsFacing)
				WoWMovement.ConstantFace(unit.Guid);

			if (unit.Distance > maxMeleeRange || meToDestinationDistance >= unitToDestinationDistance)
			{
				if (StyxWoW.Me.MovementInfo.MovingBackward)
					Lua.DoString("MoveBackwardStop()");
				if (StyxWoW.Me.MovementInfo.MovingStrafeLeft)
					Lua.DoString("StrafeLeftStop()");
				if (StyxWoW.Me.MovementInfo.MovingStrafeRight)
					Lua.DoString("StrafeRightStop()");

				if (!StyxWoW.Me.MovementInfo.MovingForward)
					Lua.DoString("MoveForwardStart()");
				return;
			}

			if (StyxWoW.Me.MovementInfo.MovingForward)
				Lua.DoString("MoveForwardStop()");

			if (myLoc.DistanceSqr(unitLoc) <= maxMeleeRange * maxMeleeRange && !StyxWoW.Me.MovementInfo.MovingBackward)
				Lua.DoString("MoveBackwardStart()");
			else if (StyxWoW.Me.MovementInfo.MovingBackward)
				Lua.DoString("MoveBackwardStop()");


			var dot = Math.Abs(me2Unit.Dot(unit2End));
			// strife left or right around mob if not between mob and destination
			if (dot < 0.9f)
			{
				var isLeft = myLoc.IsPointLeftOfLine(unit.Location, destination);

				if (!StyxWoW.Me.MovementInfo.MovingStrafeLeft && isLeft)
					Lua.DoString("StrafeLeftStart()");

				if (!StyxWoW.Me.MovementInfo.MovingStrafeRight && !isLeft)
					Lua.DoString("StrafeRightStart()");
			}
			else if (StyxWoW.Me.MovementInfo.MovingStrafeLeft)
				Lua.DoString("StrafeLeftStop()");
			else if (StyxWoW.Me.MovementInfo.MovingStrafeRight)
				Lua.DoString("StrafeRightStop()");
		}

		#endregion

		#endregion
	}

	#endregion

	#region Heroic Difficulty

	public class ShadowmoonBurialGroundsHeroic : ShadowmoonBurialGrounds
	{
		#region Overrides of Dungeon

		public override uint DungeonId { get { return 784; } }


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
			//base.OnEnter();
		}

		#endregion
	}

	#endregion
}