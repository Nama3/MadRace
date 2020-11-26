#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class ArraySizeDrawer : BaseAutoValueDrawer<ArraySizeAttribute>
    {
        protected override InitState InitPropertyImpl(MightySerializedField serializedField, ArraySizeAttribute attribute)
        {
            var property = serializedField.Property;
            if (!property.IsCollection()) return new InitState(false, "\"" + property.displayName + "\" should be an array");

            if (!serializedField.GetValueFromMember(attribute.Target, attribute.SizeCallback, out int size))
                size = attribute.Size;
            
            if (size != property.arraySize)
            {
                property.arraySize = size;
                property.serializedObject.ApplyModifiedProperties();
            }
            return new InitState(true);
        }
    }
}
#endif