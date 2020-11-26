namespace MightyAttributes
{
    public class ItemNamesAttribute : BaseArrayAttribute
    {
        public string[] ItemNames { get; }
        public string ItemNamesCallback { get; }

        public bool ForceSize { get; }

        /// <summary>
        /// Allows you to overwrite the label of each elements of the array.
        /// </summary>
        /// <param name="itemNames">The names of the items of the array.</param>
        public ItemNamesAttribute(params string[] itemNames) : base(ArrayOption.Nothing) => ItemNames = itemNames;

        /// <summary>
        /// Allows you to overwrite the label of each elements of the array.
        /// </summary>
        /// <param name="itemNamesCallback">Callback for the names of the items of the array</param>
        /// <param name="forceSize">Choose whether or not the size of the array should be the same as the size of the item names (default: false).</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ItemNamesAttribute(string itemNamesCallback, bool forceSize = false, ArrayOption options = ArrayOption.Nothing)
            : base(options)
        {
            ItemNamesCallback = itemNamesCallback;
            ForceSize = forceSize;
        }

        /// <summary>
        /// Allows you to overwrite the label of each elements of the array.
        /// </summary>
        /// <param name="itemNamesCallback">Callback for the names of the items of the array</param>
        /// <param name="optionsCallback">Callback for the drawing options of the field.
        /// The callback type should be ArrayOption.</param>
        /// <param name="forceSize">Choose whether or not the size of the array should be the same as the size of the item names (default: false).</param>
        public ItemNamesAttribute(string itemNamesCallback, string optionsCallback, bool forceSize = false) : base(optionsCallback)
        {
            ItemNamesCallback = itemNamesCallback;
            ForceSize = forceSize;
        }
    }
}