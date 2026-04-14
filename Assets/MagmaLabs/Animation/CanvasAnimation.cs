using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace MagmaLabs.Animation{
public class CanvasAnimation : MonoBehaviour
{
    public static CanvasAnimation instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Debug.LogWarning("Multiple instances of CanvasAnimation detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public static IEnumerator Fade(Canvas canvas, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, endAlpha, Easing.EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvas.GetComponent<CanvasGroup>().alpha = endAlpha;
    }

    public static IEnumerator Fade(Canvas fromCanvas, Canvas toCanvas, float duration)
    {
        toCanvas.enabled = true;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            fromCanvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1f, 0f, Easing.EaseInOutCubic(elapsedTime / duration));
            toCanvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0f, 1f, Easing.EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fromCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        toCanvas.GetComponent<CanvasGroup>().alpha = 1f;
        fromCanvas.enabled = false;
    }

    public static void LoadingScreen(Canvas fromCanvas, Canvas loadCanvas, Canvas toCanvas, float duration)
    {
        if (instance != null)
        {
            instance.StartCoroutine(LoadingScreenCoroutine(fromCanvas, loadCanvas, toCanvas, duration));
        }
    }

    public static IEnumerator LoadingScreenCoroutine(Canvas fromCanvas, Canvas loadCanvas, Canvas toCanvas, float duration)
    {
        yield return instance.StartCoroutine(Fade(fromCanvas, loadCanvas, duration / 4));
        yield return new WaitForSeconds(duration / 2);
        yield return instance.StartCoroutine(Fade(loadCanvas, toCanvas, duration / 4));
    }

    public static IEnumerator Slide(GameObject wrapper, Vector2 fromPosition, Vector2 toPosition, float duration)
    {
        RectTransform rectTransform = wrapper.GetComponent<RectTransform>();
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            rectTransform.localPosition = Vector2.Lerp(fromPosition, toPosition, Easing.EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.localPosition = toPosition;
    }

    

    

}
}