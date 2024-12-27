using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utilities
{
    // Start is called before the first frame update
    public static GameObject LoadButtonPrefab(string prefabName)
    {
        return Resources.Load<GameObject>("Prefabs/Combat Icons/" + prefabName);
    }

    public static List<CharacterData> LoadCharactersJson()
    {
        return null;
    }

    public static T LoadAsset<T>(string fileLocation) where T : Object
    {
        return Resources.Load<T>(fileLocation);
    }

    public static void SortByXPosDescending<T>(List<T> characters) where T : Component
    {
        characters.Sort((go1, go2) => go2.transform.position.x.CompareTo(go1.transform.position.x));
    }

    public static void SortByXPosAscending<T>(List<T> characters) where T : Component
    {
        characters.Sort((go1, go2) => go1.transform.position.x.CompareTo(go2.transform.position.x));
    }

    public static void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public static bool IsACharacterWithinRange(List<_EntityController> deployList, _EntityController referenceCharacter)
    {
        float referenceX = referenceCharacter.transform.position.x;
        float attackRange = referenceCharacter.characterData.AttackRange;
        float damageRange = referenceCharacter.characterData.DamageRange;
        bool isAreaAttack = referenceCharacter.characterData.AreaAtk;
        bool isEnemy = referenceCharacter.characterData.isEnemy;

        foreach (_EntityController character in deployList)
        {
            float characterX = character.transform.position.x;

            // Determine the direction based on the relationship (friend or enemy)
            float distance = isEnemy ? referenceX - characterX : characterX - referenceX;

            // If the closest character is beyond the attack range, break out of the loop
            if (distance > attackRange)
            {
                break;
            }

            // If character is too close or out of attack range, skip to the next one
            else if (distance <= 0.5f)
            {
                continue;
            }

            else if (distance < damageRange)
            {
                return true;
            }

            // If character is within attack range and not too close, add to targets
            else if (distance <= attackRange && distance >= damageRange)
            {
                return true;
            }
        }
        return false;
    }

    //assumes the character is already attacking
    public static void AddCharactersToTargets(List<_EntityController> deployList, _EntityController referenceCharacter)
    {
        //referenceCharacter.clearTargets();
        float referenceX = referenceCharacter.transform.position.x;
        float attackRange = referenceCharacter.characterData.AttackRange;
        float damageRange = referenceCharacter.characterData.DamageRange;
        bool isAreaAttack = referenceCharacter.characterData.AreaAtk;
        bool isEnemy = referenceCharacter.characterData.isEnemy;

        foreach (_EntityController character in deployList)
        {
            if(referenceCharacter is _CharacterController refChar)
            {
                float characterX = character.transform.position.x;

                // Determine the direction based on the relationship (friend or enemy)
                float distance = isEnemy ? referenceX - characterX : characterX - referenceX;

                // If the closest character is beyond the attack range, break out of the loop
                if (distance > attackRange)
                {
                    break;
                }

                // If character is too close or out of attack range, skip to the next one
                else if (distance <= 5f)
                {
                    continue;
                }

                // If character is within attack range and not too close, add to targets
                else if (distance <= attackRange && distance >= damageRange)
                {
                    refChar.TARGETS.Add(character);

                    // If single-target attack, stop after first match
                    if (!isAreaAttack)
                    {
                        break;
                    }
                }
            }
        }
    }
}
