using UnityEngine;
using System.Collections;
public class LoadingScreenAnimation : OpenCloseAnimation
{
    [SerializeField] private Canvas openLoadingScreen;
    [SerializeField] private Canvas closeLoadingScreen;
    [SerializeField] private Canvas closeToScreen;
    [SerializeField] private Canvas openToScreen;

    public IEnumerator LoadingScreen(float duration, Canvas fromCanvas, Canvas loadCanvas, Canvas toCanvas)
    {
        loadCanvas.enabled = true;
        yield return StartCoroutine(Fade(loadCanvas, 0, 1, duration/4));
        //fade in the loading screen
        fromCanvas.GetComponent<Canvas>().enabled = false;
        fromCanvas.GetComponent<CanvasGroup>().alpha = 0;
        //remove the from canvas
        yield return new WaitForSeconds(duration/2);
        toCanvas.GetComponent<Canvas>().enabled = true;
        toCanvas.GetComponent<CanvasGroup>().alpha = 1;
        //add the to canvas
        yield return StartCoroutine(Fade(loadCanvas, 1, 0, duration/4));
        if (isEntireCanvas)
        {
            loadCanvas.enabled = false;
        }
        
        //fade out the loading screen

    }

    override public void Open()
    {
        StartCoroutine(LoadingScreen(duration, closeToScreen, openLoadingScreen, openToScreen));
        isOpen = true;
    }

    override public void Close()
    {
        StartCoroutine(LoadingScreen(duration, openToScreen, closeLoadingScreen, closeToScreen));
        isOpen = false;
        
    }

    override public void Toggle()
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

    
}
