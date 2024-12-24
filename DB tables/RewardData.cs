using UnityEngine;
using SQLite4Unity3d;
using System;
using System.Globalization;

public class RewardData //this can be used as GENERIC reward data - levels, daily reward, survival, etc
{
    [PrimaryKey]
    public int DBRewardID { get; set; }
    public int Gold { get; set; }
    public int Gems { get; set; }
    public int Evos { get; set; }
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

    public int DailyRewardDayNum { get; set; } //for daily rewards
    public string DateOfDailyReward { get; set; } // ISO 8601 strings ("YYYY-MM-DD HH:MM:SS.SSS" format)
    public bool RewardObtained { get; set; }

    public RewardData()
    {

    }

    public RewardData(
        int DBRewardID,
        int Gold,
        int Gems,
        int Evos,
        int BronzePlatters,
        int SilverPlatters,
        int GoldPlatters,
        int DiamondPlatters,
        int SurvivalRevives,
        int EnergyRefills,
        int DailyRewardDayNum,
        string DateOfDailyReward,
        bool RewardObtained
        )
    {
        setDBRewardID(DBRewardID);
        setGold(Gold);
        setGems(Gems);
        setEvos(Evos);
        setBronzePlatters(BronzePlatters);
        setSilverPlatters(SilverPlatters);
        setGoldPlatters(GoldPlatters);
        setDiamondPlatters(DiamondPlatters);
        setSurvivalRevives(SurvivalRevives);
        setEnergyRefills(EnergyRefills);
        setDailyRewardDayNum(DailyRewardDayNum);
        setDateOfDailyReward(DateOfDailyReward);
        setRewardObtained(RewardObtained);
    }

    public void setDBRewardID(int a)
    {
        Debug.Assert(a >= 0);
        DBRewardID = a;
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

    public void setDailyRewardDayNum(int a)
    {
        Debug.Assert(a >= -1);
        DailyRewardDayNum = a;
    }
    
    public void setDateOfDailyReward(string a)
    {
        bool isDateTime = DateTime.TryParseExact(
                a,
                "yyyy-MM-ddTHH:mm:ssZ", // ISO 8601 datetime format
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out _);
        Debug.Assert(isDateTime || a == "");
        DateOfDailyReward = a;
    }
    public void setRewardObtained(bool a)
    {
        RewardObtained = a;
    }
}