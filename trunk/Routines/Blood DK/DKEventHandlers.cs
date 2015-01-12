#region

using System;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;
using System.Windows.Media;




#endregion

namespace DK
{
    public static class EventHandlers
    {
        public static bool CombatLogAttached;

        /// <summary>
        /// time of last "Target not in line of sight" spell failure.
        /// Used by movement functions for situations where the standard
        /// LoS and LoSS functions are true but still fails in WOW.
        /// See CreateMoveToLosBehavior() for usage
        /// </summary>
        internal static DateTime LastLineOfSightError { get; set; }

        internal static DateTime LastUnitNotInfrontError { get; set; }
        internal static DateTime LastShapeshiftError { get; set; }

        /// <summary>
        /// the value of localized values for testing certain types of spell failures
        /// </summary>
        internal static string LocalizedLineOfSightError;

        internal static string LocalizedUnitNotInfrontError;

        internal static string LocalizedAnotheractionisinprogress;

        internal static string CastFailReason;

        internal static HashSet<string> LocalizedShapeshiftErrors;

        public static void AttachCombatLogEvent()
        {
            if (CombatLogAttached)
                return;

            // DO NOT EDIT THIS UNLESS YOU KNOW WHAT YOU'RE DOING!
            // This ensures we only capture certain combat log events, not all of them.
            // This saves on performance, and possible memory leaks. (Leaks due to Lua table issues.)
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);

            string filterCriteria =
                "return args[4] == UnitGUID('player')"
                + " and (args[2] == 'SPELL_MISSED'"
                + " or args[2] == 'RANGE_MISSED'"
                + " or args[2] == 'SWING_MISSED')";
            //+ " or args[2] == 'SPELL_CAST_START'"
            //+ " or args[2] == 'SPELL_CAST_SUCCESS'"
            //+ " or args[2] == 'SPELL_CAST_FAILED')";

            if (!Lua.Events.AddFilter("COMBAT_LOG_EVENT_UNFILTERED", filterCriteria))
            {
                Logging.Write(
                    "ERROR: Could not add combat log event filter! - Performance may be horrible, and things may not work properly!");
            }

            // get localized copies of spell failure error messages
            LocalizedLineOfSightError = Lua.GetReturnVal<string>("return SPELL_FAILED_LINE_OF_SIGHT", 0);
            LocalizedUnitNotInfrontError = Lua.GetReturnVal<string>("return SPELL_FAILED_UNIT_NOT_INFRONT", 0);
            LocalizedAnotheractionisinprogress = Lua.GetReturnVal<string>("return SPELL_FAILED_SPELL_IN_PROGRESS", 0);

            LocalizedShapeshiftErrors = new HashSet<string>();
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return ERR_CANT_INTERACT_SHAPESHIFTED", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return ERR_MOUNT_SHAPESHIFTED", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return ERR_NOT_WHILE_SHAPESHIFTED", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return ERR_NO_ITEMS_WHILE_SHAPESHIFTED", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return ERR_SHAPESHIFT_FORM_CANNOT_EQUIP", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return ERR_TAXIPLAYERSHAPESHIFTED", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return SPELL_FAILED_CUSTOM_ERROR_125", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return SPELL_FAILED_CUSTOM_ERROR_99", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return SPELL_FAILED_NOT_SHAPESHIFT", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return SPELL_FAILED_NO_ITEMS_WHILE_SHAPESHIFTED", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return SPELL_NOT_SHAPESHIFTED", 0));
            LocalizedShapeshiftErrors.Add(Lua.GetReturnVal<string>("return SPELL_NOT_SHAPESHIFTED_NOSPACE", 0));

            LastLineOfSightError = DateTime.MinValue;
            LastUnitNotInfrontError = DateTime.MinValue;
            LastShapeshiftError = DateTime.MinValue;

            Logging.Write("AttachCombatLogEvent");
            CombatLogAttached = true;
        }

