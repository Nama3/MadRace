#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindObjectsDrawer : BaseSearchObjectsDrawer<Object, FindObjectsAttribute>
    {
        protected override Object[] GetFoundArray(MightySerializedField mightyMember, FindObjectsAttribute attribute) => 
            mightyMember.Property.FindObjectsWithName(attribute.Name, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif