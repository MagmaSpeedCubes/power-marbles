using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LocalClickManager : MonoBehaviour
{

    public GameObject ballParent;
    public GameObject spawnZone;

    public static LocalClickManager localInstance;
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
            var found = GameObject.Find("MarbleParent");
            if (found != null) ballParent = found;
        }
        //Debug.Log("Click Manager Created");
    }




    void Update()
    {
        //Debug.Log("Click Manager Update");
        // Mouse left click
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Debug.Log("Mouse Left Click");
            Vector2 screenPos = Mouse.current.position.ReadValue();
            //Debug.Log("Screen Pos: " + screenPos);
            Vector3 worldPos = ScreenToWorld(screenPos);
            //Debug.Log("World Pos: " + worldPos);
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
        //Debug.Log("Tap at World Pos: " + worldPos + " Screen Pos: " + screenPos);
        if(LevelStats.selectedBall != null)
        {
            //Debug.Log("Selected Ball: " + LevelStats.selectedBall.name);
            Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

            foreach (Collider2D hit in hits)
            {
                
                if(hit.gameObject == spawnZone)
                {
                    //Debug.Log("Clicked on Spawn Zone");
                    switch(SceneManager.GetActiveScene().name){
                        case "MarbleKingdom":
                            if(LevelManager.instance.currentLevel.activeBalls.Count-1 >= LevelStats.MAX_BALL_COUNT)
                            {
                                //AlertManager.instance.ThrowUIWarning("Marble cap reached", new string[]{"Marbles capped at " + LevelStats.MAX_BALL_COUNT + ". Destroying extras."});
                                break;
                            }

                            if(LevelManager.instance.currentLevel.active == false)
                            {
                
                                break;
                            }
                            

                            if(LevelStats.selectedBall.price <= LevelStats.energy || Constants.DEBUG_MODE)
                            {

                                SpawnMarble(worldPos);
                                LevelStats.energy -= LevelStats.selectedBall.price;
                                LevelStats.marblesUsed++;

                            }
                            else
                            {
                                /*
                                AlertManager.instance.ThrowUIWarning("Not enough energy", new string[]
                                {"You have " + LevelStats.energy + "energy, but " + 
                                LevelStats.selectedBall.name + " costs " + LevelStats.selectedBall.price + " energy"});
                                */
                            }
                            break;
                        case "TreasureHunt":
                            if(TreasureHuntManager.instance.UseEnergy(LevelStats.selectedBall.price))
                            {
                                SpawnMarble(worldPos); 
                            }
                            break;
                        default:
                            break;
                        
                    }

                    
                }
            }


        }

        
    }

    void SpawnMarble(Vector3 worldPos){
        GameObject newBall = Instantiate(LevelStats.selectedBall.ballPrefabObject, worldPos, Quaternion.identity);
        newBall.name = LevelStats.selectedBall.ballPrefabObject.name;
        newBall.GetComponent<SpriteRenderer>().color = LevelStats.selectedBall.prefab.defaultColor;
        newBall.transform.parent = ballParent.transform;
        Vector3 liftPosition = new Vector3(newBall.transform.position.x, newBall.transform.position.y, 
        newBall.transform.position.z - LevelManager.instance.currentLevel.activeBalls.Count * 0.01f);
        newBall.transform.position = liftPosition;
    }
}


