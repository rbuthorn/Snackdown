using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
        List<_CharacterController> enemies = cc.deployedEnemies.OfType<_CharacterController>().ToList();
        Utilities.SortByXPosDescending(enemies);
        _CharacterController target = enemies[0];
        character.DealDamageToTarget(character.characterData.Attack * (evolutionNumber + .5f), target, true);
    }

    //carrot
    void DamageToBackmostEnemy(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> enemies = cc.deployedEnemies.OfType<_CharacterController>().ToList();
        Utilities.SortByXPosDescending(enemies);
        _CharacterController target = enemies[-1];
        character.DealDamageToTarget(character.characterData.Attack * (1.5f + evolutionNumber*.5f), target, true);
    }

    //pea
    void DamageToRangeOfEnemies(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> enemies = cc.deployedEnemies.OfType<_CharacterController>().ToList();
        List<_CharacterController> friendlies = cc.deployedFriendlies.OfType<_CharacterController>().ToList();
        Utilities.SortByXPosDescending(enemies);
        float rangeBegin = 30 - (evolutionNumber * 5);
        float rangeEnd = 50 + (evolutionNumber * 5);
        _CharacterController frontmostCharacter = Utilities.FindFrontmostCharacter(character, friendlies);
        var startingIndexForRange = frontmostCharacter.transform.position.x;
        IEnumerable<_CharacterController> targets = enemies.Where(enemy => enemy.transform.position.x - startingIndexForRange >= rangeBegin && enemy.transform.position.x - startingIndexForRange <= rangeEnd);
        foreach (_CharacterController target in targets)
        {
            character.DealDamageToTarget(character.characterData.Attack * (1.5f + evolutionNumber * .5f), target, true);
        }
    }

    //mussels
    void AllAlliesDamageToFrontmostEnemy(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> friendlies = cc.deployedFriendlies.OfType<_CharacterController>().ToList();
        List<_CharacterController> enemies = cc.deployedEnemies.OfType<_CharacterController>().ToList();
        Utilities.SortByXPosDescending(enemies);
        _CharacterController target = enemies[0];
        float totalDamage = friendlies.Aggregate(0f, (accumulator, friendly) => accumulator + friendly.characterData.Attack);
        character.DealDamageToTarget(totalDamage * (1 + (.5f*evolutionNumber)), target, true);
    }

    //brownie
    void HitAllEnemiesOneHundredHitFriendliesFifty(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> friendlies = cc.deployedFriendlies.OfType<_CharacterController>().ToList();
        List<_CharacterController> enemies = cc.deployedEnemies.OfType<_CharacterController>().ToList();
        float damageToEnemy = character.characterData.Attack * (1f + (evolutionNumber * .5f));
        float damageToFriendly = character.characterData.Attack * (.5f + (evolutionNumber * .1f));
        foreach (_CharacterController enemy in enemies)
        {
            character.DealDamageToTarget(damageToEnemy, enemy, true);
        }
        foreach (_CharacterController friendly in friendlies)
        {
            character.DealDamageToTarget(damageToFriendly, friendly, true);
        }
    }

    //gumball
    void HitAllEnemies(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> enemies = cc.deployedEnemies.OfType<_CharacterController>().ToList();
        float damageToEnemy = character.characterData.Attack * (1f + (evolutionNumber * .5f));
        foreach (_CharacterController enemy in enemies)
        {
            character.DealDamageToTarget(damageToEnemy, enemy, true);
        }
    }

    //HEALING ABILITIES
    //Kiwi

    //jellyfish
    void RandomFriendliesGainLifesteal(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> friendlies = cc.deployedFriendlies.OfType<_CharacterController>().ToList();
        System.Random random = new System.Random();
        float percentLifesteal = .2f + (evolutionNumber * .15f);
        List<_CharacterController> selectedFriendlies = friendlies.OrderBy(friendly => random.Next()).Take(3).ToList();
        foreach(_CharacterController friendly in selectedFriendlies)
        {
            Lifesteal lifesteal = new Lifesteal(float.MaxValue, percentLifesteal);
            friendly.AddStatusEffect(lifesteal);
        }
    }

    //meatball
    void HealAllAlliesPerNumSameCharacter(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> friendlies = cc.deployedFriendlies.OfType<_CharacterController>().ToList();
        float percentHeal = .03f + (evolutionNumber * .02f);
        int numMatchingCharacters = friendlies.Where(friendly => friendly.characterData.Name == character.characterData.Name).Count();
        foreach (_CharacterController friendly in friendlies)
        {
            friendly.Heal(friendly.maxHealth * percentHeal);
        }
    }
    
    //avocado
    void BuffAttackAndDefenseWithinRange(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> friendlies = cc.deployedFriendlies.OfType<_CharacterController>().ToList();
        _CharacterController frontmostCharacter = Utilities.FindFrontmostCharacter(character, friendlies);
        var startingIndexForRange = frontmostCharacter.transform.position.x;
        float buffMultiplier = 1.1f + (evolutionNumber * .1f);
        float rangeBegin = 20f;
        float rangeEnd = 40f;
        IEnumerable<_CharacterController> selectedFriendlies = friendlies.Where(friendly => friendly.transform.position.x - startingIndexForRange >= rangeBegin && friendly.transform.position.x - startingIndexForRange <= rangeEnd);
        foreach (_CharacterController friendly in selectedFriendlies)
        {
            AttackBuff attackBuff = new AttackBuff(float.MaxValue, buffMultiplier);
            DefenseBuff defenseBuff = new DefenseBuff(float.MaxValue, buffMultiplier);
            friendly.AddStatusEffect(attackBuff);
            friendly.AddStatusEffect(defenseBuff);
        }
    }

    //Coffee
    void RandomFriendliesGainSpeedBuff(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> friendlies = cc.deployedFriendlies.OfType<_CharacterController>().ToList();
        System.Random random = new System.Random();
        float speedBuffMultiplier = 1.5f + (.25f * evolutionNumber);
        float duration = 3 + (3 * evolutionNumber);
        List<_CharacterController> shuffledFriendlies = friendlies.OrderBy(f => random.Next()).ToList();
        _CharacterController chosenFriendly = shuffledFriendlies.First();
        List<_CharacterController> matchingFriendlies = friendlies.Where(f => f.characterData.Name == chosenFriendly.characterData.Name).ToList();  
        foreach (_CharacterController friendly in matchingFriendlies)
        {
            SpeedBuff speedBuff = new SpeedBuff(duration, speedBuffMultiplier);
            friendly.AddStatusEffect(speedBuff);
        }
    }

    //Donut
    void RandomFriendliesGainAttackBuff(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> friendlies = cc.deployedFriendlies.OfType<_CharacterController>().ToList();
        System.Random random = new System.Random();
        float attackBuffMultiplier = 1.5f + (.25f * evolutionNumber);
        float duration = 5 + (5 * evolutionNumber);
        List<_CharacterController> shuffledFriendlies = friendlies.OrderBy(f => random.Next()).ToList();
        _CharacterController chosenFriendly = shuffledFriendlies.First();
        List<_CharacterController> matchingFriendlies = friendlies.Where(f => f.characterData.Name == chosenFriendly.characterData.Name).ToList();
        foreach (_CharacterController friendly in matchingFriendlies)
        {
            AttackBuff attackBuff = new AttackBuff(duration, attackBuffMultiplier);
            friendly.AddStatusEffect(attackBuff);
        }
    }

    //DEBUFF ABILITIES

    //Strawberry
    void SlowAllAlliesPerNumSameCharacter(_CharacterController character, int evolutionNumber)
    {
        List<_CharacterController> friendlies = cc.deployedFriendlies.OfType<_CharacterController>().ToList();
        List<_CharacterController> enemies = cc.deployedEnemies.OfType<_CharacterController>().ToList();
        int numMatchingCharacters = friendlies.Where(friendly => friendly.characterData.Name == character.characterData.Name).Count();
        float percentSlow = numMatchingCharacters * (.04f + (.04f * evolutionNumber));
        percentSlow = 1.0f - percentSlow;
        float duration = 5.0f;
        float result = Math.Max(percentSlow, 0.01f);
        foreach (_CharacterController enemy in enemies)
        {
            SlowDebuff slowDebuff = new SlowDebuff(duration, result);
            enemy.AddStatusEffect(slowDebuff);
        }
    }
}
