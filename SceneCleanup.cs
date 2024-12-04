using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCleanup : MonoBehaviour
{
    public void CleanUpScene()
    {
        Debug.Log("cleaning up scene");
        // Destroy dynamically created game objects
        GameObject[] dynamicObjects = GameObject.FindGameObjectsWithTag("Finish");
        foreach (GameObject obj in dynamicObjects)
        {
            Destroy(obj);
        }

        // Unload unused assets
        Resources.UnloadUnusedAssets();

        // If the dynamically created scene is loaded additively, unload it
        SceneManager.UnloadSceneAsync("Combat Placeholder");
    }
}