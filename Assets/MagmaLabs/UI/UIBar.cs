using UnityEngine;
using UnityEngine.UI;

namespace MagmaLabs.UI{
    public class UIBar : InfographicBaseEnhanced
    {
        [SerializeField]private RectTransform fillRect;
        [SerializeField]private RectTransform backgroundRect;
        public override void Refresh()
        {
            //float percentage = valueRange.GetPercentage(currentValue);
            //fillRect.localScale = new Vector3(percentage, 1f, 1f);
        }

        public override void SetColor(Color color)
        {
            fillRect.GetComponent<Image>().color = color;
        }

    }
}
