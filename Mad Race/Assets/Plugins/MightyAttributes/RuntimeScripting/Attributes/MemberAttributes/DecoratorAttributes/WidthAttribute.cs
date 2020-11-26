namespace MightyAttributes
{
    public class WidthAttribute : BaseDecoratorAttribute, IDrawAnywhereAttribute
    {
        public float? LabelWidth { get; }
        public float? ContentWidth { get; }

        public string LabelWidthCallback { get; }
        public string ContentWidthCallback { get; }

        public bool Force { get; }

        /// <summary>
        /// Changes the width of the member's content and label.
        /// </summary>
        /// <param name="labelWidth">The width of the label.</param>
        /// <param name="contentWidth">The width of the content.</param>
        /// <param name="force">Choose whether the width of the content should be forced or set as a minimum width.</param>
        public WidthAttribute(float labelWidth, float contentWidth, bool force = true)
        {
            LabelWidth = labelWidth;
            ContentWidth = contentWidth;

            Force = force;
        }

        /// <summary>
        /// Changes the width of the member's content and label.
        /// </summary>
        /// <param name="labelWidthCallback">Callback for the width of the label.
        /// The callback type should be float.</param>
        /// <param name="contentWidthCallback">Callback for the width of the content.
        /// The callback type should be float.</param>
        /// <param name="force">Choose whether the width of the content should be forced or set as a minimum width.</param>
        public WidthAttribute(string labelWidthCallback, string contentWidthCallback, bool force = true)
        {
            LabelWidthCallback = labelWidthCallback;
            ContentWidthCallback = contentWidthCallback;

            Force = force;
        }

        protected WidthAttribute(float width, bool forLabel, bool force)
        {
            if (forLabel) LabelWidth = width;
            else ContentWidth = width;
            
            Force = force;
        }
    }

    public class LabelWidthAttribute : WidthAttribute, IInheritDrawer
    {
        /// <summary>
        /// Changes the width of the label of the member.
        /// </summary>
        /// <param name="width">The width of the label.</param>
        public LabelWidthAttribute(float width) : base(width, true, false)
        {
        }
        
        /// <summary>
        /// Changes the width of the label of the member.
        /// </summary>
        /// <param name="widthCallback">Callback for the width of the label.
        /// The callback type should be float.</param>
        public LabelWidthAttribute(string widthCallback) : base(widthCallback, null)
        {
        }
    }

    public class ContentWidthAttribute : WidthAttribute, IInheritDrawer
    {
        /// <summary>
        /// Changes the width of the content of the member.
        /// </summary>
        /// <param name="width">The width of the content.</param>
        /// <param name="force">Choose whether the width of the content should be forced or set as a minimum width.</param>
        public ContentWidthAttribute(float width, bool force = true) : base(width, false, force)
        {
        }

        /// <summary>
        /// Changes the width of the content of the member.
        /// </summary>
        /// <param name="widthCallback">Callback for the width of the content.
        /// The callback type should be float.</param>
        /// <param name="force">Choose whether the width of the content should be forced or set as a minimum width.</param>
        public ContentWidthAttribute(string widthCallback, bool force = true) : base(null, widthCallback, force)
        {
        }
    }
}