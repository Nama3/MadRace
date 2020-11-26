namespace MightyAttributes
{
    public class OnEnableAttribute : BaseMethodAttribute
    {
        /// <summary>
        /// Runs a method when you select your Game Object in the hierarchy.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: true).</param>
        public OnEnableAttribute(bool executeInPlayMode = true) : base(executeInPlayMode)
        {
        }
    }
}