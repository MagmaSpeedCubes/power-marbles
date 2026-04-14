using UnityEngine;
namespace MagmaLabs{

public static class Numerics
{

    public static bool InBound(float value, float target, float tolerance)
        {
            float min = target - tolerance;
            float max = target + tolerance;
            return value>=min&&value<=max;
        }
    public static int Fibonacci(int n)
    {
        if (n <= 0) return 0;
        if (n == 1) return 1;
        int a = 0, b = 1, c = 1;
        for (int i = 2; i <= n; i++)
        {
            c = a + b;
            a = b;
            b = c;
        }
        return c;
    }

    public static float EaseInOutCubic(float t)
    {
        if (t < 0.5) return InCubic(t * 2) / 2;
        return 1 - InCubic((1 - t) * 2) / 2;
    }
    public static float InCubic(float t) => t * t * t;

    public static float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }
}
}
