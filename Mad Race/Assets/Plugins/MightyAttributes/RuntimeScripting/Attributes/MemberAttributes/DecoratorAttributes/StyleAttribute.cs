namespace MightyAttributes
{
    public class StyleAttribute : BaseDecoratorAttribute, IDrawAnywhereAttribute
    {
        public string StyleName { get; }

        /// <summary>
        /// Wraps the member inside an area of the specified GUIStyle.
        /// </summary>
        /// <param name="styleName">The style name for the GUIStyle of the area in which your field will be drawn.
        /// See the doc for more info on style names.</param>
        public StyleAttribute(string styleName) => StyleName = styleName;
    }
}