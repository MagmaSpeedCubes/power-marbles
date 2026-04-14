using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MagmaLabs.UI{
    public class TextInfographic : InfographicBase
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private string prefix, suffix;
        public override void SetColor(Color color)
        {
            text.color = color;
        }
        public override void Refresh()
        {
            text.text = prefix+currentValue+suffix;
        }

        public virtual void SetPrefix(string newPrefix){
            prefix = newPrefix;
        }

        public virtual void SetSuffix(string newSuffix)
        {
            suffix = newSuffix;
        }
    }
}
