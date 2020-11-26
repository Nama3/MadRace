#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentsInChildrenDrawer : BaseSearchObjectsDrawer<Component, GetComponentsInChildrenAttribute>
    {
        protected override Component[] GetFoundArray(MightySerializedField mightyMember, GetComponentsInChildrenAttribute attribute) => 
            mightyMember.Property.GetComponentsInChildrenWithName(attribute.Name, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif