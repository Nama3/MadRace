namespace MightyAttributes
{
    public abstract class BaseSearchRuntimeObjectAttribute : BaseSearchObjectAttribute
    {
        public bool IncludeInactive { get; }

        public bool IgnoreSelf { get; }

        protected BaseSearchRuntimeObjectAttribute(bool includeInactive, bool ignoreSelf, bool executeInPlayMode) : base(executeInPlayMode)
        {
            IncludeInactive = includeInactive;
            IgnoreSelf = ignoreSelf;
        }
    }
}