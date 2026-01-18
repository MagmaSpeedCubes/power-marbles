using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MagmaLabs.Economy.Security;
using MagmaLabs.Economy;
using MagmaLabs.Animation;
using MagmaLabs.Audio;

public class PrizeSkiesManager : AuthorizedModifier
{
    int level = 1;

    [Header("Loot tables are ordered from lowest to highest tier")]
    [Header("Every jackpot level, the loot table advances by one")]
    [Header("The jackpot levels use a loot table two tiers higher than the current level's loot table")]
    [Header("The super jackpot levels use a loot table four times higher than the current level's loot table")]
    [Header("The final level must be a super jackpot")]
    [SerializeField]private LootTable[] lootTables;
    
    [SerializeField]private int levelsPerJackpot = 5;
    [SerializeField]private int[] superJackpotLevels = new int[] {15, 25, 30};
    [SerializeField]private OpenCloseAnimation levelCanvas, giftBoxCanvas;
    [SerializeField]private OpenCloseAnimation lossCanvas;
    [SerializeField]private GiftBox[] giftBoxes;
    [SerializeField]private float loseChanceMultiplier = 0.6f;
    [SerializeField]private Ownable lossItem;

    [SerializeField]private Color[] standardColors;
    [SerializeField]private Color jackpotBoxColor, jackpotRibbonColor;
    [SerializeField]private Color superJackpotBoxColor, superJackpotRibbonColor;
    private List<Ownable> collectedRewards;

    [SerializeField]private Image rewardDisplay;

    private SecureProfileStats sps;

    private int previousLosses;
    private int lastLossLevel;

    System.Random rng;

    private bool processingInProgress = false;

    void Start(){
        StartCoroutine(LateStart());    
    }
    IEnumerator LateStart(){
        yield return new WaitForSeconds(0.1f);
        sps = SecureProfileStats.instance;
    }
    public void RunPrizeSkies()
    {
        //designed for ui button
        StartCoroutine(RunPrizeSkiesCoroutine());
    }
    public IEnumerator RunPrizeSkiesCoroutine()
    {
        Debug.Log("Running Prize Skies");
        collectedRewards = new List<Ownable>();
        level = 1;
        previousLosses = 0;
        lastLossLevel = 0;
        rng = new System.Random();
        while(level <= superJackpotLevels[superJackpotLevels.Length - 1])
        {
            yield return new WaitForSeconds(0.5f);
            InitializeLevel();
            int levelToWaitFor = level;
            giftBoxCanvas.Toggle();

            yield return new WaitUntil(() => level != levelToWaitFor);

            if(level >= superJackpotLevels[superJackpotLevels.Length - 1] + 1)
            {
                InputPlayerDecision("endWin");
                //end the game if final level completed
            }
            if(level == 0)
            {
                levelCanvas.Close();
                break;
                
                //end the game early if player decides to end
            }
            giftBoxCanvas.Toggle();
            yield return new WaitForSeconds(1f);




        }
    }

    private void InitializeLevel()
    {
        Debug.Log("Initializing level " + level);
        LootTable currentTable = lootTables[(level - 1) / levelsPerJackpot];
        foreach(GiftBox giftBox in giftBoxes)
        {
            if(Array.IndexOf(superJackpotLevels, level) != -1)
            {
                //super jackpot level
                giftBox.SetRewards(new Ownable[] {
                    lootTables[Mathf.Min(lootTables.Length - 1, ((level - 1) / levelsPerJackpot) + 4)].GetLoot()
                });
                giftBox.SetColor(superJackpotBoxColor,superJackpotRibbonColor);

                
                
            }
            else if(level % levelsPerJackpot == 0)
            {
                //jackpot level
                giftBox.SetRewards(new Ownable[] {
                    lootTables[Mathf.Min(lootTables.Length - 1, ((level - 1) / levelsPerJackpot) + 2)].GetLoot()
                });
                giftBox.SetColor(jackpotBoxColor,jackpotRibbonColor);
            }
            else
            {
                //normal level
                giftBox.SetRewards(new Ownable[] {
                    currentTable.GetLoot()
                });

                Color rc1, rc2;
                do{
                    rc1 = standardColors[UnityEngine.Random.Range(0, standardColors.Length)];
                    rc2 = standardColors[UnityEngine.Random.Range(0, standardColors.Length)];
                }while(rc1 == rc2);
                giftBox.SetColor(rc1, rc2);
                
            }
            //assign reward to gift box
        }


    }



