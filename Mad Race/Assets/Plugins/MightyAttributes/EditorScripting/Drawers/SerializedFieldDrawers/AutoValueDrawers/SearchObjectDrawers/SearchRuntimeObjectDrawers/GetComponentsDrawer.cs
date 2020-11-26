#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class GetComponentsDrawer : BaseSearchObjectsDrawer<Component, GetComponentsAttribute>
    {
        protected override Component[] GetFoundArray(MightySerializedField mightyMember, GetComponentsAttribute baseAttribute) => 
            mightyMember.Property.GetGameObject().GetComponents(mightyMember.PropertyType);

    }
}
#endif