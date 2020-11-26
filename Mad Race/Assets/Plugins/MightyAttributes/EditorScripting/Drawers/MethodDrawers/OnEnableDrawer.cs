#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class OnEnableDrawer : BaseMethodDrawer<OnEnableAttribute>
    {
        protected override void OnEnable(MightyMethod mightyMethod, OnEnableAttribute attribute) => InvokeMethod(mightyMethod, attribute);

        protected override void OnModifiedProperties(bool modified, MightyMethod mightyMethod, OnEnableAttribute attribute)
        {
        }

        protected override void OnInspectorGUI(bool canDraw, MightyMethod mightyMethod, OnEnableAttribute attribute)
        {
        }
    }
}
#endif