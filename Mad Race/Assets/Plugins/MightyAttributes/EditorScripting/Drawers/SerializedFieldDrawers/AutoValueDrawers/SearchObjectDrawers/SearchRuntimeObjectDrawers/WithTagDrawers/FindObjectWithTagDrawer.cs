#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindObjectWithTagDrawer : BaseSearchObjectDrawer<Object, FindObjectWithTagAttribute>
    {
        protected override Object GetObject(MightySerializedField mightyMember, FindObjectWithTagAttribute attribute) => 
            mightyMember.Property.FindObjectWithTag(attribute.Tag, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif