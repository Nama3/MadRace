namespace MightyAttributes
{
    public class ColorAttribute : BaseDecoratorAttribute, IDrawAnywhereAttribute
    {
        public ColorValue BackgroundColor { get; }
        public ColorValue ContentColor { get; }
        
        public string BackgroundColorName { get; }
        public string ContentColorName { get; }

        /// <summary>
        /// Changes the color of the member.
        /// </summary>
        /// <param name="backgroundColor">The background color of the member.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color of the member.
        /// See the doc fore more info on color values.</param>
        public ColorAttribute(ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default)
        {
            BackgroundColor = backgroundColor;
            ContentColor = contentColor;
        }
        
        /// <summary>
        /// Changes the color of the member.
        /// </summary>
        /// <param name="backgroundColorName">The color name for the background color of the member.
        /// See the doc for more info on color names.</param>
        /// <param name="contentColorName">The color name for the content color of the member.
        /// See the doc for more info on color names.</param>
        public ColorAttribute(string backgroundColorName = null, string contentColorName = null)
        {
            BackgroundColorName = backgroundColorName;
            ContentColorName = contentColorName;
            
            BackgroundColor = ColorValue.Default;
            ContentColor = ColorValue.Default;
        }
    }
}