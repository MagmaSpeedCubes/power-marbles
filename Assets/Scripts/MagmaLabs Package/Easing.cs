using UnityEngine;

namespace MagmaLabs.Animation
{
    public class Easing : MonoBehaviour
    {

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
