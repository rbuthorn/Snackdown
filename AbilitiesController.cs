using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //tomato
    void DamageToFrontmostEnemy(_CharacterController character, int evolutionNumber){
        List<_EntityController> enemies = new List<_EntityController>(cc.deployedEnemies);
        Utilities.SortByXPosDescending(enemies);
        _EntityController target = enemies[0];
        //if list of targets, add a for loop here
        character.DealDamageToTarget(character.characterData.Attack * (evolutionNumber + .5f), target, true);
    }
}
