namespace MightyAttributes
{
    public abstract class BaseWithNameAttribute : BaseSearchRuntimeObjectAttribute
    {
        public readonly string Name;

        protected BaseWithNameAttribute(bool includeInactive, bool ignoreSelf, bool executeInPlayMode) : base(includeInactive, ignoreSelf,
            executeInPlayMode)
        {
        }

        protected BaseWithNameAttribute(string name, bool includeInactive, bool ignoreSelf, bool executeInPlayMode) : base(
            includeInactive, ignoreSelf, executeInPlayMode) => Name = name;
    }
}