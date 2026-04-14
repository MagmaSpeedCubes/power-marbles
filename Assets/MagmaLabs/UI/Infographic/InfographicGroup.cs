using UnityEngine;

namespace MagmaLabs.UI
{
    public class InfographicGroup : InfographicBase
    {
        [SerializeField]protected InfographicBase[] infographics;
        void OnEnable()
        {
            SetValue(currentValue);
            SetRange(valueRange.min, valueRange.max);
            Refresh();
        }
        public void SetValue(float value)
        {
            base.SetValue(value);
            foreach(InfographicBase ib in infographics)
            {
                ib.SetValue(value);
            }
        }
        public void SetRange(float min, float max)
        {
            base.SetRange(min, max);
            foreach(InfographicBase ib in infographics)
            {
                ib.SetRange(min, max);
            }
        }                  

        public void Refresh()
        {
            base.Refresh();
            foreach(InfographicBase ib in infographics)
            {
                ib.Refresh();
            }
        }

        public void SetColor(Color color)
        {
            base.SetColor(color);   
            foreach(InfographicBase ib in infographics)
            {
                ib.SetColor(color);
            }
        }                              

    }

}
