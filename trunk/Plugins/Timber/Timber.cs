using Buddy.Coroutines;
using Bots.Grind;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.Pathing;
using Styx.Plugins;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;



// A FAIRE
// verifier que l'abbatage est possible
// verifier le niveau
// verifier si il y'en a deja dans les sac et combien
// stoper quand on est à 250
// saver les hotspots, gerer les blackspots
// bouger quand pas d'arbre a proximité
// gerer la prio des events.
// arranger l'activation/desactivation.

namespace Styx
{
    public class Timber : HBPlugin
    {
        // standard plugin overrides
        public override string Name { get { return "Timber - auto-chop trees"; } }
        public override string Author { get { return "scarecrow"; } }
        public override Version Version { get { return new Version(0, 1, 1, 0); } }

        // Player
        public static LocalPlayer Me { get { return Styx.StyxWoW.Me; } }

        // Tree aimed
        private WoWGameObject current_tree = null;
        private WoWGameObject previous_tree = null;


        // Timers for Pulse()
        private static TimeSpan chop_delay = new TimeSpan(0, 0, 0, 0, 3200+200);
        private static WaitTimer Chop_Timer = new WaitTimer(chop_delay);
       // private static WaitTimer ScanTree_Timer = WaitTimer.FiveSeconds;

        // Blacklisting variable
        private List<WoWGameObject> BL_tree = new List<WoWGameObject>();


        // debugging param
        private bool debugging = false;
        

        /// constructor (ctor)
        public Timber()
        {
           
        }

        public override void OnEnable()
        {
            base.OnEnable();
            // force behaviors of bot
            LevelBot.BehaviorFlags |= BehaviorFlags.All;
            LevelBot.BehaviorFlags &= ~BehaviorFlags.Pull;   // Will not pull
            LevelBot.BehaviorFlags &= ~BehaviorFlags.Loot;   // will not loot
            Logging.Write(System.Windows.Media.Colors.Pink, "[Timber] Enabled");
        }

        public override void OnDisable()
        {
            // reactivate bot behaviors
            LevelBot.BehaviorFlags |= BehaviorFlags.All;
            Logging.Write(System.Windows.Media.Colors.Pink, "[Timber] Disabled");
            base.OnDisable();
        }

        public override void Pulse()
        {
            if ((Me.IsBeingAttacked && !Me.Mounted)
                || (Me.IsActuallyInCombat && !Me.Mounted)
                || !Me.IsAlive
                || Me.OnTaxi)
            {
                return;
            }

            //   if (ScanTree_Timer.IsFinished) {
            current_tree = ScanForTrees();
            //       ScanTree_Timer.Reset();
            //   }

            if (Chop_Timer.IsFinished)           // We won't go to another tree while the time to cast tree harvesting spell.
            {

                if (current_tree == null)                                     // If no tree found
                {
                    Log("no tree in area, move manually");
                    //random pathing here - or hospot move.
                    return;

                } else  // Case where we found a tree
                    if (current_tree.Distance > (current_tree.InteractRange * 0.8)) {// if not reached yet, we move
                        Log("tree found:" + current_tree.Name.ToString());
                        Log("Moving to " + current_tree.Location.ToString());
                        Navigator.MoveTo(current_tree.Location);
                        return;
                    
                } else if (Me.IsMoving) {                                // we are near the targeted tree, we must stop before
                        Navigator.PlayerMover.MoveStop();
                        return;

                } else {
                        ChopTree(current_tree);
                        return;
                }
            }
        }

        private WoWGameObject ScanForTrees()  // return a non-blacklisted and reachable tree;
        {
            ObjectManager.Update();
            
            var Tree_List = ObjectManager.GetObjectsOfTypeFast<WoWGameObject>().
                     Where(o =>

                      (o.Entry == 234109) || (o.Entry == 234110) || (o.Entry == 233922) || (o.Entry == 234097) || (o.Entry == 234126) || (o.Entry == 234193) || (o.Entry == 234197) || (o.Entry == 237727) ||
                      (o.Entry == 234021) || (o.Entry == 234080) || (o.Entry == 234122) || (o.Entry == 233604) || (o.Entry == 234198) || (o.Entry == 234000) || (o.Entry == 234123) || (o.Entry == 234022) ||
                      (o.Entry == 234119) || (o.Entry == 234196) || (o.Entry == 234194) || (o.Entry == 234111) || (o.Entry == 234098) || (o.Entry == 233634) || (o.Entry == 234127) ||

                      (o.Entry == 234023) || (o.Entry == 234120) || (o.Entry == 234128) || (o.Entry == 234199) || (o.Entry == 234007) || (o.Entry == 234195) || (o.Entry == 234124) || (o.Entry == 234099) ||
                      (o.Entry == 233635)
                     
                    && Navigator.CanNavigateFully(Me.Location, o.Location)          // get rid of unreachable trees  
                   ).OrderBy(o => o.Distance).ToList();

            
            if (Tree_List.FirstOrDefault() == null)
            {
                return null;
            }
            else
            {
                Log("nearer tree found : " + Tree_List.FirstOrDefault().Guid.ToString());
                int i = 0;

                if (BL_tree.FirstOrDefault() != null)
                {
                    while ((i < (Tree_List.Count - 1)) && BL_tree.Contains(Tree_List[i])) {       // get rid of blacklisted trees
                                                
                        i += 1;
                    
                    }
                }

                return Tree_List[i];
            }
        }


        private void ChopTree(WoWGameObject tree)
        {
            Log("try to chop ... ");
            
            tree.Interact();
            BlackListTree(tree);
            previous_tree = tree;
            Chop_Timer.Reset();
          //  current_tree = ScanForTrees(); 
        
        }

        private void BlackListTree(WoWGameObject tree)
        {
            Log("Blacklisting treeGuid : " + tree.Guid.ToString());
            BL_tree.Add(tree);
        }

        private void Log(string s) { 
            if (debugging)
                Logging.Write(Colors.Brown, s);
        }

    }
}
