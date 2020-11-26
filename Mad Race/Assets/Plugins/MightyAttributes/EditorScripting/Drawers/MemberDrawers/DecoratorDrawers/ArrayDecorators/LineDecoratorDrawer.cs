#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class LineDecoratorDrawer : BaseArrayDecoratorDrawer<LineAttribute>, IDrawAnywhereDecorator, IRefreshDrawer
    {
        private readonly MightyCache<MightyInfo<Color?>> m_colorCache = new MightyCache<MightyInfo<Color?>>();

        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (LineAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (LineAttribute) baseAttribute);

        protected override void DrawDecorator(BaseMightyMember mightyMember, LineAttribute attribute) =>
            MightyGUIUtilities.DrawLine(m_colorCache[mightyMember].Value);

        protected override void DrawDecoratorElement(MightySerializedField serializedField, int index, LineAttribute attribute) =>
            MightyGUIUtilities.DrawLine(m_colorCache[serializedField].Value);

        protected override Rect DrawDecoratorElement(Rect position, MightySerializedField serializedField, int index,
            LineAttribute attribute) => MightyGUIUtilities.DrawLine(position, m_colorCache[serializedField].Value);

        protected override float GetDecoratorHeight(MightySerializedField serializedField, int index, LineAttribute attribute) =>
            MightyStyleUtilities.GetLineHeight(true);

        protected override void Enable(BaseMightyMember mightyMember, LineAttribute attribute)
        {
            base.Enable(mightyMember, attribute);

            m_colorCache[mightyMember] = mightyMember.GetColorInfo(attribute.Target, attribute.ColorName, attribute.Color);
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            m_colorCache.ClearCache();
        }

        void IRefreshDrawer.RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            base.RefreshDrawer(mightyMember, mightyAttribute);
            if (!m_colorCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_colorCache[mightyMember].RefreshValue();
        }
    }
}
#endif