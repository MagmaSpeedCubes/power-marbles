using UnityEngine;

[System.Serializable]
public class Ball : MonoBehaviour
{

    [HideInInspector] public Sprite sprite;
    public BallPrefab prefab;
    public Ownable ownable;

    [HideInInspector]public float gravity;
    [HideInInspector]public float power;
    [HideInInspector]public float size;
    [HideInInspector]public float movementSpeed;
    [HideInInspector]public int price;

    [HideInInspector]public AudioClip bounceSound;
    [HideInInspector]public AudioClip specialSound;


    public GameObject ballPrefabObject;

    public Ball(BallPrefab pfb, Ownable own)
    {
        prefab = pfb;
        ownable = own;

    }
    void Start()
    {
        // Delegate to a reusable initializer so other code can initialize immediately
        InitializeValues();
    }

    // Call this to compute values immediately (useful when creating Balls at runtime)
    public void InitializeValues()
    {
        if (prefab == null)
        {
            Debug.LogError("Ball prefab is null on " + this.name);
            return;
        }

        if (ownable == null)
        {
            Debug.LogWarning("Ownable is null for Ball '" + this.name + "' - using default multipliers");
        }

        float gravityMultiplier = GetMultiplier("gravityMultiplier", 1f);
        gravity = prefab.gravity * gravityMultiplier;

        float powerMultiplier = GetMultiplier("powerMultiplier", 1f);
        power = prefab.power * powerMultiplier;

        float sizeMultiplier = GetMultiplier("sizeMultiplier", 1f);
        size = prefab.size * sizeMultiplier;

        float movementSpeedMultiplier = GetMultiplier("movementSpeedMultiplier", 1f);
        movementSpeed = prefab.movementSpeed * movementSpeedMultiplier;

        float priceMultiplier = GetMultiplier("priceMultiplier", 1f);

        price = Mathf.Max(0, (int)(prefab.price * priceMultiplier));

        sprite = ownable != null ? ownable.sprite : null;

        bounceSound = prefab.bounceSound;
        specialSound = prefab.specialSound;
    }

    private float GetMultiplier(string tagName, float defaultValue = 1f)
    {
        if (ownable == null)
        {
            return defaultValue;
        }

        string raw = ownable.FindTag(tagName);
        if (string.IsNullOrEmpty(raw))
        {
            Debug.LogWarning($"Tag '{tagName}' missing for '{this.name}' - using default {defaultValue}");
            return defaultValue;
        }

        float parsed;
        if (!float.TryParse(raw, out parsed))
        {
            Debug.LogWarning($"Tag '{tagName}' value '{raw}' is not a number for '{this.name}' - using default {defaultValue}");
            return defaultValue;
        }

        return parsed;
    }
}
