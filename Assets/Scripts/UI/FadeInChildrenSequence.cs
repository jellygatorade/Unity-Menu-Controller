using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInChildrenSequence : MonoBehaviour
{
    // Important:
    // All direct children must have a CanvasGroup component
    // Depends on AnimationHandler.NewFadeIn method

    [Tooltip("Duration of fade in animation in seconds.")]
    [SerializeField] private float AnimationDuration = 0.3f;

    private Coroutine SequenceCoroutine;
    private Coroutine AnimationCoroutine;

    /**
     * Reset all CanvasGroup components prior to launching animation
     */
    public void Setup(GameObject parent)
    {
        foreach(Transform child in parent.transform)
        {
            CanvasGroup CanvasGroup = child.gameObject.GetComponent<CanvasGroup>();
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
            CanvasGroup.alpha = 0;
        }
    }

    public void Animate(GameObject parent)
    {
        SequenceCoroutine = StartCoroutine(EnterChildren(parent, AnimationDuration));
    }

    public void Cancel()
    {
        if (SequenceCoroutine != null)
        {
            StopCoroutine(SequenceCoroutine);
        }

        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
    }

    private IEnumerator EnterChildren(GameObject parent, float delay)
    {
        foreach(Transform child in parent.transform)
        {
            AnimationCoroutine = StartCoroutine(AnimationHandler.NewFadeIn(child.gameObject.GetComponent<CanvasGroup>(), delay, null));
            yield return new WaitForSeconds(delay);
        }
    }
}
