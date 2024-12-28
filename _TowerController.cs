using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class _TowerController : _EntityController
{
    void Awake()
    {
        combatController = GameObject.Find("combatController").GetComponent<CombatController>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        string prefabName = GetComponent<PrefabName>().originalPrefabName;
        characterData = LocalDatabaseAccessLayer.LoadCharacterData(prefabName);
        if (!characterData.isEnemy)
        {
            LocalDatabaseAccessLayer.ApplySensitiveCharacterData(prefabName, characterData);
        }
        color = sprite.color;
        SetMaxHealth();
    }

    public void TriggerDeathProcess()
    {
        combatController.UpdateDeployedList(this, false);
        combatController.DestroyPrefab(this);
    }

    public void UpdateAnimatorParameters(string stateName) //unset parameters and then set the right one
    {
        if (!characterData.isEnemy)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Cook", false);
            switch (stateName)
            {
                case "Idle":
                    animator.SetBool(stateName, true);
                    break;

                case "Cook":
                    animator.SetBool(stateName, true);
                    break;

                case "Deploy":
                    animator.SetTrigger(stateName);
                    break;
            }
        }
        else
        {
            animator.SetBool("Idle", false);
            switch (stateName)
            {
                case "Idle":
                    animator.SetBool(stateName, true);
                    break;

                case "Deploy":
                    animator.SetTrigger(stateName);
                    break;
            }
        }
    }
}
