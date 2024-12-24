using UnityEngine;
using SQLite4Unity3d;

public class LevelData
{
    [PrimaryKey]
    public int DBLevelId { get; set; }
    public string LevelName { get; set; }

    //names of graphics
    public string Wall { get; set; }
    public string Floor { get; set; }
    public string Basement { get; set; }
    public string EnemyTower { get; set; }

    public int EnemyTowerHealth { get; set; }
    //enemies list is stored in DB as SpawnPointData
    public string LevelType { get; set; } //campaign, pvp, spacelines, survival, etc
    public string Chapter { get; set; } //"" if not a campaign level, otherwise name of chapter
    public float EnemyMultiplier { get; set; } //sets multiplier to attack, defense, and health
    public int NumTimesBeaten { get; set; }
    public int Difficulty { get; set; } //rating of how difficult this level is
    public bool isHardModeLevel { get; set; }
    public int HardModeLevelId { get; set; } // for campaign or any other place where theres two versions of a level. should be -1 if no hard mode version.
    public int RewardDataId { get; set; }

    public LevelData()
    {

    }

    public LevelData(
        int DBLevelId,
        string LevelName,
        string Wall,
        string Floor,
        string Basement,
        string EnemyTower,
        int EnemyTowerHealth,
        string LevelType,
        string Chapter,
        int EnemyMultiplier,
        int NumTimesBeaten,
        int Difficulty,
        bool isHardModeLevel,
        int HardModeLevelId,
        int RewardDataId
    )
    {
        setDBLevelId(DBLevelId);
        setLevelName(LevelName);
        setWall(Wall);
        setFloor(Floor);
        setBasement(Basement);
        setEnemyTower(EnemyTower);
        setEnemyTowerHealth(EnemyTowerHealth);
        setLevelType(LevelType);
        setChapter(Chapter);
        setEnemyMultiplier(EnemyMultiplier);
        setNumTimesBeaten(NumTimesBeaten);
        setDifficulty(Difficulty);
        setIsHardModeLevel(isHardModeLevel);
        setHardModeLevelId(HardModeLevelId);
        setRewardDataId(RewardDataId);
    }

    public void setDBLevelId(int a)
    {
        Debug.Assert(a >= 0);
        DBLevelId = a;
    }
    public void setLevelName(string a)
    {
        Debug.Assert(a.Length > 0);
        LevelName = a;
    }
    public void setWall(string a)
    {
        Debug.Assert(a.Length > 0);
        Wall = a;
    }
    public void setFloor(string a)
    {
        Debug.Assert(a.Length > 0);
        Floor = a;
    }
    public void setBasement(string a)
    {
        Debug.Assert(a.Length > 0);
        Basement = a;
    }
    public void setEnemyTower(string a)
    {
        Debug.Assert(a.Length > 0);
        EnemyTower = a;
    }
    public void setEnemyTowerHealth(int a)
    {
        Debug.Assert(a > 0);
        EnemyTowerHealth = a;
    }
    public void setLevelType(string a)
    {
        Debug.Assert(a == "campaign" || a == "pvp" || a == "survival" || a == "spacelines");
        LevelType = a;
    }
    public void setChapter(string a)
    {
        Debug.Assert(a == "herbshire" || a == "sizzle city" || a == "citrus shores" || a == "milky meadows" || a == "marshmallow mountains" || a == "underwater palace");
        Chapter = a;
    }
    public void setEnemyMultiplier(float a)
    {
        Debug.Assert(a > 0);
        EnemyMultiplier = a;
    }
    public void setNumTimesBeaten(int a)
    {
        Debug.Assert(a >= 0);
        NumTimesBeaten = a;
    }
    public void setEnemyMultiplier(int a)
    {
        Debug.Assert(a >= 1);
        EnemyMultiplier = a;
    }
    public void setDifficulty(int a)
    {
        Debug.Assert(a >= 1);
        Difficulty = a;
    }
    public void setIsHardModeLevel(bool a)
    {
        isHardModeLevel = a;
    }
    public void setHardModeLevelId(int a)
    {
        Debug.Assert(a >= 0);
        HardModeLevelId = a;
    }
    public void setRewardDataId(int a)
    {
        Debug.Assert(a >= 0);
        RewardDataId = a;
    }
}