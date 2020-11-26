namespace MightyAttributes
{
    public class LogOnChangedAttribute : BaseChangeCheckAttribute
    {
        /// <summary>
        /// Logs the value of the field when it is modified.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public LogOnChangedAttribute(bool executeInPlayMode = false) : base(executeInPlayMode)
        {
        }
    }
}
