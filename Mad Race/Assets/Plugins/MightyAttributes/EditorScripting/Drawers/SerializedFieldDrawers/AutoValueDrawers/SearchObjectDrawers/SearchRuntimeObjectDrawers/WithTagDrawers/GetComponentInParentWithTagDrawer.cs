#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentInParentWithTagDrawer : BaseSearchObjectDrawer<Component, GetComponentInParentWithTagAttribute>
    {
        protected override Component GetObject(MightySerializedField mightyMember, GetComponentInParentWithTagAttribute attribute) => 
            mightyMember.Property.GetComponentInParentWithTag(attribute.Tag);
    }
}
#endif