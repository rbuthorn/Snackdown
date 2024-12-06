using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Linq;
using SQLite4Unity3d;

public class StaticButtonOnClickHandler : MonoBehaviour
{
    //public UnityEvent<int> onButtonClick = new UnityEvent<int>();

    //void questsButtonHandler()
    //{
    //    onButtonClick.Invoke(questsButtonFunction);
    //}
    //void questsButtonFunction()
    //{

    //}

    public void onButtonClick(int levelId)
    {
        Utilities.LoadScene("Combat Placeholder");
        GameManager GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameManager.setLevelId(levelId);
    }

    //void profileButtonHandler()
    //{
    //    onButtonClick.Invoke(profileButtonFunction);
    //}
    //void profileButtonFunction()
    //{

    //}

    //void campaignButtonHandler()
    //{
    //    onButtonClick.Invoke(campaignButtonFunction);
    //}
    //void campaignButtonFunction()
    //{

    //}

    //void survivalButtonHandler()
    //{
    //    onButtonClick.Invoke(survivalButtonFunction);
    //}
    //void survivalButtonFunction()
    //{

    //}

    //void spacelinesButtonHandler()
    //{
    //    onButtonClick.Invoke(spacelinesButtonFunction());
    //}
    //int spacelinesButtonFunction()
    //{

    //}

}
