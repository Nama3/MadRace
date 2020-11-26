namespace MightyAttributes
{
    public class GetComponentAttribute : BaseSearchRuntimeObjectAttribute
    {
        /// <summary>
        /// Finds the first component in the current object that have the same type than the field.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public GetComponentAttribute(bool executeInPlayMode = false) : base(false, false, executeInPlayMode)
        {
        }
    }
}