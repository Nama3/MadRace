namespace MightyAttributes
{
    public class ButtonAttribute : BaseButtonAttribute
    {
        /// <summary>
        /// Draws a button that launches a method of your script.
        /// The method can’t have any parameters.
        /// </summary>
        /// <param name="height">The height of the button in pixels (default: 20).</param>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: true).</param>
        public ButtonAttribute(float height = 20, bool executeInPlayMode = true) : base(height, executeInPlayMode)
        {
        }

        /// <summary>
        /// Draws a button that launches a method of your script.
        /// The method can’t have any parameters.
        /// </summary>
        /// <param name="label">The label of the button.</param>
        /// <param name="height">The height of the button in pixels (default: 20).</param>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: true).</param>
        public ButtonAttribute(string label, float height = 20, bool executeInPlayMode = true) : base(label, height, executeInPlayMode)
        {
        }
    }
}