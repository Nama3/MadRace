namespace MightyAttributes
{
    public abstract class BaseArrayDecoratorAttribute : BaseGlobalDecoratorAttribute
    {
        public ArrayDecoratorPosition Position { get; }
        public string PositionCallback { get; }

        protected BaseArrayDecoratorAttribute(ArrayDecoratorPosition position) => Position = position;
        protected BaseArrayDecoratorAttribute(string positionCallback, ArrayDecoratorPosition defaultPosition)
        {
            Position = defaultPosition;
            PositionCallback = positionCallback;
        }
    }
}