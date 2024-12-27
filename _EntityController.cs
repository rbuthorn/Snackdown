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
    protected float maxHealth;
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

    protected void Start()
    {

    }

    protected void Update()
    {
        if (!characterData.isTower)
        {
            Color newColor = new Color(
                0.5f + (characterData.Health / maxHealth) / 2,
                0.5f + (characterData.Health / maxHealth) / 2,
                0.5f + (characterData.Health / maxHealth) / 2
            );
            sprite.color = newColor;
        }
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