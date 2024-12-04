using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    private Camera mainCamera;
    private CombatController cc;
    private MannaController mannaController;

    void Start()
    {
        mannaController = GameObject.Find("MannaController").GetComponent<MannaController>();
        cc = GameObject.Find("combatController").GetComponent<CombatController>();
        mainCamera = Camera.main;
    }

    //ENEMIES
    public void StartSpawnEnemyCoroutine(SpawnPointData spd, Dictionary<int, GameObject> lineupEnemies)
    {
        StartCoroutine(SpawnEnemyCoroutine(spd, lineupEnemies));
    }

    IEnumerator SpawnEnemyCoroutine(SpawnPointData spd, Dictionary<int, GameObject> lineupEnemies)
    {
        yield return new WaitForSeconds(spd.SpawnAfterXSecs);
        for (int i = 0; i < spd.NumSpawning; i++)
        {
            DeployEnemy(lineupEnemies[spd.DBCharacterId]);
            yield return new WaitForSeconds(spd.TimeBetweenSpawns);
        }
    }

    void DeployEnemy(GameObject enemy)
    {
        Vector3 cameraCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.9f, 0.2f, mainCamera.nearClipPlane));
        GameObject enemyInstance = Instantiate(enemy, cameraCenter, Quaternion.identity);
        _CharacterController enemyController = enemyInstance.GetComponent<_CharacterController>();
        enemyController.PREFAB = enemyInstance;
        cc.UpdateDeployedList(enemyController, true);
    }

    //FRIENDLIES
    public int DeployFriendly(GameObject prefab, (int cost, float cookTime) tuple)
    {
        var (cost, cookTime) = tuple;

        if (mannaController.CheckIfEnoughManna(cost))
        {
            StartSpawnFriendlyCoroutine(prefab, cookTime, cost);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void StartSpawnFriendlyCoroutine(GameObject friendly, float cookTime, int cost)
    {
        StartCoroutine(SpawnFriendlyCoroutine(friendly, cookTime, cost));
    }

    IEnumerator SpawnFriendlyCoroutine(GameObject prefab, float cookTime, int cost)
    {
        mannaController.UpdateCurrentManna(-1*cost);
        yield return new WaitForSeconds(cookTime);
        Vector3 deployLocation = mainCamera.ViewportToWorldPoint(new Vector3(0.1f, 0.2f, mainCamera.nearClipPlane));
        GameObject characterInstance = Instantiate(prefab, deployLocation, Quaternion.identity);
        _CharacterController friendlyController = characterInstance.GetComponent<_CharacterController>();
        friendlyController.PREFAB = characterInstance;
        cc.UpdateDeployedList(friendlyController, true);
        cc.IncrementNumFriendliesSpawned();
    }
}
