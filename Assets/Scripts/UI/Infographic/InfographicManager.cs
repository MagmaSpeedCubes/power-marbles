using UnityEngine;
using MagmaLabs.Utilities.Reflection;
using MagmaLabs.UI;
public class InfographicManager : MonoBehaviour
{
    [SerializeField] protected Infographic[] infographics;
    [SerializeField] protected string scriptValueIsStoredIn, nameOfValue;


    // Update is called once per frame
    void Update()
    {
        float value = (float)Utility.GetVariableValue(scriptValueIsStoredIn, nameOfValue);
        UpdateInfographics(value);
    }

    protected void UpdateInfographics(float value)
    {


        foreach (Infographic graph in infographics)
        {
            graph?.SetValue(value);

        }
        

    }
}
