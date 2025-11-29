using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;
public class NumberText : Infographic
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected string prefix, suffix;

    void Awake()
    {
        type = "Text";
    }

    override protected void UpdateInfo(float oldValue)
    {
        base.UpdateInfo(oldValue);

        float roundingPrecision = GetRoundingPrecision();
        // Debug.Log("RP:" + roundingPrecision);
        float roundedValue = (int)Mathf.Round(value / roundingPrecision) * roundingPrecision;
        text.text = prefix + roundedValue + suffix;
        // animateToValue(oldValue, roundedValue, 0.5f);

    }

    protected void AnimateToValue(float oldValue, float newValue, float time)
    {
        
        StartCoroutine(animateToValueCoroutine(oldValue, newValue, time));
    }

    protected IEnumerator animateToValueCoroutine(float oldValue, float newValue, float time)
    {

        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            float progression = elapsedTime / time;
            float midValue = oldValue + (newValue - oldValue) * progression;
            float roundingPrecision = GetRoundingPrecision();
            float roundedMidValue = Mathf.Floor(midValue / roundingPrecision + 0.5f) * roundingPrecision;

            text.text = prefix + midValue + suffix;
            elapsedTime += Time.deltaTime;
        }
        




        yield return null;
    }



}

