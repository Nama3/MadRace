namespace MightyAttributes
{
    public abstract class BaseWithTagAttribute : BaseSearchRuntimeObjectAttribute
    {
        public string Tag { get; }

        protected BaseWithTagAttribute(string tag, bool includeInactive, bool ignoreSelf, bool executeInPlayMode)
            : base(includeInactive, ignoreSelf, executeInPlayMode) => Tag = tag;
    }
}