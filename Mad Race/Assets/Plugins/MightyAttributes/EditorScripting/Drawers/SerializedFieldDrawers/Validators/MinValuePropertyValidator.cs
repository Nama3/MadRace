#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class MinValuePropertyValidator : BaseValidatorDrawer<MinValueAttribute>
    {
        protected override void ValidateProperty(MightySerializedField serializedField, MinValueAttribute attribute)
        {
            var property = serializedField.Property;

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    if (property.intValue < attribute.MinValue) property.intValue = (int) attribute.MinValue;
                    break;
                case SerializedPropertyType.Float:
                    if (property.floatValue < attribute.MinValue) property.floatValue = attribute.MinValue;
                    break;
                default:
                    MightyGUIUtilities.DrawHelpBox($"{nameof(MinValueAttribute)} can be used only on int or float fields");
                    break;
            }
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, MinValueAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif