using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Styx.Pathing;
using Styx.Common;
using System.Windows.Media;
using Styx.CommonBot.Frames;
using System.Diagnostics;
using Styx.CommonBot.Profiles.Quest.Order;

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
#endregion

namespace DeathKnight.Helpers
{
    class UseItems
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private static DateTime nextTrinketTimeAllowed;
        public static void SetNextNextTrinketTimeAllowed()
        {
            nextTrinketTimeAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 10000);
        }
        
        private static bool CanUseEquippedItem(WoWItem item)
        {
            string itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;
            return item.Usable && item.Cooldown <= 0;
        }

        public static Composite useTrinket1()
        {
            return new Action(ret =>
            {
                var Trinket = StyxWoW.Me.Inventory.Equipped.Trinket1;

                if (Trinket != null
                    && CanUseEquippedItem(Trinket))
                {
                    Trinket.Use();
                    Logging.Write(Colors.OrangeRed, "Using 1st Trinket");
                    SetNextNextTrinketTimeAllowed();
                }
                return RunStatus.Failure;
            });
        }

        public static Composite useTrinket2()
        {
            return new Action(ret =>
            {
                var Trinket = StyxWoW.Me.Inventory.Equipped.Trinket2;

                if (Trinket != null
                    && CanUseEquippedItem(Trinket))
                {
                    Trinket.Use();
                    Logging.Write(Colors.OrangeRed, "Using 1nd Trinket");
                    SetNextNextTrinketTimeAllowed();
                }
                return RunStatus.Failure;
            });
        }

        private static DateTime nextHandsAllowed;
        public static void SetNextHandsAllowed()
        {
            nextHandsAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 15000);
        }
        public static Composite useGloves()
        {
            return new Action(ret =>
            {
                var Hands = StyxWoW.Me.Inventory.Equipped.Hands;

                if (Hands != null
                    && CanUseEquippedItem(Hands))
                {
                    Hands.Use();
                    Logging.Write(Colors.OrangeRed, "Using Engineer Gloves");
                    SetNextHandsAllowed();
                }
                return RunStatus.Failure;
            });
        }

        private static int RaidFlask = P.myPrefs.RaidFlaskKind;

        #region buffwait
        public static DateTime nextBuffAllowed;

        public static void SetNextBuffAllowed()
        {
            nextBuffAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 5000);
        }
        #endregion

        public static bool HasItem(uint itemId)
        {
            return StyxWoW.Me.CarriedItems.Any(i => i.Entry == itemId);
        }
        public static Composite UseItem(uint id)
        {
            return new PrioritySelector(
                ctx => ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(item => item.Entry == id),
                new Decorator(ctx => nextBuffAllowed <= DateTime.Now && ctx != null && CanUseItem((WoWItem)ctx),
                    new Action(ctx => 
                    {
                        if(id == RaidFlask && Me.HasAura("Visions of Insanity"))
                        {
                            Lua.DoString("RunMacroText(\"/cancelaura Visions of Insanity\")");
                        }
                        UseItem((WoWItem)ctx);
                        SetNextBuffAllowed();
                    })));
        }

        public static bool CanUseItem(WoWItem item)
        {
            return item.Usable && item.Cooldown <= 0;
        }

        public static void UseItem(WoWItem item)
        {
            Logging.Write(Colors.Orange, "using " + item.Name);
            item.Use();
        }

        

        
    }
}
