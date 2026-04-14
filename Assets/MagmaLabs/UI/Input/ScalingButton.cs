using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[DisallowMultipleComponent]
[AddComponentMenu("UI/Scaling Button")]
public class ScalingButton : Button
{
    [Header("Text Source")]
    [SerializeField] private bool autoFindText = true;
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private Text legacyText;

    [Header("Sizing")]
    [SerializeField] private bool scalingEnabled = true;
    [SerializeField] private Vector2 padding = new Vector2(24f, 12f);
    [SerializeField] private Vector2 minSize = new Vector2(80f, 36f);
    [SerializeField] private Vector2 maxSize = new Vector2(10000f, 10000f);
    [SerializeField] private bool snapToMultiples = false;
    [SerializeField] private Vector2 snapMultiples = new Vector2(8f, 8f);
    [SerializeField] private bool resizeInPlayMode = true;

    private RectTransform cachedRectTransform;
    private string lastTextHash;
    private Vector2 lastAppliedSize = Vector2.negativeInfinity;

    protected override void Awake()
    {
        base.Awake();
        CacheReferences();
        TryResize();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        CacheReferences();
        TryResize();
    }

    protected void OnValidate()
    {
        CacheReferences();
        TryResize(force: true);
    }

    private void Reset()
    {
        CacheReferences();
        TryResize(force: true);
    }

    private void Update()
    {
        if (Application.isPlaying && !resizeInPlayMode)
        {
            return;
        }

        if (HasTextStateChanged())
        {
            TryResize();
        }
    }

    public void RefreshSize()
    {
        CacheReferences();
        TryResize(force: true);
    }

    public void SetScalingEnabled(bool enabled)
    {
        scalingEnabled = enabled;

        if (scalingEnabled)
        {
            TryResize(force: true);
        }
    }

    private void CacheReferences()
    {
        if (cachedRectTransform == null)
        {
            cachedRectTransform = transform as RectTransform;
        }

        if (!autoFindText)
        {
            return;
        }

        if (tmpText == null)
        {
            tmpText = GetComponentInChildren<TMP_Text>(true);
        }

        if (legacyText == null)
        {
            legacyText = GetComponentInChildren<Text>(true);
        }
    }

    private bool HasTextStateChanged()
    {
        string stateHash = BuildTextStateHash();
        if (stateHash == lastTextHash)
        {
            return false;
        }

        lastTextHash = stateHash;
        return true;
    }

    private string BuildTextStateHash()
    {
        if (tmpText != null)
        {
            return "TMP|" + tmpText.text + "|" + tmpText.fontSize + "|" + tmpText.enableAutoSizing;
        }

        if (legacyText != null)
        {
            return "UI|" + legacyText.text + "|" + legacyText.fontSize + "|" + legacyText.resizeTextForBestFit;
        }

        return "NONE";
    }

    private void TryResize(bool force = false)
    {
        if (cachedRectTransform == null)
        {
            return;
        }

        if (!scalingEnabled)
        {
            return;
        }

        if (Application.isPlaying && !resizeInPlayMode)
        {
            return;
        }

        if (!TryGetPreferredTextSize(out Vector2 preferred))
        {
            return;
        }

        Vector2 targetSize = preferred + padding;
        targetSize.x = Mathf.Clamp(targetSize.x, minSize.x, maxSize.x);
        targetSize.y = Mathf.Clamp(targetSize.y, minSize.y, maxSize.y);

        if (snapToMultiples)
        {
            targetSize.x = SnapUp(targetSize.x, snapMultiples.x);
            targetSize.y = SnapUp(targetSize.y, snapMultiples.y);
            targetSize.x = Mathf.Clamp(targetSize.x, minSize.x, maxSize.x);
            targetSize.y = Mathf.Clamp(targetSize.y, minSize.y, maxSize.y);
        }

        if (!force && targetSize == lastAppliedSize)
        {
            return;
        }

        cachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetSize.x);
        cachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetSize.y);
        lastAppliedSize = targetSize;
    }

    private bool TryGetPreferredTextSize(out Vector2 preferred)
    {
        if (tmpText != null)
        {
            preferred = tmpText.GetPreferredValues(tmpText.text);
            return true;
        }

        if (legacyText != null)
        {
            TextGenerationSettings settings = legacyText.GetGenerationSettings(Vector2.zero);
            float width = legacyText.cachedTextGeneratorForLayout.GetPreferredWidth(legacyText.text, settings) / legacyText.pixelsPerUnit;
            float height = legacyText.cachedTextGeneratorForLayout.GetPreferredHeight(legacyText.text, settings) / legacyText.pixelsPerUnit;
            preferred = new Vector2(width, height);
            return true;
        }

        preferred = Vector2.zero;
        return false;
    }

    private static float SnapUp(float value, float multiple)
    {
        if (multiple <= 0f)
        {
            return value;
        }

        return Mathf.Ceil(value / multiple) * multiple;
    }
}
