namespace MightyAttributes
{
    public class GetComponentsInParentAttribute : BaseWithNameAttribute
    {        
        /// <summary>
        /// Finds all the components in the parents of the current object that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentsInParentAttribute(bool executeInPlayMode = false) : base(false, false, executeInPlayMode)
        {
        }
        
        /// <summary>
        /// Finds all the components in the parents of the current object that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="name">The name of the Game Objects to look into.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentsInParentAttribute(string name, bool executeInPlayMode = false) : base(name, false, false, executeInPlayMode)
        {
        }
    }
}