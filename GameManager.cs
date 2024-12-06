using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string _environment;
    private int _levelId;

    public string environment
    {
        get { return _environment; }
        set { _environment = value; }
    }

    public int levelId
    {
        get { return _levelId; }
    }

    public void setLevelId(int value)
    {
        _levelId = value;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        setEnvironment();
    }

    void Update()
    {
        
    }

    void setEnvironment()
    {
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
        else if (sceneName.ToUpper().Contains("COMBAT"))
        {
            environment = "combat";
        }
        else
        {
            environment = "misc";
        }
    }
}
