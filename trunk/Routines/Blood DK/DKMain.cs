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
using Form1 = DeathKnight.GUI.Form1;
using HKM = DeathKnight.Helpers.HotkeyManager;
using S = DeathKnight.DKSpells.DKSpells;
using CL = DeathKnight.Handlers.CombatLogEventArgs;
using EH = DeathKnight.Handlers.EventHandlers;
using L = DeathKnight.Helpers.Logs;
using T = DeathKnight.Helpers.targets;
using U = DeathKnight.Helpers.Unit;
using UI = DeathKnight.Helpers.UseItems;
using P = DeathKnight.DKSettings.DKPrefs;
using M = DeathKnight.Helpers.Movement;
using I = DeathKnight.Helpers.Interrupts;
using R = DeathKnight.DKRotations.BloodRot;
#endregion




namespace DeathKnight
{
    public class Main : CombatRoutine
    {
        public override string Name { get { return "DeathKnight by Pasterke"; } }
        public override WoWClass Class { get { return Me.Specialization == WoWSpec.DeathKnightBlood ? WoWClass.DeathKnight : WoWClass.None; } }

        public LocalPlayer Me { get { return StyxWoW.Me; } }

        public override Composite CombatBehavior { get { return Combat(); } }
        public override Composite PreCombatBuffBehavior { get { return PreCombatBuffs(); } }
        public override Composite CombatBuffBehavior { get { return CombatBuffs(); } }
        public override Composite PullBehavior { get { return Pull(); } }
        public override Composite PullBuffBehavior { get { return PullBuffs(); } }

        public string usedBot { get { return BotManager.Current.Name.ToUpper(); } }

        private static bool IsInGame
        {
            get { return StyxWoW.IsInGame; }
        }
        public Stopwatch mountTimer = new Stopwatch();
        public override bool WantButton { get { return true; } }
        public override void OnButtonPress()
        {
            Form1 ConfigForm = new Form1();
            ConfigForm.ShowDialog();
        }
        public override void Initialize()
        {
            Logging.Write("\r\n");
            Logging.Write("-------------------------------------");
            Logging.Write("-- Hello {0}                       --", Me.Name);
            Logging.Write("--        Thanks for using         --");
            Logging.Write("-- The DeathKnight Combat Routine  --");
            Logging.Write("--          by Pasterke            --");
            Logging.Write("-------------------------------------" + "\r\n");
            HKM.registerHotKeys();

            Lua.Events.AttachEvent("UI_ERROR_MESSAGE", CL.CombatLogErrorHandler);
            EH.AttachCombatLogEvent();

            if (Me.Specialization != WoWSpec.DeathKnightBlood)
            {
                MessageBox.Show("Sorry, Only Blood DeathKnight Supported" 
                    + "\r\n", "Wrong Specialization");
            }
            
        }
        public override void ShutDown()
        {
            HKM.removeHotkeys();
            EH.DetachCombatLogEvent();
            Lua.Events.DetachEvent("UI_ERROR_MESSAGE", CL.CombatLogErrorHandler);
        }

        #region Pulse
        public override void Pulse()
        {
            if (!StyxWoW.IsInWorld || Me == null || !Me.IsValid || Me.IsGhost)
            {
                return;
            }
            if (Me.IsDead
                && AutoBot)
            {
                Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/script RepopMe()"));
            }
        }
        #endregion
        public override bool NeedRest
        {
            get
            {
                return CanBuff
                    && !HKM.pauseRoutineOn
                    && !HKM.manualOn
                    && !Me.GroupInfo.IsInParty
                    && Me.HealthPercent <= P.myPrefs.FoodHPOoC;
            }
        }

        public override void Rest()
        {
            if (CanBuff
                && !Me.IsSwimming
                && Me.HealthPercent <= P.myPrefs.FoodHPOoC)
            {
                Styx.CommonBot.Rest.Feed();
            }

        }

