using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
[DisallowMultipleComponent]
public class ViewController : MonoBehaviour
{
    [SerializeField] private View InitializationView;
    [SerializeField] public View IdleView;
    [SerializeField] private GameObject FirstFocusItem;
    [SerializeField] private View MainMenuView;
    [SerializeField] public View InactivityView;

    private Canvas RootCanvas;

    private Stack<View> ViewStack = new Stack<View>();

    private List<Coroutine> VCCoroutineList = new List<Coroutine>();

    private void Awake()
    {
        RootCanvas = GetComponent<Canvas>();

        EventManager.AddListener("UserActive", OnUserActive);
        EventManager.AddListener("UserInactive", OnUserInactive);
        EventManager.AddListener("UserTimeout", OnUserTimeout);
    }

    private void OnUserActive(Dictionary<string, object> message)
    {
        if (IsViewOnTopOfStack(InactivityView))
        {
            PopOneView();
        }
    }

    private void OnUserInactive(Dictionary<string, object> message)
    {
        if (!IsViewOnTopOfStack(InactivityView))
        {
            PushView(InactivityView);
        }
    }

    private void OnUserTimeout(Dictionary<string, object> message)
    {
        PopAllViews();
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
                PopOneView();
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

    // Switch from InitializationView to IdleView
    // This prevents a loading view from being at the bottom of the stack
    public void InitIdleView()
    {
        if (IsViewOnTopOfStack(InitializationView))
        {
            View currentView = ViewStack.Pop();
            currentView.Exit(false);
            
            ResetCoroutineList();
            VCCoroutineList.Add(StartCoroutine(DelayEnter(IdleView.AnimationDuration, IdleView, audio: true, push: true)));

            if (FirstFocusItem != null)
            {
                EventSystem.current.SetSelectedGameObject(FirstFocusItem);
            }
        }
    }

    // REVISIT IF ELSE STACK HERE
    public void PushView(View View)
    {
        if (ViewStack.Count > 0)
        {   
            if (IsViewOnTopOfStack(View))
            {
                return;
            }

            if (View.AlwaysOverlay)
            {
                View.Enter(true);
                ViewStack.Push(View);
            }
            else
            {
                ResetCoroutineList();
                float entryDelay = ExitVisibleViews(audio: false);
                VCCoroutineList.Add(StartCoroutine(DelayEnter(entryDelay, View, audio: true, push: true)));
            }
        }
        else
        {
            View.Enter(true);
            ViewStack.Push(View);
        }
    }

    public void PopOneView()
    {
        if (ViewStack.Count > 1)
        {
            View lastView = ViewStack.Pop();
            lastView.Exit(true);

            // transition in next view(s) if not already shown due to View.AlwaysOverlay
            if (!lastView.AlwaysOverlay)
            {
                ResetCoroutineList();
                List<View> ViewsToEnter = FindVisibleViews(keep: false);
                foreach (View view in ViewsToEnter)
                {
                    VCCoroutineList.Add(StartCoroutine(DelayEnter(lastView.AnimationDuration, view, audio: false, push: true)));
                }
            }
        }
        else
        {
            Debug.LogWarning($"Trying to pop a view but only 1 view remains in the stack! {ViewStack.Peek().name}");
        }
    }

    public void PopViewsToMainMenu()
    {
        // Transition out visible views, then pop all
        float entryDelay = ExitVisibleViews(audio: true);

        int numViews = ViewStack.Count;
        for (int i = 0; i < numViews; i++)
        {
            ViewStack.Pop();
        }

        ViewStack.Push(IdleView);

        // Transition in MainMenuView
        ResetCoroutineList();
        VCCoroutineList.Add(StartCoroutine(DelayEnter(entryDelay, MainMenuView, audio: false, push: true)));
    }

    public void PopAllViews()
    {
        // Transition out visible views, then pop all
        float entryDelay = ExitVisibleViews(audio: true);

        int numViews = ViewStack.Count;
        for (int i = 0; i < numViews; i++)
        {
            ViewStack.Pop();
        }

        // Transition in IdleView
        ResetCoroutineList();
        VCCoroutineList.Add(StartCoroutine(DelayEnter(entryDelay, IdleView, audio: false, push: true)));
    }

    private float ExitVisibleViews(bool audio)
    {
        List<View> VisibleViews = FindVisibleViews(keep: true);
        float LongestAnim = 0.0f;
        bool first = true;
        bool playAudio = audio;

        foreach (View view in VisibleViews)
        {
            if (first)
            {
                view.Exit(playAudio);
                first = false;
            }
            else 
            {
                view.Exit(false);
            }

            if (view.AnimationDuration > LongestAnim) LongestAnim = view.AnimationDuration;
        }

        return LongestAnim;
    }
    
    private List<View> FindVisibleViews(bool keep)
    {
        int numViews = ViewStack.Count;
        List<View> VisibleViews = new List<View>();

        for (int i = 0; i < numViews; i++)
        {   
            View view = ViewStack.Pop();

            VisibleViews.Insert(0, view); // insert at beginning

            if (!view.AlwaysOverlay) break;
        }

        if (keep) // ensure views remain in ViewStack
        {
            for (int i = 0; i < VisibleViews.Count; i++)
            {
                ViewStack.Push(VisibleViews[i]);
            }
        }

        return VisibleViews;
    }

    private IEnumerator DelayEnter(float seconds, View View, bool audio, bool push)
    {
        yield return new WaitForSeconds(seconds);
        
        View.Enter(audio);

        if (push)
        {
            ViewStack.Push(View);
        }
    }

    private void ResetCoroutineList()
    {
        if (VCCoroutineList.Count > 0)
        {
            foreach (Coroutine CR in VCCoroutineList)
            {
                StopCoroutine(CR);
            }

            VCCoroutineList.Clear();
        }
    }
}
