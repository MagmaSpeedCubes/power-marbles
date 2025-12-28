using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class LoadingScreenAnimation : OpenCloseAnimation
{
    [SerializeField] private Canvas openLoadingScreen;
    [SerializeField] private Canvas closeLoadingScreen;
    [SerializeField] private Canvas closeToScreen;
    [SerializeField] private Canvas openToScreen;
    [SerializeField] private string openToScene;
    [SerializeField] private string closeToScene;

    public IEnumerator LoadingScreen(float duration, Canvas fromCanvas, Canvas loadCanvas, Canvas toCanvas)
    {
        yield return ToLoadingScreen(duration/4, fromCanvas, loadCanvas);
        yield return new WaitForSeconds(duration/2);
        yield return FromLoadingScreen(duration/4, toCanvas, loadCanvas);


    }

    public IEnumerator ToLoadingScreen(float duration, Canvas fromCanvas, Canvas loadCanvas)
    {
        loadCanvas.enabled = true;
        yield return StartCoroutine(Fade(loadCanvas, 0, 1, duration));
        //fade in the loading screen
        fromCanvas.GetComponent<Canvas>().enabled = false;
        fromCanvas.GetComponent<CanvasGroup>().alpha = 0;
    }

    public IEnumerator FromLoadingScreen(float duration, Canvas toCanvas, Canvas loadCanvas)
    {
        toCanvas.GetComponent<Canvas>().enabled = true;
        toCanvas.GetComponent<CanvasGroup>().alpha = 1;
        yield return StartCoroutine(Fade(loadCanvas, 1, 0, duration));
        if (isEntireCanvas)
        {
            loadCanvas.enabled = false;
        }
    }

    public IEnumerator SceneLoadingScreen(float duration, Canvas fromCanvas, Canvas loadCanvas, Canvas toCanvas, string sceneName)
    {
        yield return ToLoadingScreen(duration/4, fromCanvas, loadCanvas);
        SceneManager.LoadScene(sceneName);
        yield return FromLoadingScreen(duration/4, toCanvas, loadCanvas);
    }

    override public void Open()
    {
        if (openToScene.Equals(""))
        {
            StartCoroutine(LoadingScreen(duration, closeToScreen, openLoadingScreen, openToScreen));
        }
        else
        {
            StartCoroutine(SceneLoadingScreen(duration, closeToScreen, openLoadingScreen, openToScreen, openToScene));
        }

        
        isOpen = true;
    }

    override public void Close()
    {
        if(closeToScene.Equals("")){
            StartCoroutine(LoadingScreen(duration, openToScreen, closeLoadingScreen, closeToScreen));
        }
        else
        {
            StartCoroutine(SceneLoadingScreen(duration, openToScreen, closeLoadingScreen, closeToScreen, closeToScene));
        }
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
