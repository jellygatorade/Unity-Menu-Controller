using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUIs : MonoBehaviour
{
    [SerializeField]
    private ViewController ViewController;

    private List<CanvasGroup> FoundCanvasGroups;

    void Awake()
    {
        var foundCanvasGroupArray = FindObjectsOfType<CanvasGroup>();
        FoundCanvasGroups = new List<CanvasGroup>(foundCanvasGroupArray);
 
    }

    public void ResetAll()
    {
        ViewController.PopAllViews();

        // foreach(CanvasGroup canvasGroup in FoundCanvasGroups)
        // {
        //     canvasGroup.blocksRaycasts = false;
        //     canvasGroup.interactable = false;
        //     canvasGroup.alpha = 0;
        // }
    }
}
