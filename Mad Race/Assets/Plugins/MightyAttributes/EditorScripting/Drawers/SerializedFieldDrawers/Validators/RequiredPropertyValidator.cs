#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class RequiredPropertyValidator : BaseValidatorDrawer<RequiredAttribute>
    {
        protected override void ValidateProperty(MightySerializedField serializedField, RequiredAttribute attribute)
        {
            var property = serializedField.Property;

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null) return;
                
                var errorMessage = $"{property.name} is required";
                if (!string.IsNullOrEmpty(attribute.Message)) errorMessage = attribute.Message;

                MightyGUIUtilities.DrawHelpBox(errorMessage, MessageType.Error, property.GetTargetObject(), attribute.LogToConsole);
            }
            else
                MightyGUIUtilities.DrawHelpBox($"{nameof(RequiredAttribute)} works only on reference types");
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, RequiredAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif