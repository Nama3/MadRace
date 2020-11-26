namespace MightyAttributes
{
    public class ValueFromAttribute : BaseAutoValueAttribute
    {
        public string ValueCallback { get; }

        /// <summary>
        /// Initialize a field with the return value of the specified callback.
        /// </summary>
        /// <param name="valueCallback">The callback for the value of the field.
        /// The callback type should be the same than the field.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public ValueFromAttribute(string valueCallback, bool executeInPlayMode = false) : base(executeInPlayMode) =>
            ValueCallback = valueCallback;
    }
}