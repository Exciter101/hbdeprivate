using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bots.DungeonBuddy.Avoidance;
using Bots.DungeonBuddy.Enums;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.CommonBot.POI;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Bots.DungeonBuddy.Attributes;
using Bots.DungeonBuddy.Helpers;

// ReSharper disable CheckNamespace
namespace Bots.DungeonBuddy.DungeonScripts.WarlordsOfDraenor
// ReSharper restore CheckNamespace
{

    #region Normal Mode

    public class UpperBlackrockSpire : Dungeon
    {
        #region Overrides of Dungeon

        public override uint DungeonId
        {
            get { return 828; }
        }

        public override WoWPoint Entrance
        {
            get { return new WoWPoint(-7481.414, -1323.271, 301.3931); }
        }

        public override WoWPoint ExitLocation
        {
            get { return new WoWPoint(106.1491, -318.6653, 65.47925); }
        }

        public override void IncludeTargetsFilter(List<WoWObject> incomingObjects, HashSet<WoWObject> outgoingObjects)
        {
            var isleader = Me.IsLeader();
            foreach (var unit in incomingObjects.OfType<WoWUnit>())
            {
                if (unit.Entry == MobId_RallyingBanner && Me.IsDps())
                {
                    outgoingObjects.Add(unit);
                }
                else if (unit.Entry == MobId_SentryCannon && unit.IsHostile)
                {
                    outgoingObjects.Add(unit);
                }
                else if (isleader && MobsIds_hostileNeutralMobs.Contains(unit.Entry) && !Me.Combat && unit.DistanceSqr < 35 * 35 && unit.ZDiff < 10)
                {
                    outgoingObjects.Add(unit);
                }
            }
        }

        public override void WeighTargetsFilter(List<Targeting.TargetPriority> units)
        {
            var isDps = Me.IsDps();
            foreach (var p in units)
            {
                WoWUnit unit = p.Object.ToUnit();
                switch (unit.Entry)
                {
                    case 10442: // Chromatic Whelp. should be focused by DPS but left for last by tank.
                        p.Score += isDps ? 1200 : -1200;
                        break;
                    case MobId_RallyingBanner:
                    case MobId_DrakonidMonstrosity:
                    case MobId_DrakonidMonstrosityTrash:
                    case MobId_IronbarbSkyreaver:
                        if (isDps)
                            p.Score += 3500;
                        break;

                    case MobId_VilemawHatchling:
                        if (Me.IsRange())
                            p.Score += 3500;
                        break;
                }
            }
        }

        public override void RemoveTargetsFilter(List<WoWObject> units)
        {
            units.RemoveAll(
                o =>
                {
                    var unit = o as WoWUnit;
                    if (unit == null) return false;

                    if (unit.Entry == MobId_SentryCannon && !unit.IsHostile)
                        return true;

                    if (unit.Entry == MobId_VilemawHatchling && Me.IsMelee() && unit.ZDiff > 8)
                        return true;

                    return false;
                });
        }

        public override void OnEnter()
        {
            _tharbeksDoorBlackspot = new DynamicBlackspot(
                ShouldAvoidTharbeksDoor,
                () => _tharbeksDoorLoc,
                LfgDungeon.MapId,
                2,
                3,
                "Commander Tharbek", 
                true);
            DynamicBlackspotManager.AddBlackspot(_tharbeksDoorBlackspot);

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
            DynamicBlackspotManager.RemoveBlackspot(_tharbeksDoorBlackspot);
            _tharbeksDoorBlackspot = null;
        }

        private DynamicBlackspot _tharbeksDoorBlackspot;

        #endregion

        #region Root

        private const uint MobId_RuneGlow = 76396;
        private const uint MobId_SentryCannon = 76314;
        private const uint GameObjectId_RuneConduit = 226704;
        private const uint MobId_RallyingBanner = 76222;
        private const uint MobId_DrakonidMonstrosityTrash = 76018;
        private const uint MobId_BlackIronAlchemist = 76100;
        private const uint MobId_BlackIronVeteran = 77034;
        private const uint MobId_BlackIronEngineer = 76101;
        private const uint GameObjectId_TharbeksDoor = 164726;

        private bool _shouldAvoidTharbeksDoor;
        private readonly WaitTimer _tharbeksDoorTimer = new WaitTimer(TimeSpan.FromSeconds(2));
        readonly WoWPoint _tharbeksDoorLoc = new WoWPoint(106.7544, -421.1986, 110.9228);

        // these mobs showup as neutral but are really hostile.
        private readonly uint[] MobsIds_hostileNeutralMobs =
        {
            MobId_DrakonidMonstrosityTrash,
            MobId_BlackIronAlchemist,
            MobId_BlackIronVeteran,
            MobId_BlackIronEngineer
        };

        // I might add this to the profile system sometime...
        private readonly BossKillStage[] BossKillStages =
        {
            new BossKillStage("Orebender Gor'ashan", 2, 1),
            new BossKillStage("Kyrak", 2, 2),
            new BossKillStage("Commander Tharbek", 2, 3),
        };

        private readonly WaitTimer _updateBossKillStateTimer = WaitTimer.OneSecond;

        private LocalPlayer Me { get { return StyxWoW.Me; } }

        [EncounterHandler(0, "Root")]
        public Func<WoWUnit, Task<bool>> RootHandler()
        {
            return async npc =>
            {
                UpdateBossKillState();
                return false;
            };
        }

        // makes sure bosses are marked as dead if their stage and step is complete.
        private void UpdateBossKillState()
        {
            if (!_updateBossKillStateTimer.IsFinished)
                return;

            var stage = ScriptHelpers.CurrentScenarioInfo.CurrentStage;
            foreach (var entry in BossKillStages.Where(e => ScriptHelpers.IsBossAlive(e.Name)))
            {
                if (stage.StageNumber > entry.Stage 
                    || (stage.StageNumber == entry.Stage && stage.GetStep(entry.Step).IsComplete))
                {
                    ScriptHelpers.MarkBossAsDead(entry.Name, "Stage and step is complete");
                }
            }
            _updateBossKillStateTimer.Reset();
        }

        private bool ShouldAvoidTharbeksDoor()
        {
            if (_tharbeksDoorTimer.IsFinished)
            {
                var tharbeksDoor =
                    ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(g => g.Entry == GameObjectId_TharbeksDoor);

                _shouldAvoidTharbeksDoor = tharbeksDoor != null && !((WoWDoor)tharbeksDoor.SubObj).IsOpen;
                _tharbeksDoorTimer.Reset();
            }

            return _shouldAvoidTharbeksDoor;
        }

        #endregion


        [ScenarioStage(1, "Extinguish Runes")]
        public async Task<bool> StageOne(ScenarioStage stage)
        {
            if (BotPoi.Current.Type != PoiType.None || !Targeting.Instance.IsEmpty())
                return false;

            TreeRoot.StatusText = "Extinguishing Runes";
            if (BotPoi.Current.Type == PoiType.None && Me.IsLeader())
            {
                var rune =
                    ObjectManager.GetObjectsOfType<WoWUnit>()
                        .Where(u => u.Entry == MobId_RuneGlow && u.HasAura("Rune Glow"))
                        .OrderBy(u => u.DistanceSqr)
                        .FirstOrDefault();

                // should always be not null.
                if (rune != null)
                {
                    ScriptHelpers.SetLeaderMoveToPoi(rune.Location);
                }
            }

            // Idle if there is nothing to do as a tank.. (highly unlikely case)
            return Me.IsLeader();
        }

        #region Stage 2

        #region Gor'ashan

        private const uint MobId_LightningField = 76464;
        private const int AuraTriggerId_LodestoneSpike = 6164;

        [ScenarioStage(2, "Gor'ashan", 1)]
        public Func<ScenarioStage, Task<bool>> StageTwoStepOne()
        {
            var rightDoorEdge = new WoWPoint(174.7191, -258.7988, 91.54621);
            var leftDoorEdge = new WoWPoint(174.8173, -259.9115, 91.54621);

            var pointInsideRoom = new WoWPoint(164.4416, -262.2839, 91.54202);
            var randomPointInsideRoom = WoWMathHelper.GetRandomPointInCircle(pointInsideRoom, 3);

            return async stage =>
            {
                if (BotPoi.Current.Type != PoiType.None || Me.IsActuallyInCombat)
                    return false;

                var leader = ScriptHelpers.Leader;

                if (leader == null)
                    return false;

                // move inside room to avoid getting locked out.
                if (!leader.IsMe && leader.Location.IsPointLeftOfLine(leftDoorEdge, rightDoorEdge) &&
                    leader.Z > 85
                    && (!Me.Location.IsPointLeftOfLine(leftDoorEdge, rightDoorEdge) || Me.Z <= 85)
                    && await ScriptHelpers.MoveToContinue(() => randomPointInsideRoom))
                {
                    return true;
                }

                // move inside room to avoid getting locked out.
                if (Me.IsLeader() && Me.Location.IsPointLeftOfLine(leftDoorEdge, rightDoorEdge) && Me.Z > 85
                    &&
                    !ScriptHelpers.GroupMembers.All(
                        g => g.Location.IsPointLeftOfLine(leftDoorEdge, rightDoorEdge) && g.Location.Z > 85)
                    &&
                    WoWMathHelper.GetNearestPointOnLineSegment(Me.Location, leftDoorEdge, rightDoorEdge)
                        .DistanceSqr(Me.Location) > 6 * 6)
                {
                    TreeRoot.StatusText = "Waiting on group members to enter room";
                    return true;
                }

                TreeRoot.StatusText = "Clearing to Gor'ashan";

                var conduits = ObjectManager.GetObjectsOfType<WoWGameObject>()
                        .Where(u => u.Entry == GameObjectId_RuneConduit && u.CanUse())
                        .ToList();

                WoWGameObject conduit = null;

                if (Me.IsLeader())
                {
                    conduit = conduits.OrderBy(u => u.Location.DistanceSqr(pointInsideRoom)).FirstOrDefault();
                } 
                else if (conduits.Count > 1)
                {
                    // Decide which follower helps activate the conduits based on max health.. 
                    // this prevents multiple bots trying to activate same conduit 
                    var highestMaxHpFollower = ScriptHelpers.GroupMembers.Where(g => !g.IsTank)
                            .OrderByDescending(g => g.MaxHealth).FirstOrDefault();

                    if (highestMaxHpFollower != null && highestMaxHpFollower.Guid == Me.Guid)
                        conduit = conduits.OrderByDescending(u => u.Location.DistanceSqr(pointInsideRoom)).FirstOrDefault();
                }

                if (conduit != null)
                    ScriptHelpers.SetInteractPoi(conduit);

                return false;
            };
        }

        [EncounterHandler(76413, "Gor'ashan")]
        public Func<WoWUnit, Task<bool>> GorashanEncounter()
        {
            WoWGameObject conduit = null;
            // avoid these spikes
            AddAvoidObject(ctx => true, 5, o => o.Entry == AuraTriggerId_LodestoneSpike, ignoreIfBlocking: true);
            AddAvoidObject(
                ctx => true,
                12,
                o => o.Entry == MobId_LightningField,
                o => o.Location.RayCast(o.Rotation, 10));

            var bossLoc = new WoWPoint(144.5426, -258.0315, 96.32333);
            var randomPointAtBoss = WoWMathHelper.GetRandomPointInCircle(bossLoc, 5);

            return async boss =>
            {
                TreeRoot.StatusText = "Doing Gor'ashan boss encounter";
                var leader = ScriptHelpers.Leader;
                // having tank click the conduits seems to work best since they have larger hp pools thus can survive better
                // and dps can still continue dpsing.
                WoWPlayer conduitClicker = null;
                if (leader != null && leader.IsMe)
                {
                    conduitClicker = Me;
                }
                else if (leader == null || !leader.IsAlive)
                {
                    // if tank dies have a dps do the clicking.
                    conduitClicker =
                        ScriptHelpers.GroupMembers.Where(g => g.IsAlive && g.IsDps)
                            .OrderByDescending(g => g.MaxHealth)
                            .Select(g => g.Player)
                            .FirstOrDefault();
                }

                if (conduitClicker == Me)
                {
                    conduit = ObjectManager.GetObjectsOfType<WoWGameObject>()
                    .Where(u => u.Entry == GameObjectId_RuneConduit && u.CanUse())
                    .OrderBy(u => u.Location.DistanceSqr(bossLoc))
                    .FirstOrDefault();
                }

                if (ScriptHelpers.IsViable(conduit) &&
                    await ScriptHelpers.InteractWithObject(conduit, 3000, true))
                    return true;

                if (Me.IsRange()
                    && await ScriptHelpers.StayAtLocationWhile(
                        () => conduit == null && ScriptHelpers.IsViable(boss) && boss.Combat,
                        randomPointAtBoss,
                        "location near Gor'ashan"))
                {
                    return true;
                }

                return false;
            };
        }


        #endregion

        #region Kyrak

        [EncounterHandler(82556, "Drakonid Monstrosity")]
        [EncounterHandler(76018, "Drakonid Monstrosity")]
        public Func<WoWUnit, Task<bool>> DrakonidMonstrosityEncounter()
        {
            // These NPCs casts a spell that does damage to enemies in a line in front of it.
            AddAvoidObject(
                ctx => true,
                3,
                o =>
                    (o.Entry == MobId_DrakonidMonstrosity || o.Entry == MobId_DrakonidMonstrosityTrash)
                    && o.ToUnit().CastingSpellId == SpellId_Eruption,
                o => WoWMathHelper.GetNearestPointOnLineSegment(Me.Location, o.Location, o.Location.RayCast(o.Rotation, 20)));

            // non-tanks should stay away from the front of these NPCs since they have a cleave.
            AddAvoidObject(
                ctx => !Me.IsLeader(),
                5,
                o =>
                    (o.Entry == MobId_DrakonidMonstrosity || o.Entry == MobId_DrakonidMonstrosityTrash),
                o => o.Location.RayCast(o.Rotation, 4));

            return async npc => await ScriptHelpers.DispelEnemy(
                    "Rejuvenating Serum",
                    ScriptHelpers.EnemyDispelType.Magic,
                    npc);
        }

        private const uint MobId_DrakonidMonstrosity = 82556;
        private const uint AreaTriggerId_VilebloodSerum = 6823;
        private const int SpellId_Eruption = 155037;
        private const int SpellId_DebilitatingFixation = 161199;

        [EncounterHandler(76021, "Kyrak")]
        public Func<WoWUnit, Task<bool>> KyrakEncounter()
        {
            AddAvoidObject(ctx => true, 2.5f, AreaTriggerId_VilebloodSerum);

            return async boss =>
            {
                TreeRoot.StatusText = "Doing Kyrak boss encounter";

                if (await ScriptHelpers.DispelEnemy( "Rejuvenating Serum", ScriptHelpers.EnemyDispelType.Magic, boss)) 
                    return true;

                if (await ScriptHelpers.InterruptCast(boss, SpellId_DebilitatingFixation))
                    return true;

                return false;
            };
        }

        #endregion

        #region Commander Tharbek

        private const int MissileSpellId_NoxiousSpit = 161824;
        private const int MissileSpellId_ImbuedIronAxe = 162090;

        private const uint AreaTriggerId_NoxiousSpit = 6880;
        private const uint MobId_BlackIronSiegebreaker = 77033;
        private const uint MobId_IronbarbSkyreaver = 80098;
        private const uint MobId_CommanderTharbek = 79912;
        private const uint MobId_VilemawHatchling = 77096;
        private const uint MobId_ImbuedIronAxe = 80307;

        // handles challenging trash pulls
        [LocationHandler(112.2449, -312.1981, 106.4356, radius: 40)]
        public Func<WoWPoint, Task<bool>> TrashToTharbek()
        {
            var siegeBreakerWestPathEnd = new WoWPoint(151.4737, -270.8368, 110.9437);
            var siegeBreakerEastPathEnd = new WoWPoint(151.8634, -335.1172, 110.9524);

            var packByEastDoorLoc = new WoWPoint(140.1003, -299.1566, 110.9622);
            var packTwoLoc = new WoWPoint(161.4485, -317.8856, 110.9393);

            var losByEastDoorLoc = new WoWPoint(107.9661, -305.4395, 106.4356);
            var pullFromLoc = new WoWPoint(126.643, -312.1581, 110.9481);

            return async loc =>
            {
                var stage = ScriptHelpers.CurrentScenarioInfo.CurrentStage;

                if (stage.StageNumber != 2 || !stage.GetStep(2).IsComplete)
                    return false;

                if (Me.Z < 100)
                    return false;
                var roamingSiegeBreaker = ObjectManager.GetObjectsOfType<WoWUnit>()
                    .FirstOrDefault(
                        u =>
                            u.Entry == MobId_BlackIronSiegebreaker && u.IsAlive &&
                            WoWMathHelper.GetNearestPointOnLineSegment(
                                u.Location,
                                siegeBreakerWestPathEnd,
                                siegeBreakerEastPathEnd).DistanceSqr(u.Location) < 5 * 5);

                // pull the pack just to the left side of the east doorway.
                var packByEastDoor =
                    ScriptHelpers.GetUnfriendlyNpsAtLocation(packByEastDoorLoc, 7).FirstOrDefault();
                if (await ScriptHelpers.PullNpcToLocation(
                    () => packByEastDoor != null,
                    () =>
                        roamingSiegeBreaker == null ||
                        roamingSiegeBreaker.Location.DistanceSqr(packByEastDoor.Location) > 30 * 30,
                    packByEastDoor,
                    losByEastDoorLoc,
                    loc,
                    5000,
                    waitAtLocationRadius:3))
                {
                    return true;
                }

                // pack across the room of the east door entrance
                var packTwo = ScriptHelpers.GetUnfriendlyNpsAtLocation(packTwoLoc, 7).FirstOrDefault();
                if (await ScriptHelpers.PullNpcToLocation(
                    () => packTwo != null,
                    () =>
                        roamingSiegeBreaker == null ||
                        roamingSiegeBreaker.Location.DistanceSqr(packTwo.Location) > 30 * 30,
                    packTwo,
                    losByEastDoorLoc,
                    Me.IsLeader() ? pullFromLoc : loc,
                    5000,
                    waitAtLocationRadius: 3))
                {
                    return true;
                }

                // Finally, pull the roaming Siege Breaker
                if (await ScriptHelpers.PullNpcToLocation(
                    () => roamingSiegeBreaker != null,
                    () => roamingSiegeBreaker.Location.DistanceSqr(packTwoLoc) < 25 * 25,
                    roamingSiegeBreaker,
                    pullFromLoc,
                    Me.IsLeader() ? pullFromLoc : loc,
                    5000,
                    waitAtLocationRadius: 3))
                {
                    return true;
                }

                return false;
            };
        }


        [EncounterHandler(79912, "Commander Tharbek", Mode = CallBehaviorMode.CurrentBoss)]
        public Func<WoWUnit, Task<bool>> CommanderTharbekStartEncounter()
        {
            var tankSpot = new WoWPoint(169.1132, -419.7492, 110.4723);

            return async boss =>
            {
                if (!Me.IsLeader())
                    return false;
                
                if (ScriptHelpers.CurrentScenarioInfo.CurrentStageNumber != 2
                    || ScriptHelpers.CurrentScenarioInfo.CurrentStage.GetStep(3).IsComplete)
                {
                    return false;
                }

                if (Me.Location.DistanceSqr(tankSpot) > 30*30)
                    return false;

                if (Tharbek != null && Tharbek.Combat)
                    return false;

                return await ScriptHelpers.StayAtLocationWhile(() => Tharbek == null || !Tharbek.Combat, tankSpot, precision:15);
            };
        }

        private PerFrameCachedValue<WoWUnit> _tharbek;

        private WoWUnit Tharbek
        {
            get
            {
                return _tharbek ??
                       (_tharbek = new PerFrameCachedValue<WoWUnit>(
                           () => ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == MobId_CommanderTharbek)));
            }
        }

        [EncounterHandler(79912, "Commander Tharbek")]
        public Func<WoWUnit, Task<bool>> CommanderTharbekEncounter()
        {
            AddAvoidLocation(
                ctx => true,
                8,
                o => ((WoWMissile) o).ImpactPosition,
                () => WoWMissile.InFlightMissiles.Where(m => m.SpellId == MissileSpellId_ImbuedIronAxe));

            AddAvoidObject(ctx => true, 8, MobId_ImbuedIronAxe);

            return async boss => { return false; };
        }

        [EncounterHandler(80098, "Ironbarb Skyreaver")]
        public Func<WoWUnit, Task<bool>> IronbarbSkyreaverEncounter()
        {
            AddAvoidLocation(
                ctx => true,
                6,
                o => ((WoWMissile)o).ImpactPosition,
                () => WoWMissile.InFlightMissiles.Where(m => m.SpellId == MissileSpellId_NoxiousSpit));

            AddAvoidObject(ctx => true, 6, AreaTriggerId_NoxiousSpit);

            // his breath is very directional and deadly
            AddAvoidObject(
                ctx => true,
                7,
                o => o.Entry == MobId_IronbarbSkyreaver && o.ToUnit().HasAura("Incinerating Breath"),
                o => WoWMathHelper.GetNearestPointOnLineSegment(Me.Location, o.Location, o.Location.RayCast(o.Rotation, 30)));

            return async boss => false;
        }

        #endregion

        private class BossKillStage
        {
            public BossKillStage(string name, int stage, int step)
            {
                Name = name;
                Stage = stage;
                Step = step;
            }

            public int Stage { get; private set; }
            public int Step { get; private set; }
            public string Name { get; private set; }
        }
    }

        #endregion

    #endregion

    public class UpperBlackrockSpireHeroic : UpperBlackrockSpire
    {
        #region Overrides of Dungeon

        public override uint DungeonId
        {
            get { return 860; }
        }
        #endregion
    }
}