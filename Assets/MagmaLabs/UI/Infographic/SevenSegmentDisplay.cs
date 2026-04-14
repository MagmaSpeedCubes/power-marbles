using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagmaLabs;

namespace MagmaLabs.UI{
    [System.Serializable]
    public class SevenSegmentDisplay : InfographicBaseEnhanced
    {

        void Start()
        {
            base.SetRange(0, 9);
        }
        [SerializeField] private GameObject top, topRight, bottomRight, bottom, bottomLeft, topLeft, middle;
        [SerializeField] private Dictionary<int, bool[]> digitSegments = new Dictionary<int, bool[]>()
        {
            {0, new bool[]{true, true, true, true, true, true, false}},
            {1, new bool[]{false, true, true, false, false, false, false}},
            {2, new bool[]{true, true, false, true, true, false, true}},
            {3, new bool[]{true, true, true, true, false, false, true}},
            {4, new bool[]{false, true, true, false, false, true, true}},
            {5, new bool[]{true, false, true, true, false, true, true}},
            {6, new bool[]{true, false, true, true, true, true, true}},
            {7, new bool[]{true, true, true, false, false, false, false}},
            {8,new bool[]{true,true,true,true,true,true,true}},
            {9,new bool[]{true,true,true,true,false,true,true}}
        };

        public override void Refresh()
        {
            int digit = Mathf.Clamp(Mathf.RoundToInt(currentValue), 0, 9);
            bool[] segments = digitSegments[digit];

            top.SetActive(segments[0]);
            topRight.SetActive(segments[1]);
            bottomRight.SetActive(segments[2]);
            bottom.SetActive(segments[3]);
            bottomLeft.SetActive(segments[4]);
            topLeft.SetActive(segments[5]);
            middle.SetActive(segments[6]);
        }
        public override void SetValue(float value)
        {
            currentValue = Mathf.Clamp(value, 0, 9);
            Refresh();
        }

        public override void SetRange(float min, float max)
        {
            return; // Range is fixed for 0-9
        }

        public override void SetColor(Color color)
        {
            foreach (var segment in new GameObject[] { top, topRight, bottomRight, bottom, bottomLeft, topLeft, middle })
            {
                Image img = segment.GetComponent<Image>();
                if (img != null){
                    img.color = color; 
                }
            }
        }
    }
}
