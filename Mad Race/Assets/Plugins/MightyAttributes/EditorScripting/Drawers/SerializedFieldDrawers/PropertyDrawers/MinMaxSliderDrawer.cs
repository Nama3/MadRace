#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class MinMaxSliderDrawer : BasePropertyDrawer<MinMaxSliderAttribute>, IArrayElementDrawer, IRefreshDrawer
    {
        private readonly MightyCache<(MightyInfo<float>, MightyInfo<float>)> m_minMaxSliderCache =
            new MightyCache<(MightyInfo<float>, MightyInfo<float>)>();

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property,
            MinMaxSliderAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawSlider(mightyMember, property, attribute.Options);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawSlider(serializedField, serializedField.GetElement(index), baseAttribute.Options);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawSlider(serializedField, serializedField.GetElement(index), baseAttribute.Options, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawSlider(position, serializedField, serializedField.GetElement(index), baseAttribute.Options);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var propertyType = serializedField.GetElement(index).propertyType;
            return propertyType != SerializedPropertyType.Vector2 && propertyType != SerializedPropertyType.Vector2Int
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;
        }

        private void DrawSlider(BaseMightyMember mightyMember, SerializedProperty property, FieldOption options, GUIContent label = null)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                {
                    var (min, max) = m_minMaxSliderCache[mightyMember];
                    DrawFloatSlider(EditorGUILayout.GetControlRect(), min.Value, max.Value, property, options, label);
                    break;
                }
                case SerializedPropertyType.Vector2Int:
                {
                    var (min, max) = m_minMaxSliderCache[mightyMember];
                    DrawIntSlider(EditorGUILayout.GetControlRect(), (int) min.Value, (int) max.Value, property, options);
                    break;
                }
                default:
                    MightyGUIUtilities.DrawPropertyField(property, label);
                    MightyGUIUtilities.DrawHelpBox($"{nameof(MinMaxSliderAttribute)} can be used only on Vector2 fields");
                    return;
            }
        }

        private void DrawSlider(Rect position, BaseMightyMember mightyMember, SerializedProperty property, FieldOption options)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2:
                {
                    var (min, max) = m_minMaxSliderCache[mightyMember];
                    DrawFloatSlider(position, min.Value, max.Value, property, options);
                    break;
                }
                case SerializedPropertyType.Vector2Int:
                {
                    var (min, max) = m_minMaxSliderCache[mightyMember];
                    DrawIntSlider(position, (int) min.Value, (int) max.Value, property, options);
                    break;
                }
                default:
                    position = MightyGUIUtilities.DrawPropertyField(position, property);
                    MightyGUIUtilities.DrawHelpBox(position, $"{nameof(MinMaxSliderAttribute)} can be used only on Vector2 fields");
                    return;
            }
        }

        private static Rect GetLabelRect(Rect position)
        {
            var labelWidth = EditorGUIUtility.labelWidth;

            return new Rect(position.x, position.y, labelWidth, MightyGUIUtilities.FIELD_HEIGHT);
        }

        private static Rect GetSliderRect(Rect position)
        {
            var labelWidth = EditorGUIUtility.labelWidth;
            var fieldWidth = EditorGUIUtility.fieldWidth;
            var sliderWidth = position.width - labelWidth - 2f * fieldWidth;
            var sliderPadding = 5f;

            return new Rect(position.x + labelWidth + fieldWidth + sliderPadding, position.y, sliderWidth - 2f * sliderPadding,
                MightyGUIUtilities.FIELD_HEIGHT);
        }

        private static Rect GetMinFieldRect(Rect position)
        {
            var labelWidth = EditorGUIUtility.labelWidth;
            var fieldWidth = EditorGUIUtility.fieldWidth;

            return new Rect(position.x + labelWidth, position.y, fieldWidth, MightyGUIUtilities.FIELD_HEIGHT);
        }

        private static Rect GetMaxFieldRect(Rect position)
        {
            var labelWidth = EditorGUIUtility.labelWidth;
            var fieldWidth = EditorGUIUtility.fieldWidth;
            var sliderWidth = position.width - labelWidth - 2f * fieldWidth;

            return new Rect(position.x + labelWidth + fieldWidth + sliderWidth, position.y, fieldWidth, MightyGUIUtilities.FIELD_HEIGHT);
        }

        private void DrawFloatSlider(Rect position, float min, float max, SerializedProperty property, FieldOption options,
            GUIContent label = null)
        {
            var labelRect = GetLabelRect(position);
            DrawLabel(ref labelRect, property, options, label);

            // DrawSlider the slider
            EditorGUI.BeginChangeCheck();

            var sliderValue = property.vector2Value;
            EditorGUI.MinMaxSlider(GetSliderRect(position), ref sliderValue.x, ref sliderValue.y, min, max);

            sliderValue.x = EditorGUI.FloatField(GetMinFieldRect(position), sliderValue.x);
            sliderValue.x = Mathf.Clamp(sliderValue.x, min, Mathf.Min(max, sliderValue.y));

            sliderValue.y = EditorGUI.FloatField(GetMaxFieldRect(position), sliderValue.y);
            sliderValue.y = Mathf.Clamp(sliderValue.y, Mathf.Max(min, sliderValue.x), max);

            if (EditorGUI.EndChangeCheck()) property.vector2Value = sliderValue;
        }

        private void DrawIntSlider(Rect position, int min, int max, SerializedProperty property, FieldOption options,
            GUIContent label = null)
        {
            var labelRect = GetLabelRect(position);
            DrawLabel(ref labelRect, property, options, label);

            // DrawSlider the slider
            EditorGUI.BeginChangeCheck();

            var sliderValue = property.vector2IntValue;
            float xSlider = sliderValue.x;
            float ySlider = sliderValue.y;

            EditorGUI.MinMaxSlider(GetSliderRect(position), ref xSlider, ref ySlider, min, max);

            sliderValue = new Vector2Int((int) xSlider, (int) ySlider);

            sliderValue.x = EditorGUI.IntField(GetMinFieldRect(position), sliderValue.x);
            sliderValue.x = Mathf.Clamp(sliderValue.x, min, Mathf.Min(max, sliderValue.y));

            sliderValue.y = EditorGUI.IntField(GetMaxFieldRect(position), sliderValue.y);
            sliderValue.y = Mathf.Clamp(sliderValue.y, Mathf.Max(min, sliderValue.x), max);

            if (EditorGUI.EndChangeCheck()) property.vector2IntValue = sliderValue;
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, MinMaxSliderAttribute attribute)
        {
            var propertyType = serializedField.Property.GetPropertyType();

            if (propertyType == SerializedPropertyType.Vector2 || propertyType == SerializedPropertyType.Vector2Int)
            {
                var target = attribute.Target;

                var minValueInfo = new MightyInfo<float>(null, null, attribute.MinValue);
                if (serializedField.GetInfoFromMember<float>(target, attribute.MinValueCallback, out var minInfo))
                    minValueInfo = new MightyInfo<float>(minInfo, minInfo.Value);

                var maxValueInfo = new MightyInfo<float>(null, null, attribute.MaxValue);
                if (serializedField.GetInfoFromMember<float>(target, attribute.MaxValueCallback, out var maxInfo))
                    maxValueInfo = new MightyInfo<float>(maxInfo, maxInfo.Value);

                m_minMaxSliderCache[serializedField] = (minValueInfo, maxValueInfo);
            }
            else
                m_minMaxSliderCache[serializedField] = default;
        }

        protected override void ClearCache() => m_minMaxSliderCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_minMaxSliderCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            if (((MightySerializedField) mightyMember).Property.GetPropertyType() != SerializedPropertyType.Vector2) return;

            var (min, max) = m_minMaxSliderCache[mightyMember];

            min.RefreshValue();
            max.RefreshValue();
        }
    }
}
#endif