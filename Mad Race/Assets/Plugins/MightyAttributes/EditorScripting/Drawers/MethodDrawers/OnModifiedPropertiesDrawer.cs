#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class OnModifiedPropertiesDrawer : BaseMethodDrawer<OnModifiedPropertiesAttribute>
    {
        protected override void OnEnable(MightyMethod mightyMethod, OnModifiedPropertiesAttribute attribute) => 
            InvokeMethod(mightyMethod, attribute);

        protected override void OnModifiedProperties(bool modified, MightyMethod mightyMethod, OnModifiedPropertiesAttribute attribute)
        {
            if (modified) InvokeMethod(mightyMethod, attribute);
        }

        protected override void OnInspectorGUI(bool canDraw, MightyMethod mightyMethod, OnModifiedPropertiesAttribute attribute)
        {
        }
    }
}
#endif