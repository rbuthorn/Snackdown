using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class _TowerController : _EntityController
{
    private AnimatorOverrideController overrideController;
    private AnimatorOverrideController newOverrideController;

    void Awake()
    {
        combatController = GameObject.Find("combatController").GetComponent<CombatController>();
        sprite = GetComponent<SpriteRenderer>();
        string prefabName = GetComponent<PrefabName>().originalPrefabName;
        characterData = LocalDatabaseAccessLayer.LoadCharacterData(prefabName);
        if (!characterData.isEnemy)
        {
            LocalDatabaseAccessLayer.ApplySensitiveCharacterData(prefabName, characterData);
        }
        SetAnimators();
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
        animator.SetBool("Idle", false);
        switch (stateName)
        {
            case "Idle":
                animator.SetBool(stateName, true);
                break;

            case "Cook":
                animator.SetTrigger(stateName);
                break;

            case "Deploy":
                animator.SetTrigger(stateName);
                break;
        }
    }

    void SetAnimationOverrides()
    {
        if (characterData.isEnemy)
        {
            AnimationClip idleAnimation = Utilities.LoadAsset<AnimationClip>("Animations/Cabinet Idle");
            AnimationClip deployAnimation = Utilities.LoadAsset<AnimationClip>("Animations/Cabinet Deploy");
            AnimationClip cookAnimation = Utilities.LoadAsset<AnimationClip>("Animations/Cabinet Cook");
            ChangeAnimation("Chef Idle", idleAnimation);
            ChangeAnimation("Chef Deploy", deployAnimation);
            ChangeAnimation("Chef Cooking", cookAnimation);
        }
    }

    void ChangeAnimation(string placeholderName, AnimationClip newAnimation)
    {
        // Retrieve current overrides
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(newOverrideController.overridesCount);
        newOverrideController.GetOverrides(overrides);

        // Find and replace the placeholder animation
        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key.name == placeholderName)
            {
                Debug.Log(overrides[i].Key);
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, newAnimation);
            }
        }
        newOverrideController.ApplyOverrides(overrides);
    }

    void SetAnimators()
    {
        try
        {
            animator = GetComponent<Animator>();
            overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            newOverrideController = new AnimatorOverrideController(overrideController);
            SetAnimationOverrides();
            animator.runtimeAnimatorController = newOverrideController;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
