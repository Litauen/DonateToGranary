using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace DonateToGranary
{
    internal class LHelpers
    {

        public static string ModName = Assembly.GetExecutingAssembly().GetName().Name;
        public static Version ModVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static string GetModName()
        {
            bool flag = LHelpers.ModName.ToString() == null;
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                result = LHelpers.ModName.ToString() + " v" + string.Format("{0}.{1}.{2}", LHelpers.ModVersion.Major, LHelpers.ModVersion.Minor, LHelpers.ModVersion.Build);
            }
            return result;
        }



        // Many Distance models described in DefaultMapDistanceModel
        //public override float GetDistance(MobileParty fromParty, Settlement toSettlement)
        // float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty, closestSettlement);

        public static Settlement GetClosestSettlement(MobileParty heroParty)
        {
            Settlement? closestSettlement = null;
            try
            {
                List<Settlement> settlements = Settlement.FindAll((Settlement s) => s.IsTown || s.IsCastle || s.IsVillage).ToList<Settlement>();
                closestSettlement = settlements.MinBy((Settlement s) => heroParty.GetPosition().DistanceSquared(s.GetPosition()));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return closestSettlement;
        }

        public static List<Settlement> GetClosestTownsFromSettlement(Settlement settlement, int amount)
        {
            List<Settlement> closestSettlements = new();
            try
            {

                if (settlement == null) return closestSettlements;
                
                if (settlement.IsTown) amount++; 
                
                List<Settlement> settlements = Settlement.FindAll((Settlement s) => s.IsTown).ToList<Settlement>();
                closestSettlements = settlements.OrderBy((Settlement s) => settlement.GetPosition().DistanceSquared(s.GetPosition())).Take(amount).ToList<Settlement>();
                
                if (settlement.IsTown) closestSettlements.RemoveAt(0); // removing the origin town
                
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return closestSettlements;
        }


        public static List<Settlement> GetClosestSettlementsFromSettlement(Settlement settlement, int amount)
        {
            List<Settlement> closestSettlements = new();
            try
            {

                if (settlement == null) return closestSettlements;

                amount++;

                List<Settlement> settlements = Settlement.FindAll((Settlement s) => s.IsTown || s.IsCastle || s.IsVillage).ToList<Settlement>();
                closestSettlements = settlements.OrderBy((Settlement s) => settlement.GetPosition().DistanceSquared(s.GetPosition())).Take(amount).ToList<Settlement>();

                closestSettlements.RemoveAt(0); // removing the origin settlement

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return closestSettlements;
        }

        // any settlement
        //Settlement randomSettlement = Settlement.All.GetRandomElement<Settlement>();

        //List<Settlement> list = (from x in hero.MapFaction.Settlements
        //                         where x.IsTown
        //                         select x).ToList<Settlement>();

        public static Settlement? GetRandomTown()
        {
            int num = 0;
            foreach (Settlement settlement in Campaign.Current.Settlements)
            {
                if (settlement.IsTown)
                {
                    num++;
                }
            }
            int num2 = MBRandom.RandomInt(0, num - 1);
            foreach (Settlement settlement2 in Campaign.Current.Settlements)
            {
                if (settlement2.IsTown)
                {
                    num2--;
                    if (num2 < 0)
                    {
                        return settlement2;
                    }
                }
            }
            return null;
        }

    }
}
