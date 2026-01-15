using UnityEngine;
using UnityEditor;
using MagmaLabs.Utilities.Editor;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ShouldShow(property) 
            ? EditorGUI.GetPropertyHeight(property, label, true)
            : 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
            EditorGUI.PropertyField(position, property, label, true);
    }

    private bool ShouldShow(SerializedProperty property)
    {
        var showIf = (ShowIfAttribute)attribute;

        SerializedProperty condition =
            property.serializedObject.FindProperty(showIf.conditionField);

        if (condition == null)
        {
            Debug.LogError($"ShowIf: Field '{showIf.conditionField}' not found.");
            return true;
        }

        switch (showIf.compareType)
        {
            case ShowIfAttribute.CompareType.Bool:
                return condition.boolValue == showIf.boolValue;

            case ShowIfAttribute.CompareType.Int:
                return condition.intValue == showIf.intValue;

            case ShowIfAttribute.CompareType.String:
                return condition.stringValue == showIf.stringValue;
        }

        return true;
    }
}
