using UnityEngine;
using SQLite4Unity3d;

public class SpawnPointData
{
    [PrimaryKey]
    public int DBSpawnPointId { get; set; }
    public int DBCharacterId { get; set; } //must match id of intended enemy
    public int DBLevelId { get; set; }

    //for every entry in spawnpoint data - two of these will be Math.inf, one will be an actual value... 1 inf and 2 actual values will act as an or clause between the 2 actual values
    public float StartSpawningInXSecs { get; set; }
    public int SpawnAfterXFriendlies { get; set; }
    public int SpawnAfterXTowerDamage { get; set; }

    public int NumSpawning { get; set; } //how many are going to be spawned
    public float TimeBetweenSpawns { get; set; } //time between the numspawning

    //extra time before the spawning actually begins - eg if spawnafterXtowerdamage condition is it, wait spawnAfterXSecs to actually spawn
    public float SpawnAfterXSecs { get; set; }

    public SpawnPointData(
        int DBSpawnPointId,
        int DBCharacterId,
        int DBLevelId,
        float StartSpawningInXSecs,
        int SpawnAfterXFriendlies,
        int SpawnAfterXTowerDamage,
        int NumSpawning,
        float TimeBetweenSpawns,
        float SpawnAfterXSecs
    )
    {
        setDBSpawnPointId(DBSpawnPointId);
        setDBCharacterId(DBCharacterId);
        setDBLevelId(DBLevelId);
        setStartSpawningInXSecs(StartSpawningInXSecs);
        setSpawnAfterXFriendlies(SpawnAfterXFriendlies);
        setSpawnAfterXTowerDamage(SpawnAfterXTowerDamage);
        setNumSpawning(NumSpawning);
        setTimeBetweenSpawns(TimeBetweenSpawns);
        setSpawnAfterXSecs(SpawnAfterXSecs);
    }
    public void setDBSpawnPointId(int a)
    {
        Debug.Assert(a >= 0);
        DBSpawnPointId = a;
    }
    public void setDBCharacterId(int a)
    {
        Debug.Assert(a >= 0);
        DBCharacterId = a;
    }
    public void setDBLevelId(int a)
    {
        Debug.Assert(a >= 0);
        DBLevelId = a;
    }
    public void setStartSpawningInXSecs(float a)
    {
        Debug.Assert(a >= 0);
        StartSpawningInXSecs = a;
    }
    public void setSpawnAfterXFriendlies(int a)
    {
        Debug.Assert(a >= 0);
        SpawnAfterXFriendlies = a;
    }
    public void setSpawnAfterXTowerDamage(int a)
    {
        Debug.Assert(a >= 0);
        SpawnAfterXTowerDamage = a;
    }
    public void setNumSpawning(int a)
    {
        Debug.Assert(a > 0);
        NumSpawning = a;
    }
    public void setTimeBetweenSpawns(float a)
    {
        Debug.Assert(a >= 0);
        TimeBetweenSpawns = a;
    }
    public void setSpawnAfterXSecs(float a)
    {
        Debug.Assert(a >= 0);
        SpawnAfterXSecs = a;
    }
    public SpawnPointData()
    {

    }
}