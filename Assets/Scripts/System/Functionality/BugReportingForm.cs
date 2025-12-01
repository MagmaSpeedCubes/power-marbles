using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class BugReportingForm : MonoBehaviour
{
    
    string formLink = "https://docs.google.com/forms/d/e/1FAIpQLSf11RU2WvhAe-fStO2g6jQndZnyJOQeNGO0rfEEPMtQOX8dHA/viewform";

    //https://docs.google.com/forms/d/e/1FAIpQLSf11RU2WvhAe-fStO2g6jQndZnyJOQeNGO0rfEEPMtQOX8dHA/viewform?usp=pp_url&&&entry.&&&&
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI methodOfContact, contactInfo, issue, howOften, stepsToReproduce, details;
    IEnumerator SubmitForm()
    {
        // Build query string with form entries
        string queryString = $"?usp=pp_url" +
            $"&entry.1723489875={UnityWebRequest.EscapeURL(GameCenterBridge.username)}" +
            $"&entry.999409435={UnityWebRequest.EscapeURL(methodOfContact.text)}" +
            $"&entry.714105628={UnityWebRequest.EscapeURL(contactInfo.text)}" +
            $"&entry.1559262901={UnityWebRequest.EscapeURL(issue.text)}" +
            $"&entry.2071647532={UnityWebRequest.EscapeURL(howOften.text)}" +
            $"&entry.1814473364={UnityWebRequest.EscapeURL(stepsToReproduce.text)}" +
            $"&entry.768415762={UnityWebRequest.EscapeURL(details.text)}" +
            $"&entry.527849321={UnityWebRequest.EscapeURL("" + SecureProfileStats.instance.GetDevSupport())}";

        string submitUrl = "https://docs.google.com/forms/d/e/1FAIpQLSf11RU2WvhAe-fStO2g6jQndZnyJOQeNGO0rfEEPMtQOX8dHA/formResponse" + queryString;

        using UnityWebRequest req = UnityWebRequest.Get(submitUrl);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            AlertManager.instance.ThrowUIError("Failed to submit form");
            Debug.LogError(req.error);
        }
        else
        {
            AlertManager.instance.ThrowUISuccess("Form submitted successfully");
        }
    }



    public void OnSubmissionButtonClick()
    {
        StartCoroutine(SubmitForm());
    }




}
