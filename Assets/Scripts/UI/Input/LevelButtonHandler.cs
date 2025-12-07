using UnityEngine;
using TMPro;
public class LevelButtonHandler : UIElementHandler{
    public void OnClick()
    {
        GameObject textObject = transform.Find("LevelButtonText").gameObject;
        int levelNumber = int.Parse(textObject.GetComponent<TextMeshProUGUI>().text);
        LevelManager.instance.LoadLevel(levelNumber);

        GameObject levelSelectCanvas = GameObject.Find("LevelSelect");
        levelSelectCanvas.GetComponent<Canvas>().enabled = false;
    }
}
