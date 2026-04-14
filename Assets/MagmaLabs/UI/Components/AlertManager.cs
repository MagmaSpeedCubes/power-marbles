using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MagmaLabs.Animation;

namespace MagmaLabs.UI
{
    public class AlertManager : MonoBehaviour
    {

        [SerializeField]private Transform alertParent;
        [SerializeField]private GameObject simpleAlertPrefab, infoAlertPrefab;

        [HideInInspector]public static AlertManager instance;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Debug.LogWarning("Multiple instances of AlertManager detected. Destroying duplicate.");
                Destroy(this);
            }
        }

        public void BroadcastAlert(string message, float duration)
        {
            GameObject alert = Instantiate(simpleAlertPrefab, alertParent);
            
            
            Alert alertBody = alert.GetComponent<Alert>();

            alertBody.body.text = message;
            alertBody.duration = duration;
        }

        public void BroadcastAlert(string message, Sprite icon, float duration)
        {
            GameObject alert = Instantiate(simpleAlertPrefab, alertParent);
            Alert alertBody = alert.GetComponent<Alert>();

            alertBody.body.text = message;
            alertBody.icon.sprite = icon;
            alertBody.duration = duration;
        
        }

        
    }


}