        Composite PreCombatBuffs()
        {
            return new PrioritySelector(
                new Decorator(ret => CanBuff 
                    && !Me.HasAura(S.CRYSTAL_OF_INSANITY_BUFF)
                    && S.NotHaveFlaskBuff
                    && P.myPrefs.FlaskCrystal
                    && UI.HasItem(crystal), UI.UseItem(crystal)),
                new Decorator(ret => CanBuff
                    && !Me.HasAura(S.CRYSTAL_OF_INSANITY_BUFF)
                    && S.NotHaveFlaskBuff
                    && S.NotHaveAlchemyBuff
                    && P.myPrefs.FlaskAlchemy
                    && UI.HasItem(AlchemyFlask), UI.UseItem(AlchemyFlask)),
                S.castHornOfWinter(),
                S.castPresence()
                );
        }

        Composite PullBuffs()
        {
            return new PrioritySelector(

                );
        }

        Composite Pull()
        {
            return new PrioritySelector(
                new Decorator(ret => ((Me.Mounted && !AutoBot) || HKM.pauseRoutineOn || HKM.manualOn), Hold()),
                new Decorator(ret => (Me.CurrentTarget == null || (Me.GotTarget && Me.CurrentTarget.IsDead)) && M.AllowTargeting, T.FindTarget()),
                new Decorator(ret => Me.GotTarget
                    && M.AllowTargeting
                    && !Me.CurrentTarget.IsWithinMeleeRange
                    && T.MeleeAddCount >= 1, T.FindMeleeAttackers()),
                new Decorator(ret => Me.CurrentTarget != null
                    && AutoBot
                    && Me.CurrentTarget.IsFriendly, new Action(ret => { Me.ClearTarget(); return RunStatus.Failure; })),
                new Decorator(ret => Me.GotTarget && M.AllowMovement && !Me.CurrentTarget.IsWithinMeleeRange, M.CreateMovement()),
                new Decorator(ret => Me.GotTarget && M.AllowMovement && Me.CurrentTarget.IsWithinMeleeRange, M.CreateStopMovement()),
                new Decorator(ret => Me.GotTarget && M.AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget), M.FacingTarget()),
                S.castDeathGrip(),
                S.castDarkCommand(),
                S.castDeathCoilPull(),
                S.castDeathIcyTouchPull()
                );
        }
        Composite CombatBuffs()
        {
            return new PrioritySelector(
                new Decorator(ret => CanBuff
                    && S.NotHaveFlaskBuff
                    && canUseRaidFlask
                    && UI.HasItem(myRaidFlask), UI.UseItem(myRaidFlask)),
                new Decorator(ret => CanBuff
                    && !Me.HasAura(S.CRYSTAL_OF_INSANITY_BUFF)
                    && S.NotHaveFlaskBuff
                    && P.myPrefs.FlaskCrystal
                    && UI.HasItem(crystal), UI.UseItem(crystal)),
                new Decorator(ret => CanBuff
                    && !Me.HasAura(S.CRYSTAL_OF_INSANITY_BUFF)
                    && S.NotHaveFlaskBuff
                    && S.NotHaveAlchemyBuff
                    && P.myPrefs.FlaskAlchemy
                    && UI.HasItem(AlchemyFlask), UI.UseItem(AlchemyFlask)),
                new Decorator(ret => CanBuff && Me.HealthPercent <= P.myPrefs.PercentHealthstone && UI.HasItem(healthstone), UI.UseItem(healthstone)),
                new Decorator(ret => CanBuff && Me.HealthPercent <= P.myPrefs.PercentTrinket1HP, UI.useTrinket1()),
                new Decorator(ret => CanBuff && Me.HealthPercent <= P.myPrefs.PercentTrinket2HP, UI.useTrinket2()),
                S.castIceboundFortitude(),
                S.castRuneTap(),
                S.castVampiricBlood(),
                S.castGiftOfTheNaaru(),
                S.castHornOfWinter(),
                S.castPresence(),
                S.castBoneShield(),
                S.castConversion(),
                S.cancelConversion()
                );
        }
        Composite Combat()
        {
            return new PrioritySelector(
                new Decorator(ret => ((Me.Mounted && !AutoBot) || HKM.pauseRoutineOn || HKM.manualOn), Hold()),
                new Decorator(ret => (Me.CurrentTarget == null || (Me.GotTarget && Me.CurrentTarget.IsDead)) && M.AllowTargeting, T.FindTarget()),
                new Decorator(ret => Me.GotTarget
                    && M.AllowTargeting
                    && !Me.CurrentTarget.IsWithinMeleeRange
                    && T.MeleeAddCount >= 1, T.FindMeleeAttackers()),
                new Decorator(ret => Me.CurrentTarget != null
                    && AutoBot
                    && Me.CurrentTarget.IsFriendly, new Action(ret => { Me.ClearTarget(); return RunStatus.Failure; })),
                new Decorator(ret => Me.GotTarget && M.AllowMovement && !Me.CurrentTarget.IsWithinMeleeRange, M.CreateMovement()),
                new Decorator(ret => Me.GotTarget && M.AllowMovement && Me.CurrentTarget.IsWithinMeleeRange, M.CreateStopMovement()),
                new Decorator(ret => Me.GotTarget && M.AllowFacing && !Me.IsSafelyFacing(Me.CurrentTarget), M.FacingTarget()),
                new Decorator(ret => S.gotTarget && Me.CurrentTarget.IsWithinMeleeRange && !Me.IsAutoAttacking, AutoAttack()),
                new Decorator(ret => S.gotTarget && P.myPrefs.Trinket1 == 3 && HKM.cooldownsOn && Me.CurrentTarget.IsWithinMeleeRange, UI.useTrinket1()),
                new Decorator(ret => S.gotTarget && P.myPrefs.Trinket2 == 3 && HKM.cooldownsOn && Me.CurrentTarget.IsWithinMeleeRange, UI.useTrinket2()),
                new Decorator(ret => S.gotTarget && P.myPrefs.Gloves == 2 && HKM.cooldownsOn && Me.CurrentTarget.IsWithinMeleeRange, UI.useGloves()),
                S.castEmpowerRuneWeapon(),
                S.castDancingRuneWeapon(),
                S.castRacial(),
                S.castWarStomp(),
                S.castMindFreeze(),
                S.castAxphyxiate(),
                S.castStrangulate(),
                S.castBloodTap(),
                S.castOutbreak(),
                S.castIcyTouch(),
                S.castPlagueStrike(),
                S.castDeathAndDecay(),
                S.castRemorselesWinter(),
                S.castBloodBoil(),
                S.castSoulReaper(),
                S.castDeathStrike(),
                S.castDeathCoil()
                );
        }







