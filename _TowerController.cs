using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class _TowerController : _EntityController
{
    private List<long> states = new List<long>(); //create here so that only one list is ever used for targets

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

    void Update()
    {
        UpdateAnimatorParameters("");
    }

    public void TriggerDeathProcess()
    {
        combatController.UpdateDeployedList(this, false);
        combatController.DestroyPrefab(this);
    }

    public void TriggerDeploy()
    {
        animator.SetTrigger("Deploy");
    }

    public void AddToStateList(long ts)
    {
        states.Add(ts);
    }

    public void RemoveFromStateList(long ts)
    {
        states.Remove(ts);
    }

    public void UpdateAnimatorParameters(string stateName) //unset parameters and then set the right one
    {
        if (!characterData.isEnemy)
        {
            if (states.Count > 0)
            {
                animator.SetBool("Cook", true);
                animator.SetBool("Idle", false);
            }
            else
            {
                animator.SetBool("Cook", false);
                animator.SetBool("Idle", true);
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
