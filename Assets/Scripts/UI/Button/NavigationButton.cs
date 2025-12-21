using UnityEngine;
using System.Collections;

public class NavigationButtonHandler : ButtonHandler
{
    [SerializeField] protected float closeDelay = 0.2f;
    [Header("Use canvasesToClose and canvasesToOpen for instant open/close ONLY")]
    [SerializeField] protected Canvas[] canvasesToClose;
    [SerializeField] protected Canvas[] canvasesToOpen;
    [Header("Use canvasesToAnimate for animated open/close ONLY")]
    [SerializeField] protected OpenCloseAnimation[] canvasesToAnimate;
    
    override public void OnClick()
    {
        AnimateCanvases();
        base.OnClick();
    }
    override public void LateOnClick()
    {
        StartCoroutine(LateOnClickCoroutine());
    }

    protected IEnumerator LateOnClickCoroutine()
    {
        yield return new WaitForSeconds(closeDelay);
        CloseCanvases();
        
        OpenCanvases();

    }

    public void CloseCanvases()
    {
        foreach(Canvas c in canvasesToClose)
        {
            c.enabled = false;
        }
    }

    public void OpenCanvases()
    {
        foreach(Canvas c in canvasesToOpen)
        {
            c.enabled = true;
        }
    }

    public void AnimateCanvases()
    {
        foreach(OpenCloseAnimation oca in canvasesToAnimate)
        {
            oca.Toggle();
        }
    }
}
