#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentsInChildrenWithTagDrawer : BaseSearchObjectsDrawer<Component, GetComponentsInChildrenWithTagAttribute>
    {
        protected override Component[] GetFoundArray(MightySerializedField mightyMember, GetComponentsInChildrenWithTagAttribute attribute)
            => mightyMember.Property.GetComponentsInChildrenWithTag(attribute.Tag, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif