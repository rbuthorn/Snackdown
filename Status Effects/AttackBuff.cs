using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuff : StatusEffect
{
    private float buffMultiplier;

    public AttackBuff(float duration, float buffMultiplier) : base("AttackBuff", duration)
    {
        this.buffMultiplier = buffMultiplier;
    }

    public override void ApplyEffect(_CharacterController character)
    {
        character.characterData.Attack *= buffMultiplier; 
        Debug.Log("Attack Buff applied to " + character.characterData.Name);
    }

    public override void RemoveEffect(_CharacterController character)
    {
        character.characterData.Attack /= buffMultiplier;
        Debug.Log("Attack Buff removed from " + character.characterData.Name);
    }
}