#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class MaxValuePropertyValidator : BaseValidatorDrawer<MaxValueAttribute>
    {
        protected override void ValidateProperty(MightySerializedField serializedField, MaxValueAttribute attribute)
        {
            var property = serializedField.Property;

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    if (property.intValue > attribute.MaxValue) property.intValue = (int) attribute.MaxValue;
                    break;
                case SerializedPropertyType.Float:
                    if (property.floatValue > attribute.MaxValue) property.floatValue = attribute.MaxValue;
                    break;
                default:
                    MightyGUIUtilities.DrawHelpBox($"{nameof(MaxValueAttribute)} can be used only on int or float fields");
                    break;
            }
        }
        
        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, MaxValueAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif