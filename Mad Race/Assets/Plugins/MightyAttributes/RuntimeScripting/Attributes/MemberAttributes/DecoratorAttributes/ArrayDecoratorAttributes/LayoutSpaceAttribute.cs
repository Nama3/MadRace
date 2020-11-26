namespace MightyAttributes
{
    public class LayoutSpaceAttribute : BaseArrayDecoratorAttribute, IDrawAnywhereAttribute
    {
        public float Size { get; }

        /// <summary>
        /// Add some space horizontally or vertically, according to the layout you’re currently in.
        /// </summary>
        /// <param name="size">The size of space (default: 8).</param>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public LayoutSpaceAttribute(float size = 8, ArrayDecoratorPosition position = ArrayDecoratorPosition.Before) : base(position)
            => Size = size;

        /// <summary>
        /// Add some space horizontally or vertically, according to the layout you’re currently in.
        /// </summary>
        /// <param name="position">The position options of the decoration.</param>
        public LayoutSpaceAttribute(ArrayDecoratorPosition position) : base(position) => Size = 8;

        /// <summary>
        /// Add some space horizontally or vertically, according to the layout you’re currently in.
        /// </summary>
        /// <param name="size">The size of space.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public LayoutSpaceAttribute(float size, string positionCallback) : base(positionCallback, ArrayDecoratorPosition.Before)
            => Size = size;

        /// <summary>
        /// Add some space horizontally or vertically, according to the layout you’re currently in.
        /// </summary>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public LayoutSpaceAttribute(string positionCallback) : base(positionCallback, ArrayDecoratorPosition.Before) => Size = 8;
    }
}