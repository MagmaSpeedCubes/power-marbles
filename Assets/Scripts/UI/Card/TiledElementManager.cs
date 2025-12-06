using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TiledElementManager : MonoBehaviour
{
    [SerializeField] protected Vector2 beginPosition;
    [SerializeField] protected GameObject elementPrefab;
    [SerializeField] protected int numElements;
    [SerializeField] protected GameObject parentObject;
    [SerializeField] protected float separationDistance;
    
    [SerializeField] protected int itemsPerRow;
    
    [SerializeField] protected bool useDefaultColors;
    [ShowIf("useDefaultColors", false)]
    public Color normal, highlighted, pressed, selected, disabled;
    protected List<GameObject> items = new List<GameObject>();



    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Instantiate();
    }

    virtual public void Instantiate()
    {
        if (useDefaultColors)
        {
            normal = ProfileCustomization.instance.normal;
            highlighted = ProfileCustomization.instance.highlighted;
            pressed = ProfileCustomization.instance.pressed;
            selected = ProfileCustomization.instance.selected;
            disabled = ProfileCustomization.instance.disabled;
        }

        if(itemsPerRow <= 0)
        {
            throw new ArithmeticException("Items per row needs to be positive");
        }

        for(int i=0; i<numElements; i++)
        {
            //Debug.Log("Creating new item");
            RectTransform prefabRect = elementPrefab.GetComponent<RectTransform>();
            Vector2 offset = new Vector2(
                (i % itemsPerRow) * (prefabRect.rect.width + separationDistance),
                (i / itemsPerRow) * (prefabRect.rect.height + separationDistance)
            );
            
            // Instantiate as child of parentObject to preserve layout and dimensions
            GameObject newItem = Instantiate(elementPrefab, parentObject.transform);
            items.Add(newItem);
            RectTransform newItemRect = newItem.GetComponent<RectTransform>();
            
            // Set anchored position relative to parent
            newItemRect.anchoredPosition = beginPosition + offset;
            
        }

    }

    virtual public void Reinitialize(){
        foreach(GameObject item in items){
            Destroy(item);
        }
        Instantiate();
    }

    virtual public void RefreshColors(){
        foreach(GameObject item in items){
            item.GetComponent<Image>().color = normal;
        }
    }


}
