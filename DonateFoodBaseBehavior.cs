using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace DonateToGranary
{
    public partial class DonateFoodBaseBehavior : CampaignBehaviorBase
    {

        private static GauntletLayer? _gauntletLayer;
        private static GauntletMovie? _gauntletMovie;
        private static DTGPopupVM? _popupVM;

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddGranaryMenuItems));
        }
         
        private void AddGranaryMenuItems(CampaignGameStarter campaignGameStarter)
        {

            int playerGrain = 0;
            int freeSpaceInGranary = 0;

            campaignGameStarter.AddGameMenuOption("town_keep", "granary_keep", "Visit the granary", delegate (MenuCallbackArgs args)
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



            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_max", "Add the maximum amount of grain", delegate (MenuCallbackArgs args)
            {
                if (playerGrain == 0 || freeSpaceInGranary == 0) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(10000);
            }, false, -1, false);

            
            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_50", "Add 50 units of grain", delegate (MenuCallbackArgs args)
            {

                if (playerGrain < 50 || freeSpaceInGranary < 50) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(50);
            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_100", "Add 100 units of grain", delegate (MenuCallbackArgs args)
            {
                if (playerGrain < 100 || freeSpaceInGranary < 100) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(100);
            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_150", "Add 150 units of grain", delegate (MenuCallbackArgs args)
            {
                if (playerGrain < 150 || freeSpaceInGranary < 150) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(150);
            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_200", "Add 200 units of grain", delegate (MenuCallbackArgs args)
            {
                if (playerGrain < 200 || freeSpaceInGranary < 200) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(200);
            }, false, -1, false);

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_add_300", "Add 300 units of grain", delegate (MenuCallbackArgs args)
            {
                if (playerGrain < 300 || freeSpaceInGranary < 300) return false;
                args.optionLeaveType = GameMenuOption.LeaveType.Ransom;
                return true;
            }, delegate (MenuCallbackArgs args)
            {
                this.AddGrain(300);
            }, false, -1, false);
            

            campaignGameStarter.AddGameMenuOption("granary_keep_menu", "granary_keep_decline", "Leave", delegate (MenuCallbackArgs args)
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
            campaignGameStarter.AddGameMenuOption("castle", "granary_castle", "Visit the granary", delegate (MenuCallbackArgs args)
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

            string title = "Grain Transferred";
            string message = reportAmount.ToString() + " grain have been transferred to " + Settlement.CurrentSettlement.GetName().ToString() + "'s granary.\n";

            if (Settlement.CurrentSettlement.IsTown)
            {
                message += "The local notables were impressed with your generosity.";
            }

            //InformationManager.ShowInquiry(new InquiryData(title, message, true, false, "OK", "", null, null, ""), true);

            DonateFoodBaseBehavior.CreatePopupVMLayer(title, "", message, "", "donatetogranary4", "Continue");

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


        public override void SyncData(IDataStore dataStore) {}
    }
}
