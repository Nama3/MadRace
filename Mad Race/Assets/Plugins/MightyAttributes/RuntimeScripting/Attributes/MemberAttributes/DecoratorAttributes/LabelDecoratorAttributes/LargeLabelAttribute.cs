namespace MightyAttributes
{
    public class LargeLabelAttribute : BaseLabelAttribute
    {
        /// <summary>
        /// Draws a large label around the member.
        /// </summary>
        /// <param name="label">The text of the label.</param>
        /// <param name="position">The position options of the decoration.</param>
        public LargeLabelAttribute(string label, DecoratorPosition position) : base(label, false, position)
        {
        }

        /// <summary>
        /// Draws a large label with a prefix around the member.
        /// </summary>
        /// <param name="prefix"> The text of the prefix.</param>
        /// <param name="label">The text of the label.</param>
        /// <param name="position">The position options of the decoration.</param>
        public LargeLabelAttribute(string prefix, string label, DecoratorPosition position) : base(prefix, label, false, false, position)
        {
        }

        /// <summary>
        /// Draws a large label around the member.
        /// </summary>
        /// <param name="label">The text of the label.</param>
        /// <param name="labelAsCallback">Choose whether or not the label should be considered as a callback of type string.</param>
        /// <param name="position">The position options of the decoration.</param>
        public LargeLabelAttribute(string label, bool labelAsCallback, DecoratorPosition position) : base(label, labelAsCallback, position)
        {
        }

        /// <summary>
        /// Draws a large label with a prefix around the member.
        /// </summary>
        /// <param name="prefix"> The text of the prefix.</param>
        /// <param name="label">The text of the label.</param>
        /// <param name="prefixAsCallback">Choose whether or not the prefix should be considered as a callback of type string (default: false).</param>
        /// <param name="labelAsCallback">Choose whether or not the label should be considered as a callback of type string (default: false).</param>
        /// <param name="position">The position options of the decoration (default: After).</param>
        public LargeLabelAttribute(string prefix, string label, bool prefixAsCallback = false, bool labelAsCallback = false,
            DecoratorPosition position = DecoratorPosition.After) : base(prefix, label, prefixAsCallback, labelAsCallback, position)
        {
            
        }

        /// <summary>
        /// Draws a large label around the member.
        /// </summary>
        /// <param name="label">The text of the label.</param>
        /// <param name="labelAsCallback">Choose whether or not the label should be considered as a callback of type string (default: false).</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public LargeLabelAttribute(string label, bool labelAsCallback = false, string positionCallback = null)
            : base(label, labelAsCallback, positionCallback)
        {
        }

        /// <summary>
        /// Draws a large label with a prefix around the member.
        /// </summary>
        /// <param name="prefix"> The text of the prefix.</param>
        /// <param name="label">The text of the label.</param>
        /// <param name="prefixAsCallback">Choose whether or not the prefix should be considered as a callback of type string (default: false).</param>
        /// <param name="labelAsCallback">Choose whether or not the label should be considered as a callback of type string (default: false).</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public LargeLabelAttribute(string prefix, string label, bool prefixAsCallback = false, bool labelAsCallback = false,
            string positionCallback = null) : base(prefix, label, prefixAsCallback, labelAsCallback, positionCallback)
        {
        }
    }
}