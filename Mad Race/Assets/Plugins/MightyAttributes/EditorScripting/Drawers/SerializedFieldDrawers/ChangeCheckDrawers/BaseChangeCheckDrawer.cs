#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IChangeCheckDrawer : ISerializedFieldDrawer
    {
        void BeginChangeCheck(MightySerializedField serializedField, BaseChangeCheckAttribute baseAttribute);
        void EndChangeCheck(bool changed, MightySerializedField serializedField, BaseChangeCheckAttribute baseAttribute);
    }

    public abstract class BaseChangeCheckDrawer<T> : BaseSerializeFieldDrawer<T>, IChangeCheckDrawer where T : BaseChangeCheckAttribute
    {
        public void BeginChangeCheck(MightySerializedField serializedField, BaseChangeCheckAttribute baseAttribute) =>
            BeginChangeCheck(serializedField, (T) baseAttribute);

        public void EndChangeCheck(bool changed, MightySerializedField serializedField, BaseChangeCheckAttribute baseAttribute) =>
            EndChangeCheck(changed, serializedField, (T) baseAttribute);

        protected abstract void BeginChangeCheck(MightySerializedField serializedField, T attribute);
        protected abstract void EndChangeCheck(bool changed, MightySerializedField serializedField, T attribute);
    }
}
#endif