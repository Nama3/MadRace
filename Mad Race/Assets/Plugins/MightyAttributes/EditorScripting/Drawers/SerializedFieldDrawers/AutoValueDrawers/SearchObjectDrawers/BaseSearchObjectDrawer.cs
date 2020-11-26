#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface ISearchObjectDrawer : IAutoValueDrawer
    {
    }

    public abstract class BaseSearchObjectDrawer<To, Ta> : BaseAutoValueDrawer<Ta>, ISearchObjectDrawer
        where Ta : BaseSearchObjectAttribute where To : Object
    {
        protected override InitState InitPropertyImpl(MightySerializedField serializedField, Ta attribute)
        {
            var property = serializedField.Property;
            if (property.IsCollection())
                return new InitState(false, $"\"{property.displayName}\" should not be an array");

            if (property.propertyType != SerializedPropertyType.ObjectReference || !serializedField.PropertyType.IsSubclassOf(typeof(To)))
                return new InitState(false, $"\"{property.displayName}\" should inherit from {typeof(To).FullName}");

            property.objectReferenceValue = GetObject(serializedField, attribute);
            return new InitState(true);
        }

        protected abstract To GetObject(MightySerializedField serializedField, Ta attribute);
    }
}
#endif