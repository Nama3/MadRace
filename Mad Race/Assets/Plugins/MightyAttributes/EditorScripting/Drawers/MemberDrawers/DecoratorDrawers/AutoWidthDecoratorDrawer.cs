#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class AutoWidthDecoratorDrawer : BaseDecoratorDrawer<AutoWidthAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<float> m_labelWidthCache = new MightyCache<float>();
        private readonly MightyCache<MightyInfo<float?>> m_fieldWidthCache = new MightyCache<MightyInfo<float?>>();

        private readonly MightyCache<(float, float)> m_previousWidthCache = new MightyCache<(float, float)>();

        protected override void BeginDraw(BaseMightyMember mightyMember, AutoWidthAttribute attribute)
        {
            if (!m_labelWidthCache.Contains(mightyMember)) InitLabelWidth(mightyMember);
            var labelWidth = m_labelWidthCache[mightyMember];

            if (!m_fieldWidthCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var fieldWidthInfo = m_fieldWidthCache[mightyMember];

            MightyGUIUtilities.BeginLabelWidth(labelWidth);

            var fieldWidth = fieldWidthInfo.Value;
            if (MightyGUIUtilities.BeginFieldWidth(fieldWidth) && attribute.Force)
                GUILayout.BeginHorizontal(GUILayout.Width(labelWidth + fieldWidth ?? 0));
        }

        protected override void EndDraw(BaseMightyMember mightyMember, AutoWidthAttribute attribute)
        {
            MightyGUIUtilities.EndLabelWidth();

            if (MightyGUIUtilities.EndFieldWidth() && attribute.Force)
                GUILayout.EndHorizontal();
        }

        private void InitLabelWidth(BaseMightyMember mightyMember)
        {
            var labelWidth = MightyGUIUtilities.TextWidth(((MightySerializedField) mightyMember).Property.displayName);

            if (mightyMember.HasAttribute<BoldAttribute>() ||
                mightyMember.TryGetAttribute<BasePropertyDrawerAttribute>(out var drawerAttribute) &&
                drawerAttribute.Options.Contains(FieldOption.BoldLabel) ||
                mightyMember.TryGetAttribute<BaseArrayAttribute>(out var arrayAttribute) &&
                ((BaseArrayDrawer<BaseArrayAttribute>) arrayAttribute.Drawer).GetOptionsForMember(mightyMember, arrayAttribute)
                .Contains(ArrayOption.BoldLabel))
                labelWidth += MightyGUIUtilities.TAB_SIZE;

            m_labelWidthCache[mightyMember] = labelWidth;
        }

        protected override void Enable(BaseMightyMember mightyMember, AutoWidthAttribute attribute)
        {
            if (!mightyMember.GetInfoFromMember<float?>(attribute.Target, attribute.ContentWidthCallback, out var fieldWidthInfo))
                fieldWidthInfo = new MightyInfo<float?>(attribute.ContentWidth);

            m_fieldWidthCache[mightyMember] = fieldWidthInfo;
        }

        protected override void ClearCache()
        {
            m_fieldWidthCache.ClearCache();
            m_previousWidthCache.ClearCache();
        }

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_fieldWidthCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_fieldWidthCache[mightyMember].RefreshValue();
        }
    }
}
#endif