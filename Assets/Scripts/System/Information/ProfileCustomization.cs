using UnityEngine;
using System.Collections.Generic;

public class ProfileCustomization : MonoBehaviour
{

    public static string infoLevel = "technical";
    //-1 = aesthetic
    // 0 = balanced
    // 1 = technical
    // 2 = debug
    public static float masterVolume = 1f;
    

    public static float uiVolume = 1f;
    public static float musicVolume = 1f;
    public static float alertVolume = 1f;
    public static float playerVolume = 1f;
    public static float enemyVolume = 1f;
    public static float worldVolume = 1f;

    public static LinkedList<Color> rarityColors;
    public List<Color> assignRarityColors;


    void Awake()
    {
        
        DontDestroyOnLoad(this.gameObject);
        rarityColors = new LinkedList<Color>();
        foreach(Color color in assignRarityColors)
        {
            rarityColors.AddLast(color);
        }
        LoadPrefs();

    }

    public static void LoadPrefs()
    {
        infoLevel = PlayerPrefs.GetString("infoLevel");
        masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
    }

    public static void SavePrefs()
    {
        PlayerPrefs.SetString("infoLevel", infoLevel);
        Debug.Log("Saved infoLevel as " + infoLevel);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        // Debug.Log("Saved masterVolume as " + masterVolume);
        // Debug.Log("ProfileStats saved");
    }
}
