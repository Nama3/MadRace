namespace MightyAttributes
{
    public abstract class BaseExplorerAttribute : BasePropertyDrawerAttribute
    {
        public string DefaultPath { get; }
        public bool PathAsCallback { get; }
        
        protected BaseExplorerAttribute(string defaultPath, bool pathAsCallback, FieldOption options) : base(options)
        {
            DefaultPath = defaultPath;
            PathAsCallback = pathAsCallback;
        }
    }
}