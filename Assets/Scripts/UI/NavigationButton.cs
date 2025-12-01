using UnityEngine;

public class NavigationButton : MonoBehaviour
{
    [SerializeField] private Canvas[] canvasesToClose;
    [SerializeField] private Canvas[] canvasesToOpen;

    public void OnClick()
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
