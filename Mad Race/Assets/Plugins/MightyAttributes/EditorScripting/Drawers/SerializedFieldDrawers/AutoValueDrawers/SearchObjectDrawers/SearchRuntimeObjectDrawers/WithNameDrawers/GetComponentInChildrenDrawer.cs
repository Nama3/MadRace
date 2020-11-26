#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentInChildrenDrawer : BaseSearchObjectDrawer<Component, GetComponentInChildrenAttribute>
    {
        protected override Component GetObject(MightySerializedField mightyMember, GetComponentInChildrenAttribute attribute) => 
            mightyMember.Property.GetComponentInChildrenWithName(attribute.Name, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif