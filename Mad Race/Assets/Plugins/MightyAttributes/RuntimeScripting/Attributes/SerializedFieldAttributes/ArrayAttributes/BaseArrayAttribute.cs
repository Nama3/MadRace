namespace MightyAttributes
{
    public abstract class BaseArrayAttribute : BaseSerializedFieldAttribute
    {
        public ArrayOption Options { get; }
        
        public string OptionsCallback { get; }

        protected BaseArrayAttribute(ArrayOption options) => Options = options;

        protected BaseArrayAttribute(string optionsCallback) => OptionsCallback = optionsCallback;
    }
}