using UnityEngine;
using System.Collections;

public class NavigationButtonHandler : ButtonHandler
{
    [SerializeField] protected float closeDelay = 0f;

    [SerializeReference] protected OpenCloseAnimation[] canvasesToAnimate;
    
    override public void OnClick()
    {
        AnimateCanvases();
        base.OnClick();
        
    }


    public void AnimateCanvases()
    {
        foreach(OpenCloseAnimation oca in canvasesToAnimate)
        {
            oca?.Toggle();
        }
    }
}
