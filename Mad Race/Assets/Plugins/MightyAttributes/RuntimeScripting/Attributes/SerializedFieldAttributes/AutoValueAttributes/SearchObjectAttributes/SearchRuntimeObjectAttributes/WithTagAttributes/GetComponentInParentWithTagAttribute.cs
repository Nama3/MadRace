namespace MightyAttributes
{
    public class GetComponentInParentWithTagAttribute : BaseWithTagAttribute
    {
        /// <summary>
        /// Finds the first component in the parents of the current object that have the same type than the field.
        /// </summary>
        /// <param name="tag">The tag of the Game Object to look into.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentInParentWithTagAttribute(string tag, bool executeInPlayMode = false) : base(tag, false, false,
            executeInPlayMode)
        {
        }
    }
}