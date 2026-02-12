using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using MagmaLabs;
using MagmaLabs.Economy;
using MagmaLabs.Editor;
public class LevelHandler : MonoBehaviour
{
    public const int DEBUG_INFO_LEVEL = 1;

    public static LevelHandler instance;
    public ProgressionNode levelData;
    public List<BallHandler> activeBalls;

    public bool active;

    public Vector2 levelSize = new Vector2(12,26);

    public int energy { get; private set; }
    public float timeRemaining { get; private set; }
    public int marblesUsed { get; private set; }
    public float damageDealt;

    public GameObject background;

    public List<GameObject> winZones = new List<GameObject>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            LevelUIManager.instance.currentLevel = this;
            LevelUIManager.instance.OpenLevel();
            LevelUIManager.instance.LoadLevelData();
            energy = levelData.GetInt("startingEnergy");
            timeRemaining = levelData.GetInt("maxTime");
            marblesUsed = 0;

        }
        else
        {
            Debug.LogWarning("Multiple instances of LevelHandler detected. Destroying duplicate.");
            Destroy(this.gameObject);
        }
    }
    public void StartLevel()
    {
        active = true; 
        InvokeRepeating("AbilityTick", Constants.ABILITY_TICK_INTERVAL, Constants.ABILITY_TICK_INTERVAL);

    }

    public void EndLevel()
    {
        
        Debug.Log("End Level");
        CancelInvoke("AbilityTick");
        active = false;

        List<Tag> levelStats = new List<Tag>();
        if(timeRemaining > 0)
        {
            levelStats.Add(new Tag("win", "1"));

            levelData.Activate();
            ProgressionNode.Refresh();
        }
        else
        {
            levelStats.Add(new Tag("win", "0"));
        }

        

        float levelMaxTime = levelData.GetInt("maxTime");
        levelStats.Add(new Tag("s_levelTime", "" + Math.Round(timeRemaining, 2)));
        float efficiencyScore = (float)Math.Round(timeRemaining, 2);
        //levelStats.Add(new Tag("s_efficiency_0", "" + Math.Round(levelMaxTime-timeRemaining, 2)));

        levelStats.Add(new Tag("s_damageDealt", "" + damageDealt));
        DebugEnhanced.LogInfoLevel("Damage Dealt: " + damageDealt, 2, DEBUG_INFO_LEVEL);
        //efficiencyScore = (float)Math.Round(levelMaxTime);
        //levelStats.Add(new Tag("s_efficiency_1", "" + Math.Round(levelMaxTime-timeRemaining, 2)));


        levelStats.Add(new Tag("s_marblesUsed","" +  marblesUsed));
        DebugEnhanced.LogInfoLevel("Marbles Used: " + marblesUsed, 2, DEBUG_INFO_LEVEL);
        efficiencyScore -= marblesUsed*0.1f;
        //levelStats.Add(new Tag("s_efficiency_2", "" + Math.Round(levelMaxTime-timeRemaining, 2)));
        
        

        levelStats.Add(new Tag("s_efficiency", "" + Math.Round(timeRemaining, 2)));
        DebugEnhanced.LogInfoLevel("New score " + efficiencyScore, 1, DEBUG_INFO_LEVEL);

        if(levelData.GetFloat("highScore") < efficiencyScore)
        {
            
            levelData.SetTag("highScore", ""+efficiencyScore);
            levelData.Save();
        }


        levelStats.Add(new Tag("xpReward", "" + damageDealt));
        //Debug.Log("XP Reward: " + LevelStats.damageDealt);
        //Debug.Log("Returned Level Stats " + levelStats.ToString());

        StartCoroutine(LevelUIManager.instance.EndLevelUIAnimation(levelStats));
        
        //check for pb in manager, not handler
        //also check for global ranking in manager, not handler

        //for technical stats
        //damage speed
        //breakdown by marble


    }

    public void AddBall(BallHandler ball)
    {
        activeBalls.Add(ball);
        marblesUsed++;
    }

    public void AddEnergy(int amount)
    {
        energy += amount;
    }

    public void UseEnergy(int amount)
    {
        energy -= amount;
    }

    void Update()
    {
 
        if (active)
        {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining < 0f)
            {
                EndLevel();
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

    public void OnWinZoneToggled(WinHandler source)
    {
        GameObject wzo = source.gameObject;
        for(int i=winZones.Count-1; i>=0; i--)
        {
            if (winZones[i].Equals(wzo))
            {
                winZones.RemoveAt(i);
                break;
            }
        }
        if (winZones.Count == 0)
        {
            EndLevel();
        }
    }

}
