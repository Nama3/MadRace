namespace MightyAttributes
{
    public abstract class BaseFoldGroupAttribute : BaseGroupAttribute
    {
        protected BaseFoldGroupAttribute(string groupName, bool drawName, bool nameAsCallback, string backgroundColorName,
            string contentColorName) : base(groupName, drawName, nameAsCallback, backgroundColorName, contentColorName)
        {
        }

        protected BaseFoldGroupAttribute(string groupName, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor) : base(groupName, drawName, nameAsCallback, backgroundColor, contentColor)
        {
        }      
        
        protected BaseFoldGroupAttribute(string groupName, ColorValue backgroundColor, ColorValue contentColor)
            : base(groupName, backgroundColor, contentColor)
        {
        }

        protected BaseFoldGroupAttribute(ColorValue backgroundColor, ColorValue contentColor) : base(backgroundColor, contentColor)
        {
            
        }
    }
}