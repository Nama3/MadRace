namespace MightyAttributes
{
    public class ConfirmButtonAttribute : BaseButtonAttribute
    {
        public string ConfirmMessage { get; }
        public string DialogTitle { get; }
        
        /// <summary>
        /// Draws a button that shows a Confirm Dialog and launches a method of your script on confirmation.
        /// The method can’t have any parameters.
        /// </summary>
        /// <param name="confirmMessage">The message to show in the Confirm Dialog.</param>
        /// <param name="height">The height of the button in pixels.</param>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: true).</param>
        public ConfirmButtonAttribute(string confirmMessage, float height, bool executeInPlayMode = true) : base(height, executeInPlayMode) 
            => ConfirmMessage = confirmMessage;

        /// <summary>
        /// Draws a button that shows a Confirm Dialog and launches a method of your script on confirmation.
        /// The method can’t have any parameters.
        /// </summary>
        /// <param name="confirmMessage">The message to show in the Confirm Dialog (default: “Do you want to run {methodName}?”).</param>
        /// <param name="buttonLabel">The label of the button (default: the display name of the method).</param>
        /// <param name="dialogTitle">The title of the dialog (default: the display name of the method).</param>
        /// <param name="height">The height of the button in pixels (default: 20).</param>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: true).</param>
        public ConfirmButtonAttribute(string confirmMessage = null, string buttonLabel = null, string dialogTitle = null, float height = 20,
            bool executeInPlayMode = true) : base(buttonLabel, height, executeInPlayMode)
        {
            ConfirmMessage = confirmMessage;
            DialogTitle = dialogTitle ?? buttonLabel;
        }
    }
}