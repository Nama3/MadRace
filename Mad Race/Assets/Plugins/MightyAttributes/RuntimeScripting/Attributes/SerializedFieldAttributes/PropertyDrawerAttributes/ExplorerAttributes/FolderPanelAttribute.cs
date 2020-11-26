namespace MightyAttributes
{
    public class FolderPanelAttribute : BaseExplorerAttribute
    {
        /// <summary>
        /// Draws a button to the right of the field that opens a panel that lets you select a folder.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public FolderPanelAttribute(FieldOption options = FieldOption.Nothing) : base(null, false, options)
        {
        }

        /// <summary>
        /// Draws a button to the right of the field that opens a panel that lets you select a folder.
        /// </summary>
        /// <param name="defaultPath">The default path at which the panel will be open.
        /// If it’s left null, the panel will open at the “Assets” folder.</param>
        /// <param name="pathAsCallback">Choose whether or not the default path should be consider as a callback (default: false).</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public FolderPanelAttribute(string defaultPath, bool pathAsCallback = false, FieldOption options = FieldOption.Nothing)
            : base(defaultPath, pathAsCallback, options)
        {
        }
    }
}