using System;
using System.Reflection;
using UnityEngine;
using System.Linq;
//using UnityEditor;

public class Utility : MonoBehaviour
{

    public static void CallFunction(string scriptName, string functionName)
    {
        Type type = Type.GetType(scriptName);
        if (type == null)
        {
            Debug.LogError("Type not found: " + scriptName);
            return;
        }
        MethodInfo method = type.GetMethod(functionName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        if (method == null)
        {
            Debug.LogError("Method not found: " + functionName);
            return;
        }

        object instance = null;
        if (!method.IsStatic)
        {
            // Find the first active instance in the scene
            instance = GameObject.FindFirstObjectByType(type);
            if (instance == null)
            {
                Debug.LogError("Instance of " + scriptName + " not found in scene.");
                return;
            }
        }

        method.Invoke(instance, null);
    }

    public static T CallReturnableFunction<T>(string scriptName, string functionName, params object[] args)
    {
        Type type = Type.GetType(scriptName);
        if (type == null)
        {
            Debug.LogError("Type not found: " + scriptName);
            return default;
        }

        // If you have overloads, you should resolve by parameter types:
        var paramTypes = args?.Select(a => a?.GetType() ?? typeof(object)).ToArray() ?? Type.EmptyTypes;
        // Debug.Log("Looking for method: " + functionName + " with parameters: " + string.Join(", ", paramTypes.Select(t => t.Name)));
        MethodInfo method = type.GetMethod(functionName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, paramTypes, null);
        if (method == null)
        {
            Debug.LogError("Method not found: " + functionName);
            return default;
        }

        object instance = null;
        if (!method.IsStatic)
        {
            instance = GameObject.FindFirstObjectByType(type);
            if (instance == null)
            {
                Debug.LogError("Instance of " + scriptName + " not found in scene.");
                return default;
            }
        }

        try
        {
            object raw = method.Invoke(instance, args);

            // Handle Task<T> methods (optional)
            if (raw is System.Threading.Tasks.Task task)
            {
                task.GetAwaiter().GetResult(); // wait synchronously
                var resultProp = task.GetType().GetProperty("Result");
                raw = resultProp != null ? resultProp.GetValue(task) : null;
            }

            if (raw == null)
                return default;

            // If the return type is already T, direct cast is best (handles UnityEngine.Object types, structs, etc.)
            if (raw is T t) return t;

            // Otherwise try Convert.ChangeType for primitive/value conversions
            return (T)Convert.ChangeType(raw, typeof(T));
        }
        catch (TargetInvocationException tie)
        {
            // unwrap the inner exception to get the real error from the invoked method
            Debug.LogError($"Invocation error: {tie.InnerException?.Message ?? tie.Message}");
            return default;
        }
        catch (Exception e)
        {
            Debug.LogError($"Reflection call failed: {e.Message}");
            return default;
        }
    }


    public static object GetVariableValue(string scriptName, string variableName)
    {
        // Get the type
        Type type = Type.GetType(scriptName);
        if (type == null)
        {
            Debug.LogError("Type not found: " + scriptName);
            return null;
        }

        // Try to find the field first
        FieldInfo field = type.GetField(variableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

        // If not found, try property
        PropertyInfo property = null;
        if (field == null)
        {
            property = type.GetProperty(variableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (property == null)
            {
                Debug.LogError("Variable or property not found: " + variableName);
                return null;
            }
        }

        // Prepare instance (for non-static fields/properties)
        object instance = null;
        bool isStatic = field != null ? field.IsStatic : property.GetGetMethod(true).IsStatic;
        if (!isStatic)
        {
            instance = GameObject.FindFirstObjectByType(type);
            if (instance == null)
            {
                Debug.LogError("Instance of " + scriptName + " not found in scene.");
                return null;
            }
        }

        // Get value
        object value = field != null
            ? field.GetValue(instance)
            : property.GetValue(instance);

        //Debug.Log($"Value of {scriptName}.{variableName} = {value}");
        return value;
    }



    public static void SetVariableValue(string scriptName, string variableName, object newValue)
        {
            Type type = Type.GetType(scriptName);
            if (type == null)
            {
                Debug.LogError("Type not found: " + scriptName);
                return;
            }

            // Try to find field or property
            FieldInfo field = type.GetField(variableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            PropertyInfo property = null;
            if (field == null)
            {
                property = type.GetProperty(variableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                if (property == null)
                {
                    Debug.LogError("Variable or property not found: " + variableName);
                    return;
                }
            }

            bool isStatic = field != null ? field.IsStatic : property.GetSetMethod(true).IsStatic;
            object instance = null;
            if (!isStatic)
            {
                instance = GameObject.FindFirstObjectByType(type);
                if (instance == null)
                {
                    Debug.LogError("Instance of " + scriptName + " not found in scene.");
                    return;
                }
            }

            // Get target type (for conversion)
            Type targetType = field != null ? field.FieldType : property.PropertyType;

            try
            {
                // Convert value if needed
                object convertedValue = Convert.ChangeType(newValue, targetType);
                if (field != null)
                    field.SetValue(instance, convertedValue);
                else
                    property.SetValue(instance, convertedValue);

                Debug.Log($"Set {scriptName}.{variableName} = {convertedValue} ({targetType.Name})");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to set {variableName} on {scriptName}: {e.Message}");
            }
        }
/*

    public static Sprite FindSpriteByName(string spriteName)
    {
        // Search for sprites (t:Sprite) that match the name token.
        // This search may return multiple GUIDs; we check each for an exact match.
        string filter = $"t:Sprite {spriteName}";
        string[] guids = AssetDatabase.FindAssets(filter);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // Try load directly as a Sprite (works if the asset is a sprite asset)
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null && sprite.name == spriteName)
                return sprite;

            // For texture atlases / multi-sprite textures: load all sub-assets and check names
            UnityEngine.Object[] all = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (var o in all)
            {
                if (o is Sprite s && s.name == spriteName)
                    return s;
            }
        }

        // If not found, try a broader search without the name filter (rarely needed)
        // string[] allGuids = AssetDatabase.FindAssets("t:Sprite");
        // ... iterate if necessary

        return null;
    }
    */
    
}