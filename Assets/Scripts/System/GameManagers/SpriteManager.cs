using UnityEngine;

namespace MagmaLabs.Economy
{
    ///
public class SpriteManager : MonoBehaviour
{
    /// <summary>
    /// MonoBehaviour script that manages sprites used by <see cref="Ownable"/> objects
    /// </summary>
    [SerializeField]private Sprite[] sprites;
    public Sprite placeholder;
    public static SpriteManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Sprite GetSpriteByName(string name)
    {
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == name)
            {
                return sprite;
            }
        }
        return null;
    }
}
}
