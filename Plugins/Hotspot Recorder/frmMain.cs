using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;
using Styx;
using Styx.Pathing;
using Styx.WoWInternals;


namespace HotspotRecorder
{
    public partial class frmMain : Form
    {
        System.Threading.Thread newThread = null;
        private string targetname = string.Empty;
        private HotspotRecorderPlugin.XYZ targetlocation = null;
        private int targetentry = 0;
        private bool isrepair = false;
        private HotspotRecorderPlugin.XYZ mylocation = null;
        private HotspotRecorderPlugin.UpdateUIDelegate updateui = null;
        private HotspotRecorderPlugin.XYZ oldloc = null;
        private string profile = null;
        private Queue<KeyValuePair<string, object>> queue = new Queue<KeyValuePair<string, object>>();
        private int closesthotspot = -1;
        private List<HotspotRecorderPlugin.XYZ> points = null;
        private HotspotRecorderPlugin fakeplugin = null;
        private List<HotspotRecorderPlugin.XYZ> checkedpoints = null;
        static CultureInfo culture = CultureInfo.InvariantCulture;
        private bool autorun = false;
        private string header = @"<?xml version=""1.0"" encoding=""utf-8""?>
<HBProfile>
  <Name>{0}</Name>
  <MinDurability>0.4</MinDurability>
  <MinFreeBagSlots>1</MinFreeBagSlots>
  <MinLevel>1</MinLevel>
  <MaxLevel>999</MaxLevel>
  <Factions>99999</Factions>
  <MailGrey>False</MailGrey>
  <MailWhite>True</MailWhite>
  <MailGreen>True</MailGreen>
  <MailBlue>True</MailBlue>
  <MailPurple>True</MailPurple>
  <SellGrey>True</SellGrey>
  <SellWhite>False</SellWhite>
  <SellGreen>False</SellGreen>
  <SellBlue>False</SellBlue>
  <SellPurple>False</SellPurple>
  <SellPurple>False</SellPurple>
";

        #region Supporting Routines

        private void display(String fmt, params object[] args)
        {
            try
            {
                String s = string.Format(fmt, args);
                s = Environment.NewLine + s;
                // Do a thread-safe update on txtOutput
                // (this routine is used by On_Update, which is called by an external thread.)
                this.txtOutput.Invoke((MethodInvoker)(() => txtOutput.Text += s));
                this.txtOutput.Invoke((MethodInvoker)(() => txtOutput.SelectionStart = txtOutput.Text.Length));
                this.txtOutput.Invoke((MethodInvoker)(() => txtOutput.ScrollToCaret()));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
            }
        }

        private double FastDistance(HotspotRecorderPlugin.XYZ pt1, HotspotRecorderPlugin.XYZ pt2)
        {
            double x = pt2.X - pt1.X;
            double y = pt2.Y - pt1.Y;
            double z = pt2.Z - pt1.Z;
            return x * x + y * y + z * z;
        }
        private double Distance(HotspotRecorderPlugin.XYZ pt1, HotspotRecorderPlugin.XYZ pt2)
        {
            double x = pt2.X - pt1.X;
            double y = pt2.Y - pt1.Y;
            double z = pt2.Z - pt1.Z;
            return Math.Sqrt(x * x + y * y + z * z);
        }
        private void ClearPoints()
        {
            points = null;
        }
        private string PointToString(string spottype, HotspotRecorderPlugin.XYZ spot)
        {
            string suffix = string.Empty;
            if (spottype.ToLower() == "blackspot")
                suffix = string.Format(@"Radius=""{0}""", this.txtBlackspotRadius.Text);
            string s = string.Format(culture, @"<{0} X=""{1:0.0000}"" Y=""{2:0.0000}"" Z=""{3:0.0000}"" {4} />", spottype, spot.X, spot.Y, spot.Z, suffix);
            return s;
        }

        private WoWPoint PointToWoWPoint(HotspotRecorderPlugin.XYZ spot)
        {
            return new WoWPoint(spot.X, spot.Y, spot.Z);
        }

