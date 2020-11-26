namespace MightyAttributes
{
    public abstract class BaseValidatorAttribute : BaseSerializedFieldAttribute
    {
        public bool LogToConsole { get; }
        
        protected BaseValidatorAttribute(bool logToConsole) => LogToConsole = logToConsole;
    }
}