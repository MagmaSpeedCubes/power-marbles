using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
public class ImageInfographic : Infographic
{

    [SerializeField] protected List<NumberedSprite> infoSprites = new List<NumberedSprite>();
    [SerializeField] protected TextMeshProUGUI subtitle;
    [SerializeField] protected Tag[] roundingPrecision;



    override protected void UpdateInfo(float oldValue)
    {

        int spriteIndex = 0;
        foreach (NumberedSprite spr in infoSprites)
        {

            if (value >= spr.beginValue && value <= spr.endValue)
            {

                GetComponent<Image>().sprite = spr.sprite;
                spriteIndex = infoSprites.IndexOf(spr);
                break;
            }
        }
        // Debug.Log("Value" + value);
        if (subtitle != null)
        {
            subtitle.text = infoSprites[spriteIndex].name;

        }
        
        
    }
}
