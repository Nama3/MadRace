namespace MightyAttributes
{
    public abstract class BaseGroupAttribute : BaseMemberAttribute
    {
        public string GroupName { get; }
        public bool DrawName { get; }
        public bool NameAsCallback { get; }

        public string BackgroundColorName { get; }
        public string ContentColorName { get; }

        public ColorValue? BackgroundColor { get; }
        public ColorValue ContentColor { get; }

        public abstract ColorValue GetDefaultBackgroundColor();
        
        protected BaseGroupAttribute(string groupName, bool drawName, bool nameAsCallback, string backgroundColorName,
            string contentColorName)
        {
            GroupName = groupName;
            DrawName = drawName;
            NameAsCallback = nameAsCallback;
            
            BackgroundColorName = backgroundColorName;
            ContentColorName = contentColorName;
            
            ContentColor = ColorValue.Default;
        }

        protected BaseGroupAttribute(string groupName, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor)
        {
            GroupName = groupName;
            DrawName = drawName;
            NameAsCallback = nameAsCallback;
            
            BackgroundColor = backgroundColor;
            ContentColor = contentColor;
        }      
        
        protected BaseGroupAttribute(string groupName, ColorValue backgroundColor, ColorValue contentColor)
        {
            GroupName = groupName;
            DrawName = true;
            NameAsCallback = false;
            
            BackgroundColor = backgroundColor;
            ContentColor = contentColor;
        }      
        
        protected BaseGroupAttribute(ColorValue backgroundColor, ColorValue contentColor)
        {
            GroupName = null;
            DrawName = false;
            NameAsCallback = false;
            
            BackgroundColor = backgroundColor;
            ContentColor = contentColor;
        }
    }
}