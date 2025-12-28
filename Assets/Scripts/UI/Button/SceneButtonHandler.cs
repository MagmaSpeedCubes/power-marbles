using UnityEngine;

public class SceneButtonHandler : NavigationButtonHandler
{
    public string sceneName;
    override public void OnClick()
    {

        base.OnClick();
        LocalSceneManager.instance.LoadScene(sceneName);
        
    }
}
