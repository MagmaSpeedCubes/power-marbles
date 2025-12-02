using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIElementHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] protected GameObject backgroundGameObject, foregroundGameObject, subjectGameObject;
    [SerializeField] protected float hoverExpandAmount;
    protected Vector3 originalScale;

    
    [SerializeField] protected bool useDefaultColors;
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
        transform.localScale = originalScale * (1 + hoverExpandAmount);
        backgroundGameObject.GetComponent<Image>().color = highlighted;
    }

    virtual public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        backgroundGameObject.GetComponent<Image>().color = normal;
    }


}
