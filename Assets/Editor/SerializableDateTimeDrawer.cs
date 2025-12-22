using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SerializableDateTime))]
public class SerializableDateTimeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float h = EditorGUIUtility.singleLineHeight;
        float spacing = 2f;

        // Label
        Rect labelRect = new Rect(position.x, position.y, position.width, h);
        EditorGUI.LabelField(labelRect, label);

        // Date row
        Rect dateRect = new Rect(position.x, position.y + h + spacing, position.width, h);
        float partW = (dateRect.width - 8f) / 3f;
        var yearProp = property.FindPropertyRelative("year");
        var monthProp = property.FindPropertyRelative("month");
        var dayProp = property.FindPropertyRelative("day");

        EditorGUI.LabelField(new Rect(dateRect.x, dateRect.y, 36, h), "Date");
        yearProp.intValue = EditorGUI.IntField(new Rect(dateRect.x + 40, dateRect.y, partW - 4, h), yearProp.intValue);
        monthProp.intValue = EditorGUI.IntField(new Rect(dateRect.x + 40 + partW, dateRect.y, partW - 4, h), monthProp.intValue);
        dayProp.intValue = EditorGUI.IntField(new Rect(dateRect.x + 40 + partW * 2, dateRect.y, partW - 4, h), dayProp.intValue);

        // Time row
        Rect timeRect = new Rect(position.x, position.y + (h + spacing) * 2, position.width, h);
        float tpartW = (timeRect.width - 8f) / 3f;
        var hourProp = property.FindPropertyRelative("hour");
        var minuteProp = property.FindPropertyRelative("minute");
        var secondProp = property.FindPropertyRelative("second");

        EditorGUI.LabelField(new Rect(timeRect.x, timeRect.y, 36, h), "Time");
        hourProp.intValue = EditorGUI.IntField(new Rect(timeRect.x + 40, timeRect.y, tpartW - 4, h), hourProp.intValue);
        minuteProp.intValue = EditorGUI.IntField(new Rect(timeRect.x + 40 + tpartW, timeRect.y, tpartW - 4, h), minuteProp.intValue);
        secondProp.intValue = EditorGUI.IntField(new Rect(timeRect.x + 40 + tpartW * 2, timeRect.y, tpartW - 4, h), secondProp.intValue);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (EditorGUIUtility.singleLineHeight * 3) + (2f * 2f);
    }
}
