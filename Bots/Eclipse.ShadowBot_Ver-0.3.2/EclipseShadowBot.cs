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
using Styx.CommonBot.Coroutines;
using System.IO;
using ArachnidCreations;
using System.Data;
using Eclipse.ShadowBot.Data;

namespace Eclipse.ShadowBot
{
    public class EclipseShadowBot : BotBase
    {

        public static LocalPlayer Me;
        public static WoWPlayer Leader;
        public static bool AssistLeader = false;
        public static bool IgnoreAttackers = false;
        public static bool PickUpQuests = false;
        public static int FollowDistance = 8;
        public static bool HealBotMode = true;
        public static string DataPath = string.Empty;
        public static ShadowBotSettings settings = null;
        private ShadowBotConfig _gui;
        private Composite _root;

        #region Overrides
        public override string Name
        {
            get { return "Eclipse - ShadowBot 0.1"; }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        public override Form ConfigurationForm
        {
            get
            {
                if (_gui == null || _gui.IsDisposed) _gui = new ShadowBotConfig();
                return _gui;
            }
        }

        public override void Start()
        {
            if (Me == null) Me = StyxWoW.Me;
            if (DataPath == string.Empty) FindDB();
            if (DataPath != string.Empty)
            {
                LoadSettings();
        }
        }

        private void LoadSettings()
        {
            DataTable dt = DAL.LoadSL3Data("Select * from ShadowbotSettings");
        }

        public override void Stop()
        {
            log("Stop Called");
            base.Stop();
        }
        public override void Pulse()
        {
            //Core.Pulse();
        }
        public static void log(string text)
        {
            Logging.Write(Color.FromRgb(144, 0, 255), "Eclipse=>" + text);
        }
        public override void Initialize()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logging.Write(Colors.Red, ex.ToString());
            }
        }

        #endregion

