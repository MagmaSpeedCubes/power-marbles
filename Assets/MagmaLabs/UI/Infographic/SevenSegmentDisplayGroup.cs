using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagmaLabs;

namespace MagmaLabs.UI{
    [System.Serializable]
    public class SevenSegmentDisplayGroup : InfographicBaseEnhanced
    {
        [Header("Seven Segment Displays")]
        [Header("Assign each display in order from left to right")]
        [SerializeField] private SevenSegmentDisplay[] displays;

        void Refresh()
        {
            foreach (var display in displays)
            {
                display.Refresh();
            }
        }

        public override void SetValue(float value)
        {

            currentValue = Mathf.Clamp(value, valueRange.min, valueRange.max);
            int intValue = Mathf.RoundToInt(currentValue);
            for (int i = 0; i < displays.Length; i++)
            {
                int digitValue = (intValue / (int)Mathf.Pow(10, displays.Length - 1 - i)) % 10;
                displays[i].SetValue(digitValue);
                Refresh();
            }
        }

        public override void SetColor(Color color)
        {
            foreach (var display in displays)
            {
                display.SetColor(color);
            }
            Refresh();
        }
        public override void SetRange(float min, float max)
        {
            valueRange = new Range<float>() { min = 0, max = Mathf.Pow(10, displays.Length) - 1}; 
            //max is capped by number of digits
        }

    }

}