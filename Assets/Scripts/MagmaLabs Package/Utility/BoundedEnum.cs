using UnityEngine;
using System.Collections.Generic;
namespace MagmaLabs.Utilities{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Bounded Enum", menuName = "MagmaLabs/Utilities/Bounded Enum")]
public class BoundedEnum : ScriptableObject
{

    public List<BoundedValue> values;

    public BoundedEnum()
    {
        values = new List<BoundedValue>();
    }

    public BoundedEnum(List<BoundedValue> initialValues)
    {
        values = initialValues;
    }


    public void AddValue(string value, float min, float max)
    {
        values.Add(new BoundedValue(){value=value, min=min, max=max});
    }

    public BoundedValue GetValue(string value)
    {
        return values.Find(v => v.value == value);
    }

    public List<BoundedValue> GetAllValues()
    {
        return values;
    }

    public void ClearValues()
    {
        values.Clear();
    }

    public string GetValueAtPosition(float position)
    {
        foreach (var value in values)
        {
            if (position >= value.min && position <= value.max)
            {
                return value.value;
            }
        }
        return null;
    }

    public int IndexOf(string value)
    {
        return values.FindIndex(v => v.value == value);
    }

}
[System.Serializable]
public struct BoundedValue
    {
        public string value;
        public float min;
        public float max;


    }

}
