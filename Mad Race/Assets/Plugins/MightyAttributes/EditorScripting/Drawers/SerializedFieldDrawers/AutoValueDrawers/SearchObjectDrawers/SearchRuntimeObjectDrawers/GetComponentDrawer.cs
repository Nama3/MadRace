#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentDrawer : BaseSearchObjectDrawer<Component, GetComponentAttribute>
    {
        protected override Component GetObject(MightySerializedField serializedField, GetComponentAttribute attribute) =>
            serializedField.Property.GetGameObject().GetComponent(serializedField.PropertyType);
    }
}
#endif