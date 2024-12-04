using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class SensitiveCharacterData
{
    [PrimaryKey]
    public int DBCharacterId { get; set; }
    public float Level { get; set; } //float so that progress bars can be implemented, but int in practice(UI/UX)
    public bool IsUnlocked { get; set; }
    public bool DeadInSurvival {get; set; }

    public SensitiveCharacterData(
        int DBCharacterId,
        float Level,
        bool IsUnlocked,
        bool DeadInSurvival
        )
    {
        setDBCharacterID(DBCharacterId);
        setLevel(Level);
        setIsUnlocked(IsUnlocked);
        setDeadInSurvival(DeadInSurvival);
    }

    public SensitiveCharacterData()
    {

    }
    public void setDBCharacterID(int a)
    {
        Debug.Assert(a >= 0);
        DBCharacterId = a;
    }
    public void setLevel(float a)
    {
        Debug.Assert(a > 0);
        Level = a;
    }
    public void setIsUnlocked(bool a)
    {
        IsUnlocked = a;
    }
    public void setDeadInSurvival(bool a)
    {
        DeadInSurvival = a;
    }
}