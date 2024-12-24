using UnityEngine;
using SQLite4Unity3d;

public class PlayerData
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int XP { get; set; }
    public int PlayerLevel {get; set; } //calculate player level every time xp is increased
    public int PowerLevel {get; set; }
    public int FriendlyTowerHealth { get; set; }

    //all items
    public int Gold { get; set; }
    public int Gems { get; set; }
    public int Evos { get; set; }
    public int LevelVegetableStone { get; set; }
    public int LevelFruitStone { get; set; }
    public int LevelDairyStone { get; set; }
    public int LevelSeafoodStone { get; set; }
    public int LevelMeatStone { get; set; }
    public int LevelDessertStone { get; set; }
    //public int SaltyCrystals { get; set; }
    //public int SourCrystals { get; set; }
    //public int SweetCrystals { get; set; }
    //public int UmamiCrystals { get; set; }
    //public int BitterCrystals { get; set; }
    public int BronzePlatters { get; set; }
    public int SilverPlatters { get; set; }
    public int GoldPlatters { get; set; }
    public int DiamondPlatters { get; set; }
    public int SurvivalRevives { get; set; }
    public int EnergyRefills { get; set; }
    //xp bottles here

    public PlayerData()
    {

    }

    public PlayerData(
        int Id,
        string Username,
        int XP,
        int PlayerLevel,
        int PowerLevel,
        int FriendlyTowerHealth,
        int Gold,
        int Gems,
        int Evos,
        int LevelVegetableStone,
        int LevelFruitStone,
        int LevelDairyStone,
        int LevelSeafoodStone,
        int LevelMeatStone,
        int LevelDessertStone,
        //int SaltyCrystals,
        //int SourCrystals,
        //int SweetCrystals,
        //int UmamiCrystals,
        //int BitterCrystals,
        int BronzePlatters,
        int SilverPlatters,
        int GoldPlatters,
        int DiamondPlatters,
        int SurvivalRevives,
        int EnergyRefills
   )
    {
        setId(Id);
        setUsername(Username);
        setXP(XP);
        setPlayerLevel(PlayerLevel);
        setPowerLevel(PowerLevel);
        setFriendlyTowerHealth(FriendlyTowerHealth);
        setGold(Gold);
        setGems(Gems);
        setEvos(Evos);
        setLevelVegetableStone(LevelVegetableStone);
        setLevelFruitStone(LevelFruitStone);
        setLevelDairyStone(LevelDairyStone);
        setLevelSeafoodStone(LevelSeafoodStone);
        setLevelMeatStone(LevelMeatStone);
        setLevelDessertStone(LevelDessertStone);
        //setSaltyCrystals(SaltyCrystals);
        //setSourCrystals(SourCrystals);
        //setSweetCrystals(SweetCrystals);
        //setUmamiCrystals(UmamiCrystals);
        //setBitterCrystals(BitterCrystals);
        setBronzePlatters(BronzePlatters);
        setSilverPlatters(SilverPlatters);
        setGoldPlatters(GoldPlatters);
        setDiamondPlatters(DiamondPlatters);
        setSurvivalRevives(SurvivalRevives);
        setEnergyRefills(EnergyRefills);
    }

    public void setId(int a)
    {
        Debug.Assert(a >= 0);
        Id = a;
    }
    public void setUsername(string a)
    {
        Debug.Assert(a.Length > 0);
        Username = a;
    }
    public void setXP(int a)
    {
        Debug.Assert(a >= 0);
        XP = a;
    }
    public void setPlayerLevel(int a)
    {
        Debug.Assert(a >= 1);
        PlayerLevel = a;
    }
    public void setPowerLevel(int a)
    {
        Debug.Assert(a >= 0);
        PowerLevel = a;
    }
    public void setFriendlyTowerHealth(int a)
    {
        Debug.Assert(a >= 0);
        FriendlyTowerHealth = a;
    }
    public void setGold(int a)
    {
        Debug.Assert(a >= 0);
        Gold = a;
    }
    public void setGems(int a)
    {
        Debug.Assert(a >= 0);
        Gems = a;
    }
    public void setEvos(int a)
    {
        Debug.Assert(a >= 0);
        Evos = a;
    }
    public void setLevelVegetableStone(int a)
    {
        Debug.Assert(a >= 0);
        LevelVegetableStone = a;
    }
    public void setLevelFruitStone(int a)
    {
        Debug.Assert(a >= 0);
        LevelFruitStone = a;
    }
    public void setLevelDairyStone(int a)
    {
        Debug.Assert(a >= 0);
        LevelDairyStone = a;
    }
    public void setLevelSeafoodStone(int a)
    {
        Debug.Assert(a >= 0);
        LevelSeafoodStone = a;
    }
    public void setLevelMeatStone(int a)
    {
        Debug.Assert(a >= 0);
        LevelMeatStone = a;
    }
    public void setLevelDessertStone(int a)
    {
        Debug.Assert(a >= 0);
        LevelDessertStone = a;
    }
    //public void setSaltyCrystals(int a)
    //{
    //    Debug.Assert(a >= 0);
    //    SaltyCrystals = a;
    //}
    //public void setSourCrystals(int a)
    //{
    //    Debug.Assert(a >= 0);
    //    SourCrystals = a;
    //}
    //public void setSweetCrystals(int a)
    //{
    //    Debug.Assert(a >= 0);
    //    SweetCrystals = a;
    //}
    //public void setUmamiCrystals(int a)
    //{
    //    Debug.Assert(a >= 0);
    //    UmamiCrystals = a;
    //}
    //public void setBitterCrystals(int a)
    //{
    //    Debug.Assert(a >= 0);
    //    BitterCrystals = a;
    //}
    public void setBronzePlatters(int a)
    {
        Debug.Assert(a >= 0);
        BronzePlatters = a;
    }
    public void setSilverPlatters(int a)
    {
        Debug.Assert(a >= 0);
        SilverPlatters = a;
    }
    public void setGoldPlatters(int a)
    {
        Debug.Assert(a >= 0);
        GoldPlatters = a;
    }
    public void setDiamondPlatters(int a)
    {
        Debug.Assert(a >= 0);
        DiamondPlatters = a;
    }
    public void setSurvivalRevives(int a)
    {
        Debug.Assert(a >= 0);
        SurvivalRevives = a;
    }
    public void setEnergyRefills(int a)
    {
        Debug.Assert(a >= 0);
        EnergyRefills = a;
    }
}

