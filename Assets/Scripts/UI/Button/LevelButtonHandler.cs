using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LevelButtonHandler : UIElementHandler{
    public int levelNumber;
    public void OnClick()
    {
        GameObject textObject = transform.Find("LevelButtonText").gameObject;
        levelNumber = int.Parse(textObject.GetComponent<TextMeshProUGUI>().text);
        LevelManager.instance.LoadKingdomLevel(levelNumber);

        GameObject levelSelectCanvas = GameObject.Find("LevelSelect");
        levelSelectCanvas.GetComponent<Canvas>().enabled = false;
    }

    override public void OnPointerEnter(PointerEventData eventData)
    {
            
        int difficulty = (int)LevelGenerator.instance.GetLevelDifficulty(levelNumber);
        

        if (expandOnHover)
        {
            transform.localScale = originalScale * (1 + hoverExpandAmount);
        }
        if (changeColorOnHover)
        {
            element.GetComponent<Image>().color = highlighted;
        }

    }

    override public void OnPointerExit(PointerEventData eventData)
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
