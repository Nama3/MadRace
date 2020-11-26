namespace MightyAttributes
{
    public abstract class BaseSimpleGroupAttribute : BaseGroupAttribute
    {
        public bool DrawLine { get; }
        public ColorValue LineColor { get; }

        protected BaseSimpleGroupAttribute(string groupName, bool drawName, bool nameAsCallback, string backgroundColorName,
            string contentColorName, bool drawLine, ColorValue lineColor) : base(groupName, drawName, nameAsCallback, backgroundColorName,
            contentColorName)
        {
            DrawLine = drawLine;
            LineColor = lineColor;
        }


        protected BaseSimpleGroupAttribute(string groupName, ColorValue backgroundColor, ColorValue contentColor, ColorValue lineColor) 
            : base(groupName, backgroundColor, contentColor)
        {
            DrawLine = true;
            LineColor = lineColor;
        }

        protected BaseSimpleGroupAttribute(string groupName, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor, bool drawLine, ColorValue lineColor) : base(groupName, drawName, nameAsCallback, backgroundColor,
            contentColor)
        {
            DrawLine = drawLine;
            LineColor = lineColor;
        }
        
        protected BaseSimpleGroupAttribute(ColorValue backgroundColor, ColorValue contentColor) : base(backgroundColor, contentColor)
        {
            DrawLine = false;
            LineColor = ColorValue.Default;
        }
    }
}