using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;
    
    public LevelHandler currentLevel;
    public GameObject[] kingdomLevels;
    [SerializeField]private Canvas beginScreen, endScreen;

    [SerializeField] private Infographic timeDisplay, energyDisplay;

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


    public void NextLevel()
    {
        Destroy(currentLevel.gameObject);

        
    }

    public void PreviousLevel()
    {
        Destroy(currentLevel.gameObject);

    }

    public void EndLevel(List<KeyValuePair<string, float>> levelStats)
    {


        
    }

    IEnumerator EndLevelUIAnimation(List<KeyValuePair<string, float>> levelStats)
    {
        OpenCloseAnimation anim = endScreen.GetComponent<OpenCloseAnimation>();
        yield return StartCoroutine(anim.OpenCoroutine());
        bool win = (levelStats.FirstOrDefault(kvp => kvp.Key == "win").Value)==0 ? false : true;
        string title = win ? "Stage Complete" : "Try Again";

        string mainText = "";

        foreach(KeyValuePair<string, float> stat in levelStats)
        {
            string initialText = mainText;
            

        }
        yield break;



    }

    public void LoadKingdomLevel(int number)
    {
        Destroy(currentLevel.gameObject);
        GameObject levelPrefab = kingdomLevels[number-2];


        GameObject levelObject = Instantiate(levelPrefab);
        levelObject.transform.localPosition = new Vector3(0, 0, 0);
    }








    // Update is called once per frame 

}
