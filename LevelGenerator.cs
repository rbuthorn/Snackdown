using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelGenerator : MonoBehaviour
{
    private GameObject enemyPrefab; // Prefab for the enemy objects
    private GameObject background; // Reference to the floor sprite renderer
    private GameObject floor; // Reference to the background sprite renderer
    private GameObject basement; // Reference to the bottom sprite renderer
    private GameObject mannaLevelGO;
    private GameObject upgradeMannaCostGO;
    private GameObject currentMannaGO;
    private Camera mainCamera;
    private GameObject buttonGenerator;
    private GameObject combatController;
    private GameObject MannaController;

    private void Start()
    {
        //tag all dynamic gameobjects with "DYNAMIC"
        GenerateScenery();
        InitScripts();
    }

    private void GenerateScenery()
    {
        mainCamera = Camera.main;
        mainCamera.orthographicSize = 220;
        //get level data from the db and put it into the createCanvas method
        createTexts();
        createCanvas("Basement", "Aged Cheese Prefab", 55f);
    }

    private void createCanvas(string GOName, string levelSprite, float yPos)
    {
        background = new GameObject(GOName);
        background.tag = "Finish";
        Canvas canvas = background.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        //Image image = background.AddComponent<Image>();
        background.GetComponent<RectTransform>().localPosition = new Vector3(349.5f, yPos, 0f);
        background.AddComponent<CanvasScaler>();
        background.AddComponent<GraphicRaycaster>();
    }

    private void createTexts()
    {
        mannaLevelGO = new GameObject("mannaLevelGO");
        upgradeMannaCostGO = new GameObject("upgradeMannaCostGO");
        currentMannaGO = new GameObject("currentMannaGO");

        Text mannaLevelText = mannaLevelGO.AddComponent<Text>();
        Text upgradeMannaCostText = upgradeMannaCostGO.AddComponent<Text>();
        Text currentMannaText = currentMannaGO.AddComponent<Text>();
    }

    private void InitScripts()
    {
        buttonGenerator = new GameObject("buttonGenerator");
        combatController = new GameObject("combatController");
        MannaController = new GameObject("MannaController");

        buttonGenerator.tag = "Finish";
        combatController.tag = "Finish";
        MannaController.tag = "Finish";

        buttonGenerator.AddComponent<ButtonGenerator>();
        buttonGenerator.AddComponent<CharacterSpawner>();
        combatController.AddComponent<CombatController>();
        combatController.AddComponent<SceneCleanup>();
        combatController.AddComponent<CharacterSpawner>();
        MannaController.AddComponent<MannaController>();
    }
}
