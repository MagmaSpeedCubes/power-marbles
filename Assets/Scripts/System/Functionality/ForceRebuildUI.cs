using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ForceRebuildUI : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null; // wait 1 frame
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            GetComponent<RectTransform>()
        );
    }
}
