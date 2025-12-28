using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning("Multiple instances of CameraManager detected. Destroying duplicate.");
            Destroy(this.gameObject);
        }
    }
}
