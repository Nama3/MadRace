namespace MightyAttributes
{
    public class FlexibleSpaceAttribute : BaseArrayDecoratorAttribute, IDrawAnywhereAttribute
    {
        /// <summary>
        /// Add a flexible space if there’s any available.
        /// </summary>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public FlexibleSpaceAttribute(ArrayDecoratorPosition position = ArrayDecoratorPosition.Before) : base(position)
        {
        }

        /// <summary>
        /// Add a flexible space if there’s any available.
        /// </summary>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be ArrayDecoratorPosition.</param>
        public FlexibleSpaceAttribute(string positionCallback) : base(positionCallback, ArrayDecoratorPosition.Before)
        {
        }
    }
}