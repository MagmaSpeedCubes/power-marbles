using UnityEngine;
using System.Collections;

public class NavigationButtonHandler : ButtonHandler
{
    [SerializeField] protected float closeDelay = 0f;

    [SerializeReference] protected OpenCloseAnimation[] canvasesToAnimate;
    
    override public void OnClick()
    {
        base.OnClick();
        AnimateCanvases();
    }

    override public void LateOnClick()
    {
        // Called after pop animation completes (or immediately if popOnClick is disabled)
        
    }

    public void AnimateCanvases()
    {
        foreach(OpenCloseAnimation oca in canvasesToAnimate)
        {
            oca?.Toggle();
        }
    }
}
