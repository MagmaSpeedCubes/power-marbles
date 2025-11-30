using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIElementManager : MonoBehaviour
{
    [SerializeField] protected GameObject backgroundImage, foregroundImage, subjectImage;
    [SerializeField] protected float hoverExpandAmount;
    protected Vector3 originalScale;

    [SerializeField] protected Color normal, highlighted, pressed, selected, disabled;
        
    void Awake()
    {
        originalScale = transform.localScale;
    }
    virtual public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverExpandAmount;
        GetComponent<Image>().color = highlighted;
    }

    virtual public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        GetComponent<Image>().color = normal;
    }


}
