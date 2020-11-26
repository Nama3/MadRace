namespace MightyAttributes
{
    [BeginHorizontal, AutoWidth("ContentWidth")]
    public class BeginHorizontalAutoWidthAttribute : BaseWrapperAttribute
    {
        public float? ContentWidth { get; }

        /// <summary>
        /// Begins a horizontal area and automatically set the width of the member's label.
        /// </summary>
        public BeginHorizontalAutoWidthAttribute() => ContentWidth = null;
        
        /// <summary>
        /// Begins a horizontal area and automatically set the width of the member's label.
        /// Forces the width of the member's content.
        /// </summary>
        /// <param name="contentWidth">The width of the content.</param>
        public BeginHorizontalAutoWidthAttribute(float contentWidth) => ContentWidth = contentWidth;
    }
}