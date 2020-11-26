#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindObjectsWithLayerDrawer : BaseSearchObjectsDrawer<Object, FindObjectsWithLayerAttribute>
    {
        protected override Object[] GetFoundArray(MightySerializedField mightyMember, FindObjectsWithLayerAttribute attribute) => 
            mightyMember.Property.FindObjectsWithLayer(attribute.Layer, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif