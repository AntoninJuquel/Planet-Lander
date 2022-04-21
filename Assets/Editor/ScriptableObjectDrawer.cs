using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ScriptableObject), true)]
public class ScriptableObjectDrawer : PropertyDrawer
{
    // Cached scriptable object editor
    private Editor _editor;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Draw label
        EditorGUI.PropertyField(position, property, label, true);

        // Draw foldout arrow
        if (property.objectReferenceValue != null)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
        }

        // Draw foldout properties
        if (!property.isExpanded) return;
        // Make child fields be indented
        EditorGUI.indentLevel++;

        // background
        GUILayout.BeginVertical("box");

        if (!_editor)
            Editor.CreateCachedEditor(property.objectReferenceValue, null, ref _editor);

        // Draw object properties
        EditorGUI.BeginChangeCheck();
        if (_editor) // catch empty property
        {
            _editor.OnInspectorGUI();
        }

        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        GUILayout.EndVertical();

        // Set indent back to what it was
        EditorGUI.indentLevel--;
    }
}