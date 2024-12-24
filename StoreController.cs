using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreController : MonoBehaviour
{
    private Dictionary<string, int> prices = new Dictionary<string, int>{{"bronzePlatter",3}, {"silverPlatter",30 }, {"goldPlatter",180 }, {"diamondPlatter",600 }, {"evo",5 }, {"XPCup",10 },
        {"XPPint",20 }, {"XPQuart",40 }, {"XPGallon",80 }, {"energyRefill",25 }, {"survivalRevive",25} };
    private const float multiplicativeDiscountOnBulk = 2 / 3;

    //implement sales somehow... without a server - we will just have to update the game code and then send out the updates. maybe force players to update before playing
    //with a server - can deploy sales at any time in real time

    void BuyBulkPlatters(int numPlatters, string platterType)
    {
        int totalCost = (int) multiplicativeDiscountOnBulk * (prices[platterType] * numPlatters);
        LocalDatabaseAccessLayer.SpendXGems(totalCost);
        LocalDatabaseAccessLayer.AddXPlattersToInventory(platterType, numPlatters);
    }

    void BuyOnePlatter(string platterType)
    {
        int totalCost = prices[platterType];
        LocalDatabaseAccessLayer.SpendXGems(totalCost);
        LocalDatabaseAccessLayer.AddXPlattersToInventory(platterType, 1);
    }

    void BuyBulkEvos(int numEvos)
    {
        int totalCost = (int) multiplicativeDiscountOnBulk * (numEvos*prices["evo"]);
        LocalDatabaseAccessLayer.SpendXGems(totalCost);
        LocalDatabaseAccessLayer.AddXEvosToInventory(numEvos);
    }

    void BuyOneEvo()
    {
        int totalCost = prices["evo"];
        LocalDatabaseAccessLayer.SpendXGems(totalCost);
        LocalDatabaseAccessLayer.AddXEvosToInventory(1);
    }

    void BuyOneXPBottle(string bottleType)
    {
        int totalCost = prices[bottleType];
        LocalDatabaseAccessLayer.SpendXGems(totalCost);
        LocalDatabaseAccessLayer.AddXPBottlesToInventory(bottleType, 1);
    }

    void BuyOneEnergyRefill()
    {
        int totalCost = prices["energyRefill"];
        LocalDatabaseAccessLayer.SpendXGems(totalCost);
        LocalDatabaseAccessLayer.AddXEnergyRefillsToInventory(1);
    }

    void BuyOneSurvivalRevive()
    {
        int totalCost = prices["survivalRevive"];
        LocalDatabaseAccessLayer.SpendXGems(totalCost);
        LocalDatabaseAccessLayer.AddXSurvivalRevivesToInventory(1);
    }
}
