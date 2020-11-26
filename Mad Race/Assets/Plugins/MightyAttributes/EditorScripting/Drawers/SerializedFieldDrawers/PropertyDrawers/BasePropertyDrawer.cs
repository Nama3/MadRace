#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface IPropertyDrawer : ISerializedFieldDrawer
    {
        void DrawProperty(MightySerializedField serializedField, SerializedProperty property, BasePropertyDrawerAttribute attribute);
    }

    public abstract class BasePropertyDrawer<T> : BaseSerializeFieldDrawer<T>, IPropertyDrawer where T : BasePropertyDrawerAttribute
    {
        public void DrawProperty(MightySerializedField serializedField, SerializedProperty property, BasePropertyDrawerAttribute attribute) 
            => DrawProperty(serializedField, property, (T) attribute);

        protected abstract void DrawProperty(MightySerializedField serializedField, SerializedProperty property, T attribute);

        protected bool DrawLabel(SerializedProperty property, FieldOption options, GUIContent label = null)
        {
            if (options.Contains(FieldOption.HideLabel)) return false;

            if (!options.Contains(FieldOption.BoldLabel))
                DrawNormalLabel(property, label);
            else
                DrawBoldLabel(property, label);

            return true;
        }

        protected bool DrawLabel(ref Rect position, SerializedProperty property, FieldOption options,
            GUIContent label)
        {
            if (options.Contains(FieldOption.HideLabel)) return false;

            var labelPosition = new Rect(position.x, position.y, position.width, MightyGUIUtilities.FIELD_HEIGHT);

            position = !options.Contains(FieldOption.BoldLabel)
                ? DrawNormalLabel(labelPosition, property, label)
                : DrawBoldLabel(labelPosition, property, label);
            return true;
        }

        protected virtual void DrawNormalLabel(SerializedProperty property, GUIContent label) =>
            EditorGUILayout.LabelField(label ?? EditorGUIUtility.TrTextContent(property.displayName));

        protected virtual void DrawBoldLabel(SerializedProperty property, GUIContent label) =>
            EditorGUILayout.LabelField(label ?? EditorGUIUtility.TrTextContent(property.displayName), EditorStyles.boldLabel);

        protected virtual Rect DrawNormalLabel(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(position, label ?? EditorGUIUtility.TrTextContent(property.displayName));
            position.y += MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;
            return position;
        }

        protected virtual Rect DrawBoldLabel(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(position, label ?? EditorGUIUtility.TrTextContent(property.displayName), EditorStyles.boldLabel);
            position.y += MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;
            return position;
        }
    }
}
#endif