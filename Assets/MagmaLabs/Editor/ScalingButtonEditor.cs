using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ScalingButton))]
[CanEditMultipleObjects]
public class ScalingButtonEditor : ButtonEditor
{
    private SerializedProperty autoFindTextProp;
    private SerializedProperty tmpTextProp;
    private SerializedProperty legacyTextProp;
    private SerializedProperty scalingEnabledProp;
    private SerializedProperty paddingProp;
    private SerializedProperty minSizeProp;
    private SerializedProperty maxSizeProp;
    private SerializedProperty snapToMultiplesProp;
    private SerializedProperty snapMultiplesProp;
    private SerializedProperty resizeInPlayModeProp;

    protected override void OnEnable()
    {
        base.OnEnable();

        autoFindTextProp = serializedObject.FindProperty("autoFindText");
        tmpTextProp = serializedObject.FindProperty("tmpText");
        legacyTextProp = serializedObject.FindProperty("legacyText");
        scalingEnabledProp = serializedObject.FindProperty("scalingEnabled");
        paddingProp = serializedObject.FindProperty("padding");
        minSizeProp = serializedObject.FindProperty("minSize");
        maxSizeProp = serializedObject.FindProperty("maxSize");
        snapToMultiplesProp = serializedObject.FindProperty("snapToMultiples");
        snapMultiplesProp = serializedObject.FindProperty("snapMultiples");
        resizeInPlayModeProp = serializedObject.FindProperty("resizeInPlayMode");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Scaling Button", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(autoFindTextProp);
        EditorGUILayout.PropertyField(tmpTextProp);
        EditorGUILayout.PropertyField(legacyTextProp);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(scalingEnabledProp);
        EditorGUILayout.PropertyField(paddingProp);
        EditorGUILayout.PropertyField(minSizeProp);
        EditorGUILayout.PropertyField(maxSizeProp);
        EditorGUILayout.PropertyField(snapToMultiplesProp);
        if (snapToMultiplesProp.boolValue)
        {
            EditorGUILayout.PropertyField(snapMultiplesProp);
        }
        EditorGUILayout.PropertyField(resizeInPlayModeProp);

        serializedObject.ApplyModifiedProperties();
    }
}
