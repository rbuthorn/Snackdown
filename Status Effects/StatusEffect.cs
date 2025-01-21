using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    private string Name;
    private float Duration;
    private float TimeElapsed;

    protected StatusEffect(string name, float duration)
    {
        Name = name;
        Duration = duration;
        TimeElapsed = 0;
    }

    public abstract void ApplyEffect(_CharacterController character);
    public abstract void RemoveEffect(_CharacterController character);

    public virtual void OnAttack(_CharacterController character, float rawDamage) { }
    public virtual void OnDamageTaken(_CharacterController character, float damageTaken) { }

    public bool UpdateEffect(float deltaTime)
    {
        TimeElapsed += deltaTime;
        return TimeElapsed >= Duration;
    }

}
