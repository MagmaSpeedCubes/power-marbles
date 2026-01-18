using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using MagmaLabs.Utilities.Primitives;
using MagmaLabs.Utilities.Editor;
using MagmaLabs.UI;
using MagmaLabs.Economy.Security;
using MagmaLabs.Utilities;
using MagmaLabs.Animation;
using MagmaLabs.Audio;
[RequireComponent(typeof(AuthorizedModifier))]
public class LevelManager : MonoBehaviour
{
    [SerializeField] private int debugInfoLevel = Constants.DEBUG_INFO_LEVEL;
    public static LevelManager instance;
    
    public LevelHandler currentLevel;
    public GameObject[] kingdomLevels;
    

    [SerializeField] private Infographic timeDisplay, energyDisplay;
    [SerializeField] private TMPEnhanced beginLevelTitle, beginLevelInfo, beginLevelDifficulty, countdownText;
    [SerializeField] private TMPEnhanced endTitle, endMainLeft, endMainRight, endBottom;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private Canvas main, ingame, loading;
    private string state = "main";
    //main, ingame, loading
    [SerializeField] private GameObject beginWrapper, endWrapper;

    public BoundedEnum levelDifficultyDescriptions;

    public List<Color> levelDifficultyColors = new List<Color>();
    public Color winColor, loseColor;
    private int levelIndex=0;

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



    public void StartLevel()
    {
        DebugEnhanced.LogInfoLevel("Starting Level UI Animation", 1, debugInfoLevel);
        StartCoroutine(StartLevelUIAnimation());
    }


    void Update()
    {
        if(currentLevel != null && state == "ingame")
        {
            timeDisplay.SetValue(LevelStats.timeRemaining);
            energyDisplay.SetValue(LevelStats.energy);

        }        
    }

    public void EndLevel()
    {
        if (currentLevel == null)
        {
            Debug.LogError("EndLevel called but currentLevel is null!");
            return;
        }
        if(!currentLevel.active)
        {
            Debug.LogWarning("EndLevel called but currentLevel is not active!");
            return;
        }
        List<Tag> levelStats = currentLevel.EndLevel();
        if (levelStats == null)
        {
            Debug.LogError("LevelHandler.EndLevel() returned null!");
            return;
        }
        StartCoroutine(EndLevelUIAnimation(levelStats));
    }


    public void ExitInGame()
    {
        StartCoroutine(CloseLevel());
        //StartCoroutine(CloseLevelUIAnimation());
        AudioManager.instance.PlaySoundWithRandomPitchShift("pop", ProfileCustomization.uiVolume, 0.3f);
        StartCoroutine(CanvasAnimation.LoadingScreenCoroutine(ingame, loading, main, 2f));
        StartCoroutine(CanvasAnimation.Slide(endWrapper, new Vector2(0, 0), new Vector2(0, -2000), 1f));
        state = "main";
    }

    public void NextLevel()
    {

        levelIndex++;
        LoadKingdomLevel(levelIndex + 1);

        
    }
    public void ReplayLevel()
    {

        LoadKingdomLevel(levelIndex + 1);
    }

    public void LoadKingdomLevel(int number)
    {
        StartCoroutine(LoadKingdomLevelCoroutine(number));
    }

    public IEnumerator LoadKingdomLevelCoroutine(int number)
    {
        //StartCoroutine(CloseLevelUIAnimation());
        DebugEnhanced.LogInfoLevel("Loading Level " + number, 1, debugInfoLevel);
        yield return StartCoroutine(CloseLevel());

        GameObject levelPrefab = kingdomLevels[number-1];
        GameObject levelObject = Instantiate(levelPrefab);
        levelObject.transform.localPosition = new Vector3(0, 0, 0);
        currentLevel = levelObject.GetComponent<LevelHandler>();
        LoadLevelData();
        if(state.Equals("main"))
        {
            DebugEnhanced.LogInfoLevel("Entering from menu", 1, debugInfoLevel);
            StartCoroutine(EnterInGameUIAnimation());   
        }
        else if(state.Equals("ingame"))
        {
            DebugEnhanced.LogInfoLevel("Transitioning levels ingame", 1, debugInfoLevel);
            StartCoroutine(LevelTransitionUIAnimation());   
        }
        
    }

