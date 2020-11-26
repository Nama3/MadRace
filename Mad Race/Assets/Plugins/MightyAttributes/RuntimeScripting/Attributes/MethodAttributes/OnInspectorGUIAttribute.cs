namespace MightyAttributes
{
    public class OnInspectorGUIAttribute : BaseMethodAttribute
    {
        /// <summary>
        /// Runs a method when the inspector tab is moved or rescaled or when the mouse hovers over it.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: true).</param>
        public OnInspectorGUIAttribute(bool executeInPlayMode = true) : base(executeInPlayMode)
        {
        }
    }
}