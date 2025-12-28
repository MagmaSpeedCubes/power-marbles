using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PrimaryHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI xpText, glassMarbleText;
    void Update()
    {
        xpText.text = "" + SecureProfileStats.instance.GetXP();
        glassMarbleText.text = "" + SecureProfileStats.instance.GetGlassMarbles();
    }
}
