using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class ResetCountdown : MonoBehaviour
{
    [HideInInspector]
    public bool timerIsRunning = false;

    [SerializeField] float InactivityCountdownSeconds = 15;
    private float timeRemaining;

    [Tooltip("LocalizedStringEvent that contains the variable for inserting countdown time")]
    [SerializeField] LocalizeStringEvent localizedStringEvent;
    private LocalizedString CountdownText;
    private StringVariable StringVariable = new StringVariable();
    private string stringTimeRemaining = "Clock not yet started.";

    [SerializeField] ViewController ViewController;
    [SerializeField] UserActivity UserActivity;

    void Awake()
    {   
        InitComponents();
    }

    private void InitComponents()
    {
        // Set up the StringVariable to be inserted into the Smart String
        StringVariable.Value = stringTimeRemaining;
        CountdownText = localizedStringEvent.StringReference;
        CountdownText.Add("VarTimeRemaining", StringVariable);

        // Set up the initial time remaining
        timeRemaining = InactivityCountdownSeconds;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {   
                // Increment countdown
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {   
                // Ensure timer is 0, stopped, and invoke inactivity actions
                timeRemaining = 0;
                timerIsRunning = false;

                ViewController.PopAllViews();
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        // Format the time to be printed to
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        stringTimeRemaining = string.Format("{0:00}:{1:00}", minutes, seconds); // as a string
        StringVariable.Value = stringTimeRemaining; // as a StringVariable to insert into a LocalizedStringEvent.StringReference

        // Debug.Log(StringVariable.Value + ", " + CountdownText);
    }

    public void RestartTimer()
    {
        timeRemaining = InactivityCountdownSeconds;
        timerIsRunning = true;
    }

    public void StopAndResetTimer()
    {
        timeRemaining = InactivityCountdownSeconds;
        timerIsRunning = false;
    }
}