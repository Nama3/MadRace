namespace MightyAttributes
{
    public class MinMaxSliderAttribute : BasePropertyDrawerAttribute
    {
        public string MinValueCallback { get; }
        public string MaxValueCallback { get; }

        public float MinValue { get; }
        public float MaxValue { get; }

        /// <summary>
        /// Displays a double slider for a Vector2 or a Vector2Int
        /// The min value will be saved in the x property, and the max value in the y property
        /// </summary>
        /// <param name="minValue">The minimum value of the slider</param>
        /// <param name="maxValue">The maximum value of the slider</param>
        /// <param name="options">Some drawing options for the field (default: Nothing)</param>
        public MinMaxSliderAttribute(float minValue, float maxValue, FieldOption options = FieldOption.Nothing) : base(options)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Displays a double slider for a Vector2Int
        /// The min value will be saved in the x property, and the max value in the y property
        /// </summary>
        /// <param name="minValue">The minimum value of the slider</param>
        /// <param name="maxValue">The maximum value of the slider</param>
        /// <param name="options">Some drawing options for the field (default: Nothing)</param>
        public MinMaxSliderAttribute(int minValue, int maxValue, FieldOption options = FieldOption.Nothing) : base(options)
        {
            MaxValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Displays a double slider for a Vector2 or a Vector2Int
        /// The min value will be saved in the x property, and the max value in the y property
        /// </summary>
        /// <param name="minValueCallback">The callback for the minimum value of the slider</param>
        /// <param name="maxValueCallback">The callback for the maximum value of the slider</param>
        /// <param name="options">Some drawing options for the field (default: Nothing)</param>
        public MinMaxSliderAttribute(string minValueCallback, string maxValueCallback, FieldOption options = FieldOption.Nothing) :
            base(options)
        {
            MinValueCallback = minValueCallback;
            MaxValueCallback = maxValueCallback;
        }
    }
}