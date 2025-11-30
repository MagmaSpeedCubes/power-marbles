using UnityEngine;
using System.Collections.Generic;
public class LevelManager : MonoBehaviour
{
    [SerializeField] protected int ballLayer;
    public static LevelManager instance;
    public List<BallManager> activeBalls;

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
        Physics2D.IgnoreLayerCollision(ballLayer, ballLayer, true);
    }

    void AbilityTick()
    {
        foreach(BallManager ball in activeBalls)
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
        InvokeRepeating("AbilityTick", LevelStats.ABILITY_TICK_INTERVAL, LevelStats.ABILITY_TICK_INTERVAL);
        LevelStats.energy = 50;
    }

    public void EndLevel()
    {
        CancelInvoke("AbilityTick");
    }

    public void AddBall(BallManager ball)
    {
        activeBalls.Add(ball);
    }


    // Update is called once per frame
    void Update()
    {
        for(int i=activeBalls.Count-1; i>=LevelStats.MAX_BALL_COUNT; i--)
        {
            BallManager ball = activeBalls[i];
            Debug.Log("Balls capped at " + LevelStats.MAX_BALL_COUNT + ". Destroying ball " + i);
            Destroy(ball.gameObject);
            activeBalls.Remove(ball);
        }
    }
}
