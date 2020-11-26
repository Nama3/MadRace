namespace MightyAttributes
{
    [DarkArea, ButtonArray("OptionsCallback")]
    public class DarkButtonArrayAttribute : BaseWrapperAttribute
    {
        [CallbackName] public string OptionsCallback { get; }

        /// <summary>
        /// Draws an array with buttons inside a dark area.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public DarkButtonArrayAttribute(ArrayOption options = ArrayOption.Nothing) => OptionsCallback = options.ToString();

        /// <summary>
        /// Draws an array with buttons inside a dark area.
        /// </summary>
        /// <param name="optionsCallback">Callback for the drawing options of the field.
        /// The callback type should be ArrayOption.</param>
        public DarkButtonArrayAttribute(string optionsCallback) => OptionsCallback = optionsCallback;
    }
}