        private string PointToVendorString(string vendorname, int entry, string vendortype, HotspotRecorderPlugin.XYZ spot)
        {
            string s = string.Format(culture, @"<Vendor Name=""{0}"" Entry=""{1}"" Type=""{2}"" X=""{3:0.0000}"" Y=""{4:0.0000}"" Z=""{5:0.0000}"" />", vendorname, entry, vendortype, spot.X, spot.Y, spot.Z);
            return s;
        }
        private HotspotRecorderPlugin.XYZ StringToPoint(string input)
        {
            try
            {
                string pattern = @"X=""(?<X>.*?)"".*Y=""(?<Y>.*?)"".*Z=""(?<Z>.*?)""";
                Regex regex = new Regex(pattern);
                var match = regex.Match(input);
                string strX = match.Groups["X"].Value;
                string strY = match.Groups["Y"].Value;
                string strZ = match.Groups["Z"].Value;

                double x = double.Parse(strX);
                double y = double.Parse(strY);
                double z = double.Parse(strZ);

                return new HotspotRecorderPlugin.XYZ(x, y, z);
            }
            catch (Exception ex)
            {
                string errmsg = string.Format("Error: {0}", ex.Message);
                display(errmsg);
                MessageBox.Show(errmsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void AutoGenerateHotspot()
        {
            int spacing = 20;
            if (!(Int32.TryParse(txtSpacing.Text, out spacing)))
            {
                display("Hotspot spacing must be a numeric integer!");
                return;
            }

            if (oldloc == null)
            {
                oldloc = mylocation;
                return;
            }

            double dist = Distance(mylocation, oldloc);
            if (dist > spacing)
            {
                AddSpot(lstHotSpots, "Hotspot", mylocation);
                oldloc = mylocation;
            }

        }

        private void HandleError(Exception ex)
        {
            string s = string.Format("Error! Make sure HonorBuddy is running, with combat bot, or grind bot with an empty profile. {0}", ex.Message);
            System.Diagnostics.Debug.Print(s);
            System.Diagnostics.Debug.Print(ex.ToString());
            MessageBox.Show(s, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw ex;
        }

        private void AddSpot(ListView lb, string spottype, HotspotRecorderPlugin.XYZ spot)
        {
            try
            {
                if (spot == null)
                    throw new Exception("Spot is null");
                lb.Items.Add(PointToString(spottype, spot));
                //int visibleItems = lb.ClientSize.Height / lb.ItemHeight;
                //lb.TopIndex = Math.Max(lb.Items.Count - visibleItems + 1, 0);
                ClearPoints();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
        private void AddVendor(ListView lb, string vendorname, int entry, string vendortype, HotspotRecorderPlugin.XYZ spot)
        {
            try
            {
                if (spot == null)
                    throw new Exception("Spot is null");
                lb.Items.Add(PointToVendorString(vendorname, entry, vendortype, spot));
                //int visibleItems = lb.ClientSize.Height / lb.ItemHeight;
                //lb.TopIndex = Math.Max(lb.Items.Count - visibleItems + 1, 0);
                ClearPoints();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }

        }
        private void GoTo(string input)
        {
            HotspotRecorderPlugin.XYZ xyz = StringToPoint(input);
            if (xyz != null)
                GoTo(xyz);
        }
        private void GoTo(HotspotRecorderPlugin.XYZ xyz)
        {
            // Go to the specified point in WoW, on the next pulse.
            // Don't do it right away! You will probably fail with memory read errors.
            // Just stuff it in a queue, and wait for the next 'On_Update' to execute it.
            queue.Enqueue(new KeyValuePair<string, object>("goto", xyz));
        }
        private void CalculateClosestHotspot()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(
                    () => CalculateClosestHotspot()
                ));
            }
            else
            {
                if (points == null)
                {
                    points = new List<HotspotRecorderPlugin.XYZ>();
                    for (int i = 0; i < lstHotSpots.Items.Count; i++)
                    {
                        points.Add(StringToPoint(lstHotSpots.Items[i].Text));
                    }
                }
                double closestdist = -1;
                int closestindex = -1;
                for (int i = 0; i < points.Count; i++)
                {
                    double dist = FastDistance(mylocation, points[i]);
                    if (closestdist < 0 || dist < closestdist)
                    {
                        closestdist = dist;
                        closestindex = i;
                    }
                }
                if (closesthotspot != closestindex)
                {
                    display("Closest hotspot is {0}", closestindex);
                    closesthotspot = closestindex;
                    for (int i = 0; i < lstHotSpots.Items.Count; i++)
                    {
                        ListViewItem lvi = lstHotSpots.Items[i];
                        if (lvi.BackColor != Color.White)
                            lvi.BackColor = Color.White;
                    }
                    if (closestindex >= 0)
                        lstHotSpots.Items[closestindex].BackColor = Color.Yellow;
                    lstHotSpots.Invalidate();
                }
            }
        }
        private ListView theSelectedTab()
        {
            ListView lb;
            if (tabMain.SelectedTab.Text == "Hotspots")
                lb = lstHotSpots;
            else if (tabMain.SelectedTab.Text == "Blackspots")
                lb = lstBlackSpots;
            else if (tabMain.SelectedTab.Text == "Mailboxes")
                lb = lstMailboxes;
            else if (tabMain.SelectedTab.Text == "NPC")
                lb = lstNPC;
            else
            {
                display("Unknown tab {0}!", tabMain.SelectedTab.Text);
                return null;
            }
            return lb;
        }

        private void UpdateCheckedPoints()
        {
            // Variable 'checkedpoints' is not null, that means On_Update is done with the check.
            // Update the current list with those points.
            List<HotspotRecorderPlugin.XYZ> localpoints = checkedpoints;
            checkedpoints = null;
            ListView lb = theSelectedTab();
            List<string> lst = new List<string>();
            if (lb == null) return;
            for (int i = 0; i < localpoints.Count; i++)
            {
                string s = "";
                if (lb == this.lstNPC)
                    s = PointToVendorString(this.targetname, this.targetentry, (this.isrepair ? "Repair" : "Food"), localpoints[i]);
                else if (lb == this.lstMailboxes)
                    s = PointToString("Mailbox", localpoints[i]);
                else if (lb == this.lstBlackSpots)
                    s = PointToString("Blackspot", localpoints[i]);
                else if (lb == this.lstHotSpots)
                    s = PointToString("Hotspot", localpoints[i]);
                string s2;
                if (localpoints[i].navigable)
                    s2 = s;
                else
                    s2 = string.Format("<!-- BAD POINT {0} -->", s);
                lst.Add(s2);
            }
            lb.Items.Clear();
            foreach (string s in lst)
                lb.Items.Add(new ListViewItem(s));
            ClearPoints();
        }

        #endregion

        #region Routine called by Honorbuddy Pulse method
        public void On_Update(HotspotRecorderPlugin.CustomEventArgs args)
        {
            //try
            //{
                string fmt = "{0:0.000}, {1:0.000}, {2:0.000}";
                targetlocation = args.targetlocation;
                targetname = args.targetname;
                targetentry = args.targetentry;
                isrepair = args.isrepair;
                mylocation = args.mylocation;
                CalculateClosestHotspot();
                // Do a thread-safe update on the fields 
                // (this routine (On_Update) is called by an external thread.)
                this.lblLocationNPC.Invoke((MethodInvoker)(() => lblLocationNPC.Text = (targetlocation == null ? "<target location>" : string.Format(fmt, targetlocation.X, targetlocation.Y, targetlocation.Z))));
                this.lblLocationMe.Invoke((MethodInvoker)(() => lblLocationMe.Text = (mylocation == null ? "<my location>" : string.Format(fmt, mylocation.X, mylocation.Y, mylocation.Z))));
                this.lblNPCName.Invoke((MethodInvoker)(() => lblNPCName.Text = (targetname == null ? "<target name>" : targetname)));

                if (queue.Count > 0)
                {
                    int commandcnt = queue.Count;
                    Cursor.Current = Cursors.WaitCursor;
                    for (int i = 0; i < commandcnt; i++)
                    {
                        KeyValuePair<string, object> kvp = queue.Dequeue();
                        display("Executing queued command {0} of {1}", i + 1, commandcnt);
                        switch (kvp.Key)
                        {
                            case "goto":
                                autorun = true;
                                HotspotRecorderPlugin.XYZ xyz = (HotspotRecorderPlugin.XYZ)kvp.Value;
                                display("Go to {0:0.0000}, {1:0.0000}, {2:0.0000}", xyz.X, xyz.Y, xyz.Z);
                                if (StyxWoW.Me == null)
                                    return;
                                WoWPoint pt = new WoWPoint(xyz.X, xyz.Y, xyz.Z);
                                while (autorun && StyxWoW.Me.Location.Distance(pt) >= 5 && (Styx.CommonBot.BotManager.Current.Name == "Grind Bot" || !StyxWoW.Me.Combat))
                                {
                                    CalculateClosestHotspot();
                                    Navigator.MoveTo(pt);
                                    Thread.Sleep(500);
                                }
                                if (StyxWoW.Me.Combat && StyxWoW.Me.Mounted)
                                    Styx.CommonBot.Mount.Dismount("Because reasons.");
                                break;
                            case "run":
                                autorun = true;
                                List<HotspotRecorderPlugin.XYZ> list2 = (List<HotspotRecorderPlugin.XYZ>)kvp.Value;
                                int startpt = Math.Max(closesthotspot, 0);
                                List<int> indexes = new List<int>();
                                for (int j = startpt; j < list2.Count; j++)
                                    indexes.Add(j);
                                for (int j = 0; j < startpt; j++)
                                    indexes.Add(j);
                                for (int j = 0; j < indexes.Count; j++)
                                {
                                    if (!autorun || (StyxWoW.Me.Combat && Styx.CommonBot.BotManager.Current.Name != "Grind Bot"))
                                        break;
                                    WoWPoint pt2 = PointToWoWPoint(list2[indexes[j]]);
                                    display("Running to point {0} {1}", indexes[j], list2[indexes[j]].ToString());
                                    while (autorun && StyxWoW.Me.Location.Distance(pt2) >= 5 && (Styx.CommonBot.BotManager.Current.Name == "Grind Bot" || !StyxWoW.Me.Combat))
                                    {
                                        CalculateClosestHotspot();
                                        Navigator.MoveTo(pt2);
                                        Thread.Sleep(500);
                                    }
                                }
                                if (StyxWoW.Me.Combat && StyxWoW.Me.Mounted)
                                    Styx.CommonBot.Mount.Dismount("Because reasons.");
                                break;
                            case "CheckHotspots":
                                List<HotspotRecorderPlugin.XYZ> list = (List<HotspotRecorderPlugin.XYZ>)kvp.Value;
                                for (int j = 0; j < list.Count; j++)
                                    list[j].navigable = true;
                                this.Invoke((MethodInvoker)(() => toolStripProgressBar1.Maximum = list.Count));
                                int badpts = 0;
                                for (int j = 1; j < list.Count; j++)
                                {
                                    this.Invoke((MethodInvoker)(() => toolStripProgressBar1.Value = j));
                                    if (StyxWoW.Me == null)
                                    {
                                        // Do a fake check here.
                                        if (j % 10 == 0)
                                        {
                                            list[j].navigable = false;
                                            badpts++;
                                        }
                                    }
                                    else
                                    {
                                        // Do a real check here.
                                        if (!Styx.Pathing.Navigator.CanNavigateFully(PointToWoWPoint(list[j - 1]), PointToWoWPoint(list[j])))
                                        {
                                            list[j].navigable = false;
                                            badpts++;
                                        }
                                    }
                                }
                                display("Done checking hotspots, {0} bad points found out of a total of {1}.", badpts, list.Count);
                                checkedpoints = list;
                                this.Invoke((MethodInvoker)(() => toolStripProgressBar1.Value = list.Count));
                                break;
                            default:
                                display("Unknown command {0}!", kvp.Key);
                                break;
                        }
                        display("Done executing queued command {0} of {1}", i + 1, commandcnt);
                    }
                    Cursor.Current = Cursors.Default;
                }
            //}
            //catch (Exception ex)
            //{
            //    if (!(ex is System.Threading.ThreadAbortException))
            //        HandleError(ex);
            //}
        }

        #endregion

        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updateui = new HotspotRecorderPlugin.UpdateUIDelegate(On_Update);
            if (StyxWoW.Me == null)
            {
                // Create the plugin manually since we're testing in standalone mode.
                fakeplugin = new HotspotRecorderPlugin();
                newThread = new System.Threading.Thread(fakeplugin.MessagePump);
                newThread.Start();
            }
            HotspotRecorderPlugin.UpdateUI += updateui;
            timer1.Enabled = true;
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            this.Size = new Size(this.Width + 1, this.Height);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                HotspotRecorderPlugin.UpdateUI -= updateui;
                if (newThread != null)
                    newThread.Abort();
            }
            catch (Exception ex)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Styx.CommonBot.TreeRoot.IsRunning && StyxWoW.Me != null)
            {
                if (btnAddBlackspot.Enabled == true)
                {
                    ShowControls(false);
                    string s = "Choose 'Combat Bot' or 'Grind Bot' with an empty profile, then click 'Start' on Honorbuddy.";
                    display(s);
                    if (StyxWoW.Me != null)
                        MessageBox.Show(s);
                }
            }
            else if (btnAddBlackspot.Enabled == false)
            {
                ShowControls(true);
            }
            if (chkGenerateHotspots.Checked)
                AutoGenerateHotspot();
            if (checkedpoints != null)
                UpdateCheckedPoints();
        }

