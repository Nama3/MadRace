namespace MightyAttributes
{
    public class GetComponentInChildrenWithTagAttribute : BaseWithTagAttribute
    {
        /// <summary>
        /// Finds the first component in the current object or its children that have the same type than the field.
        /// </summary>
        /// <param name="tag">The tag of the Game Object to look into.</param>
        /// <param name="includeInactive">Choose whether or not inactive objects should be included in the research (default: true).</param>
        /// <param name="ignoreSelf">Choose whether or not the object where the script is attached should be ignore in the research (default: false).</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentInChildrenWithTagAttribute(string tag, bool includeInactive = false, bool ignoreSelf = false,
            bool executeInPlayMode = false) : base(tag, includeInactive, ignoreSelf, executeInPlayMode)
        {
        }
    }
}