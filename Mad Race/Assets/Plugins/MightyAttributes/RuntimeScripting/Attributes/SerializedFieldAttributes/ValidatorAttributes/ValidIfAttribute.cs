namespace MightyAttributes
{
    public class ValidIfAttribute : BaseValidatorAttribute
    {
        public string ConditionCallback { get; }
        public string Message { get; }

        /// <summary>
        /// Allows you to define a condition on which the field value is valid, and displays a message if it’s not.
        /// </summary>
        /// <param name="conditionCallback">The callback for the condition on which the field is valid or not.
        /// The callback type should be bool and have a single parameter of the same type than the field.</param>
        /// <param name="message">The message to show if the field value is null (default: “{fieldName} is required”).</param>
        /// <param name="logToConsole">Choose whether or not the error message should be displayed in Unity’s console (default: false).</param>
        public ValidIfAttribute(string conditionCallback, string message = null, bool logToConsole = false) : base(logToConsole)
        {
            ConditionCallback = conditionCallback;
            Message = message;
        }

        /// <summary>
        /// Allows you to define a condition on which the field value is valid, and displays a message if it’s not.
        /// </summary>
        /// <param name="conditionCallback">The callback for the condition on which the field is valid or not.
        /// The callback type should be bool and have a single parameter of the same type than the field.</param>
        /// <param name="logToConsole">Choose whether or not the error message should be displayed in Unity’s console (default: false).</param>
        public ValidIfAttribute(string conditionCallback, bool logToConsole) : base(logToConsole) => ConditionCallback = conditionCallback;
    }
}