using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : StatusEffect
{
    private float debuffMultiplier; //(0,1)

    public SlowDebuff(float duration, float debuffMultiplier) : base("SlowDebuff", duration)
    {
        this.debuffMultiplier = debuffMultiplier;
    }

    public override void ApplyEffect(_CharacterController character)
    {
        character.characterData.Speed *= debuffMultiplier;
        Debug.Log("Slow Debuff applied to " + character.characterData.Name);
    }

    public override void RemoveEffect(_CharacterController character)
    {
        character.characterData.Speed /= debuffMultiplier;
        Debug.Log("Slow Debuff removed from " + character.characterData.Name);
    }
}
