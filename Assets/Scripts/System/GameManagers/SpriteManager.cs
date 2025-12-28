using UnityEngine;

public class SpriteManager : MonoBehaviour
{
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
