using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
[DisallowMultipleComponent]
public class ViewController : MonoBehaviour
{
    [SerializeField] private View HomeView;
    [SerializeField] private GameObject FirstFocusItem;
    [SerializeField] private View InitializationView;

    private Canvas RootCanvas;

    private Stack<View> ViewStack = new Stack<View>();

    private Coroutine VCCoroutine;

    private void Awake()
    {
        RootCanvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        if (InitializationView != null)
        {
            PushView(InitializationView);
        }
    }

    private void OnCancel()
    {
        if (RootCanvas.enabled && RootCanvas.gameObject.activeInHierarchy)
        {
            if (ViewStack.Count > 1)
            {
                PopView();
            }
            else if  (ViewStack.Count == 1)
            {
                Application.Quit();
            }
        }
    }

    public bool IsViewInStack(View View)
    {
        return ViewStack.Contains(View);
    }

    public bool IsViewOnTopOfStack(View View)
    {
        return ViewStack.Count > 0 && View == ViewStack.Peek();
    }

    // Switch from InitializationView to HomeView
    // This prevents a loading view from being at the bottom of the stack
    public void InitHomeView()
    {
        View currentView = ViewStack.Peek();

        if (currentView == InitializationView)
        {
            ViewStack.Pop();
            currentView.Exit(false);
            
            ResetCoroutine();
            VCCoroutine = StartCoroutine(DelayEnter(HomeView.AnimationDuration, HomeView, true));

            if (FirstFocusItem != null)
            {
                EventSystem.current.SetSelectedGameObject(FirstFocusItem);
            }
        }
    }

    public void PushView(View View)
    {
        if (ViewStack.Count > 0)
        {
            View currentView = ViewStack.Peek();

            if (View.AlwaysOverlay)
            {
                View.Enter(true);
                ViewStack.Push(View);
            }
            else
            {
                currentView.Exit(false);
                ResetCoroutine();
                VCCoroutine = StartCoroutine(DelayEnter(currentView.AnimationDuration, View, true));
            }
        }
        else
        {
            View.Enter(true);
            ViewStack.Push(View);
        }
    }

    public void PopView()
    {
        if (ViewStack.Count > 1)
        {
            View lastView = ViewStack.Pop();
            lastView.Exit(true);
            
            if (!lastView.AlwaysOverlay) // transition in next view if not already shown due to lastView.AlwaysOverlay
            {
                View newCurrentView = ViewStack.Peek();
                ResetCoroutine();
                VCCoroutine = StartCoroutine(DelayEnter(lastView.AnimationDuration, newCurrentView, false));
            }
        }
        else
        {
            Debug.LogWarning("Trying to pop a view but only 1 view remains in the stack!");
        }
    }

    public void PopAllViews()
    {
        int numViews = ViewStack.Count;
        bool foundLowestVisible = false;
        float entryDelay = 0.3f;

        // Transition out all views until first found that is not an overlay
        // Remove all others silently
        for (int i = 0; i < numViews; i++)
        {
            View currentView = ViewStack.Peek();

            if (foundLowestVisible)
            {
                ViewStack.Pop(); // remove silently
            }
            else 
            {
                ViewStack.Pop();
                currentView.Exit(true); // transition out
            }

            if (!foundLowestVisible && !currentView.AlwaysOverlay)
            {
                foundLowestVisible = true;
                entryDelay = currentView.AnimationDuration;
            }
        }

        // Transition in HomeView
        ResetCoroutine();
        VCCoroutine = StartCoroutine(DelayEnter(entryDelay, HomeView, true));
    }

    private IEnumerator DelayEnter(float seconds, View View, bool Push)
    {
        yield return new WaitForSeconds(seconds);
        
        View.Enter(Push);

        if (Push)
        {
            ViewStack.Push(View);
        }
    }

    private void ResetCoroutine()
    {
        if (VCCoroutine != null)
        {
            StopCoroutine(VCCoroutine);
        }
    }
}
