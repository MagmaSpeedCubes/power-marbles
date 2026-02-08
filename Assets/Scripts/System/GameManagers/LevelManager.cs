//System
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

//Unity
using UnityEngine;
using UnityEngine.SceneManagement;

using MagmaLabs;
using MagmaLabs.Utilities.Primitives;
using MagmaLabs.Editor;
using MagmaLabs.UI;
using MagmaLabs.Economy.Security;
using MagmaLabs.Utilities;
using MagmaLabs.Animation;
using MagmaLabs.Audio;
using MagmaLabs.Economy;
using MagmaLabs.SceneManagement;

[RequireComponent(typeof(AuthorizedModifier))]
public class LevelUIManager : MonoBehaviour
{
    private const int DEBUG_INFO_LEVEL = 2;
    public static LevelUIManager instance;
    
    public LevelHandler currentLevel;
    public Dictionary<string, GameObject> kingdomLevels = new Dictionary<string, GameObject>();
    public List<string> levelOrder = new List<string>(); // Defines the order of levels
    

    [SerializeField] private Infographic timeDisplay, energyDisplay;
    [SerializeField] private TMPEnhanced beginLevelTitle, beginLevelInfo, beginLevelDifficulty, countdownText;
    [SerializeField] private TMPEnhanced endTitle, endMainLeft, endMainRight, endBottom;
    [SerializeField] private Canvas main, ingame, loading;
    private string state = "main";
    //main, ingame, loading
    [SerializeField] private GameObject beginWrapper, endWrapper;

    public BoundedEnum levelDifficultyDescriptions;

    public List<Color> levelDifficultyColors = new List<Color>();
    public Color winColor, loseColor;
    private string currentLevelName = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Debug.LogWarning("Multiple instances detected for LevelManager. Destroying duplicate.");
            Destroy(gameObject);
        }
    }


    public void OpenLevel()
    {
        StartCoroutine(EnterInGameUIAnimation());

    }
    public void StartLevel()
    {
        DebugEnhanced.LogInfoLevel("Starting Level UI Animation", 1, DEBUG_INFO_LEVEL);
        StartCoroutine(StartLevelUIAnimation());
    }


    void Update()
    {
        if(currentLevel != null && state == "ingame")
        {
            timeDisplay.SetValue(currentLevel.timeRemaining);
            energyDisplay.SetValue(currentLevel.energy);

        }        
    }

    public void ExitInGame()
    {
        AudioManager.instance.PlaySoundWithRandomPitchShift("pop", ProfileCustomization.uiVolume, 0.3f);
        CloseLevel();
        state = "main";
    }


    public void ReplayLevel()
    {
        StartCoroutine(CanvasAnimation.Slide(endWrapper, new Vector2(0, 0), new Vector2(0, -2000), 1f));
        SceneManagerEnhanced.instance.LoadSceneWithLoadingScreen(SceneManager.GetActiveScene().name, ingame, loading, ingame, 2f);


    }

    public void CloseLevel()
    {
        StartCoroutine(CanvasAnimation.Slide(endWrapper, new Vector2(0, 0), new Vector2(0, -2000), 1f));
        SceneManagerEnhanced.instance.LoadSceneWithLoadingScreen("MainMenu", ingame, loading, main, 2f);
    }
    public IEnumerator EndLevelUIAnimation(List<Tag> levelStats)
    {
        
        //StartCoroutine(CanvasAnimation.LoadingScreenCoroutine(ingame, loading, end, 1f));
        var winStat = levelStats.FirstOrDefault(kvp => kvp.name == "win");
        bool win = winStat.name != null && winStat.value == "0" ? false : true;
        string title = win ? "Stage Complete" : "Try Again";
        AudioManager.instance.PlaySound(win ? "level-pass" : "level-fail", ProfileCustomization.uiVolume);

        endTitle.SetText(title);
        endTitle.SetColor(win ? winColor : loseColor);

        endMainLeft.SetWriteOn(0);
        endMainRight.SetWriteOn(0);
        endMainLeft.SetText("");
        endMainRight.SetText("");



        yield return StartCoroutine(CanvasAnimation.Slide(endWrapper, new Vector2(0, -2000), new Vector2(0, 0), 1f));



        endBottom.SetText("");

        


        foreach(Tag stat in levelStats)
        {
            string key = stat.name;
            string value = stat.value;

            if(key.Substring(0,2) == "s_") key = key.Substring(2);
            else continue;
            endMainLeft.AddHiddenText(Strings.CamelCaseToWords(key) + ": \n");
            endMainRight.AddHiddenText(value + "\n");
        }
        

        while (endMainLeft.GetWriteOnNormalized() < 0.97f)
        {
            DebugEnhanced.LogInfoLevel("Left write on: " + endMainLeft.GetWriteOnNormalized(), 2, DEBUG_INFO_LEVEL);
            DebugEnhanced.LogInfoLevel("Right write on: " + endMainRight.GetWriteOnNormalized(), 2, DEBUG_INFO_LEVEL);
            AudioManager.instance.PlaySound("triple-beep", ProfileCustomization.uiVolume);
            yield return StartCoroutine(endMainLeft.WriteLineEarly(0.3f));
            yield return new WaitForSeconds(0.7f);
            AudioManager.instance.PlaySound("prize-small", ProfileCustomization.uiVolume);
            yield return StartCoroutine(endMainRight.WriteLineEarly(0.3f));
            yield return new WaitForSeconds(0.7f);
        }

        float efficiency = float.Parse(levelStats.FirstOrDefault(kvp => kvp.name == "s_efficiency").value);
        int levelIndex = levelOrder.IndexOf(currentLevelName);
        float pb = 0f;//patch
        
        if(efficiency > pb || pb == 0)
        {
            endBottom.AddHiddenText("Personal Best!");
            AudioManager.instance.PlaySound("prize-large", ProfileCustomization.uiVolume);
            yield return StartCoroutine(endBottom.PopIn(1.2f, 0.5f));
        }


        


        

        //actions = replay, next level, main menu
        yield break;
    }

    IEnumerator EnterInGameUIAnimation()
    {
        ingame.enabled = true;
        
        yield return StartCoroutine(CanvasAnimation.LoadingScreenCoroutine(main, loading, ingame, 2f));
        state = "ingame";
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

    public void LoadLevelData()
    {
        beginLevelTitle.SetText(currentLevel.levelData.name);
        beginLevelInfo.SetText("Energy: " + currentLevel.levelData.GetInt("startingEnergy") + "\nTime Limit: " + currentLevel.levelData.GetInt("maxTime") + "s");
        string difficultyDesc = levelDifficultyDescriptions.GetValueAtPosition(currentLevel.levelData.GetFloat("difficulty"));
        beginLevelDifficulty.SetText("" + difficultyDesc);
        beginLevelDifficulty.SetColor(levelDifficultyColors[levelDifficultyDescriptions.IndexOf(difficultyDesc)]);
    }

}
