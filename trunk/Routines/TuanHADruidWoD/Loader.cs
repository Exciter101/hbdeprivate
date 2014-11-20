// Credits to Phelon and Mirabis for making this work without using Assembly redirection.//

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;

namespace Loader
{
    public class Loader : CombatRoutine
    {
        private CombatRoutine CC;

        private readonly String[] Keep = new[] { "Loader.cs", "TuanHA_Combat_Routine.dll", "Settings", "User Settings", "Files", "Please Read - Installation Guide.txt", ".svn", "ChangeLog.txt" };

        public Loader()
        {

            #region SpecialEditionRuntime

            string HonorbuddyDirectory = Utilities.AssemblyDirectory;
            string SpecialEditionRuntimeFolder = Path.Combine(Utilities.AssemblyDirectory, "Routines\\TuanHADruidWoD\\Files\\SpecialEditionRuntime.dll");
            string SpecialEditionRuntimeRoot = Path.Combine(Utilities.AssemblyDirectory, "SpecialEditionRuntime.dll");

            if (File.Exists(SpecialEditionRuntimeRoot))
            {
                FileInfo f = new FileInfo(SpecialEditionRuntimeRoot);
                long s1 = f.Length;

                if (s1 != 663552)//Version 3.68 647168
                {
                    try
                    {
                        File.Delete(SpecialEditionRuntimeRoot);
                    }
                    catch (IOException ex)
                    {
                        Logging.Write(ex.ToString()); // Write error
                    }

                    try
                    {
                        File.Copy(SpecialEditionRuntimeFolder, SpecialEditionRuntimeRoot);
                    }
                    catch (IOException ex)
                    {
                        Logging.Write(ex.ToString()); // Write error
                    }
                }
            }
            else
            {
                try
                {
                    File.Copy(SpecialEditionRuntimeFolder, SpecialEditionRuntimeRoot);
                }
                catch (IOException ex)
                {
                    Logging.Write(ex.ToString()); // Write error
                }
            }

            #endregion

            string settingsDirectory = Path.Combine(Utilities.AssemblyDirectory, "Routines\\TuanHADruidWoD");

            string path = settingsDirectory + @"\TuanHA_Combat_Routine.dll";

            if (!File.Exists(path))
            {
                MessageBox.Show("TuanHADruidWoD is not installed correctly! Ensure files look like;" +
                            Environment.NewLine +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHADruidWoD/Files/<.doc .jpg>" +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHADruidWoD/Preset/<XML Files>" +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHADruidWoD/Settings/<XML Files>" +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHADruidWoD/User Settings/<XML Files>" +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHADruidWoD/ChangeLog.txt" +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHADruidWoD/Loader.cs" +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHADruidWoD/Please Read - Installation Guide.txt" +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHADruidWoD/TuanHA_Combat_Routine.dll");
                return;
            }

            bool removed = false;
            var DInfo = new DirectoryInfo(settingsDirectory);
            foreach (FileInfo file in DInfo.GetFiles())
            {
                if (!Keep.Contains(file.Name))
                {
                    removed = true;
                    Logging.Write("Removing " + file.Name + " from TuanHADruidWoD directory");
                    file.Delete();
                }
            }

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
                {
                    try
                    {
                        AssemblyName requestedName = new AssemblyName(e.Name);
                        if (requestedName.Name == "Honorbuddy")
                        {
                            return Assembly.LoadFile(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                        }
                        if (requestedName.Name == "GreyMagic")
                        {
                            return Assembly.LoadFile(Utilities.AssemblyDirectory + @"\GreyMagic.dll");
                        }
                        return null;
                    }
                    catch (System.Exception)
                    {
                        return null;
                    }
                };

                byte[] Bytes = File.ReadAllBytes(path);
                Assembly asm = Assembly.Load(Bytes);

                foreach (Type t in asm.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(CombatRoutine)) && t.IsClass)
                    {
                        object obj = Activator.CreateInstance(t);
                        CC = (CombatRoutine)obj;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
                Logging.Write(errorMessage);
            }
            catch (Exception e)
            {
                Logging.Write(Colors.DarkRed, "An error occured while loading TuanHADruidWoD!");
                Logging.Write(e.ToString());
            }
        }

        #region Overrides of CombatRoutine

        public override string Name
        {
            get
            {
                return CC.Name;
            }
        }

        public override void Initialize()
        {
            CC.Initialize();
        }

        public override Composite CombatBehavior
        {
            get { return CC.CombatBehavior; }
        }

        public override Composite PreCombatBuffBehavior
        {
            get { return CC.PreCombatBuffBehavior; }
        }

        //public override Composite PullBehavior
        //{
        //    get { return CC.PullBehavior; }
        //}

        public override Composite RestBehavior
        {
            get { return CC.RestBehavior; }
        }

        public override Composite DeathBehavior
        {
            get { return CC.DeathBehavior; }
        }

        public override WoWClass Class
        {
            get
            {
                return WoWClass.Druid;
            }
        }

        public override void OnButtonPress()
        {
            CC.OnButtonPress();
        }

        public override void Pulse()
        {
            CC.Pulse();
        }

        public override bool WantButton
        {
            get { return true; }
        }

        #endregion
    }
}
