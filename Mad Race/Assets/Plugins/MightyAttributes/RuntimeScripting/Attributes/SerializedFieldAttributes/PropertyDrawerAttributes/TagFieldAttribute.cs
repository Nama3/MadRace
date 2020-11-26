namespace MightyAttributes
{
    public class TagFieldAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Displays a dropdown that lets you select a tag name.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public TagFieldAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }  
    
    public class TagAttribute : TagFieldAttribute, IInheritDrawer
    {
        /// <summary>
        /// Displays a dropdown that lets you select a tag name.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public TagAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}