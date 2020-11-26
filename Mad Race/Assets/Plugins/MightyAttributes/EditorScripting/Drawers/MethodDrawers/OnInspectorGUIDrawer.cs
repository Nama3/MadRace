#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class OnInspectorGUIDrawer : BaseMethodDrawer<OnInspectorGUIAttribute>
    {
        protected override void OnEnable(MightyMethod mightyMethod, OnInspectorGUIAttribute attribute)
        {
        }

        protected override void OnModifiedProperties(bool modified, MightyMethod mightyMethod, OnInspectorGUIAttribute attribute)
        {
        }

        protected override void OnInspectorGUI(bool canDraw, MightyMethod mightyMethod, OnInspectorGUIAttribute attribute) => 
            InvokeMethod(mightyMethod, attribute);
    }
}
#endif