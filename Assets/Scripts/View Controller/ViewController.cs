using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
[DisallowMultipleComponent]
public class ViewController : MonoBehaviour
{
    [SerializeField]
    private View InitialView;
    [SerializeField]
    private GameObject FirstFocusItem;

    private Canvas RootCanvas;

    private Stack<View> ViewStack = new Stack<View>();

    private Coroutine VCCoroutine;

    private void Awake()
    {
        RootCanvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        if (FirstFocusItem != null)
        {
            EventSystem.current.SetSelectedGameObject(FirstFocusItem);
        }

        if (InitialView != null)
        {
            PushView(InitialView);
        }
    }

    private void OnCancel()
    {
        if (RootCanvas.enabled && RootCanvas.gameObject.activeInHierarchy)
        {
            if (ViewStack.Count != 0)
            {
                PopView();
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

    public void PushView(View View)
    {
        if (ViewStack.Count > 0)
        {
            View currentView = ViewStack.Peek();

            if (View.AlwaysOverlay) // disgregard currentView.ExitOnNewViewPush
            {
                View.Enter(true);
                ViewStack.Push(View);
            }
            else if (currentView.ExitOnNewViewPush)
            {
                currentView.Exit(false);
                ResetCoroutine();
                VCCoroutine = StartCoroutine(DelayEnter(currentView.AnimationDuration, View, true));
            }
            else
            {
                View.Enter(true);
                ViewStack.Push(View);
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
                if (newCurrentView.ExitOnNewViewPush)
                {
                    ResetCoroutine();
                    VCCoroutine = StartCoroutine(DelayEnter(lastView.AnimationDuration, newCurrentView, false));
                }
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

        for (int i = 1; i < numViews; i++)
        {
            PopView();
        }
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
