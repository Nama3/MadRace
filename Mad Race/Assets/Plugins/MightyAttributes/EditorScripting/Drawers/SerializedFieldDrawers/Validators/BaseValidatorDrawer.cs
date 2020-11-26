#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IValidatorDrawer : ISerializedFieldDrawer
    {
        void ValidateProperty(MightySerializedField serializedField, BaseValidatorAttribute baseAttribute);
    }

    public abstract class BaseValidatorDrawer<T> : BaseSerializeFieldDrawer<T>, IValidatorDrawer where T : BaseValidatorAttribute
    {
        public void ValidateProperty(MightySerializedField serializedField, BaseValidatorAttribute baseAttribute) =>
            ValidateProperty(serializedField, (T) baseAttribute);

        protected abstract void ValidateProperty(MightySerializedField serializedField, T attribute);
    }
}
#endif