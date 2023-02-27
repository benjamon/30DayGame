using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class OnlyAllowAttribute : PropertyAttribute
{
    public System.Type AllowedType { get; }
    public System.Type FilterType { get; }
    public bool useFilter;

    public OnlyAllowAttribute(System.Type type)
    {
        AllowedType = type;
    }
    public OnlyAllowAttribute(System.Type type, System.Type filter)
    {
        AllowedType = type;
        FilterType = filter;
        useFilter = true;
    }
}