using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LocalClickManager : MonoBehaviour
{

    [SerializeField] private GameObject ballParent;

    private static LocalClickManager localInstance;
    void Awake()
    {
        if (localInstance == null)
        {
            localInstance = this;
        }
        else if (localInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Try to locate a ballParent if it wasn't assigned in the inspector
        if (ballParent == null)
        {
            var found = GameObject.Find("BallParent");
            if (found != null) ballParent = found;
        }
        Debug.Log("Click Manager Created");
    }




    void Update()
    {
        //Debug.Log("Click Manager Update");
        // Mouse left click
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse Left Click");
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = ScreenToWorld(screenPos);
            OnTap(worldPos, screenPos);
        }

        // Touch (multiple touches supported)
        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.wasPressedThisFrame)
                {
                    Vector2 screenPos = touch.position.ReadValue();
                    Vector3 worldPos = ScreenToWorld(screenPos);
                    OnTap(worldPos, screenPos);
                }
            }
        }
    }

    Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            cam = FindObjectOfType<Camera>();
            if (cam == null) return Vector3.zero;
        }
        float z = -cam.transform.position.z; // distance to z=0 plane (works for 2D)
        Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
        world.z = 0f; // keep on 2D plane
        return world;
    }

    void OnTap(Vector3 worldPos, Vector2 screenPos)
    {
        if(LevelStats.selectedBall != null)
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

            foreach (Collider2D hit in hits)
            {
                
                if(hit.name == "SpawnZone")
                {
                    Debug.Log("Clicked on Spawn Zone");
                    if(LevelManager.instance.activeBalls.Count-1 >= LevelStats.MAX_BALL_COUNT)
                    {
                        AlertManager.instance.ThrowUIWarning("Marble cap reached", new string[]{"Marbles capped at " + LevelStats.MAX_BALL_COUNT + ". Destroying extras."});
                        break;
                    }
                    //continue spawning process if and only if clicked in spawn zone
                    if(LevelStats.selectedBall.price <= LevelStats.energy)
                    {

                        if (!LevelManager.instance.active)
                        {
                            LevelManager.instance.StartLevel();
                        }
                        LevelStats.energy -= LevelStats.selectedBall.price;
                        GameObject newBall = Instantiate(LevelStats.selectedBall.ballPrefabObject, worldPos, Quaternion.identity);
                        newBall.name = LevelStats.selectedBall.ballPrefabObject.name;
                        newBall.GetComponent<SpriteRenderer>().color = LevelStats.selectedBall.prefab.defaultColor;
                        newBall.transform.parent = ballParent.transform;
                        Vector3 liftPosition = new Vector3(newBall.transform.position.x, newBall.transform.position.y, 
                        newBall.transform.position.z - LevelManager.instance.activeBalls.Count * 0.01f);
                        newBall.transform.position = liftPosition;

                        
                        
                    }
                    
                }
            }


        }

        
    }
}


