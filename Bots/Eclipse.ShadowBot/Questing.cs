using Styx;
using Styx.CommonBot.Frames;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.ShadowBot
{

    public static class Questing
    {
        public static uint QuestContextId = 0;
        #region Attach and Detach LUA Events
        // PARts of This questing class were jacked from FPSWare's RAF2 Botbase. 
        // I could use it as a reference and create my own stuff, but decided to just give him credit instead. Yay FPSWare!
        public static void AttachQuestingEvents()
        {
            EclipseShadowBot.log("Attaching q events.");
            Lua.Events.AttachEvent("QUEST_GREETING", OnQuestGreeting);
            Lua.Events.AttachEvent("QUEST_DETAIL", OnQuestDetail);
            Lua.Events.AttachEvent("GOSSIP_SHOW", OnGossipShow);
            Lua.Events.AttachEvent("QUEST_COMPLETE", OnQuestComplete);
            Lua.Events.AttachEvent("QUEST_PROGRESS", OnQuestProgress);
        }
        public static void DetatchQuestingEvents()
        {
            EclipseShadowBot.log("Deattaching q events.");
            Lua.Events.DetachEvent("QUEST_GREETING", OnQuestGreeting);
            Lua.Events.DetachEvent("QUEST_DETAIL", OnQuestDetail);
            Lua.Events.DetachEvent("GOSSIP_SHOW", OnGossipShow);
            Lua.Events.DetachEvent("QUEST_COMPLETE", OnQuestComplete);
            Lua.Events.DetachEvent("QUEST_PROGRESS", OnQuestProgress);

        }
        #endregion
        
        #region Lua Event Handlers
        public static void OnQuestProgress(object obj, LuaEventArgs args)
        {

        }
        public static void OnQuestDetail(object obj, LuaEventArgs args)
        {
            var i = QuestFrame.Instance.CurrentShownQuest;
            QuestContextId = StyxWoW.Me.CurrentTarget.Entry;
            QuestFrame.Instance.AcceptQuest();
            EclipseShadowBot.log("Accepted a Quest..." + i.Name);
        }
        public static void OnQuestGreeting(object obj, LuaEventArgs args)
        {
            int activeQuests = QuestFrame.Instance.AvailableQuests.Count;
            EclipseShadowBot.log("Accepting a q ...");
            QuestFrame.Instance.SelectAvailableQuest(0);


            // Get a list of active quests from the currently open window
            List<GossipQuestEntry> gossipQuests = QuestFrame.Instance.IsVisible ? QuestFrame.Instance.ActiveQuests : GossipFrame.Instance.ActiveQuests;

            // Check if the q is complete against our q logs
            GossipQuestEntry quest = (from entry in gossipQuests
                                      let pQuest = StyxWoW.Me.QuestLog.GetQuestById((uint)entry.Id)
                                      where pQuest != null && pQuest.IsCompleted
                                      select entry).FirstOrDefault();

            // We have a valid q we can turn in to this q giver!
            if (quest == null) return;

            // DO IT BITCH!
            QuestFrame.Instance.SelectActiveGossipQuest(quest.Index);
            EclipseShadowBot.log("====== it appeared to work, right?");
        }
        public static void OnGossipShow(object obj, LuaEventArgs args)
        {
            EclipseShadowBot.log("===== OnGossipShow");


            // Get a list of active quests from the currently open window
            List<GossipQuestEntry> gossipQuests = QuestFrame.Instance.IsVisible ? QuestFrame.Instance.ActiveQuests : GossipFrame.Instance.ActiveQuests;

            // Check if the q is complete against our q logs
            GossipQuestEntry quest = (from entry in gossipQuests
                                      let pQuest = StyxWoW.Me.QuestLog.GetQuestById((uint)entry.Id)
                                      where pQuest != null && pQuest.IsCompleted
                                      select entry).FirstOrDefault();

            // We have a valid q we can turn in to this q giver!
            if (quest != null)
            {
                // DO IT BITCH!
                GossipFrame.Instance.SelectActiveGossipQuest(quest.Index);
            }

            EclipseShadowBot.log("Picking up a q");

            if (QuestFrame.Instance.IsVisible)
            {
                if (QuestFrame.Instance.AvailableQuests.Count > 0) QuestFrame.Instance.SelectAvailableQuest(0);
            }
            else if (GossipFrame.Instance.IsVisible)
            {
                if (GossipFrame.Instance.AvailableQuests.Count > 0) GossipFrame.Instance.SelectAvailableQuest(0);
            }

        }
        public static void OnQuestComplete(object obj, LuaEventArgs args)
        {
            EclipseShadowBot.log("===== OnQuestComplete");

            int countOfQuestRewards = Lua.GetReturnVal<int>("return GetNumQuestChoices()", 0);
            EclipseShadowBot.log("countOfQuestRewards: " + countOfQuestRewards);

            // More than 1 reward to choose from
            if (countOfQuestRewards > 1)
            {
                float bestOverallItemScore = Single.MinValue;
                int bestOverallItemChoice = -1;

                /*
                // Enumerate q rewards to find the best one
                for (int i = 1; i <= countOfQuestRewards; i++)
                {
                    string itemLink = Lua.GetReturnVal<string>("return GetQuestItemLink(\"choice\", " + i.ToString(CultureInfo.InvariantCulture) + ")", 0);
                    string[] splitted = itemLink.Split(':');

                    uint itemId;
                    if (String.IsNullOrEmpty(itemLink) || (splitted.Length == 0 || splitted.Length < 2) || (!UInt32.TryParse(splitted[1], out itemId) || itemId == 0))
                    {
                        EclipseShadowBot.log("Parsing ItemLink for q item failed!");
                        EclipseShadowBot.log("ItemLink: {0}", itemLink);
                        continue;
                    }
                    else
                    {
                        EclipseShadowBot.log("itemId is good:" + itemId);
                    }

                    ItemInfo choiceItemInfo = ItemInfo.FromId(itemId);
                    if (choiceItemInfo == null)
                    {
                        EclipseShadowBot.log("Retrieving item info for reward item failed");
                        EclipseShadowBot.log("Item Id:{0} ItemLink:{1}", itemId, itemLink);
                        continue;
                    }
                    else
                    {
                        EclipseShadowBot.log("choiceItemInfo is good:" + choiceItemInfo.Name);
                    }

                    // Score of the item.
                    float choiceItemScore = WeightSet.EvaluateItem(choiceItemInfo, new ItemStats(itemLink));

                    // Score the equipped item if any. otherwise 0
                    float bestEquipItemScore = Single.MinValue;

                    // The best slot
                    InventorySlot bestSlot = InventorySlot.None;
                    List<InventorySlot> inventorySlots = InventoryManager.GetInventorySlotsByEquipSlot(choiceItemInfo.EquipSlot);

                    foreach (InventorySlot slot in inventorySlots)
                    {
                        EclipseShadowBot.log("foreach inventoryslots");
                        WoWItem equipped = EquippedItems[slot];
                        if (equipped == null) continue;

                        bestEquipItemScore = WeightSet.EvaluateItem(equipped, true);
                        bestSlot = slot;
                    }

                    if (bestEquipItemScore > Single.MinValue)
                    {
                        EclipseShadowBot.log("Equipped Item {0} scored {1}", EquippedItems[bestSlot].Name, bestEquipItemScore.ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        EclipseShadowBot.log("bestEquipItemScore < Single.MinValue");
                    }


                    // If this our armor type?
                    bool goodArmor = choiceItemInfo.ItemClass == WoWItemClass.Armor && (choiceItemInfo.ArmorClass == WeightSet.GetWantedArmorClass());// || miscArmorType.Contains(choiceItemInfo.InventoryType));
                    EclipseShadowBot.log("goodArmor: " + goodArmor);

                    // Now we assign the overall score of this item
                    // 1. Is it a higher score than our equipped item?
                    // 2. Is this armor type good for us?
                    if (choiceItemScore > bestEquipItemScore && bestSlot != InventorySlot.None && (goodArmor && (choiceItemScore - bestEquipItemScore) > bestOverallItemScore))
                    {
                        bestOverallItemScore = (choiceItemScore - bestEquipItemScore);
                        bestOverallItemChoice = i;
                        EclipseShadowBot.log("choiceItemScore and other stuff SUCCEEDED");
                        bestQuestReward = choiceItemInfo.Name;

                    }
                    else
                    {
                        EclipseShadowBot.log("choiceItemScore and other stuff failed");
                    }
                    
                }
                */
                EclipseShadowBot.log("finished enumerating q rewards...");

                // We have not found a replacement item, so choose the most value item for vendoring
                if (bestOverallItemScore == Single.MinValue)
                {
                    EclipseShadowBot.log("No upgrades available, choosing most valuable item for vendoring");
                    ItemInfo highestItemInfo = null;

                    for (int i = 1; i <= countOfQuestRewards; i++)
                    {
                        string itemLink = Lua.GetReturnVal<string>("return GetQuestItemLink(\"choice\", " + i.ToString(CultureInfo.InvariantCulture) + ")", 0);
                        string[] splitted = itemLink.Split(':');

                        uint itemId;
                        if (String.IsNullOrEmpty(itemLink) || (splitted.Length == 0 || splitted.Length < 2) || (!UInt32.TryParse(splitted[1], out itemId) || itemId == 0))
                        {
                            EclipseShadowBot.log("Parsing ItemLink failed!");
                            EclipseShadowBot.log(string.Format("ItemLink: {0}", itemLink));
                            continue;
                        }

                        ItemInfo choiceItemInfo = ItemInfo.FromId(itemId);
                        if (choiceItemInfo == null)
                        {
                            EclipseShadowBot.log("Retrieving item info failed!");
                            EclipseShadowBot.log(string.Format("Item Id:{0} ItemLink:{1}", itemId, itemLink));
                            continue;
                        }

                        if (highestItemInfo == null || highestItemInfo.SellPrice > choiceItemInfo.SellPrice)
                        {
                            highestItemInfo = choiceItemInfo;
                            bestOverallItemChoice = i;
                        }
                    }
                }

                // Select the best q reward
                //if (bestOverallItemChoice > 0)
                if (bestOverallItemChoice > 0)
                {
                    EclipseShadowBot.log("bestOverallItemChoice > 0 and it is: " + bestOverallItemChoice);
                    //EclipseShadowBot.log("QuestFrame.Instance.IsVisible: " + QuestFrame.Instance.IsVisible);
                    QuestFrame.Instance.SelectQuestReward(bestOverallItemChoice - 1);
                }
                else
                {
                    EclipseShadowBot.log("AHHHHH SHIIIIIIT!");
                    EclipseShadowBot.log("If we're here something has gone wrong and this is a fall through");
                    EclipseShadowBot.log("Inside OnQuestComplete. We failed to choose a q reward, either for upgrade or vendoring.");
                    QuestFrame.Instance.SelectQuestReward(0);
                }

            }
            QuestFrame.Instance.SelectQuestReward(0);
            QuestFrame.Instance.CompleteQuest();

        }
        #endregion

    }
}