        #region Behaviors
        public override Composite Root
        {
            get
            {
                return _root ?? (_root =
                    new PrioritySelector(
                        new Decorator(r => Me.IsDead, CreateDeadBehavior),
                        new Decorator(r=> HealBotMode,  CreateHealBehavior()),
                        new Decorator(r => Leader != null && Me.IsAlive,
                            new PrioritySelector(
                                new Decorator(r => Leader.Distance > FollowDistance,new Action(r => Flightor.MoveTo(Leader.Location))),
                                new Decorator(r => TargetClosestLootableMob(), CreateLootingBehavior),
                                new Decorator(r => TargetClosestSkinnableMob(), CreateSkinningBehavior),
                                new Decorator(r => PickUpQuests && GetQuestGiver(), CreateQuestBehavior),
                                new Decorator(r => AssistLeader && !HealBotMode && Leader.Combat && Leader.CurrentTarget != null,
                                    new Sequence(
                                        new Action(a => Leader.CurrentTarget.Target()),          CreateCombatBehavior())
                                    )
                                ))
                        )

                        
                );
            }
        }
        private static bool TargetClosestLootableMob()
        {
            var mob = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && ((WoWUnit)n).Lootable).OrderBy(m => m.Distance).FirstOrDefault();
            if (mob != null)
            {
                log("Found a lootable mob");
                mob.Target();
                return true;
            }
            else return false;
        }
        private static bool TargetClosestSkinnableMob()
        {
            var mob = (WoWUnit)ObjectManager.ObjectList.Where(n => n.Type == WoWObjectType.Unit && isSkinnable((WoWUnit)n)).OrderBy(m => m.Distance).FirstOrDefault();
            if (mob != null)
            {
                log("Found a skinnable mob");
                mob.Target();
                return true;
            }
            else return false;
        }
        #region Combat Behavior
        private static Composite CreateHealBehavior()
        {
            return new PrioritySelector(RoutineManager.Current.HealBehavior);
        }
        private static Composite CreateCombatBehavior()
        {
            return new PrioritySelector(
                new Decorator(r=> Me.CurrentTarget == null, new Action(a=>Leader.Target())),
                new Decorator(ret => !StyxWoW.Me.Combat,
                            new PrioritySelector(
                        RoutineManager.Current.PreCombatBuffBehavior)),
                new Decorator(ret => Leader.Combat,
                    new LockSelector(
                        RoutineManager.Current.HealBehavior,
                        new Decorator(ret => StyxWoW.Me.GotTarget && !StyxWoW.Me.CurrentTarget.IsFriendly && !StyxWoW.Me.CurrentTarget.IsDead,
                            new PrioritySelector(
                                RoutineManager.Current.CombatBuffBehavior,
                                RoutineManager.Current.CombatBehavior))))
            );    
            
        }

        #endregion
        public Composite CreateLootingBehavior
        {
            get
            {
                return new Decorator(r=> Me.CurrentTarget != null,
                    new PrioritySelector(
                        new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange && Me.CurrentTarget.Lootable,
                                    new Sequence(
                                new Action(r => Flightor.MoveTo(Me.CurrentTarget.Location))
                                        )
                                    ),
                        new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= Me.InteractRange && Me.CurrentTarget.Lootable,
                            new Sequence(
                                new Action(r => Me.CurrentTarget.Interact()),
                                new Action(r => TreeRoot.StatusText = String.Format("Looting {0}", Me.CurrentTarget.Name))
                                )
                            )
                        )
                    );
            }
        }
        public Composite CreateSkinningBehavior
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => StyxWoW.Me.CurrentTarget != null,
                                    new PrioritySelector(
                            new Decorator(r => Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance > Me.InteractRange,
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
        }
        public Composite CreateQuestBehavior
            {
            get
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
                                   new Action(r => Flightor.MoveTo(Leader.Location)),
                                   new ActionAlwaysSucceed()
                                )
                            )
                        )
                    )
                )
            );
            }
        }
        public Composite CreateDeadBehavior //this is commmunity contributed content I assume from  FPSWare's RAF bot. If that IS the case than THANK YOU FPSWare!
        {
            get
            {
                return new PrioritySelector(

                // Mount up - for rez sickness wait
                    //new Decorator(ret => !Me.IsDead && !Me.IsGhost && !Me.Mounted && Me.HasAura(15007) && Flightor.MountHelper.CanMount, Common.MountUpFlying()),

                // Mounted? The ascend and just wait out rez sickness
                new Decorator(ret => Me.Mounted && !Me.IsFlying && !StyxWoW.Me.MovementInfo.IsAscending,
                    new Sequence(
                        new Action(context => log("Flying up to wait out rez sickness")),
                        new Action(context => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromSeconds(4)))
                    )),

                // Just wait out rez sickness
                new Decorator(ret => Me.IsFlying && Me.HasAura(15007), new Action(ctx => { TreeRoot.StatusText = "Waiting out rez sickness"; TreeRoot.StatusText = "Waiting out rez sickness"; return RunStatus.Success; })),

                // Release body
                new Decorator(ret => Me.IsDead && !Me.IsGhost,
                    new Sequence(
                        new Action(context => log("We're dead! Releasing corpse")),
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
                                new Action(context => log("Recovering our body")),
                                new Action(context => Lua.DoString("RetrieveCorpse()"))
                        ))
                    ))


                );
        }
        }
        #endregion
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
        private bool GetQuestGiver()
        {
            //ToDo: Converting the Delegate 12 times in the lamda expression is probably super slow and crappy we should convert the whole list.
            var questgivers = ObjectManager.ObjectList.Where(n =>
                n.Type == WoWObjectType.Unit
            ).ToList().Where(n =>
                ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.Available
                && ((WoWUnit)n).IsFriendly
                && ((WoWUnit)n).Distance <= FollowDistance
                || ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.TurnIn
                && ((WoWUnit)n).Distance <= FollowDistance
                || ((WoWUnit)n).QuestGiverStatus == QuestGiverStatus.TurnInRepeatable
                && ((WoWUnit)n).Distance <= FollowDistance
                && ((WoWUnit)n).IsFriendly
            ).OrderBy(m => m.Distance).ToList();


            if (questgivers.Count > 0)
            {
                var mob = (WoWUnit)questgivers.OrderBy(m => m.Distance).FirstOrDefault();
                log("Found a quest giver: " + mob.Name);
                mob.Target();

                return true;
            }

            //log("No q givers around.");
            return false;
        }

        internal static void FindDB()
        {
            var path = Application.StartupPath;
            if (!File.Exists(DataPath))
            {
                var results = Directory.GetFiles(path, "*.edb", SearchOption.AllDirectories).ToList();
                if (results.Count > 0)
                {
                    DataPath = results[0];
                    log(string.Format("--------------------------------Found {0}------------------------------------", results[0]));
                }
            }
        }

        #region Nested type: LockSelector
        //Taken from raidbot that ships with HB.
        private class LockSelector : PrioritySelector
        {
            public LockSelector(params Composite[] children)
                : base(children)
            {
            }

            public override RunStatus Tick(object context)
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    return base.Tick(context);
                }
            }
        }

        #endregion
    }
}
