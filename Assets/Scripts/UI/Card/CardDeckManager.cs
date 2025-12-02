using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CardDeckManager : MonoBehaviour
{
    [SerializeField] private Vector2 beginPosition;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Ball[] ballPrefabs;
    [SerializeField] private GameObject cardParent;
    [SerializeField] private float separationDistance;
    
    [SerializeField] private int cardsPerRow;
    [SerializeField] private TextMeshProUGUI dropdownText;
    
    [SerializeField] protected bool useDefaultColors;
    [ShowIf("useDefaultColors", false)]
    public Color normal, highlighted, pressed, selected, disabled;
    private List<GameObject> cards = new List<GameObject>();



    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        if (useDefaultColors)
        {
            normal = ProfileCustomization.instance.normal;
            highlighted = ProfileCustomization.instance.highlighted;
            pressed = ProfileCustomization.instance.pressed;
            selected = ProfileCustomization.instance.selected;
            disabled = ProfileCustomization.instance.disabled;
        }

        if(cardsPerRow <= 0)
        {
            throw new ArithmeticException("Cards per row needs to be positive");
        }

        for(int i=0; i<ballPrefabs.Length; i++)
        {
            //Debug.Log("Creating new card");
            RectTransform prefabRect = cardPrefab.GetComponent<RectTransform>();
            Vector2 offset = new Vector2(
                (i % cardsPerRow) * (prefabRect.rect.width + separationDistance),
                (i / cardsPerRow) * (prefabRect.rect.height + separationDistance)
            );
            
            // Instantiate as child of cardParent to preserve layout and dimensions
            GameObject newCard = Instantiate(cardPrefab, cardParent.transform);
            cards.Add(newCard);
            RectTransform newCardRect = newCard.GetComponent<RectTransform>();
            
            // Set anchored position relative to parent
            newCardRect.anchoredPosition = beginPosition + offset;
            
            CardHandler cm = newCard.GetComponent<CardHandler>();
            
            // Initialize the Ball prefab's values immediately before assignment
            Ball ballToAssign = ballPrefabs[i];
            if (ballToAssign != null)
            {
                ballToAssign.InitializeValues();
            }
            cm.subject = ballToAssign;
            //Debug.Log("New card created");
        }
        //Debug.Log("Created " + ballPrefabs.Length + " cards");
    }

    void Reinitialize(){
        foreach(GameObject card in cards){
            Destroy(card);
        }
        Initialize();
    }

    public void RefreshColors(){
        foreach(GameObject card in cards){
            card.GetComponent<Image>().color = normal;
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
        Reinitialize();
    }



}
