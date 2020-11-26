#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentInChildrenWithTagDrawer : BaseSearchObjectDrawer<Component, GetComponentInChildrenWithTagAttribute>
    {
        protected override Component GetObject(MightySerializedField mightyMember, GetComponentInChildrenWithTagAttribute attribute) =>
            mightyMember.Property.GetComponentInChildrenWithTag(attribute.Tag, attribute.IncludeInactive);
    }
}
#endif