using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnimationHandler
{
    /**
     * Original Zoom In / Out by LlamaAcademy
     * Accepts a "Speed" param
     */
    public static IEnumerator OriginalZoomIn(RectTransform Transform, float Speed, UnityEvent OnEnd)
    {
        float time = 0;
        while (time < 1)
        {
            Transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        Transform.localScale = Vector3.one;

        OnEnd?.Invoke();
    }

    public static IEnumerator OriginalZoomOut(RectTransform Transform, float Speed, UnityEvent OnEnd)
    {
        float time = 0;
        while (time < 1)
        {
            Transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        Transform.localScale = Vector3.zero;
        OnEnd?.Invoke();
    }

    /**
     * Original Fade In / Out by LlamaAcademy
     * Accepts a "Speed" param
     */
    public static IEnumerator OriginalFadeIn(CanvasGroup CanvasGroup, float Speed, UnityEvent OnEnd)
    {
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.interactable = true;

        float time = 0;
        while (time < 1)
        {
            CanvasGroup.alpha = Mathf.Lerp(0, 1, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        CanvasGroup.alpha = 1;
        OnEnd?.Invoke();
    }

    public static IEnumerator OriginalFadeOut(CanvasGroup CanvasGroup, float Speed, UnityEvent OnEnd)
    {
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;

        float time = 0;
        while (time < 1)
        {
            CanvasGroup.alpha = Mathf.Lerp(1, 0, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        CanvasGroup.alpha = 0;
        OnEnd?.Invoke();
    }

    /**
     * Original Zoom In / Out by LlamaAcademy
     * Accepts a "Speed" param
     */
    public static IEnumerator OriginalSlideIn(RectTransform Transform, Direction Direction, float Speed, UnityEvent OnEnd)
    {
        Vector2 startPosition;
        switch (Direction)
        {
            case Direction.UP:
                startPosition = new Vector2(0, -Screen.height);
                break;
            case Direction.RIGHT:
                startPosition = new Vector2(-Screen.width, 0);
                break;
            case Direction.DOWN:
                startPosition = new Vector2(0, Screen.height);
                break;
            case Direction.LEFT:
                startPosition = new Vector2(Screen.width, 0);
                break;
            default:
                startPosition = new Vector2(0, -Screen.height);
                break;
        }

        float time = 0;
        while (time < 1)
        {
            Transform.anchoredPosition = Vector2.Lerp(startPosition, Vector2.zero, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        Transform.anchoredPosition = Vector2.zero;
        OnEnd?.Invoke();
    }

    public static IEnumerator OriginalSlideOut(RectTransform Transform, Direction Direction, float Speed, UnityEvent OnEnd)
    {
        Vector2 endPosition;
        switch (Direction)
        {
            case Direction.UP:
                endPosition = new Vector2(0, Screen.height);
                break;
            case Direction.RIGHT:
                endPosition = new Vector2(Screen.width, 0);
                break;
            case Direction.DOWN:
                endPosition = new Vector2(0, -Screen.height);
                break;
            case Direction.LEFT:
                endPosition = new Vector2(-Screen.width, 0);
                break;
            default:
                endPosition = new Vector2(0, Screen.height);
                break;
        }

        float time = 0;
        while (time < 1)
        {
            Transform.anchoredPosition = Vector2.Lerp(Vector2.zero, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }

        Transform.anchoredPosition = endPosition;
        OnEnd?.Invoke();
    }

    /**
     * Kevin's updated Zoom In / Out
     *
     * Accepts a duration / seconds param
     * Uses while loop
     */
    public static IEnumerator NewZoomIn(RectTransform Transform, float seconds, UnityEvent OnEnd)
    {
        float time = 0;
        float duration = seconds;

        while (time < duration)
        {
            Transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / duration);  
            yield return null;
            time += Time.deltaTime;
        }

        Transform.localScale = Vector3.one;

        OnEnd?.Invoke();
    }

    public static IEnumerator NewZoomOut(RectTransform Transform, float seconds, UnityEvent OnEnd)
    {
        float time = 0;
        float duration = seconds;

        while (time < duration)
        {
            Transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time / duration);  
            yield return null;
            time += Time.deltaTime;
        }

        Transform.localScale = Vector3.zero;
        OnEnd?.Invoke();
    }

    /**
     * Kevin's updated Fade In / Out
     *
     * Accepts a duration / seconds param
     * Uses while loop
     */
    public static IEnumerator NewFadeIn(CanvasGroup CanvasGroup, float seconds, UnityEvent OnEnd)
    {
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.interactable = true;

        float time = 0;
        float duration = seconds;

        while (time < duration)
        {
            CanvasGroup.alpha = Mathf.Lerp(0, 1, time / duration); // linearly interpolates between a and b by t - Mathf.Lerp(a, b, t)    
            yield return null;
            time += Time.deltaTime;
        }

        CanvasGroup.alpha = 1;
        OnEnd?.Invoke();
    }

    public static IEnumerator NewFadeOut(CanvasGroup CanvasGroup, float seconds, UnityEvent OnEnd)
    {
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;

        float time = 0;
        float duration = seconds;

        while (time < duration)
        {
            CanvasGroup.alpha = Mathf.Lerp(1, 0, time / duration);
            yield return null;
            time += Time.deltaTime;
        }

        CanvasGroup.alpha = 0;
        OnEnd?.Invoke();
    }

    /**
     * Kevin's updated Slide In / Out
     *
     * Accepts a duration / seconds param
     * Uses while loop
     */
    public static IEnumerator NewSlideIn(RectTransform Transform, Direction Direction, float seconds, UnityEvent OnEnd)
    {
        Vector2 startPosition;
        switch (Direction)
        {
            case Direction.UP:
                startPosition = new Vector2(0, -Screen.height);
                break;
            case Direction.RIGHT:
                startPosition = new Vector2(-Screen.width, 0);
                break;
            case Direction.DOWN:
                startPosition = new Vector2(0, Screen.height);
                break;
            case Direction.LEFT:
                startPosition = new Vector2(Screen.width, 0);
                break;
            default:
                startPosition = new Vector2(0, -Screen.height);
                break;
        }

        float time = 0;
        float duration = seconds;

        while (time < duration)
        {
            Transform.anchoredPosition = Vector2.Lerp(startPosition, Vector2.zero, time / duration);
            yield return null;
            time += Time.deltaTime;
        }

        Transform.anchoredPosition = Vector2.zero;
        OnEnd?.Invoke();
    }

    public static IEnumerator NewSlideOut(RectTransform Transform, Direction Direction, float seconds, UnityEvent OnEnd)
    {
        Vector2 endPosition;
        switch (Direction)
        {
            case Direction.UP:
                endPosition = new Vector2(0, Screen.height);
                break;
            case Direction.RIGHT:
                endPosition = new Vector2(Screen.width, 0);
                break;
            case Direction.DOWN:
                endPosition = new Vector2(0, -Screen.height);
                break;
            case Direction.LEFT:
                endPosition = new Vector2(-Screen.width, 0);
                break;
            default:
                endPosition = new Vector2(0, Screen.height);
                break;
        }

        float time = 0;
        float duration = seconds;

        while (time < duration)
        {
            Transform.anchoredPosition = Vector2.Lerp(Vector2.zero, endPosition, time / duration);
            yield return null;
            time += Time.deltaTime;
        }

        Transform.anchoredPosition = endPosition;
        OnEnd?.Invoke();
    }

    /**
     * Kevin's first test to update Fade In / Out
     *
     * Accepts a duration / seconds param
     * Uses for loop
     */
    // public static IEnumerator NewFadeIn2(CanvasGroup CanvasGroup, float seconds, UnityEvent OnEnd)
    // {
    //     CanvasGroup.blocksRaycasts = true;
    //     CanvasGroup.interactable = true;

    //     float time = 0f;
    //     float duration = seconds;

    //     for (time = 0f; time < duration; time += Time.deltaTime) {       
    //         CanvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);        
    //         yield return null;
    //     }

    //     CanvasGroup.alpha = 1;
    //     OnEnd?.Invoke();
    // }

    // public static IEnumerator NewFadeOut2(CanvasGroup CanvasGroup, float seconds, UnityEvent OnEnd)
    // {
    //     CanvasGroup.blocksRaycasts = false;
    //     CanvasGroup.interactable = false;

    //     float time = 0f;
    //     float duration = seconds;

    //     for (time = 0f; time < duration; time += Time.deltaTime) {       
    //         CanvasGroup.alpha = Mathf.Lerp(1, 0, time / duration);      
    //         yield return null;
    //     }

    //     CanvasGroup.alpha = 0;
    //     OnEnd?.Invoke();
    // }
}
