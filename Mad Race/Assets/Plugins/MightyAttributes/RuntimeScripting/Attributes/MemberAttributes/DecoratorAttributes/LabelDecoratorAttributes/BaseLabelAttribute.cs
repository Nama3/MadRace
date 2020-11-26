namespace MightyAttributes
{
    public abstract class BaseLabelAttribute : BasePositionDecoratorAttribute, IDrawAnywhereAttribute
    {
        public string Prefix { get; }
        public string Label { get; }

        public bool PrefixAsCallback { get; }
        public bool LabelAsCallback { get; }

        protected BaseLabelAttribute(string label, bool labelAsCallback, DecoratorPosition position) : base(position)
        {
            Label = label;
            LabelAsCallback = labelAsCallback;
        }

        protected BaseLabelAttribute(string prefix, string label, bool prefixAsCallback, bool labelAsCallback, DecoratorPosition position)
            : base(position)
        {
            Prefix = prefix;
            Label = label;

            PrefixAsCallback = prefixAsCallback;
            LabelAsCallback = labelAsCallback;
        }     
        
        protected BaseLabelAttribute(string label, bool labelAsCallback, string positionCallback)
            : base(positionCallback, DecoratorPosition.After)
        {
            Label = label;
            LabelAsCallback = labelAsCallback;
        }

        protected BaseLabelAttribute(string prefix, string label, bool prefixAsCallback, bool labelAsCallback, string positionCallback)
            : base(positionCallback, DecoratorPosition.After)
        {
            Prefix = prefix;
            Label = label;

            PrefixAsCallback = prefixAsCallback;
            LabelAsCallback = labelAsCallback;
        }
    }
}