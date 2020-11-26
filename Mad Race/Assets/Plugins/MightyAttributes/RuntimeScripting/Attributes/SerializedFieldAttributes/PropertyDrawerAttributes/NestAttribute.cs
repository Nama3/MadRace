namespace MightyAttributes
{
    public class NestAttribute : BasePropertyDrawerAttribute
    {
        public NestOption NestOptions { get; } = NestOption.Nothing;
        
        public string OptionsCallback { get; }

        /// <summary>
        /// Activates all Mighty Attributes that are nested in the Serializable class or struct of this field.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public NestAttribute(NestOption options = NestOption.Nothing) : base((FieldOption) options) => NestOptions = options;

        /// <summary>
        /// Activates all Mighty Attributes that are nested in the Serializable class or struct of this field.
        /// </summary>
        /// <param name="optionsCallback">Callback for the drawing options of the field.
        /// The callback type should be NestOption.</param>
        public NestAttribute(string optionsCallback) : base(FieldOption.Nothing) => OptionsCallback = optionsCallback;
    }
}