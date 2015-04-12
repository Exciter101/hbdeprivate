// Credits to Apoc, Main and Aash

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace HighVoltz
{
    internal struct WorldMapCoord : IEquatable<object>
    {
        public WorldMapCoord(uint mapId, float x, float y)
        {
            MapID = mapId;
            X = x;
            Y = y;
        }
        public readonly uint MapID;
        public readonly float X;
        public readonly float Y;

        #region IEquatable<object> Members

        public override bool Equals(object obj)
        {
            if (!(obj is WorldMapCoord))
                return false;
            var wmc = (WorldMapCoord)obj;
            return wmc.X == X && wmc.Y == Y && wmc.MapID == MapID;
        }

        #endregion

        public override int GetHashCode()
        {
            return (int)((X * 100F) + (Y * 100f) + (MapID * 100f));
        }

        public static bool operator ==(WorldMapCoord a, WorldMapCoord b)
        {
            return a.X == b.X && a.Y == b.Y && a.MapID == b.MapID;
        }

        public static bool operator !=(WorldMapCoord a, WorldMapCoord b)
        {
            return a.X != b.X || a.Y != b.Y || a.MapID != b.MapID;
        }
    }

    internal class ClickToGo : BotBase
    {
        private static WorldMapCoord _oldMapCoord;
        private static WorldMapCoord _mapCoord;
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static ClickToGoSettings MySettings;
        private static readonly Random Rand = new Random();
        private const NumberStyles Style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
        private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-US");
        private readonly Version _version = new Version(1, 13);
        private WoWPoint _gotoLocation;
        public static ClickToGo Instance { get; private set; }

        public ClickToGo()
        {
            Instance = this;
        }

        public override string Name
        {
            get { return "ClickToGo" + " " + Version; }
        }

        private Composite _root;
        public override Composite Root
        {
            get
            {
                return _root ?? (_root = new PrioritySelector(
                    new Decorator(ctx => !StyxWoW.IsInGame,
                        new ActionAlwaysSucceed()),

                    // combat behaviors.
                    new Decorator(c => !Me.IsAlive,
                              Bots.Grind.LevelBot.CreateDeathBehavior()),
                    new Decorator(ctx => !Me.IsFlying,
                        new PrioritySelector(
                            new Decorator(ctx => !Me.Combat,
                                new PrioritySelector(
                                    RoutineManager.Current.RestBehavior,
                                    RoutineManager.Current.PreCombatBuffBehavior)),
                            new Decorator(ctx => Me.Combat,
                                new PrioritySelector(
                                    Bots.Grind.LevelBot.CreateCombatBehavior())))),

                    // moveto behavior
                    new Decorator(ctx => _gotoLocation != WoWPoint.Zero,
                        new PrioritySelector(
                            new Decorator(ctx => _gotoLocation.DistanceSqr(Me.Location) > 5 * 5,
                                new PrioritySelector(
                                    new Decorator(ctx => Flightor.MountHelper.CanMount,
                                        new Action(ctx => Flightor.MoveTo(_gotoLocation))),
                                    new Action(ctx => Navigator.MoveTo(_gotoLocation)))),
                            new Decorator(ctx => _gotoLocation.DistanceSqr(Me.Location) <= 5 * 5,
                                new Action(ctx => _gotoLocation = WoWPoint.Zero))))));
            }
        }

        public override PulseFlags PulseFlags
        {
            get { return PulseFlags.All; }
        }

        //public override Form ConfigurationForm
        //{
        //    get { return _gui; }
        //}

        public Version Version
        {
            get { return _version; }
        }

        public static string RandomString
        {
            get
            {
                int size = Rand.Next(6, 15);
                var sb = new StringBuilder(size);
                for (int i = 0; i < size; i++)
                {
                    // random upper/lowercase character using ascii code
                    sb.Append((char)(Rand.Next(2) == 1 ? Rand.Next(65, 91) + 32 : Rand.Next(65, 91)));
                }
                return sb.ToString();
            }
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public override void Initialize()
        {
            MySettings = new ClickToGoSettings(Path.Combine(Application.StartupPath,
                                                            string.Format(@"Settings\\{0}\\{0}-{1}.xml", Name, Me.Name)));
            if (MySettings.WorldMapCoordName == "")
            {
                MySettings.WorldMapCoordName = RandomString;
                MySettings.GetWorldMapCoordName = RandomString;
                MySettings.WorldMapButtonOnClickHook = RandomString;
                MySettings.WorldMapButtonOnUpdateHook = RandomString;
                MySettings.WorldMapButtonOldOnClickHook = RandomString;
                MySettings.Save();
            }
            InitWorldMapClickOverride();
        }

        public override void Pulse()
        {
            try
            {
                if (!StyxWoW.IsInGame)
                    return;
                _mapCoord = GetWorldMapCoord();
                if (_mapCoord != _oldMapCoord)
                {
                    _oldMapCoord = _mapCoord;
                    _gotoLocation = MapToWorldCoords(_mapCoord);
                    if (_gotoLocation == WoWPoint.Empty)
                    {
                        Log("Unable to Genorate path to location. Try a different spot nearby");
                        return;
                    }
                    Log("{0}", ToHotSpot(_gotoLocation));
                }
            }
            catch (ThreadAbortException)
            {
            }

            catch (Exception e)
            {
                Log("Exception in Pulse:{0}", e);
            }
        }

        private static WoWPoint MapToWorldCoords(WorldMapCoord wmc)
        {
            var worldPoint = new WoWPoint(wmc.X, wmc.Y, 0);
            /* world coords are now calculated in lua.
            WoWDb.DbTable worldMapArea = StyxWoW.Db[ClientDb.WorldMapArea];
            WoWDb.Row worldMapAreaFields = worldMapArea.GetRow(wmc.MapID);
            var ay = worldMapAreaFields.GetField<float>(4);
            var by = worldMapAreaFields.GetField<float>(5);
            var ax = worldMapAreaFields.GetField<float>(6);
            var bx = worldMapAreaFields.GetField<float>(7);
            worldPoint.X = ax + (wmc.Y * (bx - ax));
            worldPoint.Y = ay + (wmc.X * (by - ay));
             */
            try
            {
                worldPoint.Z = Navigator.FindHeights(worldPoint.X, worldPoint.Y).Max();
            }
            catch
            {
                Log("Clicked at location: {0}", worldPoint);
                return WoWPoint.Empty;
            }
            Log("Clicked at location: {0}", worldPoint);
            return worldPoint;
        }

        private static WorldMapCoord GetWorldMapCoord()
        {
            uint mapId = 0;
            float x = 0f, y = 0f;
            List<string> ret = Lua.GetReturnValues(string.Format("return {0}()", MySettings.GetWorldMapCoordName),
                                                   "clicky.lua");
            if (ret != null && ret.Count == 3)
            {
                float.TryParse(ret[0], Style, Culture, out x);
                float.TryParse(ret[1], Style, Culture, out y);
                uint.TryParse(ret[2], Style, Culture, out mapId);
            }
            return new WorldMapCoord(mapId, x, y);
        }

        public static void Log(string msg, params object[] args)
        {
            Logging.Write(Instance.Name + ": " + msg, args);
        }

        public static void Log(Color c, string msg, params object[] args)
        {
            Logging.Write(c, msg, args);
        }

        private static string ToHotSpot(WoWPoint point)
        {
            return string.Format(Culture, "<Hotspot X=\"{0}\" Y=\"{1}\" Z=\"{2}\" />", point.X, point.Y, point.Z);
        }

        public void InitWorldMapClickOverride()
        {
            string getWorldMapCoordLua =
                "function " + MySettings.GetWorldMapCoordName + "() " +
                    "if " + MySettings.WorldMapCoordName + " then " +
                        "return unpack({" +
                            MySettings.WorldMapCoordName + "['u'], " +
                            MySettings.WorldMapCoordName + "['v'], " +
                            MySettings.WorldMapCoordName + "['map_id']}) " +
                       "else " +
                            "return nil " +
                    "end " +
                "end ";

            var worldMapOnClickLua =
                "function " + MySettings.WorldMapButtonOnClickHook + "(button, mouseButton) " +
                    "CloseDropDownMenus(); " +
                    "if ( mouseButton == 'LeftButton' ) then " +
                        "local x, y = GetCursorPosition() " +
                        "x = x / button:GetEffectiveScale() " +
                        "y = y / button:GetEffectiveScale() " +
                        "local centerX, centerY = button:GetCenter() " +
                        "local width = button:GetWidth() " +
                        "local height = button:GetHeight() " +
                        "local adjustedY = (centerY + (height/2) - y) / height " +
                        "local adjustedX = (x - (centerX - (width/2))) / width " +

                        "local mapLevel, dleft, dtop, dright, dbott = GetCurrentMapDungeonLevel() " +
                        "local mapArea, left, top, right, bott = GetCurrentMapZone() " +
                        "if mapLevel~= 0 or mapArea ~= 0 then " +
                            // are we in a dungeon or multi-layer zone like Dalaran?
                            "if mapLevel > 0 and dbott ~= nil then " +
                                MySettings.WorldMapCoordName + "['u'] = dbott - (dbott - dtop) * adjustedY " +
                                MySettings.WorldMapCoordName + "['v'] = dright - (dright-dleft)*adjustedX " +
                                MySettings.WorldMapCoordName + "['map_id'] = GetCurrentMapAreaID() " +
                            // only save mouse click if we're not viewing a continent map.
                            "elseif mapArea > 0 then " +
                                MySettings.WorldMapCoordName + "['u'] = (1.0-adjustedY)*top+adjustedY*bott " +
                                MySettings.WorldMapCoordName + "['v'] = (1.0-adjustedX)*left+adjustedX*right " +
                                MySettings.WorldMapCoordName + "['map_id'] = GetCurrentMapAreaID() " +
                            "end " +
                            // WorldMap ping change in WoD and currently don't have the time to fix it.
                            //"WorldMapPing:Show() " +
                            //"WorldMapPing:SetPoint('TOPLEFT', 'WorldMapDetailFrame', 'CENTER',(x - centerX ) - 24,(y - centerY)+ 24) " +
                        "end " +
                    "end " +
                    MySettings.WorldMapButtonOldOnClickHook + "(button, mouseButton) " +
                "end ";

            var worldMapOnUpdateLua =
                "function " + MySettings.WorldMapButtonOnUpdateHook + "(self, elapsed) " +
                    "if GetCurrentMapAreaID() == " + MySettings.WorldMapCoordName + "[3] then " +
                        "WorldMapPing:Show() " +
                        "WorldMapPing:SetPoint('CENTER', 'WorldMapDetailFrame', 'TOPLEFT', " + MySettings.WorldMapCoordName +
                        "[1] * WorldMapDetailFrame:GetWidth()," + MySettings.WorldMapCoordName +
                        "[2] * WorldMapDetailFrame:GetHeight()) " +
                    "else " +
                        "WorldMapPing:Hide() " +
                    "end " +
                "end ";

            var myLua = MySettings.WorldMapCoordName + "= {} " +
                           getWorldMapCoordLua +
                           worldMapOnClickLua +
                           "if not " + MySettings.WorldMapButtonOldOnClickHook + " then " +
                           MySettings.WorldMapButtonOldOnClickHook + "=WorldMapButton:GetScript('OnMouseUp') " +
                           "end " +
                           "WorldMapButton:SetScript('OnMouseUp'," + MySettings.WorldMapButtonOnClickHook + ") " +
                           worldMapOnUpdateLua +
                           "WorldMapButton:HookScript('OnUpdate'," + MySettings.WorldMapButtonOnUpdateHook + ") ";
            Lua.DoString(myLua);
        }

        #region Nested type: ClickToGoSettings

        public class ClickToGoSettings : Settings
        {
            public ClickToGoSettings(string settingsPath)
                : base(settingsPath)
            {
                Load();
            }

            [Setting, Styx.Helpers.DefaultValue(true)]
            public bool Fly { get; set; }

            [Setting, Styx.Helpers.DefaultValue("")]
            public string WorldMapCoordName { get; set; }

            [Setting, Styx.Helpers.DefaultValue("")]
            public string GetWorldMapCoordName { get; set; }

            [Setting, Styx.Helpers.DefaultValue("")]
            public string WorldMapButtonOnClickHook { get; set; }

            [Setting, Styx.Helpers.DefaultValue("")]
            public string WorldMapButtonOldOnClickHook { get; set; }

            [Setting, Styx.Helpers.DefaultValue("")]
            public string WorldMapButtonOnUpdateHook { get; set; }
        }

        #endregion
    }
}