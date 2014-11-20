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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Action = Styx.TreeSharp.Action;

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

namespace Druid.DRotations
{
    class Feral
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Composite FeralRot()
        {
            return new S.FrameLockSelector(
                S.castSwitchBearform(),
                T.LookToRes(),
                new Decorator(ret => P.myPrefs.Trinket1 == 4 && Me.HealthPercent <= P.myPrefs.PercentTrinket1HP, UI.useTrinket1()),
                new Decorator(ret => P.myPrefs.Trinket2 == 4 && Me.HealthPercent <= P.myPrefs.PercentTrinket2HP, UI.useTrinket2()),
                new Decorator(ret => P.myPrefs.Trinket1 == 5 && Me.ManaPercent <= P.myPrefs.PercentTrinket1Mana, UI.useTrinket1()),
                new Decorator(ret => P.myPrefs.Trinket2 == 5 && Me.ManaPercent <= P.myPrefs.PercentTrinket2Mana, UI.useTrinket2()),
                new Decorator(ret => P.myPrefs.Trinket1 == 2 && HKM.cooldownsOn, UI.useTrinket1()),
                new Decorator(ret => P.myPrefs.Trinket2 == 2 && HKM.cooldownsOn, UI.useTrinket2()),
                new Decorator(ret => P.myPrefs.Trinket1 == 3 && T.IsWoWBoss(Me.CurrentTarget), UI.useTrinket1()),
                new Decorator(ret => P.myPrefs.Trinket2 == 3 && T.IsWoWBoss(Me.CurrentTarget), UI.useTrinket2()),
                new Decorator(ret => S.gotTarget && S.needAoe,
                    new PrioritySelector(
                        S.castCatForm(),
                        S.castSavageRoar(),
                        S.castSkullBash(),
                        S.castWarStomp(),
                        S.castMightyBash(),
                        S.castTyphoon(),
                        S.castIncapacitatingRoar(),
                        S.castHealingTouchMe(),
                        S.castHealingTouchOthers(),
                        S.castTigersFury(),
                        S.castFoN(),
                        S.castThrashAoe(),
                        S.castSwipe()
                        )),
                new Decorator(ret => S.gotTarget && !S.needAoe,
                    new PrioritySelector(
                        S.castCatForm(),
                        S.castThrash(),
                        S.castSavageRoar(),
                        S.castSkullBash(),
                        S.castWarStomp(),
                        S.castMightyBash(),
                        S.castTyphoon(),
                        S.castIncapacitatingRoar(),
                        S.castHealingTouchMe(),
                        S.castHealingTouchOthers(),
                        S.castTigersFury(),
                        S.castBerserk(),
                        S.castIncarnation(),
                        S.castBerserking(),
                        S.castHotW(),
                        S.castFoNBurst(),
                        S.castFoN(),
                        S.castRip(),
                        S.castRipLow(),
                        S.castFBR(),
                        S.castFB(),
                        S.castFBLow(),
                        S.castRake(),
                        S.castRakeLow(),
                        S.castShred()
                )));
        }
        private static bool _loop;
        private static bool _pulling;
        private static string lastSpell { get; set; }
        public const string
            SAVAGE_ROAR = "Savage Roar",
            TIGERS_FURY = "Tiger's Fury",
            BERSERK = "Berserk",
            INCARNATION = "Incarnation: King of the Jungle",
            FORCE_OF_NATURE = "Force of Nature",
            FEROCIOUS_BITE = "Ferocious Bite",
            RIP = "Rip",
            RAKE = "Rake",
            SHRED = "Shred",
            THRASH = "Thrash",
            SWIPE = "Swipe",
            HEALING_TOUCH = "Healing Touch",
            REJUVENATION = "Rejuvenation",
            SKULL_BASH = "Skull Bash",
            TYPHOON = "Typhoon",
            INCAPACITATING_ROAR = "Incapacitating Roar",
            MIGHTY_BASH = "Mighty Bash",
            WAR_STOMP = "War Stomp",
            CAT_FORM = "Cat Form",
            SUVIVAL_INSTINCTS = "Survival Instincts",
            PROWL = "Prowl",
            einde = "einde";
        public static int HEALING_TOUCHI = 5185;
        public static Composite feral2rot()
        {
            return new S.FrameLockSelector(
                new Decorator(ret => Me.Combat && S.gotTarget && Me.CurrentTarget.IsWithinMeleeRange,
                    new Action(ret =>
                    {
                        _loop = true;
                        while (_loop)
                        {
                            try
                            {
                                #region aoe
                                if (S.needAoe)
                                {
                                    if (SpellManager.HasSpell(CAT_FORM)
                                        && Me.Shapeshift != ShapeshiftForm.Cat)
                                    {
                                        SpellManager.Cast(CAT_FORM);
                                        Logging.Write(CAT_FORM);
                                        lastSpell = CAT_FORM;
                                    }
                                    if (SpellManager.HasSpell(SAVAGE_ROAR)
                                        && !Me.HasAura(SAVAGE_ROAR)
                                        && Me.ComboPoints >= 1
                                        && Me.EnergyPercent >= 25
                                        && lastSpell != SAVAGE_ROAR
                                        && SpellManager.CanCast(SAVAGE_ROAR))
                                    {
                                        SpellManager.Cast(SAVAGE_ROAR);
                                        Logging.Write(SAVAGE_ROAR);
                                        lastSpell = SAVAGE_ROAR;
                                    }
                                    else if (SpellManager.HasSpell(SUVIVAL_INSTINCTS)
                                        && !U.spellOnCooldown(SUVIVAL_INSTINCTS)
                                        && Me.HealthPercent <= P.myPrefs.PercentSurvivalInstincts
                                        && lastSpell != SUVIVAL_INSTINCTS
                                        && SpellManager.CanCast(SUVIVAL_INSTINCTS))
                                    {
                                        SpellManager.Cast(SUVIVAL_INSTINCTS);
                                        Logging.Write(SUVIVAL_INSTINCTS);
                                        lastSpell = SUVIVAL_INSTINCTS;
                                    }
                                    else if (SpellManager.HasSpell(REJUVENATION)
                                        && Me.HealthPercent <= P.myPrefs.PercentRejuCombat
                                        && !Me.HasAura(REJUVENATION)
                                        && lastSpell != REJUVENATION
                                        && SpellManager.CanCast(REJUVENATION))
                                    {
                                        SpellManager.Cast(REJUVENATION, Me);
                                        Logging.Write(REJUVENATION);
                                        lastSpell = REJUVENATION;
                                    }
                                    else if (SpellManager.HasSpell(HEALING_TOUCH)
                                        && S.IsOverlayed(HEALING_TOUCHI)
                                        && lastSpell != HEALING_TOUCH
                                        && SpellManager.CanCast(HEALING_TOUCH))
                                    {
                                        SpellManager.Cast(HEALING_TOUCH);
                                        Logging.Write(HEALING_TOUCH);
                                        lastSpell = HEALING_TOUCH;
                                    }
                                    else if (SpellManager.HasSpell(SKULL_BASH)
                                        && !U.spellOnCooldown(SKULL_BASH)
                                        && (Me.CurrentTarget.IsCasting
                                        && I.ItsTimeToInterrupt)
                                        && lastSpell != SKULL_BASH
                                        && interruptTime >= DateTime.Now
                                        && SpellManager.CanCast(SKULL_BASH))
                                    {
                                        SpellManager.Cast(SKULL_BASH);
                                        Logging.Write(SKULL_BASH);
                                        lastSpell = SKULL_BASH;
                                        interruptTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(INCAPACITATING_ROAR)
                                        && !U.spellOnCooldown(INCAPACITATING_ROAR)
                                        && (Me.CurrentTarget.IsCasting
                                        && I.ItsTimeToInterrupt)
                                        && lastSpell != INCAPACITATING_ROAR
                                        && interruptTime >= DateTime.Now
                                        && SpellManager.CanCast(INCAPACITATING_ROAR))
                                    {
                                        SpellManager.Cast(INCAPACITATING_ROAR);
                                        Logging.Write(INCAPACITATING_ROAR);
                                        lastSpell = INCAPACITATING_ROAR;
                                        interruptTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(TYPHOON)
                                        && !U.spellOnCooldown(TYPHOON)
                                        && (Me.CurrentTarget.IsCasting
                                        && I.ItsTimeToInterrupt)
                                        && lastSpell != TYPHOON
                                        && interruptTime >= DateTime.Now
                                        && SpellManager.CanCast(TYPHOON))
                                    {
                                        SpellManager.Cast(TYPHOON);
                                        Logging.Write(TYPHOON);
                                        lastSpell = TYPHOON;
                                        interruptTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(MIGHTY_BASH)
                                        && !U.spellOnCooldown(MIGHTY_BASH)
                                        && (Me.CurrentTarget.IsCasting
                                        && !I.ItsTimeToInterrupt)
                                        && lastSpell != MIGHTY_BASH
                                        && stunTime >= DateTime.Now
                                        && SpellManager.CanCast(MIGHTY_BASH))
                                    {
                                        SpellManager.Cast(MIGHTY_BASH);
                                        Logging.Write(MIGHTY_BASH);
                                        lastSpell = MIGHTY_BASH;
                                        stunTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(WAR_STOMP)
                                        && !U.spellOnCooldown(WAR_STOMP)
                                        && (Me.CurrentTarget.IsCasting
                                        && !I.ItsTimeToInterrupt)
                                        && lastSpell != WAR_STOMP
                                        && stunTime >= DateTime.Now
                                        && SpellManager.CanCast(WAR_STOMP))
                                    {
                                        SpellManager.Cast(WAR_STOMP);
                                        Logging.Write(WAR_STOMP);
                                        lastSpell = WAR_STOMP;
                                        stunTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(TIGERS_FURY)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && !U.spellOnCooldown(TIGERS_FURY)
                                        && Me.EnergyPercent < 35
                                        && lastSpell != TIGERS_FURY
                                        && SpellManager.CanCast(TIGERS_FURY))
                                    {
                                        SpellManager.Cast(TIGERS_FURY);
                                        Logging.Write(TIGERS_FURY);
                                        lastSpell = TIGERS_FURY;
                                    }
                                    else if (SpellManager.HasSpell(BERSERK)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && !U.spellOnCooldown(BERSERK)
                                        && lastSpell != BERSERK
                                        && T.IsWoWBoss(Me.CurrentTarget)
                                        && SpellManager.CanCast(BERSERK))
                                    {
                                        SpellManager.Cast(BERSERK);
                                        Logging.Write(BERSERK);
                                        lastSpell = BERSERK;
                                    }
                                    else if (SpellManager.HasSpell(INCARNATION)
                                        && !U.spellOnCooldown(INCARNATION)
                                        && Me.HasAura(BERSERK)
                                        && SpellManager.CanCast(INCARNATION))
                                    {
                                        SpellManager.Cast(INCARNATION);
                                        Logging.Write(INCARNATION);
                                        lastSpell = INCARNATION;
                                    }
                                    else if (SpellManager.HasSpell(FEROCIOUS_BITE)
                                            && U.debuffExists(RIP, Me.CurrentTarget)
                                            && (Me.HasAura(SAVAGE_ROAR) && U.buffTimeLeft(SAVAGE_ROAR, Me) > 6000)
                                            && (Me.EnergyPercent >= 50 || (Me.EnergyPercent >= 25 && U.debuffTimeLeft(RIP, Me.CurrentTarget) <= 5000))
                                            && (Me.ComboPoints >= 5 || (Me.ComboPoints >= 1 && U.debuffTimeLeft(RIP, Me.CurrentTarget) <= 5000))
                                            && lastSpell != FEROCIOUS_BITE
                                            && SpellManager.CanCast(FEROCIOUS_BITE))
                                    {
                                        SpellManager.Cast(FEROCIOUS_BITE);
                                        Logging.Write(FEROCIOUS_BITE);
                                        lastSpell = FEROCIOUS_BITE;
                                    }
                                    else if (SpellManager.HasSpell(FEROCIOUS_BITE)
                                        && !needRip
                                        && (Me.HasAura(SAVAGE_ROAR) && U.buffTimeLeft(SAVAGE_ROAR, Me) > 4000)
                                        && Me.EnergyPercent >= 25
                                        && Me.ComboPoints >= 5
                                        && lastSpell != FEROCIOUS_BITE
                                        && SpellManager.CanCast(FEROCIOUS_BITE))
                                    {
                                        SpellManager.Cast(FEROCIOUS_BITE);
                                        Logging.Write(FEROCIOUS_BITE);
                                        lastSpell = FEROCIOUS_BITE;
                                    }
                                    else if (SpellManager.HasSpell(RIP)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && needRip
                                        && (!U.debuffExists(RIP, Me.CurrentTarget)
                                        || (U.debuffExists(RIP, Me.CurrentTarget) && U.debuffTimeLeft(RIP, Me.CurrentTarget) <= 5000))
                                        && Me.EnergyPercent >= 30
                                        && Me.ComboPoints >= 5
                                        && lastSpell != RIP
                                        && SpellManager.CanCast(RIP))
                                    {
                                        SpellManager.Cast(RIP);
                                        Logging.Write(RIP);
                                        lastSpell = RIP;
                                    }
                                    else if (SpellManager.HasSpell(THRASH)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && !U.debuffExists(THRASH, Me.CurrentTarget)
                                        && (Me.EnergyPercent >= 50 || U.buffExists("Clearcasting", Me))
                                        && lastSpell != THRASH
                                        && SpellManager.CanCast(THRASH))
                                    {
                                        SpellManager.Cast(THRASH);
                                        Logging.Write(THRASH);
                                        lastSpell = THRASH;
                                    }
                                    else if (SpellManager.HasSpell(SWIPE)
                                        && Me.EnergyPercent >= 40
                                        && SpellManager.CanCast(SWIPE))
                                    {
                                        SpellManager.Cast(SWIPE);
                                        Logging.Write(SWIPE);
                                        lastSpell = SWIPE;
                                    }
                                }
                                #endregion

                                #region noAoe
                                else if (!S.needAoe)
                                {
                                    if (SpellManager.HasSpell(CAT_FORM)
                                        && Me.Shapeshift != ShapeshiftForm.Cat)
                                    {
                                        SpellManager.Cast(CAT_FORM);
                                        Logging.Write(CAT_FORM);
                                        lastSpell = CAT_FORM;
                                    }
                                    else if (SpellManager.HasSpell(SAVAGE_ROAR)
                                        && !Me.HasAura(SAVAGE_ROAR)
                                        && Me.ComboPoints >= 1
                                        && Me.EnergyPercent >= 25
                                        && lastSpell != SAVAGE_ROAR
                                        && SpellManager.CanCast(SAVAGE_ROAR))
                                    {
                                        SpellManager.Cast(SAVAGE_ROAR);
                                        Logging.Write(SAVAGE_ROAR);
                                        lastSpell = SAVAGE_ROAR;
                                    }
                                    else if (SpellManager.HasSpell(SUVIVAL_INSTINCTS)
                                        && !U.spellOnCooldown(SUVIVAL_INSTINCTS)
                                        && Me.HealthPercent <= P.myPrefs.PercentSurvivalInstincts
                                        && lastSpell != SUVIVAL_INSTINCTS
                                        && SpellManager.CanCast(SUVIVAL_INSTINCTS))
                                    {
                                        SpellManager.Cast(SUVIVAL_INSTINCTS);
                                        Logging.Write(SUVIVAL_INSTINCTS);
                                        lastSpell = SUVIVAL_INSTINCTS;
                                    }
                                    else if (SpellManager.HasSpell(REJUVENATION)
                                        && Me.HealthPercent <= P.myPrefs.PercentRejuCombat
                                        && !Me.HasAura(REJUVENATION)
                                        && lastSpell != REJUVENATION
                                        && SpellManager.CanCast(REJUVENATION))
                                    {
                                        SpellManager.Cast(REJUVENATION, Me);
                                        Logging.Write(REJUVENATION);
                                        lastSpell = REJUVENATION;
                                    }
                                    else if (SpellManager.HasSpell(HEALING_TOUCH)
                                        && S.IsOverlayed(HEALING_TOUCHI)
                                        && lastSpell != HEALING_TOUCH
                                        && SpellManager.CanCast(HEALING_TOUCH))
                                    {
                                        SpellManager.Cast(HEALING_TOUCH);
                                        Logging.Write(HEALING_TOUCH);
                                        lastSpell = HEALING_TOUCH;
                                    }
                                    else if (SpellManager.HasSpell(SKULL_BASH)
                                        && !U.spellOnCooldown(SKULL_BASH)
                                        && (Me.CurrentTarget.IsCasting
                                        && CanInterrupt)
                                        && lastSpell != SKULL_BASH
                                        && interruptTime >= DateTime.Now
                                        && SpellManager.CanCast(SKULL_BASH))
                                    {
                                        SpellManager.Cast(SKULL_BASH);
                                        Logging.Write(SKULL_BASH);
                                        lastSpell = SKULL_BASH;
                                        interruptTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(INCAPACITATING_ROAR)
                                        && !U.spellOnCooldown(INCAPACITATING_ROAR)
                                        && (Me.CurrentTarget.IsCasting
                                        && CanInterrupt)
                                        && lastSpell != INCAPACITATING_ROAR
                                        && interruptTime >= DateTime.Now
                                        && SpellManager.CanCast(INCAPACITATING_ROAR))
                                    {
                                        SpellManager.Cast(INCAPACITATING_ROAR);
                                        Logging.Write(INCAPACITATING_ROAR);
                                        lastSpell = INCAPACITATING_ROAR;
                                        interruptTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(TYPHOON)
                                        && !U.spellOnCooldown(TYPHOON)
                                        && (Me.CurrentTarget.IsCasting
                                        && CanInterrupt)
                                        && lastSpell != TYPHOON
                                        && interruptTime >= DateTime.Now
                                        && SpellManager.CanCast(TYPHOON))
                                    {
                                        SpellManager.Cast(TYPHOON);
                                        Logging.Write(TYPHOON);
                                        lastSpell = TYPHOON;
                                        interruptTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(MIGHTY_BASH)
                                        && !U.spellOnCooldown(MIGHTY_BASH)
                                        && (Me.CurrentTarget.IsCasting
                                        && !CanInterrupt)
                                        && lastSpell != MIGHTY_BASH
                                        && stunTime >= DateTime.Now
                                        && SpellManager.CanCast(MIGHTY_BASH))
                                    {
                                        SpellManager.Cast(MIGHTY_BASH);
                                        Logging.Write(MIGHTY_BASH);
                                        lastSpell = MIGHTY_BASH;
                                        stunTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(WAR_STOMP)
                                        && !U.spellOnCooldown(WAR_STOMP)
                                        && (Me.CurrentTarget.IsCasting
                                        && !CanInterrupt)
                                        && lastSpell != WAR_STOMP
                                        && stunTime >= DateTime.Now
                                        && SpellManager.CanCast(WAR_STOMP))
                                    {
                                        SpellManager.Cast(WAR_STOMP);
                                        Logging.Write(WAR_STOMP);
                                        lastSpell = WAR_STOMP;
                                        stunTime.AddSeconds(5);
                                    }
                                    else if (SpellManager.HasSpell(TIGERS_FURY)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && !U.spellOnCooldown(TIGERS_FURY)
                                        && Me.EnergyPercent < 35
                                        && lastSpell != TIGERS_FURY
                                        && SpellManager.CanCast(TIGERS_FURY))
                                    {
                                        SpellManager.Cast(TIGERS_FURY);
                                        Logging.Write(TIGERS_FURY);
                                        lastSpell = TIGERS_FURY;
                                    }
                                    else if (SpellManager.HasSpell(BERSERK)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && !U.spellOnCooldown(BERSERK)
                                        && lastSpell != BERSERK
                                        && T.IsWoWBoss(Me.CurrentTarget)
                                        && SpellManager.CanCast(BERSERK))
                                    {
                                        SpellManager.Cast(BERSERK);
                                        Logging.Write(BERSERK);
                                        lastSpell = BERSERK;
                                    }
                                    else if (SpellManager.HasSpell(INCARNATION)
                                        && !U.spellOnCooldown(INCARNATION)
                                        && Me.HasAura(BERSERK)
                                        && SpellManager.CanCast(INCARNATION))
                                    {
                                        SpellManager.Cast(INCARNATION);
                                        Logging.Write(INCARNATION);
                                        lastSpell = INCARNATION;
                                    }
                                    else if (SpellManager.HasSpell(FORCE_OF_NATURE)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && !U.spellOnCooldown(FORCE_OF_NATURE)
                                        && lastSpell != FORCE_OF_NATURE
                                        && T.IsWoWBoss(Me.CurrentTarget)
                                        && SpellManager.CanCast(FORCE_OF_NATURE))
                                    {
                                        SpellManager.Cast(FORCE_OF_NATURE);
                                        Logging.Write(FORCE_OF_NATURE);
                                        lastSpell = FORCE_OF_NATURE;
                                    }
                                    else if (SpellManager.HasSpell(FORCE_OF_NATURE)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && !U.spellOnCooldown(FORCE_OF_NATURE)
                                        && lastSpell != FORCE_OF_NATURE
                                        && !T.IsWoWBoss(Me.CurrentTarget)
                                        && fonTime >= DateTime.Now
                                        && SpellManager.CanCast(FORCE_OF_NATURE))
                                    {
                                        SpellManager.Cast(FORCE_OF_NATURE);
                                        Logging.Write(FORCE_OF_NATURE);
                                        lastSpell = FORCE_OF_NATURE;
                                        fonTime.AddSeconds(15);
                                    }
                                    else if (SpellManager.HasSpell(FEROCIOUS_BITE)
                                        && U.debuffExists(RIP, Me.CurrentTarget)
                                        && (Me.HasAura(SAVAGE_ROAR) && U.buffTimeLeft(SAVAGE_ROAR, Me) > 6000)
                                        && (Me.EnergyPercent >= 50 || (Me.EnergyPercent >= 25 && U.debuffTimeLeft(RIP, Me.CurrentTarget) <= 5000))
                                        && (Me.ComboPoints >= 5 || (Me.ComboPoints >= 1 && U.debuffTimeLeft(RIP, Me.CurrentTarget) <= 5000))
                                        && lastSpell != FEROCIOUS_BITE
                                        && SpellManager.CanCast(FEROCIOUS_BITE))
                                    {
                                        SpellManager.Cast(FEROCIOUS_BITE);
                                        Logging.Write(FEROCIOUS_BITE);
                                        lastSpell = FEROCIOUS_BITE;
                                    }
                                    else if (SpellManager.HasSpell(FEROCIOUS_BITE)
                                        && !needRip
                                        && (Me.HasAura(SAVAGE_ROAR) && U.buffTimeLeft(SAVAGE_ROAR, Me) > 4000)
                                        && Me.EnergyPercent >= 25
                                        && Me.ComboPoints >= 5
                                        && lastSpell != FEROCIOUS_BITE
                                        && SpellManager.CanCast(FEROCIOUS_BITE))
                                    {
                                        SpellManager.Cast(FEROCIOUS_BITE);
                                        Logging.Write(FEROCIOUS_BITE);
                                        lastSpell = FEROCIOUS_BITE;
                                    }
                                    else if (SpellManager.HasSpell(RIP)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && needRip
                                        && (!U.debuffExists(RIP, Me.CurrentTarget)
                                        || (U.debuffExists(RIP, Me.CurrentTarget) && U.debuffTimeLeft(RIP, Me.CurrentTarget) <= 5000))
                                        && Me.EnergyPercent >= 30
                                        && Me.ComboPoints >= 5
                                        && lastSpell != RIP
                                        && SpellManager.CanCast(RIP))
                                    {
                                        SpellManager.Cast(RIP);
                                        Logging.Write(RIP);
                                        lastSpell = RIP;
                                    }
                                    else if (SpellManager.HasSpell(RAKE)
                                        && Me.HasAura(SAVAGE_ROAR)
                                        && (!U.debuffExists(RAKE, Me.CurrentTarget)
                                        || (U.debuffExists(RAKE, Me.CurrentTarget) && U.debuffTimeLeft(RAKE, Me.CurrentTarget) <= 5000))
                                        && Me.EnergyPercent >= 35
                                        && lastSpell != RAKE
                                        && SpellManager.CanCast(RAKE))
                                    {
                                        SpellManager.Cast(RAKE);
                                        Logging.Write(RAKE);
                                        lastSpell = RAKE;
                                    }
                                    else if (SpellManager.HasSpell(FEROCIOUS_BITE)
                                        && (Me.HasAura(SAVAGE_ROAR) && U.buffTimeLeft(SAVAGE_ROAR, Me) > 6000)
                                        && Me.EnergyPercent >= 50
                                        && Me.ComboPoints >= 5
                                        && (U.debuffExists(RIP, Me.CurrentTarget) && U.debuffTimeLeft(RIP, Me.CurrentTarget) > 6000)
                                        && lastSpell != FEROCIOUS_BITE
                                        && SpellManager.CanCast(FEROCIOUS_BITE))
                                    {
                                        SpellManager.Cast(FEROCIOUS_BITE);
                                        Logging.Write(FEROCIOUS_BITE);
                                        lastSpell = FEROCIOUS_BITE;
                                    }
                                    else if (SpellManager.HasSpell(SHRED)
                                        && Me.EnergyPercent >= 50
                                        && SpellManager.CanCast(SHRED))
                                    {
                                        SpellManager.Cast(SHRED);
                                        Logging.Write(SHRED);
                                        lastSpell = SHRED;
                                    }
                                    Thread.Sleep(10);
                                }
                                #endregion

                            }
                            catch (Exception e)
                            {
                                Logging.Write("Feral Routine > : " + e);
                            }
                            _loop = false;
                        }
                    }
                    )));
        }
        public static int FAERIE_SWARM = 106707;
        public static int FAERIE_FIRE = 770;
        public static Composite feralPull()
        {
            return new Action(ret =>
            {
                _pulling = true;
                while (_pulling)
                {
                    try
                    {
                        if (SpellManager.HasSpell(RAKE)
                            && Me.HasAura(PROWL)
                            && P.myPrefs.PullPref
                            && Me.CurrentTarget.IsWithinMeleeRange
                            && SpellManager.CanCast(RAKE))
                        {
                            SpellManager.Cast(RAKE);
                            Logging.Write(RAKE);
                            lastSpell = RAKE;
                        }
                        else if (SpellManager.HasSpell(FAERIE_SWARM)
                            && !U.spellOnCooldown(FAERIE_SWARM)
                            && !P.myPrefs.PullPref
                            && Me.CurrentTarget.Distance <= 30
                            && SpellManager.CanCast(FAERIE_SWARM))
                        {
                            SpellManager.Cast(FAERIE_SWARM);
                            Logging.Write(WoWSpell.FromId(FAERIE_SWARM).Name);
                        }
                        else if (SpellManager.HasSpell(FAERIE_FIRE)
                            && !U.spellOnCooldown(FAERIE_FIRE)
                            && !P.myPrefs.PullPref
                            && Me.CurrentTarget.Distance <= 30
                            && SpellManager.CanCast(FAERIE_FIRE))
                        {
                            SpellManager.Cast(FAERIE_FIRE);
                            Logging.Write(WoWSpell.FromId(FAERIE_FIRE).Name);
                        }
                        Thread.Sleep(10);
                    }
                    catch (Exception e)
                    {
                        Logging.Write(Colors.Red, "Feral Pull -> " + e);
                        throw;
                    }
                    _pulling = false;
                }
            }
            );
        }

        public static DateTime fonTime;
        public static DateTime interruptTime;
        public static DateTime stunTime;
        public static bool needRip
        {
            get { return Me.CurrentTarget.MaxHealth >= Me.MaxHealth * 1.5; }
        }
        public static bool CanInterrupt
        {
            get
            {
                var s = Lua.GetReturnVal<string>(
                    "ret = \"0\";" +
                    "local spell, rank, displayName, icon, startTime, endTime, isTradeSkill, castID, interrupt = UnitCastingInfo(\"target\");" +
                    "if interrupt == \"1\" then ret = \"1\" end;" +
                    "return ret", 0);
                Logging.Write(Colors.PowderBlue, "Interrupt: " + s);
                if (s == "1") return true;
                return false;
            }
        }
    }
}
