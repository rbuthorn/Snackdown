using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonProcessorToDBUpserter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ImportJsons();
        //ProcessAndCleanLevelJsonData();
        //ProcessAndCleanCharacterJsonData();
        //ProcessAndCleanRewardJsonData();
        ProcessAndCleanSpawnPointJsonData();
        //UpsertDataToDB(); //generic function for any data
    }

    void ProcessAndCleanSpawnPointJsonData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("JSON docs/Enemy Spawns");

        if (textAsset != null)
        {
            // Use StringReader to read it line by line
            using (StringReader reader = new StringReader(textAsset.text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    SpawnPlaceholderData spawn = JsonUtility.FromJson<SpawnPlaceholderData>(line);
                    SpawnPointData spawnPoint = new SpawnPointData(
                        DBSpawnPointId: spawn.DBSpawnPointId,
                        DBCharacterId: spawn.DBCharacterId,
                        DBLevelId: spawn.DBLevelId,
                        StartSpawningInXSecs: spawn.StartSpawningInXSecs,
                        SpawnAfterXFriendlies: spawn.SpawnAfterXFriendlies,
                        SpawnAfterXTowerDamage: spawn.SpawnAfterXTowerDamage,
                        NumSpawning: spawn.NumSpawning,
                        TimeBetweenSpawns: spawn.TimeBetweenSpawns,
                        SpawnAfterXSecs: spawn.SpawnAfterXSecs,
                        NumBatches: spawn.NumBatches,
                        TimeBetweenBatches: spawn.TimeBetweenBatches,
                        SpawnAfterXBosses: spawn.SpawnAfterXBosses
                    );
                    Debug.Log(line);
                    LocalDatabaseAccessLayer.InsertSpawnPointData(spawnPoint);
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load the text file. Make sure it's in Resources/JsonDocs/ and has no file extension in the Load call.");
        }
    }
}
