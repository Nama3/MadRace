#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentInParentDrawer : BaseSearchObjectDrawer<Component, GetComponentInParentAttribute>
    {
        protected override Component GetObject(MightySerializedField mightyMember, GetComponentInParentAttribute attribute) => 
            mightyMember.Property.GetComponentInParentWithName(attribute.Name);
    }
}
#endif