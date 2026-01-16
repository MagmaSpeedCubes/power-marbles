using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using MagmaLabs.Utilities;
using MagmaLabs.UI;


public class CountdownClock : Infographic
{

    [SerializeField] private SerializableDateTime endTimeUtc; 
    [SerializeField] private TextMeshProUGUI timerText;

    void Update()
    {
        TimeSpan timeLeft = endTimeUtc.ToDateTimeUtc() - DateTime.UtcNow;
        value = (float)timeLeft.TotalSeconds;
        if (timeLeft.TotalSeconds <= 0)
        {
            timerText.text = "Ended";
        }
        else
        {


            if(timeLeft.TotalSeconds < 60)
            {
                int seconds = (int)(timeLeft.TotalSeconds % 60);
                timerText.text = timeLeft.TotalSeconds + "s";
            }else if(timeLeft.TotalSeconds < 3600)
            {
                int minutes = (int)(timeLeft.TotalSeconds / 60);
                int seconds = (int)(timeLeft.TotalSeconds % 60);
                timerText.text = minutes + "m " + seconds + "s";
            }else if(timeLeft.TotalSeconds < 86400)
            {
                int hours = (int)(timeLeft.TotalSeconds / 3600);
                int minutes = (int)((timeLeft.TotalSeconds % 3600) / 60);
                timerText.text = hours + "h " + minutes + "m";
            }else{
                int days = (int)(timeLeft.TotalSeconds / 86400);
                int hours = (int)((timeLeft.TotalSeconds % 86400) / 3600);
                timerText.text = days + "d " + hours + "h";
            }


        }
    }

    

}
