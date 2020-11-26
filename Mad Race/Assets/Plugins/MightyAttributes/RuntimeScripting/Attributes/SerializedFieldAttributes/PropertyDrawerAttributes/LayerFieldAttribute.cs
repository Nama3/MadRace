namespace MightyAttributes
{
    public class LayerFieldAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Displays a dropdown that lets you select the flag value of a layer.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public LayerFieldAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }  
    
    public class LayerAttribute : LayerFieldAttribute, IInheritDrawer
    {
        /// <summary>
        /// Displays a dropdown that lets you select the flag value of a layer.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public LayerAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}