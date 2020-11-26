namespace MightyAttributes
{
    public class MinMaxAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Replaces the "X" and "Y" labels of a Vector2 with "Min" and "Max".
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public MinMaxAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}