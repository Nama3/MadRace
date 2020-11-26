#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MightyAttributes.Editor
{
    public class SceneDropdownDrawer : BasePropertyDrawer<SceneDropdownAttribute>, IArrayElementDrawer
    {
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, SceneDropdownAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawDropdown(property, attribute.Options);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawDropdown(serializedField.GetElement(index), baseAttribute.Options);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index, 
            BasePropertyDrawerAttribute baseAttribute) => DrawDropdown(serializedField.GetElement(index), baseAttribute.Options, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) 
            => DrawDropdown(position, serializedField.GetElement(index), baseAttribute.Options);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var propertyType = serializedField.GetElement(index).propertyType;
            return propertyType != SerializedPropertyType.Integer
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;
        }

        private static void DrawDropdown(SerializedProperty property, FieldOption option, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"{nameof(SceneDropdownAttribute)} can be used only on int fields");
                return;
            }

            GetDisplayOptions(out var displayOptions, out var values);

            var value = MightyGUIUtilities.DrawDropdown(label ?? EditorGUIUtility.TrTextContent(property.displayName), property.intValue,
                values, displayOptions, option);
            if (value != null)
                property.intValue = (int) value;
        }

        private static void DrawDropdown(Rect position, SerializedProperty property, FieldOption option)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"{nameof(SceneDropdownAttribute)} can be used only on int fields");
                return;
            }

            GetDisplayOptions(out var displayOptions, out var values);

            var value = MightyGUIUtilities.DrawDropdown(position, property.displayName, property.intValue,
                values, displayOptions, option);

            if (value != null)
                property.intValue = (int) value;
        }

        private static void GetDisplayOptions(out string[] displayOptions, out object[] values)
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            values = new object[sceneCount];
            displayOptions = new string[sceneCount];

            for (var i = 0; i < sceneCount; i++)
            {
                values[i] = i;
                displayOptions[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, SceneDropdownAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif