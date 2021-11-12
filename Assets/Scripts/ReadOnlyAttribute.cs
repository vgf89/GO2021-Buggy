// Adds a [ReadOnly] attribute so we can create read-only field in the inspector.
// Check ReadOnlyDrawer.cs for the better half.
using UnityEngine;
using UnityEditor;
/// <summary>
/// Read Only attribute.
/// Attribute is use only to mark ReadOnly properties.
/// </summary>
//https://www.patrykgalach.com/2020/01/20/readonly-attribute-in-unity-editor/
public class ReadOnlyAttribute : PropertyAttribute { }