using UnityEngine;

public class NavigationButtonHandler : UIElementHandler
{
    [SerializeField] protected Canvas[] canvasesToClose;
    [SerializeField] protected Canvas[] canvasesToOpen;

    virtual public void OnClick()
    {
        foreach(Canvas c in canvasesToClose)
        {
            c.enabled = false;
        }
        foreach(Canvas c in canvasesToOpen)
        {
            c.enabled = true;
        }
    }
}
