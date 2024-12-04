using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class PrefabCreator : MonoBehaviour
{
    public List<CharacterData> charJson;

    void Start()
    {
        List<CharacterData> char_json = Utilities.LoadCharactersJson();
        foreach (CharacterData character in char_json)
        {
            CreatePrefabFromGameObject(character);
        }
    }
    // Example method to create a prefab from a GameObject during runtime
    public void CreatePrefabFromGameObject(CharacterData chardata)
    {
        //remember to implement the layer on the spriterenderer set to 10
        GameObject character = new GameObject(chardata.PrefabName);
        var spr = character.AddComponent<SpriteRenderer>();
        spr.sortingOrder = 10;
        character.AddComponent<_CharacterController>();
        var pn = character.AddComponent<PrefabName>();
        pn.originalPrefabName = chardata.PrefabName;
        // Create a prefab from the instantiated GameObject
#if UNITY_EDITOR
        string prefabPath = "Assets/Resources/Prefabs/" + chardata.PrefabName + ".prefab";
        PrefabUtility.SaveAsPrefabAsset(character, "Assets/Resources/Prefabs/Character Prefabs/" + chardata.PrefabName + ".prefab");
        Debug.Log("Prefab saved at: " + prefabPath);

#endif
        // Destroy the instantiated GameObject (optional)
        //Destroy(character);
    }
}
