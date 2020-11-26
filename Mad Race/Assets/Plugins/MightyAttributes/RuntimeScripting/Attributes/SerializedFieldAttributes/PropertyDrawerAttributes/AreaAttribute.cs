namespace MightyAttributes
{
    public class AreaAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Replaces the "X", "Y", "Z" and "W" labels of a Vector4 with "Left", "Right", "Bottom" and "Top".
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public AreaAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}