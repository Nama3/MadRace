namespace MightyAttributes
{
    public class IconButtonAttribute : BaseButtonAttribute
    {
        public string IconName { get; }

        /// <summary>
        /// Draws a button with an icon that launches a method of your script.
        /// The method can’t have any parameters.
        /// </summary>
        /// <param name="iconName">The name of the icon you want to draw.
        /// See the doc for more info on icon names.</param>
        /// <param name="height">The height of the button in pixels.</param>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: true).</param>
        public IconButtonAttribute(string iconName, float height, bool executeInPlayMode = true)
            : base(null, height, executeInPlayMode) => IconName = iconName;

        /// <summary>
        /// Draws a button with an icon that launches a method of your script.
        /// The method can’t have any parameters.
        /// </summary>
        /// <param name="iconName">The name of the icon you want to draw.
        /// See the doc for more info on icon names.</param>
        /// <param name="buttonLabel">The label of the button (default: the display name of the method).</param>
        /// <param name="height">The height of the button in pixels (default: 20).</param>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: true).</param>
        public IconButtonAttribute(string iconName, string buttonLabel = null, float height = 20, bool executeInPlayMode = true) 
            : base(buttonLabel, height, executeInPlayMode) => IconName = iconName;
    }
}