using UnityEngine;
using System;
[CreateAssetMenu(menuName = "LootTable")]


public class LootTable : ScriptableObject
{
    public string name;
    public Loot[] items;
    public Ownable GetLoot()
    {
        float totalProb = 0f;
        foreach (var loot in items)
        {

            totalProb += loot.probability;
        }

        float roll = UnityEngine.Random.Range(0f, totalProb);
        float cumulative = 0f;
        foreach (var loot in items)
        {
            cumulative += loot.probability;
            if (roll <= cumulative)
            {
                return loot.item;
            }
        }

        throw new Exception("LootTable GetLoot failed to return an item");
    }
}
[System.Serializable]
public struct Loot
{
    public Ownable item;
    public float probability;
}