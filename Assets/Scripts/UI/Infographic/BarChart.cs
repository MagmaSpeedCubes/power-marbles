using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public class BarChart : Infographic
{
    [SerializeField] protected Image beginSegment, scaleSegment, endSegment;
    [SerializeField] protected float minValue, maxValue;
    [SerializeField] protected float textObject;
    protected Vector2 startPosition, endPosition;
    protected float maxDistance, angle;

    void Awake()
    {
        type = "Bar";
        startPosition = beginSegment.transform.position;
        endPosition = endSegment.transform.position;
        float x = endPosition.x - startPosition.x; 
        float y = endPosition.y - startPosition.y;
        maxDistance = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
        angle = Mathf.Atan(y / x);
    }
    protected override void UpdateInfo(float oldValue)
    {
        float roundingPrecision = GetRoundingPrecision();
        base.UpdateInfo(oldValue);
        float roundedValue = Mathf.Floor(value / roundingPrecision + 0.5f) * roundingPrecision;
        //animateToValue(oldValue, roundedValue, 0.5f);

        
    }

    

}