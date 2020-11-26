namespace MightyAttributes
{
    public class GetComponentsInParentWithLayerAttribute : BaseWithLayerAttribute
    {
        /// <summary>
        /// Finds all the components in the parents of the current object that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="layer">The layer of the Game Objects to look into.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentsInParentWithLayerAttribute(string layer, bool executeInPlayMode = false) : base(layer, false, false,
            executeInPlayMode)
        {
        }
    }
}