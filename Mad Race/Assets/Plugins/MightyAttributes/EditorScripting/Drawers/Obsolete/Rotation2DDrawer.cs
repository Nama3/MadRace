#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class Rotation2DDrawer : BasePropertyDrawer<Rotation2DAttribute>, IArrayElementDrawer
    {
        [Obsolete("Suffers from Gimbal Lock.")]
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, Rotation2DAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawRotation2D(property, attribute);
        }

        [Obsolete("Suffers from Gimbal Lock.")]
        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawRotation2D(serializedField.GetElement(index), (Rotation2DAttribute) baseAttribute);

        [Obsolete("Suffers from Gimbal Lock.")]
        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawRotation2D(serializedField.GetElement(index), (Rotation2DAttribute) baseAttribute, label);

        [Obsolete("Suffers from Gimbal Lock.")]
        public void DrawElement(Rect position, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawRotation2D(position, serializedField.GetElement(index), (Rotation2DAttribute) baseAttribute);

        [Obsolete("Suffers from Gimbal Lock.")]
        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            serializedField.GetElement(index).propertyType != SerializedPropertyType.Quaternion
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;

        [Obsolete("Suffers from Gimbal Lock.")]
        private void DrawRotation2D(SerializedProperty property, Rotation2DAttribute attribute, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Quaternion)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"{property.name} should be of type Quaternion");
                return;
            }

            var angles = property.quaternionValue.eulerAngles;

            angles.z = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUILayout.Slider(angles.z, 0, 359.9f)
                : EditorGUILayout.Slider(label ?? EditorGUIUtility.TrTextContent(property.displayName), angles.z, 0, 359.9f);

            property.quaternionValue = Quaternion.Euler(angles);
        }

        [Obsolete("Suffers from Gimbal Lock.")]
        private void DrawRotation2D(Rect position, SerializedProperty property, Rotation2DAttribute attribute)
        {
            if (property.propertyType != SerializedPropertyType.Quaternion)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"{property.name} should be of type Quaternion");
                return;
            }

            var angles = property.quaternionValue.eulerAngles;

            angles.z = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUI.Slider(position, angles.z, 0, 359.9f)
                : EditorGUI.Slider(position, property.displayName, angles.z, 0, 359.9f);

            property.quaternionValue = Quaternion.Euler(angles);
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, Rotation2DAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif