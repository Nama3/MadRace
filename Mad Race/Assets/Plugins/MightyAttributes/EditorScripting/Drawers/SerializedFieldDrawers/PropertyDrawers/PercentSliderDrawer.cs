#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class PercentSliderDrawer : BasePropertyDrawer<PercentSliderAttribute>, IArrayElementDrawer
    {
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property,
            PercentSliderAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawSlider(property, attribute);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawSlider(serializedField.GetElement(index), (PercentSliderAttribute) baseAttribute);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawSlider(serializedField.GetElement(index), (PercentSliderAttribute) baseAttribute, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawSlider(position, serializedField.GetElement(index), (PercentSliderAttribute) baseAttribute);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var propertyType = serializedField.GetElement(index).propertyType;
            return propertyType != SerializedPropertyType.Integer && propertyType != SerializedPropertyType.Float
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;
        }

        public void DrawSlider(SerializedProperty property, PercentSliderAttribute attribute, GUIContent label = null)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    GUILayout.BeginHorizontal();

                    if (attribute.Options.Contains(FieldOption.HideLabel))
                        property.intValue = EditorGUILayout.IntSlider(property.intValue, 0, 100);
                    else if (label != null)
                        property.intValue = EditorGUILayout.IntSlider(label, property.intValue, 0, 100);
                    else
                        property.intValue = EditorGUILayout.IntSlider(property.displayName, property.intValue, 0, 100);

                    GUILayout.Label("%", GUILayout.Width(15));
                    GUILayout.EndHorizontal();
                    break;
                case SerializedPropertyType.Float:
                    GUILayout.BeginHorizontal();

                    var value = attribute.Between01 ? property.floatValue * 100 : property.floatValue;

                    if (attribute.Options.Contains(FieldOption.HideLabel))
                        value = EditorGUILayout.Slider(value, 0, 100);
                    else if (label != null)
                        value = EditorGUILayout.Slider(label, value, 0, 100);
                    else
                        value = EditorGUILayout.Slider(property.displayName, value, 0, 100);

                    property.floatValue = attribute.Between01 ? value / 100 : value;

                    GUILayout.Label("%", GUILayout.Width(15));
                    GUILayout.EndHorizontal();
                    break;
                default:
                    MightyGUIUtilities.DrawPropertyField(property, label);
                    MightyGUIUtilities.DrawHelpBox($"{nameof(PercentSliderAttribute)} can be used only on int and float fields");
                    break;
            }
        }

        public void DrawSlider(Rect position, SerializedProperty property, PercentSliderAttribute attribute)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:

                    position.width -= 15;
                    if (attribute.Options.Contains(FieldOption.HideLabel))
                        property.intValue = EditorGUI.IntSlider(position, property.intValue, 0, 100);
                    else
                        property.intValue = EditorGUI.IntSlider(position, property.displayName, property.intValue, 0, 100);

                    position.x += position.width;
                    GUI.Label(position, "%");
                    break;
                case SerializedPropertyType.Float:

                    var value = attribute.Between01 ? property.floatValue * 100 : property.floatValue;

                    position.width -= 15;
                    if (attribute.Options.Contains(FieldOption.HideLabel))
                        value = EditorGUI.Slider(position, value, 0, 100);
                    else
                        value = EditorGUI.Slider(position, property.displayName, value, 0, 100);

                    property.floatValue = attribute.Between01 ? value / 100 : value;

                    position.x += position.width;
                    GUI.Label(position, "%");
                    break;
                default:
                    position = MightyGUIUtilities.DrawPropertyField(position, property);
                    MightyGUIUtilities.DrawHelpBox(position,
                        $"{nameof(PercentSliderAttribute)} can be used only on int and float fields");
                    break;
            }
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, PercentSliderAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif