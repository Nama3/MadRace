#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindObjectWithLayerDrawer : BaseSearchObjectDrawer<Object, FindObjectWithLayerAttribute>
    {
        protected override Object GetObject(MightySerializedField mightyMember, FindObjectWithLayerAttribute attribute) => 
            mightyMember.Property.FindObjectWithLayer(attribute.Layer, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif