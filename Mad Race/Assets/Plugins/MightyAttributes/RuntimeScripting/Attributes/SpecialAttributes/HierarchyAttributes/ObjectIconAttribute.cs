namespace MightyAttributes
{
    public class ObjectIconsAttribute : BaseHierarchyAttribute
    {
        public string IconPath { get; }
        
        public ObjectIconsAttribute(string iconPath, int priority = 0) : base(priority) => IconPath = iconPath;
    }
}