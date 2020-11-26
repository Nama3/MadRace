namespace MightyAttributes
{
    public class AvailableMaskAttribute : BasePropertyDrawerAttribute
    {
        public int AvailableMask { get; }

        public string AvailableMaskCallback { get; }

        public bool AllowEverything { get; }

        /// <summary>
        /// Allows you to select what flags are available for an enum mask.
        /// The enum targeted has to be marked by the [System.Flags] attribute.
        /// </summary>
        /// <param name="availableMask">The mask that contains all the available flags.</param>
        /// <param name="allowEverything">Choose whether or not the "Everything" option should be available (default: true).</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public AvailableMaskAttribute(int availableMask, bool allowEverything = true, FieldOption options = FieldOption.Nothing)
            : base(options)
        {
            AvailableMask = availableMask;
            AllowEverything = allowEverything;
        }

        /// <summary>
        /// Allows you to select what flags are available for an enum mask.
        /// The enum targeted has to be marked by the [System.Flags] attribute.
        /// </summary>
        /// <param name="availableMaskCallback">The callback for the mask that contains all the available flags.</param>
        /// <param name="allowEverything">Choose whether or not the "Everything" option should be available (default: true).</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public AvailableMaskAttribute(string availableMaskCallback, bool allowEverything = true, FieldOption options = FieldOption.Nothing)
            : base(options)
        {
            AvailableMaskCallback = availableMaskCallback;
            AllowEverything = allowEverything;
        }
    }
}