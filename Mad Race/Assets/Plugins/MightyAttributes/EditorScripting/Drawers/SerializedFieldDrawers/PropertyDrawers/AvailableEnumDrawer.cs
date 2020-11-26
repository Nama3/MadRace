#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class AvailableEnumDrawer : BasePropertyDrawer<AvailableEnumAttribute>, IArrayElementDrawer, IRefreshDrawer
    {
        private readonly MightyCache<(bool, MightyInfo<int[]>, string[])> m_availableEnumCache =
            new MightyCache<(bool, MightyInfo<int[]>, string[])>();


        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property,
            AvailableEnumAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawAvailableEnum(mightyMember, property, attribute);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawAvailableEnum(serializedField, serializedField.GetElement(index), (AvailableEnumAttribute) baseAttribute);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawAvailableEnum(serializedField, serializedField.GetElement(index), (AvailableEnumAttribute) baseAttribute, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) 
            => DrawAvailableEnum(position, serializedField, serializedField.GetElement(index), (AvailableEnumAttribute) baseAttribute);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            serializedField.GetElement(index).propertyType != SerializedPropertyType.Enum
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;

        public void DrawAvailableEnum(BaseMightyMember mightyMember, SerializedProperty property, AvailableEnumAttribute attribute,
            GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"\"{property.displayName}\" type should be an enum");
                return;
            }

            if (!m_availableEnumCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, valuesInfo, names) = m_availableEnumCache[mightyMember];

            if (!valid) return;

            if (valuesInfo.Value.Length == 0)
            {
                property.intValue = 0;
                return;
            }

            var selectedIndex = GetSelectedIndex(property, valuesInfo.Value);

            EditorGUI.BeginChangeCheck();

            selectedIndex = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUILayout.Popup(selectedIndex, names)
                : EditorGUILayout.Popup(label ?? EditorGUIUtility.TrTextContent(property.displayName), selectedIndex, names);

            if (EditorGUI.EndChangeCheck() && !EditorApplication.isPlaying)
                property.intValue = GetSelectedValue(selectedIndex, valuesInfo.Value);
        }

        public void DrawAvailableEnum(Rect position, BaseMightyMember mightyMember, SerializedProperty property,
            AvailableEnumAttribute attribute)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"\"{property.displayName}\" type should be an enum");
                return;
            }

            if (!m_availableEnumCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, valuesInfo, names) = m_availableEnumCache[mightyMember];

            if (!valid) return;

            if (valuesInfo.Value.Length == 0)
            {
                property.intValue = 0;
                return;
            }

            var selectedIndex = GetSelectedIndex(property, valuesInfo.Value);

            EditorGUI.BeginChangeCheck();

            selectedIndex = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUI.Popup(position, selectedIndex, names)
                : EditorGUI.Popup(position, property.displayName, selectedIndex, names);

            if (EditorGUI.EndChangeCheck() && !EditorApplication.isPlaying)
                property.intValue = GetSelectedValue(selectedIndex, valuesInfo.Value);
        }

        private static string[] GetNames(Array allValues, int[] availableValues, bool allowNothing)
        {
            if (availableValues == null || availableValues.Length == 0) return null;

            var names = new string[availableValues.Length];

            for (int i = 0, j = allowNothing ? 1 : 0; i < allValues.Length; i++)
            {
                var value = allValues.GetValue(i);
                if (!availableValues.Contains(Convert.ToInt32(value))) continue;
                names[j++] = value.ToString();
            }

            if (allowNothing) names[0] = "Nothing";

            return names;
        }

        private static int GetSelectedIndex(SerializedProperty property, int[] availableValues)
        {
            var selectedIndex = 0;
            var selectedValue = property.intValue;

            for (var i = 0; i < availableValues.Length; i++)
            {
                if (selectedValue != availableValues[i]) continue;
                selectedIndex = i;
                break;
            }

            return selectedIndex;
        }

        private static int GetSelectedValue(int selectedIndexes, int[] values) => values[selectedIndexes];

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, AvailableEnumAttribute attribute)
        {
            var type = serializedField.PropertyType;

            if (!serializedField.GetArrayInfoFromMember(attribute.Target, attribute.AvailableValuesCallback, out var mightyInfo))
            {
                m_availableEnumCache[serializedField] = (false, null, null);
                return;
            }

            var availableValues = mightyInfo.Value.Select(Convert.ToInt32).ToArray();

            var allowNothing = attribute.AllowNothing && type.GetCustomAttribute(typeof(FlagsAttribute), true) != null;
            if (allowNothing) availableValues = ArrayUtilities.InsertArrayElement(availableValues, 0);

            var valuesInfo = new MightyInfo<int[]>(mightyInfo, availableValues);
            var names = GetNames(Enum.GetValues(type), availableValues, allowNothing);

            m_availableEnumCache[serializedField] = (true, valuesInfo, names);
        }

        protected override void ClearCache() => m_availableEnumCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_availableEnumCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, valuesInfo, _) = m_availableEnumCache[mightyMember];
            if (!valid) return;

            var type = ((MightySerializedField) mightyMember).PropertyType;

            var values = valuesInfo.RefreshValue().Select(Convert.ToInt32).ToArray();

            var allowNothing = ((AvailableEnumAttribute) mightyAttribute).AllowNothing &&
                               type.GetCustomAttribute(typeof(FlagsAttribute), true) != null;

            if (allowNothing) values = ArrayUtilities.InsertArrayElement(values, 0);

            valuesInfo.Value = values;
            var names = GetNames(Enum.GetValues(type), values, allowNothing);

            m_availableEnumCache[mightyMember] = (true, valuesInfo, names);
        }
    }
}
#endif