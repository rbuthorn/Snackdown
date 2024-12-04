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
    private List<_CharacterController> _deployedEnemies = new List<_CharacterController>();
    private List<_CharacterController> _deployedFriendlies = new List<_CharacterController>();
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
    private SceneCleanup sceneCleanup { get; set; }

    public List<_CharacterController> deployedEnemies
    {
        get { return _deployedEnemies; }
        set { _deployedEnemies = value; }
    }

    public List<_CharacterController> deployedFriendlies
    {
        get { return _deployedFriendlies; }
        set { _deployedFriendlies = value; }
    }

    void Start()
    {
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
        int LevelID = 0; //hardcoded solution for now
        //queries the spawnpoints of the level
        spawnPoints = LocalDatabaseAccessLayer.GetSpawnPoints(LevelID);
        //then queries the enmy id of those spawn point, loads prefab into level, puts prefab into lineupenms
        foreach (var spawnPoint in spawnPoints)
        {
            CharacterData enemy = LocalDatabaseAccessLayer.GetEnemyFromSpawnPoint(spawnPoint.DBCharacterId);
            GameObject enemyPrefab = Utilities.LoadAsset<GameObject>("Prefabs/Character Prefabs/" + enemy.PrefabName);
            lineupEnemies.Add(enemy.DBCharacterId, enemyPrefab);
        }
    }

    void InitTowers()
    {
        GameObject friendlyTower = Utilities.LoadAsset<GameObject>("Prefabs/Character Prefabs/Friendly Stove Prefab");
        GameObject enemyTower = Utilities.LoadAsset<GameObject>("Prefabs/Character Prefabs/TEMP Enemy Stove Prefab");

        Vector3 deployLocation = mainCamera.ViewportToWorldPoint(new Vector3(0.1f, 0.15f, mainCamera.nearClipPlane));
        friendlyTowerInstance = Instantiate(friendlyTower, deployLocation, Quaternion.identity);
        deployedFriendlies.Add(friendlyTowerInstance.GetComponent<_CharacterController>());

        deployLocation = mainCamera.ViewportToWorldPoint(new Vector3(0.9f, 0.15f, mainCamera.nearClipPlane));
        enemyTowerInstance = Instantiate(enemyTower, deployLocation, Quaternion.identity);
        deployedEnemies.Add(enemyTowerInstance.GetComponent<_CharacterController>());
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
                spawner.StartSpawnEnemyCoroutine(spawnPoint, lineupEnemies);
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
        foreach (_CharacterController friendly in deployedFriendlies)
        {
            bool charWithinRange = Utilities.IsACharacterWithinRange(deployedEnemies, friendly);
            if (friendly.characterData.isTower)
            {
                continue;
            }
            //if a character is walking - state change is triggered upon an enemy coming into range
            else if (friendly.CheckAnimatorStateName("Walk"))
            {
                if(charWithinRange) // if an enemy comes within range while walking
                {
                    friendly.UpdateAnimatorParameters("Fight");
                }
            }

            else if(friendly.CheckAnimatorStateName("Idle"))
            {
                if (!charWithinRange) // if no enemy in range while idling
                {
                    friendly.UpdateAnimatorParameters("Walk");
                }
            }
        }

        //enemies attack sequence
        foreach (_CharacterController enemy in deployedEnemies)
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

    public void RefreshTargetsForCharacter(_CharacterController character)
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

    public void UpdateDeployedList(_CharacterController character, bool isAdding)
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

    public void DestroyPrefab(_CharacterController character)
    {
        Destroy(character.PREFAB);
    }

    public void IncrementNumFriendliesSpawned()
    {
        totalFriendliesSpawned += 1;
    }

    void CheckWinCondition()
    {
        if (!deployedEnemies.Contains(enemyTowerInstance.GetComponent<_CharacterController>()))
        {
            Debug.Log("you win!");
            sceneCleanup.CleanUpScene();
        }
        else if (!deployedFriendlies.Contains(friendlyTowerInstance.GetComponent<_CharacterController>()))
        {
            Debug.Log("you lost!");
            sceneCleanup.CleanUpScene();
        }
    }
}