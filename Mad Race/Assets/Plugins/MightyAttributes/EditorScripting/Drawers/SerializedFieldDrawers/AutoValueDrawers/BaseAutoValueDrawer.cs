#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public readonly struct InitState
    {
        public readonly bool isOk;
        public readonly string message;

        public InitState(bool isOk, string message = null)
        {
            this.isOk = isOk;
            this.message = message;
        }
    }

    public interface IAutoValueDrawer : ISerializedFieldDrawer
    {
        InitState InitProperty(MightySerializedField serializedField, BaseAutoValueAttribute baseAttribute);
    }

    public abstract class BaseAutoValueDrawer<T> : BaseSerializeFieldDrawer<T>, IAutoValueDrawer where T : BaseAutoValueAttribute
    {
        public InitState InitProperty(MightySerializedField serializedField, BaseAutoValueAttribute baseAttribute) =>
            baseAttribute.ExecuteInPlayMode ? InitPropertyImpl(serializedField, (T) baseAttribute) :
            !EditorApplication.isPlaying ? InitPropertyImpl(serializedField, (T) baseAttribute) : new InitState(true);

        protected abstract InitState InitPropertyImpl(MightySerializedField serializedField, T baseAttribute);

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, T attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif