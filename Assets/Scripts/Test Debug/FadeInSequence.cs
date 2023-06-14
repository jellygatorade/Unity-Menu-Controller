using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInSequence : MonoBehaviour
{
    public void LaunchAnim(GameObject parent)
    {
        StartCoroutine(EnterChildren(parent, 0.3f));
    }

    public void SetUpFadeIn(GameObject parent)
    {
        foreach(Transform child in parent.transform)
        {
            CanvasGroup CanvasGroup = child.gameObject.GetComponent<CanvasGroup>();
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
            CanvasGroup.alpha = 0;
        }
    }

    // public void FadeOutAll(GameObject parent)
    // {   
    //     Debug.Log("FadeOutAll");
    //     foreach(Transform child in parent.transform)
    //     {
    //         CanvasGroup CanvasGroup = child.gameObject.GetComponent<CanvasGroup>();
    //         CanvasGroup.blocksRaycasts = false;
    //         CanvasGroup.interactable = false;
    //         CanvasGroup.alpha = 0;
    //     }
    // }

    private IEnumerator EnterChildren(GameObject parent, float delay)
    {
        foreach(Transform child in parent.transform)
        {
            StartCoroutine(AnimationHandler.NewFadeIn(child.gameObject.GetComponent<CanvasGroup>(), delay, null));
            yield return new WaitForSeconds(delay);
        }
    }

    // private static IEnumerator FadeIn(CanvasGroup CanvasGroup, float seconds)
    // {
    //     CanvasGroup.blocksRaycasts = true;
    //     CanvasGroup.interactable = true;

    //     float time = 0;
    //     float duration = seconds;

    //     while (time < duration)
    //     {
    //         CanvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);  
    //         yield return null;
    //         time += Time.deltaTime;
    //     }

    //     CanvasGroup.alpha = 1;
    // }
}
