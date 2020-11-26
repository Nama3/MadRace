namespace MightyAttributes
{
    public class GetComponentInParentWithLayerAttribute : BaseWithLayerAttribute
    {
        /// <summary>
        /// Finds the first component in the parents of the current object that have the same type than the field.
        /// </summary>
        /// <param name="layer">The layer of the Game Object to look into.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentInParentWithLayerAttribute(string layer, bool executeInPlayMode = false)
            : base(layer, false, false, executeInPlayMode)
        {
        }
    }
}