#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class LayerNameDrawer : BaseAutoValueDrawer<LayerNameAttribute>
    {
        protected override InitState InitPropertyImpl(MightySerializedField serializedField, LayerNameAttribute attribute)
        {
            var property = serializedField.Property;
            var attributeTarget = property.GetAttributeTarget<LayerNameAttribute>();

            if (property.propertyType != SerializedPropertyType.Integer)
                return new InitState(false, "\"" + property.displayName + "\" should be of type int");
            
            var name = attribute.LayerName;
            if (attribute.NameAsCallback && serializedField.GetValueFromMember(attributeTarget, name, out string nameValue))
                name = nameValue;

            property.intValue = LayerMask.NameToLayer(name);;
            return new InitState(true);
        }
    }
}
#endif