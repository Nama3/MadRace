namespace MightyAttributes
{
    public class GetComponentsInChildrenWithLayerAttribute : BaseWithLayerAttribute
    {
        /// <summary>
        /// Finds all the components in the current object or its children that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="layer">The layer of the Game Objects to look into.</param>
        /// <param name="includeInactive">Choose whether or not inactive objects should be included in the research (default: true).</param>
        /// <param name="ignoreSelf">Choose whether or not the object where the script is attached should be ignore in the research (default: false).</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentsInChildrenWithLayerAttribute(string layer, bool includeInactive = false, bool ignoreSelf = false,
            bool executeInPlayMode = false) : base(layer, includeInactive, ignoreSelf, executeInPlayMode)
        {
        }
    }
}