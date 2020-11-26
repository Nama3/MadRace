namespace MightyAttributes
{
    public class FindObjectsWithTagAttribute : BaseWithTagAttribute
    {
        /// <summary>
        /// Finds all the objects in the scene that has the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="tag">The tag of the Game Objects to look into.</param>
        /// <param name="includeInactive">Choose whether or not inactive objects should be included in the research (default: true).</param>
        /// <param name="ignoreSelf">Choose whether or not the object where the script is attached should be ignore in the research (default: false).</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindObjectsWithTagAttribute(string tag, bool includeInactive = true, bool ignoreSelf = false,
            bool executeInPlayMode = false) : base(tag, includeInactive, ignoreSelf, executeInPlayMode)
        {
        }
    }
}