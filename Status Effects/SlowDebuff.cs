using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : StatusEffect
{
    private float debuffPercent; //(0,1)

    public SlowDebuff(float duration, float debuffPercent) : base("SlowDebuff", duration)
    {
        this.debuffPercent = debuffPercent;
    }

    public override void ApplyEffect(_CharacterController character)
    {
        character.characterData.Speed *= debuffPercent;
        Debug.Log("Slow Debuff applied to " + character.characterData.Name);
    }

    public override void RemoveEffect(_CharacterController character)
    {
        character.characterData.Speed /= debuffPercent;
        Debug.Log("Slow Debuff removed from " + character.characterData.Name);
    }
}
