using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class LevelHandler : MonoBehaviour
{
    public string levelName;
    public float levelDifficulty;
    public List<BallHandler> activeBalls;
    public int levelMaxTime, levelStartingEnergy;

    public bool active;


    public void StartLevel()
    {
       active = true; 
       InvokeRepeating("AbilityTick", LevelStats.ABILITY_TICK_INTERVAL, LevelStats.ABILITY_TICK_INTERVAL);
        LevelStats.energy = levelStartingEnergy;
        LevelStats.timeRemaining = levelMaxTime;
        LevelStats.marblesUsed = 0;

    }

    public List<Tag> EndLevel()
    {
        
        Debug.Log("End Level");
        CancelInvoke("AbilityTick");
        active = false;

        List<Tag> levelStats = new List<Tag>();
        if(LevelStats.timeRemaining > 0)
        {
            levelStats.Add(new Tag("win", "1"));
        }
        else
        {
            levelStats.Add(new Tag("win", "0"));
        }


        levelStats.Add(new Tag("s_damageDealt", "" + LevelStats.damageDealt));
        //Debug.Log("Damage Dealt: " + LevelStats.damageDealt);
        levelStats.Add(new Tag("s_marblesUsed","" +  LevelStats.marblesUsed));
        //Debug.Log("Marbles Used: " + LevelStats.marblesUsed);
        levelStats.Add(new Tag("s_levelTime", "" + Math.Round((levelMaxTime-LevelStats.timeRemaining), 2)));

        levelStats.Add(new Tag("s_efficiency", "" + Math.Round(LevelStats.timeRemaining, 2)));


        levelStats.Add(new Tag("xpReward", "" + LevelStats.damageDealt));
        //Debug.Log("XP Reward: " + LevelStats.damageDealt);
        //Debug.Log("Returned Level Stats " + levelStats.ToString());

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
            LevelStats.timeRemaining -= Time.deltaTime;
            if(LevelStats.timeRemaining < 0f)
            {
                LevelManager.instance.EndLevel();
            }
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
