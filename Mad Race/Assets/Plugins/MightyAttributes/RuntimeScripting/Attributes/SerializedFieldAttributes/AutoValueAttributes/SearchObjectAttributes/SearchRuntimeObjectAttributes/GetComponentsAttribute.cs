namespace MightyAttributes
{
    public class GetComponentsAttribute : BaseSearchObjectAttribute
    {
        /// <summary>
        /// Finds all the components in the current object that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentsAttribute(bool executeInPlayMode = false) : base(executeInPlayMode)
        {
        }
    }
}