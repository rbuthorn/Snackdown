using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifesteal : StatusEffect
{
    private float lifestealPercentage;

    public Lifesteal(float duration, float lifestealPercentage) : base("Lifesteal", duration)
    {
        this.lifestealPercentage = lifestealPercentage;
    }

    public override void ApplyEffect(_CharacterController character)
    {
        Debug.Log("Lifesteal applied to " + character.characterData.Name);
    }

    public override void RemoveEffect(_CharacterController character)
    {
        Debug.Log("Lifesteal removed from " + character.characterData.Name);
    }

    public override void OnAttack(_CharacterController character, float rawDamage)
    {
        character.Heal(rawDamage * lifestealPercentage);
    }
}