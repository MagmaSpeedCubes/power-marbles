using UnityEngine;
using TMPro;
using System;
using MagmaLabs.Economy.Security;
using MagmaLabs.UI;
public class LevelSelectManager : TiledElementManager
{
    [SerializeField] private NumberText efficiencyDisplay;
    [SerializeField] private int MAX_LEVELS = 1000;

    void Start()
    {
        
        Instantiate();
    }

    override public void Instantiate()
    {
        numElements = MAX_LEVELS;
        base.Instantiate();
        for(int i=1; i<=items.Count; i++)
        {
            if(i > MAX_LEVELS)
            {
                Destroy(items[i-1]);
            }
            else
            {
                GameObject item = items[i-1];
                GameObject textObject = item.transform.Find("LevelButtonText").gameObject;
                TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
                text.text = "" + i;
                text.fontSize = (float) (i==1 ? 200 : (200/Math.Ceiling(Math.Log10(i+1))));
            }



        }
    }

    // Update is called once per frame
    void Update()
    {
        efficiencyDisplay.SetValue(SecureProfileStats.instance.GetEfficiencyScore());
    }
}
