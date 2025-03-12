using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

//Relationship between lineup and deployment:
//1. grab lineup from playerprefs and store it in lineupNames
//2. find the corresponding PREFAB to each lineupName and store all of them in lineupCharacters: dict<name, prefab>
//3. when friendly is deployed, instantiate corresponding prefab from lineupCharacters and add it to deployedFriendlies

public class CombatController : MonoBehaviour
{
    private List<SpawnPointData> spawnPoints = new List<SpawnPointData>();
    private List<SpawnPointData> spawnPointsToDelete = new List<SpawnPointData>();
    private List<_EntityController> _deployedEnemies = new List<_EntityController>();
    private List<_EntityController> _deployedFriendlies = new List<_EntityController>();
    private List<string> lineupNames = new List<string> { };
    private Dictionary<string, GameObject> lineupCharacters = new Dictionary<string, GameObject>();
    private Dictionary<int, GameObject> lineupEnemies = new Dictionary<int, GameObject>();
    private GameObject friendlyTowerInstance;
    private GameObject enemyTowerInstance;
    private CharacterSpawner spawner;
    private Camera mainCamera;
    private ButtonGenerator btnGenerator; // Reference to the ButtonLoader script
    private float totalTimeElapsed = 0f;
    private int totalFriendliesSpawned = 0;
    private int startingTowerHealth = 0; //grab these from the level data later and delete any references to these
    private int currTowerHealth = 0;
    private SceneCleanup sceneCleanup;
    private GameManager gameManager;
    private int currentLevelID;
    private float currentEnemyMultiplier;
    private Queue<float> friendlySpawnQueue;
    private Queue<float> enemySpawnQueue;
    private const int towerLayer = 1;

    public List<_EntityController> deployedEnemies
    {
        get { return _deployedEnemies; }
        set { _deployedEnemies = value; }
    }

    public List<_EntityController> deployedFriendlies
    {
        get { return _deployedFriendlies; }
        set { _deployedFriendlies = value; }
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentLevelID = gameManager.levelId;
        spawnPoints = LocalDatabaseAccessLayer.LoadSpawnPointData(currentLevelID);
        currentEnemyMultiplier = LocalDatabaseAccessLayer.GetEnemyMultiplier(currentLevelID);
        spawner = GetComponent<CharacterSpawner>();
        sceneCleanup = GetComponent<SceneCleanup>();
        mainCamera = Camera.main;
        CaptureLineupCharacters();
        InitLineupCharacters();
        InitLineupEnemies();
        InitTowers();
    }

    void Update()
    {
        CheckWinCondition(); //checks if someone has won every single frame
        UpdateElapsedTime();
        CheckEnemySpawns();
        InitateUpdateSequence();
    }

    //iterate over the lineupdata in playerprefs, and instantiate a dict with the controllers
    void CaptureLineupCharacters()
    {
        //string charjson = PlayerPrefs.GetString("CharacterLineupData");
        //lineupNames = JsonUtility.FromJson<List<string>>(charjson);
    }

    void InitLineupCharacters()
    {
        foreach (string name in lineupNames)
        {
            GameObject charPrefab = Utilities.LoadAsset<GameObject>("Prefabs/Character Prefabs/" + name);
            lineupCharacters.Add(name, charPrefab);
        }
    }

    void InitLineupEnemies()
    {
        //queries the enmy id of those spawn point, loads prefab into level, puts prefab into lineupenms
        foreach (var spawnPoint in spawnPoints)
        {
            CharacterData enemy = LocalDatabaseAccessLayer.GetEnemyFromSpawnPoint(spawnPoint.DBCharacterId);
            if (!lineupEnemies.ContainsKey(enemy.DBCharacterId))
            {
                GameObject enemyPrefab = Utilities.LoadAsset<GameObject>("Prefabs/Character Prefabs/" + enemy.PrefabName);
                lineupEnemies.Add(enemy.DBCharacterId, enemyPrefab);
            }
        }
    }

    void InitTowers()
    {
        int enemyTowerHealth = LocalDatabaseAccessLayer.GetEnemyTowerHealth(currentLevelID);
        int friendlyTowerHealth = LocalDatabaseAccessLayer.GetFriendlyTowerHealth();
        //^^instantiates towers with placeholder health first, then dynamic health implemented
        GameObject friendlyTower = Utilities.LoadAsset<GameObject>("Prefabs/Character Prefabs/Friendly Stove Prefab");
        GameObject enemyTower = Utilities.LoadAsset<GameObject>("Prefabs/Character Prefabs/Enemy Cabinet Prefab");

        Vector3 deployLocation = mainCamera.ViewportToWorldPoint(new Vector3(0.1f, 0.36f, mainCamera.nearClipPlane));
        friendlyTowerInstance = Instantiate(friendlyTower, deployLocation, Quaternion.identity);
        friendlyTowerInstance.name = "Friendly Tower";
        friendlyTowerInstance.layer = towerLayer;
        deployedFriendlies.Add(friendlyTowerInstance.GetComponent<_TowerController>());
        friendlyTowerInstance.GetComponent<_TowerController>().characterData.Health = friendlyTowerHealth;

        deployLocation = mainCamera.ViewportToWorldPoint(new Vector3(0.9f, 0.36f, mainCamera.nearClipPlane));
        enemyTowerInstance = Instantiate(enemyTower, deployLocation, Quaternion.identity);
        enemyTowerInstance.name = "Enemy Tower";
        enemyTowerInstance.layer = towerLayer;
        deployedEnemies.Add(enemyTowerInstance.GetComponent<_TowerController>());
        enemyTowerInstance.GetComponent<_TowerController>().characterData.Health = enemyTowerHealth;
    }

