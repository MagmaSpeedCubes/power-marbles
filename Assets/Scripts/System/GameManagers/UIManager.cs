using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate
            return;
        }
    }
}
