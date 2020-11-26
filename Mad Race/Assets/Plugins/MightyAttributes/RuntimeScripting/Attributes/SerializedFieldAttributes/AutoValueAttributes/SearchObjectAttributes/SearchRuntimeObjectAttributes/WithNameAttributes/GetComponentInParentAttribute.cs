namespace MightyAttributes
{
    public class GetComponentInParentAttribute : BaseWithNameAttribute
    {
        /// <summary>
        /// Finds the first component in the parents of the current object that have the same type than the field.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentInParentAttribute(bool executeInPlayMode = false) : base(false, false, executeInPlayMode)
        {
        }

        /// <summary>
        /// Finds the first component in the parents of the current object that have the same type than the field.
        /// </summary>
        /// <param name="name">The name of the Game Object to look into.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentInParentAttribute(string name, bool executeInPlayMode = false) : base(name, false, false, executeInPlayMode)
        {
        }
    }
}