    void UpdateElapsedTime()
    {
        totalTimeElapsed += Time.deltaTime;
    }

    void CheckEnemySpawns()
    {
        //process the spawnPoints
        foreach (SpawnPointData spawnPoint in spawnPoints)
        {
            if (totalTimeElapsed >= spawnPoint.StartSpawningInXSecs
                || startingTowerHealth - currTowerHealth >= spawnPoint.SpawnAfterXTowerDamage
                || totalFriendliesSpawned >= spawnPoint.SpawnAfterXFriendlies
                )
            {
                spawnPointsToDelete.Add(spawnPoint);
                spawner.StartSpawnEnemyCoroutine(spawnPoint, lineupEnemies, currentEnemyMultiplier);
            }
        }

        //remove the used spawnpoint
        foreach (SpawnPointData spawnpoint in spawnPointsToDelete)
        {
            spawnPoints.Remove(spawnpoint);
        }
    }

    void InitateUpdateSequence()
    {
        Utilities.SortByXPosDescending(deployedFriendlies);
        Utilities.SortByXPosAscending(deployedEnemies);

        //sort deployed friendlies and deployed enemies by transfrom.position

        //big question here is - when does a character change state? since all logic of what the character is doing is handled by the character controller and the state manager,
        //the combat controller should only be used for updating the charcaters state
        foreach (_EntityController entity in deployedFriendlies)
        {
            if (entity is _CharacterController friendly)
            {
                bool charWithinRange = Utilities.IsACharacterWithinRange(deployedEnemies, friendly);
                if (friendly.characterData.isTower)
                {
                    continue;
                }
                //if a character is walking - state change is triggered upon an enemy coming into range
                else if (friendly.CheckAnimatorStateName("Walk"))
                {
                    if (charWithinRange) // if an enemy comes within range while walking
                    {
                        friendly.UpdateAnimatorParameters("Fight");
                    }
                }

                else if (friendly.CheckAnimatorStateName("Idle"))
                {
                    if (!charWithinRange) // if no enemy in range while idling
                    {
                        friendly.UpdateAnimatorParameters("Walk");
                    }
                }
            }
        }

        //enemies attack sequence
        foreach (_EntityController entity in deployedEnemies)
        {
            if (entity is _CharacterController enemy)
            {
                bool charWithinRange = Utilities.IsACharacterWithinRange(deployedFriendlies, enemy);
                if (enemy.characterData.isTower)
                {
                    continue;
                }
                else if (enemy.CheckAnimatorStateName("Walk"))
                {
                    if (charWithinRange) // if an enemy comes within range while walking
                    {
                        enemy.UpdateAnimatorParameters("Fight");
                    }
                }

                else if (enemy.CheckAnimatorStateName("Idle"))
                {
                    if (!charWithinRange) // if no enemy in range while idling
                    {
                        enemy.UpdateAnimatorParameters("Walk");
                    }
                }
            }
        }
    }

    public void RefreshTargetsForCharacter(_EntityController character)
    {
        if(character.characterData.isEnemy)
        {
            Utilities.AddCharactersToTargets(deployedFriendlies, character);
        }
        else
        {
            Utilities.AddCharactersToTargets(deployedEnemies, character);
        }
    }

    public void UpdateDeployedList(_EntityController character, bool isAdding)
    {
        var list = character.characterData.isEnemy ? deployedEnemies : deployedFriendlies;

        if (isAdding)
        {
            list.Add(character);
        }
        else
        {
            list.Remove(character);
        }
    }

    public void DestroyPrefab(_EntityController character)
    {
        Destroy(character.PREFAB);
    }

    public void IncrementNumFriendliesSpawned()
    {
        totalFriendliesSpawned += 1;
    }

    RewardData GrabRewardDataForCurrentLevel(int currentLevelID)
    {
        return LocalDatabaseAccessLayer.GetRewardData(currentLevelID);
    }

    void CheckWinCondition()
    {
        if (!deployedEnemies.Contains(enemyTowerInstance.GetComponent<_TowerController>()))
        {
            Debug.Log("you win!");
            //PauseScene();
            LocalDatabaseAccessLayer.UpdateNumTimesBeaten(currentLevelID, 1);
            //ModalController.DisplayRewardModalAfterWin(GrabRewardDataForCurrentLevel);
            sceneCleanup.CleanUpScene();
        }
        else if (!deployedFriendlies.Contains(friendlyTowerInstance.GetComponent<_TowerController>()))
        {
            Debug.Log("you lose!");
            //PauseScene();
            //ModalController.DisplayModalAfterLoss();
            sceneCleanup.CleanUpScene();
        }
    }
}