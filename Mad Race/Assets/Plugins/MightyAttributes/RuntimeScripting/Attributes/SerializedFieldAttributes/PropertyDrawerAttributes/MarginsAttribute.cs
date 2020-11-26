namespace MightyAttributes
{
    public class MarginsAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Replaces the "X", "Y", "Z" and "W" labels of a Vector4 with "Left", "Top", "Right" and "Bottom".
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public MarginsAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}