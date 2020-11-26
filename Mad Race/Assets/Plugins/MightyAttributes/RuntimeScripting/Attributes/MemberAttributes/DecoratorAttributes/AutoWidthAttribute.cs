namespace MightyAttributes
{
    public class AutoWidthAttribute : BaseDecoratorAttribute
    {
        public float? ContentWidth { get; }
        public string ContentWidthCallback { get; }

        public bool Force { get; }

        /// <summary>
        /// Changes the width of the label to fit the size of it's text.
        /// </summary>
        public AutoWidthAttribute() => ContentWidth = null;

        /// <summary>
        /// Changes the width of the label to fit the size of it's text and changes the width of the content.
        /// </summary>
        /// <param name="contentWidth">The width of the content.</param>
        /// <param name="force">Choose whether the width of the content should be forced or set as a minimum width.</param>
        public AutoWidthAttribute(float contentWidth, bool force = true)
        {
            ContentWidth = contentWidth;
            Force = force;
        }

        /// <summary>
        /// Changes the width of the label to fit the size of it's text and changes the width of the content.
        /// </summary>
        /// <param name="contentWidthCallback">Callback for the width of the content.
        /// The callback type should be float.</param>
        /// <param name="force">Choose whether the width of the content should be forced or set as a minimum width.</param>
        public AutoWidthAttribute(string contentWidthCallback, bool force = true)
        {
            ContentWidthCallback = contentWidthCallback;
            Force = force;
        }
    }
}