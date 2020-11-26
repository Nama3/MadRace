#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class TitleDecoratorDrawer : BasePositionDecoratorDrawer<TitleAttribute>, IDrawAnywhereDecorator, IRefreshDrawer
    {
        private readonly MightyCache<MightyInfo<string>> m_headerCache = new MightyCache<MightyInfo<string>>();

        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (TitleAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (TitleAttribute) baseAttribute);

        protected override void DrawDecorator(BaseMightyMember mightyMember, TitleAttribute attribute)
        {
            if (!m_headerCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);

            MightyGUIUtilities.DrawTitle(m_headerCache[mightyMember].Value);
        }

        protected override void Enable(BaseMightyMember mightyMember, TitleAttribute attribute)
        {
            base.Enable(mightyMember, attribute);

            if (!attribute.TitleAsCallback ||
                !mightyMember.GetInfoFromMember<string>(attribute.Target, attribute.Title, out var headerInfo))
                headerInfo = new MightyInfo<string>(attribute.Title);

            m_headerCache[mightyMember] = headerInfo;
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            m_headerCache.ClearCache();
        }

        void IRefreshDrawer.RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            base.RefreshDrawer(mightyMember, mightyAttribute);

            if (!m_headerCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_headerCache[mightyMember]?.RefreshValue();
        }
    }
}
#endif