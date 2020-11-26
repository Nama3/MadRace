namespace MightyAttributes
{
    public abstract class BaseSimpleScrollGroupAttribute : BaseSimpleGroupAttribute
    {
        public float Height { get; }

        public override ColorValue GetDefaultBackgroundColor() => ColorValue.White;

        protected BaseSimpleScrollGroupAttribute(string groupName, float height, bool drawName, bool nameAsCallback, string backgroundColorName,
            string contentColorName, bool drawLine, ColorValue lineColor)
            : base(groupName, drawName, nameAsCallback, backgroundColorName, contentColorName, drawLine, lineColor) => Height = height;

        protected BaseSimpleScrollGroupAttribute(string groupName, float height, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor, bool drawLine, ColorValue lineColor)
            : base(groupName, drawName, nameAsCallback, backgroundColor, contentColor, drawLine, lineColor) => Height = height;

        protected BaseSimpleScrollGroupAttribute(string groupName, float height, ColorValue backgroundColor, ColorValue contentColor, 
            ColorValue lineColor) : base(groupName, backgroundColor, contentColor, lineColor) => Height = height;

        protected BaseSimpleScrollGroupAttribute(float height, ColorValue backgroundColor, ColorValue contentColor) 
            : base(backgroundColor, contentColor) => Height = height;
    }
}