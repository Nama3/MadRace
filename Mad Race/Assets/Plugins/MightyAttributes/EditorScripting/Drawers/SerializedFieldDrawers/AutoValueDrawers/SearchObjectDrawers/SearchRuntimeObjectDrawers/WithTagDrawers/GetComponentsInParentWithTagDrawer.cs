#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentsInParentWithTagDrawer : BaseSearchObjectsDrawer<Component, GetComponentsInParentWithTagAttribute>
    {
        protected override Component[] GetFoundArray(MightySerializedField mightyMember, GetComponentsInParentWithTagAttribute attribute) => 
            mightyMember.Property.GetComponentsInParentWithTag(attribute.Tag);
    }
}
#endif