        #region autoattack
        Composite AutoAttack()
        {
            return new Action(ret =>
            {
                Lua.DoString("StartAttack()");
                return RunStatus.Failure;
            });
        }
        #endregion

        #region AutoBot
        bool AutoBot
        {
            get
            {
                return usedBot.Contains("QUEST") || usedBot.Contains("GRIND") || usedBot.Contains("GATHER") || usedBot.Contains("ANGLER") || usedBot.Contains("ARCHEO");

            }
        }
        #endregion

        #region can buff
        public bool CanBuff
        {
            get
            {
                return !Me.Mounted && !Me.IsFlying && !Me.OnTaxi && !Me.IsDead && !Me.IsGhost && !Me.IsCasting;
            }
        }
        #endregion

        #region Hold
        Composite Hold()
        {
            return new Action(ret =>
            {
                return;
            });
        }
        #endregion

        #region use flasks, healthstone and others
        public uint myRaidFlask = (uint)P.myPrefs.RaidFlaskKind;
        public uint crystal = 86569;
        public uint RaidFlaskAgility = 76084;
        public uint RaidFlaskStamina = 76084;
        public uint RaidFlaskStrenght = 76084;
        public uint RaidFlaskSpirit = 76084;
        public uint RaidFlaskIntellect = 76084;
        public uint AlchemyFlask = 75525;
        public uint healthstone = 5512;
        public uint CRYSTAL_OF_INSANITY_BUFF = 127230;

        public bool canUseRaidFlask
        {
            get
            {
                if (P.myPrefs.RaidFlask == 1) 
                { 
                    return false; 
                }
                if (P.myPrefs.RaidFlask == 2 && (Me.CurrentMap.IsRaid || Me.GroupInfo.IsInRaid))
                { 
                    return true; 
                }
                if (P.myPrefs.RaidFlask == 3 && (Me.CurrentMap.IsRaid || Me.GroupInfo.IsInRaid) && !Me.GroupInfo.IsInLfgParty) 
                { 
                    return true; 
                }
                if (P.myPrefs.RaidFlask == 4 && (Me.CurrentMap.IsDungeon || Me.CurrentMap.IsInstance))
                { 
                    return true; 
                }
                return false;
            }
        }


        #endregion

    }
}
