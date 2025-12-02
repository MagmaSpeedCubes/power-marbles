using UnityEngine;

public class ShowIfAttribute : PropertyAttribute
{
    public string conditionField;
    public bool boolValue;
    public int intValue;
    public string stringValue;
    public enum CompareType { Bool, Int, String }
    public CompareType compareType;

    public ShowIfAttribute(string conditionField, bool value)
    {
        this.conditionField = conditionField;
        this.boolValue = value;
        compareType = CompareType.Bool;
    }

    public ShowIfAttribute(string conditionField, int value)
    {
        this.conditionField = conditionField;
        this.intValue = value;
        compareType = CompareType.Int;
    }

    public ShowIfAttribute(string conditionField, string value)
    {
        this.conditionField = conditionField;
        this.stringValue = value;
        compareType = CompareType.String;
    }
}
