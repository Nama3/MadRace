namespace MightyAttributes
{
    public class OnValueChangedAttribute : BaseChangeCheckAttribute
    {
        public string ValueChangedCallback { get; }

        /// <summary>
        /// Execute a callback when the value of the field is changed inside the inspector.
        /// The callback won’t be executed at runtime, just within the Editor.
        /// </summary>
        /// <param name="valueChangedCallback">The name of the function that should be launched when the value of the field is modified.
        /// This function should have no parameters.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public OnValueChangedAttribute(string valueChangedCallback, bool executeInPlayMode = false) : base(executeInPlayMode) =>
            ValueChangedCallback = valueChangedCallback;
    }
}