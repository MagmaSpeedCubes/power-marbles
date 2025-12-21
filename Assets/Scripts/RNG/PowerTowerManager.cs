using UnityEngine;
using System;


public class PowerTowerManager : MonoBehaviour
{
    int level = 1;
    float loseChance = 0.15f;
    System.Random rng;
    public void RunPowerTower()
    {
        rng = new System.Random();
    }

    public void OpenBox()
    {
        bool loseOnLevel = rng.NextDouble() < loseChance;
        if(loseOnLevel && CanLoseOnLevel(level))
        {
            
        }
        else
        {
            
        }
    }

    public bool CanLoseOnLevel(int levelNum)
    {
        return levelNum != 1 && levelNum % 5 != 0;
    }


}
