#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class IconButtonDrawer : BaseButtonDrawer<IconButtonAttribute>
    {
        protected override bool DrawButton(IconButtonAttribute attribute, string label, string methodName) =>
            MightyGUIUtilities.DrawButton(attribute.IconName, label, attribute.Height);
    }
}
#endif