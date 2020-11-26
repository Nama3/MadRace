namespace MightyAttributes
{
    public class ShowPropertyAttribute : BaseNativePropertyAttribute
    {
        public bool Enabled { get; }
        public bool DrawPrettyName { get; }

        /// <summary>
        /// Shows a native property inside the inspector.
        /// </summary>
        /// <param name="enabled">Choose whether or not the field is enabled (default: false).
        /// Even it is enabled, it can't be modify since it's not serialized.</param>
        /// <param name="drawPrettyName">Choose whether or not the field name should be drawn as a Pretty Name, like Unity does with serializable field (default: true).</param>
        public ShowPropertyAttribute(bool enabled = false, bool drawPrettyName = true)
        {
            Enabled = enabled;
            DrawPrettyName = drawPrettyName;
        }
    }
}
