#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class NotFlagsDrawer : BasePropertyDrawer<NotFlagsAttribute>, IArrayElementDrawer
    {
        private readonly MightyCache<(bool, Type, string[])> m_notFlagsCache = new MightyCache<(bool, Type, string[])>();

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property,
            NotFlagsAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawNotFlags(mightyMember, property, attribute);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawNotFlags(serializedField, serializedField.GetElement(index), (NotFlagsAttribute) baseAttribute);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawNotFlags(serializedField, serializedField.GetElement(index), (NotFlagsAttribute) baseAttribute, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawNotFlags(position, serializedField, serializedField.GetElement(index), (NotFlagsAttribute) baseAttribute);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            if (!m_notFlagsCache.Contains(serializedField)) EnableDrawer(serializedField, baseAttribute);
            return m_notFlagsCache[serializedField].Item1
                ? MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING
                : MightyGUIUtilities.WARNING_HEIGHT;
        }

        public void DrawNotFlags(BaseMightyMember mightyMember, SerializedProperty property, NotFlagsAttribute attribute,
            GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"\"{property.displayName}\" type should be an enum");
                return;
            }

            if (!m_notFlagsCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, type, names) = m_notFlagsCache[mightyMember];

            if (attribute.AllowNothing)
            {
                if (property.intValue == -1) property.intValue = 0;

                try
                {
                    var enumObject = Enum.ToObject(type, property.intValue);

                    var index = attribute.Options.Contains(FieldOption.HideLabel)
                        ? EditorGUILayout.Popup(Convert.ToUInt32(enumObject).GetBitIndex(true), names)
                        : EditorGUILayout.Popup(label ?? EditorGUIUtility.TrTextContent(property.displayName),
                            Convert.ToUInt32(enumObject).GetBitIndex(true), names);

                    property.intValue = index == 0 ? 0 : (index - 1).ToBitMask();
                }
                catch
                {
                    property.intValue = 0;
                }
            }
            else
            {
                if (property.intValue == -1 || property.intValue == 0) property.intValue = 1;

                var enumObject = (Enum) Enum.ToObject(type, property.intValue);

                var enumValue = attribute.Options.Contains(FieldOption.HideLabel)
                    ? EditorGUILayout.EnumPopup(enumObject)
                    : EditorGUILayout.EnumPopup(label ?? EditorGUIUtility.TrTextContent(property.displayName), enumObject);

                property.intValue = Convert.ToInt32(enumValue);
            }

            if (!valid)
                MightyGUIUtilities.DrawHelpBox($"Enum \"{property.displayName}\" is not marked by [Flags] baseAttribute");
        }

        public void DrawNotFlags(Rect position, BaseMightyMember mightyMember, SerializedProperty property, NotFlagsAttribute attribute)
        {
            if (property.propertyType != SerializedPropertyType.Enum)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"\"{property.displayName}\" type should be an enum");
                return;
            }

            if (!m_notFlagsCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, type, names) = m_notFlagsCache[mightyMember];

            if (attribute.AllowNothing)
            {
                if (property.intValue == -1) property.intValue = 0;

                try
                {
                    var enumObject = Enum.ToObject(type, property.intValue);

                    var index = attribute.Options.Contains(FieldOption.HideLabel)
                        ? EditorGUI.Popup(position, Convert.ToUInt32(enumObject).GetBitIndex(true), names)
                        : EditorGUI.Popup(position, property.displayName, Convert.ToUInt32(enumObject).GetBitIndex(true), names);

                    property.intValue = index == 0 ? 0 : (index - 1).ToBitMask();
                }
                catch
                {
                    property.intValue = 0;
                }
            }
            else
            {
                if (property.intValue == -1 || property.intValue == 0) property.intValue = 1;

                var enumObject = (Enum) Enum.ToObject(type, property.intValue);

                var enumValue = attribute.Options.Contains(FieldOption.HideLabel)
                    ? EditorGUI.EnumPopup(position, enumObject)
                    : EditorGUI.EnumPopup(position, property.displayName, enumObject);

                property.intValue = Convert.ToInt32(enumValue);
            }

            if (!valid)
                MightyGUIUtilities.DrawHelpBox(position, $"Enum \"{property.displayName}\" is not marked by [Flags] baseAttribute");
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, NotFlagsAttribute attribute)
        {
            var type = serializedField.PropertyType;

            List<string> optionsList = null;
            if (attribute.AllowNothing)
            {
                optionsList = new List<string>(Enum.GetNames(type));
                optionsList.Insert(0, "Nothing");
            }

            var valid = serializedField.Property.GetPropertyType() == SerializedPropertyType.Enum &&
                        type.GetCustomAttributes(typeof(FlagsAttribute), true).Length != 0;

            m_notFlagsCache[serializedField] = (valid, type, optionsList?.ToArray());
        }

        protected override void ClearCache() => m_notFlagsCache.ClearCache();
    }
}
#endif