using UnityEngine;
using System.Collections.Generic;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private int ballLayer;
    public static LevelManager instance;
    public List<BallHandler> activeBalls;
    public int levelMaxTime;
    public float levelTimer;
    public int levelNumber;
    public GameObject currentLevel;
    public bool active { get; private set; }

    [SerializeField] private Infographic timeDisplay;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Debug.LogWarning("Multiple instances detected for LevelManager. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //Physics2D.IgnoreLayerCollision(ballLayer, ballLayer, true);
    }

    void AbilityTick()
    {
        foreach(BallHandler ball in activeBalls)
        {
            if(ball != null)
            {
                ball.OnAbilityTick();
            }
            else
            {
                activeBalls.Remove(ball);
            }
            
        }
    }

    public void StartLevel()
    {
        active = true;
        InvokeRepeating("AbilityTick", LevelStats.ABILITY_TICK_INTERVAL, LevelStats.ABILITY_TICK_INTERVAL);
        LevelStats.energy = 50;
        levelTimer = levelMaxTime;
    }

    public void NextLevel()
    {
        Destroy(currentLevel);
        levelNumber++;
        currentLevel = LevelGenerator.instance.GenerateLevel(levelNumber); 
        
    }

    public void PreviousLevel()
    {
        Destroy(currentLevel);
        levelNumber--;
        currentLevel = LevelGenerator.instance.GenerateLevel(levelNumber); 
    }

    public void LoadLevel(int number)
    {
        Destroy(currentLevel);
        levelNumber = number;
        currentLevel = LevelGenerator.instance.GenerateLevel(number); 
    }

    public void EndLevel()
    {
        Debug.Log("End Level");
        active = false;
        CancelInvoke("AbilityTick");
    }

    public void AddBall(BallHandler ball)
    {
        activeBalls.Add(ball);
    }

    public void AddEnergy(int amount)
    {
        
    }


    // Update is called once per frame
    void Update()
    {
 
        if (active)
        {
            levelTimer -= Time.deltaTime;
        }


    }
}
