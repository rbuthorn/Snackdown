using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class _CharacterController : _EntityController
{
    private AnimatorOverrideController overrideController;
    private AnimatorOverrideController newOverrideController;
    private List<_EntityController> _TARGETS = new List<_EntityController>(); //create here so that only one list is ever used for targets
    private Coroutine idleCoroutine;
    private List<StatusEffect> _activeEffects = new List<StatusEffect>();

    public List<_EntityController> TARGETS //this is effectively the same as a public variable. but, within the getter and setter, can add validaiton and whatnot
    {
        get { return _TARGETS; }
        set { _TARGETS = value; }
    }

    public List<StatusEffect> activeEffects
    {
        get { return _activeEffects; }
        set { _activeEffects = value; }
    }

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

    void Start()
    {
        sprite.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    void Update()
    {
        Color newColor = new Color(
            0.5f + (characterData.Health / maxHealth) / 2,
            0.5f + (characterData.Health / maxHealth) / 2,
            0.5f + (characterData.Health / maxHealth) / 2
        );
        sprite.color = newColor;
        UpdateEffects();
    }

    public void AttackProcess()
    {
        combatController.RefreshTargetsForCharacter(this);
        PerformAttack(TARGETS);
        ClearTargets();
    }

    public void PerformAttack(List<_EntityController> targets)
    {
        foreach (_EntityController target in targets)
        {
            DealDamageToTarget(characterData.Attack, target, false);
        }
    }

    public void DealDamageToTarget(float attack, _EntityController target, bool applyRawDamage)
    {
        if(target is _CharacterController targetChar)
        {
            float damage;
            if (applyRawDamage)
            {
                damage = attack;
            }
            else
            {
                damage = (float)(attack / ((4 / Math.PI) * Math.Atan(Math.Pow((characterData.Defense / attack), 2)) + 1));
            }
            targetChar.TakeDamage(damage);
            Debug.Log(this.characterData.Name + " dealt " + damage + " to " + targetChar.characterData.Name + ", who now has " + targetChar.characterData.Health);
            if (targetChar.CheckIfDead())
            {
                targetChar.TriggerDeathProcess();
            }
        }

        else if(target is _TowerController targetTower)
        {
            float damage;
            if (applyRawDamage)
            {
                damage = attack;
            }
            else
            {
                damage = (float)(attack / ((4 / Math.PI) * Math.Atan(Math.Pow((characterData.Defense / attack), 2)) + 1));
            }
            targetTower.TakeDamage(damage);
            Debug.Log(this.characterData.Name + " dealt " + damage + " to " + targetTower.characterData.Name + ", who now has " + targetTower.characterData.Health);
            if (targetTower.CheckIfDead())
            {
                targetTower.TriggerDeathProcess();
            }
        }
    }

    public void PerformAbility(List<_CharacterController> targets)
    {
        //perform ability
    }

    public void MoveRight()
    {
        if (!characterData.isTower)
        {
            transform.position += new Vector3(characterData.Speed * Time.deltaTime, 0f, 0f);
        }
    }

    public void MoveLeft()
    {
        if (!characterData.isTower)
        {
            transform.position -= new Vector3(characterData.Speed * Time.deltaTime, 0f, 0f);
        }
    }

    public void BeginIdleCoroutine()
    {
        if (!characterData.isTower)
        {
            idleCoroutine = StartCoroutine(IdleCoroutine());
        }
    }

    IEnumerator IdleCoroutine() //time elapsed over one frame
    {
        yield return new WaitForSeconds(characterData.AttackRate);
        UpdateAnimatorParameters("Fight");
    }

    public void StopIdleCoroutine()
    {
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }
    }

    public void TriggerDeathProcess()
    {
        combatController.UpdateDeployedList(this, false);
        combatController.DestroyPrefab(this);
        UpdateAnimatorParameters("Die");
    }

    public void UpdateAnimatorParameters(string stateName) //unset parameters and then set the right one
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", false);
        switch (stateName)
        {
            case "Walk":
                animator.SetBool(stateName, true);
                break;

            case "Idle":
                animator.SetBool(stateName, true);
                break;

            case "Fight":
                animator.SetTrigger(stateName);
                break;

            case "Die":
                animator.SetTrigger(stateName);
                break;
        }
    }

    public void ExitAttackState()
    {
        UpdateAnimatorParameters("Idle");
    }

    void ClearTargets()
    {
        TARGETS.Clear();
    }

    void SetAnimationOverrides()
    {
        AnimationClip idleAnimation = Utilities.LoadAsset<AnimationClip>("Animations/" + characterData.Name + "/Idle " + characterData.Name);
        AnimationClip walkAnimation = Utilities.LoadAsset<AnimationClip>("Animations/" + characterData.Name + "/Walk " + characterData.Name);
        AnimationClip fightAnimation = Utilities.LoadAsset<AnimationClip>("Animations/" + characterData.Name + "/Fight " + characterData.Name);
        ChangeAnimation("Idle Baby Carrot", idleAnimation);
        ChangeAnimation("Walk Baby Carrot", walkAnimation);
        ChangeAnimation("Fight Baby Carrot", fightAnimation);
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

    void UpdateEffects()
    {
        foreach(StatusEffect effect in activeEffects)
        {
            if (effect.UpdateEffect(Time.deltaTime)) //is the duration completed?
            {
                RemoveStatusEffect(effect);
            }
        }
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        effect.ApplyEffect(this);
        activeEffects.Add(effect);
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        effect.RemoveEffect(this);
        activeEffects.Remove(effect);
    }

    public void Heal(float healing)
    {
        characterData.Health += healing;
    }
}