namespace MightyAttributes
{
    public class CustomDrawerAttribute : BasePropertyDrawerAttribute
    {
        public string DrawerCallback { get; }

        public string ElementHeightCallback { get; }

        /// <summary>
        /// Lets you code your own way to draw a field.
        /// See the doc to understand how to use it.
        /// </summary>
        /// <param name="drawerCallback">The callback that will draw the field.</param>
        /// <param name="elementHeightCallback">The callback that returns the height of an element, if the field is an array.</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public CustomDrawerAttribute(string drawerCallback, string elementHeightCallback = null, FieldOption options = FieldOption.Nothing)
            : base(options)
        {
            DrawerCallback = drawerCallback;
            ElementHeightCallback = elementHeightCallback;
        }
    }
}