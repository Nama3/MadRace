namespace MightyAttributes
{
    public class HelpBoxAttribute : BasePositionDecoratorAttribute, IDrawAnywhereAttribute
    {
        public string Message { get; }
        public HelpBoxType Type { get; }
        public string VisibleCallback { get; }

        /// <summary>
        /// Draws a help box around the member.
        /// </summary>
        /// <param name="message">The message of the box.</param>
        /// <param name="type">The type of the box. It can be Info, Warning or Error (default: Info).</param>
        /// <param name="visibleCallback">callback to define whether or not the box is visible.
        /// The callback type should be bool.</param>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public HelpBoxAttribute(string message, HelpBoxType type = HelpBoxType.Info, string visibleCallback = null,
            DecoratorPosition position = DecoratorPosition.Before) : base(position)
        {
            Message = message;
            Type = type;
            VisibleCallback = visibleCallback;
        }

        /// <summary>
        /// Draws a help box around the member.
        /// </summary>
        /// <param name="message">The message of the box.</param>
        /// <param name="visibleCallback">callback to define whether or not the box is visible.
        /// The callback type should be bool.</param>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public HelpBoxAttribute(string message, string visibleCallback, DecoratorPosition position = DecoratorPosition.Before)
            : this(message, HelpBoxType.Info, visibleCallback, position)
        {
        }

        /// <summary>
        /// Draws a help box around the member.
        /// </summary>
        /// <param name="message">The message of the box.</param>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public HelpBoxAttribute(string message, DecoratorPosition position) : this(message, HelpBoxType.Info, null, position)
        {
        }
        
        /// <summary>
        /// Draws a help box around the member.
        /// </summary>
        /// <param name="message">The message of the box.</param>
        /// <param name="type">The type of the box. It can be Info, Warning or Error (default: Info).</param>
        /// <param name="position">The position options of the decoration (default: Before).</param>
        public HelpBoxAttribute(string message, HelpBoxType type, DecoratorPosition position) : this(message, type, null, position)
        {
        }
        
        /// <summary>
        /// Draws a help box around the member.
        /// </summary>
        /// <param name="message">The message of the box.</param>
        /// <param name="type">The type of the box. It can be Info, Warning or Error (default: Info).</param>
        /// <param name="visibleCallback">callback to define whether or not the box is visible.
        /// The callback type should be bool.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public HelpBoxAttribute(string message, HelpBoxType type, string visibleCallback, string positionCallback)
            : base(positionCallback, DecoratorPosition.Before)
        {
            Message = message;
            Type = type;
            VisibleCallback = visibleCallback;
        }

        /// <summary>
        /// Draws a help box around the member.
        /// </summary>
        /// <param name="message">The message of the box.</param>
        /// <param name="visibleCallback">callback to define whether or not the box is visible.
        /// The callback type should be bool.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public HelpBoxAttribute(string message, string visibleCallback, string positionCallback)
            : this(message, HelpBoxType.Info, visibleCallback, positionCallback)
        {
        }
        
        /// <summary>
        /// Draws a help box around the member.
        /// </summary>
        /// <param name="message">The message of the box.</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public HelpBoxAttribute(string message, string positionCallback) : this(message, HelpBoxType.Info, null, positionCallback)
        {
        }
        
        /// <summary>
        /// Draws a help box around the member.
        /// </summary>
        /// <param name="message">The message of the box.</param>
        /// <param name="type">The type of the box. It can be Info, Warning or Error (default: Info).</param>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public HelpBoxAttribute(string message, HelpBoxType type, string positionCallback) : this(message, type, null, positionCallback)
        {
        }
    }
}