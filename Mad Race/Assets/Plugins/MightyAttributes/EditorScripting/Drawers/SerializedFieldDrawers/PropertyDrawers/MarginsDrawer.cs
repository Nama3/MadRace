#if UNITY_EDITOR
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class MarginsDrawer : BaseMultiFieldDrawer<MarginsAttribute>
    {
        private static readonly float[] MarginsWidths = {35, 27, 40, 45};

        private static readonly GUIContent[] MarginsContent =
        {
            EditorGUIUtility.TrTextContent("Left"),
            EditorGUIUtility.TrTextContent("Top"),
            EditorGUIUtility.TrTextContent("Right"),
            EditorGUIUtility.TrTextContent("Bottom")
        };

        protected override int Columns => 4;
        protected override Orientation Orientation => Orientation.Vertical;

        protected override float[] GetLabelWidths() => MarginsWidths;
        protected override GUIContent[] GetLabelContents() => MarginsContent;

        protected override bool IsPropertyTypeValid(SerializedPropertyType propertyType) => propertyType == SerializedPropertyType.Vector4;
        protected override string TypeName => "UnityEngine.Vector4";

        protected override float[] GetValues(SerializedProperty property) => VectorUtilities.Vector4ToArray(property.vector4Value);

        protected override void SetValues(SerializedProperty property, float[] values) =>
            property.vector4Value = VectorUtilities.ArrayToVector4(values);

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, MarginsAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif