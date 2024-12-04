using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    private WaitForSeconds waitThirtySeconds;
    private WaitForSeconds waitOneMinute;

    void Start()
    {
        StartTimePlayedCoroutine();
        StartEnergyCoroutine();
    }

    void StartTimePlayedCoroutine()
    {
        StartCoroutine(UpdateTimePlayed());
    }

    void StartEnergyCoroutine()
    {
        StartCoroutine(UpdateEnergy());
    }

    IEnumerator UpdateTimePlayed()
    {
        int timePlayed;
        while (true)
        {
            yield return waitOneMinute;
            timePlayed = PlayerPrefs.GetInt("TimePlayed");
            timePlayed += 1;
            PlayerPrefs.SetInt("TimePlayed", timePlayed);
            PlayerPrefs.Save();
        }
    }

    IEnumerator UpdateEnergy()
    {
        int currentEnergy;
        while (true)
        {
            yield return waitThirtySeconds;
            currentEnergy = PlayerPrefs.GetInt("Energy");
            currentEnergy += 1;
            PlayerPrefs.SetInt("Energy", currentEnergy);
            PlayerPrefs.Save();
        }
    }
}