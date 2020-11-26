#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class HelpBoxDecoratorDrawer : BasePositionDecoratorDrawer<HelpBoxAttribute>, IDrawAnywhereDecorator, IRefreshDrawer
    {
        private readonly MightyCache<MightyInfo<bool>> m_helpBoxCache = new MightyCache<MightyInfo<bool>>();

        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (HelpBoxAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (HelpBoxAttribute) baseAttribute);

        protected override void DrawDecorator(BaseMightyMember mightyMember, HelpBoxAttribute attribute)
        {
            if (!m_helpBoxCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var visibleInfo = m_helpBoxCache[mightyMember];

            if (visibleInfo.Value)
                MightyGUIUtilities.DrawHelpBox(attribute.Message, attribute.Type);
            else
                MightyGUIUtilities.DrawHelpBox($"{nameof(HelpBoxAttribute)} needs a valid boolean condition to work");
        }

        protected override void Enable(BaseMightyMember mightyMember, HelpBoxAttribute attribute)
        {
            base.Enable(mightyMember, attribute);

            if (!mightyMember.GetBoolInfo(attribute.Target, attribute.VisibleCallback, out var visibleInfo))
                visibleInfo = new MightyInfo<bool>(true);

            m_helpBoxCache[mightyMember] = visibleInfo;
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            m_helpBoxCache.ClearCache();
        }

        void IRefreshDrawer.RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            base.RefreshDrawer(mightyMember, mightyAttribute);
            if (!m_helpBoxCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_helpBoxCache[mightyMember].RefreshValue();
        }
    }
}
#endif