        public static void DetachCombatLogEvent()
        {
            if (!CombatLogAttached)
                return;

            Logging.Write("DetachCombatLogEvent");
            Logging.Write("Removed combat log filter");
            Lua.Events.RemoveFilter("COMBAT_LOG_EVENT_UNFILTERED");
            Logging.Write("Detached combat log");
            Lua.Events.DetachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);
            CombatLogAttached = false;
        }

        internal static void HandleCombatLog(object sender, LuaEventArgs args)
        {
            var e = new CombatLogEventArgs(args.EventName, args.FireTimeStamp, args.Args);
            if (e.SourceGuid != StyxWoW.Me.Guid)
                return;

            //Logging.Write("[CombatLog] " + e.Event + " - " + e.SourceName + " - " + e.Spell.Name);

            switch (e.Event)
            {
                default:
                    Logging.Write("[CombatLog] filter out this event -- " + e.Event + " - " + e.SourceName + " - " +
                                  e.Spell.Name);
                    break;

                // spell_cast_failed only passes filter in Singular debug mode
                case "SPELL_CAST_FAILED":
                    CastFailReason = e.Args[14].ToString();

                    if (CastFailReason != "Your target is dead" &&
                        CastFailReason != "No target" &&
                        CastFailReason != "Not yet recovered")
                    {
                        Logging.Write(LogLevel.Diagnostic, "[CombatLog] {0} {1}#{2} failure: '{3}'", e.Event,
                                      e.Spell.Name,
                                      e.SpellId,
                                      CastFailReason);
                    }
                    break;

                case "SWING_MISSED":
                    if (e.Args[11].ToString() == "EVADE")
                    {
                        HandleEvadeBuggedMob(args, e);
                    }
                    else if (e.Args[11].ToString() == "IMMUNE")
                    {
                        WoWUnit unit = e.DestUnit;
                        if (unit != null && !unit.IsPlayer)
                        {
                            Logging.Write("{0} is immune to Physical spell school", unit.Name);
                        }
                    }
                    break;

                case "SPELL_MISSED":
                case "RANGE_MISSED":
                    // Why log misses?  Because users of classes with DoTs testing on training dummies
                    // .. that they don't have enough +Hit for will get DoT spam.  This allows easy
                    // .. diagnosis of false reports of rotation issues where a user simply isn't geared
                    // .. this happens more at the beginning of an expansion especially
                    Logging.Write(
                        "[CombatLog] {0} {1}#{2} {3}",
                        e.Event,
                        e.Spell.Name,
                        e.SpellId,
                        e.Args[14]
                        );

                    if (e.Args[14].ToString() == "EVADE")
                    {
                        HandleEvadeBuggedMob(args, e);
                    }
                    else if (e.Args[14].ToString() == "IMMUNE")
                    {
                        WoWUnit unit = e.DestUnit;
                        if (unit != null && !unit.IsPlayer)
                        {
                            Logging.Write("{0} is immune to {1} spell school", unit.Name, e.SpellSchool);
                        }
                    }
                    break;
            }
        }

        public static Dictionary<WoWGuid, int> MobsThatEvaded = new Dictionary<WoWGuid, int>();

        internal static void HandleEvadeBuggedMob(LuaEventArgs args, CombatLogEventArgs e)
        {
            WoWUnit unit = e.DestUnit;
            WoWGuid guid = e.DestGuid;

            if (unit == null && StyxWoW.Me.CurrentTarget != null)
            {
                unit = StyxWoW.Me.CurrentTarget;
                guid = StyxWoW.Me.CurrentTargetGuid;
                Logging.Write("Evade: bugged mob guid:{0}, so assuming current target instead", args.Args[7]);
            }

            if (unit != null)
            {
                if (!MobsThatEvaded.ContainsKey(unit.Guid))
                    MobsThatEvaded.Add(unit.Guid, 0);

                MobsThatEvaded[unit.Guid]++;
                if (MobsThatEvaded[unit.Guid] <= 5)
                {
                    Logging.Write("Mob {0} has evaded {1} times.  Keeping an eye on {2:X0} for now!", unit.Name,
                                  MobsThatEvaded[unit.Guid], unit.Guid);
                }
                else
                {
                    const int secondsToBlacklist = 60;

                    if (Blacklist.Contains(unit.Guid, BlacklistFlags.Combat))
                        Logging.Write(Colors.LightGoldenrodYellow,
                                      "Mob {0} has evaded {1} times. Previously blacklisted {2:X0} for {3} seconds!",
                                      unit.Name, MobsThatEvaded[unit.Guid], unit.Guid, secondsToBlacklist);
                    else
                    {
                        Logging.Write(Colors.LightGoldenrodYellow,
                                      "Mob {0} has evaded {1} times. Blacklisting {2:X0} for {3} seconds!", unit.Name,
                                      MobsThatEvaded[unit.Guid], unit.Guid, secondsToBlacklist);
                        Blacklist.Add(unit.Guid, BlacklistFlags.Combat, TimeSpan.FromSeconds(secondsToBlacklist));
                        if (!Blacklist.Contains(unit.Guid, BlacklistFlags.Combat))
                        {
                            Logging.Write(Colors.Pink, "error: blacklist does not contain entry for {0} so adding {1}",
                                          unit.Name, unit.Guid);
                        }
                    }

                    if (BotPoi.Current.Guid == unit.Guid)
                    {
                        Logging.Write("EvadeHandling: Current BotPOI type={0} is Evading, clearing now...",
                                      BotPoi.Current.Type);
                        BotPoi.Clear("Singular recognized Evade bugged mob");
                    }

                    if (StyxWoW.Me.CurrentTargetGuid == guid)
                    {
                        foreach (var target in Targeting.Instance.TargetList)
                        {
                            if (target != null &&
                                target.IsValid &&
                                target.IsAlive &&
                                !Blacklist.Contains(target, BlacklistFlags.Combat))
                            {
                                Logging.Write(Colors.Pink, "Setting target to {0} to get off evade bugged mob!",
                                              target.Name);
                                target.Target();
                                return;
                            }
                        }

                        Logging.Write(Colors.Pink,
                                      "BotBase has 0 entries in Target list not blacklisted -- nothing else we can do at this point!");
                        // StyxWoW.Me.ClearTarget();
                    }
                }
            }

            /// line below was originally in Evade logic, but commenting to avoid Sleeps
            // StyxWoW.SleepForLagDuration();
        }
    }
}