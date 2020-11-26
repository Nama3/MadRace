namespace MightyAttributes
{
    public class ShowNonSerializedAttribute : BaseNonSerializedFieldAttribute
    {
        public bool Enabled { get; }
        public bool DrawPrettyName { get; }

        /// <summary>
        /// Shows inside the inspector a field that is not serialized by Unity.
        /// </summary>
        /// <param name="enabled">Choose whether or not the field is enabled (default: false).
        /// Even it is enabled, it can't be modify since it's not serialized.</param>
        /// <param name="drawPrettyName">Choose whether or not the field name should be drawn as a Pretty Name, like Unity does with serializable field (default: true).</param>
        public ShowNonSerializedAttribute(bool enabled = false, bool drawPrettyName = true)
        {
            Enabled = enabled;
            DrawPrettyName = drawPrettyName;
        }
    }
}