namespace MightyAttributes
{
    public class ProgressBarAttribute : BasePropertyDrawerAttribute
    {
        public string Label { get; }

        public float MaxValue { get; } = 100;
        public string MaxValueCallback { get; }

        public ColorValue Color { get; } = ColorValue.Blue;
        public string ColorName { get; }

        /// <summary>
        /// Displays a progress bar for an int or a float.
        /// </summary>
        /// <param name="label">The label displayed on the bar.</param>
        /// <param name="maxValue">The maximum value of the bar (default: 100).</param>
        /// <param name="color">The color of the bar (default: Blue).</param>
        public ProgressBarAttribute(string label = "", float maxValue = 100, ColorValue color = ColorValue.Blue) : base(FieldOption.Nothing)
        {
            Label = label;
            MaxValue = maxValue;
            Color = color;
        }

        /// <summary>
        /// Displays a progress bar for an int or a float.
        /// </summary>
        /// <param name="label">The label displayed on the bar.</param>
        /// <param name="maxValueCallback">The callback for the maximum value of the bar.</param>
        /// <param name="color">The color of the bar (default: Blue).</param>
        public ProgressBarAttribute(string label, string maxValueCallback, ColorValue color) : base(FieldOption.Nothing)
        {
            Label = label;
            MaxValueCallback = maxValueCallback;
            Color = color;
        }

        /// <summary>
        /// Displays a progress bar for an int or a float.
        /// </summary>
        /// <param name="label">The label displayed on the bar.</param>
        /// <param name="maxValue">The maximum value of the bar (default: 100).</param>
        /// <param name="colorName">The Color Label (see doc) for the color of the bar.</param>
        public ProgressBarAttribute(string label, float maxValue, string colorName = null) : base(FieldOption.Nothing)
        {
            Label = label;
            MaxValue = maxValue;
            ColorName = colorName;
        }

        /// <summary>
        /// Displays a progress bar for an int or a float.
        /// </summary>
        /// <param name="label">The label displayed on the bar.</param>
        /// <param name="maxValueCallback">The callback for the maximum value of the bar.</param>
        /// <param name="colorName">The Color Label (see doc) for the color of the bar.</param>
        public ProgressBarAttribute(string label, string maxValueCallback, string colorName = null) : base(FieldOption.Nothing)
        {
            Label = label;
            MaxValueCallback = maxValueCallback;
            ColorName = colorName;
        }
    }
}