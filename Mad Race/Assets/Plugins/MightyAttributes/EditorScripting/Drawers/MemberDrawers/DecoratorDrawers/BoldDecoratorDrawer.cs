#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class BoldDecoratorDrawer : BaseDecoratorDrawer<BoldAttribute>, IDrawAnywhereDecorator
    {
        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            MightyStyleUtilities.SetBoldDefaultFont(true);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            MightyStyleUtilities.SetBoldDefaultFont(false);

        protected override void BeginDraw(BaseMightyMember mightyMember, BoldAttribute attribute) =>
            MightyStyleUtilities.SetBoldDefaultFont(true);

        protected override void EndDraw(BaseMightyMember mightyMember, BoldAttribute attribute) =>
            MightyStyleUtilities.SetBoldDefaultFont(false);

        protected override void Enable(BaseMightyMember mightyMember, BoldAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif