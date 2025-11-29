using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardManager : MonoBehaviour
{
    public Ball subject;
    [SerializeField] protected Image cardImage;
    [SerializeField] protected TextMeshProUGUI cardTitle;
    [SerializeField] protected TextMeshProUGUI cardCost;

    void Update()
    {
        cardImage.sprite = subject.sprite;
        cardTitle.text = subject.name;
        cardCost.text = ""+subject.price;
    }

    public void OnClick()
    {
        LevelStats.selectedBall = subject;
        AudioManager.instance.PlaySound(subject.bounceSound, ProfileCustomization.uiVolume*ProfileCustomization.masterVolume);
    }
}
