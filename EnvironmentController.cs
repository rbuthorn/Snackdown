using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//use this for setting up the combat controller/character controller stuff per environment
//each environment should have slightly different rules for the way the combat and character controllers act
public class EnvironmentController : MonoBehaviour
{
    string environment = "misc";
    void Start() {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.ToUpper().Contains("PVP"))
        {
            environment = "pvp";
        }
        else if (sceneName.ToUpper().Contains("CAMPAIGN"))
        {
            environment = "campaign";
        }
        else if (sceneName.ToUpper().Contains("SURVIVAL"))
        {
            environment = "survival";
        }
        else
        {
            environment = "misc";
        }
    }
}
