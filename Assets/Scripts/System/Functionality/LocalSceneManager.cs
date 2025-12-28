using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalSceneManager : MonoBehaviour
{
    public static LocalSceneManager instance;
    [SerializeField]private GameObject ui;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(ui);
        }
        else
        {
            Debug.LogWarning("Multiple SceneManagers detected. Destroying duplicate.");
            Destroy(this.gameObject);
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
