namespace MightyAttributes
{
    public class FindAssetsAttribute : BaseSearchAssetAttribute
    {
        /// <summary>
        /// Finds all the objects in the asset database that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindAssetsAttribute(bool executeInPlayMode = false) : base(executeInPlayMode)
        {
        }

        /// <summary>
        /// Finds all the objects in the asset database that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="name">The name of the assets to look for.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindAssetsAttribute(string name, bool executeInPlayMode = false) : base(name, executeInPlayMode)
        {
        }
    }
}