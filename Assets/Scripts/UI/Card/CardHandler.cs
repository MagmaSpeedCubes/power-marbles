using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
public class CardHandler : UIElementHandler
{
    public Ball subject;
    [SerializeField] private TextMeshProUGUI cardTitle;
    [SerializeField] private TextMeshProUGUI cardCost;
    [SerializeField] private GameObject marbleImage;

    

    private CardDeckManager cdm;
    void Awake()
    {
        originalScale = transform.localScale;
        cdm = UnityEngine.Object.FindFirstObjectByType<CardDeckManager>();
        
        normal = cdm.normal;
        highlighted = cdm.highlighted;
        pressed = cdm.pressed;
        selected = cdm.selected;
        disabled = cdm.disabled;

    }

    void Start()
    {
        
        
        if(marbleImage.GetComponent<UIElementHandler>().type != UIElement.imageElement)
        {
            throw new Exception("UIElement is not defined as UIElement.imageElement so cannot modify image");
        }
        else
        {
            Image subjectSprite = marbleImage.GetComponent<Image>();
            subjectSprite.sprite = subject.mainSprite;
            subjectSprite.color = subject.spriteColor;
            cardTitle.text = subject.name;
            cardCost.text = ""+subject.price;
        }

    }

    public void OnClick()
    {
        LevelStats.selectedBall = subject;
        cdm.RefreshColors();
        GetComponent<Image>().color = cdm.pressed;
        
        AudioManager.instance.PlaySound(subject.bounceSound, ProfileCustomization.uiVolume*ProfileCustomization.masterVolume);
    }

    override public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        if(LevelStats.selectedBall != subject)
        {
            GetComponent<Image>().color = normal;
        }
        else
        {
            GetComponent<Image>().color = selected;
        }
        
    }
}
