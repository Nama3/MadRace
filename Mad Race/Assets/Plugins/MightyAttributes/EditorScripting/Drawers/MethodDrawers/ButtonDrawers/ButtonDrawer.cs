#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class ButtonDrawer : BaseButtonDrawer<ButtonAttribute>
    {
        protected override bool DrawButton(ButtonAttribute attribute, string label, string methodName) =>
            MightyGUIUtilities.DrawButton(label, attribute.Height);
    }
}
#endif