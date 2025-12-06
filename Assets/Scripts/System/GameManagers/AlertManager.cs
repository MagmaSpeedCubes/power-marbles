using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;
    public GameObject alertPrefab;
    public GameObject[] specialButtons;
    

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple AlertManagers detected: Destroying duplicate.");
        }
    }

    void Start()
    {
        //test
        //ThrowUIInfo("This is a game", new string[] {"Alex made this"});
        //ThrowUIError("This Game Sucks", new string[] {"Alex is bad at game dev"});
    }



    // Update is called once per frame
    public AlertHandler ThrowUIAlert(string type, string title, string[] messages)
    {
        GameObject alert = Instantiate(alertPrefab, transform);
        AlertHandler ah = alert.GetComponent<AlertHandler>();
        ah.Initialize(type, title, messages);

        return ah;
    }

    public GameObject FindFirstObjectFromName(GameObject[] arr, string name)
    {
        foreach(GameObject obj in arr)
        {
            if(obj.name == name){
                return obj;
            }
        }
        return null;
    }

    public void ThrowDataError(string title, string[] messages)
    {
        
    }

    public void ThrowUIError(string title, string[] messages)
    {
        AlertHandler ah = ThrowUIAlert("error", title, messages);

        GameObject bugReportButton = FindFirstObjectFromName(specialButtons, "BugReportButton");

        ah.InitializeButton(bugReportButton);
    }


    public void ThrowUIWarning(string title, string[] messages)
    {
        AlertHandler ah = ThrowUIAlert("error", title, messages);

        ah.InitializeButton(null);
    }

    public void ThrowUIInfo(string title, string[] messages)
    {
        AlertHandler ah = ThrowUIAlert("info", title, messages);
        ah.InitializeButton(null);
    }

    public void ThrowUISuccess(string title, string[] messages)
    {
        AlertHandler ah = ThrowUIAlert("error", title, messages);

        ah.InitializeButton(null);
    }
}




