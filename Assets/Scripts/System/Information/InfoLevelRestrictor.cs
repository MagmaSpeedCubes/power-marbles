using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
public class InfoLevelRestrictor : MonoBehaviour
{
    [Tooltip("The info level values that something will be shown for")]
    public List<string> showValues = new List<string>();

    private CanvasGroup parent;

    void Awake()
    {
        parent = GetComponent<CanvasGroup>();

    }

    void Update()
    {
        string infoLevel = ProfileCustomization.infoLevel.ToString();

        if (showValues.IndexOf(infoLevel)==-1)
        {
            parent.alpha = 0f;
        }
        else
        {
            parent.alpha = 1f;
        }

    }
    
}

