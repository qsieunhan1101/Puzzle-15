#if UNITY_EDITOR && SORTIFY_ATTRIBUTES
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Sortify
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;
            if (property.propertyType == SerializedPropertyType.Boolean)
            {
                EditorGUI.BeginProperty(position, label, property);

                if (GUILayout.Button(buttonAttribute.ButtonText))
                    property.boolValue = !property.boolValue;

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use with bool or method.");
            }
        }
    }

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonMethodDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var mono = (MonoBehaviour)target;
            var methods = mono.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var buttonAttribute = (ButtonAttribute)method.GetCustomAttribute(typeof(ButtonAttribute));
                if (buttonAttribute != null)
                {
                    if (GUILayout.Button(buttonAttribute.ButtonText))
                        method.Invoke(mono, null);
                }
            }
        }
    }
}
#endif
