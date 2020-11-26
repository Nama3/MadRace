namespace MightyAttributes
{
    public class MaxValueAttribute : BaseValidatorAttribute
    {
        public float MaxValue { get; }
        
        /// <summary>
        /// Force the value of the float field to be lower than some value
        /// </summary>
        /// <param name="maxValue">The maximum value of the field</param>
        public MaxValueAttribute(float maxValue) : base(false) => MaxValue = maxValue;
        
        /// <summary>
        /// Force the value of the int field to be lower than some value
        /// </summary>
        /// <param name="maxValue">The maximum value of the field</param>
        public MaxValueAttribute(int maxValue) : base(false) => MaxValue = maxValue;
    }
}
