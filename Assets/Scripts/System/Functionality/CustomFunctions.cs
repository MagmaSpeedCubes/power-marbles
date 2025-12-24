using UnityEngine;

public class CustomFunctions : MonoBehaviour
{
    
    public static float EaseInOutCubic(float t)
    {
        if (t < 0.5) return InCubic(t * 2) / 2;
        return 1 - InCubic((1 - t) * 2) / 2;
    }
    public static float InCubic(float t) => t * t * t;

}