    public IEnumerator CloseLevel()
    {
        if(currentLevel!=null){
            DebugEnhanced.LogInfoLevel("Destroying current level", 1, debugInfoLevel);
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }
        yield break;
    }
    IEnumerator EndLevelUIAnimation(List<Tag> levelStats)
    {
        
        //StartCoroutine(CanvasAnimation.LoadingScreenCoroutine(ingame, loading, end, 1f));
        var winStat = levelStats.FirstOrDefault(kvp => kvp.name == "win");
        bool win = winStat.name != null && winStat.value == "0" ? false : true;
        string title = win ? "Stage Complete" : "Try Again";
        AudioManager.instance.PlaySound(win ? "level-pass" : "level-fail", ProfileCustomization.uiVolume);

        endTitle.SetText(title);
        endTitle.SetColor(win ? winColor : loseColor);

        endMainLeft.SetWriteOn(0f);
        endMainRight.SetWriteOn(0f);
        endMainLeft.SetText("");
        endMainRight.SetText("");



        yield return StartCoroutine(CanvasAnimation.Slide(endWrapper, new Vector2(0, -2000), new Vector2(0, 0), 1f));

        if (win)
        {
            nextLevelButton.SetActive(true);
            nextLevelButton.GetComponent<TMPEnhanced>().SetText("Level " + (levelIndex + 2));
            //note+ +2 is +1 for index, +1 for next level
            yield return StartCoroutine(AnimationManager.instance.PopIn(nextLevelButton, 1.2f, 0.5f));
        }
        else
        {
            nextLevelButton.SetActive(false);
        }
        


        foreach(Tag stat in levelStats)
        {
            string key = stat.name;
            string value = stat.value;

            if(key.Substring(0,2) == "s_") key = key.Substring(2);
            else continue;
            endMainLeft.AddText(Strings.CamelCaseToWords(key) + ": \n");
            endMainRight.AddText(value + "\n");
        }
        

        while (endMainLeft.GetWriteOn() < 1f && endMainRight.GetWriteOn() < 1f)
        {
            yield return StartCoroutine(endMainLeft.WriteLine(0.5f));
            yield return StartCoroutine(endMainRight.WriteLine(0.5f));
        }

        float efficiency = float.Parse(levelStats.FirstOrDefault(kvp => kvp.name == "s_efficiency").value);
        float pb = SecureProfileStats.instance.GetEfficiencyAtLevel(levelIndex);

        if(efficiency < pb || pb == 0)
        {
            SecureProfileStats.instance.ModifyEfficiencyScore(levelIndex, efficiency, GetComponent<AuthorizedModifier>());
            endBottom.SetText("Personal Best!");
            yield return StartCoroutine(endBottom.PopIn(1.2f, 0.5f));
        }else{
            endBottom.SetText("");
        }

        //actions = replay, next level, main menu
        yield break;
    }

    IEnumerator EnterInGameUIAnimation()
    {
        ingame.enabled = true;
        LoadLevelData();
        yield return StartCoroutine(CanvasAnimation.LoadingScreenCoroutine(main, loading, ingame, 2f));
        state = "ingame";
        yield return StartCoroutine(CanvasAnimation.Slide(beginWrapper, new Vector2(0, 2000), new Vector2(0, 0), 1f));
    }

    IEnumerator LevelTransitionUIAnimation()
    {

        StartCoroutine(CanvasAnimation.LoadingScreenCoroutine(ingame, loading, ingame, 2f));
        state = "ingame";
        DebugEnhanced.LogInfoLevel("Level Transition Animation", 1, debugInfoLevel);
        DebugEnhanced.LogInfoLevel("Sliding out end wrapper", 2, debugInfoLevel);//more detailed messages get higher level
        yield return StartCoroutine(CanvasAnimation.Slide(endWrapper, new Vector2(0, 0), new Vector2(0, -2000), 1f));
        DebugEnhanced.LogInfoLevel("Sliding in begin wrapper", 2, debugInfoLevel);
        yield return StartCoroutine(CanvasAnimation.Slide(beginWrapper, new Vector2(0, 2000), new Vector2(0, 0), 1f));
    }
    IEnumerator StartLevelUIAnimation()
    {

        ingame.enabled = true;
        state = "ingame";
        yield return StartCoroutine(CanvasAnimation.Slide(beginWrapper, new Vector2(0, 0), new Vector2(0, 2000), 1f));


        int countdown = 3;
        while(countdown > 0)
        {
            countdownText.SetText(countdown.ToString());
            AudioManager.instance.PlaySound("beep", ProfileCustomization.uiVolume);
            yield return StartCoroutine(countdownText.PopIn(1.2f, 0.5f));
            yield return new WaitForSeconds(0.5f);
            countdown--;
        }
        countdownText.SetText("Go!");
        AudioManager.instance.PlaySoundWithRandomPitchShift("whoosh", ProfileCustomization.uiVolume, 0.2f);
        yield return StartCoroutine(countdownText.PopIn(1.2f, 0.5f));
        yield return new WaitForSeconds(0.5f);
        countdownText.SetText("");
    
        currentLevel.StartLevel();
        yield break;
    }

    void LoadLevelData()
    {
        beginLevelTitle.SetText(currentLevel.levelName);
        beginLevelInfo.SetText("Energy: " + currentLevel.levelStartingEnergy + "\nTime Limit: " + currentLevel.levelMaxTime + "s");
        string difficultyDesc = levelDifficultyDescriptions.GetValueAtPosition(currentLevel.levelDifficulty);
        beginLevelDifficulty.SetText("" + difficultyDesc);
        beginLevelDifficulty.SetColor(levelDifficultyColors[levelDifficultyDescriptions.IndexOf(difficultyDesc)]);
    }

}
