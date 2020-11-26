using System;
#if UNITY_EDITOR
using MightyAttributes.Editor;
#endif

namespace MightyAttributes
{
    public abstract class BaseMightyAttribute : Attribute
    {
#if UNITY_EDITOR
        public object Target { get; private set; }
        public IMightyDrawer Drawer { get; private set; }

        public void InitAttribute(object target)
        {
            Target = target;
            Drawer = MightyDrawersDatabase.GetDrawerForAttribute<IMightyDrawer>(GetType());
        }
#endif
    }
}