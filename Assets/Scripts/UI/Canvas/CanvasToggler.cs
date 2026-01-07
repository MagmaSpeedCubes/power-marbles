using UnityEngine;

public class CanvasToggler : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public void Toggle()
    {
        canvas.enabled = !canvas.enabled;
    }
}
