namespace MightyAttributes
{
    public class LineAttribute : BaseArrayDecoratorAttribute, IDrawAnywhereAttribute
    {
        public ColorValue Color { get; } = ColorValue.Contrast;
        public string ColorName { get; }

        /// <summary>
        /// Draws a horizontal line around the member.
        /// </summary>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public LineAttribute(ArrayDecoratorPosition position = ArrayDecoratorPosition.Before) : base(position)
        {
        }

        /// <summary>
        /// Draws a horizontal line around the member.
        /// </summary>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public LineAttribute(string positionCallback) : base(positionCallback, ArrayDecoratorPosition.Before)
        {
        }

        /// <summary>
        /// Draws a horizontal line around the member.
        /// </summary>
        /// <param name="color">The color of the line.
        /// See the doc fore more info on color values.</param>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public LineAttribute(ColorValue color, ArrayDecoratorPosition position = ArrayDecoratorPosition.Before) : base(position) =>
            Color = color;

        /// <summary>
        /// Draws a horizontal line around the member.
        /// </summary>
        /// <param name="color">The color of the line.
        /// See the doc fore more info on color values.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public LineAttribute(ColorValue color, string positionCallback) : base(positionCallback, ArrayDecoratorPosition.Before) => Color = color;

        /// <summary>
        /// Draws a horizontal line around the member.
        /// </summary>
        /// <param name="colorName">The color name for the color of the line.
        /// See the doc for more info on color names.</param>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public LineAttribute(string colorName, ArrayDecoratorPosition position) : base(position) => ColorName = colorName;

        /// <summary>
        /// Draws a horizontal line around the member.
        /// </summary>
        /// <param name="colorName">The color name for the color of the line.
        /// See the doc for more info on color names.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public LineAttribute(string colorName, string positionCallback) : base(positionCallback, ArrayDecoratorPosition.Before) =>
            ColorName = colorName;
    }
}