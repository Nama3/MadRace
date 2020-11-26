namespace MightyAttributes
{
    public class SliderAttribute : BasePropertyDrawerAttribute
    {
        public string MinValueCallback { get; }
        public string MaxValueCallback { get; }

        public float MinValue { get; }
        public float MaxValue { get; }

        /// <summary>
        /// Displays a slider with a min and max value for a float
        /// </summary>
        /// <param name="minValue">The minimum value of the slider</param>
        /// <param name="maxValue">The maximum value of the slider</param>
        /// <param name="options">Some drawing options for the field (default: Nothing)</param>
        public SliderAttribute(float minValue, float maxValue, FieldOption options = FieldOption.Nothing) : base(options)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Displays a slider with a min and max value for an int
        /// </summary>
        /// <param name="minValue">The minimum value of the slider</param>
        /// <param name="maxValue">The maximum value of the slider</param>
        /// <param name="options">Some drawing options for the field (default: Nothing)</param>
        public SliderAttribute(int minValue, int maxValue, FieldOption options = FieldOption.Nothing) : base(options)
        {
            MaxValue = minValue;
            MaxValue = maxValue;
        }
        
        /// <summary>
        /// Displays a slider with a min and max value for an int or a float
        /// </summary>
        /// <param name="minValueCallback">The callback for the minimum value of the slider</param>
        /// <param name="maxValueCallback">The callback for the maximum value of the slider</param>
        /// <param name="options">Some drawing options for the field (default: Nothing)</param>
        public SliderAttribute(string minValueCallback, string maxValueCallback, FieldOption options = FieldOption.Nothing) : base(options)
        {
            MinValueCallback = minValueCallback;
            MaxValueCallback = maxValueCallback;
        }
    }
}