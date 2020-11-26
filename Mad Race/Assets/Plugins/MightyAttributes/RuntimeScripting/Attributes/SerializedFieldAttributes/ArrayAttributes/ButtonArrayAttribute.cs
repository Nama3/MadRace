namespace MightyAttributes
{
    public class ButtonArrayAttribute : BaseArrayAttribute
    {
        /// <summary>
        /// Provides a handy way to add or remove elements from an array, using simple buttons.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ButtonArrayAttribute(ArrayOption options = ArrayOption.Nothing) : base(options)
        {
        }       
        
        /// <summary>
        /// Provides a handy way to add or remove elements from an array, using simple buttons.
        /// </summary>
        /// <param name="optionsCallback">Callback for the drawing options of the field.
        /// The callback type should be ArrayOption.</param>
        public ButtonArrayAttribute(string optionsCallback) : base(optionsCallback)
        {
        }
    }
}