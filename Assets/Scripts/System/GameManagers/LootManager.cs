using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LootManager : MonoBehaviour
{
    public LootTable[] lootTables;

    public LootManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of LootManager detected. Destroying duplicate.");
            Destroy(this.gameObject);
        }
    }

    public Ownable GetLootFromTable(string name)
    {
        LootTable table = GetLootTableFromName(name);
        return table.GetLoot();

    }

    public LootTable GetLootTableFromName(string name)
    {
        foreach(LootTable table in lootTables)
        {
            if (table.name.Equals(name))
            {
                return table;
            }
        }
        return null;
    }
}
