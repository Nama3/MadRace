#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class ConfirmButtonDrawer : BaseButtonDrawer<ConfirmButtonAttribute>
    {
        protected override bool DrawButton(ConfirmButtonAttribute attribute, string label, string methodName)
        {
            if (!MightyGUIUtilities.DrawButton(label, attribute.Height)) return false;

            if (MightyGUIUtilities.ConfirmDialog(attribute.DialogTitle ?? label,
                attribute.ConfirmMessage ?? $"Do you want to run {methodName}?"))
                return true;

            throw MightyExceptionUtilities.AbortAfterConfirmDialog;
        }

        protected override void OnFunctionHasBeenCalled() => throw MightyExceptionUtilities.AbortAfterConfirmDialog;
    }
}
#endif