        private void ShowControls(bool value)
        {
            btnAddHotspot.Enabled = value;
            btnAddBlackspot.Enabled = value;
            btnAddMailbox.Enabled = value;
            btnAddVendor.Enabled = value;
            chkGenerateHotspots.Enabled = value;
            btnPrev.Enabled = value;
            btnNext.Enabled = value;
            btnGoTo.Enabled = value;
            btnRunPoints.Enabled = value;
            btnCheckPoints.Enabled = value;
            btnReplace.Enabled = Visible;
        }

        private void btnLoadProfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                profile = openFileDialog1.FileName;
                string[] lines = System.IO.File.ReadAllLines(profile);
                lstHotSpots.Items.Clear();
                lstBlackSpots.Items.Clear();
                lstNPC.Items.Clear();
                lstMailboxes.Items.Clear();
                foreach (string origline in lines)
                {
                    string line = origline.Trim();
                    if (line.StartsWith("<Hotspot "))
                        lstHotSpots.Items.Add(line);
                    else if (line.StartsWith("<Blackspot "))
                        lstBlackSpots.Items.Add(line);
                    else if (line.StartsWith("<Vendor "))
                        lstNPC.Items.Add(line);
                    else if (line.StartsWith("<Mailbox "))
                        lstMailboxes.Items.Add(line);
                }
                Cursor.Current = Cursors.Default;
                string s = string.Format("Loaded profile '{0}'.", profile);
                display(s);
                txtProfileName.Text = profile;
                ClearPoints();
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML files (*.xml)|*.xml";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                profile = saveFileDialog1.FileName;
                txtProfileName.Text = profile;
                List<string> lines = new List<string>();
                string shortfilename = System.IO.Path.GetFileName(profile);
                lines.Add(string.Format(header, shortfilename));
                lines.Add("<Vendors>");
                for (int i = 0; i < lstNPC.Items.Count; i++)
                    lines.Add(string.Format("    {0}", lstNPC.Items[i].Text));
                lines.Add("</Vendors>");
                lines.Add("");
                lines.Add("<Mailboxes>");
                for (int i = 0; i < lstMailboxes.Items.Count; i++)
                    lines.Add(string.Format("    {0}", lstMailboxes.Items[i].Text));
                lines.Add("</Mailboxes>");
                lines.Add("");
                lines.Add("<Blackspots>");
                for (int i = 0; i < lstBlackSpots.Items.Count; i++)
                    lines.Add(string.Format("    {0}", lstBlackSpots.Items[i].Text));
                lines.Add("</Blackspots>");
                lines.Add("");
                lines.Add("<Hotspots>");
                for (int i = 0; i < lstHotSpots.Items.Count; i++)
                    lines.Add(string.Format("    {0}", lstHotSpots.Items[i].Text));
                lines.Add("</Hotspots>");
                lines.Add("");
                lines.Add("</HBProfile>");
                System.IO.File.WriteAllLines(profile, lines.ToArray());
                string msg = string.Format("Saved file to '{0}'.", profile);
                display(msg);
            }
        }

        private void btnAddHotspot_Click(object sender, EventArgs e)
        {
            AddSpot(lstHotSpots, "Hotspot", mylocation);
        }

        private void btnAddBlackspot_Click(object sender, EventArgs e)
        {
            AddSpot(lstBlackSpots, "Blackspot", mylocation);
        }

        private void btnAddMailbox_Click(object sender, EventArgs e)
        {
            AddSpot(lstMailboxes, "Mailbox", mylocation);
        }

        private void btnAddVendor_Click(object sender, EventArgs e)
        {
            AddVendor(lstNPC, targetname, targetentry, (isrepair ? "Repair" : "Food"), targetlocation);
        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
            ListView lb = theSelectedTab();
            if (lb == null) return;
            List<string> lst = new List<string>();
            for (int i = 0; i < lb.Items.Count; i++ )
                lst.Add(lb.Items[i].Text);
            lst.Reverse();
            lb.Items.Clear();
            foreach (string s in lst)
                lb.Items.Add(new ListViewItem(s));
            ClearPoints();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ListView lb = theSelectedTab();
            if (lb == null) return;
            if (lb.SelectedIndices.Count == 0)
                return;
            if (MessageBox.Show("This will delete the currently selected point(s)! Continue?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Cancel)
                return;
            List<string> newlist = new List<string>();

            foreach (ListViewItem lvi in lb.Items)
            {
                if (lvi.Selected == false)
                    newlist.Add(lvi.Text);
            }

            lb.Items.Clear();
            foreach (String s in newlist)
                lb.Items.Add(new ListViewItem(s));
            ClearPoints();
        }

        private void btnSetTop_Click(object sender, EventArgs e)
        {
            ListView lb = theSelectedTab();
            if (lb == null) return;
            if (lb.SelectedIndices.Count == 0)
                return;
            List<string> newlist = new List<string>();
            int top = lb.SelectedIndices[0];
            for (int i = top; i < lb.Items.Count; i++)
                newlist.Add(lb.Items[i].Text);
            for (int i = 0; i < top; i++)
                newlist.Add(lb.Items[i].Text);
            lb.Items.Clear();
            foreach (String s in newlist)
                lb.Items.Add(new ListViewItem(s));
            ClearPoints();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            ListView lb = theSelectedTab();
            if (lb == null) return;
            if (lb.SelectedIndices.Count == 0)
                return;
            int index = lb.SelectedIndices[0];
            if (lb == this.lstNPC)
                lb.Items[index] = new ListViewItem(PointToVendorString(this.targetname, this.targetentry, (this.isrepair ? "Repair" : "Food"), targetlocation));
            else if (lb == this.lstMailboxes)
                lb.Items[index] = new ListViewItem(PointToString("Mailbox", this.mylocation));
            else if (lb == this.lstBlackSpots)
                lb.Items[index] = new ListViewItem(PointToString("Blackspot", this.mylocation));
            else if (lb == this.lstHotSpots)
                lb.Items[index] = new ListViewItem(PointToString("Hotspot", this.mylocation));
            ClearPoints();
        }

        private void btnGoTo_Click(object sender, EventArgs e)
        {
            ListView lb = theSelectedTab();
            if (lb == null) return;
            if (lb.SelectedIndices.Count == 0)
                return;
            GoTo(lb.Items[lb.SelectedIndices[0]].Text);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            ListView lb = theSelectedTab();
            if (lb == null) return;
            int index = -1;
            if (lb.SelectedIndices.Count > 0)
                index = lb.SelectedIndices[0];
            lb.SelectedIndices.Clear();
            int newindex = -1;
            if (index <= 0)
                newindex = lb.Items.Count - 1;
            else
                newindex = index - 1;
            lb.Items[newindex].Selected = true;
            GoTo(lb.Items[newindex].Text);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ListView lb = theSelectedTab();
            if (lb == null) return;
            int index = -1;
            if (lb.SelectedIndices.Count > 0)
                index = lb.SelectedIndices[0];
            lb.SelectedIndices.Clear();
            int newindex = -1;
            if (index >= lb.Items.Count - 1)
                newindex = 0;
            else
                newindex = index + 1;
            lb.Items[newindex].Selected = true;
            GoTo(lb.Items[newindex].Text);
        }

        private void btnRunPoints_Click(object sender, EventArgs e)
        {
            queue.Enqueue(new KeyValuePair<string, object>("run", points));
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            double adj;
            if (double.TryParse(txtAdjustHeight.Text, out adj) == false)
            {
                display("Height adjustment must be a numeric integer!");
                return;
            }
            // Only apply height adjustment to hotspots.
            tabMain.SelectedIndex = 0;
            ListView lb = lstHotSpots;
            RefreshPoints();
            lb.Items.Clear();
            for (int i = 0; i < points.Count - 1; i++)
            {
                points[i].Z += adj;
                lb.Items.Add(PointToString("Hotspot", points[i]));
            }
            display("{0} added to height of all hotspots.", adj);
        }

        private void RefreshPoints()
        {
            ListView lb = theSelectedTab();
            if (lb == null) return;
            points = new List<HotspotRecorderPlugin.XYZ>();
            for (int i = 0; i < lb.Items.Count; i++)
                points.Add(StringToPoint(lb.Items[i].Text));
        }

        private void btnCheckPoints_Click(object sender, EventArgs e)
        {
            RefreshPoints();
            queue.Enqueue(new KeyValuePair<string, object>("CheckHotspots", points));
        }

        private void lstBlackspots_DoubleClick(object sender, EventArgs e)
        {
            if (lstBlackSpots.SelectedIndices.Count > 0)
                GoTo(lstBlackSpots.Items[lstBlackSpots.SelectedIndices[0]].Text);
        }

        private void lstHotSpots_DoubleClick(object sender, EventArgs e)
        {
            if (lstHotSpots.SelectedIndices.Count > 0)
                GoTo(lstHotSpots.Items[lstHotSpots.SelectedIndices[0]].Text);
        }

        private void lstMailboxes_DoubleClick(object sender, EventArgs e)
        {
            if (lstMailboxes.SelectedIndices.Count > 0)
                GoTo(lstMailboxes.Items[lstMailboxes.SelectedIndices[0]].Text);
        }

        private void lstNPC_DoubleClick(object sender, EventArgs e)
        {
            if (lstNPC.SelectedIndices.Count > 0)
                GoTo(lstNPC.Items[lstNPC.SelectedIndices[0]].Text);
        }

        private void statusStrip1_Resize(object sender, EventArgs e)
        {
            toolStripProgressBar1.Width = statusStrip1.Width - 20;
        }

        private void lstHotSpots_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListView lst = (ListView)sender;
            e.DrawBackground();
            if (e.Index == closesthotspot)
                e.Graphics.FillRectangle(new SolidBrush(Color.Yellow), e.Bounds);
            e.Graphics.DrawString(lst.Items[e.Index].Text, e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            autorun = false;
        }

    }
}
