using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class _EntityController : MonoBehaviour
{
    protected CombatController combatController;
    protected Animator animator;  // Reference to the Animator component attached to the character prefab
    protected SpriteRenderer sprite;
    protected CharacterData _characterData;
    protected GameObject _PREFAB;
    protected float _maxHealth;
    protected Color color;

    public CharacterData characterData
    {
        get { return _characterData; }
        set { _characterData = value; }
    }

    public GameObject PREFAB
    {
        get { return _PREFAB; }
        set { _PREFAB = value; }
    }

    public float maxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    protected void Start()
    {

    }

    public void TakeDamage(float damage)
    {
        characterData.Health -= damage;
    }

    public bool CheckIfDead()
    {
        if (characterData.Health <= 0)
        {
            return true;
        }
        return false;
    }

    protected void SetMaxHealth()
    {
        maxHealth = characterData.Health;
    }

    public bool CheckAnimatorStateName(string stateName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
    }
}