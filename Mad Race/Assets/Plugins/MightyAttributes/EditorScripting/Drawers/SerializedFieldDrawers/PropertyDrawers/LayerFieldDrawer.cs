#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class LayerFieldDrawer : BasePropertyDrawer<LayerFieldAttribute>, IArrayElementDrawer
    {
        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, LayerFieldAttribute attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawLayerField(property, attribute);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawLayerField(serializedField.GetElement(index), (LayerFieldAttribute) baseAttribute);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) =>
            DrawLayerField(serializedField.GetElement(index), (LayerFieldAttribute) baseAttribute, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute)
            => DrawLayerField(position, serializedField.GetElement(index), (LayerFieldAttribute) baseAttribute);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            serializedField.GetElement(index).propertyType != SerializedPropertyType.Integer
                ? MightyGUIUtilities.WARNING_HEIGHT
                : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.FIELD_SPACING;

        public void DrawLayerField(SerializedProperty property, LayerFieldAttribute attribute, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"\"{property.displayName}\" should be of type int");
                return;
            }

            int layer;
            if (attribute.Options.Contains(FieldOption.HideLabel))
                layer = EditorGUILayout.LayerField(property.intValue);
            else if (attribute.Options.Contains(FieldOption.BoldLabel))
                layer = EditorGUILayout.LayerField(label ?? EditorGUIUtility.TrTextContent(property.displayName), property.intValue,
                    EditorStyles.boldLabel);
            else
                layer = EditorGUILayout.LayerField(label ?? EditorGUIUtility.TrTextContent(property.displayName), property.intValue);

            property.intValue = layer;
        }

        public void DrawLayerField(Rect position, SerializedProperty property, LayerFieldAttribute attribute, GUIContent label = null)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"\"{property.displayName}\" should be of type int");
                return;
            }

            int layer;
            if (attribute.Options.Contains(FieldOption.HideLabel))
                layer = EditorGUI.LayerField(position, property.intValue);
            else if (attribute.Options.Contains(FieldOption.BoldLabel))
                layer = EditorGUI.LayerField(position, label ?? EditorGUIUtility.TrTextContent(property.displayName), property.intValue,
                    EditorStyles.boldLabel);
            else
                layer = EditorGUI.LayerField(position, label ?? EditorGUIUtility.TrTextContent(property.displayName), property.intValue);

            property.intValue = layer;
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, LayerFieldAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif