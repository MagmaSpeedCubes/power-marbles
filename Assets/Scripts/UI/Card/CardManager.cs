using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class CardManager : UIElementManager
{
    public Ball subject;
    [SerializeField] private TextMeshProUGUI cardTitle;
    [SerializeField] private TextMeshProUGUI cardCost;

    private CardDeckManager cdm;
    void Awake()
    {
        originalScale = transform.localScale;
        cdm = Object.FindFirstObjectByType<CardDeckManager>();
        
        normal = cdm.normal;
        highlighted = cdm.highlighted;
        pressed = cdm.pressed;
        selected = cdm.selected;
        disabled = cdm.disabled;

    }

    void Start()
    {
        
        Image subjectSprite = subjectImage.GetComponent<Image>();
        subjectSprite.sprite = subject.sprite;
        subjectSprite.color = subject.spriteColor;
        cardTitle.text = subject.name;
        cardCost.text = ""+subject.price;
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
