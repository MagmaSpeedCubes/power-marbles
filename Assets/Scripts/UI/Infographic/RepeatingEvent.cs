using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class RepeatingEvent : Infographic
{
    [SerializeField] private string prefix, suffix;
    [SerializeField] private bool startOnLaunch;

    [ShowIf("startOnLaunch", false)]
    [SerializeField] private SerializableDateTime startDate; 
    [SerializeField] private float repeatIntervalDays = 7f; // Repeat interval in days (default: weekly)
    [SerializeField] private TextMeshProUGUI timerText;

    private DateTime nextOccurrenceUtc;
    private bool initialized = false;

    void Start(){
        if(startOnLaunch){
            startDate = new SerializableDateTime(Constants.LAUNCH_DATE);
        }
    }

    void Update()
    {
        // Initialize next occurrence on first update
        if (!initialized)
        {
            CalculateNextOccurrence();
            initialized = true;
        }

        TimeSpan timeLeft = nextOccurrenceUtc - DateTime.UtcNow;
        value = (float)timeLeft.TotalSeconds;
        
        // If current occurrence has passed, calculate the next one
        if (timeLeft.TotalSeconds <= 0)
        {
            CalculateNextOccurrence();
            timeLeft = nextOccurrenceUtc - DateTime.UtcNow;
        }

        if (timeLeft.TotalSeconds <= 0)
        {
            timerText.text = "Starting...";
        }
        else
        {
            string timerString = "";
            if(timeLeft.TotalSeconds < 60)
            {
                int seconds = (int)(timeLeft.TotalSeconds % 60);
                 timerString = timeLeft.TotalSeconds + "s";
            }else if(timeLeft.TotalSeconds < 3600)
            {
                int minutes = (int)(timeLeft.TotalSeconds / 60);
                int seconds = (int)(timeLeft.TotalSeconds % 60);
                 timerString = minutes + "m " + seconds + "s";
            }else if(timeLeft.TotalSeconds < 86400)
            {
                int hours = (int)(timeLeft.TotalSeconds / 3600);
                int minutes = (int)((timeLeft.TotalSeconds % 3600) / 60);
                 timerString = hours + "h " + minutes + "m";
            }else{
                int days = (int)(timeLeft.TotalSeconds / 86400);
                int hours = (int)((timeLeft.TotalSeconds % 86400) / 3600);
                 timerString = days + "d " + hours + "h";
            }
            timerText.text = prefix + timerString + suffix;

        }
    }

    private void CalculateNextOccurrence()
    {
        DateTime startUtc = startDate.ToDateTimeUtc();
        DateTime now = DateTime.UtcNow;
        TimeSpan repeatInterval = TimeSpan.FromDays(repeatIntervalDays);

        // If the start date is in the future, next occurrence is the start date
        if (startUtc > now)
        {
            nextOccurrenceUtc = startUtc;
            return;
        }

        // Calculate how many intervals have passed since start date
        TimeSpan elapsed = now - startUtc;
        long intervalsPasssed = (long)Math.Floor(elapsed.TotalDays / repeatIntervalDays);

        // Next occurrence is: start date + (intervals passed + 1) * repeat interval
        nextOccurrenceUtc = startUtc.AddDays((intervalsPasssed + 1) * repeatIntervalDays);
    }}
