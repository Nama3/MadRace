#if UNITY_EDITOR
using MightyAttributes.Utilities;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class MinMaxDrawer : BaseMultiFieldDrawer<MinMaxAttribute>
    {
        private static readonly float[] MinMaxWidths = {25, 27};

        private static readonly GUIContent[] MinMaxContent =
        {
            EditorGUIUtility.TrTextContent("Min"),
            EditorGUIUtility.TrTextContent("Max")
        };

        protected override int Columns => 2;
        protected override Orientation Orientation => Orientation.Horizontal;

        protected override float[] GetLabelWidths() => MinMaxWidths;
        protected override GUIContent[] GetLabelContents() => MinMaxContent;

        protected override bool IsPropertyTypeValid(SerializedPropertyType propertyType) =>
            propertyType == SerializedPropertyType.Vector2 || propertyType == SerializedPropertyType.Vector2Int;

        protected override string TypeName => "UnityEngine.Vector2 or UnityEngine.Vector2Int";

        protected override float[] GetValues(SerializedProperty property) =>
            property.propertyType == SerializedPropertyType.Vector2Int
                ? VectorUtilities.Vector2IntToFloatArray(property.vector2IntValue)
                : VectorUtilities.Vector2ToArray(property.vector2Value);

        protected override void SetValues(SerializedProperty property, float[] values)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Vector2Int:
                    property.vector2IntValue = VectorUtilities.FloatArrayToVector2Int(values);
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = VectorUtilities.ArrayToVector2(values);
                    break;
            }
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, MinMaxAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif