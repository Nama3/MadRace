namespace MightyAttributes
{
    [EndHorizontal, AutoWidth("ContentWidth")]
    public class EndHorizontalAutoWidthAttribute : BaseWrapperAttribute
    {
        public float? ContentWidth { get; }

        /// <summary>
        /// Ends a horizontal area and automatically set the width of the member's label.
        /// </summary>
        public EndHorizontalAutoWidthAttribute() => ContentWidth = null;

        /// <summary>
        /// Ends a horizontal area and automatically set the width of the member's label.
        /// Forces the width of the member's content.
        /// </summary>
        /// <param name="contentWidth">The width of the content.</param>
        public EndHorizontalAutoWidthAttribute(float contentWidth) => ContentWidth = contentWidth;
    }
}