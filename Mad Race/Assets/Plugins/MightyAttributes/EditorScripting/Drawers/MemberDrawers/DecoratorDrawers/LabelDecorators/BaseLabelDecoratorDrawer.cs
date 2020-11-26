#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public abstract class BaseLabelDecoratorDrawer<T> : BasePositionDecoratorDrawer<T>, IRefreshDrawer, IDrawAnywhereDecorator
        where T : BaseLabelAttribute
    {
        private readonly MightyCache<(MightyInfo<string>, MightyInfo<string>)> m_labelCache =
            new MightyCache<(MightyInfo<string>, MightyInfo<string>)>();

        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (T) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (T) baseAttribute);

        protected override void BeginDraw(BaseMightyMember mightyMember, T attribute)
        {
            var position = PositionByMember(mightyMember, attribute);
            if (position.Contains(DecoratorPosition.Horizontal)) GUILayout.BeginHorizontal();
            if (position.Contains(DecoratorPosition.Before)) DrawDecorator(mightyMember, attribute);
        }

        protected override void EndDraw(BaseMightyMember mightyMember, T attribute)
        {
            var position = PositionByMember(mightyMember, attribute);
            if (position.Contains(DecoratorPosition.After)) DrawDecorator(mightyMember, attribute);
            if (position.Contains(DecoratorPosition.Horizontal)) GUILayout.EndHorizontal();
        }

        protected override void DrawDecorator(BaseMightyMember mightyMember, T attribute)
        {
            if (!m_labelCache.Contains(mightyMember)) Enable(mightyMember, attribute);
            var (labelInfo, prefixInfo) = m_labelCache[mightyMember];

            var label = labelInfo.Value;
            var prefix = prefixInfo?.Value;

            DrawLabel(mightyMember, attribute, prefix, label);
        }

        protected abstract void DrawLabel(BaseMightyMember mightyMember, T attribute, string prefix, string label);

        protected override void Enable(BaseMightyMember mightyMember, T attribute)
        {
            base.Enable(mightyMember, attribute);

            var target = attribute.Target;

            if (!attribute.PrefixAsCallback || !mightyMember.GetInfoFromMember<string>(target, attribute.Prefix, out var prefixInfo))
                prefixInfo = new MightyInfo<string>(attribute.Prefix);

            if (!attribute.LabelAsCallback || !mightyMember.GetInfoFromMember<string>(target, attribute.Label, out var labelInfo))
                labelInfo = new MightyInfo<string>(attribute.Label);

            m_labelCache[mightyMember] = (labelInfo, prefixInfo);
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            m_labelCache.ClearCache();
        }

        void IRefreshDrawer.RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute attribute)
        {
            base.RefreshDrawer(mightyMember, attribute);
            if (!m_labelCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, attribute);
                return;
            }

            var (labelInfo, prefixInfo) = m_labelCache[mightyMember];
            labelInfo.RefreshValue();
            prefixInfo.RefreshValue();
        }
    }
}
#endif