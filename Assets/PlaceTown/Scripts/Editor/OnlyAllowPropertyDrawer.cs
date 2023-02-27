using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(OnlyAllowAttribute))]
public class OnlyAllowPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (OnlyAllowAttribute)attribute;
        EditorGUI.BeginProperty(position, label, property);

        if (property.objectReferenceValue != null)
        {
            if (!property.isArray)
            {
                if (!attr.AllowedType.IsInstanceOfType(property.objectReferenceValue))
                {
                    Debug.LogWarning($"Invalid type assigned: {property.type}. Only {attr.AllowedType} is allowed.");
                    property.objectReferenceValue = null;
                }
            }
            else
            {
                var wrong = new List<int>();
                for (int i = 0; i < property.arraySize; i++)
                {
                    var obj = property.GetArrayElementAtIndex(i);
                    if (obj != null && !attr.AllowedType.IsInstanceOfType(obj))
                    {
                        wrong.Add(i);
                    }    
                }
                foreach (var n in wrong)
                {
                    property.DeleteArrayElementAtIndex(n);
                    property.InsertArrayElementAtIndex(n);
                }
            }
        }
        var type = (!attr.useFilter) ? typeof(ScriptableObject) : attr.FilterType;
        EditorGUI.ObjectField(position, property, type, label);

        EditorGUI.EndProperty();
    }
}