using UnityEngine;
using System.Collections;
[System.Serializable]
public enum AnimationType
{
    Slide,
    Fade

}
[RequireComponent(typeof(CanvasGroup))]
public class OpenCloseAnimation : MonoBehaviour
{
    private RectTransform parent;
    
    [SerializeField] private AnimationType openAnimation;
    [SerializeField] private Vector3 openPosition;
    
    [SerializeField] private AnimationType closeAnimation;
    [SerializeField] private Vector3 closePosition;
    [SerializeField] private float duration = 1f;
    private bool isOpen = false;



    void Awake()
    {
        parent = GetComponent<RectTransform>();
    }

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            if (openAnimation == AnimationType.Slide)
            {
                StartCoroutine(Slide(openPosition, duration));
            }
            else if (openAnimation == AnimationType.Fade)
            {
                StartCoroutine(Fade(0, 1, duration));
            }
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            if (closeAnimation == AnimationType.Slide)
            {
                StartCoroutine(Slide(closePosition, duration));
            }
            else if (closeAnimation == AnimationType.Fade)
            {
                StartCoroutine(Fade(1, 0, duration));
            }
        }
    }

    public void Toggle()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public IEnumerator Slide(Vector3 endPosition, float duration)
    {
        Vector3 startPosition = parent.localPosition;


        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            parent.localPosition = Vector3.Lerp(startPosition, endPosition, EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        parent.localPosition = endPosition;
        Debug.Log("End Position: " + parent.localPosition);
    }

    public IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            parent.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, endAlpha, EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        parent.GetComponent<CanvasGroup>().alpha = endAlpha;
    }




    public static float EaseInOutCubic(float t)
    {
        if (t < 0.5) return InCubic(t * 2) / 2;
        return 1 - InCubic((1 - t) * 2) / 2;
    }
    public static float InCubic(float t) => t * t * t;

}
