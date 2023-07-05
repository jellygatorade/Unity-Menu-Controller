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

    private void OnUserActive()
    {   
        EventManager.TriggerEvent("UserActive", new Dictionary<string, object> { { "event", "UserActive" } });
        ResetInvokeOnUserInactive();
    }


    private void OnCancel() 
    {   
        OnUserActive();
    }

    private void Update()
    {   
        // Cancel is captured as a user activity action above (Esc key)

        // Check if the left mouse button is clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnUserActive();
        }

        // Check if the touch initiated
        // (at least one touch)
        if (
            Touch.activeTouches.Count > 0 && 
            Touch.activeTouches[0].phase == TouchPhase.Began
        ) {
            OnUserActive();
        }
    }

    // Could use a coroutine here instead of invoke
    // StartCoroutine, WaitForSeconds, StopCoroutine
    public void ResetInvokeOnUserInactive()
    {
        // Debug.Log("User is active on " + this.gameObject);

        CancelInvoke("OnUserInactive");
        Invoke("OnUserInactive", InactivityTimeoutSeconds);
    }

    private void OnUserInactive()
    {
        // Debug.Log("User is inactive on " + this.gameObject);

        if (TimerActive)
        {
            EventManager.TriggerEvent("UserInactive", new Dictionary<string, object> { { "event", "UserInactive" } });
        }
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
