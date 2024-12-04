using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[System.Serializable]
public class LineupData
{
    public List<string> charLineup; //names of the character prefabs

    void SaveLineup(){
        // Serialize the list into JSON format
        string charjson = JsonUtility.ToJson(charLineup);

        // Store the JSON string in PlayerPrefs
        PlayerPrefs.SetString("CharacterLineupData", charjson);
        PlayerPrefs.Save();
    }

    void UpdateLineup(string character)
    {
        if (charLineup.Contains(character))
        {
            charLineup.Remove(character);
        }
        else if (!charLineup.Contains(character))
        {
            charLineup.Add(character);
        }
        SaveLineup();
    }
}