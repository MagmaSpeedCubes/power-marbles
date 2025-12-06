using UnityEngine;

public class LevelSelectManager : TiledElementManager
{
    [SerializeField] private NumberText efficiencyDisplay;

    void Start()
    {
        
        Instantiate();
    }

    // Update is called once per frame
    void Update()
    {
        efficiencyDisplay.SetValue(SecureProfileStats.instance.GetEfficiencyScore());
    }
}
