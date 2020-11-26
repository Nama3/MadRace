#if UNITY_EDITOR
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class AreaDrawer : BaseMultiFieldDrawer<AreaAttribute>
    {
        private static readonly float[] AreaWidths = {35, 40, 45, 27};

        private static readonly GUIContent[] AreaContent =
        {
            EditorGUIUtility.TrTextContent("Left"),
            EditorGUIUtility.TrTextContent("Right"),
            EditorGUIUtility.TrTextContent("Bottom"),
            EditorGUIUtility.TrTextContent("Top")
        };

        protected override int Columns => 4;
        protected override Orientation Orientation => Orientation.Vertical;

        protected override float[] GetLabelWidths() => AreaWidths;
        protected override GUIContent[] GetLabelContents() => AreaContent;

        protected override bool IsPropertyTypeValid(SerializedPropertyType propertyType) => propertyType == SerializedPropertyType.Vector4;
        protected override string TypeName => "UnityEngine.Vector4";

        protected override float[] GetValues(SerializedProperty property) => VectorUtilities.Vector4ToArray(property.vector4Value);

        protected override void SetValues(SerializedProperty property, float[] values) =>
            property.vector4Value = VectorUtilities.ArrayToVector4(values);

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, AreaAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif