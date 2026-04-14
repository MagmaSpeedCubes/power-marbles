using System;

using UnityEngine;
using UnityEditor;

namespace MagmaLabs
{

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //Debug.Log($"ShowIfDrawer.GetPropertyHeight for '{property.propertyPath}'");
            bool show = ShouldShow(property);
            //Debug.Log($" ShowIfDrawer:GetPropertyHeight -> {show}");
            return show 
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Debug.Log($"ShowIfDrawer.OnGUI for '{property.propertyPath}'");
            if (ShouldShow(property))
                EditorGUI.PropertyField(position, property, label, true);
        }

        virtual protected bool ShouldShow(SerializedProperty property)
        {
            // if this property represents an element inside an array, redirect the
            // check to the array field itself so that we hide/show the whole
            // collection rather than each element individually.
            if (property.propertyPath.Contains(".Array.data["))
            {
                string parentPath = property.propertyPath.Substring(0, property.propertyPath.IndexOf(".Array.data["));
                SerializedProperty parent = property.serializedObject.FindProperty(parentPath);
                if (parent != null && parent != property)
                {
                    return ShouldShow(parent);
                }
            }

            var showIf = (ShowIfAttribute)attribute;

            // debug: investigate why ShowIf sometimes misbehaves
            //Debug.Log($"ShowIfDrawer: evaluating '{property.propertyPath}' cond='{showIf.conditionField}' compare='{showIf.compareValue}'");

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);
            //Debug.Log($"ShowIfDrawer: FindProperty('{showIf.conditionField}') returned {condition}");

            if (condition == null)
            {
                //Debug.LogError($"ShowIf: Field '{showIf.conditionField}' not found.");
                return true;
            }

            object conditionValue = GetSerializedPropertyValue(condition);
            if (conditionValue == null && showIf.compareValue == null)
                return true;

            if (conditionValue is string s && showIf.compareValue != null)
                return s == showIf.compareValue.ToString();

            if (conditionValue != null)
                return conditionValue.Equals(showIf.compareValue);

            return false;
        }

        protected object GetSerializedPropertyValue(SerializedProperty property)
        {
            return property.propertyType switch
            {
                SerializedPropertyType.Boolean => property.boolValue,
                SerializedPropertyType.Integer => property.intValue,
                SerializedPropertyType.Enum => property.enumNames[property.enumValueIndex],
                SerializedPropertyType.Float => property.floatValue,
                SerializedPropertyType.String => property.stringValue,
                _ => null,
            };
        }


        }
    

[CustomPropertyDrawer(typeof(ShowIfNotAttribute))]
public class ShowIfNotDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            return !base.ShouldShow(property);
        }
    }   
[CustomPropertyDrawer(typeof(ShowIfGreaterThanAttribute))]
public class ShowIfGreaterThanDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            var showIf = (ShowIfAttribute)attribute;
            Debug.Log($"ShowIfDrawer.ShouldShow for '{property.propertyPath}' cond='{showIf.conditionField}' compare='{showIf.compareValue}'");

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);
            Debug.Log($" ShowIfDrawer.ShouldShow: found condition property = {condition}");

            if (condition == null)
            {
                Debug.LogError($"ShowIfGreaterThan: Field '{showIf.conditionField}' not found.");
                return true;
            }

            object conditionValue = GetSerializedPropertyValue(condition);
            if (conditionValue is IComparable comparableCondition && showIf.compareValue is IComparable comparableCompare)
            {
                return comparableCondition.CompareTo(comparableCompare) > 0;
            }
            else
            {
                Debug.LogError($"ShowIfGreaterThan: Field '{showIf.conditionField}' or compare value is not comparable.");
                return true;
            }
        }
    }

