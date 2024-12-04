using UnityEngine;
using SQLite4Unity3d;
using System;
using System.Globalization;

public class AchievementData
{
    //an achievement cant be coded into the data - so each obtained achievement has to be triggered by something in the actual game code - and then can update the data here
    [PrimaryKey]
    public int DBAchievementId { get; set; }
    public bool Obtained { get; set; }
    public string DateObtained { get; set; } // ISO 8601 strings ("YYYY-MM-DD HH:MM:SS.SSS" format)
    public string Name { get; set; }
    public string Description { get; set; }
    public string AchievementLevel { get; set; } //has to be bronze, silver, or gold

    public AchievementData(
        int DBAchievementId,
        bool Obtained,
        string DateObtained,
        string Name,
        string Description,
        string AchievementLevel
        )
    {
        setDBAchievementId(DBAchievementId);
        setObtained(Obtained);
        setDateObtained(DateObtained);
        setName(Name);
        setDescription(Description);
        setAchievementLevel(AchievementLevel);
    }

    public void setDBAchievementId(int a)
    {
        Debug.Assert(a >= 0);
        DBAchievementId = a;
    }

    public void setObtained(bool a)
    {
        Obtained = a;
    }

    public void setDateObtained(string a)
    {
        bool isDateTime = DateTime.TryParseExact(
                a,
                "yyyy-MM-ddTHH:mm:ssZ", // ISO 8601 datetime format
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal,
                out _);
        Debug.Assert(isDateTime);
        DateObtained = a;
    }

    public void setName(string a)
    {
        Debug.Assert(a != "");
        Name = a;
    }

    public void setDescription(string a)
    {
        Debug.Assert(a != "");
        Description = a;
    }

    public void setAchievementLevel(string a)
    {
        Debug.Assert(a == "bronze" || a == "silver" || a == "gold");
        AchievementLevel = a;
    }
}
