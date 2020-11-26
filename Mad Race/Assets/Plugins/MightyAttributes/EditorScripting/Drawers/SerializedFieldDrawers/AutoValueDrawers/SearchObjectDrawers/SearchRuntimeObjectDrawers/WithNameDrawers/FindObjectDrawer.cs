#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindObjectDrawer : BaseSearchObjectDrawer<Object, FindObjectAttribute>
    {
        protected override Object GetObject(MightySerializedField mightyMember, FindObjectAttribute attribute) => 
            mightyMember.Property.FindObjectWithName(attribute.Name, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif