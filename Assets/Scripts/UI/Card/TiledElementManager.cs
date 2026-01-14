using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using MagmaLabs.Utilities.Editor;

public class TiledElementManager : MonoBehaviour
{
    [SerializeField] protected Transform centerPosition;
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
        items = new List<GameObject>();
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

        // Position items centered around the provided centerPosition transform.
        RectTransform prefabRect = elementPrefab.GetComponent<RectTransform>();
        if (prefabRect == null)
        {
            throw new Exception("elementPrefab must have a RectTransform");
        }

        int totalRows = Mathf.CeilToInt((float)numElements / itemsPerRow);
        float cellWidth = prefabRect.rect.width + separationDistance;
        float cellHeight = prefabRect.rect.height + separationDistance;

        for (int i = 0; i < numElements; i++)
        {
            Debug.Log("Creating new item");

            int col = i % itemsPerRow;
            int row = i / itemsPerRow;

            // Determine how many columns are in this row (last row may be partial)
            int columnsInThisRow;
            if (row == totalRows - 1)
            {
                // Last row: may be partial
                columnsInThisRow = numElements - row * itemsPerRow;
                if (columnsInThisRow <= 0) columnsInThisRow = itemsPerRow; // full row
            }
            else
            {
                columnsInThisRow = itemsPerRow;
            }

            // Calculate centered offsets for this row: columns go left->right, rows top->bottom
            float xOffset = (col - (columnsInThisRow - 1) / 2f) * cellWidth;
            float yOffset = - (row - (totalRows - 1) / 2f) * cellHeight;

            // Instantiate as child of parentObject to preserve layout and dimensions
            GameObject newItem = Instantiate(elementPrefab, parentObject.transform);
            items.Add(newItem);
            RectTransform newItemRect = newItem.GetComponent<RectTransform>();

            Vector3 centerLocal = centerPosition != null ? centerPosition.localPosition : Vector3.zero;
            newItemRect.localPosition = centerLocal + new Vector3(xOffset, yOffset, 0f);
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
