#if UNITY_EDITOR
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ProgressBarDrawer : BasePropertyDrawer<ProgressBarAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<(MightyInfo<float>, MightyInfo<Color?>)> m_progressBarCache =
            new MightyCache<(MightyInfo<float>, MightyInfo<Color?>)>();

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, ProgressBarAttribute attribute)
        {
            if (property.propertyType != SerializedPropertyType.Float && property.propertyType != SerializedPropertyType.Integer)
            {
                MightyGUIUtilities.DrawHelpBox($"Field {property.name} is not a number");
                return;
            }

            var value = property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue;
            var valueFormatted = property.propertyType == SerializedPropertyType.Integer
                ? value.ToString(CultureInfo.InvariantCulture)
                : $"{value:0.00}";

            var progressBarAttribute = attribute;

            if (!m_progressBarCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (max, color) = m_progressBarCache[mightyMember];

            var fillPercentage = value / max.Value;
            var barLabel =
                $"{(!string.IsNullOrEmpty(progressBarAttribute.Label) ? $"[{progressBarAttribute.Label}] " : "")}{valueFormatted}/{max.Value}";

            DrawBar(MightyGUIUtilities.FIELD_HEIGHT, Mathf.Clamp01(fillPercentage), barLabel, color.Value ?? Color.white, Color.white);
        }

        public static void DrawBar(float height, float fillPercent, string label, Color barColor, Color labelColor) =>
            DrawBar(EditorGUILayout.GetControlRect(true, height), Mathf.Clamp01(fillPercent), label, barColor, labelColor);

        public static void DrawBar(Rect position, float fillPercent, string label, Color barColor, Color labelColor)
        {
            if (Event.current.type != EventType.Repaint) return;

            EditorGUI.DrawRect(position, MightyColorUtilities.Darker);
            EditorGUI.DrawRect(new Rect(position.x, position.y, position.width * fillPercent, position.height), barColor);

            var align = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            MightyColorUtilities.BeginContentColor(labelColor);

            EditorGUI.DropShadowLabel(new Rect(position.x, (position.height - MightyGUIUtilities.FIELD_HEIGHT) / 2, position.width,
                    MightyGUIUtilities.FIELD_HEIGHT), label);

            MightyColorUtilities.EndContentColor();
            GUI.skin.label.alignment = align;
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, ProgressBarAttribute attribute)
        {
            var propertyType = serializedField.Property.GetPropertyType();

            if (propertyType == SerializedPropertyType.Float || propertyType == SerializedPropertyType.Integer)
            {
                var target = attribute.Target;

                var maxValueInfo = new MightyInfo<float>(null, null, attribute.MaxValue);
                if (serializedField.GetInfoFromMember<float>(target, attribute.MaxValueCallback, out var maxInfo))
                    maxValueInfo = new MightyInfo<float>(maxInfo, maxInfo.Value);

                else if (serializedField.GetInfoFromMember<int>(target, attribute.MaxValueCallback, out var maxInfoInt))
                    maxValueInfo = new MightyInfo<float>(maxInfoInt, maxInfoInt.Value);

                var colorInfo = serializedField.GetColorInfo(target, attribute.ColorName, attribute.Color);

                m_progressBarCache[serializedField] = (maxValueInfo, colorInfo);
            }
            else
                m_progressBarCache[serializedField] = default;
        }

        protected override void ClearCache() => m_progressBarCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_progressBarCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var propertyType = ((MightySerializedField) mightyMember).Property.GetPropertyType();
            if (propertyType != SerializedPropertyType.Float && propertyType != SerializedPropertyType.Integer) return;

            var (max, color) = m_progressBarCache[mightyMember];

            max.RefreshValue();
            color.RefreshValue();
        }
    }
}
#endif