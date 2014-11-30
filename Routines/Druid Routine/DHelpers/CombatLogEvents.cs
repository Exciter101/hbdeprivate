#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: apoc $
// $Date: 2012-09-05 16:16:37 +0000 (Mi, 05 Sep 2012) $
// $HeadURL: https://subversion.assembla.com/svn/singular_v3/trunk/Singular/Utilities/CombatLog.cs $
// $LastChangedBy: apoc $
// $LastChangedDate: 2012-09-05 16:16:37 +0000 (Mi, 05 Sep 2012) $
// $LastChangedRevision: 653 $
// $Revision: 653 $

#endregion

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Common;
using System.Windows.Media;

#region methods
using Form1 = Druid.DGUI.Form1;
using HKM = Druid.Helpers.HotkeyManager;
using S = Druid.DSpells.SpellCasts;
using CL = Druid.Handlers.CombatLogEventArgs;
using EH = Druid.Handlers.EventHandlers;
using L = Druid.Helpers.Logs;
using T = Druid.Helpers.targets;
using U = Druid.Helpers.Unit;
using UI = Druid.Helpers.UseItems;
using P = Druid.DSettings.DruidPrefs;
using M = Druid.Helpers.Movement;
using I = Druid.Helpers.Interrupts;
#endregion

namespace Druid.Handlers
{
    internal class CombatLogEventArgs : LuaEventArgs
    {
        #region line of sight
        public static bool IsNotInLineOfSight = false;
        public static void CombatLogErrorHandler(object sender, LuaEventArgs args)
        {

            foreach (object arg in args.Args)
            {

                var s = (string)arg;

                //Logging.Write(Colors.Red, "Error message = " + s.ToUpper());
                string errorLog = s.ToUpper();

                if (errorLog == "TARGET NOT IN LINE OF SIGHT")
                {
                    Lua.DoString("StopAttack()");
                    IsNotInLineOfSight = true;
                }
                else { IsNotInLineOfSight = false; }
            }
        }
        #endregion

        public CombatLogEventArgs(string eventName, uint fireTimeStamp, object[] args)
            : base(eventName, fireTimeStamp, args)
        {
        }

        public double Timestamp { get { return (double)Args[0]; } }

        public string Event { get { return Args[1].ToString(); } }

        // Is this a string? bool? what? What the hell is it even used for?
        // it's a boolean, and it doesn't look like it has any real impact codewise apart from maybe to break old addons? - exemplar 4.1
        public string HideCaster { get { return Args[2].ToString(); } }

        public WoWGuid SourceGuid { get { return ArgToGuid(Args[3]); } }

        public WoWUnit SourceUnit
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, true).FirstOrDefault(
                        o => o.IsValid && (o.Guid == SourceGuid || o.DescriptorGuid == SourceGuid));
            }
        }

        public string SourceName { get { return Args[4].ToString(); } }

        public int SourceFlags { get { return (int)(double)Args[5]; } }

        public WoWGuid DestGuid { get { return ArgToGuid(Args[7]); } }

        public WoWUnit DestUnit
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, true).FirstOrDefault(
                        o => o.IsValid && (o.Guid == DestGuid || o.DescriptorGuid == DestGuid));
            }
        }

        public string DestName { get { return Args[8].ToString(); } }

        public int DestFlags { get { return (int)(double)Args[9]; } }

        public int SpellId { get { return (int)(double)Args[11]; } }

        public WoWSpell Spell { get { return WoWSpell.FromId(SpellId); } }

        public string SpellName { get { return Args[12].ToString(); } }

        public WoWSpellSchool SpellSchool { get { return (WoWSpellSchool)(int)(double)Args[13]; } }

        public object[] SuffixParams
        {
            get
            {
                var args = new List<object>();
                for (int i = 11; i < Args.Length; i++)
                {
                    if (Args[i] != null)
                    {
                        args.Add(args[i]);
                    }
                }
                return args.ToArray();
            }
        }
        private static WoWGuid ArgToGuid(object o)
        {
            string s = o.ToString();
            WoWGuid guid;
            if (!WoWGuid.TryParseFriendly(s, out guid))
                guid = WoWGuid.Empty;

            return guid;
        }
    }
}