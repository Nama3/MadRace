#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class AvailableMaskDrawer : BasePropertyDrawer<AvailableMaskAttribute>, IArrayElementDrawer, IRefreshDrawer
    {
        private readonly MightyCache<(bool, MightyInfo<int>, int[], string[])> m_availableMaskCache =
            new MightyCache<(bool, MightyInfo<int>, int[], string[])>();

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property,
            AvailableMaskAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawAvailableMask(mightyMember, property, attribute);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawAvailableMask(serializedField, serializedField.GetElement(index), (AvailableMaskAttribute) baseAttribute);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawAvailableMask(serializedField, serializedField.GetElement(index), (AvailableMaskAttribute) baseAttribute, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawAvailableMask(position, serializedField, serializedField.GetElement(index), (AvailableMaskAttribute) baseAttribute);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            if (!m_availableMaskCache.Contains(serializedField)) EnableDrawer(serializedField, (AvailableMaskAttribute) baseAttribute);
            return m_availableMaskCache[serializedField].Item1
                ? MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING
                : MightyGUIUtilities.WARNING_HEIGHT;
        }

        public void DrawAvailableMask(BaseMightyMember mightyMember, SerializedProperty property, AvailableMaskAttribute attribute,
            GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"\"{property.displayName}\" type should be an enum");
                return;
            }

            if (!m_availableMaskCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, _, values, names) = m_availableMaskCache[mightyMember];

            if (values == null || values.Length == 0)
            {
                property.intValue = 0;
                return;
            }

            var selectedIndexes = GetSelectedIndexes(property, values, attribute.AllowEverything);

            EditorGUI.BeginChangeCheck();

            selectedIndexes = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUILayout.MaskField(selectedIndexes, names)
                : EditorGUILayout.MaskField(label ?? EditorGUIUtility.TrTextContent(property.displayName), selectedIndexes, names);

            if (EditorGUI.EndChangeCheck() && !EditorApplication.isPlaying)
                property.intValue = GetSelectedMask(selectedIndexes, values);

            if (!valid)
                MightyGUIUtilities.DrawHelpBox($"Enum \"{property.displayName}\" is not marked by [Flags] attribute");
        }

        public void DrawAvailableMask(Rect position, BaseMightyMember mightyMember, SerializedProperty property,
            AvailableMaskAttribute attribute)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"\"{property.displayName}\" type should be an enum");
                return;
            }

            if (!m_availableMaskCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, _, values, names) = m_availableMaskCache[mightyMember];

            if (values == null || values.Length == 0)
            {
                property.intValue = 0;
                return;
            }

            var selectedIndexes = GetSelectedIndexes(property, values, attribute.AllowEverything);

            EditorGUI.BeginChangeCheck();

            selectedIndexes = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUI.MaskField(position, selectedIndexes, names)
                : EditorGUI.MaskField(position, property.displayName, selectedIndexes, names);

            if (EditorGUI.EndChangeCheck() && !EditorApplication.isPlaying)
                property.intValue = GetSelectedMask(selectedIndexes, values);

            if (!valid)
                MightyGUIUtilities.DrawHelpBox(position, $"Enum \"{property.displayName}\" is not marked by [Flags] attribute");
        }

        public static (int[], string[]) GetValuesAndNames(Type enumType, int availableMask)
        {
            if (availableMask == 0) return (null, null);

            var allValues = Enum.GetValues(enumType);

            var nameList = new List<string>();
            var valueList = new List<int>();

            for (var i = 0; i < allValues.Length; i++)
            {
                var value = allValues.GetValue(i);
                var intValue = Convert.ToInt32(value);
                if (!availableMask.MaskContains(intValue)) continue;

                nameList.Add(value.ToString());
                valueList.Add(intValue);
            }

            return (valueList.ToArray(), nameList.ToArray());
        }

        public static int GetSelectedIndexes(SerializedProperty property, int[] values, bool allowEverything)
        {
            var everything = allowEverything;
            var selectedIndexes = 0;
            var selectedValues = property.intValue;

            for (var i = 0; i < values.Length; i++)
                if (selectedValues.MaskContains(values[i]))
                    selectedIndexes |= i.ToBitMask();
                else
                    everything = false;

            if (everything) selectedIndexes = -1;

            return selectedIndexes;
        }

        public static int GetSelectedMask(int selectedIndexes, int[] values)
        {
            var selectedValues = 0;

            for (var i = 0; i < values.Length; i++)
                if ((selectedIndexes & i.ToBitMask()) != 0)
                    selectedValues = selectedValues.AddFlag(values[i]);

            return selectedValues;
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, AvailableMaskAttribute attribute)
        {
            var property = serializedField.Property;

            var availableMask = attribute.AvailableMask;
            if (serializedField.GetInfoFromMember<int>(attribute.Target, attribute.AvailableMaskCallback, out var availableMaskInfo))
                availableMask = availableMaskInfo.Value;
            else
                availableMaskInfo = new MightyInfo<int>(availableMask);

            var enumType = serializedField.PropertyType;
            var valid = property.GetPropertyType() == SerializedPropertyType.Enum &&
                        enumType.GetCustomAttribute(typeof(FlagsAttribute), true) != null;

            var (values, names) = GetValuesAndNames(enumType, availableMask);

            m_availableMaskCache[serializedField] = (valid, availableMaskInfo, values, names);
        }

        protected override void ClearCache() => m_availableMaskCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_availableMaskCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, availableMaskInfo, _, _) = m_availableMaskCache[mightyMember];

            var (values, names) =
                GetValuesAndNames(((MightySerializedField) mightyMember).PropertyType, availableMaskInfo.RefreshValue());

            m_availableMaskCache[mightyMember] = (valid, availableMaskInfo, values, names);
        }
    }
}
#endif