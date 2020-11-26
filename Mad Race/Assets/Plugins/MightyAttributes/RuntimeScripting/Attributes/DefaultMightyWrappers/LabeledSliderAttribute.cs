namespace MightyAttributes
{
    [Slider("MinValueCallback", "MaxValueCallback")]
    [BoldLabel("LabelCallback", labelAsCallback: true)]
    public class LabeledSliderAttribute : BaseWrapperAttribute
    {
        [CallbackName] public string MinValueCallback { get; }
        [CallbackName] public string MaxValueCallback { get; }

        [CallbackName] public string LabelCallback { get; }

        /// <summary>
        /// Draws a slider for an int field followed at the next line by a bold label.
        /// </summary>
        /// <param name="labelCallback">Callback for the label.
        /// The callback type should be string.</param>
        /// <param name="maxValue">The maximum value of the slider.</param>
        public LabeledSliderAttribute(string labelCallback, int maxValue)
        {
            LabelCallback = labelCallback;
            MinValueCallback = "0";
            MaxValueCallback = maxValue.ToString();
        }

        /// <summary>
        /// Draws a slider for an int field followed at the next line by a bold label.
        /// </summary>
        /// <param name="labelCallback">Callback for the label.
        /// The callback type should be string.</param>
        /// <param name="minValue">The minimum value of the slider.</param>
        /// <param name="maxValue">The maximum value of the slider.</param>
        public LabeledSliderAttribute(string labelCallback, int minValue, int maxValue)
        {
            LabelCallback = labelCallback;
            MinValueCallback = minValue.ToString();
            MaxValueCallback = maxValue.ToString();
        }  
        
        /// <summary>
        /// Draws a slider for an int field followed at the next line by a bold label.
        /// </summary>
        /// <param name="labelCallback">Callback for the label.
        /// The callback type should be string.</param>
        /// <param name="maxValueCallback">Callback for the maximum value of the slider.
        /// The callback type should be int.</param>
        public LabeledSliderAttribute(string labelCallback, string maxValueCallback)
        {
            LabelCallback = labelCallback;
            MinValueCallback = "0";
            MaxValueCallback = maxValueCallback;
        }

        /// <summary>
        /// Draws a slider for an int field followed at the next line by a bold label.
        /// </summary>
        /// <param name="labelCallback">Callback for the label.
        /// The callback type should be string.</param>
        /// <param name="minValueCallback">Callback for the minimum value of the slider.
        /// The callback type should be int.</param>
        /// <param name="maxValueCallback">Callback for the maximum value of the slider.
        /// The callback type should be int.</param>
        public LabeledSliderAttribute(string labelCallback, string minValueCallback, string maxValueCallback)
        {
            LabelCallback = labelCallback;
            MinValueCallback = minValueCallback;
            MaxValueCallback = maxValueCallback;
        }
    }
}