#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class MaskDrawer : BasePropertyDrawer<MaskAttribute>, IArrayElementDrawer, IRefreshDrawer
    {
        private readonly MightyCache<(bool, string[], MightyInfo<string[]>)> m_maskCache =
            new MightyCache<(bool, string[], MightyInfo<string[]>)>();

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, MaskAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawMask(mightyMember, property, attribute);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawMask(serializedField, serializedField.GetElement(index), (MaskAttribute) baseAttribute);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawMask(serializedField, serializedField.GetElement(index), (MaskAttribute) baseAttribute, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawMask(position, serializedField, serializedField.GetElement(index), (MaskAttribute) baseAttribute);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            if (!m_maskCache.Contains(serializedField)) EnableDrawer(serializedField, baseAttribute);
            return m_maskCache[serializedField].Item1
                ? MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING
                : MightyGUIUtilities.WARNING_HEIGHT;
        }

        public void DrawMask(BaseMightyMember mightyMember, SerializedProperty property, MaskAttribute attribute, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"\"{property.displayName}\" type should be an integer");
                return;
            }

            if (!m_maskCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, maskNames, maskNamesInfo) = m_maskCache[mightyMember];

            if (maskNamesInfo != null) maskNames = maskNamesInfo.Value;

            if (!valid)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox("The mask names callback is invalid or there's no names specified");
                return;
            }

            if (maskNames.Length == 0)
            {
                property.intValue = 0;
                return;
            }

            property.intValue = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUILayout.MaskField(property.intValue, maskNames)
                : EditorGUILayout.MaskField(label ?? EditorGUIUtility.TrTextContent(property.displayName), property.intValue, maskNames);
        }

        public void DrawMask(Rect position, BaseMightyMember mightyMember, SerializedProperty property, MaskAttribute attribute)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"\"{property.displayName}\" type should be an integer");
                return;
            }

            if (!m_maskCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, maskNames, maskNamesInfo) = m_maskCache[mightyMember];

            if (maskNamesInfo != null) maskNames = maskNamesInfo.Value;

            if (!valid)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, "The mask names callback is invalid or there's no names specified");
                return;
            }

            if (maskNames.Length == 0)
            {
                property.intValue = 0;
                return;
            }

            property.intValue = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUI.MaskField(position, property.intValue, maskNames)
                : EditorGUI.MaskField(position, property.displayName, property.intValue, maskNames);
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, MaskAttribute attribute)
        {
            var maskNames = attribute.MaskNames;
            if (maskNames?.Length > 0)
            {
                m_maskCache[serializedField] = (true, maskNames, null);
                return;
            }

            var maskNamesInfo = new MightyInfo<string[]>(null, null, maskNames);

            var valid = serializedField.GetArrayInfoFromMember(attribute.Target, attribute.MaskNamesCallback, out var valuesFromInfo);

            if (valid) maskNamesInfo = new MightyInfo<string[]>(valuesFromInfo, valuesFromInfo.Value.Select(n => n.ToString()).ToArray());

            m_maskCache[serializedField] = (valid, null, maskNamesInfo);
        }

        protected override void ClearCache() => m_maskCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_maskCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, _, maskNamesInfo) = m_maskCache[mightyMember];

            if (valid) maskNamesInfo?.RefreshValue();
        }
    }
}
#endif