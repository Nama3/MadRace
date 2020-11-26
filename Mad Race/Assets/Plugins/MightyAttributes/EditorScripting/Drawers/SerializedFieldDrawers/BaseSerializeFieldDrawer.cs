#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface ISerializedFieldDrawer : IMightyDrawer
    {
    }

    public abstract class BaseSerializeFieldDrawer<T> : BaseMightyDrawer<T>, ISerializedFieldDrawer where T : BaseSerializedFieldAttribute
    {
        protected override void Enable(BaseMightyMember mightyMember, T attribute)
        {
            if (mightyMember is MightySerializedField serializedField)
                EnableSerializeFieldDrawer(serializedField, attribute);
        }

        protected abstract void EnableSerializeFieldDrawer(MightySerializedField serializedField, T attribute);
    }
}
#endif