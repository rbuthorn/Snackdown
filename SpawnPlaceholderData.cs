using UnityEngine;
using SQLite4Unity3d;

[System.Serializable]
public class SpawnPlaceholderData
{
    public int DBSpawnPointId;
    public int DBCharacterId; //must match id of intended enemy
    public int DBLevelId;

    //for every entry in spawnpoint data - two of these will be Math.inf, one will be an actual value... 1 inf and 2 actual values will act as an or clause between the 2 actual values
    public float StartSpawningInXSecs;
    public int SpawnAfterXFriendlies;
    public int SpawnAfterXTowerDamage;

    public int NumSpawning; //how many are going to be spawned
    public float TimeBetweenSpawns; //time between the numspawning

    //extra time before the spawning actually begins - eg if spawnafterXtowerdamage condition is it, wait spawnAfterXSecs to actually spawn
    public float SpawnAfterXSecs;

    public int NumBatches;
    public float TimeBetweenBatches;
    public int SpawnAfterXBosses;

    public SpawnPlaceholderData()
    {
    }
}