[CustomPropertyDrawer(typeof(ShowIfLessThanAttribute))]
public class ShowIfLessThanDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            var showIf = (ShowIfAttribute)attribute;

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);

            if (condition == null)
            {
                Debug.LogError($"ShowIfLessThan: Field '{showIf.conditionField}' not found.");
                return true;
            }

            object conditionValue = GetSerializedPropertyValue(condition);
            if (conditionValue is IComparable comparableCondition && showIf.compareValue is IComparable comparableCompare)
            {
                return comparableCondition.CompareTo(comparableCompare) < 0;
            }
            else
            {
                Debug.LogError($"ShowIfLessThan: Field '{showIf.conditionField}' or compare value is not comparable.");
                return true;
            }
        }
    }

[CustomPropertyDrawer(typeof(ShowIfGreaterThanOrEqualAttribute))]
public class ShowIfGreaterThanOrEqualDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            var showIf = (ShowIfAttribute)attribute;

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);

            if (condition == null)
            {
                Debug.LogError($"ShowIfGreaterEqual: Field '{showIf.conditionField}' not found.");
                return true;
            }

            object conditionValue = GetSerializedPropertyValue(condition);
            if (conditionValue is IComparable comparableCondition && showIf.compareValue is IComparable comparableCompare)
            {
                return comparableCondition.CompareTo(comparableCompare) >= 0;
            }
            else
            {
                Debug.LogError($"ShowIfGreaterEqual: Field '{showIf.conditionField}' or compare value is not comparable.");
                return true;
            }
        }
    }

[CustomPropertyDrawer(typeof(ShowIfLessThanOrEqualAttribute))]

public class ShowIfLessThanOrEqualDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            var showIf = (ShowIfAttribute)attribute;

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);

            if (condition == null)
            {
                Debug.LogError($"ShowIfLessEqual: Field '{showIf.conditionField}' not found.");
                return true;
            }
        
            object conditionValue = GetSerializedPropertyValue(condition);
            if (conditionValue is IComparable comparableCondition && showIf.compareValue is IComparable comparableCompare)
            {
                return comparableCondition.CompareTo(comparableCompare) <= 0;
            }
            else
            {
                Debug.LogError($"ShowIfLessEqual: Field '{showIf.conditionField}' or compare value is not comparable.");
                return true;
            }
        }
    }
    [CustomPropertyDrawer(typeof(ShowIfAnyAttribute))]
public class ShowIfAnyDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            // if this property is an element inside an array, forward to the
            // array's property so the entire collection is hidden/shown together
            if (property.propertyPath.Contains(".Array.data["))
            {
                string parentPath = property.propertyPath.Substring(0, property.propertyPath.IndexOf(".Array.data["));
                SerializedProperty parent = property.serializedObject.FindProperty(parentPath);
                if (parent != null && parent != property)
                {
                    return ShouldShow(parent);
                }
            }

            var showIf = (ShowIfAnyAttribute)attribute;

            // Determine the single condition field to check
            string conditionField = showIf.conditionField;
            if (string.IsNullOrEmpty(conditionField))
            {
                // fallback to any older-field storage
                if (showIf.conditionFields == null || showIf.conditionFields.Length == 0)
                    return true;
                conditionField = showIf.conditionFields[0];
            }

            // Get compare values: prefer the explicit array, otherwise see if base.compareValue holds an object[]
            object[] compareValues = null;
            if (showIf.compareValues != null && showIf.compareValues.Length > 0)
                compareValues = showIf.compareValues;
            else if (showIf.compareValue is object[] arr)
                compareValues = arr;
            else if (showIf.compareValue != null)
                compareValues = new object[] { showIf.compareValue };
            else
                compareValues = new object[0];

            SerializedProperty conditionProp = property.serializedObject.FindProperty(conditionField);
            if (conditionProp == null)
            {
                Debug.LogError($"ShowIfAny: Field '{conditionField}' not found.");
                return true;
            }

            object conditionValue = GetSerializedPropertyValue(conditionProp);

            foreach (var compareValue in compareValues)
            {
                if (conditionValue == null && compareValue == null)
                    return true;

                if (conditionValue != null && compareValue != null)
                {
                    // If condition is stored as string (enum), compare to the compareValue's name.
                    if (conditionValue is string s)
                    {
                        if (s == compareValue.ToString())
                            return true;
                    }
                    else
                    {
                        if (conditionValue.Equals(compareValue))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}


