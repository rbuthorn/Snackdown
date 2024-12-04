using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using System.Linq;

public class GachaManager : MonoBehaviour
{
    private System.Random random;

    void Start()
    {
        //consider using another system random variable and adding it to the millisecond random variable

        //use system time as the seed for the random numbers, 
        //that way its as random as possible
        DateTime now = DateTime.Now;
        int seed = now.Millisecond;
        random = new System.Random(seed);
    }

    CharacterData SingleBronze(){      
        int randVal = random.Next(0, 200);
        CharacterData _char = null;
        if(randVal < 140){
            _char = ChooseRandChar("rotten");
        }
        else if(randVal >= 140 && randVal < 190){
            _char = ChooseRandChar("stale");
        }
        else if(randVal >= 190 && randVal < 199){
            _char = ChooseRandChar("fresh");
        }
        else if(randVal == 199){
            _char = ChooseRandChar("tasty");
        }
        else{
            Debug.Log("Error in singleBronze");
        }
        return _char;
    }

    CharacterData SingleSilver(){      
        int randVal = random.Next(0, 100);
        CharacterData _char = null;
        if (randVal < 50){
            _char = ChooseRandChar("stale");
        }
        else if(randVal >= 50 && randVal < 80){
            _char = ChooseRandChar("fresh");
        }
        else if(randVal >= 80 && randVal < 95){
            _char = ChooseRandChar("tasty");
        }
        else if (randVal >= 95 && randVal < 100)
        {
            _char = ChooseRandChar("delectable");
        }
        else
        {
            Debug.Log("Error in singleSilver");
        }
        return _char;
    }

    CharacterData SingleGold(){
        int randVal = random.Next(0, 100);
        CharacterData _char = null;
        if (randVal < 40){
            _char = ChooseRandChar("fresh");
        }
        else if(randVal >= 40 && randVal < 80){
            _char = ChooseRandChar("tasty");
        }
        else if(randVal >= 80 && randVal < 95){
            _char = ChooseRandChar("delectable");
        }
        else if(randVal >= 95 && randVal < 100){
            _char = ChooseRandChar("gourmet");
        }
        else{
            Debug.Log("Error in singleGold");
        }
        return _char;
    }

    CharacterData SingleDiamond(){
        int randVal = random.Next(0, 100);
        CharacterData _char = null;
        if (randVal < 40){
            _char = ChooseRandChar("tasty");
        }
        else if(randVal >= 40 && randVal < 80){
            _char = ChooseRandChar("delectable");
        }
        else if(randVal >= 80 && randVal < 100){
            _char = ChooseRandChar("gourmet");
        }
        else{
            Debug.Log("Error in singleDiamond");
        }
        return _char;
    }

    List<CharacterData> TenBronze(){
        List<CharacterData> chars = new List<CharacterData>();
        for(int i = 0; i < 10; i++){
            chars.Add(SingleBronze());
        }
        return chars;
    }

    List<CharacterData> FiveSilver(){
        List<CharacterData> chars = new List<CharacterData>();
        for (int i = 0; i < 5; i++){
            chars.Add(SingleSilver());
        }
        return chars;
    }

    List<CharacterData> ThreeGold(){
        List<CharacterData> chars = new List<CharacterData>();
        for (int i = 0; i < 3; i++){
            chars.Add(SingleGold());
        }
        return chars;
    }
    List<CharacterData> TwoDiamond(){
        List<CharacterData> chars = new List<CharacterData>();
        for (int i = 0; i < 2; i++){
            chars.Add(SingleDiamond());
        }
        return chars;
    }

    CharacterData ChooseRandChar(string quality){
        List<CharacterData> gachaCharacters = LocalDatabaseAccessLayer.GetGachaCharacters(quality);
        int randVal = random.Next(0, gachaCharacters.Count);
        return gachaCharacters[randVal];
    }
}
