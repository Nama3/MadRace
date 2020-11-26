namespace MightyAttributes
{
    public class LabelAttribute : BaseLabelAttribute
    {
        public string StyleName { get; }

        /// <summary>
        /// Draws a label around the member.
        /// </summary>
        /// <param name="label">The text of the label.</param>
        /// <param name="position">The position options of the decoration.</param>
        public LabelAttribute(string label, DecoratorPosition position) : base(label, false, position)
        {
        }

        /// <summary>
        /// Draws a label with a prefix around the member.
        /// </summary>
        /// <param name="prefix"> The text of the prefix.</param>
        /// <param name="label">The text of the label.</param>
        /// <param name="position">The position options of the decoration.</param>
        public LabelAttribute(string prefix, string label, DecoratorPosition position) : base(prefix, label, false, false, position)
        {
        }

        /// <summary>
        /// Draws a label around the member.
        /// </summary>
        /// <param name="label">The text of the label.</param>
        /// <param name="labelAsCallback">Choose whether or not the label should be considered as a callback of type string.</param>
        /// <param name="styleName">The style name for the GUIStyle of the label (it won't affect the prefix).
        /// See the doc for more info on style names.</param>
        /// <param name="position">The position options of the decoration.</param>
        public LabelAttribute(string label, bool labelAsCallback, string styleName, DecoratorPosition position) : base(label,
            labelAsCallback, position) => StyleName = styleName;

        /// <summary>
        /// Draws a label with a prefix around the member.
        /// </summary>
        /// <param name="prefix"> The text of the prefix.</param>
        /// <param name="label">The text of the label.</param>
        /// <param name="prefixAsCallback">Choose whether or not the prefix should be considered as a callback of type string.</param>
        /// <param name="labelAsCallback">Choose whether or not the label should be considered as a callback of type string.</param>
        /// <param name="styleName">The style name for the GUIStyle of the label (it won't affect the prefix).
        /// See the doc for more info on style names.</param>
        /// <param name="position">The position options of the decoration.</param>
        public LabelAttribute(string prefix, string label, bool prefixAsCallback, bool labelAsCallback, string styleName, DecoratorPosition position) : base(prefix, label, prefixAsCallback,
            labelAsCallback, position) => StyleName = styleName;

        /// <summary>
        /// Draws a label around the member.
        /// </summary>
        /// <param name="label">The text of the label.</param>
        /// <param name="labelAsCallback">Choose whether or not the label should be considered as a callback of type string (default: false).</param>
        /// <param name="styleName">The style name for the GUIStyle of the label (it won't affect the prefix).
        /// See the doc for more info on style names.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public LabelAttribute(string label, bool labelAsCallback = false, string styleName = null, string positionCallback = null)
            : base(label, labelAsCallback, positionCallback) => StyleName = styleName;

        /// <summary>
        /// Draws a label with a prefix around the member.
        /// </summary>
        /// <param name="prefix"> The text of the prefix.</param>
        /// <param name="label">The text of the label.</param>
        /// <param name="prefixAsCallback">Choose whether or not the prefix should be considered as a callback of type string (default: false).</param>
        /// <param name="labelAsCallback">Choose whether or not the label should be considered as a callback of type string (default: false).</param>
        /// <param name="styleName">The style name for the GUIStyle of the label (it won't affect the prefix).
        /// See the doc for more info on style names.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public LabelAttribute(string prefix, string label, bool prefixAsCallback = false, bool labelAsCallback = false,
            string styleName = null, string positionCallback = null) : base(prefix, label, prefixAsCallback, labelAsCallback,
            positionCallback) => StyleName = styleName;
    }
}