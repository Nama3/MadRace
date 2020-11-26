#if UNITY_EDITOR
using System.Reflection;

namespace MightyAttributes.Editor
{
    public class ShowNonSerializedFieldDrawer : BaseNonSerializedFieldDrawer<ShowNonSerializedAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<MightyInfo<object>> m_showNonSerializedCache = new MightyCache<MightyInfo<object>>();

        protected override void DrawField(MightyNonSerializedField nonSerializedField, ShowNonSerializedAttribute attribute)
        {
            var field = nonSerializedField.MemberInfo;

            if (!m_showNonSerializedCache.Contains(nonSerializedField)) EnableDrawer(nonSerializedField, attribute);
            var value = m_showNonSerializedCache[nonSerializedField].Value;

            if (MightyGUIUtilities.DrawLayoutField(attribute.DrawPrettyName ? field.Name.GetPrettyName() : field.Name, value,
                nonSerializedField.MemberInfo.FieldType, attribute.Enabled)) return;

            MightyGUIUtilities.DrawHelpBox($"{nameof(ShowNonSerializedAttribute)} doesn't support the type {field.FieldType.Name}");
        }

        protected override void Enable(BaseMightyMember mightyMember, ShowNonSerializedAttribute attribute)
        {
            var field = (MightyMember<FieldInfo>) mightyMember;
            var target = attribute.Target;

            m_showNonSerializedCache[mightyMember] = new MightyInfo<object>(target, field.MemberInfo, field.MemberInfo.GetValue(target));
        }

        protected override void ClearCache() => m_showNonSerializedCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_showNonSerializedCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_showNonSerializedCache[mightyMember].RefreshValue();
        }
    }
}
#endif