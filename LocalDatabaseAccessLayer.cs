using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.Linq;

public static class LocalDatabaseAccessLayer
{
    private static string databasePath = Application.persistentDataPath + "/mydatabase.db";

    public static LevelData LoadLevelData(int DBLevelId)
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            LevelData levelData = connection.Table<LevelData>().FirstOrDefault(c => c.DBLevelId == DBLevelId);

            if (levelData == null)
            {
                Debug.LogError("level data not found in the database for level id: " + DBLevelId);
            }
            return levelData;
        };
    }

    public static CharacterData LoadCharacterData(string prefabName)
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            CharacterData character = connection.Table<CharacterData>().FirstOrDefault(c => c.PrefabName == prefabName);

            if (character == null)
            {
                Debug.LogError("Character data not found in the database for prefab: " + prefabName);
            }
            return character;
        };
    }

    public static PlayerData LoadPlayerData()
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            PlayerData playerData = connection.Table<PlayerData>().FirstOrDefault();

            if (playerData == null)
            {
                Debug.LogError("Player data not found in the database");
            }
            return playerData;
        };
    }

    public static List<SpawnPointData> LoadSpawnPointData(int DBLevelId)
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            List<SpawnPointData> spawnPoints = connection.Table<SpawnPointData>().Where(spd => spd.DBLevelId == DBLevelId).ToList();

            if (spawnPoints == null)
            {
                Debug.LogError("spawnpoints not found in the database for levelId: " + DBLevelId);
            }
            return spawnPoints;
        };
    }

    public static RewardData LoadRewardData(int DBRewardID)
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadOnly))
        {
            RewardData rewardData = connection.Table<RewardData>().FirstOrDefault(c => c.DBRewardID == DBRewardID);

            if (rewardData == null)
            {
                Debug.LogError("Reward data not found in the database");
            }
            return rewardData;
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

    public static int GetEnemyTowerHealth(int DBLevelId)
    {
        LevelData levelData = LoadLevelData(DBLevelId);
        return levelData.EnemyTowerHealth;
    }

    public static int GetFriendlyTowerHealth()
    {
        PlayerData playerData = LoadPlayerData();
        return playerData.FriendlyTowerHealth;
    }

    public static float GetEnemyMultiplier(int DBLevelId)
    {
        LevelData levelData = LoadLevelData(DBLevelId);
        return levelData.EnemyMultiplier;
    }

    public static RewardData GetRewardData(int DBLevelId)
    {
        LevelData levelData = LoadLevelData(DBLevelId);
        RewardData rewardData = LoadRewardData(levelData.RewardDataId);
        return rewardData;
    }

    public static void UpdateNumTimesBeaten(int DBLevelId, int timesBeatenAdditive)
    {
        LevelData levelData = LoadLevelData(DBLevelId);
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite))
        {
            if (levelData == null)
            {
                Debug.LogError("level data not found in the database for level id: " + DBLevelId);
            }
            levelData.NumTimesBeaten += timesBeatenAdditive;
            connection.Update(levelData);
        };
    }

    public static void SpendXGems(int gemsSpent)
    {
        PlayerData playerData = LoadPlayerData();
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite))
        {
            if (playerData == null)
            {
                Debug.LogError("player data not found in the database");
            }
            playerData.Gems -= gemsSpent;
            connection.Update(playerData);
        };
    }

    public static void AddXPlattersToInventory(string platterType, int numPlatters)
    {
        PlayerData playerData = LoadPlayerData();
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite))
        {
            if (playerData == null)
            {
                Debug.LogError("player data not found in the database");
            }
            switch (platterType)
            {
                case ("bronzePlatter"):
                    playerData.BronzePlatters += numPlatters;
                    break;
                case ("silverPlatter"):
                    playerData.SilverPlatters += numPlatters;
                    break;
                case ("goldPlatter"):
                    playerData.GoldPlatters += numPlatters;
                    break;
                case ("DiamondPlatter"):
                    playerData.DiamondPlatters += numPlatters;
                    break;
                default:
                    Debug.Log("platterType does not exist");
                    break;
            }
            connection.Update(playerData);
        };
    }

    public static void AddXEvosToInventory(int numEvos)
    {
        PlayerData playerData = LoadPlayerData();
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite))
        {
            if (playerData == null)
            {
                Debug.LogError("player data not found in the database");
            }
            playerData.Evos += numEvos;
            connection.Update(playerData);
        };
    }

    public static void AddXPBottlesToInventory(string bottleType, int numBottles)
    {
        //PlayerData playerData = LoadPlayerData();
        //using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite))
        //{
        //    if (playerData == null)
        //    {
        //        Debug.LogError("player data not found in the database");
        //    }
        //    switch (bottleType)
        //    {
        //        case ("XPCup"):
        //            playerData.XPCups += numBottles;
        //            break;
        //        case ("XPPint"):
        //            playerData.XPPints += numBottles;
        //            break;
        //        case ("XPQuart"):
        //            playerData.XPQuarts += numBottles;
        //            break;
        //        case ("XPGallon"):
        //            playerData.XPGallons += numBottles;
        //            break;
        //        default:
        //            Debug.Log("bottle Type does not exist");
        //            break;
        //    }
        //    connection.Update(playerData);
        //};
    }

    public static void AddXEnergyRefillsToInventory(int numRefills)
    {
        PlayerData playerData = LoadPlayerData();
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite))
        {
            if (playerData == null)
            {
                Debug.LogError("player data not found in the database");
            }
            playerData.EnergyRefills += numRefills;
            connection.Update(playerData);
        };
    }

    public static void AddXSurvivalRevivesToInventory(int numRevives)
    {
        PlayerData playerData = LoadPlayerData();
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite))
        {
            if (playerData == null)
            {
                Debug.LogError("player data not found in the database");
            }
            playerData.SurvivalRevives += numRevives;
            connection.Update(playerData);
        };
    }

    public static void InsertSpawnPointData(SpawnPointData DBSpawnPointData)
    {
        using (var connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite))
        {
            if (DBSpawnPointData == null)
            {
                Debug.LogError("spawn data null");
            }
            connection.Insert(DBSpawnPointData);
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
