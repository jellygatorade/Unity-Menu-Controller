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

    private Coroutine CountdownCoroutine;

    private void Awake()
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

    private void DisplayTime(float timeToDisplay)
    {
        // Format the time
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        stringTimeRemaining = string.Format("{0:00}:{1:00}", minutes, seconds); // as a string
        StringVariable.Value = stringTimeRemaining; // as a StringVariable to insert into a LocalizedStringEvent.StringReference

        //Debug.Log($"DisplayTime! {stringTimeRemaining}");
    }

    public void RestartTimer()
    {
        if (CountdownCoroutine != null)
        {
            StopCoroutine(CountdownCoroutine);
        }

        timeRemaining = InactivityCountdownSeconds;
        CountdownCoroutine = StartCoroutine(DoTimer());
    }

    public void StopAndResetTimer()
    {
        if (CountdownCoroutine != null)
        {
            StopCoroutine(CountdownCoroutine);
        }

        timeRemaining = InactivityCountdownSeconds;
    }

    private IEnumerator DoTimer()
    {
        while (timeRemaining > 0)
        {   
            // Increment countdown
            DisplayTime(timeRemaining);
            timeRemaining--;

            yield return new WaitForSeconds(1f);
        }

        DisplayTime(timeRemaining); // 00:00
        yield return new WaitForSeconds(1f);

        ViewController.PopAllViews();
    }
}