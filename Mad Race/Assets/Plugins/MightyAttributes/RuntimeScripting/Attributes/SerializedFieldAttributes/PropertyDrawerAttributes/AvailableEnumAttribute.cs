namespace MightyAttributes
{
    public class AvailableEnumAttribute : BasePropertyDrawerAttribute
    {
        public string AvailableValuesCallback { get; }

        public bool AllowNothing { get; }

        /// <summary>
        /// Allows you to select what values of the targeted enum are available in the selection dropdown.
        /// </summary>
        /// <param name="availableValuesCallback">The callback for the available values.</param>
        /// <param name="allowNothing">Choose whether or not a "Nothing" option should be available (default: false).
        /// If you select "Nothing", your field value will be 0.
        /// This parameter works only if the targeted enum is marked by the [System.Flags] attribute.</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public AvailableEnumAttribute(string availableValuesCallback, bool allowNothing = false, FieldOption options = FieldOption.Nothing)
            : base(options)
        {
            AvailableValuesCallback = availableValuesCallback;
            AllowNothing = allowNothing;
        }
    }
}