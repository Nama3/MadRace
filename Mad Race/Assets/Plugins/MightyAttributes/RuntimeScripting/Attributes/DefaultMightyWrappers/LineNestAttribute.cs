namespace MightyAttributes
{
    [Line("PositionCallback"), Nest("OptionsCallback")]
    public class LineNestAttribute : BaseWrapperAttribute
    {
        [CallbackName] public string PositionCallback { get; }
        [CallbackName] public string OptionsCallback { get; }

        /// <summary>
        /// Activates the attributes of a Serializable class or struct and draws lines around the field.
        /// </summary>
        /// <param name="linePositionCallback">Callback for the position of the lines.
        /// The callback type should be ArrayDecoratorPosition.</param>
        /// <param name="nestOptionsCallback">Callback for the options of the Serializable class or struct.
        /// The callback type should be NestOption.</param>
        public LineNestAttribute(string linePositionCallback = null, string nestOptionsCallback = null)
        {
            PositionCallback = linePositionCallback;
            OptionsCallback = nestOptionsCallback;
        }

        /// <summary>
        /// Activates the attributes of a Serializable class or struct and draws lines around the field.
        /// </summary>
        /// <param name="linePosition">The position options of the decoration.</param>
        /// <param name="nestOptionsCallback">Callback for the options of the Serializable class or struct.
        /// The callback type should be NestOption.</param>
        public LineNestAttribute(ArrayDecoratorPosition linePosition, string nestOptionsCallback = null)
        {
            PositionCallback = linePosition.ToString();
            OptionsCallback = nestOptionsCallback;
        }
        
        /// <summary>
        /// Activates the attributes of a Serializable class or struct and draws lines around the field.
        /// </summary>
        /// <param name="linePositionCallback">Callback for the position of the lines.
        /// The callback type should be ArrayDecoratorPosition.</param>
        /// <param name="options">Some drawing options for the field.</param>
        public LineNestAttribute(string linePositionCallback, NestOption options)
        {
            PositionCallback = linePositionCallback;
            OptionsCallback = options.ToString();
        }
        
        /// <summary>
        /// Activates the attributes of a Serializable class or struct and draws lines around the field.
        /// </summary>
        /// <param name="linePosition">The position options of the decoration.</param>
        /// <param name="options">Some drawing options for the field.</param>
        public LineNestAttribute(ArrayDecoratorPosition linePosition, NestOption options)
        {
            PositionCallback = linePosition.ToString();
            OptionsCallback = options.ToString();
        }
    }
}