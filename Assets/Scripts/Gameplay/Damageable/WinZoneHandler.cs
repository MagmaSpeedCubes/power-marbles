using UnityEngine;

public class WinHandler : MonoBehaviour
{   void OnTriggerEnter2D(Collider2D other)
    {
        BallHandler bh = other.GetComponent<BallHandler>();
        if(bh != null)
        {
            LevelManager.instance.EndLevel();
        }
        
    }
}
