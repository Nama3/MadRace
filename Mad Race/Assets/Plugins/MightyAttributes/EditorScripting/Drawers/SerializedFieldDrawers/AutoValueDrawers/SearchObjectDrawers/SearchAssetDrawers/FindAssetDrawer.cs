#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindAssetDrawer : BaseSearchObjectDrawer<Object, FindAssetAttribute>
    {
        protected override Object GetObject(MightySerializedField mightyMember, FindAssetAttribute attribute) => 
            mightyMember.Property.FindAssetWithName(attribute.Name);
    }
}
#endif