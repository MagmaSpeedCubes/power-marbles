using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LevelHandler : MonoBehaviour
{
    public string levelName;
    public int startingEnergy;
    public float levelDifficulty;
    public List<BallHandler> activeBalls;
    public int levelMaxTime;
    public float levelTimer, levelEnergy;
    private bool active;


    private int damageDealt;
    private int marblesUsed;

    public void StartLevel()
    {
       active = true; 
       InvokeRepeating("AbilityTick", LevelStats.ABILITY_TICK_INTERVAL, LevelStats.ABILITY_TICK_INTERVAL);
        LevelStats.energy = levelEnergy;
        levelTimer = levelMaxTime;
    }

    public List<KeyValuePair<string, float>> EndLevel()
    {
        
        Debug.Log("End Level");
        CancelInvoke("AbilityTick");
        active = false;

        List<KeyValuePair<string, float>> levelStats = new List<KeyValuePair<string, float>>();
        if(levelTimer <= levelMaxTime)
        {
            levelStats.Add(new KeyValuePair<string, float>("win", 1));
        }
        else
        {
            levelStats.Add(new KeyValuePair<string, float>("win", 0));
        }


        levelStats.Add(new KeyValuePair<string, float>("s_damageDealt", damageDealt));
        levelStats.Add(new KeyValuePair<string, float>("s_marblesUsed", 0));
        levelStats.Add(new KeyValuePair<string, float>("s_levelTime", levelTimer));
        levelStats.Add(new KeyValuePair<string, float>("s_efficiency", levelMaxTime-levelTimer));

        levelStats.Add(new KeyValuePair<string, float>("xpReward", damageDealt));

        return levelStats;
        
        //check for pb in manager, not handler
        //also check for global ranking in manager, not handler

        //for technical stats
        //damage speed
        //breakdown by marble


    }

    public void AddBall(BallHandler ball)
    {
        activeBalls.Add(ball);
    }

    public void AddEnergy(int amount)
    {
        
    }

    void Update()
    {
 
        if (active)
        {
            levelTimer -= Time.deltaTime;
        }


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

    public bool IsActive()
    {
        return active;
    }

}
