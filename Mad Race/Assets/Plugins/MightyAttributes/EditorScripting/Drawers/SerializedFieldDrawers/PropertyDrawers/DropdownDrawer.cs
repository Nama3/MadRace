#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class DropdownDrawer : BasePropertyDrawer<DropdownAttribute>, IArrayElementDrawer, IRefreshDrawer
    {
        private readonly MightyCache<(bool, MightyInfo<object[]>, MightyInfo<IDropdownValues>, MightyInfo<object>)> m_dropdownCache =
            new MightyCache<(bool, MightyInfo<object[]>, MightyInfo<IDropdownValues>, MightyInfo<object>)>();

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, DropdownAttribute attribute)
        {
            if (!m_dropdownCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, _, _, _) = m_dropdownCache[mightyMember];
            if (!valid)
            {
                MightyGUIUtilities.DrawPropertyField(property);
                MightyGUIUtilities.DrawHelpBox($"{nameof(DropdownAttribute)} for {mightyMember.MemberName} is invalid");
                return;
            }

            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            var (_, valuesInfo, dropdownValuesInfo, propertyInfo) = m_dropdownCache[mightyMember];

            var value = DrawDropdown(valuesInfo, dropdownValuesInfo, propertyInfo, property, propertyInfo.Value,
                attribute);
            if (value != null)
                propertyInfo.SetNewValue(value);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var (_, valuesInfo, dropdownValuesInfo, propertyInfo) = m_dropdownCache[serializedField];

            if (!(propertyInfo.Value is IList array)) return;

            if (index >= array.Count)
            {
                EnableDrawer(serializedField, baseAttribute);
                return;
            }

            var value = DrawDropdown(valuesInfo, dropdownValuesInfo, propertyInfo, serializedField.GetElement(index), array[index],
                (DropdownAttribute) baseAttribute);

            if (value == null || array.Count <= index) return;

            array[index] = value;
            propertyInfo.Value = array;
        }

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute)
        {
            var (_, valuesInfo, dropdownValuesInfo, propertyInfo) = m_dropdownCache[serializedField];

            if (!(propertyInfo.Value is IList array)) return;

            if (index >= array.Count)
            {
                EnableDrawer(serializedField, baseAttribute);
                return;
            }

            var value = DrawDropdown(valuesInfo, dropdownValuesInfo, propertyInfo, serializedField.GetElement(index), array[index],
                (DropdownAttribute) baseAttribute, label);

            if (value == null || array.Count <= index) return;

            array[index] = value;
            propertyInfo.Value = array;
        }

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var (_, valuesInfo, dropdownValuesInfo, propertyInfo) = m_dropdownCache[serializedField];

            if (!(propertyInfo.Value is IList array)) return;

            if (index >= array.Count)
            {
                EnableDrawer(serializedField, baseAttribute);
                return;
            }

            var value = DrawDropdown(position, valuesInfo, dropdownValuesInfo, propertyInfo, serializedField.GetElement(index),
                array[index], (DropdownAttribute) baseAttribute);

            if (value == null || array.Count <= index) return;

            array[index] = value;
            propertyInfo.Value = array;
        }

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute attribute)
        {
            if (!m_dropdownCache.Contains(serializedField)) EnableDrawer(serializedField, attribute);
            return m_dropdownCache[serializedField].Item1
                ? MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING
                : MightyGUIUtilities.WARNING_HEIGHT;
        }


        private static object DrawDropdown(MightyInfo<object[]> valuesInfo, MightyInfo<IDropdownValues> dropdownValuesInfo,
            MightyInfo<object> propertyInfo, SerializedProperty property, object selectedValue, DropdownAttribute dropdownAttribute,
            GUIContent label = null)
        {
            if (valuesInfo == null && dropdownValuesInfo == null)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox(
                    $"{nameof(DropdownAttribute)} callback name \"{dropdownAttribute.ValuesFieldName}\" is invalid");
                return null;
            }

            if (GetDisplayOptions(valuesInfo, dropdownValuesInfo, propertyInfo, selectedValue, out var displayOptions, out var values,
                out var selectedValueIndex))
                return MightyGUIUtilities.DrawDropdown(label ?? EditorGUIUtility.TrTextContent(property.displayName),
                    selectedValueIndex, values, displayOptions, dropdownAttribute.Options);

            MightyGUIUtilities.DrawPropertyField(property, label);
            MightyGUIUtilities.DrawHelpBox(
                $"{nameof(DropdownAttribute)} works only when the type of the field is equal to the element type of the array");

            return null;
        }

        private static object DrawDropdown(Rect position, MightyInfo<object[]> valuesInfo, MightyInfo<IDropdownValues> dropdownValuesInfo,
            MightyInfo<object> propertyInfo, SerializedProperty property, object selectedValue, DropdownAttribute dropdownAttribute)
        {
            if (valuesInfo == null && dropdownValuesInfo == null)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position,
                    $"{nameof(DropdownAttribute)} callback name \"{dropdownAttribute.ValuesFieldName}\" is invalid");
                return null;
            }

            if (GetDisplayOptions(valuesInfo, dropdownValuesInfo, propertyInfo, selectedValue, out var displayOptions, out var values,
                out var selectedValueIndex))
                return MightyGUIUtilities.DrawDropdown(position, property.displayName, selectedValueIndex,
                    values, displayOptions, dropdownAttribute.Options);

            position = MightyGUIUtilities.DrawPropertyField(position, property);
            MightyGUIUtilities.DrawHelpBox(position,
                $"{nameof(DropdownAttribute)} works only when the type of the field is equal to the element type of the array");

            return null;
        }

        private static bool GetDisplayOptions(MightyInfo<object[]> valuesInfo, MightyInfo<IDropdownValues> dropdownValuesInfo,
            MightyInfo<object> propertyInfo, object selectedValue, out string[] displayOptions, out object[] values,
            out int selectedValueIndex)
        {
            selectedValueIndex = -1;
            values = null;
            displayOptions = null;

            if (valuesInfo != null)
            {
                if (valuesInfo.Value == null ||
                    propertyInfo.MemberType != valuesInfo.ElementType && propertyInfo.ElementType != valuesInfo.ElementType) return false;

                displayOptions = GetDisplayOptions(valuesInfo.Value, selectedValue, out values, out selectedValueIndex);
                return true;
            }

            if (dropdownValuesInfo != null)
            {
                var genericDropdownType = dropdownValuesInfo.MemberType.GenericTypeArguments[0];
                if (dropdownValuesInfo.Value == null ||
                    propertyInfo.MemberType != genericDropdownType && propertyInfo.ElementType != genericDropdownType) return false;

                displayOptions = GetDisplayOptions(dropdownValuesInfo.Value, selectedValue, out values, out selectedValueIndex);
                return true;
            }

            return false;
        }

        private static string[] GetDisplayOptions(IList valuesList, object selectedValue, out object[] values, out int selectedValueIndex)
        {
            selectedValueIndex = 0;

            values = new object[valuesList.Count];
            var displayOptions = new string[valuesList.Count];

            for (var i = 0; i < values.Length; i++)
            {
                var value = valuesList[i];
                values[i] = value;
                displayOptions[i] = value.ToString();

                if (value.Equals(selectedValue))
                    selectedValueIndex = i;
            }

            return displayOptions;
        }

        private static string[] GetDisplayOptions(IDropdownValues dropdownValues, object selectedValue, out object[] values,
            out int selectedValueIndex)
        {
            var index = 0;
            var increaseIndex = true;
            selectedValueIndex = 0;

            var valuesList = new List<object>();
            var displayOptions = new List<string>();
            using (var dropdownEnumerator = dropdownValues.GetEnumerator())
            {
                while (dropdownEnumerator.MoveNext())
                {
                    var current = dropdownEnumerator.Current;

                    if (current.Value?.Equals(selectedValue) == true)
                    {
                        increaseIndex = false;
                        selectedValueIndex = index;
                    }

                    if (increaseIndex)
                        index++;

                    valuesList.Add(current.Value);

                    var key = current.Key;
                    displayOptions.Add(key ?? string.Empty);
                }
            }

            values = valuesList.ToArray();
            return displayOptions.ToArray();
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, DropdownAttribute attribute)
        {
            var property = serializedField.Property;
            var target = attribute.Target;

            if (serializedField.GetArrayInfoFromMember(target, attribute.ValuesFieldName, out var valuesInfo))
            {
                var valid = serializedField.GetInfoFromMember<object>(target, property.name, out var propertyInfo, neverInWrapper: true);
                m_dropdownCache[serializedField] = (valid, valuesInfo, null, propertyInfo);
            }
            else if (serializedField.GetInfoFromMember<IDropdownValues>(target, attribute.ValuesFieldName, out var dropdownValuesInfo))
            {
                var valid = serializedField.GetInfoFromMember<object>(target, property.name, out var propertyInfo, neverInWrapper: true);
                m_dropdownCache[serializedField] = (valid, null, dropdownValuesInfo, propertyInfo);
            }
            else
                m_dropdownCache[serializedField] = (false, null, null, null);
        }

        protected override void ClearCache() => m_dropdownCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_dropdownCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, valuesInfo, dropdownValuesInfo, propertyInfo) = m_dropdownCache[mightyMember];
            if (!valid) return;

            valuesInfo?.RefreshValue();
            dropdownValuesInfo?.RefreshValue();
            propertyInfo.RefreshValue();
        }
    }
}
#endif