namespace MightyAttributes
{
    public class FilePanelAttribute : BaseExplorerAttribute
    {
        public string Extension { get; }
        public bool ExtensionAsCallback { get; }

        /// <summary>
        /// Draws a button to the right of the field that opens a panel that lets you select a file.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public FilePanelAttribute(FieldOption options = FieldOption.Nothing) : base(null, false, options)
        {
        }

        /// <summary>
        /// Draws a button to the right of the field that opens a panel that lets you select a file.
        /// </summary>
        /// <param name="extension">The extension of the file to select.
        /// Can hold multiple values if they are separated with a comma (without space).
        /// Accept any file if left null.</param>
        /// <param name="defaultPath">The default path at which the panel will be open.
        /// If it’s left null, the panel will open at the “Assets” folder.</param>
        /// <param name="extensionAsCallback">Choose whether or not the extension should be consider as a callback (default: false).</param>
        /// <param name="pathAsCallback">Choose whether or not the default path should be consider as a callback (default: false).</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public FilePanelAttribute(string extension, string defaultPath, bool extensionAsCallback = false, bool pathAsCallback = false,
            FieldOption options = FieldOption.Nothing) : base(defaultPath, pathAsCallback, options)
        {
            Extension = extension;
            ExtensionAsCallback = extensionAsCallback;
        }

        /// <summary>
        /// Draws a button to the right of the field that opens a panel that lets you select a file.
        /// </summary>
        /// <param name="extension">The extension of the file to select.
        /// Can hold multiple values if they are separated with a comma (without space).
        /// Accept any file if left null.</param>
        /// <param name="extensionAsCallback">Choose whether or not the extension should be consider as a callback (default: false).</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public FilePanelAttribute(string extension, bool extensionAsCallback = false, FieldOption options = FieldOption.Nothing)
            : base(null, false, options)
        {
            Extension = extension;
            ExtensionAsCallback = extensionAsCallback;
        }
    }
}