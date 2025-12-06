using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[System.Serializable]
public enum UIElement
{
    background,
    foreground,
    imageElement,
    textElement,
    inputElement,
    canvasBackground
}
public class UIElementHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] protected GameObject element;



    public UIElement type;
    protected Vector3 originalScale;

    [SerializeField] protected bool expandOnHover = false;
    [ShowIf("expandOnHover", true)]
    [SerializeField] protected float hoverExpandAmount = 0f;
    [SerializeField] protected bool changeColorOnHover = false;

    [ShowIf("changeColorOnHover", true)]
    [SerializeField] protected bool useDefaultColors = true;
    [ShowIf("useDefaultColors", false)]
    [SerializeField] protected Color normal, highlighted, pressed, selected, disabled;
    //[ShowIf("useDefaultColors", false)]
    //[SerializeField] protected Color backgroundColor, foregroundColor, subjectColor;
        
    void Start()
    {
        originalScale = transform.localScale;

        if (useDefaultColors)
        {
            normal = ProfileCustomization.instance.normal;
            highlighted = ProfileCustomization.instance.highlighted;
            pressed = ProfileCustomization.instance.pressed;
            selected = ProfileCustomization.instance.selected;
            disabled = ProfileCustomization.instance.disabled;
        }
    }
    virtual public void OnPointerEnter(PointerEventData eventData)
    {
            
            
        if (expandOnHover)
        {
            transform.localScale = originalScale * (1 + hoverExpandAmount);
        }
        if (changeColorOnHover)
        {
            element.GetComponent<Image>().color = highlighted;
        }

    }

    virtual public void OnPointerExit(PointerEventData eventData)
    {
        if (expandOnHover)
        {
            transform.localScale = originalScale;
        }
        if (changeColorOnHover)
        {
            element.GetComponent<Image>().color = normal;
        }
    }


}
