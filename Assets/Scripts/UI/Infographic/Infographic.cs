using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


[System.Serializable]
public class Infographic : MonoBehaviour
{

    [HideInInspector]public string type;
    protected float value;
    [SerializeField] protected List<Tag> roundingPrecisionList;

    public void SetValue(float newValue)
    {
        float oldValue = value;
        value = newValue;
        UpdateInfo(oldValue);
    }

    public float GetValue()
    {
        return value;
    }

    public void ChangeValue(float difference)
    {
        SetValue(value + difference);
    }

    virtual protected void UpdateInfo(float oldValue)
    {

    }

    protected float GetRoundingPrecision()
    {

        foreach (Tag rp in roundingPrecisionList)
        {
            if(rp.name.Equals(ProfileCustomization.infoLevel))
            {
                return float.Parse(rp.value);
            }
        }
        return 1;
    }
}

