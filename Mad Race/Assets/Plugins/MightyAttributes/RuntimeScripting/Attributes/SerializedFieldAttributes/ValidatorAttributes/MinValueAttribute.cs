namespace MightyAttributes
{
    public class MinValueAttribute : BaseValidatorAttribute
    {
        public float MinValue { get; }

        /// <summary>
        /// Force the value of the float field to be greater than some value
        /// </summary>
        /// <param name="minValue">The minimum value of the field</param>
        public MinValueAttribute(float minValue) : base(false) => MinValue = minValue;
        
        /// <summary>
        /// Force the value of the int field to be greater than some value
        /// </summary>
        /// <param name="minValue">The minimum value of the field</param>
        public MinValueAttribute(int minValue) : base(false) => MinValue = minValue;
    }
}
