using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseDebuff : StatusEffect
{
    private float debuffMultiplier; //(0,1)

    public DefenseDebuff(float duration, float debuffMultiplier) : base("DefenseDebuff", duration)
    {
        this.debuffMultiplier = debuffMultiplier;
    }

    public override void ApplyEffect(_CharacterController character)
    {
        character.characterData.Defense *= debuffMultiplier;
        Debug.Log("Defense Debuff applied to " + character.characterData.Name);
    }

    public override void RemoveEffect(_CharacterController character)
    {
        character.characterData.Defense /= debuffMultiplier;
        Debug.Log("Defense Debuff removed from " + character.characterData.Name);
    }
}