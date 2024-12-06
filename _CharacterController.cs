using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class _CharacterController : MonoBehaviour
{
    private CombatController combatController;
    private Animator animator;  // Reference to the Animator component attached to the character prefab
    private AnimatorOverrideController overrideController;
    private AnimatorOverrideController newOverrideController;
    private SpriteRenderer sprite;
    private CharacterData _characterData;
    private GameObject _PREFAB;
    private float maxHealth;
    private Color color;
    private List<_CharacterController> _TARGETS = new List<_CharacterController>(); //create here so that only one list is ever used for targets
    private Coroutine idleCoroutine;

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

    public List<_CharacterController> TARGETS
    {
        get { return _TARGETS; }
        set { _TARGETS = value; }
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

    }

    void Update()
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

    public void AttackProcess()
    {
        combatController.RefreshTargetsForCharacter(this);
        PerformAttack(TARGETS);
        ClearTargets();
    }

    public void PerformAttack(List<_CharacterController> targets)
    {
        foreach (_CharacterController target in targets)
        {
            DealDamageToTarget(characterData.Attack, target, false);
        }
    }

    public void DealDamageToTarget(float attack, _CharacterController target, bool applyRawDamage)
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
        target.TakeDamage(damage);
        Debug.Log(this.characterData.Name + " dealt " + damage + " to " + target.characterData.Name + ", who now has " + target.characterData.Health);
        if (target.CheckIfDead())
        {
            target.TriggerDeathProcess();
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

    public void TriggerDeathProcess()
    {
        combatController.UpdateDeployedList(this, false);
        combatController.DestroyPrefab(this);
        UpdateAnimatorParameters("Die");
    }

    public void PerformAbility(List<_CharacterController> targets)
    {
        animator.SetTrigger("Ability");
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

    void SetMaxHealth()
    {
        maxHealth = characterData.Health;
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

    public bool CheckAnimatorStateName(string stateName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
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
}