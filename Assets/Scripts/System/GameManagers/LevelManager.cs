using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using MagmaLabs.Utilities.Strings;
using MagmaLabs.UI;
using MagmaLabs.Economy.Security;
using MagmaLabs.Utilities;
using MagmaLabs.Animation;
[RequireComponent(typeof(AuthorizedModifier))]
public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    
    public LevelHandler currentLevel;
    public GameObject[] kingdomLevels;
    [SerializeField]private Canvas beginScreen, endScreen;
    

    [SerializeField] private Infographic timeDisplay, energyDisplay;
    [SerializeField] private TextMeshMaxUGUI beginLevelTitle, beginLevelInfo, beginLevelDifficulty, countdownText;
    [SerializeField] private TextMeshMaxUGUI endTitle, endMainLeft, endMainRight, endBottom;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private Canvas main, ingame, loading;
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

        }
        else
        {
            Debug.LogWarning("Multiple instances detected for LevelManager. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    void OpenLevel()
    {
        Debug.Log("Opening Level UI");
        StartCoroutine(OpenLevelUIAnimation());
    }

    IEnumerator OpenLevelUIAnimation()
    {
        
        beginLevelTitle.SetText(currentLevel.levelName);
        beginLevelInfo.SetText("Energy: " + currentLevel.startingEnergy + "\nTime Limit: " + currentLevel.levelMaxTime + "s");
        string difficultyDesc = levelDifficultyDescriptions.GetValueAtPosition(currentLevel.levelDifficulty);
        beginLevelDifficulty.SetText("" + difficultyDesc);
        beginLevelDifficulty.SetColor(levelDifficultyColors[levelDifficultyDescriptions.IndexOf(difficultyDesc)]);

        
        yield return StartCoroutine(CanvasAnimation.LoadingScreenCoroutine(main, loading, ingame, 2f));
        yield return StartCoroutine(CanvasAnimation.Slide(beginWrapper, new Vector2(0, 2000), new Vector2(0, 0), 1f));


        yield break;

        
    }

    public void StartLevel()
    {
        Debug.Log("Starting Level UI Animation");
        StartCoroutine(StartLevelUIAnimation());
    }

    IEnumerator StartLevelUIAnimation()
    {

        ingame.enabled = true;

        int countdown = 3;
        while(countdown > 0)
        {
            countdownText.SetText(countdown.ToString());
            AudioManager.instance.PlaySoundWithPitchShift("beep", ProfileCustomization.uiVolume, 0.2f);
            yield return StartCoroutine(countdownText.PopIn(1.2f, 0.5f));
            yield return new WaitForSeconds(0.5f);
            countdown--;
        }
        countdownText.SetText("Go!");
        AudioManager.instance.PlaySoundWithPitchShift("start", ProfileCustomization.uiVolume, 0.2f);
        yield return StartCoroutine(countdownText.PopIn(1.2f, 0.5f));
        yield return new WaitForSeconds(0.5f);
    
        currentLevel.StartLevel();
         yield break;
    }

    public void EndLevel()
    {
        Debug.Log("Ending Level UI Animation");
        List<KeyValuePair<string, float>> levelStats = currentLevel.EndLevel();
        StartCoroutine(EndLevelUIAnimation(levelStats));


        
    }

    IEnumerator EndLevelUIAnimation(List<KeyValuePair<string, float>> levelStats)
    {
        
        //StartCoroutine(CanvasAnimation.LoadingScreenCoroutine(ingame, loading, end, 1f));
        bool win = (levelStats.FirstOrDefault(kvp => kvp.Key == "win").Value)==0 ? false : true;
        string title = win ? "Stage Complete" : "Try Again";

        endTitle.SetText(title);
        endTitle.SetColor(win ? winColor : loseColor);


        endMainLeft.SetText("");
        endMainLeft.SetWriteOn(0f);
        endMainRight.SetText("");
        endMainRight.SetWriteOn(0f);

        if (win)
        {
            nextLevelButton.SetActive(true);
            nextLevelButton.GetComponent<TextMeshMaxUGUI>().SetText("Level " + (levelIndex + 1));

        }
        else
        {
            nextLevelButton.SetActive(false);
        }
        


        foreach(KeyValuePair<string, float> stat in levelStats)
        {
            string key;
            float value;
            stat.Deconstruct(out key, out value);
            if(key.Substring(0,2) == "s_") key = key.Substring(2);
            else continue;
            endMainLeft.AddText(Strings.CamelCaseToWords(key) + ": \n");
            endMainRight.AddText(value.ToString() + "\n");
        }
        

        while (endMainLeft.GetWriteOn() < 1f)
        {
            yield return StartCoroutine(endMainLeft.WriteLine(0.5f));
            yield return StartCoroutine(endMainRight.WriteLine(0.5f));
        }

        float efficiency = levelStats.FirstOrDefault(kvp => kvp.Key == "s_efficiency").Value;
        float pb = SecureProfileStats.instance.GetEfficiencyAtLevel(levelIndex);

        if(efficiency < pb || pb == 0)
        {
            SecureProfileStats.instance.ModifyEfficiencyScore(levelIndex, efficiency, GetComponent<AuthorizedModifier>());
            endBottom.SetText("New Personal Best!");
            endBottom.PopIn(1.2f, 0.5f);
        }

        
        //actions = replay, next level, main menu
        yield break;

        

    }

    
    public void NextKingdomLevel()
    {

        levelIndex++;
        LoadKingdomLevel(levelIndex);

        
    }
    public void ReplayLevel()
    {
        
        LoadKingdomLevel(levelIndex);
    }
    public void LoadKingdomLevel(int number)
    {
        Debug.Log("Loading Level " + number);
        if(currentLevel!=null)
        Destroy(currentLevel.gameObject);

        currentLevel = null;
        GameObject levelPrefab = kingdomLevels[number-1];


        GameObject levelObject = Instantiate(levelPrefab);
        levelObject.transform.localPosition = new Vector3(0, 0, 0);
        currentLevel = levelObject.GetComponent<LevelHandler>();

        OpenLevel();
    }

    public void CloseLevel()
    {
        if(currentLevel!=null)
        Destroy(currentLevel.gameObject);

        currentLevel = null;

        AudioManager.instance.PlaySoundWithPitchShift("pop", ProfileCustomization.uiVolume, 0.3f);
        StartCoroutine(CanvasAnimation.LoadingScreenCoroutine(ingame, loading, main, 2f));

    }








    // Update is called once per frame 

}
