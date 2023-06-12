using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    // [SerializeField] CanvasManager CanvasManager;

    // [SerializeField] ResetCountdown ResetCountdown;

    [SerializeField] float InactivityTimeoutSeconds = 5;

    [HideInInspector]
    public bool TimerActive = true;

    void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    void Start() 
    {
        ResetInvokeOnUserInactive();
    }

    void Update()
    {   
        // Check if the left mouse button is clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse.current.leftButton.wasPressedThisFrame");
            ResetInvokeOnUserInactive();
        }

        // Check if the touch initiated
        if (
            Touch.activeTouches.Count > 0 && 
            Touch.activeTouches[0].phase == TouchPhase.Began
        ) {
            Debug.Log("At least one touch");
            ResetInvokeOnUserInactive();
        }
    }

    // Could use a coroutine here instead of invoke
    // StartCoroutine, WaitForSeconds, StopCoroutine
    public void ResetInvokeOnUserInactive()
    {
        Debug.Log("User is active on " + this.gameObject);

        CancelInvoke();

        // Remove inactivity timeout countdown
        //CanvasManager.FadeOut(CanvasManager.InactivityResetOverlay);
        //ResetCountdown.StopAndResetTimer();

        if (TimerActive)
        {
            Debug.Log("TimerActive, resetting");
            Invoke("OnUserInactive", InactivityTimeoutSeconds);
        }
        else 
        {
            Debug.Log("Timer is not active, not reseting");
        }
    }

    void OnUserInactive()
    {
        Debug.Log("User is inactive on " + this.gameObject);
        Debug.Log("Launch an inactivity timeout countdown");

        // Launch the reset timer countdown

        //CanvasManager.FadeIn(CanvasManager.InactivityResetOverlay);
        //ResetCountdown.RestartTimer();
    }

    public void setTimerInactive()
    {
        TimerActive = false;
    }

    public void setTimerActive()
    {
        TimerActive = true;
    }
}
