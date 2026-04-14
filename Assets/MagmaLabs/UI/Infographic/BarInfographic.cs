using UnityEngine;

namespace MagmaLabs.UI{
    public class BarInfographic : InfographicBase{
        [SerializeField]protected RectTransform barScale;
        private float fullWidth;

        void Awake()
        {
            fullWidth = barScale.rect.width;
        }
        public override void Refresh()
        {
            barScale.sizeDelta = new Vector2(GetPercentage()*fullWidth, barScale.rect.height);
        }

    }
}
