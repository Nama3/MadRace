namespace MightyAttributes
{
    public abstract class BasePositionDecoratorAttribute : BaseDecoratorAttribute
    {
        public DecoratorPosition Position { get; }
        public string PositionCallback { get; }

        protected BasePositionDecoratorAttribute(DecoratorPosition position) => Position = position;
        protected BasePositionDecoratorAttribute(string positionCallback, DecoratorPosition defaultPosition)
        {
            Position = defaultPosition;
            PositionCallback = positionCallback;
        }
    }
}