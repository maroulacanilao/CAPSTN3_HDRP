using UnityEditor;
using UnityEngine;

public class ShowScriptableReference : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ShowScriptableReference))]
public class ShowScriptableReferenceDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(property, label, true);

        if (property.objectReferenceValue != null)
        {
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            SerializedProperty scriptableProperties = serializedObject.GetIterator();

            bool enterChildren = true;
            while (scriptableProperties.NextVisible(enterChildren))
            {
                if (scriptableProperties.name == "m_Script") // Exclude the "Script" property
                    continue;

                height += EditorGUI.GetPropertyHeight(scriptableProperties, true);
                enterChildren = false;
            }
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property, label, true);

        if (property.objectReferenceValue != null)
        {
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            SerializedProperty scriptableProperties = serializedObject.GetIterator();

            Rect propertyRect = EditorGUI.IndentedRect(position);
            propertyRect.y += EditorGUI.GetPropertyHeight(property, label) + EditorGUIUtility.standardVerticalSpacing;
            propertyRect.height = EditorGUIUtility.singleLineHeight;

            bool enterChildren = true;
            while (scriptableProperties.NextVisible(enterChildren))
            {
                if (scriptableProperties.name == "m_Script") // Exclude the "Script" property
                    continue;

                EditorGUI.PropertyField(propertyRect, scriptableProperties, true);
                propertyRect.y += EditorGUI.GetPropertyHeight(scriptableProperties, true);
                enterChildren = false;
            }
        }

        EditorGUI.EndProperty();
    }
}

