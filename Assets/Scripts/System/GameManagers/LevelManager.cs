using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using MagmaLabs.Utilities.Strings;
using MagmaLabs.UI;
using MagmaLabs.Economy.Security;
using MagmaLabs.Utilities;
[RequireComponent(typeof(AuthorizedModifier))]
public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    
    public LevelHandler currentLevel;
    public GameObject[] kingdomLevels;
    [SerializeField]private Canvas beginScreen, endScreen;
    

    [SerializeField] private Infographic timeDisplay, energyDisplay;
    [SerializeField] private TextMeshMaxUGUI beginLevelTitle, beginLevelInfo, beginLevelDifficulty;
    [SerializeField] private TextMeshMaxUGUI endTitle, endMainLeft, endMainRight, endBottom;
    [SerializeField] private OpenCloseAnimation endActions;

    public BoundedEnum levelDifficultyDescriptions;

    public List<Color> levelDifficultyColors = new List<Color>();
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
        StartCoroutine(OpenLevelUIAnimation());
    }

    IEnumerator OpenLevelUIAnimation()
    {
        beginLevelTitle.SetText(currentLevel.levelName);
        beginLevelInfo.SetText("Energy: " + currentLevel.startingEnergy + "\nTime Limit: " + currentLevel.levelMaxTime + "s");
        string difficultyDesc = levelDifficultyDescriptions.GetValueAtPosition(currentLevel.levelDifficulty);
        beginLevelDifficulty.SetText("Difficulty: " + difficultyDesc);
        beginLevelDifficulty.SetColor(levelDifficultyColors[levelDifficultyDescriptions.IndexOf(difficultyDesc)]);


        OpenCloseAnimation anim = beginScreen.GetComponent<OpenCloseAnimation>();
        yield return null;
        anim.Open();

        yield break;

        
    }

    public void StartLevel()
    {
        StartCoroutine(StartLevelUIAnimation());
    }

    IEnumerator StartLevelUIAnimation()
    {
        OpenCloseAnimation anim = beginScreen.GetComponent<OpenCloseAnimation>();
        anim.Close();
        yield return null;
        currentLevel.StartLevel();
         yield break;
    }

    public void EndLevel()
    {
        List<KeyValuePair<string, float>> levelStats = currentLevel.EndLevel();
        StartCoroutine(EndLevelUIAnimation(levelStats));


        
    }

    IEnumerator EndLevelUIAnimation(List<KeyValuePair<string, float>> levelStats)
    {
        OpenCloseAnimation anim = endScreen.GetComponent<OpenCloseAnimation>();
        yield return StartCoroutine(anim.OpenCoroutine());
        bool win = (levelStats.FirstOrDefault(kvp => kvp.Key == "win").Value)==0 ? false : true;
        string title = win ? "Stage Complete" : "Try Again";

        endTitle.SetText(title);


        endMainLeft.SetText("");
        endMainLeft.SetWriteOn(0f);
        endMainRight.SetText("");
        endMainRight.SetWriteOn(0f);


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

        endActions.Open();
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
        AudioManager.instance.PlaySoundWithPitchShift("pop", ProfileCustomization.uiVolume, 0.3f);
        endActions.Close();
        if(currentLevel!=null)
        Destroy(currentLevel.gameObject);

        GameObject levelPrefab = kingdomLevels[number-1];


        GameObject levelObject = Instantiate(levelPrefab);
        levelObject.transform.localPosition = new Vector3(0, 0, 0);

        OpenLevel();
    }








    // Update is called once per frame 

}
