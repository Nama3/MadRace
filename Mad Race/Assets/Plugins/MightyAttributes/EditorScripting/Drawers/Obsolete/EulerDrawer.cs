#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class EulerDrawer : BasePropertyDrawer<EulerAttribute>, IArrayElementDrawer
    {
        [Obsolete("Suffers from Gimbal Lock.")]
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, EulerAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawEuler(property, attribute);
        }

        [Obsolete("Suffers from Gimbal Lock.")]
        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawEuler(serializedField.GetElement(index), (EulerAttribute) baseAttribute);

        [Obsolete("Suffers from Gimbal Lock.")]
        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawEuler(serializedField.GetElement(index), (EulerAttribute) baseAttribute, label);

        [Obsolete("Suffers from Gimbal Lock.")]
        public void DrawElement(Rect position, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawEuler(position, serializedField.GetElement(index), (EulerAttribute) baseAttribute);

        [Obsolete("Suffers from Gimbal Lock.")]
        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            serializedField.GetElement(index).propertyType != SerializedPropertyType.Quaternion
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;

        [Obsolete("Suffers from Gimbal Lock.")]
        private static void DrawEuler(SerializedProperty property, EulerAttribute attribute, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Quaternion)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"{property.name} should be of type Quaternion");
                return;
            }

            if (attribute.Options.Contains(FieldOption.HideLabel))
                MightyGUIUtilities.DrawRotationEuler(GUIContent.none, property);
            else
                MightyGUIUtilities.DrawRotationEuler(label ?? EditorGUIUtility.TrTextContent(property.displayName), property);
        }

        [Obsolete("Suffers from Gimbal Lock.")]
        private static void DrawEuler(Rect position, SerializedProperty property, EulerAttribute attribute)
        {
            if (property.propertyType != SerializedPropertyType.Quaternion)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"{property.name} should be of type Quaternion");
                return;
            }

            if (attribute.Options.Contains(FieldOption.HideLabel))
                MightyGUIUtilities.DrawRotationEuler(position, GUIContent.none, property);
            else
                MightyGUIUtilities.DrawRotationEuler(position, property);
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, EulerAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif