using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Linq;
using System.Drawing;

using Styx;
using Styx.Common;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using Styx.Pathing;
using Styx.CommonBot.Frames;
using Styx.CommonBot;


/* Credits to Hazard for all the code from RepCollector */

namespace CollectUnderWater
{
    class CollectUnderWater : HBPlugin
    {
        public static string name { get { return "UnderWater Collector "; } }
        public override string Name { get { return name; } }
        public override string Author { get { return "Superyeti"; } }
        private readonly static Version _version = new Version(1, 0);
        public override Version Version { get { return _version; } }
        public override string ButtonText { get { return "Don't touch me"; } }
        public override bool WantButton { get { return false; } }
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

		public static float richGhostIronDeposit = 209328;
		public static float GhostIronDeposit = 209311;
		public static float TrilliumVein = 209313;
		public static float RichTrilliumVein = 209330;
		public static float FoolsCap = 209355;
		public static float GoldenLotus = 209354;
		
		
        public override void Pulse()
        {
            Thread.Sleep(1 / 30);
            try
            {
                if (!Me.Combat && !Me.IsDead)
                    objs();
            }
            catch (ThreadAbortException)
            {
            }
        }

		public static bool isNode (float i)
		{
			bool a;
			bool b;
			if (!TrainingSkills.HasLearnt(SkillLine.Herbalism))
				a = false;
			else
				a = (i ==  FoolsCap || i == GoldenLotus);
			if (!TrainingSkills.HasLearnt(SkillLine.Mining))
				b = false;
			else
				b = (i == richGhostIronDeposit || i == GhostIronDeposit || i == TrilliumVein || i == RichTrilliumVein);
			return (a ^ b);
		}
		
		public static string aff(float id)
		{
			string s;
			if (id == richGhostIronDeposit)
				s = "Rich Ghost iron deposit";
			else if(id == GhostIronDeposit)
				s = "Ghost iron deposit";
			else if (id == TrilliumVein)
				s = "Trillium vein";
			else if (id == RichTrilliumVein)
				s = "Rich Trillium vein";
			else if (id == GoldenLotus)
				s = "Golden lotus";
			else
				s = "Fool's cap";
			return s;
		}
		
		public static void affG(float id)
		{
			string s = aff(id);
			Logging.Write(Styx.Common.LogLevel.Normal, Colors.LightSeaGreen, string.Format("[UnderWater Collector]: Gathered " + s + "."));
		}
		
		public static void affDir(WoWGameObject obj)
		{
			string s = aff(obj.Entry);
			Logging.Write(Styx.Common.LogLevel.Normal, Colors.LightSeaGreen, string.Format("[UnderWater Collector]: Approaching " + s + " at " + "X=" + obj.Location.X.ToString("G") + ", Y=" + obj.Location.Y.ToString("G") + ", Z=" + obj.Location.Z.ToString("G")));
		}
		
        static public void objs()
        {
			
            ObjectManager.Update();
            List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .Where(objs => (objs.Distance2D <= 60 && (isNode(objs.Entry)))).OrderBy(objs => objs.Distance).ToList();
			Flightor.Clear();
            foreach (WoWGameObject objs in objList)
            {
				if (objs.Z > 0 || !objs.InLineOfSight || Me.Combat) return;
				affDir(objs);
				if (objs.Location.Distance(Me.Location) > 3)
				{
					while (objs.Location.Distance(Me.Location) > 3)
					{
					Flightor.MoveTo(objs.Location);
					if (Math.Abs(Me.Location.X - objs.Location.X) < 1 && Math.Abs(Me.Location.Y - objs.Location.Y) < 1)
						return;
					}
				 }
				 else
				 {
				WoWMovement.MoveStop();
				Thread.Sleep(200);
                Flightor.MountHelper.Dismount();
                Thread.Sleep(1000);
                objs.Interact();
                Thread.Sleep(3500);
                affG(objs.Entry);
                Flightor.MountHelper.MountUp();
				}
				return;
            }
        }
    }
}