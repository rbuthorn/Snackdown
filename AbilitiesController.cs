using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AbilitiesController : MonoBehaviour
{
    public delegate void _SimpleAbilitiesFuncs(_CharacterController character, int evolutionNumber);
    private Dictionary<int, _SimpleAbilitiesFuncs> abilitiesDict = new Dictionary<int, _SimpleAbilitiesFuncs>();
    private CombatController cc;

    void Start(){
        cc = GameObject.Find("combatController").GetComponent<CombatController>();
        //match character abilityId to number in the dictionary
        abilitiesDict.Add(0, DamageToFrontmostEnemy);
    }

    //DAMAGE ABILITIES
    //tomato
    void DamageToFrontmostEnemy(_CharacterController character, int evolutionNumber){
        List<_EntityController> enemies = new List<_EntityController>(cc.deployedEnemies);
        Utilities.SortByXPosDescending(enemies);
        _EntityController target = enemies[0];
        //if list of targets, add a for loop here
        character.DealDamageToTarget(character.characterData.Attack * (evolutionNumber + .5f), target, true);
    }

    //carrot
    void DamageToBackmostEnemy(_CharacterController character, int evolutionNumber)
    {
        List<_EntityController> enemies = new List<_EntityController>(cc.deployedEnemies);
        Utilities.SortByXPosDescending(enemies);
        _EntityController target = enemies[-1];
        //if list of targets, add a for loop here
        character.DealDamageToTarget(character.characterData.Attack * (1.5f + evolutionNumber*.5f), target, true);
    }

    //pea
    void DamageToRangeOfEnemies(_CharacterController character, int evolutionNumber)
    {
        List<_EntityController> enemies = new List<_EntityController>(cc.deployedEnemies);
        Utilities.SortByXPosDescending(enemies);
        var char_pos = character.transform.position.x;
        float range_begin = 30 - (evolutionNumber * 5);
        float range_end = 50 + (evolutionNumber * 5);
        IEnumerable<_EntityController> targets = enemies.Where(enemy => enemy.transform.position.x - char_pos >= range_begin && enemy.transform.position.x - char_pos <= range_end);
        foreach (_EntityController target in targets)
        {
            character.DealDamageToTarget(character.characterData.Attack * (1.5f + evolutionNumber * .5f), target, true);
        }
    }

    //mussels
    void AllAlliesDamageToFrontmostEnemy(_CharacterController character, int evolutionNumber)
    {
        List<_EntityController> friendlies = new List<_EntityController>(cc.deployedFriendlies);
        List<_EntityController> enemies = new List<_EntityController>(cc.deployedEnemies);
        Utilities.SortByXPosDescending(enemies);
        _EntityController target = enemies[0];
        float totalDamage = friendlies.Aggregate(0f, (accumulator, friendly) => accumulator + friendly.characterData.Attack);
        character.DealDamageToTarget(totalDamage * (1 + (.5f*evolutionNumber)), target, true);
    }

    //brownie
    void HitAllEnemiesOneHundredHitFriendliesFifty(_CharacterController character, int evolutionNumber)
    {
        List<_EntityController> friendlies = new List<_EntityController>(cc.deployedFriendlies);
        List<_EntityController> enemies = new List<_EntityController>(cc.deployedEnemies);
        float damageToEnemy = character.characterData.Attack * (1f + (evolutionNumber * .5f));
        float damageToFriendly = character.characterData.Attack * (.5f + (evolutionNumber * .1f));
        foreach (_EntityController enemy in enemies)
        {
            character.DealDamageToTarget(damageToEnemy, enemy, true);
        }
        foreach (_EntityController friendly in friendlies)
        {
            character.DealDamageToTarget(damageToFriendly, friendly, true);
        }
    }

    //gumball
    void HitAllEnemies(_CharacterController character, int evolutionNumber)
    {
        List<_EntityController> enemies = new List<_EntityController>(cc.deployedEnemies);
        float damageToEnemy = character.characterData.Attack * (1f + (evolutionNumber * .5f));
        foreach (_EntityController enemy in enemies)
        {
            character.DealDamageToTarget(damageToEnemy, enemy, true);
        }
    }

    //HEALING ABILITIES
    //Kiwi

}
