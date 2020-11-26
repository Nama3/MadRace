#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface INativePropertyDrawer : IMightyDrawer
    {
        void DrawNativeProperty(MightyNativeProperty nativeProperty, BaseNativePropertyAttribute attribute);
    }

    public abstract class BaseNativePropertyDrawer<T> : BaseMightyDrawer<T>, INativePropertyDrawer where T : BaseNativePropertyAttribute
    {
        public void DrawNativeProperty(MightyNativeProperty nativeProperty, BaseNativePropertyAttribute attribute) =>
            DrawNativeProperty(nativeProperty, (T) attribute);

        protected abstract void DrawNativeProperty(MightyNativeProperty nativeProperty, T attribute);
    }
}
#endif