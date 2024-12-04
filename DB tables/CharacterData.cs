using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class CharacterData
{
    //ENEMY AND CHARACTER
    [PrimaryKey]
    public int DBCharacterId { get; set; }
    public string Name { get; set; }
    public string PrefabName { get; set; }
    public string Description { get; set; }
    public bool isTower { get; set; }
    public string Quality { get; set; } //has to be none(for utensils), rotten, stale, fresh, etc
    public float Attack { get; set; }
    public float Defense { get; set; }
    public float Health { get; set; }
    public float Speed { get; set; }
    public float CookTime { get; set; } //can be a disappearing bar over the character button
    public float Cooldown { get; set; } //cooldown in seconds
    public float AttackRange { get; set; } //furthest away an enemy can be
    //for attack animation to start
    public float AttackRate { get; set; } //time between frames of starting the attack animation. idle time is not inlcuded in attack animation(in case target enemy dies before starting an atk animation
    //attack rate = idle time + atk animation length. say an atk animation lasts 1 second, and the character is idle for 4 seconds before starting again. atk rate is 5 seconds
    //therefore, attack rate is generally more a measure of idle time, since atk animation is dictated by jackson
    public bool AreaAtk { get; set; }
    public float DamageRange { get; set; } //the minimum distance AWAY from the character the enemy has to be in order to take damage
    public bool isEnemy { get; set; } //this is not just for enemy characters9spoon, fork, etc) this is also for pvp for the characters you are fighting against

    //JUST CHARACTER
    public string Type { get; set; } //veggie, fruit, etc
    public int Cost { get; set; }
    public float AttackAdditivePerLevel { get; set; }
    public float DefenseAdditivePerLevel { get; set; }
    public float HealthAdditivePerLevel { get; set; }
    public int AbilityId { get; set; }
    public string AbilityDescription { get; set; }
    public int FirstEvolutionCharacterId { get; set; }
    public int SecondEvolutionCharacterId { get; set; }
    public int ThirdEvolutionCharacterId { get; set; }
    public int EvolutionNumber { get; set; } //choices are 0, 1, or 2.
    public int NumDuplicatesObtained { get; set; }
    public int NumDuplicatesToStarUp { get; set; }
    public int Stars { get; set; }
    public float FlyingHeight { get; set; }

    public CharacterData(
        int DBCharacterId,
        string Name,
        string PrefabName,
        bool isTower,
        string Description,
        string Type,
        string Quality,
        float Attack,
        float Defense,
        float Health,
        float Speed,
        float CookTime,
        float Cooldown,
        int Cost,
        float AttackRange,
        float AttackRate,
        bool AreaAtk,
        float DamageRange,
        bool isEnemy,
        float AttackAdditivePerLevel,
        float DefenseAdditivePerLevel,
        float HealthAdditivePerLevel,
        int AbilityId,
        string AbilityDescription,
        int FirstEvolutionCharacterId,
        int SecondEvolutionCharacterId,
        int ThirdEvolutionCharacterId,
        int EvolutionNumber,
        int NumDuplicatesObtained,
        int NumDuplicatesToStarUp, //should be constant
        int Stars,  //1-5 stars depedning on how many duplicates the player has gotten of this character
        float FlyingHeight
        )
    {
        setDBCharacterId(DBCharacterId);
        setName(Name);
        setPrefabName(PrefabName);
        setIsTower(isTower);
        setDescription(Description);
        setType(Type);
        setQuality(Quality);
        setAbilityId(AbilityId);
        setAttack(Attack);
        setDefense(Defense);
        setHealth(Health);
        setSpeed(Speed);
        setCookTime(CookTime);
        setCooldown(Cooldown);
        setCost(Cost);
        setAttackRange(AttackRange);
        setAttackRate(AttackRate);
        setAreaAtk(AreaAtk);
        setDamageRange(DamageRange);
        setIsEnemy(isEnemy);
        setAttackAdditivePerLevel(AttackAdditivePerLevel);
        setDefenseAdditivePerLevel(DefenseAdditivePerLevel);
        setHealthAdditivePerLevel(HealthAdditivePerLevel);
        setFirstEvolutionCharacterId(FirstEvolutionCharacterId);
        setSecondEvolutionCharacterId(SecondEvolutionCharacterId);
        setThirdEvolutionCharacterId(ThirdEvolutionCharacterId);
        setEvolutionNumber(EvolutionNumber);
        setNumDuplicatesObtained(NumDuplicatesObtained);
        setNumDuplicatesToStarUp(NumDuplicatesToStarUp);
        setStars(Stars);
        setFlyingHeight(FlyingHeight);
    }

    public CharacterData()
    {

    }

    public void setDBCharacterId(int a) {
        Debug.Assert(a >= 0);
        DBCharacterId = a;
    }
    public void setName(string a) {
        Debug.Assert(a.Length > 0);
        Name = a;
    }
    public void setPrefabName(string a) {
        Debug.Assert(a.Length > 0);
        PrefabName = a;
    }
    public void setDescription(string a)
    {
        Debug.Assert(a != "");
        Description = a;
    }
    public void setType(string a) {
        Debug.Assert((a == "vegetable" || a == "fruit" || a == "dairy" || a == "seafood" || a == "dessert" || a == "meat" || a == "enemy"));
        Type = a;
    }
    public void setQuality(string a){
        Debug.Assert(a=="rotten" || 
            a=="stale" || 
            a=="fresh" || 
            a=="tasty" || 
            a=="delectable" || 
            a=="gourmet" ||
            a=="none");
        Quality = a;
    }
    public void setAbilityId(int a){
        Debug.Assert(a >= 0);
        AbilityId = a;
    }
    public void setAttack(float a){
        Debug.Assert(a >= 0);
        Attack = a;
    }
    public void setDefense(float a){
        Debug.Assert(a >= 0);
        Defense = a;
    }
    public void setHealth(float a){
        Debug.Assert(a >= 0);
        Health = a;
    }
    public void setSpeed(float a){
        Debug.Assert(a >= 0);
        Speed = a;
    }
    public void setCookTime(float a){
        Debug.Assert(a >= 0);
        CookTime = a;
    }
    public void setCooldown(float a)
    {
        Debug.Assert(a >= 0);
        Cooldown = a;
    }
    public void setCost(int a){
        Debug.Assert(a >= 0);
        Cost = a;
    }
    public void setAttackRange(float a){
        Debug.Assert(a >= 0);
        AttackRange = a;
    }
    public void setAttackRate(float a){
        Debug.Assert(a >= 0);
        AttackRate = a;
    }
    public void setAreaAtk(bool a){
        AreaAtk = a;
    }
    public void setDamageRange(float a){
        Debug.Assert(a >= 0);
        DamageRange = a;
    }
    public void setIsEnemy(bool a)
    {
        isEnemy = a;
    }
    public void setIsTower(bool a)
    {
        isTower = a;
    }
    public void setAttackAdditivePerLevel(float a)
    {
        Debug.Assert(a >= 0);
        AttackAdditivePerLevel = a;
    }
    public void setDefenseAdditivePerLevel(float a)
    {
        Debug.Assert(a >= 0);
        DefenseAdditivePerLevel = a;
    }
    public void setHealthAdditivePerLevel(float a)
    {
        Debug.Assert(a >= 0);
        HealthAdditivePerLevel = a;
    }
    public void setFirstEvolutionCharacterId(int a)
    {
        Debug.Assert(a >= 0);
        FirstEvolutionCharacterId = a;
    }
    public void setSecondEvolutionCharacterId(int a)
    {
        Debug.Assert(a >= 0);
        SecondEvolutionCharacterId = a;
    }
    public void setThirdEvolutionCharacterId(int a)
    {
        Debug.Assert(a >= 0);
        ThirdEvolutionCharacterId = a;
    }
    public void setEvolutionNumber(int a)
    {
        Debug.Assert(a==0 || a==1 || a==2);
        EvolutionNumber = a;
    }
    public void setNumDuplicatesObtained(int a)
    {
        Debug.Assert(a >= 0);
        NumDuplicatesObtained = a;
    }
    public void setNumDuplicatesToStarUp(int a)
    {
        Debug.Assert(a > 0);
        NumDuplicatesToStarUp = a;
    }
    public void setStars(int a)
    {
        Debug.Assert(a >= 1 && a <= 5);
        Stars = a;
    }
    public void setFlyingHeight(float a)
    {
        Debug.Assert(a >= 0);
        FlyingHeight = a;
    }
}
