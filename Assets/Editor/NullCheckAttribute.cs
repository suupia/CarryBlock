#nullable enable
using UnityEditor;
using UnityEngine;

namespace  Carry.Editor
{
    public class NullCheckAttribute : PropertyAttribute
    {
        public NullCheckAttribute()
        {
            
        }
    }
    
    [CustomPropertyDrawer(typeof(NullCheckAttribute))]
    public class NullCheckDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.objectReferenceValue == null)
            {
                string objectName = property.serializedObject.targetObject.name;
                Debug.LogError($"Field '{label.text}' in '{objectName}' is null");
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            EditorGUI.PropertyField(position, property, label);
        }
    }
}