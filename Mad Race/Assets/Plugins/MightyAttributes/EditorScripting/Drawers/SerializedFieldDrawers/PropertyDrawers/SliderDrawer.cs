#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class SliderDrawer : BasePropertyDrawer<SliderAttribute>, IArrayElementDrawer, IRefreshDrawer
    {
        private readonly MightyCache<(bool, MightyInfo<float>, MightyInfo<float>)> m_sliderCache =
            new MightyCache<(bool, MightyInfo<float>, MightyInfo<float>)>();

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property,
            SliderAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawSlider(mightyMember, property, attribute);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawSlider(serializedField, serializedField.GetElement(index), (SliderAttribute) baseAttribute);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawSlider(serializedField, serializedField.GetElement(index), (SliderAttribute) baseAttribute, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawSlider(position, serializedField, serializedField.GetElement(index), (SliderAttribute) baseAttribute);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var propertyType = serializedField.GetElement(index).propertyType;
            return propertyType != SerializedPropertyType.Integer && propertyType != SerializedPropertyType.Float
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;
        }

        public void DrawSlider(BaseMightyMember mightyMember, SerializedProperty property, SliderAttribute attribute,
            GUIContent label = null)
        {
            if (!m_sliderCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, min, max) = m_sliderCache[mightyMember];

            if (!valid)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"{nameof(SliderAttribute)} can be used only on int or float fields");
                return;
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = attribute.Options.Contains(FieldOption.HideLabel)
                        ? EditorGUILayout.IntSlider(property.intValue, (int) min.Value, (int) max.Value)
                        : EditorGUILayout.IntSlider(label ?? EditorGUIUtility.TrTextContent(property.displayName), property.intValue,
                            (int) min.Value, (int) max.Value);
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = attribute.Options.Contains(FieldOption.HideLabel)
                        ? EditorGUILayout.Slider(property.floatValue, min.Value, max.Value)
                        : EditorGUILayout.Slider(label ?? EditorGUIUtility.TrTextContent(property.displayName), property.floatValue,
                            min.Value, max.Value);
                    break;
            }
        }

        public void DrawSlider(Rect position, BaseMightyMember mightyMember, SerializedProperty property, SliderAttribute attribute)
        {
            if (!m_sliderCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, min, max) = m_sliderCache[mightyMember];

            if (!valid)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"{nameof(SliderAttribute)} can be used only on int or float fields");
                return;
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = attribute.Options.Contains(FieldOption.HideLabel)
                        ? EditorGUI.IntSlider(position, property.intValue, (int) min.Value, (int) max.Value)
                        : EditorGUI.IntSlider(position, property.displayName, property.intValue, (int) min.Value, (int) max.Value);
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = attribute.Options.Contains(FieldOption.HideLabel)
                        ? EditorGUI.Slider(position, property.floatValue, min.Value, max.Value)
                        : EditorGUI.Slider(position, property.displayName, property.floatValue, min.Value, max.Value);
                    break;
            }
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, SliderAttribute attribute)
        {
            var target = attribute.Target;

            switch (serializedField.Property.GetPropertyType())
            {
                case SerializedPropertyType.Integer:
                {
                    var minValueInfo = new MightyInfo<float>(null, null, attribute.MinValue);
                    if (serializedField.GetInfoFromMember<int>(target, attribute.MinValueCallback, out var minInfo, int.TryParse))
                        minValueInfo = new MightyInfo<float>(minInfo, minInfo.Value);

                    var maxValueInfo = new MightyInfo<float>(null, null, attribute.MaxValue);
                    if (serializedField.GetInfoFromMember<int>(target, attribute.MaxValueCallback, out var maxInfo, int.TryParse))
                        maxValueInfo = new MightyInfo<float>(maxInfo, maxInfo.Value);

                    m_sliderCache[serializedField] = (true, minValueInfo, maxValueInfo);
                    break;
                }
                case SerializedPropertyType.Float:
                {
                    var minValueInfo = new MightyInfo<float>(null, null, attribute.MinValue);
                    if (serializedField.GetInfoFromMember<float>(target, attribute.MinValueCallback, out var minInfo, float.TryParse))
                        minValueInfo = new MightyInfo<float>(minInfo, minInfo.Value);


                    var maxValueInfo = new MightyInfo<float>(null, null, attribute.MaxValue);
                    if (serializedField.GetInfoFromMember<float>(target, attribute.MaxValueCallback, out var maxInfo, float.TryParse))
                        maxValueInfo = new MightyInfo<float>(maxInfo, maxInfo.Value);

                    m_sliderCache[serializedField] = (true, minValueInfo, maxValueInfo);
                    break;
                }
                default:
                    m_sliderCache[serializedField] = default;
                    break;
            }
        }

        protected override void ClearCache() => m_sliderCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_sliderCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, min, max) = m_sliderCache[mightyMember];
            if (!valid) return;

            min.RefreshValue();
            max.RefreshValue();
        }
    }
}
#endif