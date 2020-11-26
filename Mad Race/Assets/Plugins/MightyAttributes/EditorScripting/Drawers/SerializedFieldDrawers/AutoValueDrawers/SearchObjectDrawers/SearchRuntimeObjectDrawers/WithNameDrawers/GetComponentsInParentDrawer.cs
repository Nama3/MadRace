#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentsInParentDrawer : BaseSearchObjectsDrawer<Component, GetComponentsInParentAttribute>
    {
        protected override Component[] GetFoundArray(MightySerializedField mightyMember, GetComponentsInParentAttribute attribute) => 
            mightyMember.Property.GetComponentsInParentWithName(attribute.Name);
    }
}
#endif