using UnityEngine;

[System.Serializable]
public class Tag
{
    public string name;
    public string value;

    public Tag(string newName, string newValue)
    {
        name = newName;
        value = newValue;
    }
}
