namespace MightyAttributes
{
    public class FindObjectsAttribute : BaseWithNameAttribute
    {
        /// <summary>
        /// Finds all the objects in the scene that has the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="includeInactive">Choose whether or not inactive objects should be included in the research (default: true).</param>
        /// <param name="ignoreSelf">Choose whether or not the object where the script is attached should be ignore in the research (default: false).</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindObjectsAttribute(bool includeInactive = true, bool ignoreSelf = false, bool executeInPlayMode = false) : base(
            includeInactive, ignoreSelf, executeInPlayMode)
        {
        }

        /// <summary>
        /// Finds all the objects in the scene that has the same type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="name">The name of the Game Objects to look into.</param>
        /// <param name="includeInactive">Choose whether or not inactive objects should be included in the research (default: true).</param>
        /// <param name="ignoreSelf">Choose whether or not the object where the script is attached should be ignore in the research (default: false).</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindObjectsAttribute(string name, bool includeInactive = true, bool ignoreSelf = false, bool executeInPlayMode = false) :
            base(name, includeInactive, ignoreSelf, executeInPlayMode)
        {
        }
    }
}