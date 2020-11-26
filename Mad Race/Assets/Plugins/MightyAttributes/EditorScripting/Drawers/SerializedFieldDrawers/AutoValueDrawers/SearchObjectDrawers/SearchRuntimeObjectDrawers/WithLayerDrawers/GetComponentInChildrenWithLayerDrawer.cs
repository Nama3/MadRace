#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentInChildrenWithLayerDrawer : BaseSearchObjectDrawer<Component, GetComponentInChildrenWithLayerAttribute>
    {
        protected override Component GetObject(MightySerializedField mightyMember, GetComponentInChildrenWithLayerAttribute attribute) => 
            mightyMember.Property.GetComponentInChildrenWithLayer(attribute.Layer, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif