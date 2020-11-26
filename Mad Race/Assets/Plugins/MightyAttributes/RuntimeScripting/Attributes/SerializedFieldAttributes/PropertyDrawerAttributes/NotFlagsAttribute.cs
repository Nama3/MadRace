namespace MightyAttributes
{
    public class NotFlagsAttribute : BasePropertyDrawerAttribute
    {
        public bool AllowNothing { get; }

        /// <summary>
        /// Displays a regular enum dropdown selection for an enum marked by the [System.Flags] attribute.
        /// </summary>
        /// <param name="allowNothing">Choose whether or not a "Nothing" option should be available (default: false).
        /// If you select "Nothing", your field value will be 0.</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public NotFlagsAttribute(bool allowNothing = false, FieldOption options = FieldOption.Nothing) : base(options) =>
            AllowNothing = allowNothing;
    }
}