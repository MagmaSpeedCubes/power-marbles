using UnityEngine;
using MagmaLabs.Utilities.Reflection;
public class InfographicManager : MonoBehaviour
{
    [SerializeField] protected Infographic[] infographics;
    [SerializeField] protected string scriptValueIsStoredIn, nameOfValue;


    // Update is called once per frame
    void Update()
    {
        object value = Utility.GetVariableValue(scriptValueIsStoredIn, nameOfValue);
        UpdateInfographics(value);
    }

    protected void UpdateInfographics(object value)
    {

        float fv = (float)value;
        foreach (Infographic graph in infographics)
        {
            graph?.SetValue(fv);

        }
        

    }
}
