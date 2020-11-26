namespace MightyAttributes
{
    public class GetComponentButtonAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Displays a button to the right of the field that initializes it with the first component found on the same object.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public GetComponentButtonAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}