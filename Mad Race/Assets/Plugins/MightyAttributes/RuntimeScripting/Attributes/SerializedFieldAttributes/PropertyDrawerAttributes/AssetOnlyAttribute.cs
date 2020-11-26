namespace MightyAttributes
{
    public class AssetOnlyAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Draws an object field where Scene references aren't allowed.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public AssetOnlyAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}