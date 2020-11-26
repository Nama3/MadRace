namespace MightyAttributes
{
    public class LayerNameAttribute : BaseAutoValueAttribute
    {
        public string LayerName { get; }
        public bool NameAsCallback { get; }

        /// <summary>
        /// Initialize an int field with the value of the layer int value of the specified layer name.
        /// </summary>
        /// <param name="layerName">The name of the layer you want to convert as a LayerMask.</param>
        /// <param name="nameAsCallback">Choose whether or not layerName should be considered as a Callback of type string (default: false).</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public LayerNameAttribute(string layerName, bool nameAsCallback = false, bool executeInPlayMode = false)
            : base(executeInPlayMode)
        {
            LayerName = layerName;
            NameAsCallback = nameAsCallback;
        }
    }
}