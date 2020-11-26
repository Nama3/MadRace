#if UNITY_EDITOR
using System.Reflection;

namespace MightyAttributes.Editor
{
    public class ShowPropertyDrawer : BaseNativePropertyDrawer<ShowPropertyAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<MightyInfo<object>> m_showPropertyCache = new MightyCache<MightyInfo<object>>();

        protected override void DrawNativeProperty(MightyNativeProperty nativeProperty, ShowPropertyAttribute attribute)
        {
            var property = nativeProperty.MemberInfo;

            if (!m_showPropertyCache.Contains(nativeProperty)) EnableDrawer(nativeProperty, attribute);
            var value = m_showPropertyCache[nativeProperty].Value;

            if (MightyGUIUtilities.DrawLayoutField(attribute.DrawPrettyName ? property.Name.GetPrettyName() : property.Name, value,
                nativeProperty.MemberInfo.PropertyType, attribute.Enabled)) return;

            MightyGUIUtilities.DrawHelpBox($"{nameof(ShowPropertyDrawer)} doesn't support the type {property.PropertyType.Name}");
        }

        protected override void Enable(BaseMightyMember mightyMember, ShowPropertyAttribute attribute)
        {
            var property = (MightyMember<PropertyInfo>) mightyMember;
            var target = attribute.Target;

            m_showPropertyCache[mightyMember] = new MightyInfo<object>(target, property.MemberInfo, property.MemberInfo.GetValue(target));
        }

        protected override void ClearCache() => m_showPropertyCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_showPropertyCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_showPropertyCache[mightyMember].RefreshValue();
        }
    }
}
#endif