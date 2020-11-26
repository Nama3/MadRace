namespace MightyAttributes
{
    public abstract class BaseLayoutAttribute : BaseArrayDecoratorAttribute, IDrawAnywhereAttribute
    {
        protected BaseLayoutAttribute() : base(ArrayDecoratorPosition.Nothing)
        {
        }
    }
}