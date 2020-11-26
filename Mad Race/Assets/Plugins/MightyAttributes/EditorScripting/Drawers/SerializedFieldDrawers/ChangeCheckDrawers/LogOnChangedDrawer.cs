#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class LogOnChangedDrawer : BaseChangeCheckDrawer<LogOnChangedAttribute>
    {
        protected override void BeginChangeCheck(MightySerializedField mightyMember, LogOnChangedAttribute attribute)
        {
        }

        protected override void EndChangeCheck(bool changed, MightySerializedField mightyMember, LogOnChangedAttribute attribute)
        {
            if (!changed) return;
            var property = mightyMember.Property;

            Debug.Log($@"{property.displayName}: {PropertyUtilities.GetGenericValue(attribute.Target, property)}",
                mightyMember.Context.Object);
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, LogOnChangedAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif