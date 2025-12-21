using UnityEngine;
using System.Collections;

public class LinkButton : UIElementHandler
{
    [SerializeField] private string url;

    public void OnClick()
    {
        Application.OpenURL(url);
    }
}
