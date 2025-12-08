using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CardDeckManager : TiledElementManager
{

    [SerializeField] private Ball[] ballPrefabs;
    [SerializeField] private TextMeshProUGUI dropdownText;
    



    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numElements = ballPrefabs.Length;
        Instantiate();
    }

    override public void Instantiate()
    {
        base.Instantiate();
        // Ensure we don't access out of bounds; use the minimum of items and ballPrefabs length
        int count = Mathf.Min(items.Count, ballPrefabs.Length);
        for(int i = 0; i < count; i++)
        {
            CardHandler cm = items[i].GetComponent<CardHandler>();
            
            // Initialize the Ball prefab's values immediately before assignment
            Ball ballToAssign = ballPrefabs[i];
            if (ballToAssign != null)
            {
                ballToAssign.InitializeValues();
            }
            cm.subject = ballToAssign;
        }
    }


    void SortByName(bool ascending)
    {
        if(ascending){
            ballPrefabs = ballPrefabs.OrderBy(b => b.name).ToArray();
        }else{
            ballPrefabs = ballPrefabs.OrderByDescending(b => b.name).ToArray();
        }


        
    }



    void SortByCost(bool ascending)
    {
        if(!ascending){
            ballPrefabs = ballPrefabs.OrderBy(b => b.price).ToArray();
        }else{
            ballPrefabs = ballPrefabs.OrderByDescending(b => b.price).ToArray();
        }

    }

    void SortByRarity(bool ascending)
    {
        if (!ascending)
        {
            ballPrefabs = ballPrefabs.OrderBy(b => GetRarity(b)).ToArray();
        }
        else
        {
            ballPrefabs = ballPrefabs.OrderByDescending(b => GetRarity(b)).ToArray();
        }

    }

    // Safely parse the rarity tag into the Rarity enum. Defaults to common if parsing fails.
    private Rarity GetRarity(Ball b)
    {
        if (b == null || b.ownable == null)
            return Rarity.common;

        string raw = b.ownable.FindTag("rarity");
        if (string.IsNullOrEmpty(raw))
            return Rarity.common;

        if (Enum.TryParse<Rarity>(raw, true, out var parsed))
            return parsed;

        // If the tag is numeric (e.g. stored as int), try parsing as int then convert
        if (int.TryParse(raw, out int intVal))
        {
            if (Enum.IsDefined(typeof(Rarity), intVal))
                return (Rarity)intVal;
        }

        Debug.LogWarning($"Could not parse rarity '{raw}' on '{b.name}', defaulting to 'common'.");
        return Rarity.common;
    }

    public void Sort(){
        string method = dropdownText.text;
        switch(method){
            case "Alphabetical":
                SortByName(true);
                break;
            case "Reverse Alphabetical":
                SortByName(false);
                break;
            case "Highest Cost":
                SortByCost(true);
                break;
            case "Lowest Cost":
                SortByCost(false);break;
            case "Highest Rarity":
                SortByRarity(true);
                break;
            case "Lowest Rarity":
                SortByRarity(false);
                break;
            default:
                Debug.LogError("No sorting method found for " + method);
                break;
        }
        base.Reinitialize();
    }



}
