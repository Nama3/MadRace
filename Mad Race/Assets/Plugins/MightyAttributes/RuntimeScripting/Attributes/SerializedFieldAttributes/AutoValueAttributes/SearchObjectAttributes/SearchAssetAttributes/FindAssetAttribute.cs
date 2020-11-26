namespace MightyAttributes
{
    public class FindAssetAttribute : BaseSearchAssetAttribute
    {
        /// <summary>
        /// Finds the first object in the asset database that have the same type than the field.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindAssetAttribute(bool executeInPlayMode = false) : base(executeInPlayMode)
        {
        }

        /// <summary>
        /// Finds the first object in the asset database that have the same type than the field.
        /// </summary>
        /// <param name="name">The name of the asset to look for.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindAssetAttribute(string name, bool executeInPlayMode = false) : base(name, executeInPlayMode)
        {
        }
    }
}