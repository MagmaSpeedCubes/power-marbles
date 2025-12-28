using UnityEngine;
using TMPro;
using System.Collections;
using System;
public class SceneText : Infographic
{
    //this health bar is in the scene, not in the UI
    [SerializeField] protected TextMeshPro sceneText;
    [SerializeField] protected string prefix, suffix;

    void Awake()
    {
        type = "SceneText";
    }
    override protected void UpdateInfo(float oldValue)
    {
        float roundingPrecision = GetRoundingPrecision();
        base.UpdateInfo(oldValue);
        sceneText.text = prefix + Mathf.Floor(value / roundingPrecision + 0.5f) * roundingPrecision + suffix;
    }
}
