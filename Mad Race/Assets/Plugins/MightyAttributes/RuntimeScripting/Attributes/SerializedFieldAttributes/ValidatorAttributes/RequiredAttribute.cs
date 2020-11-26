namespace MightyAttributes
{
    public class RequiredAttribute : BaseValidatorAttribute
    {
        public string Message { get; }

        /// <summary>
        /// Specify that an object field is required, and so its value can’t be null.
        /// </summary>
        /// <param name="message">The message to show if the field value is null (default: “{fieldName} is required”).</param>
        /// <param name="logToConsole">Choose whether or not the error message should be displayed in Unity’s console (default: false).</param>
        public RequiredAttribute(string message = null, bool logToConsole = false) : base(logToConsole) => 
            Message = message;
        
        /// <summary>
        /// Specify that an object field is required, and so its value can’t be null.
        /// </summary>
        /// <param name="logToConsole">Choose whether or not the error message should be displayed in Unity’s console (default: false).</param>
        public RequiredAttribute(bool logToConsole) : base(logToConsole)
        {
        }
    }
}
