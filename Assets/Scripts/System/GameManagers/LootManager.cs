using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LootManager : MonoBehaviour
{
    public Ownable[] lootArray;
    public float[] dropChances;
    float totalDropChance;
    void Awake()
    {
        lootArray = lootArray.OrderByDescending(b => float.Parse(b.FindTag("powerTowerDropChance"))).ToArray();
        foreach(Ownable loot in lootArray)
        {
            totalDropChance += float.Parse(loot.FindTag("powerTowerDropChance"));
        }

    }

    void Start()
    {
        //TestLootDrop();
    }

    public Ownable GetLootDrop(float luckMultiplier)
    {
        System.Random rng = new System.Random();
        float hammer = (float)(rng.NextDouble() * totalDropChance * luckMultiplier);
        for(int i=0; i<lootArray.Length; i++)
        {
            //check every loot item starting with most common
            hammer -= float.Parse(lootArray[i].FindTag("powerTowerDropChance"));
            if(hammer <= 0)
            {
                return lootArray[i];
                
            }
        }
        return lootArray[lootArray.Length - 1];
        //return the most common item
    }

    

    void TestLootDrop()
    {
        float luckMultiplier = 1f;
        int numTestsToRun = 300;
        for(int i=0; i<numTestsToRun; i++)
        {
            Debug.Log("Loot Test #" + (i+1) + ": " + GetLootDrop(luckMultiplier).Serialize());
        }
    }
}
