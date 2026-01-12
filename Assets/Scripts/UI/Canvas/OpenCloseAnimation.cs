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
    protected RectTransform parent;
    protected Canvas parentCanvas;
    protected CanvasGroup canvasGroup;
    private Coroutine currentAnimation;
    
    [SerializeField] private AnimationType openAnimation;
    [SerializeField] private Vector3 openPosition;
    
    [SerializeField] private AnimationType closeAnimation;
    [SerializeField] private Vector3 closePosition;
    [SerializeField] protected float duration = 0.5f;

    [SerializeField] protected bool isEntireCanvas = true;

    protected bool isOpen = false;



    void Awake()
    {
        parent = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Initialize to closed state to avoid jumps on first Open
        if (!isOpen)
        {
            // set to configured close position and alpha using anchoredPosition (safer for UI)
            parent.anchoredPosition = new Vector2(closePosition.x, closePosition.y);
            if (closeAnimation == AnimationType.Fade && canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
            if (parentCanvas != null)
                parentCanvas.enabled = false;
        }
    }

    virtual public void Open()
    {
        StartCoroutine(OpenCoroutine());
            }

    virtual public IEnumerator OpenCoroutine()
    {
        if (!isOpen)
        {
            Debug.Log("Beginning open animation");
            isOpen = true;
            if (parentCanvas != null)
            {
                parentCanvas.enabled = true;
                // Force all descendants' layouts to rebuild immediately
                // This ensures grandchildren and deeper elements are properly positioned before animation starts
                Canvas.ForceUpdateCanvases();
            }

            // stop any ongoing animation
            if (currentAnimation != null) StopCoroutine(currentAnimation);

            if (openAnimation == AnimationType.Slide)
            {
                // ensure starting anchored position matches closePosition before animating
                parent.anchoredPosition = new Vector2(closePosition.x, closePosition.y);
                // Start slide animation immediately (now that all layouts are updated)
                currentAnimation = StartCoroutine(Slide(openPosition, duration));
                yield return currentAnimation;
            }
            else if (openAnimation == AnimationType.Fade)
            {
                currentAnimation = StartCoroutine(Fade(0, 1, duration));
                yield return currentAnimation;
            }

        }
    }



    virtual public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            // stop any ongoing animation
            if (currentAnimation != null) StopCoroutine(currentAnimation);

            if (closeAnimation == AnimationType.Slide)
            {
                currentAnimation = StartCoroutine(Slide(closePosition, duration));
            }
            else if (closeAnimation == AnimationType.Fade)
            {
                currentAnimation = StartCoroutine(Fade(1, 0, duration));
            }

        }
        
    }

    virtual public void Toggle()
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
        // if we just closed, disable the canvas to avoid showing the panel
        if (!isOpen && parentCanvas != null && isEntireCanvas)
            parentCanvas.enabled = false;
        currentAnimation = null;
    }

    public IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (canvasGroup != null)
            canvasGroup.alpha = endAlpha;

        // disable canvas when faded out
        if (endAlpha <= 0f && parentCanvas != null && isEntireCanvas){
            parentCanvas.enabled = false;

        }
            
        currentAnimation = null;
    }

    public IEnumerator Fade(Canvas canvas, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, endAlpha, EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvas.GetComponent<CanvasGroup>().alpha = endAlpha;
    }





    public static float EaseInOutCubic(float t)
    {
        if (t < 0.5) return InCubic(t * 2) / 2;
        return 1 - InCubic((1 - t) * 2) / 2;
    }
    public static float InCubic(float t) => t * t * t;

}
