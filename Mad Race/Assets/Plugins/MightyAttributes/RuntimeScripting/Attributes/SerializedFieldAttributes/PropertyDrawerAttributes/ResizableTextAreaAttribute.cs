namespace MightyAttributes
{
    public class ResizableTextAreaAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Displays a Text Area which size fit the text it contains.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ResizableTextAreaAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }

    public class ResizableTextAttribute : ResizableTextAreaAttribute, IInheritDrawer
    {        
        /// <summary>
        /// Displays a Text Area which size fit the text it contains.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public ResizableTextAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
            
        }
    }
}
