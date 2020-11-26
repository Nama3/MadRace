#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface INonSerializedFieldDrawer : IMightyDrawer
    {
        void DrawField(MightyNonSerializedField nonSerializedField, BaseNonSerializedFieldAttribute baseAttribute);
    }

    public abstract class BaseNonSerializedFieldDrawer<T> : BaseMightyDrawer<T>, INonSerializedFieldDrawer
        where T : BaseNonSerializedFieldAttribute
    {
        public void DrawField(MightyNonSerializedField nonSerializedField, BaseNonSerializedFieldAttribute baseAttribute) =>
            DrawField(nonSerializedField, (T) baseAttribute);

        protected abstract void DrawField(MightyNonSerializedField nonSerializedField, T baseAttribute);
    }
}
#endif