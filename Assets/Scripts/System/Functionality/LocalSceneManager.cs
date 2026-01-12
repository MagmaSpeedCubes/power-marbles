using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalSceneManager : MonoBehaviour
{
    public static LocalSceneManager instance;


    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
