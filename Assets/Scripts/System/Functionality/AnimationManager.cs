using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Debug.LogWarning("Multiple instances of AnimationManager detected. Destroying duplicate.");
        }
    }



    public IEnumerator PopIn(GameObject subject, float overshoot, float duration)
    {
        Vector3 start = Vector3.zero;
        Vector3 peak = Vector3.one * overshoot;
        Vector3 end = Vector3.one;

        float t = 0f;

        // Grow + overshoot
        while (t < 1f)
        {
            t += Time.deltaTime / (duration * 0.6f);
            subject.transform.localScale = Vector3.LerpUnclamped(start, peak, CustomFunctions.EaseOutBack(t));
            yield return null;
        }

        t = 0f;

        // Settle back
        while (t < 1f)
        {
            t += Time.deltaTime / (duration * 0.4f);
            subject.transform.localScale = Vector3.Lerp(peak, end, t);
            yield return null;
        }

        subject.transform.localScale = end;
    }



    public IEnumerator FadeUI(GameObject subject, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            subject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, endAlpha, CustomFunctions.EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        subject.GetComponent<CanvasGroup>().alpha = endAlpha;
    }

    public IEnumerator FadeSprite(GameObject subject, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {

            Color oc = subject.GetComponent<SpriteRenderer>().color;
            float alpha = alpha = Mathf.Lerp(startAlpha, endAlpha, CustomFunctions.EaseInOutCubic(elapsedTime / duration));
            Color nc = new Color(oc.r, oc.g, oc.b, alpha);
            subject.GetComponent<SpriteRenderer>().color = nc;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        subject.GetComponent<CanvasGroup>().alpha = endAlpha;
    }
}