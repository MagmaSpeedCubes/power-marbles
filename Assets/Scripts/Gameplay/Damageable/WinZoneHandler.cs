using UnityEngine;

public class WinHandler : MonoBehaviour
{   void OnTriggerEnter2D(Collider2D other)
    {
        LevelManager.instance.EndLevel();
    }
}
