using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace DonateToGranary
{
    public partial class DonateFoodBaseBehavior : CampaignBehaviorBase
    {

        private static GauntletLayer? _gauntletLayer;
        private static GauntletMovie? _gauntletMovie;
        private static DTGPopupVM? _popupVM;

        ItemRoster _donateRoster = new();

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddGranaryMenuItems));
        }
         
        private void AddGranaryMenuItems(CampaignGameStarter campaignGameStarter)
        {

            int playerGrain = 0;
            int freeSpaceInGranary = 0;

            campaignGameStarter.AddGameMenuOption("town_keep", "granary_keep", new TextObject("Visit the granary").ToString(), delegate (MenuCallbackArgs args)
            {
                args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                GameMenu.SwitchToMenu("granary_keep_menu");
            }, false, 1, false);

            campaignGameStarter.AddGameMenu("granary_keep_menu", "{GRANARY_INFO}", delegate (MenuCallbackArgs args)
            {
                playerGrain = this.GetPlayerGrainAmount();
                freeSpaceInGranary = this.GetGranarySpace();
                float currentFoodAmount = Settlement.CurrentSettlement.Town.FoodStocks;

                if (currentFoodAmount < 50) 
                { 
                    args.MenuContext.SetBackgroundMeshName("donatetogranary1"); 
                } else if (freeSpaceInGranary < 50)
                {
                    args.MenuContext.SetBackgroundMeshName("donatetogranary3");
                } else
                {
                    args.MenuContext.SetBackgroundMeshName("donatetogranary2");
                }


                string granaryInfo = "Grain in your inventory: " + playerGrain.ToString();

                if (currentFoodAmount == 0)
                {
                    granaryInfo += "\n\nGranary is empty.\n\nFree space in the granary: " + freeSpaceInGranary.ToString();
                }
                else
                {

                    if (freeSpaceInGranary == 0)
                    {
                        granaryInfo += "\n\nGranary is full.";
                    }
                    else
                    {
                        granaryInfo += "\n\nFree space in the granary: " + freeSpaceInGranary.ToString();
                    }
                }
                
                MBTextManager.SetTextVariable("GRANARY_INFO", granaryInfo, false);

            }, GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);



            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_max", new TextObject("Add the maximum amount of grain").ToString(), delegate (MenuCallbackArgs args)
            {
                if (playerGrain == 0 || freeSpaceInGranary == 0) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(10000);
            }, false, -1, false);

            
            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_50", new TextObject("Add 50 units of grain").ToString(), delegate (MenuCallbackArgs args)
            {

                if (playerGrain < 50 || freeSpaceInGranary < 50) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(50);
            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_100", new TextObject("Add 100 units of grain").ToString(), delegate (MenuCallbackArgs args)
            {
                if (playerGrain < 100 || freeSpaceInGranary < 100) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(100);
            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_150", new TextObject("Add 150 units of grain").ToString(), delegate (MenuCallbackArgs args)
            {
                if (playerGrain < 150 || freeSpaceInGranary < 150) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(150);
            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_200", new TextObject("Add 200 units of grain").ToString(), delegate (MenuCallbackArgs args)
            {
                if (playerGrain < 200 || freeSpaceInGranary < 200) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(200);
            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_300", new TextObject("Add 300 units of grain").ToString(), delegate (MenuCallbackArgs args)
            {
                if (playerGrain < 300 || freeSpaceInGranary < 300) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(300);
            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_other_foold", new TextObject("Add other type of food").ToString(), delegate (MenuCallbackArgs args)
            {
                if (freeSpaceInGranary < 1) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {

                InventoryManager.OpenScreenAsReceiveItems(_donateRoster, new TextObject("Granary"), new InventoryManager.DoneLogicExtrasDelegate(OnInventoryScreenDone));

            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_decline", new TextObject("Leave").ToString(), delegate (MenuCallbackArgs args)
            {
                args.optionLeaveType = GameMenuOption.LeaveType.Leave;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                if (Settlement.CurrentSettlement.IsCastle)
                {
                    GameMenu.SwitchToMenu("castle");
                }
                else
                {
                    GameMenu.SwitchToMenu("town_keep");
                }
            }, false, -1, false);



            // *** Castle ***
            campaignGameStarter.AddGameMenuOption("castle", "granary_castle", new TextObject("Visit the granary").ToString(), delegate (MenuCallbackArgs args)
            {
                args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                GameMenu.SwitchToMenu("granary_keep_menu");
            }, false, 4, false);


        }


        private void AddGrain(int amount)
        {
            int playerGrain = this.GetPlayerGrainAmount();
            int freeSpaceInGranary = this.GetGranarySpace();
            int maxTransaction;

            if (playerGrain > freeSpaceInGranary)
            {
                maxTransaction = freeSpaceInGranary;
            }
            else
            {
                maxTransaction = playerGrain;
            }

            int transferAmount = amount;

            if (transferAmount > maxTransaction)
            {
                transferAmount = maxTransaction;
            }

            float foodAmount = Settlement.CurrentSettlement.Town.FoodStocks;
            foodAmount += (float)transferAmount;

            float granaryMaxAmount = (float)Settlement.CurrentSettlement.Town.FoodStocksUpperLimit();

            if (foodAmount > granaryMaxAmount)
            {
                foodAmount = (float)Settlement.CurrentSettlement.Town.FoodStocksUpperLimit();
            }

            Settlement.CurrentSettlement.Town.FoodStocks = foodAmount;
            int reportAmount = transferAmount;

            transferAmount *= -1;
            PartyBase.MainParty.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("grain"), transferAmount);

            TextObject title = new("Grain Transferred");
            TextObject message = new(reportAmount.ToString() + " grain have been transferred to " + Settlement.CurrentSettlement.GetName().ToString() + "'s granary.");

            DonateFoodBaseBehavior.CreatePopupVMLayer(title.ToString(), "", message.ToString(), "", "donatetogranary4", new TextObject("Continue").ToString());

            SoundEvent.PlaySound2D("event:/ui/panels/panel_clan_open");

            this.GetCreditForHelping(reportAmount);

            GameMenu.SwitchToMenu("granary_keep_menu");

        }

        private int GetPlayerGrainAmount()
        {
            return PartyBase.MainParty.ItemRoster.GetItemNumber(MBObjectManager.Instance.GetObject<ItemObject>("grain"));
        }

        private int GetGranarySpace()
        {
            int foodMax = Settlement.CurrentSettlement.Town.FoodStocksUpperLimit();
            float currentFoodAmount = Settlement.CurrentSettlement.Town.FoodStocks;
            int freeSpace = foodMax - (int)currentFoodAmount;
            if (freeSpace < 0) freeSpace = 0;
            return freeSpace;
        }

        private void GetCreditForHelping(int amount)
        {
            int relation = amount / 50;
            foreach (Hero hero in Settlement.CurrentSettlement.Notables)
            {
                ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, hero, relation, true);
            }
        }


        private void OnInventoryScreenDone()
        {

            int itemCount = 0;
            bool otherTypeTransfered = false;

            for (int i = 0; i < _donateRoster.Count; i++)
            {
                ItemObject item = _donateRoster.GetItemAtIndex(i);
                if (item.IsFood)
                {
                    itemCount += _donateRoster.GetItemNumber(item);
                } else
                {
                    otherTypeTransfered = true;
                }
            }


            if (itemCount > 0 || otherTypeTransfered)
            {

                TextObject title = new("Food Transferred");
                TextObject message = new(itemCount.ToString() + " unit(s) of food have been transferred to " + Settlement.CurrentSettlement.GetName().ToString() + "'s granary.");

                float creditedAmount = (float)itemCount;

                if (itemCount > 0)
                {
                    float foodAmount = Settlement.CurrentSettlement.Town.FoodStocks;
                    float newFoodAmount = foodAmount + (float)itemCount;
                    float granaryMaxAmount = (float)Settlement.CurrentSettlement.Town.FoodStocksUpperLimit();
                    if (newFoodAmount > granaryMaxAmount)
                    {
                        newFoodAmount = (float)Settlement.CurrentSettlement.Town.FoodStocksUpperLimit();
                        otherTypeTransfered = true;
                        creditedAmount = granaryMaxAmount - foodAmount;
                    }
                    Settlement.CurrentSettlement.Town.FoodStocks = newFoodAmount;

                } else
                {
                    title = new("Items transferred");
                    message = new("No food have been transferred...");
                }


                TextObject textOverImage = new("");
                if (otherTypeTransfered) textOverImage = new("Locals are grateful for additional donations.");

                DonateFoodBaseBehavior.CreatePopupVMLayer(title.ToString(), "", message.ToString(), textOverImage.ToString(), "donatetogranary4", new TextObject("Continue").ToString());
                SoundEvent.PlaySound2D("event:/ui/panels/panel_clan_open");
                this.GetCreditForHelping((int)creditedAmount);

                GameMenu.SwitchToMenu("granary_keep_menu");

            }


            _donateRoster.Clear();

        }




        public override void SyncData(IDataStore dataStore) {}
    }


}


