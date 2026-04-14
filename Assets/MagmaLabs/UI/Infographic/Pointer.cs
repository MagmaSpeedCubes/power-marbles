using UnityEngine;
using UnityEngine.UI;
using MagmaLabs;
namespace MagmaLabs.UI{
    public class Pointer : InfographicBaseEnhanced
    {
        [SerializeField] private Image pointerImage;
        [SerializeField] private Sprite[] firstQuadrantArrows;


        void Start()
        {
            SetRange(0, 360);
        }

        override public void Refresh()
        {
            float quarterRotation = 90;
            float angleInQuadrant = currentValue % (quarterRotation);
            
            int indexInQuadrant = (int) ((1 - angleInQuadrant / quarterRotation) * firstQuadrantArrows.Length);
            //Debug.Log(indexInQuadrant);
            pointerImage.sprite = firstQuadrantArrows[indexInQuadrant];//individual rotated sprites for pixel consistency

            int quadrant = Mathf.FloorToInt(currentValue / quarterRotation);
            pointerImage.transform.rotation = Quaternion.Euler(0, 0, quadrant*90);//rotate in intervals of 90 to reuse sprites
            
        }

        override public void SetColor(Color color)
        {
            pointerImage.color = color;
        }



        
    }
}
