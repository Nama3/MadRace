namespace MightyAttributes
{
    public abstract class BaseWithLayerAttribute : BaseSearchRuntimeObjectAttribute
    {
        public string Layer { get; }

        protected BaseWithLayerAttribute(string layer, bool includeInactive, bool ignoreSelf, bool executeInPlayMode) 
            : base(includeInactive, ignoreSelf, executeInPlayMode) => Layer = layer;
    }
}