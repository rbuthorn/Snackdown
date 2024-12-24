using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System;

public class DatabaseManager : MonoBehaviour
{
    private SQLiteConnection connection;
    private const string CURRENT_VERSION = "0.0.0";
    void Start()
    {
        // Establish connection to the database
        string databasePath = Application.persistentDataPath + "/mydatabase.db";

        //check if very first launch
        using (connection = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create))
        {
            //CheckGameVersion();
            WIPE_DATABASE();
            onFirstLaunch();
        };
    }

    private void WIPE_DATABASE()
    {
        connection.DropTable<PlayerData>();
        connection.DropTable<CharacterData>();
        connection.DropTable<LevelData>();
        connection.DropTable<FlagData>();
        connection.DropTable<SpawnPointData>();
        connection.DropTable<SensitiveCharacterData>();
        connection.DropTable<GameVersion>();
    }

    private void onFirstLaunch() //call this only once ever
    {
        InitPlayerPrefs();
        CreateDBTables();
        PopulateDB();
    }

    private void InitPlayerPrefs()
    {
        //change this to include just baby carrot in the lineup
        PlayerPrefs.SetString("CharacterLineupData", "");
        PlayerPrefs.SetInt("TimePlayed", 0);
        PlayerPrefs.SetInt("Energy", 50);
    }

    private void CreateDBTables(){
        connection.CreateTable<PlayerData>(); //checks if table exists - if not creates, if so ignores
        connection.CreateTable<CharacterData>();
        connection.CreateTable<SensitiveCharacterData>();
        connection.CreateTable<SpawnPointData>();
        connection.CreateTable<LevelData>();
        connection.CreateTable<FlagData>();
        connection.CreateTable<GameVersion>();
        connection.CreateTable<AchievementData>();
    }

    private void PopulateDB(){
        initPlayer();
        initCharacters();
        initLevels();
        initFlags();
        initSpawnPoints();
        initSensitiveCharacterData();
    }

    private void initCharacters(){
        initBabyCarrot();
        initAgedCheese();
        initAvocadolet();
        initBabyMussels();
        initBananaPhone();
        initBanana();
        initFriendlyTower();
        initSpoon();
        initFork();
        initEnemyTower();
    }

    private void initLevels(){
        LevelData Level0 = new LevelData(
            DBLevelId: 0,
            LevelName: "sample level",
            Wall: "na",
            Floor: "na",
            Basement: "na",
            EnemyTower: "na",
            EnemyTowerHealth: 100,
            LevelType: "campaign",
            Chapter: "herbshire",
            EnemyMultiplier: 1,
            NumTimesBeaten: 0,
            Difficulty: 1,
            isHardModeLevel: false,
            HardModeLevelId: -1,
            RewardDataId: 0
        );
        InsertLevelData(Level0);

        LevelData Level1 = new LevelData(
            DBLevelId: 1,
            LevelName: "second level(load tester)",
            Wall: "na",
            Floor: "na",
            Basement: "na",
            EnemyTower: "na",
            EnemyTowerHealth: 100,
            LevelType: "campaign",
            Chapter: "herbshire",
            EnemyMultiplier: 1,
            NumTimesBeaten: 0,
            Difficulty: 1,
            isHardModeLevel: false,
            HardModeLevelId: -1,
            RewardDataId: 0
        );
        InsertLevelData(Level1);
    }

    private void initFlags(){

    }

    private void initSpawnPoints()
    {
        SpawnPointData spawnpoint0 = new SpawnPointData(
            DBSpawnPointId: 0,
            DBCharacterId: 7,
            DBLevelId: 0,
            StartSpawningInXSecs: 5,
            SpawnAfterXFriendlies: int.MaxValue,
            SpawnAfterXTowerDamage: int.MaxValue,
            NumSpawning: 20,
            TimeBetweenSpawns: 1f,
            SpawnAfterXSecs: 0
        );
        InsertSpawnPointData(spawnpoint0);

        SpawnPointData spawnpoint1 = new SpawnPointData(
            DBSpawnPointId: 1,
            DBCharacterId: 7,
            DBLevelId: 1,
            StartSpawningInXSecs: int.MaxValue,
            SpawnAfterXFriendlies: 1,
            SpawnAfterXTowerDamage: int.MaxValue,
            NumSpawning: 200,
            TimeBetweenSpawns: .1f,
            SpawnAfterXSecs: 5
        );
        InsertSpawnPointData(spawnpoint1);
    }

    private void initSensitiveCharacterData()
    {

    }

    private void initPlayer()
    {
        PlayerData player = new PlayerData(
            Id: 0,
            Username: "rob",
            XP: 0,
            PlayerLevel: 1,
            PowerLevel: 1,
            FriendlyTowerHealth: 300,
            Gold: 100,
            Gems: 0,
            Evos: 0,
            LevelVegetableStone: 1,
            LevelFruitStone: 1,
            LevelDairyStone: 1,
            LevelSeafoodStone: 1,
            LevelMeatStone: 1,
            LevelDessertStone: 1,
            BronzePlatters: 0,
            SilverPlatters: 0,
            GoldPlatters: 0,
            DiamondPlatters: 0,
            SurvivalRevives: 0,
            EnergyRefills: 0
        );
        InsertPlayerData(player);
    }

    private void InsertCharacterData(CharacterData DBCharacterData)
    {
        connection.Insert(DBCharacterData);
    }

    private void InsertLevelData(LevelData DBLevelData)
    {
        connection.Insert(DBLevelData);
    }

    private void InsertSpawnPointData(SpawnPointData DBSpawnPointData)
    {
        connection.Insert(DBSpawnPointData);
    }

    private void InsertSensitiveCharacterData(SensitiveCharacterData character)
    {
        connection.Insert(character);
    }

    private void InsertPlayerData(PlayerData player)
    {
        connection.Insert(player);
    }

    private void CheckGameVersion()
    {
        string gameVersion = connection.Table<GameVersion>().FirstOrDefault()._GameVersion;

        //if update was successful, delete all game version entries, then set new gameversion entry to the current version
        if (CURRENT_VERSION != gameVersion)
        {
            UpdateDBOnVersionChange();
            connection.Execute("DELETE FROM GameVersion");
            GameVersion newGameVersion = new GameVersion(
                ID: 0,
                _GameVersion: CURRENT_VERSION
            );
            connection.Insert(newGameVersion);
        }
    }

    private void UpdateDBOnVersionChange()
    {
        //compare json of text doc to current database? This is where Add and Update can come in
        //but cant implement until the text doc and corresponding script are done
    }

    //All initializations of characters -- should only be run once upon loading game for first time

    //going to have to go through and esnure all data is correct(eg aged cheese is not)
    private void initBabyCarrot()
    {
        CharacterData babyCarrot = new CharacterData(
            DBCharacterId: 1,
            Name: "Baby Carrot",
            PrefabName: "Baby Carrot Prefab",
            isTower: false,
            Description: "dumb ugly first level",
            Type: "vegetable",
            Quality: "rotten",
            Attack: 100,
            Defense: 100,
            Health: 100,
            Speed: 50,
            CookTime: .5f,
            Cooldown: 1,
            Cost: 100,
            AttackRange: 20,
            AttackRate: 1,
            AreaAtk: false,
            DamageRange: 15,
            isEnemy: false,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            AbilityId: 0,
            AbilityDescription: "baby carrot ability hehe",
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(babyCarrot);

        SensitiveCharacterData _babyCarrot = new SensitiveCharacterData(
            DBCharacterId: 1,
            Level: 1,
            IsUnlocked: true,
            DeadInSurvival: false
            );
        InsertSensitiveCharacterData(_babyCarrot);
    }

    private void initAgedCheese()
    {
        CharacterData agedCheese = new CharacterData(
            DBCharacterId: 2,
            Name: "Aged Cheese",
            PrefabName: "Aged Cheese Prefab",
            Type: "dairy",
            Quality: "rotten",
            AbilityId: 0,
            Attack: 30,
            Defense: 150,
            Health: 100,
            Speed: 50,
            CookTime: 3,
            Cost: 100,
            Cooldown: 1,
            AttackRange: 30,
            AttackRate: 3,
            AreaAtk: true,
            DamageRange: 15,
            isEnemy: false,
            isTower: false,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            AbilityDescription: "yes",
            Description: "no",
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(agedCheese);

        SensitiveCharacterData _agedCheese = new SensitiveCharacterData(
            DBCharacterId: 2,
            Level: 1,
            IsUnlocked: true,
            DeadInSurvival: false
            );
        InsertSensitiveCharacterData(_agedCheese);
    }

    private void initAvocadolet()
    {
        CharacterData avocadolet = new CharacterData(
            DBCharacterId: 3,
            Name: "Avocadolet",
            PrefabName: "Avocadolet Prefab",
            Type: "vegetable",
            Quality: "rotten",
            AbilityId: 0,
            Attack: 40,
            Defense: 80,
            Health: 200,
            Speed: 6,
            CookTime: 2,
            Cost: 100,
            Cooldown: 1,
            AttackRange: 20,
            AttackRate: 2,
            AreaAtk: true,
            DamageRange: 10,
            isEnemy: false,
            isTower: false,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            AbilityDescription: "yes",
            Description: "no",
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(avocadolet);

        SensitiveCharacterData _avocadolet = new SensitiveCharacterData(
            DBCharacterId: 3,
            Level: 1,
            IsUnlocked: true,
            DeadInSurvival: false
            );
        InsertSensitiveCharacterData(_avocadolet);
    }

    private void initBabyMussels()
    {
        CharacterData babyMussels = new CharacterData(
            DBCharacterId: 4,
            Name: "Baby Mussels",
            PrefabName: "Baby Mussels Prefab",
            Type: "seafood",
            Quality: "rotten",
            AbilityId: 0,
            Attack: 150,
            Defense: 80,
            Health: 150,
            Speed: 1,
            CookTime: 1,
            Cost: 100,
            Cooldown: 1,
            AttackRange: 10,
            AttackRate: 1,
            AreaAtk: false,
            DamageRange: 5,
            isEnemy: false,
            isTower: false,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            AbilityDescription: "yes",
            Description: "no",
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(babyMussels);

        SensitiveCharacterData _babyMussels = new SensitiveCharacterData(
            DBCharacterId: 4,
            Level: 1,
            IsUnlocked: true,
            DeadInSurvival: false
            );
        InsertSensitiveCharacterData(_babyMussels);
    }

    private void initBananaPhone()
    {
        CharacterData bananaPhone = new CharacterData(
            DBCharacterId: 5,
            Name: "Banana Phone",
            PrefabName: "Banana Phone Prefab",
            Type: "fruit",
            Quality: "rotten",
            AbilityId: 0,
            Attack: 40,
            Defense: 100,
            Health: 60,
            Speed: 3,
            CookTime: 6,
            Cost: 100,
            Cooldown: 1,
            AttackRange: 25,
            AttackRate: 1,
            AreaAtk: false,
            DamageRange: 5,
            isEnemy: false,
            isTower: false,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            AbilityDescription: "yes",
            Description: "no",
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(bananaPhone);

        SensitiveCharacterData _bananaPhone = new SensitiveCharacterData(
            DBCharacterId: 5,
            Level: 1,
            IsUnlocked: true,
            DeadInSurvival: false
            );
        InsertSensitiveCharacterData(_bananaPhone);
    }

    private void initBanana()
    {
        CharacterData banana = new CharacterData(
            DBCharacterId: 0,
            Name: "Banana",
            PrefabName: "Banana Prefab",
            Type: "fruit",
            Quality: "rotten",
            AbilityId: 0,
            Attack: 100,
            Defense: 100,
            Health: 100,
            Speed: 5,
            CookTime: 4,
            Cost: 100,
            Cooldown: 1,
            AttackRange: 60,
            AttackRate: 5,
            AreaAtk: false,
            DamageRange: 30,
            isEnemy: false,
            isTower: false,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            AbilityDescription: "yes",
            Description: "no",
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(banana);

        SensitiveCharacterData _banana = new SensitiveCharacterData(
            DBCharacterId: 0,
            Level: 1,
            IsUnlocked: true,
            DeadInSurvival: false
            );
        InsertSensitiveCharacterData(_banana);
    }

    private void initFriendlyTower()
    {
        CharacterData friendlyTower = new CharacterData(
            DBCharacterId: 6,
            Name: "Friendly Stove",
            PrefabName: "Friendly Stove Prefab",
            Type: "fruit",
            Quality: "rotten",
            AbilityId: 0,
            Attack: 0,
            Defense: 0,
            Health: 300,
            Speed: 0,
            CookTime: 0,
            Cost: 0,
            Cooldown: 1,
            AttackRange: 0,
            AttackRate: 100,
            AreaAtk: false,
            DamageRange: 0,
            isEnemy: false,
            isTower: true,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            AbilityDescription: "yes",
            Description: "no",
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(friendlyTower);

        SensitiveCharacterData _friendlyTower = new SensitiveCharacterData(
            DBCharacterId: 6,
            Level: 1,
            IsUnlocked: true,
            DeadInSurvival: false
            );
        InsertSensitiveCharacterData(_friendlyTower);
    }


    //All initializations of enemies 
    private void initSpoon()
    {
        CharacterData spoon = new CharacterData(
            DBCharacterId: 7,
            Name: "Spoon",
            PrefabName: "Spoon Prefab",
            Type: "enemy",
            Quality: "rusty",
            AbilityId: -1,
            Attack: 100,
            Defense: 100,
            Health: 300,
            Speed: 30,
            CookTime: 3,
            Cost: 0,
            Cooldown: 1,
            AttackRange: 30,
            AttackRate: 1,
            AreaAtk: false,
            DamageRange: 15,
            isEnemy: true,
            isTower: false,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            AbilityDescription: "yes",
            Description: "no",
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(spoon);
    }

    private void initFork()
    {
        CharacterData fork = new CharacterData(
            DBCharacterId: 8,
            Name: "Fork",
            PrefabName: "Fork Prefab",
            Type: "enemy",
            Quality: "rusty",
            AbilityId: -1,
            Attack: 200,
            Defense: 50,
            Health: 200,
            Speed: 40,
            CookTime: 1,
            Cooldown: 1,
            Cost: 0,
            AttackRange: 80,
            AttackRate: 1,
            AreaAtk: false,
            DamageRange: 30,
            isEnemy: true,
            isTower: false,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            AbilityDescription: "yes",
            Description: "no",
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(fork);
    }

    private void initEnemyTower()
    {
        CharacterData enemyTower = new CharacterData(
            DBCharacterId: 9,
            Name: "Enemy Stove",
            PrefabName: "TEMP Enemy Stove Prefab",
            Type: "enemy",
            Quality: "rusty",
            AbilityId: -1,
            Attack: 0,
            Defense: 0,
            Health: 300,
            Speed: 0,
            CookTime: 0,
            Cost: 0,
            Cooldown: 1,
            AttackRange: 0,
            AttackRate: 100,
            AreaAtk: false,
            DamageRange: 0,
            isEnemy: true,
            isTower: true,
            AttackAdditivePerLevel: 0,
            DefenseAdditivePerLevel: 0,
            HealthAdditivePerLevel: 0,
            FirstEvolutionCharacterId: 0,
            SecondEvolutionCharacterId: 0,
            ThirdEvolutionCharacterId: 0,
            EvolutionNumber: 0,
            AbilityDescription: "yes",
            Description: "no",
            NumDuplicatesObtained: 0,
            NumDuplicatesToStarUp: 5,
            Stars: 1,
            FlyingHeight: 0
        );
        InsertCharacterData(enemyTower);
    }
}