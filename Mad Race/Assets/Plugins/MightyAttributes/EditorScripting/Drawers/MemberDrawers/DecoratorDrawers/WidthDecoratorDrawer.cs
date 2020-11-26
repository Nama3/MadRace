#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class WidthDecoratorDrawer : BaseDecoratorDrawer<WidthAttribute>, IDrawAnywhereDecorator, IRefreshDrawer
    {
        private readonly MightyCache<(MightyInfo<float?>, MightyInfo<float?>)> m_widthCache =
            new MightyCache<(MightyInfo<float?>, MightyInfo<float?>)>();

        private readonly MightyCache<(float, float)> m_previousWidthCache = new MightyCache<(float, float)>();

        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (WidthAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (WidthAttribute) baseAttribute);

        protected override void BeginDraw(BaseMightyMember mightyMember, WidthAttribute attribute)
        {
            if (!m_widthCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (labelWidthInfo, fieldWidthInfo) = m_widthCache[mightyMember];

            var labelWidth = labelWidthInfo.Value;
            MightyGUIUtilities.BeginLabelWidth(labelWidth);

            var fieldWidth = fieldWidthInfo.Value;
            if (MightyGUIUtilities.BeginFieldWidth(fieldWidth) && attribute.Force)
                GUILayout.BeginHorizontal(GUILayout.Width(labelWidth ?? 0 + fieldWidth ?? 0));
        }

        protected override void EndDraw(BaseMightyMember mightyMember, WidthAttribute attribute)
        {
            MightyGUIUtilities.EndLabelWidth();

            if (MightyGUIUtilities.EndFieldWidth() && attribute.Force)
                GUILayout.EndHorizontal();
        }

        protected override void Enable(BaseMightyMember mightyMember, WidthAttribute attribute)
        {
            var target = attribute.Target;

            if (!mightyMember.GetInfoFromMember<float?>(target, attribute.LabelWidthCallback, out var labelWidthInfo))
                labelWidthInfo = new MightyInfo<float?>(attribute.LabelWidth);

            if (!mightyMember.GetInfoFromMember<float?>(target, attribute.ContentWidthCallback, out var fieldWidthInfo))
                fieldWidthInfo = new MightyInfo<float?>(attribute.ContentWidth);

            m_widthCache[mightyMember] = (labelWidthInfo, fieldWidthInfo);
        }

        protected override void ClearCache()
        {
            m_widthCache.ClearCache();
            m_previousWidthCache.ClearCache();
        }

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_widthCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (labelWidth, fieldWidth) = m_widthCache[mightyMember];
            labelWidth.RefreshValue();
            fieldWidth.RefreshValue();
        }
    }
}
#endif