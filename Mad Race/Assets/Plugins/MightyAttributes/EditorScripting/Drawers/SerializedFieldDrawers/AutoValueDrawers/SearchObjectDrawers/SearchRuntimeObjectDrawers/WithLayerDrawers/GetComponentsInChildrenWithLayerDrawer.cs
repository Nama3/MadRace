#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentsInChildrenWithLayerDrawer : BaseSearchObjectsDrawer<Component, GetComponentsInChildrenWithLayerAttribute>
    {
        protected override Component[] GetFoundArray(MightySerializedField mightyMember,
            GetComponentsInChildrenWithLayerAttribute attribute) =>
            mightyMember.Property.GetComponentsInChildrenWithLayer(attribute.Layer, attribute.IncludeInactive, attribute.IgnoreSelf);
    }
}
#endif