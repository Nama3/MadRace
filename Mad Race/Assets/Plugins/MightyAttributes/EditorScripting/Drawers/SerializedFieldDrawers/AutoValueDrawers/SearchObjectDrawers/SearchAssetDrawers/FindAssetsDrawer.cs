#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindAssetsDrawer : BaseSearchObjectsDrawer<Object, FindAssetsAttribute>
    {
        protected override Object[] GetFoundArray(MightySerializedField mightyMember, FindAssetsAttribute attribute) =>
            mightyMember.Property.FindAssetsWithName(attribute.Name);
    }
}
#endif