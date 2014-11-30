using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Plugins;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Windows.Media;
using System.Threading;

namespace HotspotRecorder
{
    public class HotspotRecorderPlugin : HBPlugin
    {

        public override string Author { get { return "Kamilche"; } }
        public override Version Version { get { return new Version(1, 0, 0); } }
        public override string Name { get { return "HotspotRecorder"; } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Show Recorder"; } }
        public delegate void UpdateUIDelegate(CustomEventArgs args);
        public static event UpdateUIDelegate UpdateUI; 
        public static int updateinterval = 10;
        private static WoWPoint myloc = new WoWPoint(1.0f, 2.0f, 3.0f);
        private static WoWPoint targetloc = new WoWPoint(100.0f, 101.0f, 102.0f);
        private static float amttomove = .1f;
        private static frmMain form = null;

        private static DateTime nextUIupdate = DateTime.Now;
        #region HB routines

        public override void OnButtonPress()
        {
            form = new frmMain();
            form.Show();
        }

        public override void Pulse()
        {
            if (DateTime.Now > nextUIupdate)
            {
                nextUIupdate = DateTime.Now.AddMilliseconds(updateinterval);
                CustomEventArgs args = new CustomEventArgs();
                if (UpdateUI != null)
                    UpdateUI(args);
            }
        }
        #endregion

        #region Message Pump for form testing
        public void MessagePump()
        {
            while (true)
                Pulse();
        }

        #endregion

        public void log(String format, params object[] args)
        {
            String s = string.Format(format, args);
            Logging.Write(Colors.Teal, s);
        }

        #region Custom Event
        public class CustomEventArgs : EventArgs
        {
            public XYZ targetlocation;
            public string targetname;
            public int targetentry;
            public bool isrepair;
            public XYZ mylocation;
            public CustomEventArgs()
            {
                LocalPlayer Me = StyxWoW.Me;
                if (Me == null)
                {
                    // make stuff up - debugging in standalone mode
                    myloc.X += amttomove;
                    myloc.Y += amttomove;
                    myloc.Z += amttomove;
                    targetloc.X += amttomove;
                    targetloc.Y += amttomove;
                    targetloc.Z += amttomove;
                    targetlocation = new XYZ(targetloc);
                    targetname = "Repair NPC";
                    targetentry = 12345;
                    isrepair = true;
                    mylocation = new XYZ(myloc);
                }
                else
                {
                    WoWUnit target = Me.CurrentTarget;
                    if (target == null)
                    {
                        targetlocation = null;
                        targetname = null;
                        targetentry = 0;
                        isrepair = false;
                    }
                    else
                    {
                        targetlocation = new XYZ(target.Location);
                        targetname = target.Name;
                        targetentry = (int)target.Entry;
                        isrepair = target.IsRepairMerchant;
                    }
                    mylocation = new XYZ(StyxWoW.Me.Location);
                }
            }
        }

        public class XYZ
        {
            public double X;
            public double Y;
            public double Z;
            public bool navigable = true;
            public XYZ(double x, double y, double z)
            {
                this.X = x; this.Y = y; this.Z = z;
            }
            public XYZ(WoWPoint point)
            {
                this.X = point.X; this.Y = point.Y; this.Z = point.Z;
            }
            public override string ToString() 
            {
                return string.Format("{0}, {1}, {2}", this.X, this.Y, this.Z);
            }
        }
    }
        #endregion
}
