using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

//  Alias namespaces to clear up ambiguous references
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase =  UnityEngine.InputSystem.TouchPhase;

/**
 * User activity detection without raycasting
 *
 * Using new input system 
 *
 * Raycasting is only required if action is conditional on knowing where user activity is taking place
 * For example, when tracking activity on different canvases in a split-screen game
 */

public class UserActivity : MonoBehaviour
{
    [SerializeField] ResetCountdown ResetCountdown;

    [SerializeField] ViewController ViewController;

    [SerializeField] float InactivityTimeoutSeconds = 5;

    private bool TimerActive = false;

    void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    void Start() 
    {   
        ResetInvokeOnUserInactive();
    }

    private void OnCancel() 
    {   
        if (!ViewController.IsViewOnTopOfStack(ViewController.IdleView))
        {
            ResetCountdown.StopAndResetTimer();
            setTimerActive();
        }

        ResetInvokeOnUserInactive();
    }

    private void Update()
    {   
        // Cancel is captured as a user activity action above (Esc key)

        // Check if the left mouse button is clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Debug.Log("Mouse.current.leftButton.wasPressedThisFrame");
            ResetInvokeOnUserInactive();
        }

        // Check if the touch initiated
        if (
            Touch.activeTouches.Count > 0 && 
            Touch.activeTouches[0].phase == TouchPhase.Began
        ) {
            // Debug.Log("At least one touch");
            ResetInvokeOnUserInactive();
        }
    }

    // Could use a coroutine here instead of invoke
    // StartCoroutine, WaitForSeconds, StopCoroutine
    public void ResetInvokeOnUserInactive()
    {
        //Debug.Log("User is active on " + this.gameObject);

        CancelInvoke("OnUserInactive");

        // Remove inactivity timeout countdown
        if (ViewController.IsViewOnTopOfStack(ViewController.InactivityView))
        {
            ViewController.PopOneView();
            ResetCountdown.StopAndResetTimer();
            setTimerActive();
        }

        if (TimerActive)
        {
            // Debug.Log("TimerActive, resetting");
            Invoke("OnUserInactive", InactivityTimeoutSeconds);
        }
        else 
        {
            // Debug.Log("Timer is not active, not resetting");
        }
    }

    private void OnUserInactive()
    {
        // Debug.Log("User is inactive on " + this.gameObject);
        
        setTimerInactive();
        ResetCountdown.RestartTimer();
        ViewController.PushView(ViewController.InactivityView);
    }

    public void setTimerInactive()
    {
        // Debug.Log("setTimerInactive");
        TimerActive = false;
    }

    public void setTimerActive()
    {
        // Debug.Log("setTimerActive");
        TimerActive = true;
    }
}
