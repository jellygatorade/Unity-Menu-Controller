using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

// LEFT OFF HERE on 6/9/2023
// NEED TO DECIDE BEHAVIOR OF USER ACTIVITY DETECTION
// Raycasting is not necessary if I don't need to know what UI was touched (it's not split screen)
// Need to clean up readability + usage of new input system (UnityEngine.InputSystem)

/**
 * To use legacy input system -->
 *
 * Follow
 * Detect click on canvas
 * https://answers.unity.com/questions/1526663/detect-click-on-canvas.html
 ** 
 *
 * using UnityEngine.EventSystems;
 *
 * Input.GetKeyDown(KeyCode.Mouse0) - left mouse button clicked
 * Input.mousePosition
 * Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began
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

    PointerEventData clickPointerData = new PointerEventData(EventSystem.current);
    List<RaycastResult> clickResults = new List<RaycastResult>();

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
        //ResetOnUserInactiveInvoke(); // Currently calling this in CanvasManager.Start();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            clickPointerData.position = Mouse.current.position.ReadValue();
            clickResults.Clear();

            this.raycaster.Raycast(clickPointerData, clickResults);

            foreach (RaycastResult clickResult in clickResults)
            {
                Debug.Log("Clicked " + clickResult.gameObject.name);
            }
        }


        // //Check if the left mouse button is clicked
        // if (Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     //Set up the new Pointer Event
        //     PointerEventData clickPointerData = new PointerEventData(EventSystem.current);
        //     List<RaycastResult> clickResults = new List<RaycastResult>();

        //     //Raycast using the Graphics Raycaster and mouse click position
        //     clickPointerData.position = Input.mousePosition;
        //     this.raycaster.Raycast(clickPointerData, clickResults);

        //     // If raycast detects any clicks
        //     if (clickResults.Count > 0)
        //     {
        //         ResetOnUserInactiveInvoke();
        //     }

        //     //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        //     // foreach (RaycastResult clickResult in clickResults)
        //     // {
        //     //     Debug.Log("Clicked " + clickResult.gameObject.name);
        //     // }
        // }

        // //Check if the touch initiated
        // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        // {
        //     //Set up the new Pointer Event
        //     PointerEventData touchPointerData = new PointerEventData(EventSystem.current);
        //     List<RaycastResult> touchResults = new List<RaycastResult>();

        //     Touch touch = Input.GetTouch(0);

        //     //Raycast using the Graphics Raycaster and initial touch position (touch.rawPosition)
        //     touchPointerData.position = touch.rawPosition;
        //     this.raycaster.Raycast(touchPointerData, touchResults);

        //     // If raycast detects any touches
        //     if (touchResults.Count > 0)
        //     {
        //         ResetOnUserInactiveInvoke();
        //     }

        //     //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        //     // foreach (RaycastResult touchResult in touchResults)
        //     // {
        //     //     Debug.Log("Touched " + touchResult.gameObject.name);
        //     // }
        // }

        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 0  && UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Began) 
        {
            Debug.Log("At least one touch");
        }
    }

    // Could use a coroutine here instead of invoke
    // StartCoroutine, WaitForSeconds, StopCoroutine
    public void ResetOnUserInactiveInvoke()
    {
        Debug.Log("User is active on " + this.gameObject);
        //CanvasManager.FadeOut(CanvasManager.InactivityResetOverlay);
        //ResetCountdown.StopAndResetTimer();
        //CancelInvoke();
        //Debug.Log("TimerActive is " + TimerActive);
        if (TimerActive)
        {
            Debug.Log("TimerActive, resetting");
            //Invoke("OnUserInactive", InactivityTimeoutSeconds);
        }
        else 
        {
            //Debug.Log("Timer is not active, not resetting");
        }
    }

    void OnUserInactive()
    {
        Debug.Log("User is inactive on " + this.gameObject);
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
