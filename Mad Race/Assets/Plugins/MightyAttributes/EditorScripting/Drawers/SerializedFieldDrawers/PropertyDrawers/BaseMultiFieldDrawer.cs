#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public abstract class BaseMultiFieldDrawer<T> : BasePropertyDrawer<T>, IArrayElementDrawer where T : BasePropertyDrawerAttribute
    {
        protected abstract int Columns { get; }
        protected abstract Orientation Orientation { get; }

        protected abstract float[] GetLabelWidths();
        protected abstract GUIContent[] GetLabelContents();

        protected abstract bool IsPropertyTypeValid(SerializedPropertyType propertyType);
        protected abstract string TypeName { get; }

        protected abstract float[] GetValues(SerializedProperty property);
        protected abstract void SetValues(SerializedProperty property, float[] values);

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, T attribute)
        {
            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index => DrawElement(mightyMember, index, attribute));
                return;
            }

            DrawField(property, attribute.Options);
        }

        public void DrawElement(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute baseAttribute) =>
            DrawField(serializedField.GetElement(index), baseAttribute.Options);

        public void DrawElement(GUIContent label, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) => DrawField(serializedField.GetElement(index), baseAttribute.Options, label);

        public void DrawElement(Rect position, MightySerializedField serializedField, int index,
            BasePropertyDrawerAttribute baseAttribute) => DrawField(position, serializedField.GetElement(index), baseAttribute.Options);

        public float GetElementHeight(MightySerializedField serializedField, int index, BasePropertyDrawerAttribute attribute) =>
            IsPropertyTypeValid(serializedField.GetElement(index).propertyType)
                ? GetFieldHeight(attribute.Options)
                : MightyGUIUtilities.WARNING_HEIGHT;

        private void DrawField(SerializedProperty property, FieldOption options, GUIContent label = null)
        {
            if (!IsPropertyTypeValid(property.propertyType))
            {
                MightyGUIUtilities.DrawPropertyField(property, label);
                MightyGUIUtilities.DrawHelpBox($"{property.name} should be of type {TypeName}");
                return;
            }

            EditorGUI.BeginChangeCheck();

            var values = DrawMultiField(EditorGUILayout.GetControlRect(true, GetFieldHeight(options)),
                label ?? EditorGUIUtility.TrTextContent(property.displayName), GetValues(property), options);

            if (EditorGUI.EndChangeCheck())
                SetValues(property, values);
        }

        private void DrawField(Rect position, SerializedProperty property, FieldOption options)
        {
            if (!IsPropertyTypeValid(property.propertyType))
            {
                position = MightyGUIUtilities.DrawPropertyField(position, property);
                MightyGUIUtilities.DrawHelpBox(position, $"{property.name} should be of type {TypeName}");
                return;
            }

            EditorGUI.BeginChangeCheck();

            var values = DrawMultiField(position, EditorGUIUtility.TrTextContent(property.displayName), GetValues(property), options);

            if (EditorGUI.EndChangeCheck())
                SetValues(property, values);
        }

        private float[] DrawMultiField(Rect position, GUIContent label, float[] values, FieldOption options)
        {
            if (!DrawLabel(ref position, null, options, label))
                position.width -= MightyGUIUtilities.FIELD_SPACING * (Columns - 1);

            MightyGUIUtilities.MultiFloatField(position, GetLabelContents(), values, GetLabelWidths(), Orientation);

            return values;
        }

        private float GetFieldHeight(FieldOption options)
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    return options.Contains(FieldOption.HideLabel) || EditorGUIUtility.wideMode
                        ? MightyGUIUtilities.FIELD_HEIGHT
                        : MightyGUIUtilities.FIELD_HEIGHT * 2;
                case Orientation.Vertical:
                    return options.Contains(FieldOption.HideLabel) || EditorGUIUtility.wideMode
                        ? MightyGUIUtilities.VERTICAL_FIELD_HEIGHT
                        : MightyGUIUtilities.FIELD_HEIGHT + MightyGUIUtilities.VERTICAL_FIELD_HEIGHT;
            }

            return 0;
        }

        protected override Rect DrawNormalLabel(Rect position, SerializedProperty property, GUIContent label) =>
            MightyGUIUtilities.MultiFieldPrefixLabel(position, label ?? EditorGUIUtility.TrTextContent(property.displayName), Columns);

        protected override Rect DrawBoldLabel(Rect position, SerializedProperty property, GUIContent label) =>
            MightyGUIUtilities.MultiFieldPrefixLabel(position, label ?? EditorGUIUtility.TrTextContent(property.displayName), Columns,
                EditorStyles.boldLabel);
    }
}
#endif