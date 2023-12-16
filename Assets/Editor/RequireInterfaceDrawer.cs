using System;
using CodeBase.Tools;
using NTC.Global.Cache;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomPropertyDrawer(typeof(RequireInterface))]
    public class RequireInterfaceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RequireInterface requireInterface = attribute as RequireInterface;
            Type requireType = requireInterface.RequireType;

            if (IsValid(property, requireType))
            {
                label.tooltip = "Require " + requireInterface.RequireType + " interface";
                CheckProperty(property, requireType);
            }

            EditorGUI.PropertyField(position, property, label);
        }

        private bool IsValid(SerializedProperty property, Type targetType) =>
            targetType.IsInterface && property.propertyType == SerializedPropertyType.ObjectReference;

        private void CheckProperty(SerializedProperty property, Type targetType)
        {
            if (property.objectReferenceValue == null)
                return;

            if (property.objectReferenceValue is GameObject)
                CheckGameObject(property, targetType);
            else if (property.objectReferenceValue is ScriptableObject)
                CheckScriptableObject(property, targetType);
            else if (property.objectReferenceValue is MonoBehaviour)
                CheckMonoBehavior(property, targetType);
            else if (property.objectReferenceValue is MonoCache)
                CheckMonoCache(property, targetType);
        }

        private void CheckMonoBehavior(SerializedProperty property, Type targetType)
        {
            MonoBehaviour field = property.objectReferenceValue as MonoBehaviour;

            if (field.GetComponent(targetType) == null)
            {
                property.objectReferenceValue = null;
                Debug.LogError("MonoBehavior must contain component implemented " + targetType + " interface");
            }
        }

        private void CheckMonoCache(SerializedProperty property, Type targetType)
        {
            MonoCache field = property.objectReferenceValue as MonoCache;

            if (field.GetComponent(targetType) == null)
            {
                property.objectReferenceValue = null;
                Debug.LogError("MonoCache must contain component implemented " + targetType + " interface");
            }
        }

        private void CheckGameObject(SerializedProperty property, Type targetType)
        {
            GameObject field = property.objectReferenceValue as GameObject;

            if (field.GetComponent(targetType) == null)
            {
                property.objectReferenceValue = null;
                Debug.LogError("GameObject must contain component implemented " + targetType + " interface");
            }
        }

        private void CheckScriptableObject(SerializedProperty property, Type targetType)
        {
            ScriptableObject field = property.objectReferenceValue as ScriptableObject;
            Type fieldType = field.GetType();

            if (targetType.IsAssignableFrom(fieldType) == false)
            {
                property.objectReferenceValue = null;
                Debug.LogError("ScriptableObject must implement " + targetType + " interface");
            }
        }
    }
}