using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalController : MonoBehaviour
{
    //notes:
    //player will have a lineup going into the level -- enemies keep their diminished health/death status 
    //if the player loses that attempt. Characters are not able to be used again opnce they die

    //survival resets at midnight. Enemy deployments are controlled through a server so that everyones survival
    //looks the same, and is scaled by level

    //there will be 6 entries in the LevelData designated for survival; It grabs data from
    //the server upon loading the game and overwrites the existing 6 levels from yesterday. 
    //These entries contain spawn point datas as well as modifiers for when the player has played the levels but didnt
    //beat them; things like enemy health or death.

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
