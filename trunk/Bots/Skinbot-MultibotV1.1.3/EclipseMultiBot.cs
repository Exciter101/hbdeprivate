using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Levelbot.Actions.Combat;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using Sequence = Styx.TreeSharp.Sequence;
using Styx.WoWInternals;

using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Styx;
using System;
using CommonBehaviors.Actions;
using Eclipse.WoWDatabase;
using Eclipse.WoWDatabase.Models;
using Eclipse.MultiBot;
namespace Eclipse.Bots.MultiBot
{
    public class EclipseMultiBot : BotBase
    {
        #region Overrides
        private static LocalPlayer Me;
        private static WoWPlayer Leader;
        private EclipseConfigForm _gui;
        private bool _isInit = false;
        private Composite _root;
        private static Location loc;
        private static WoWGuid targetGuid; //For Blacklisting things that stick around...
        private static int SkinningAttempts;
        public override string Name
        {
            get { return "Eclipse - MultiBot 1.1"; }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        public override Form ConfigurationForm
        {
            get
            {
                if (_gui == null || _gui.IsDisposed) _gui = new EclipseConfigForm();
                return _gui;
            }
        }

        public override void Start()
        {
            if (Me == null) Me = StyxWoW.Me;
            if (!Core.init) Core.Initialize();
        }

        public override void Stop()
        {
            EC.log("Stop Called");
            base.Stop();
        }

        public override void Pulse()
        {
            //Core.Pulse();
        }

        public override void Initialize()
        {
            try
            {
                if (!_isInit)
                {
                    if (!Core.init)
                    Core.Initialize();
                }
            }
            catch (Exception ex)
            {
                Logging.Write(Colors.Red, ex.ToString());
            }
        }
        public override Composite Root
        {
            get
            {
                return _root ?? (_root =
                    new PrioritySelector(

                        new Decorator(ret=> Core.ForceNav,  NavBehavior()),
                        new Decorator(ret => !Core.PassiveMode && !StyxWoW.Me.IsGhost, 
                            new PrioritySelector(
                                CreateCombatBehavior(),
                                CreateWaitBehavior(),
                                CreateFollowBehavior(),
                                new Decorator(ret => Core.BagsFull && FindNearestVendor(), NavBehavior()),  
                                new Decorator(ret => Core.SkinMode || Core.KillThese, CreatePatrolBehavior()),
                                new Decorator(ret => Core.QuestingMode, CreateQuestBehavior()),
                                new Decorator(ret => StyxWoW.Me.IsGhost || Me.IsDead, CreateDeadBehavior())    
                            )
                        ),
                        new Sequence(
                            LearningBehavior(),
                            new ActionAlwaysSucceed()
                        )  
                ));
            }
        }

        private Composite CreateQuestBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => GetQuestGiver() && Me.CurrentTarget != null,
                   new Decorator(ret => Me.CurrentTarget.IsAlive,
                       new PrioritySelector(
                           new Decorator(ret => Me.CurrentTarget.Distance <= Me.InteractRange,                    
                               new Sequence(
                                    new Action(r => Me.CurrentTarget.Interact()),
                                    new Action(r => TreeRoot.StatusText = String.Format("Getting Quest from {0}", Me.CurrentTarget.Name))
                                )),
                           new Decorator(ret => Me.CurrentTarget.Distance > Me.InteractRange,
                               new Sequence(
                                   new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} to get a q.", Me.CurrentTarget.Name)),
                                   //new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                   //    new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))),
                                   //new Decorator(ret => !SpellManager.HasSpell("Flight Master's License"),
                                       new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location)),
                                   new ActionAlwaysSucceed()
                                )
                            )
                        )
                    )
                ),
                new Decorator(ret => !GetQuestGiver() && FindClosestQuestLocation(), 
                    new Sequence (
                        new Action()
                        ))
            );
        }

        private bool FindClosestQuestLocation()
        {
            if (Core.Quests.Count > 0)
            {

            }
            return true;
        }

        private bool GetQuestGiver()
        {
            var questgivers = ObjectManager.ObjectList.Where(n =>
                n.Type == WoWObjectType.Unit
                && !EC.IsUnitBlackListed((WoWUnit)n)
                && ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.Available
                && ((WoWUnit)n).IsFriendly
            ).OrderBy(m => m.Distance).ToList();


            if (questgivers.Count > 0)
            {
                var mob = (WoWUnit)questgivers.OrderBy(m => m.Distance).FirstOrDefault();
                EC.log("Found a quest giver: "+mob.Name);
                mob.Target();

                return true;
            }

            EC.log("No q givers around.");
            return false;
        }

        #endregion
        
        #region LearningBehavior
        private Composite LearningBehavior()
        {
            return new PrioritySelector(
                new Action(r=>Core.Pulse()),
                new ActionAlwaysSucceed()
                );
        }
        #endregion

        #region Waiting Behavior
        private Composite CreateWaitBehavior()
        {
            return new PrioritySelector(
                // Wait on transport
                new Decorator(ret => Me.IsOnTransport,
                    new Sequence(
                        new Action(ret => TreeRoot.StatusText = "Flying on transport"),
                        new WaitContinue(5, ret => Me.IsOnTransport, new ActionAlwaysSucceed()),
                            new ActionAlwaysFail() // if we are still on transport after 5 seconds wait again
                    )
                ),
                // Wait on group members to catch  up
                new Decorator(ret => EC.PartyMode && !GroupAssembled(40),
                    new Sequence(
                        new Action(ret => TreeRoot.StatusText = "Waiting on Party Members"),
                        new WaitContinue(5, ret => !GroupAssembled(EC.PartyDistance), new ActionAlwaysSucceed()),
                        new ActionAlwaysFail()
                    )
                )

            );
        }

        #endregion

        #region Professions
        private static bool TargetClosestSkinnableMob()
        {
            var mobs = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n)).OrderBy(m => m.Distance).ToList();
            if (Core.SkinList.Count > 0) mobs = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n) && IsUnitOnSkinList((WoWUnit)n)).OrderBy(m => m.Distance).ToList();
            else mobs = ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n)).OrderBy(m => m.Distance).ToList();
            if (mobs.Count > 0)
            {
                foreach (WoWUnit mob in mobs)
                {
                    if (mob.Guid == targetGuid && SkinningAttempts > 3)
                    {
                        EC.log("Adding mob to blacklist.");
                        EC.AddMobToBlackList(mob);
                        continue;
                    }
                    //this could be done better - but i dont feel like it
                    var skinMob = false;
                    if (Core.NinjaSkin && !mob.TaggedByMe) skinMob = true;
                    if (Core.NinjaSkin && mob.TaggedByMe || Core.NinjaSkin && mob.TaggedByOther) skinMob = true;
                    if (!Core.NinjaSkin && !mob.TaggedByMe) skinMob = false;

                    if (skinMob && mob.CanSkin && mob.Skinnable )
                    {
                        targetGuid = mob.Guid;
                        EC.log(string.Format("Targeting dead skinnable mob {0}", mob.Name));
                        mob.Target();
                        return true;
                    }

                    var m = Core.MOBs.Where(cm => cm.isSkinnable && cm.Entry == mob.Entry && Core.IgnoreList.Where(i => i.Entry == cm.Entry).FirstOrDefault() == null).FirstOrDefault();
                    if (m != null)
                    {
                        if (mob.Guid == targetGuid) Core.log("We recognize thisone." + SkinningAttempts.ToString());
                        targetGuid = mob.Guid;    
                        EC.log(string.Format("Found a mob that can be killed/skinned/looted: {0}", m.Name));
                        SkinningAttempts = 0;
                        mob.Target();
                        return true;
                    }
                }
            }

            EC.log("No skinnable mob found.");
            return false;

        }
        public static bool IsUnitOnSkinList(WoWUnit mob)
        {

            if (Core.SkinList.Where(m => m.Name == mob.Name).Count() > 0)
            {
                return true;
            }
            else return false;
        }
        private static WoWUnit TargetClosestLootableMob()
        {
            var mob = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && !EC.IsUnitBlackListed((WoWUnit)n) && ((WoWUnit)n).Lootable).OrderBy(m => m.Distance).FirstOrDefault();
            if (mob != null)
            {
                EC.log("Found a lootable mob");
                mob.Target();
                return mob;
            }
            else return null;
        }
        public static Location GetNextSkinningLocation_notWorking()
        {
            EC.log(string.Format("There are {0} known hotspots for zone {1} ({2}) of which {3} have been recently visited.", Core.Locations.Where(l => l.Zone == Me.ZoneId).Count(), Me.ZoneText, Me.ZoneId, Core.RecentlyVisitedLocations.Count()));
            var _loc = Core.Locations.Where(l => l.Zone == Me.ZoneId).FirstOrDefault();
            EC.log(string.Format("Have a loc {0},{1},{2},", _loc.X, _loc.Y,+loc.Z));
            if (_loc == null)
            {
                EC.log("Found " + Core.Locations.Where(l => l.Zone == Me.ZoneId).ToList().OrderBy(d => Core.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z })).Count() + " viable locations");
                return null;
            }
            else
            {
                var nearby = (Me.X - loc.X) * 2 + (Me.Y - loc.Y) * 2 < 10 * 2;
                if (nearby)
                {
                    EC.log("point is within 10 yards of me.");
                }
                if (nearby)
                {
                    EC.log("Closest location is where we already are - adding to visited.");
                    Core.RecentlyVisitedLocations.Add(_loc);
                    return GetNextLocation();
                }
                else
                {
                    EC.log("Patting to new loc.");
                    loc = _loc;
                    return loc;
                }
            }

        }
        #endregion

        #region Items
        public bool BagsFull() {
            if (StyxWoW.Me.Inventory.FreeSlots == 0) return true;
            else return false;
        }
        #endregion

        #region Navigation

        public static Composite CreateDeadBehavior() //this is commmunity contributed content I assume from  FPSWare's RAF bot. If that IS the case than THANK YOU FPSWare!
        {
            return new PrioritySelector( 

                // Mount up - for rez sickness wait
                //new Decorator(ret => !Me.IsDead && !Me.IsGhost && !Me.Mounted && Me.HasAura(15007) && Flightor.MountHelper.CanMount, Common.MountUpFlying()),

                // Mounted? The ascend and just wait out rez sickness
                new Decorator(ret =>  Me.Mounted && !Me.IsFlying && !StyxWoW.Me.MovementInfo.IsAscending,
                    new Sequence(
                        new Action(context => EC.log("Flying up to wait out rez sickness")),
                        new Action(context => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromSeconds(4)))
                    )),

                // Just wait out rez sickness
                new Decorator(ret => Me.IsFlying && Me.HasAura(15007), new Action(ctx => { TreeRoot.StatusText = "Waiting out rez sickness"; TreeRoot.StatusText = "Waiting out rez sickness"; return RunStatus.Success; })),

                // Release body
                new Decorator(ret => Me.IsDead && !Me.IsGhost,
                    new Sequence(
                        new Action(context => EC.log("We're dead! Releasing corpse")),
                        new Action(context => Lua.DoString("RepopMe()"))
                        )),

                // Try to move to our corpse - if we can
                new Decorator(ret => Me.IsGhost && (Navigator.CanNavigateFully(Me.Location, Me.CorpsePoint) || Navigator.CanNavigateFully(Me.Location, WoWMathHelper.CalculatePointFrom(Me.Location, Me.CorpsePoint, 10))),
                    new PrioritySelector(

                        // Move to the location of our corpse
                // First, try to move to our corpse location exactly
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) > 15 && Navigator.CanNavigateFully(Me.Location, Me.CorpsePoint), new Action(context => Navigator.MoveTo(Me.CorpsePoint))),

                        // If that fails try to move within 10 yards of it
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) > 15 && Navigator.CanNavigateFully(Me.Location, WoWMathHelper.CalculatePointFrom(Me.Location, Me.CorpsePoint, 10)), new Action(context => Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(Me.Location, Me.CorpsePoint, 10)))),

                        // Within range of our body? Retrieve our body
                        new Decorator(ret => Me.Location.Distance(Me.CorpsePoint) < 15,
                            new Sequence(
                                new Action(context => EC.log("Recovering our body")),
                                new Action(context => Lua.DoString("RetrieveCorpse()"))
                        ))
                    ))


                );
        }
        public static Composite NavBehavior()
        {
            return new PrioritySelector(                                
                new Decorator(ret => Core.ForceNavLocation != null && !AtMyDestination(),
                    new Sequence(

                        new Action(r => Styx.Pathing.Flightor.MoveTo(new WoWPoint(Core.ForceNavLocation.X, Core.ForceNavLocation.Y, Core.ForceNavLocation.Z))),
                        new ActionAlwaysFail()
                        )
                    
                    ));
        }
        private bool AtCorpseLocation()
        {
            var distance = Core.Distance(new float[3] { Me.X, Me.Y, Me.Z }, new float[3] { Me.CorpsePoint.X, Me.CorpsePoint.Y, Me.CorpsePoint.Z });
            if (distance > 10)
            {
                TreeRoot.StatusText = string.Format("within {0} of my corpse.", distance);
                return false;
            }
            else
            {
                EC.log("Corpse reached cancelling nav.");
                return true;
            }
        }
        private static bool AtMyDestination()
        {
            var distance = Core.Distance(new float[3] { Me.X, Me.Y, Me.Z }, new float[3] { Core.ForceNavLocation.X, Core.ForceNavLocation.Y, Core.ForceNavLocation.Z });
            if (distance > 5)
            {
                TreeRoot.StatusText = string.Format("within {0} of my distination.", distance);
                return false;
            }
            else
            {
                EC.log("Destination reached cancelling nav.");
                Core.ForceNav = false;
                Core.ForceNavLocation = null;
                return true;
            }
        }
        public static Location GetNextLocation()
        {
            //ToDo: dont revisit recently visited places.
            List<Location> _locList = Core.Locations.Where(l => l.Zone == Me.ZoneId && !Core.RecentlyVisitedLocations.Contains(l)).OrderBy(d => Core.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z })).ToList();
            if (Core.SkinMode)
            {
                foreach (Mob mob in Core.MOBs.Where(m=>m.isSkinnable)){
                    var locs = Core.Locations.Where(l => l.Entry == mob.Entry && l.Zone == Me.ZoneId).ToList();
                    if (locs != null)
                    {
                        if (locs.Count > 0) _locList.AddRange(locs);
                    }

                }
            }
            foreach (var _loc in _locList)
            {
                var distance = Core.Distance(new float[3] { _loc.X, _loc.Y, _loc.Z }, new float[3] { Me.X, Me.Y, Me.Z });
                //if (_loc != null) EC.log("Found a location!");
                if (distance < 40)
                {
                    EC.log(string.Format("within {0} of a hotspot- and thers still no mobs here, so we are gonna add this to recently visited.", distance));
                    Core.RecentlyVisitedLocations.Add(_loc);
                    //GetNextSkinningLocation();
                    loc = null;
                }
                else
                {

                    TreeRoot.StatusText = string.Format("Found {0} locations ({2} visited) in {1}", Core.Locations.Where(l => l.Zone == Me.ZoneId).Count(), Core.RecentlyVisitedLocations.Count(), Me.ZoneId);
                    loc = _loc;
                    return null;
                }

            }
            if (loc == null)
            {
                Core.RecentlyVisitedLocations.Clear();
                EC.log("No more saved locations to visit- cleared recent locations.");

                loc = null;
            }
            return loc;
        }
        private static bool FindNearestVendor(){
            var npc = Core.NPCs.Where(n=>n.isVendor).OrderBy(d => Core.Distance(new float[3] { d.X, d.Y, d.Z }, new float[3] { Me.X, Me.Y, Me.Z })).FirstOrDefault();
            if (npc != null)
            {
                TreeRoot.StatusText = "(FullBags) Navigating to Vendor...";
                var loc = new Location { Entry = npc.Entry, Name = npc.Name, X = npc.X, Y = npc.Y, Z = npc.Z, Zone = npc.Zone };
                Core.ForceNav = true;
                Core.ForceNavLocation = loc;
                return true;
            }
            else if (npc == null){
                var nearbyvendors = ObjectManager.ObjectList.Where(w => w.Type == WoWObjectType.Unit).ToList().Where(m => ((WoWUnit)m).IsFriendly && ((WoWUnit)m).IsVendor).OrderBy(d => d.Distance).ToList();
                foreach (var vendor in nearbyvendors){
                    Core.ProcessUnit((WoWUnit)vendor);
                }
                if (nearbyvendors.Count > 0)
                {
                    var closestVendor = nearbyvendors.FirstOrDefault();
                    TreeRoot.StatusText = "(FullBags) Navigating to Vendor...";
                    var loc = new Location { Entry = closestVendor.Entry, Name = closestVendor.Name, X = closestVendor.X, Y = closestVendor.Y, Z = closestVendor.Z };
                    Core.ForceNav = true;
                    Core.ForceNavLocation = loc;
                    return true;
                }
            }
            else
            {
                EC.log("Bags are full and there is no Vendor!");
                return false;
            }
            return false;
        }
        #endregion

        #region Patrol Behavior
        private static bool TargetClosestMob()
        {
            var mobs = ObjectManager.ObjectList.Where(n =>
                n.Type == WoWObjectType.Unit 
                && !EC.IsUnitBlackListed((WoWUnit)n) 
                && !((WoWUnit)n).TaggedByOther 
                && !((WoWUnit)n).IsFriendly
            ).OrderBy(m => m.Distance).ToList();


           if (mobs.Count > 0)
           {
               foreach (WoWUnit mob in mobs)
               {
                   //this could be done better - but i dont feel like it
                   var m = Core.KillList.Where(cm => cm.Entry == mob.Entry).FirstOrDefault();
                   if (m != null && mob.IsAlive || mob.Lootable)
                   {
                        EC.log(string.Format("Found a mob that can be killed/looted: {0}", m.Name));
                        mob.Target();
                        return true;
                   }
               }
           }

           EC.log("No mobs from our kill list around.");
           return false;
           
        }
        private static Composite CreatePatrolBehavior()
        {
            //new Decorator(ret=> SpellManager.Cast("Skinning"),
            return new PrioritySelector(
                new Decorator(ret => EC.FarmMode && !Me.IsActuallyInCombat,
                    new PrioritySelector(
                        new Decorator(ret => Core.SkinMode,
                            new PrioritySelector(
                                new Decorator(ret => Me.CurrentTarget == null && !TargetClosestSkinnableMob(),
                                    new Sequence(
                                        new Action(r => GetNextLocation()),
                                        new Decorator(ret => loc != null, new Action(r => Flightor.MoveTo(new WoWPoint(loc.X, loc.Y, loc.Z)))),
                                        new ActionAlwaysSucceed()
                                    )),
                                new Decorator(ret => Me.CurrentTarget != null || TargetClosestSkinnableMob(),
                                   CreateSkinningBehavior()
                                )
                            )
                        ),
                        new Decorator(ret => Core.KillThese,
                            new PrioritySelector(
                                 new Decorator(ret => Me.CurrentTarget == null && !TargetClosestMob(),
                                    new Sequence(
                                        new Action(r => GetNextLocation()),
                                        new Decorator(ret => loc != null,
                                            new Action(r => Flightor.MoveTo(new WoWPoint(loc.X, loc.Y, loc.Z)))),
                                            new ActionAlwaysSucceed()
                                    )),
                                new Decorator(ret=> Me.CurrentTarget != null || TargetClosestMob(),
                                   CreateKillTheseBehavior()
                                )
                            )
                        )
                    )
                )
            );

        }
        private static Composite CreateSkinningBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret=> StyxWoW.Me.CurrentTarget != null,
                    new PrioritySelector(
                   new Decorator(ret => Me.CurrentTarget.IsAlive,
                       new PrioritySelector(
                           new Decorator(ret => Me.CurrentTarget.Distance <= 40 && RoutineManager.Current.PullBehavior != null, RoutineManager.Current.CombatBehavior),
                           new Decorator(ret => Me.CurrentTarget.Distance > 40 && RoutineManager.Current.PullBehavior != null,
                               new Sequence(
                                   new Action(r => EC.log("MoveToUnit")),
                                   new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} to kill it and wear it's skin.", Me.CurrentTarget.Name)),
                                   new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                       new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))),
                                   new Decorator(ret => !SpellManager.HasSpell("Flight Master's License"),
                                       new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
                                   new ActionAlwaysSucceed())
                           )
                       )),
                   new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && SanityCheck(),
                       new Sequence(
                           new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} for some epic skinning action.", Me.CurrentTarget.Name)),
                                   new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                       new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location)))
                           )
                       ),
                   new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.CanLoot,
                       new Sequence(
                           new Action(r => Me.CurrentTarget.Interact()),
                           new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Name))
                       )),
                   new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && isSkinnable(Me.CurrentTarget),
                       new Sequence(
                           new Action(r => Me.CurrentTarget.Interact()),
                           new Action(r=> SkinningAttempts++),
                           new Action(r => TreeRoot.StatusText = String.Format("Skinning {0}", Me.CurrentTarget.Name))
                       )),
                   new Decorator(r => Me.IsFlying && Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange,
                       new Sequence(
                           new Action(context => Lua.DoString("Dismount()")),
                           new Wait(4, new ActionAlwaysSucceed()))),
                   new Decorator(r => isSkinnable(Me.CurrentTarget) && !Me.IsFlying,
                       new Sequence(
                           new Action(r => Me.CurrentTarget.Interact()),
                           new Action(r => TreeRoot.StatusText = String.Format("Skinning {0}", Me.CurrentTarget.Name)),
                           new Wait(4, new ActionAlwaysSucceed())
                       )
                   )
               )
           ));
        }

        private static bool SanityCheck()
        {
            if (SkinningAttempts > 5)
            {
                EC.log("Failed Sanity Check.");
                SkinningAttempts = 0;
                EC.AddMobToBlackList(Me.CurrentTarget);
                Me.ClearTarget();
                return false;
            }
            return true;
        }
        private static bool canIKillForSkin(WoWUnit woWUnit)
        {
            var skinnable = false;
            var t = woWUnit;
            if (t.IsDead && t.Skinnable) return true;
            var mob = Core.MOBs.Where(n => n.Entry == t.Entry).FirstOrDefault();
            if (mob != null)
            {
                if (isSkinnable(t) && t.Level > 1) skinnable = true;
            }
            return skinnable;
        }
        private static bool isSkinnable(WoWUnit woWUnit)
        {
            var t = woWUnit;
            Me = StyxWoW.Me;
            if (Me.CurrentTarget != null
                && t.IsDead
                && !t.Lootable
                && t.Skinnable
                && t.CanSkin
                && !Me.IsCasting
                && !Me.IsChanneling
                && !Me.Combat
                && !Me.Looting
                && t.Distance <= Me.InteractRange
            )
                return true;
            else return false;
        }
        private static Composite CreateKillTheseBehavior()
        {
            return new PrioritySelector(
               new Decorator(ret => Me.CurrentTarget.IsAlive,
                   new PrioritySelector(
                       new Decorator(ret => Me.CurrentTarget.Distance <= 40 && RoutineManager.Current.PullBehavior != null, RoutineManager.Current.CombatBehavior),
                       new Decorator(ret => Me.CurrentTarget.Distance > 40 && RoutineManager.Current.PullBehavior != null,
                           new Sequence(
                               new Action(r => EC.log("MoveToUnit")),
                               new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} to kill it for sport.", Me.CurrentTarget.Name)),
                               new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                   new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))),
                               new Decorator(ret => !SpellManager.HasSpell("Flight Master's License"),
                                   new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
                               new ActionAlwaysSucceed())
                       )
                   )),
               new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && Me.CurrentTarget.Lootable,
                   new Sequence(
                       new Action(r => TreeRoot.StatusText = String.Format("Moving to {0} search for bloody corpse trinkets.", Me.CurrentTarget.Name)),
                               new Decorator(ret => SpellManager.HasSpell("Flight Master's License"),
                                   new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))),
                               new Decorator(ret => !SpellManager.HasSpell("Flight Master's License"),
                                   new Action(r => Navigator.MoveTo(Me.CurrentTarget.Location))),
                                   new ActionAlwaysSucceed()
                       )
                   ),
               new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.Lootable,
                   new Sequence(
                       new Action(r => Me.CurrentTarget.Interact()),
                       new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Name)),
                       new ActionAlwaysSucceed()
                   )),
                new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && !Me.CurrentTarget.Lootable,
                   new Sequence(
                       new Action(r => Me.ClearTarget()),
                       new ActionAlwaysSucceed()
                   ))
           );
        }
        #endregion

        #region Combat Behavior

        private static bool NeedPull(object context)
        {
            var target = StyxWoW.Me.CurrentTarget;

            if (target == null)
                return false;

            if (!target.InLineOfSight)
                return false;

            if (target.Distance > Targeting.PullDistance)
                return false;

            return true;
        }

        private static Composite CreateCombatBehavior()
        {
            return new PrioritySelector(

                new Decorator(ret => !StyxWoW.Me.Combat,
                    new PrioritySelector(

            #region Rest

new PrioritySelector(
                // Use the bt
                        new Decorator(ctx => RoutineManager.Current.RestBehavior != null,
                            RoutineManager.Current.RestBehavior),

                            // new ActionDebugString("[Combat] Rest -> Old Behavior"),
                // don't use the bt
                            new Decorator(ctx => RoutineManager.Current.NeedRest,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Resting"),
                                    new Action(ret => RoutineManager.Current.Rest())))
                                    ),

            #endregion

            #region PreCombatBuffs

 new PrioritySelector(
                // new ActionDebugString("[Combat] Checking PCBBehavior"),
                // Use the bt
                            new Decorator(ctx => RoutineManager.Current.PreCombatBuffBehavior != null,
                                RoutineManager.Current.PreCombatBuffBehavior),

                            // don't use the bt
                // new ActionDebugString("[Combat] Checking PCBOld"),
                            new Decorator(
                                ctx => RoutineManager.Current.NeedPreCombatBuffs,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Applying pre-combat buffs"),
                                    new Action(ret => RoutineManager.Current.PreCombatBuff())
                                    ))),

            #endregion

            #region Pull

                // new ActionDebugString("[Combat] Pull"),
                // Don't pull, unless we've decided to pull already.
                        new Decorator(ret => BotPoi.Current.Type == PoiType.Kill,
                            new PrioritySelector(

                                // Make sure we have a valid target list.
                                new Decorator(ret => Targeting.Instance.TargetList.Count != 0,
                // Force the 'correct' POI to be our target.
                                    new Decorator(ret => BotPoi.Current.AsObject != Targeting.Instance.FirstUnit &&
                                        BotPoi.Current.Type == PoiType.Kill,
                                        new Sequence(

                                            new Action(ret => BotPoi.Current = new BotPoi(Targeting.Instance.FirstUnit, PoiType.Kill)),
                                            new Action(ret => BotPoi.Current.AsObject.ToUnit().Target())))),

                                        new Decorator(NeedPull,
                                            new PrioritySelector(
                                                new Decorator(ctx => RoutineManager.Current.PullBuffBehavior != null,
                                                    RoutineManager.Current.PullBuffBehavior),

                                                new Decorator(ctx => RoutineManager.Current.PullBehavior != null,
                                                    RoutineManager.Current.PullBehavior),

                                                    new ActionPull())))))),
            #endregion

 new Decorator(ret => StyxWoW.Me.Combat,

                    new PrioritySelector(

            #region Heal

new PrioritySelector(
                // Use the Behavior
                            new Decorator(ctx => RoutineManager.Current.HealBehavior != null,
                                new Sequence(
                                    RoutineManager.Current.HealBehavior,
                                    new Action(delegate { return RunStatus.Success; })
                                    )),

                            // Don't use the Behavior
                            new Decorator(ctx => RoutineManager.Current.NeedHeal,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Healing"),
                                    new Action(ret => RoutineManager.Current.Heal())
                                    ))),

            #endregion

            #region Combat Buffs

 new PrioritySelector(
                // Use the Behavior
                            new Decorator(ctx => RoutineManager.Current.CombatBuffBehavior != null,
                                        new Sequence(
                                            RoutineManager.Current.CombatBuffBehavior,
                                            new Action(delegate { return RunStatus.Success; })
                                            )
                                ),

                            // Don't use the Behavior
                            new Decorator(ctx => RoutineManager.Current.NeedCombatBuffs,
                                        new Sequence(
                                            new Action(ret => TreeRoot.StatusText = "Applying Combat Buffs"),
                                            new Action(ret => RoutineManager.Current.CombatBuff())
                                            ))),

            #endregion

            #region Combat

 new PrioritySelector(
                // Use the Behavior
                            new Decorator(ctx => RoutineManager.Current.CombatBehavior != null,
                                new PrioritySelector(
                                    RoutineManager.Current.CombatBehavior,
                                    new Action(delegate { return RunStatus.Success; })
                                    )),

                            // Don't use the Behavior
                            new Sequence(
                                new Action(ret => TreeRoot.StatusText = "Combat"),
                                new Action(ret => RoutineManager.Current.Combat())))

            #endregion

)));
        }

        #endregion

        #region Follow Behavior

        private static Composite CreateFollowBehavior()
        {
            return new PrioritySelector(
                WhoDoIFollow(),
                new Decorator(ret => StyxWoW.Me.GroupInfo.IsInParty && (EC.FollowTarget != null && EC.FollowTarget.Distance > 20 || EC.FollowTarget != null && !EC.FollowTarget.InLineOfSight),
                    new Action(ret => Navigator.MoveTo(EC.FollowTarget.Location))
                )

            );
        }
        private static Composite WhoDoIFollow()
        {
            return new PrioritySelector(
                new Decorator(ii => Me.IsInInstance && Me.GroupInfo.PartySize > 1,
                    new PrioritySelector(
                        new Decorator(ctx => !Me.IsGroupLeader,
                            new Sequence(
                                new Action(ret => EC.FollowTarget = Me.GroupInfo.GroupLeader)
                            )
                        ),
                        new Decorator(ctx => Me.IsGroupLeader,
                            new Action(ret => EC.FollowTarget = null))

                    )
                )
            );

            //if (Me.IsInInstance && !Me.IsGroupLeader && Me.GroupInfo.PartySize > 1)
            //{
            //    return Me.GroupInfo.GroupLeader;
            //}
            //else if (Me.IsInInstance && Me.IsGroupLeader) return null;
        }
        #endregion

        #region Group Behaviors
        private bool GroupAssembled(float Distance)
        {
            var _assembled = true;
            if (Me.GroupInfo.PartySize > 1 && Me.IsGroupLeader)
            {
                foreach (var dude in Me.GroupInfo.PartyMembers.ToList())
                {
                    var pm = dude.ToPlayer();
                    if (!dude.IsOnline || !pm.IsAlive || pm.Distance > Distance) return false;
                }
            }
            return _assembled;
        }

        #endregion
    }
}
