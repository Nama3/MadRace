namespace MightyAttributes
{
    public class ScriptFieldAttribute : BasePositionDecoratorAttribute, IDrawAnywhereAttribute
    {
        /// <summary>
        /// Draws the read-only field that links to the script anywhere in the inspector.
        /// </summary>
        /// <param name="position">The position options of the decoration.</param>
        public ScriptFieldAttribute(DecoratorPosition position = DecoratorPosition.Before) : base(position)
        {
        }        
        
        /// <summary>
        /// Draws the read-only field that links to the script anywhere in the inspector.
        /// </summary>
        /// <param name="positionCallback">Callback for the position options of the decoration.
        /// The callback type should be DecoratorPosition.</param>
        public ScriptFieldAttribute(string positionCallback) : base(positionCallback, DecoratorPosition.Before)
        {
        }
    }
}