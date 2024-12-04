using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.Linq;

public static class LocalDatabaseAccessLayer
{
    private static string databasePath = Application.persistentDataPath + "/mydatabase.db";

    public static CharacterData LoadCharacterData(string prefabName)
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            CharacterData DBCharacter = connection.Table<CharacterData>().FirstOrDefault(c => c.PrefabName == prefabName);

            if (DBCharacter == null)
            {
                Debug.LogError("Character data not found in the database for prefab: " + prefabName);
            }
            return DBCharacter;
        };
    }

    public static void ApplySensitiveCharacterData(string prefabName, CharacterData characterData)
    {
        if(characterData == null)
        {
            return;
        }

        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            SensitiveCharacterData sensitiveCharacterData = connection.Table<SensitiveCharacterData>().FirstOrDefault(c => c.DBCharacterId == characterData.DBCharacterId);

            if (sensitiveCharacterData == null)
            {
                Debug.LogError("Sensitive character data not found in the database for prefab: " + prefabName);
            }
            else
            {
                characterData.Attack += sensitiveCharacterData.Level * (characterData.Attack / 10);
                characterData.Defense += sensitiveCharacterData.Level * (characterData.Defense / 10);
                characterData.Health += sensitiveCharacterData.Level * (characterData.Health / 10);
            }
        };
    }

    public static List<SpawnPointData> GetSpawnPoints(int levelId)
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            List<SpawnPointData> spawnPoints = connection.Table<SpawnPointData>().Where(spd => spd.DBLevelId == levelId).ToList();

            if (spawnPoints == null)
            {
                Debug.LogError("spawnpoints not found in the database for levelId: " + levelId);
            }
            return spawnPoints;
        };
    }

    public static CharacterData GetEnemyFromSpawnPoint(int spawnPointCharacterId)
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            CharacterData enemy = connection.Table<CharacterData>().FirstOrDefault(e => e.DBCharacterId == spawnPointCharacterId);

            if (enemy == null)
            {
                Debug.LogError("character data not found in the database for spawnpoint characterId: " + spawnPointCharacterId);
            }
            return enemy;
        };
    }

    public static List<CharacterData> GetGachaCharacters(string quality)
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            List<CharacterData> gachaCharacters = connection.Table<CharacterData>().Where(c => c.Quality == quality && c.EvolutionNumber == 0 && c.isEnemy == false && c.isTower == false).ToList();

            if (gachaCharacters == null)
            {
                Debug.LogError("gachacharacters data not found in the database for quality: " + quality);
            }
            return gachaCharacters;
        };
    }

    //public static void UpsertData(string prefabName, CharacterData characterData)
    //{
    //    if (characterData == null)
    //    {
    //        return;
    //    }

    //    using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
    //    {
    //        SensitiveCharacterData sensitiveCharacterData = connection.Table<SensitiveCharacterData>().FirstOrDefault(c => c.DBCharacterId == characterData.DBCharacterId);

    //        if (sensitiveCharacterData == null)
    //        {
    //            Debug.LogError("Sensitive character data not found in the database for prefab: " + prefabName);
    //        }
    //        else
    //        {
    //            characterData.Attack += sensitiveCharacterData.Level * (characterData.Attack / 10);
    //            characterData.Defense += sensitiveCharacterData.Level * (characterData.Defense / 10);
    //            characterData.Health += sensitiveCharacterData.Level * (characterData.Health / 10);
    //        }
    //    };
    //}
}
