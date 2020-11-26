namespace MightyAttributes
{
    public class TitleAttribute : BasePositionDecoratorAttribute, IDrawAnywhereAttribute
    {
        public string Title { get; }
        public bool TitleAsCallback { get; }

        /// <summary>
        /// Draws a title around the member, just like Unity's [Header].
        /// </summary>
        /// <param name="title">The text of the title.</param>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public TitleAttribute(string title, DecoratorPosition position = DecoratorPosition.Before) : base(position) => Title = title;

        /// <summary>
        /// Draws a title around the member, just like Unity's [Header].
        /// </summary>
        /// <param name="title">The text of the title.</param>
        /// <param name="titleAsCallback">Choose whether or not the text of the title should be considered as a Callback of type string.</param>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public TitleAttribute(string title, bool titleAsCallback, DecoratorPosition position = DecoratorPosition.Before) : base(position)
        {
            Title = title;
            TitleAsCallback = titleAsCallback;
        }
        
        /// <summary>
        /// Draws a title around the member, just like Unity's [Header].
        /// </summary>
        /// <param name="title">The text of the title.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public TitleAttribute(string title, string positionCallback) : base(positionCallback, DecoratorPosition.Before) => Title = title;

        
        /// <summary>
        /// Draws a title around the member, just like Unity's [Header].
        /// </summary>
        /// <param name="title">The text of the title.</param>
        /// <param name="titleAsCallback">Choose whether or not the text of the title should be considered as a Callback of type string.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public TitleAttribute(string title, bool titleAsCallback, string positionCallback)
            : base(positionCallback, DecoratorPosition.Before)
        {
            Title = title;
            TitleAsCallback = titleAsCallback;
        }
    }
}