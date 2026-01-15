using MagmaLabs.UI;
using UnityEngine;

public class TextTester : MonoBehaviour
{
    [SerializeField] private TextMeshMaxUGUI text;

    void Start()
    {
        text.SetText("Hello, MagmaLabs!");
    }

    
}
