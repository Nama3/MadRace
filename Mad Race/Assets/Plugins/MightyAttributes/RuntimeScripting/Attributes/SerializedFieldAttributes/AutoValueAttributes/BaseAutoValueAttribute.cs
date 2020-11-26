namespace MightyAttributes
{
    public abstract class BaseAutoValueAttribute : BaseSerializedFieldAttribute
    {
        public bool ExecuteInPlayMode { get; }

        protected BaseAutoValueAttribute(bool executeInPlayMode) => ExecuteInPlayMode = executeInPlayMode;
    }
}