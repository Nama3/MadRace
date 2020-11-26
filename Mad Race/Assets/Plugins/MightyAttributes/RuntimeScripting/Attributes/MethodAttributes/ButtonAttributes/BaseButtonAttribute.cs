namespace MightyAttributes
{
    public abstract class BaseButtonAttribute : BaseMethodAttribute
    {
        public string Label { get; }
        public float Height { get; }

        protected BaseButtonAttribute(float height, bool executeInPlayMode) : base(executeInPlayMode) => Height = height;

        protected BaseButtonAttribute(string label, float height, bool executeInPlayMode) : base(executeInPlayMode)
        {
            Label = label;
            Height = height;
        }
    }
}