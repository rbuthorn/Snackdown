using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuff : StatusEffect
{
    private float buffMultiplier;

    public DefenseBuff(float duration, float buffMultiplier) : base("DefenseBuff", duration)
    {
        this.buffMultiplier = buffMultiplier;
    }

    public override void ApplyEffect(_CharacterController character)
    {
        character.characterData.Defense *= buffMultiplier;
        Debug.Log("Defense Buff applied to " + character.characterData.Name);
    }

    public override void RemoveEffect(_CharacterController character)
    {
        character.characterData.Defense /= buffMultiplier;
        Debug.Log("Defense Buff removed from " + character.characterData.Name);
    }
}