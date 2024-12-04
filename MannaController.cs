using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class MannaController : MonoBehaviour
{
    private Text levelText;
    private Text upgradeMannaCostText;
    private Text currentMannaText;
    private Canvas canvas;
    private UnityEvent<int> onButtonClick = new UnityEvent<int>();
    private Button button;
    private float _mps = 50f; //manna per second
    private float _mpsUpgradePerLevel = 25;
    private int _currentMannaLevel = 1;
    private const float _costPerLevel = 250f;
    private float _currentManna = 0;
    private const int MAXLEVEL = 9;

    public float mps
    {
        get { return _mps; }
        set { _mps = value; }
    }

    public float mpsUpgradePerLevel
    {
        get { return _mpsUpgradePerLevel; }
    }

    public int currentMannaLevel
    {
        get { return _currentMannaLevel; }
        set { _currentMannaLevel = value; }
    }

    public float costPerLevel
    {
        get { return _costPerLevel; }
    }

    public float currentManna
    {
        get { return _currentManna; }
        set { _currentManna = value; }
    }

    void Start()
    {
        canvas = GameObject.Find("Basement").GetComponent<Canvas>();
        initTexts();
        createMannaButton();
    }
    void Update()
    {
        currentManna += mps * Time.deltaTime;
        currentMannaText.text = "Manna: " + Mathf.FloorToInt(currentManna); // Get the integer part
    }

    public void initTexts()
    {
        levelText = GameObject.Find("mannaLevelGO").GetComponent<Text>();
        upgradeMannaCostText = GameObject.Find("upgradeMannaCostGO").GetComponent<Text>();
        currentMannaText = GameObject.Find("currentMannaGO").GetComponent<Text>();

        levelText.font = Font.CreateDynamicFontFromOSFont("Anton", 16);
        levelText.color = Color.black;
        upgradeMannaCostText.font = Font.CreateDynamicFontFromOSFont("Anton", 16);
        upgradeMannaCostText.color = Color.black;
        currentMannaText.font = Font.CreateDynamicFontFromOSFont("Anton", 16);
        currentMannaText.color = Color.black;

        levelText.transform.SetParent(canvas.transform, false);
        RectTransform levelTextRect = levelText.GetComponent<RectTransform>();
        levelTextRect.anchoredPosition = new Vector2(-600f, -175f); // Set the position relative to the Canvas.
        levelTextRect.sizeDelta = new Vector2(100f, 60f);

        upgradeMannaCostText.transform.SetParent(canvas.transform, false);
        RectTransform upgradeMannaCostTextRect = upgradeMannaCostText.GetComponent<RectTransform>();
        upgradeMannaCostTextRect.anchoredPosition = new Vector2(-600f, -200f); // Set the position relative to the Canvas.
        upgradeMannaCostTextRect.sizeDelta = new Vector2(100f, 60f);

        currentMannaText.transform.SetParent(canvas.transform, false);
        RectTransform currentMannaTextRect = currentMannaText.GetComponent<RectTransform>();
        currentMannaTextRect.anchoredPosition = new Vector2(-600f, 125f); // Set the position relative to the Canvas.
        currentMannaTextRect.sizeDelta = new Vector2(100f, 60f);

        levelText.text = "Level: " + currentMannaLevel;
        upgradeMannaCostText.text = "Cost: " + (currentMannaLevel * costPerLevel);
        currentMannaText.text = "Manna: " + currentManna;
    }

    public int levelUpMannaGen()
    {
        if(currentManna >= currentMannaLevel * costPerLevel && currentMannaLevel < MAXLEVEL)
        {
            currentManna -= currentMannaLevel * costPerLevel;
            mps += mpsUpgradePerLevel;
            currentMannaLevel += 1;
            levelText.text = "Level: " + currentMannaLevel;
            upgradeMannaCostText.text = "Cost: " + (currentMannaLevel * costPerLevel);
            currentMannaText.text = "Manna: " + currentManna;
            checkMaxLevel();
        }
        else
        {
            Debug.Log("not enough manna to upgrade or at max level already"); //change to an actual message the player sees
        }
        return 1;
    }

    public void checkMaxLevel()
    {
        if(currentMannaLevel == MAXLEVEL)
        {
            button.interactable = false;
            upgradeMannaCostText.enabled = false;
            levelText.text = "    MAX";
        }
    }

    public void createMannaButton()
    {
        GameObject newButton = Instantiate(Utilities.LoadButtonPrefab("Standard Manna Button Prefab"), Vector3.zero, Quaternion.identity);
        newButton.transform.SetParent(canvas.transform, false);

        RectTransform buttonRect = newButton.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = new Vector2(-600f, -250f); // Set the position relative to the Canvas.
        buttonRect.sizeDelta = new Vector2(60f, 70f);
        button = newButton.GetComponent<Button>();
        button.onClick.AddListener(() => MannaButtonClickHandler());
    }

    void MannaButtonClickHandler()
    {
        onButtonClick.Invoke(levelUpMannaGen());
    }

    public bool CheckIfEnoughManna(int cost)
    {
        if (currentManna >= cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateCurrentManna(int updateAmount)
    {
        currentManna += updateAmount; //updateAmount can be positive or negative
    }
}
