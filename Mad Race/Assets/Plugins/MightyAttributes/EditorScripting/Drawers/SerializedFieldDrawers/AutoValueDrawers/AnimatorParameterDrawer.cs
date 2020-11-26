#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class AnimatorParameterDrawer : BaseAutoValueDrawer<AnimatorParameterAttribute>
    {
        protected override InitState InitPropertyImpl(MightySerializedField serializedField, AnimatorParameterAttribute attribute)
        {
            var property = serializedField.Property;

            if (property.propertyType != SerializedPropertyType.Integer)
                return new InitState(false, "\"" + property.displayName + "\" should be of type int");

            var name = attribute.ParameterName;
            if (attribute.NameAsCallback && serializedField.GetValueFromMember(attribute.Target, name, out string nameValue))
                name = nameValue;

            property.intValue = Animator.StringToHash(name);
            return new InitState(true);
        }
    }
}
#endif