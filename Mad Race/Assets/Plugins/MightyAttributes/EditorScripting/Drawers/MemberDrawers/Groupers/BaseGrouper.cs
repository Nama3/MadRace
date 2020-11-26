#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface IGrouper : IMemberDrawer
    {
    }

    public abstract class BaseGrouper<T> : BaseMemberDrawer<T>, IGrouper, IRefreshDrawer where T : BaseGroupAttribute
    {
        protected readonly MightyCache<(MightyInfo<Color?>, MightyInfo<Color?>)> m_colorsCache =
            new MightyCache<(MightyInfo<Color?>, MightyInfo<Color?>)>();

        private readonly MightyCache<int> m_indentLevelCache = new MightyCache<int>();

        protected int IndentLevelForMember(BaseMightyMember mightyMember) =>
            !m_indentLevelCache.TryGetValue(mightyMember, out var value)
                ? m_indentLevelCache[mightyMember] = EditorGUI.indentLevel
                : value;

        public virtual void DrawLine(ColorValue color = ColorValue.Contrast) => MightyGUIUtilities.DrawLine(color.GetColor(), false);

        protected override void Enable(BaseMightyMember mightyMember, T attribute)
        {
            var target = attribute.Target;
            
            var groupType = attribute.GetType();
            while (typeof(IInheritDrawer).IsAssignableFrom(groupType))
            {
                var baseType = groupType.BaseType;
                if (baseType == null || baseType.IsAbstract) break;

                groupType = baseType;
            }

            mightyMember.SetGroup(groupType, mightyMember.GetGroupName(attribute), attribute is BaseFoldGroupAttribute);

            var background = mightyMember.GetColorInfo(target, attribute.BackgroundColorName,
                attribute.BackgroundColor ?? attribute.GetDefaultBackgroundColor());
            var content = mightyMember.GetColorInfo(target, attribute.ContentColorName, attribute.ContentColor);

            m_colorsCache[mightyMember] = (background, content);
        }

        protected override void ClearCache()
        {
            m_indentLevelCache.ClearCache();
            m_colorsCache.ClearCache();
        }

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_colorsCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (background, content) = m_colorsCache[mightyMember];
            background.RefreshValue();
            content.RefreshValue();
        }
    }
}
#endif