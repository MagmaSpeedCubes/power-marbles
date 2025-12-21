using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class CountdownClock : Infographic
{

    [SerializeField] private DateTime endTimeUtc; 
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private CanvasGroup group;

    void Update()
    {
        TimeSpan timeLeft = endTimeUtc - DateTime.UtcNow;
        if (timeLeft.TotalSeconds <= 0)
        {
            group.alpha = 0f;
        }
        else
        {

            group.alpha = 1f;
            if(timeLeft.TotalSeconds < 60)
            {
                int seconds = (int)(timeLeft.TotalSeconds % 60);
                timerText.text = timeLeft.TotalSeconds + "s";
            }else if(timeLeft.TotalSeconds < 3600)
            {
                int minutes = (int)(timeLeft.TotalSeconds / 60);
                int seconds = (int)(timeLeft.TotalSeconds % 60);
                timerText.text = minutes + "m" + seconds + "s";
            }else if(timeLeft.TotalSeconds < 86400)
            {
                int hours = (int)(timeLeft.TotalSeconds / 3600);
                int minutes = (int)((timeLeft.TotalSeconds % 3600) / 60);
                timerText.text = hours + "h" + minutes + "m";
            }else{
                int days = (int)(timeLeft.TotalSeconds / 86400);
                int hours = (int)((timeLeft.TotalSeconds % 86400) / 3600);
                timerText.text = days + "d" + hours + "h";
            }


        }
    }

    

}
