using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : StatusEffect
{
    private float buffMultiplier;

    public SpeedBuff(float duration, float buffMultiplier) : base("SpeedBuff", duration)
    {
        this.buffMultiplier = buffMultiplier;
    }

    public override void ApplyEffect(_CharacterController character)
    {
        character.characterData.Speed *= buffMultiplier;
        Debug.Log("Speed Buff applied to " + character.characterData.Name);
    }

    public override void RemoveEffect(_CharacterController character)
    {
        character.characterData.Speed /= buffMultiplier;
        Debug.Log("Speed Buff removed from " + character.characterData.Name);
    }
}