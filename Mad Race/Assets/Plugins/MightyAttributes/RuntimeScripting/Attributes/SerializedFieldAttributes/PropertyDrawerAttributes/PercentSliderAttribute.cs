namespace MightyAttributes
{
    public class PercentSliderAttribute : BasePropertyDrawerAttribute
    {
        public bool Between01 { get; }

        /// <summary>
        /// Displays a slider between 0 and 100, followed by a "%" sign, for an int or a float.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public PercentSliderAttribute(FieldOption options = FieldOption.Nothing) : base(options) => Between01 = false;

        /// <summary>
        /// Displays a slider between 0 and 100, followed by a "%" sign, for a float.
        /// </summary>
        /// <param name="between01">Choose whether the float should be between 0 and 1 or 0 and 100.</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public PercentSliderAttribute(bool between01, FieldOption options = FieldOption.Nothing) : base(options) => 
            Between01 = between01;        
    }
}