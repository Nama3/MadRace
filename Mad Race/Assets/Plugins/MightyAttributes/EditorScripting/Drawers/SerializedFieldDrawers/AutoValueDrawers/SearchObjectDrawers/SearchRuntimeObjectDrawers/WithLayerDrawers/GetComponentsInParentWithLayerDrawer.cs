#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentsInParentWithLayerDrawer : BaseSearchObjectsDrawer<Component, GetComponentsInParentWithLayerAttribute>
    {
        protected override Component[] GetFoundArray(MightySerializedField mightyMember, GetComponentsInParentWithLayerAttribute attribute) => 
            mightyMember.Property.GetComponentsInParentWithLayer(attribute.Layer);

    }
}
#endif