namespace MightyAttributes
{
    [DarkArea]
    public class DarkNestAttribute : LineNestAttribute
    {
        /// <summary>
        /// Activates the attributes of a Serializable class or struct, draws lines around the field and wraps the field inside a dark area.
        /// </summary>
        /// <param name="linePositionCallback">Callback for the position of the lines.
        /// The callback type should be ArrayDecoratorPosition.</param>
        /// <param name="nestOptionsCallback">Callback for the options of the Serializable class or struct.
        /// The callback type should be NestOption.</param>
        public DarkNestAttribute(string linePositionCallback = null, string nestOptionsCallback = null) 
            : base(linePositionCallback, nestOptionsCallback)
        {
        }

        /// <summary>
        /// Activates the attributes of a Serializable class or struct, draws lines around the field and wraps the field inside a dark area.
        /// </summary>
        /// <param name="linePosition">The position options of the decoration.</param>
        /// <param name="nestOptionsCallback">Callback for the options of the Serializable class or struct.
        /// The callback type should be NestOption.</param>
        public DarkNestAttribute(ArrayDecoratorPosition linePosition, string nestOptionsCallback = null) 
            : base(linePosition, nestOptionsCallback)
        {
        }

        /// <summary>
        /// Activates the attributes of a Serializable class or struct, draws lines around the field and wraps the field inside a dark area.
        /// </summary>
        /// <param name="linePositionCallback">Callback for the position of the lines.
        /// The callback type should be ArrayDecoratorPosition.</param>
        /// <param name="options">Some drawing options for the field.</param>
        public DarkNestAttribute(string linePositionCallback, NestOption options) : base(linePositionCallback, options)
        {
        }
        
        /// <summary>
        /// Activates the attributes of a Serializable class or struct, draws lines around the field and wraps the field inside a dark area.
        /// </summary>
        /// <param name="linePosition">The position options of the decoration.</param>
        /// <param name="options">Some drawing options for the field.</param>
        public DarkNestAttribute(ArrayDecoratorPosition linePosition, NestOption options) : base(linePosition, options)
        {
        }
    }
}