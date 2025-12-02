using UnityEngine;
using System;
using System.Collections.Generic;

public class ProfileCustomization : MonoBehaviour
{

    public static ProfileCustomization instance;
    public enum InfoLevel
    {
        aesthetic = -1,
        balanced = 0,
        technical = 1,
        debug = 2
    }


    public static InfoLevel infoLevel = InfoLevel.balanced;
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


    [Header("UI Colors")]
    public Color normal, highlighted, pressed, selected, disabled;
    [Header("Rarity Colors")]
    public Color common, uncommon, rare, epic, legendary, mythic;
    [Header("Alert Colors")]
    public Color info, warning, error, success;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            LoadPrefs();
        }
        else
        {
            Debug.LogWarning("Multiple instances of ProfileCustomization detected. Destroying duplicate.");   
            Destroy(this);
        }
        


    }

    public static void LoadPrefs()
    {
        infoLevel = GetInfoLevel();
        masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
    }

    public static void SavePrefs()
    {
        PlayerPrefs.SetString("infoLevel", infoLevel.ToString());
        Debug.Log("Saved infoLevel as " + infoLevel);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        // Debug.Log("Saved masterVolume as " + masterVolume);
        // Debug.Log("ProfileStats saved");
    }

    private static InfoLevel GetInfoLevel()
    {

        string raw = PlayerPrefs.GetString("infoLevel");

        if (int.TryParse(raw, out int intVal))
        {
            if (Enum.IsDefined(typeof(InfoLevel), intVal))
                return (InfoLevel)intVal;
        }
        return InfoLevel.balanced;


    }
}
