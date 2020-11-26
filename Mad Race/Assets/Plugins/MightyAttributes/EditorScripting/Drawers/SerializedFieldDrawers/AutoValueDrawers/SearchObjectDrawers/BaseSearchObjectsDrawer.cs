#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface ISearchObjectsDrawer : IAutoValueDrawer
    {
    }

    public abstract class BaseSearchObjectsDrawer<To, Ta> : BaseAutoValueDrawer<Ta>, ISearchObjectsDrawer
        where Ta : BaseSearchObjectAttribute where To : Object
    {
        protected override InitState InitPropertyImpl(MightySerializedField serializedField, Ta attribute)
        {
            var property = serializedField.Property;

            if (!property.IsCollection())
                return new InitState(false, $"\"{property.displayName}\" should be an array");

            if (!serializedField.PropertyType.IsSubclassOf(typeof(To)))
                return new InitState(false, $"\"{property.displayName}\" array type should inherit from {typeof(To).FullName}");

            var objects = GetFoundArray(serializedField, attribute);
            if (property.CompareArrays(objects)) return new InitState(true);

            property.ClearArray();
            for (var i = 0; i < objects.Length; i++)
            {
                property.InsertArrayElementAtIndex(i);
                property.GetArrayElementAtIndex(i).objectReferenceValue = objects[i];
            }

            return new InitState(true);
        }

        protected abstract To[] GetFoundArray(MightySerializedField serializedField, Ta attribute);
    }
}
#endif