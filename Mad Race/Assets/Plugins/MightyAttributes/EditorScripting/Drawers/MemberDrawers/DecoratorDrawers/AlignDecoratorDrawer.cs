#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class AlignDecoratorDrawer : BaseDecoratorDrawer<BaseAlignAttribute>, IDrawAnywhereDecorator
    {
        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (BaseAlignAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (BaseAlignAttribute) baseAttribute);

        protected override void BeginDraw(BaseMightyMember mightyMember, BaseAlignAttribute attribute) =>
            MightyGUIUtilities.BeginDrawAlign(attribute.Align);

        protected override void EndDraw(BaseMightyMember mightyMember, BaseAlignAttribute attribute) =>
            MightyGUIUtilities.EndDrawAlign(attribute.Align);

        protected override void Enable(BaseMightyMember mightyMember, BaseAlignAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif