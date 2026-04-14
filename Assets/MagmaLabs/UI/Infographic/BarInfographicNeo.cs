using UnityEngine;

namespace MagmaLabs.UI
{
    public class BarInfographicNeo : InfographicBase
    {
        [Header("Bar")]
        [SerializeField] private RectTransform barBounds;
        [SerializeField] private RectTransform barFill;

        [Header("Target")]
        [SerializeField] private RectTransform targetTransform;
        [SerializeField] private float targetValue;

        [Header("Text")]
        [SerializeField] private TextInfographic textInfographic;
        [SerializeField] private bool updateTextInfographic = true;
        [SerializeField] private string textPrefix = "";
        [SerializeField] private string textSuffix = "";
        [SerializeField, Min(0)] private int textPrecision = 2;

        [Header("Acceleration Note")]
        [SerializeField] private bool showAccelerationNote = false;
        [SerializeField] private string accelerationFormat = " ({0:+0.##;-0.##;0.##})";

        private float barWidth;
        private bool hasLastAccelerationSample;
        private float lastAccelerationValue;
        private float lastAccelerationDelta;

        private void Awake()
        {
            CacheBarWidth();
            ResetAccelerationTracking();
            Refresh();
        }

        private void OnEnable()
        {
            CacheBarWidth();
            Refresh();
        }

        private void OnValidate()
        {
            CacheBarWidth();
            Refresh();
        }

        private void OnRectTransformDimensionsChange()
        {
            CacheBarWidth();
            RefreshBar();
            RefreshTarget();
        }

        public override void SetValue(float value)
        {
            currentValue = value;
            Refresh();
        }

        public override void SetRange(float min, float max)
        {
            base.SetRange(min, max);
            Refresh();
        }

        public void SetTargetValue(float value)
        {
            targetValue = value;
            Refresh();
        }

        public void SetTextPrefix(string value)
        {
            textPrefix = value;
            RefreshText();
        }

        public void SetTextSuffix(string value)
        {
            textSuffix = value;
            RefreshText();
        }

        public void SetTextPrecision(int decimals)
        {
            textPrecision = Mathf.Max(-1, decimals);
            RefreshText();
        }

        public void SetShowAccelerationNote(bool enabled)
        {
            showAccelerationNote = enabled;
            if (enabled)
            {
                ResetAccelerationTracking();
            }
            RefreshText();
        }

        public void RefreshAcceleration()
        {
            if (!hasLastAccelerationSample)
            {
                ResetAccelerationTracking();
            }
            else
            {
                lastAccelerationDelta = currentValue - lastAccelerationValue;
                lastAccelerationValue = currentValue;
            }

            RefreshText();
        }

        public float GetTargetValue()
        {
            return targetValue;
        }

        public float GetDistanceToTarget()
        {
            return targetValue - currentValue;
        }

        public override void Refresh()
        {
            CacheBarWidth();
            RefreshBar();
            RefreshTarget();
            RefreshText();
        }

        private void RefreshBar()
        {
            if (barFill == null || barWidth <= 0f)
            {
                return;
            }

            float width = Mathf.Clamp01(GetPercentage()) * barWidth;
            barFill.sizeDelta = new Vector2(width, barFill.sizeDelta.y);

            if (barBounds != null)
            {
                float leftEdge = -barWidth * barBounds.pivot.x;
                Vector2 anchored = barFill.anchoredPosition;
                anchored.x = leftEdge + (width * barFill.pivot.x);
                barFill.anchoredPosition = anchored;
            }
        }

        private void RefreshTarget()
        {
            if (targetTransform == null || barBounds == null || barWidth <= 0f)
            {
                return;
            }

            float targetPercent = Mathf.Clamp01(GetPercentage(targetValue));
            float leftEdge = -barWidth * barBounds.pivot.x;
            float x = leftEdge + (barWidth * targetPercent);

            Vector2 anchored = targetTransform.anchoredPosition;
            anchored.x = x;
            targetTransform.anchoredPosition = anchored;
        }

        private void RefreshText()
        {
            if (!updateTextInfographic || textInfographic == null)
            {
                return;
            }

            textInfographic.SetPrefix(textPrefix);
            textInfographic.SetSuffix(textSuffix + BuildAccelerationNote());
            textInfographic.SetValue(GetRoundedValue(currentValue));
        }

        private string BuildAccelerationNote()
        {
            if (!showAccelerationNote)
            {
                return string.Empty;
            }

            return string.Format(accelerationFormat, lastAccelerationDelta);
        }

        private void CacheBarWidth()
        {
            if (barBounds == null && barFill != null)
            {
                barBounds = barFill;
            }

            if (barBounds == null)
            {
                barWidth = 0f;
                return;
            }

            barWidth = barBounds.rect.width;
        }

        private float GetPercentage(float value)
        {
            if (valueRange.max - valueRange.min == 0f)
            {
                return 0f;
            }

            return (value - valueRange.min) / (valueRange.max - valueRange.min);
        }

        private float GetRoundedValue(float value)
        {
            if (textPrecision < 0)
            {
                return value;
            }

            float pow = Mathf.Pow(10f, textPrecision);
            return Mathf.Round(value * pow) / pow;
        }

        private void ResetAccelerationTracking()
        {
            lastAccelerationValue = currentValue;
            lastAccelerationDelta = 0f;
            hasLastAccelerationSample = true;
        }
    }
}
