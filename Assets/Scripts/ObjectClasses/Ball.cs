using UnityEngine;
using MagmaLabs.Economy;

[System.Serializable]
public class Ball : MonoBehaviour
{

    [HideInInspector] public Sprite mainSprite;
    [HideInInspector] public Sprite ballSprite;
    [HideInInspector] public Color spriteColor;
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

        float gravityMultiplier = GetFloat("gravityMultiplier", 1f);
        gravity = prefab.gravity * gravityMultiplier;

        float powerMultiplier = GetFloat("powerMultiplier", 1f);
        power = prefab.power * powerMultiplier;

        float sizeMultiplier = GetFloat("sizeMultiplier", 1f);
        size = prefab.size * sizeMultiplier;

        float movementSpeedMultiplier = GetFloat("movementSpeedMultiplier", 1f);
        movementSpeed = prefab.movementSpeed * movementSpeedMultiplier;

        float priceMultiplier = GetFloat("priceMultiplier", 1f);

        price = Mathf.Max(0, (int)(prefab.price * priceMultiplier));

        mainSprite = ownable != null ? ownable.sprite : null; 

        ballSprite = ProfileCustomization.instance.defaultBall;
        spriteColor = prefab.defaultColor;

        bounceSound = prefab.bounceSound;
        specialSound = prefab.specialSound;
    }



    public string GetTag(string tagName, string defaultValue = "")
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
        return raw;


    }

    public float GetFloat(string tagName, float defaultValue = 1f)
    {
        string raw = GetTag(tagName, defaultValue.ToString());
        if (float.TryParse(raw, out float result))
        {
            return result;
        }
        Debug.LogWarning($"Tag '{tagName}' not a valid float for '{this.name}' - using default {defaultValue}");
        return defaultValue;
    }
}
