#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindObjectsWithTagDrawer : BaseSearchObjectsDrawer<Object, FindObjectsWithTagAttribute>
    {
        protected override Object[] GetFoundArray(MightySerializedField mightyMember, FindObjectsWithTagAttribute attribute) => 
            mightyMember.Property.FindObjectsWithTag(attribute.Tag, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif