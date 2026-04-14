using UnityEngine;

using MagmaLabs.Economy;
using MagmaLabs.Editor;
[RequireComponent(typeof(SaveManager))]
public class SaveTester : MonoBehaviour
{
    public bool saveData;
    public bool loadData;

    void Start()
    {
        if (saveData)
        {
            SaveManager.instance.SaveString("testString", "Hello World");
            SaveManager.instance.SaveFloat("testFloat", 3.14f);
            SaveManager.instance.SaveInt("testInt", 42);
            SaveManager.instance.SaveBool("testBool", true);
            SaveManager.instance.Save();
            Debug.Log("Data Saved.");
        }

        if (loadData)
        {
            Debug.Log("Retrieving Data:");
            Debug.Log("String: " + SaveManager.instance.LoadString("testString"));
            Debug.Log("Float: " + SaveManager.instance.LoadFloat("testFloat"));
            Debug.Log("Int: " + SaveManager.instance.LoadInt("testInt"));
            Debug.Log("Bool: " + SaveManager.instance.LoadBool("testBool"));
        }
    }


}
