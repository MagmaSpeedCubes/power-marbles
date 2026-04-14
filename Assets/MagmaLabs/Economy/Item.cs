using UnityEngine;

using MagmaLabs;
using MagmaLabs.Economy;

[CreateAssetMenu(fileName = "Item", menuName = "Economy/Item", order = 1)]

public class Item : SavableBase
{
    public SerializableDictionary<Sprite> sprites; 

    // Constructor for creating new instances
    public Item()
    {
        sprites = new SerializableDictionary<Sprite>();
        tags = new SerializableDictionary<string>();
    }

    /// <summary>
    /// Gets the default sprite (useful for backward compatibility)
    /// </summary>
    public Sprite DefaultSprite
    {
        get
        {
            if (sprites == null || sprites.Count == 0)
                return null;
            
            // Try common keys first
            if (sprites.ContainsKey("default")) return sprites["default"];
            if (sprites.ContainsKey("main")) return sprites["main"];
            if (sprites.ContainsKey("icon")) return sprites["icon"];
            
            // Return first sprite if no common keys found
            return sprites.Items[0].value;
        }
        set
        {
            if (sprites == null)
                sprites = new SerializableDictionary<Sprite>();
            sprites.Set("default", value);
        }
    }

    /// <summary>
    /// Checks if the item has any sprites
    /// </summary>
    public bool HasSprites => sprites != null && sprites.Count > 0;

    public override string ToString()
    {
        // Format: id|name|spritesSerialized|tagsSerialized
        
        // Serialize sprites dictionary
        string spritesData = "";
        if (sprites != null && sprites.Count > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var kvp in sprites.Items)
            {
                if (sb.Length > 0)
                    sb.Append(";");
                
                string spritePath = "";
                if (kvp.value != null)
                {
#if UNITY_EDITOR
                    spritePath = UnityEditor.AssetDatabase.GetAssetPath(kvp.value);
#else
                    // In build, assume sprites are in Resources folder
                    spritePath = kvp.value.name;
#endif
                }
                sb.Append(kvp.key).Append("=").Append(spritePath);
            }
            spritesData = sb.ToString();
        }

        string serializedTags = tags != null ? tags.ToString() : "";
        
        return $"{id}|{name}|{spritesData}|{serializedTags}";
    }

    public override void FromString(string serialized)
    {
        if (string.IsNullOrEmpty(serialized))
            return;

        string[] parts = serialized.Split('|');
        if (parts.Length >= 4)
        {
            id = parts[0];
            name = parts[1];
            
            // Deserialize sprites
            string spritesData = parts[2];
            if (sprites == null)
                sprites = new SerializableDictionary<Sprite>();
            else
                sprites.Clear();
                
            if (!string.IsNullOrEmpty(spritesData))
            {
                string[] spritePairs = spritesData.Split(';');
                foreach (string pair in spritePairs)
                {
                    if (string.IsNullOrWhiteSpace(pair))
                        continue;

                    string[] keyValue = pair.Split('=');
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string spritePath = keyValue[1].Trim();
                        
                        if (!string.IsNullOrEmpty(spritePath))
                        {
                            Sprite loadedSprite = null;
#if UNITY_EDITOR
                            loadedSprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                            Debug.LogWarning("Loaded sprite from editor database");
#else
                            // Load from Resources - adjust path as needed
                            loadedSprite = Resources.Load<Sprite>(spritePath);
#endif
                            if (loadedSprite != null)
                            {
                                sprites.Set(key, loadedSprite);
                            }
                        }
                    }
                }
            }
            
            string tagsData = parts[3];
            if (tags == null)
                tags = new SerializableDictionary<string>();
            else
                tags.Clear();
                
            if (!string.IsNullOrEmpty(tagsData))
            {
                tags = SerializableDictionary<string>.FromString(tagsData);
            }
        }
    }

    /// <summary>
    /// Creates a copy of this item as a new ScriptableObject instance
    /// </summary>
    public Item Copy()
    {
        Item newItem = CreateInstance<Item>();
        newItem.id = this.id;
        newItem.name = this.name;
        
        // Deep copy the sprites dictionary
        newItem.sprites = new SerializableDictionary<Sprite>();
        if (this.sprites != null)
        {
            foreach (var kvp in this.sprites.Items)
            {
                newItem.sprites.Set(kvp.key, kvp.value);
            }
        }
        
        // Deep copy the tags dictionary
        newItem.tags = new SerializableDictionary<string>();
        if (this.tags != null)
        {
            foreach (var kvp in this.tags.Items)
            {
                newItem.tags.Set(kvp.key, kvp.value);
            }
        }
        
        return newItem;
    }

    /// <summary>
    /// Creates a copy with a new ID
    /// </summary>
    public Item Copy(string newId)
    {
        Item newItem = Copy();
        newItem.id = newId;
        return newItem;
    }
}
