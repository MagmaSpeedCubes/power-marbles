using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;

public class CardDeckManager : TiledElementManager
{

    public List<Ball> ballPrefabs;
    [SerializeField] private TextMeshProUGUI dropdownText;
    [SerializeField] private GameObject scroller;

    



    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DoubleLateStart());
    }

    IEnumerator DoubleLateStart(){
        yield return new WaitForSeconds(0.1f);

        
        Instantiate();
    }

    override public void Instantiate()
    {
        
        ballPrefabs = OwnableManager.instance.GetOwnedMarblePrefabs();
        numElements = ballPrefabs.Count;
        base.Instantiate();
        // Ensure we don't access out of bounds; use the minimum of items and ballPrefabs count
        int count = Mathf.Min(items.Count, ballPrefabs.Count);
        
        
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
            ballPrefabs = ballPrefabs.OrderBy(b => b.name).ToList();
        }else{
            ballPrefabs = ballPrefabs.OrderByDescending(b => b.name).ToList();
        }


        
    }



    void SortByCost(bool ascending)
    {
        if(!ascending){
            ballPrefabs = ballPrefabs.OrderBy(b => b.price).ToList();
        }else{
            ballPrefabs = ballPrefabs.OrderByDescending(b => b.price).ToList();
        }

    }

    void SortByRarity(bool ascending)
    {
        if (!ascending)
        {
            ballPrefabs = ballPrefabs.OrderBy(b => GetRarity(b)).ToList();
        }
        else
        {
            ballPrefabs = ballPrefabs.OrderByDescending(b => GetRarity(b)).ToList();
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
        // Refresh the UI without re-fetching from OwnableManager
        RefreshCardDisplay();
    }

    private void RefreshCardDisplay()
    {
        int count = Mathf.Min(items.Count, ballPrefabs.Count);
        for(int i = 0; i < count; i++)
        {
            CardHandler cm = items[i].GetComponent<CardHandler>();
            Ball ballToAssign = ballPrefabs[i];
            if (ballToAssign != null)
            {
                ballToAssign.InitializeValues();
                cm.subject = ballToAssign;
                cm.RefreshImage();
            }
        }
    }



}
