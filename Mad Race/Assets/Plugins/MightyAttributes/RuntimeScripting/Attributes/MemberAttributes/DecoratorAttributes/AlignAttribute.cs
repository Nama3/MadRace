namespace MightyAttributes
{
    public abstract class BaseAlignAttribute : BaseDecoratorAttribute, IDrawAnywhereAttribute
    {
        public Align Align { get; }
        
        protected BaseAlignAttribute(Align align) => Align = align;
    }
    
    public class AlignLeftAttribute : BaseAlignAttribute, IInheritDrawer
    {
        /// <summary>
        /// Aligns the member to the left.
        /// </summary>
        public AlignLeftAttribute() : base(Align.Left)
        {
        }
    }
    
    public class AlignRightAttribute : BaseAlignAttribute, IInheritDrawer
    {
        /// <summary>
        /// Aligns the member to the right.
        /// </summary>
        public AlignRightAttribute() : base(Align.Right)
        {
        }
    }

    public class AlignCenterAttribute : BaseAlignAttribute, IInheritDrawer
    {
        /// <summary>
        /// Aligns the member in the center.
        /// </summary>
        public AlignCenterAttribute() : base(Align.Center)
        {
        }
    }
}