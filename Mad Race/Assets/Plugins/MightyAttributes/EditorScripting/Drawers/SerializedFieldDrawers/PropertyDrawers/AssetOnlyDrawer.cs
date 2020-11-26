#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class AssetOnlyDrawer : BasePropertyDrawer<AssetOnlyAttribute>, IArrayElementDrawer
    {
        protected override void DrawProperty(MightySerializedField serializedField, SerializedProperty property,
            AssetOnlyAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(serializedField, index, attribute));
                return;
            }

            DrawAssetField(property, attribute);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawAssetField(serializedField.GetElement(index), (AssetOnlyAttribute) baseAttribute);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawAssetField(serializedField.GetElement(index), (AssetOnlyAttribute) baseAttribute, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawAssetField(position, serializedField.GetElement(index), (AssetOnlyAttribute) baseAttribute);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
        {
            var element = serializedField.GetElement(index);
            return element.propertyType != SerializedPropertyType.ObjectReference
                ? MightyGUIUtilities.WARNING_HEIGHT
                : !AssetPreview.GetAssetPreview(element.objectReferenceValue)
                    ? MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING
                    : 64;
        }

        private void DrawAssetField(SerializedProperty property, AssetOnlyAttribute attribute, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"{property.name} should be of type UnityEngine.Object");
                return;
            }

            property.objectReferenceValue = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUILayout.ObjectField(property.objectReferenceValue, property.GetSystemType(), false)
                : EditorGUILayout.ObjectField(label ?? EditorGUIUtility.TrTextContent(property.displayName),
                    property.objectReferenceValue, property.GetSystemType(), false);
        }

        private void DrawAssetField(Rect position, SerializedProperty property, AssetOnlyAttribute attribute)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"{property.name} should be of type UnityEngine.Object");
                return;
            }

            property.objectReferenceValue = attribute.Options.Contains(FieldOption.HideLabel)
                ? EditorGUI.ObjectField(position, property.objectReferenceValue, property.GetSystemType(), false)
                : EditorGUI.ObjectField(position, property.displayName, property.objectReferenceValue, property.GetSystemType(), false);
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, AssetOnlyAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif