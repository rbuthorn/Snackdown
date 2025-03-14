using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Linq;


public class ButtonGenerator : MonoBehaviour
{
    private Canvas canvas;
    private UnityEvent<int> onButtonClick = new UnityEvent<int>();
    private List<string> buttonCharacters;
    private List<string> buttonNames;
    private List<GameObject> buttonLineup;
    private Dictionary<GameObject, (int, float, float)> prefabInstances;
    private int xOffset = -474;
    private CharacterSpawner spawner;

    void Start()
    {
        canvas = GameObject.Find("Basement").GetComponent<Canvas>();
        //hardcoded solution for now
        buttonNames = new List<string> { "Baby Carrot Button Prefab", "Butter Button Prefab", "Berry Bandit Button Prefab", "Kawaii Kiwi Button Prefab"};
        buttonCharacters = new List<string> { "Baby Carrot Prefab", "Butter Prefab", "Berry Bandit Prefab", "Kawaii Kiwi Prefab"};
        buttonLineup = new List<GameObject>();
        spawner = GetComponent<CharacterSpawner>();
        prefabInstances = new Dictionary<GameObject, (int, float, float)>();

        foreach(string item in buttonCharacters)
        {
            CharacterData character = LocalDatabaseAccessLayer.LoadCharacterData(item);
            prefabInstances.Add(Utilities.LoadAsset<GameObject>("Prefabs/Character Prefabs/" + item), (character.Cost, character.CookTime, character.Cooldown));
        }

        foreach (string item in buttonNames)
        {
            buttonLineup.Add(Utilities.LoadButtonPrefab(item));
        }
        setButtons();
    }

    void setButtons()
    {
        // buttonLineup = grabButtonLineup();

        ICollection<GameObject> keys = prefabInstances.Keys;
        int index = 0;
        foreach (GameObject key in keys)
        {

            GameObject newButton = Instantiate(buttonLineup[index], Vector3.zero, Quaternion.identity);
            newButton.transform.SetParent(canvas.transform, false);

            RectTransform buttonRect = newButton.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(xOffset, -250f); // Set the position relative to the Canvas.
            buttonRect.sizeDelta = new Vector2(50f, 50f);
            xOffset += 50;

            Button button = newButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => ButtonClickHandler(button, key, prefabInstances[key]));
            }
            index += 1;
        }
    }

    void ButtonClickHandler(Button button, GameObject key, (int cost, float cookTime, float cooldown) tuple)
    {
        onButtonClick.Invoke(spawner.DeployFriendly(button, key, tuple));
    }

    void grabButtonLineup(){

    }

    // List<GameObject> GrabLineupData(){
    //     string json = PlayerPrefs.GetString("LineupData");
    //     LineupData lineupData = JsonUtility.FromJson<LineupData>(json);
    //     List<GameObject> loadedLineup = lineupData.lineup;
    //     return loadedLineup;
    // }
}
