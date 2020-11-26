#if UNITY_EDITOR
using System;

namespace MightyAttributes.Editor
{
    public class HideStatusDrawer : BaseClassDrawer<HideStatusAttribute>
    {
        protected override void OnEnableClass(MightyMember<Type> mightyType, HideStatusAttribute attribute) => 
            mightyType.Context.Drawer.hideStatus |= attribute.HideStatus;

        protected override void OnDisableClass(MightyMember<Type> mightyType, HideStatusAttribute attribute)
        {
        }

        protected override void BeginDrawClass(MightyMember<Type> mightyType, HideStatusAttribute attribute) => 
            mightyType.Context.Drawer.hideStatus |= attribute.HideStatus;

        protected override void EndDrawClass(MightyMember<Type> mightyType, HideStatusAttribute attribute)
        {
        }

        protected override void Enable(BaseMightyMember mightyMember, HideStatusAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif