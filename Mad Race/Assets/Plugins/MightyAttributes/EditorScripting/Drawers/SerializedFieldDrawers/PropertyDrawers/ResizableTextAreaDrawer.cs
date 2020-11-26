#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ResizableTextAreaDrawer : BasePropertyDrawer<ResizableTextAreaAttribute>, IArrayElementDrawer
    {
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property,
            ResizableTextAreaAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawTextArea(property, attribute.Options);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawTextArea(serializedField.GetElement(index), baseAttribute.Options);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index, 
            BasePropertyDrawerAttribute baseAttribute) => DrawTextArea(serializedField.GetElement(index), baseAttribute.Options, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) 
            => DrawTextArea(position, serializedField.GetElement(index), baseAttribute.Options);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var element = serializedField.GetElement(index);
            return element.propertyType != SerializedPropertyType.String
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.TextHeight(element.stringValue, 3) + MightyGUIUtilities.FIELD_HEIGHT +
                  MightyGUIUtilities.FIELD_SPACING * 2;
        }

        public void DrawTextArea(SerializedProperty property, FieldOption options, GUIContent label = null)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                DrawLabel(property, options, label);

                property.stringValue = EditorGUILayout.TextArea(property.stringValue,
                    GUILayout.Height(MightyGUIUtilities.TextHeight(property.stringValue, 3)));
            }
            else
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"{nameof(ResizableTextAreaAttribute)} can only be used on string fields");
            }
        }

        public void DrawTextArea(Rect position, SerializedProperty property, FieldOption options, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"{nameof(ResizableTextAreaAttribute)} can only be used on string fields");
                return;
            }

            DrawLabel(ref position, property, options, label);

            position.height = MightyGUIUtilities.TextHeight(property.stringValue, 3);
            property.stringValue = EditorGUI.TextArea(position, property.stringValue);
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, ResizableTextAreaAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif