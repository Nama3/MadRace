namespace MightyAttributes
{
    public class NoLabelAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Hides the label of the field.
        /// </summary>
        public NoLabelAttribute() : base(FieldOption.Nothing)
        {
        }
    }
}