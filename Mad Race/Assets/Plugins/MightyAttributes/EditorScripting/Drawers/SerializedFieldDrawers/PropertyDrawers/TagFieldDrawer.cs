#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class TagFieldDrawer : BasePropertyDrawer<TagFieldAttribute>, IArrayElementDrawer
    {
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, TagFieldAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawTagField(property, attribute.Options);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawTagField(serializedField.GetElement(index), baseAttribute.Options);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) => DrawTagField(serializedField.GetElement(index), baseAttribute.Options, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) => DrawTagField(position, serializedField.GetElement(index), baseAttribute.Options);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            serializedField.GetElement(index).propertyType != SerializedPropertyType.String
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;

        public static void DrawTagField(SerializedProperty property, FieldOption options, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"\"{property.displayName}\" should be of type string");
                return;
            }

            string tag;
            if (options.Contains(FieldOption.HideLabel))
                tag = EditorGUILayout.TagField(property.stringValue);
            else if (options.Contains(FieldOption.BoldLabel))
                tag = EditorGUILayout.TagField(label ?? EditorGUIUtility.TrTextContent(property.displayName), property.stringValue,
                    EditorStyles.boldLabel);
            else
                tag = EditorGUILayout.TagField(label ?? EditorGUIUtility.TrTextContent(property.displayName), property.stringValue);

            property.stringValue = tag;
        }

        public static void DrawTagField(Rect position, SerializedProperty property, FieldOption options)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"\"{property.displayName}\" should be of type string");
                return;
            }

            string tag;
            if (options.Contains(FieldOption.HideLabel))
                tag = EditorGUI.TagField(position, property.stringValue);
            else if (options.Contains(FieldOption.BoldLabel))
                tag = EditorGUI.TagField(position, property.displayName, property.stringValue, EditorStyles.boldLabel);
            else
                tag = EditorGUI.TagField(position, property.displayName, property.stringValue);

            property.stringValue = tag;
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, TagFieldAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif