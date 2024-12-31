using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public void StartSpawnEnemyCoroutine(SpawnPointData spd, Dictionary<int, GameObject> lineupEnemies, float multiplier)
    {
        StartCoroutine(SpawnEnemyCoroutine(spd, lineupEnemies, multiplier));
    }

    IEnumerator SpawnEnemyCoroutine(SpawnPointData spd, Dictionary<int, GameObject> lineupEnemies, float multiplier)
    {
        yield return new WaitForSeconds(spd.SpawnAfterXSecs);
        for (int i = 0; i < spd.NumSpawning; i++)
        {
            StartCoroutine(DeployEnemyCoroutine(lineupEnemies[spd.DBCharacterId], multiplier));
            yield return new WaitForSeconds(spd.TimeBetweenSpawns);
        }
    }

    IEnumerator DeployEnemyCoroutine(GameObject enemy, float multiplier)
    {
        _TowerController tower = GameObject.Find("Enemy Tower").GetComponent<_TowerController>();
        if (!tower.CheckAnimatorStateName("Deploy"))
        {
            tower.UpdateAnimatorParameters("Deploy");
        }
        yield return new WaitForSeconds(.35f);
        Vector3 cameraCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.9f, RandomYValueGenerator(), mainCamera.nearClipPlane));
        GameObject enemyInstance = Instantiate(enemy, cameraCenter, Quaternion.identity);
        _CharacterController enemyController = enemyInstance.GetComponent<_CharacterController>();
        enemyController.PREFAB = enemyInstance;
        ApplyMultiplier(enemyController, multiplier);
        cc.UpdateDeployedList(enemyController, true);
    }

    void ApplyMultiplier(_CharacterController character, float multiplier)
    {
        character.characterData.Attack *= multiplier;
        character.characterData.Defense *= multiplier;
        character.characterData.Health *= multiplier;
    }

    //FRIENDLIES
    public int DeployFriendly(Button button, GameObject prefab, (int cost, float cookTime, float cooldown) tuple)
    {
        var (cost, cookTime, cooldown) = tuple;

        if (mannaController.CheckIfEnoughManna(cost))
        {
            StartCoroutine(StartDeployCooldownCoroutine(button, cooldown));
            StartSpawnFriendlyCoroutine(prefab, cookTime, cost);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    IEnumerator StartDeployCooldownCoroutine(Button button, float cooldown)
    {
        button.interactable = false;
        yield return new WaitForSeconds(cooldown);
        button.interactable = true;
    }

    void StartSpawnFriendlyCoroutine(GameObject friendly, float cookTime, int cost)
    {
        _TowerController tower = GameObject.Find("Friendly Tower").GetComponent<_TowerController>();
        tower.UpdateAnimatorParameters("Cook");
        StartCoroutine(SpawnFriendlyCoroutine(friendly, cookTime, cost, tower));
    }

    IEnumerator SpawnFriendlyCoroutine(GameObject prefab, float cookTime, int cost, _TowerController tower)
    {
        mannaController.UpdateCurrentManna(-1*cost);
        yield return new WaitForSeconds(cookTime);
        tower.UpdateAnimatorParameters("Deploy");
        Vector3 deployLocation = mainCamera.ViewportToWorldPoint(new Vector3(0.1f, RandomYValueGenerator(), mainCamera.nearClipPlane));
        GameObject characterInstance = Instantiate(prefab, deployLocation, Quaternion.identity);
        //add a function to move the character with a lfying height > 0 to its flying height, after being spanwed from the default location
        _CharacterController friendlyController = characterInstance.GetComponent<_CharacterController>();
        friendlyController.PREFAB = characterInstance;
        cc.UpdateDeployedList(friendlyController, true);
        cc.IncrementNumFriendliesSpawned();
    }

    float RandomYValueGenerator()
    {
        float randomValue = Random.Range(0.22f, 0.29f);
        return randomValue;
    }
}
