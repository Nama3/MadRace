namespace MightyAttributes
{
    public class OnModifiedPropertiesAttribute : BaseMethodAttribute
    {
        /// <summary>
        /// Runs a method when a serialized property of this script is modified.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: false).</param>
        public OnModifiedPropertiesAttribute(bool executeInPlayMode = false) : base(executeInPlayMode)
        {
        }
    }
}