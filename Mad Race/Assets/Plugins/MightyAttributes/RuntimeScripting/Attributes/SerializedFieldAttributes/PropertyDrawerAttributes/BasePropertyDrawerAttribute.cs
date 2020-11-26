namespace MightyAttributes
{
    public abstract class BasePropertyDrawerAttribute : BaseSerializedFieldAttribute
    {
        public FieldOption Options { get; internal set; }

        protected BasePropertyDrawerAttribute(FieldOption options) => Options = options;
    }
}