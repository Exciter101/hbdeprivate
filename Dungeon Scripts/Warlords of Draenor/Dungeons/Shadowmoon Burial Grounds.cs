using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bots.DungeonBuddy.Attributes;
using Bots.DungeonBuddy.Helpers;
using Bots.DungeonBuddy.Enums;
using Bots.DungeonBuddy.Helpers;
using Styx;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

// ReSharper disable CheckNamespace
namespace Bots.DungeonBuddy.DungeonScripts.WarlordsOfDraenor
// ReSharper restore CheckNamespace
{
	#region Normal Difficulty

	public class ShadowmoonBurialGrounds : Dungeon
	{
		#region Overrides of Dungeon

	
		public override uint DungeonId
		{
			get { return 783; }
		}

        public override WoWPoint ExitLocation { get { return new WoWPoint(1712.285, 254.3997, 328.5056); } }

	    public override void RemoveTargetsFilter(List<WoWObject> units)
	    {
	        var isDps = Me.IsDps();

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
            foreach (var obj in incomingunits)
            {
                var unit = obj as WoWUnit;
                if (unit != null)
                {
                    if (unit.Entry == MobId_DefiledSpirit || unit.Entry == MobId_PossessedSoul || unit.Entry == MobId_RitualofBones)
                        outgoingunits.Add(unit);
                }
            }
	    }

	    public override void WeighTargetsFilter(List<Targeting.TargetPriority> units)
	    {
	        var isDps = Me.IsDps();

	        foreach (var priority in units)
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
                        case MobId_PossessedSoul:
                        case MobId_DefiledSpirit:
                        case MobId_RitualofBones:
	                        priority.Score += 4500;
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

	    public override void OnExit()
	    {
            DynamicBlackspotManager.RemoveBlackspots(_dynamicBlackspots);
	        _dynamicBlackspots = null;
	    }

	    #endregion

		#region Root
        private List<DynamicBlackspot> _dynamicBlackspots;

        private const uint AreaTriggerId_ShadowRune1 = 5992;
        private const uint AreaTriggerId_ShadowRune2 = 5994;
        private const uint AreaTriggerId_ShadowRune3 = 5996;

        private static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        [EncounterHandler(0, "Root")]
        public Func<WoWUnit, Task<bool>> RootHandler()
        {
            AddAvoidObject(ctx => true, 2, o => o.Entry ==  AreaTriggerId_ShadowRune1 && o.ZDiff < 15);
            AddAvoidObject(ctx => true, 2.5f, o => o.Entry == AreaTriggerId_ShadowRune2 && o.ZDiff < 15);
            AddAvoidObject(ctx => true, 3, o => o.Entry == AreaTriggerId_ShadowRune3 && o.ZDiff < 15);
            
            return async boss => false;
        }

		#endregion

        #region Sadana Bloodfury

	    #region Trash

	    private const int SpellId_ShadowMend = 152818;
	    private const int SpellId_VoidPulse = 152964;

	    private const uint MobId_ShadowmoonBoneMender = 75713;
        private const uint MobId_VoidSpawn = 75652;
        private const uint MobId_ShadowmoonLoyalist=75506;
        private const uint AreaTriggerId_VoidSphere=6016;

        private static readonly WoWPoint RightEntranceTrashLoc = new WoWPoint(1715.288, 48.13368, 287.0209);
        private static readonly WoWPoint LeftEntranceTrashLoc = new WoWPoint(1881.267, 59.7691, 287.0202);

        [EncounterHandler((int)MobId_ShadowmoonLoyalist, "Shadowmoon Loyalist")]
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

        [EncounterHandler((int)MobId_VoidSpawn, "Void Spawn")]
        public Func<WoWUnit, Task<bool>> VoidSpawnEncounter()
        {
            return async npc => await ScriptHelpers.InterruptCast(npc, SpellId_VoidPulse);
        }


        private readonly TimeCachedValue<bool> ShouldAvoidRightEntranceSide = new TimeCachedValue<bool>(
	        TimeSpan.FromSeconds(5),
            () => ScriptHelpers.GetUnfriendlyNpsAtLocation(RightEntranceTrashLoc, 20, unit => unit.IsHostile).Any());

	    private readonly TimeCachedValue<bool> ShouldAvoidLeftEntranceSide = new TimeCachedValue<bool>(
	        TimeSpan.FromSeconds(5),
	        () => ScriptHelpers.GetUnfriendlyNpsAtLocation(LeftEntranceTrashLoc, 20, unit => unit.IsHostile).Any());

	    IEnumerable<DynamicBlackspot> GetEntranceTrashBlackspots()
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

        // http://www.wowhead.com/guide=2668/shadowmoon-burial-grounds-dungeon-strategy-guide#sadana-bloodfury
        [EncounterHandler((int)MobId_SadanaBloodfury, "Sadana Bloodfury")]
        public Func<WoWUnit, Task<bool>> SadanaBloodfuryEncounter()
        {
            AddAvoidLocation(
                ctx => true,
                4,
                o => ((WoWMissile) o).ImpactPosition,
                () => WoWMissile.InFlightMissiles.Where(m => m.SpellId == MissileSpellId_Daggerfall));

            AddAvoidObject(ctx => true, 4, MobId_Daggerfall);

            return async boss =>
            {
                if (boss.CastingSpellId == SpellId_DarkEclipse || boss.HasAura(SpellId_DarkEclipse))
                {
                    var lunarPurity = ObjectManager.GetObjectsOfType<WoWAreaTrigger>()
                        .Where(a => a.Entry == AreaTriggerId_LunarRune2)
                        .OrderBy(a => a.DistanceSqr)
                        .FirstOrDefault();

                    if (lunarPurity != null && await ScriptHelpers.StayAtLocationWhile(
                                () => ScriptHelpers.IsViable(lunarPurity),
                                lunarPurity.Location,
                                "Lunar Purity",
                                1.8f))
                    {
                        return true;
                    }
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

	    [EncounterHandler((int) MobId_Nhallish, "Nhallish")]
	    public Func<WoWUnit, Task<bool>> NhallishEncounter()
	    {
            AddAvoidObject(ctx => true, 4.5f, AreaTriggerId_SummonAncestors);
            AddAvoidObject(ctx => true, 6, AreaTriggerId_VoidDevastation);
            AddAvoidObject(ctx => true, 15.5f, o => o.Entry == AreaTriggerId_VoidVortex || o.Entry == MobId_Nhallish && o.ToUnit().CastingSpellId == SpellId_VoidVortex);

	        return async boss => { return false; };
	    }

	    #endregion

	    #region Bonemaw

	    #region Trash

	    private const int SpellId_DeathVenom = 156717;
        private const uint MobId_MonstrousCorpseSpider = 76104;
        private const uint MobId_PlaguedBat = 75459;

	    private static readonly WoWPoint RightBonemawTrashLoc = new WoWPoint(1725.447, -282.8209, 251.9631);
	    private static readonly WoWPoint LeftBonemawTrashLoc = new WoWPoint(1722.297, -245.576, 251.0521);

	    private readonly TimeCachedValue<bool> ShouldAvoidRightBonemawSide = new TimeCachedValue<bool>(
	        TimeSpan.FromSeconds(5),
	        () => ScriptHelpers.GetUnfriendlyNpsAtLocation(RightBonemawTrashLoc, 20, unit => unit.IsHostile).Any());

	    private readonly TimeCachedValue<bool> ShouldAvoidLeftBonemawSide = new TimeCachedValue<bool>(
	        TimeSpan.FromSeconds(5),
	        () => ScriptHelpers.GetUnfriendlyNpsAtLocation(LeftBonemawTrashLoc, 20, unit => unit.IsHostile).Any());

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


        [EncounterHandler((int)MobId_PlaguedBat, "Plagued Bat")]
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

	    #endregion

	    private const int SpellId_Inhale = 153804;
	    private const int SpellId_BodySlam = 154175;
	    private const int MissileSpellId_NecroticPitch = 153689;

        private const uint MobId_Bonemaw = 75452;
        private const uint AreaTriggerId_NecroticPitch = 6098;

        [EncounterHandler((int)MobId_Bonemaw, "Bonemaw")]
        public Func<WoWUnit, Task<bool>> BonemawEncounter()
        {
            WoWUnit boss = null;
            AddAvoidObject(ctx => !ScriptHelpers.IsViable(boss) || !boss.HasAura(SpellId_Inhale), 4.5f, AreaTriggerId_NecroticPitch);
            AddAvoidObject(ctx => true, 18, o => o.Entry == MobId_Bonemaw && o.ToUnit().HasAura(SpellId_Inhale));
            // Sidestep the Body Slamm attack.
            AddAvoidObject(ctx => true, 8, o => o.Entry == MobId_Bonemaw && o.ToUnit().CastingSpellId == SpellId_BodySlam,
                o => Me.Location.GetNearestPointOnSegment(o.Location, o.Location.RayCast(o.Rotation, 60)));

            AddAvoidLocation(
                ctx => true,
                4.5f,
                o => ((WoWMissile) o).ImpactPosition,
                () => WoWMissile.InFlightMissiles.Where(m => m.SpellId == MissileSpellId_NecroticPitch));

            return async npc =>
            {
                boss = npc;

                if (boss.HasAura(SpellId_Inhale))
                {
                    // Stand in pitch to prevent getting sucked into Bonemaw's mouth during inhale.
                    var pitch = ObjectManager.GetObjectsOfType<WoWAreaTrigger>()
                        .Where(a => a.Entry == AreaTriggerId_NecroticPitch)
                        .OrderBy(a => a.DistanceSqr).FirstOrDefault();

                    if (pitch != null && await ScriptHelpers.StayAtLocationWhile(
                        () => ScriptHelpers.IsViable(boss) && boss.HasAura(SpellId_Inhale),
                        pitch.Location,
                        "Necrotic Pitch",
                        4))
                    {
                        return true;
                    }
                }
                return false;
            };
        }

	    #endregion

        #region Ner'zhul

	    private const int SpellId_Malevolence = 154442;
        private const uint MobId_OmenofDeath = 76462;
        private const uint MobId_RitualofBones = 76518;

        private const uint MobId_Nerzhul = 76407;
        private const uint AreaTriggerId_RitualofBones = 6166;

	    // http://www.wowhead.com/guide=2668/shadowmoon-burial-grounds-dungeon-strategy-guide#nerzhul
	    [EncounterHandler((int) MobId_Nerzhul, "Ner'zhul")]
	    public Func<WoWUnit, Task<bool>> NerzhulEncounter()
	    {
            AddAvoidObject(ctx => true, 5, MobId_OmenofDeath);
            AddAvoidObject(ctx => true, 9, AreaTriggerId_RitualofBones);
	        AddAvoidObject(
	            ctx => true,
	            o => 4 + (0.3f * (float)o.Distance) ,
	            o => o.Entry == MobId_Nerzhul && o.ToUnit().CastingSpellId == SpellId_Malevolence,
	            o => Me.Location.GetNearestPointOnSegment(o.Location, o.Location.RayCast(o.Rotation, 30)));

	        return async boss =>
	        {
	            if (Me.CurrentTarget == boss && await ScriptHelpers.TankFaceUnitAwayFromGroup(boss, 60))
	                return true;
	            return false;
	        };
	    }

        #endregion

	}

	#endregion

	#region Heroic Difficulty

    public class ShadowmoonBurialGroundsHeroic : ShadowmoonBurialGrounds
	{
		#region Overrides of Dungeon

		public override uint DungeonId
		{
			get { return 784; }
		}

		#endregion
	}

	#endregion
}