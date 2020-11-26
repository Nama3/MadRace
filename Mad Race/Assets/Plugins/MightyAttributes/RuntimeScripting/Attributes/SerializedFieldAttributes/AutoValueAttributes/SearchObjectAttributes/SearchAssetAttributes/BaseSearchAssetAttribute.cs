namespace MightyAttributes
{
    public abstract class BaseSearchAssetAttribute : BaseSearchObjectAttribute
    {
        public string Name { get; }

        protected BaseSearchAssetAttribute(bool executeInPlayMode) : base(executeInPlayMode)
        {
        }

        protected BaseSearchAssetAttribute(string name, bool executeInPlayMode) : base(executeInPlayMode) => Name = name;
    }
}