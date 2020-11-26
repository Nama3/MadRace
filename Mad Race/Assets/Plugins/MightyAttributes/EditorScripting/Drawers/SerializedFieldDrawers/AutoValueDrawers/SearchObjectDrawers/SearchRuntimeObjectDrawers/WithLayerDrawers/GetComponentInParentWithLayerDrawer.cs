#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentInParentWithLayerDrawer : BaseSearchObjectDrawer<Component, GetComponentInParentWithLayerAttribute>
    {
        protected override Component GetObject(MightySerializedField mightyMember, GetComponentInParentWithLayerAttribute attribute) => 
            mightyMember.Property.GetComponentInParentWithLayer(attribute.Layer);
    }
}
#endif