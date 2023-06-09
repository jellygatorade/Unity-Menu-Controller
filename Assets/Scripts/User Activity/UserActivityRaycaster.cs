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
 * User activity detection with raycasting
 *
 * Using new input system 
 *
 * Raycasting is only required if action is conditional on knowing where user activity is taking place
 * For example, when tracking activity on different canvases in a split-screen game
 */

/**
 * To use new input system -->
 *
 * Follow
 * Unity C# - How to get the UI elements clicked (new input system)
 * https://www.youtube.com/watch?v=7h1cnGggY2M
 * Multiple Touches with the new input system (Android)?
 * https://www.reddit.com/r/unity_tutorials/comments/rkmn2l/multiple_touches_with_the_new_input_system_android/
 */

public class UserActivityRaycaster : MonoBehaviour
{
    // Normal raycasts do not work on UI elements, they require a special kind
    GraphicRaycaster raycaster;

    // [SerializeField] CanvasManager CanvasManager;

    // [SerializeField] ResetCountdown ResetCountdown;

    PointerEventData pointerData = new PointerEventData(EventSystem.current);
    List<RaycastResult> clickResults = new List<RaycastResult>();
    List<RaycastResult> touchResults = new List<RaycastResult>();

    [SerializeField] float InactivityTimeoutSeconds = 5;

    [HideInInspector]
    public bool TimerActive = true;

    void Awake()
    {
        this.raycaster = GetComponent<GraphicRaycaster>();

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
            // Debug.Log("Mouse.current.leftButton.wasPressedThisFrame");

            pointerData.position = Mouse.current.position.ReadValue();
            clickResults.Clear();

            this.raycaster.Raycast(pointerData, clickResults);

            foreach (RaycastResult clickResult in clickResults)
            {
                Debug.Log("Clicked " + clickResult.gameObject.name);
            }

            ResetInvokeOnUserInactive();
        }

        // Check if the touch initiated
        if (
            Touch.activeTouches.Count > 0 && 
            Touch.activeTouches[0].phase == TouchPhase.Began
        ) {
            // Debug.Log("At least one touch");

            Touch touch = Touch.activeTouches[0];

            // Raycast using the Graphics Raycaster and initial touch position (touch.rawPosition)
            pointerData.position = touch.screenPosition;
            touchResults.Clear();

            this.raycaster.Raycast(pointerData, touchResults);

            foreach (RaycastResult touchResult in touchResults)
            {
                Debug.Log("Touched " + touchResult.gameObject.name);
            }

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