    public void OpenBox(int index)
    {
        //designed for ui button
        StartCoroutine(OpenBoxCoroutine(index));
    }
    public IEnumerator OpenBoxCoroutine(int index)
    {
        GiftBox boxToOpen = giftBoxes[index];
        /*
        this is a very important mechanic
        with the chance of losing being lower than if it were a real situation, we improve player retention
        but we still want there to be a chance of losing to keep players engaged
        */


        bool loseOnLevel = rng.NextDouble() < loseChanceMultiplier / giftBoxes.Length;
        if(loseOnLevel && CanLoseOnLevel(level))
        {
            boxToOpen.SetRewards(new Ownable[] {lossItem});
            lastLossLevel = level;
            yield return StartCoroutine(boxToOpen.Open());
            lossCanvas.Open();

            //prompt to continue or end game
        }
        else
        {
            GiftBox randomBox;
            do{
                randomBox = giftBoxes[UnityEngine.Random.Range(0, giftBoxes.Length)];
            }while(randomBox == boxToOpen);
            randomBox.SetRewards(new Ownable[] {lossItem});
            yield return StartCoroutine(boxToOpen.Open());
            foreach(Ownable reward in boxToOpen.GetRewards())
            {
                collectedRewards.Add(reward);
                Debug.Log("Won reward " + reward.name);
            }
            yield return new WaitForSeconds(0.25f);
            foreach(GiftBox box in giftBoxes){
                StartCoroutine(box.Open());
            }
            yield return new WaitForSeconds(2f);

            foreach(GiftBox box in giftBoxes){
                StartCoroutine(box.Close());
            }
            yield return new WaitForSeconds(1f);

            


            InputPlayerDecision("continue");



        }
        
        
        
        
    }

    public void ReviveWithGlassMarbles(){
        int reviveCost = (int)Math.Pow(2, previousLosses);
        if(sps.GetGlassMarbles() >= reviveCost){
            sps.ModifyGlassMarbles(-reviveCost, this);
            InputPlayerDecision("continue");
        }else{
            Debug.LogError("Do not have enough glass marbles");
        }

    }

    public void ReviveWithReviveOwnable(){
        Ownable revive = sps.FindFirstOwnableOfName("revive");
        if(revive != null){
            sps.RemoveFirstOwnableOfName("revive", this);
            InputPlayerDecision("continue");
        }else{
            Debug.LogError("Do not have a revive");
        }
        
    }

    public void InputPlayerDecision(string choice)
    {
        StartCoroutine(InputPlayerDecisionCoroutine(choice));       
    }

    public IEnumerator InputPlayerDecisionCoroutine(string choice)
    {
        if(processingInProgress){yield break;}
        //this is designed to be called in a button
        if(choice == "continue")
        {
            
            level++;
        }
        else if(choice == "endWin")
        {
            processingInProgress = true;       
            yield return StartCoroutine(DisplayRewards());
            foreach(Ownable reward in collectedRewards)
            {
                sps.AddOwnable(reward, this);
            }
            level = 0;
            processingInProgress = false;

        }
        else if(choice == "endLoss")
        {

            level = 0;

        }
    }

    public IEnumerator DisplayRewards()
    {
        rewardDisplay.color = new Color(1,1,1,1);
        foreach(Ownable reward in collectedRewards)
        {
            rewardDisplay.sprite = reward.sprite;
            AudioManager.instance.PlaySoundWithRandomPitchShift("reward", ProfileCustomization.uiVolume);
            yield return new WaitForSeconds(0.75f);
        }
        rewardDisplay.color = new Color(1,1,1,0);

    }



    public bool CanLoseOnLevel(int levelNum)
    {
        return levelNum > levelsPerJackpot && levelNum % levelsPerJackpot != 0 && Array.IndexOf(superJackpotLevels, levelNum) == -1;
    }


}
