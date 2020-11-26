#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class StyleDecoratorDrawer : BaseDecoratorDrawer<StyleAttribute>, IDrawAnywhereDecorator, IRefreshDrawer
    {
        private readonly MightyCache<int> m_indentLevelCache = new MightyCache<int>();
        private readonly MightyCache<(bool, MightyInfo<GUIStyle>)> m_styleCache = new MightyCache<(bool, MightyInfo<GUIStyle>)>();

        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (StyleAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (StyleAttribute) baseAttribute);

        protected override void BeginDraw(BaseMightyMember mightyMember, StyleAttribute attribute)
        {
            if (!m_styleCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);

            if (m_styleCache[mightyMember].Item1) EnableDrawer(mightyMember, attribute);
            var style = m_styleCache[mightyMember].Item2?.Value;

            if (style == null) return;

            if (mightyMember.IsFoldable())
            {
                GUILayout.BeginVertical(style.IndentStyle(IndentLevelForMember(mightyMember)));
                EditorGUI.indentLevel = 1;
            }
            else
                GUILayout.BeginVertical(style);
        }

        protected override void EndDraw(BaseMightyMember mightyMember, StyleAttribute attribute)
        {
            if (!m_styleCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);

            if (m_styleCache[mightyMember].Item1) EnableDrawer(mightyMember, attribute);
            var style = m_styleCache[mightyMember].Item2?.Value;

            if (style == null) return;

            if (mightyMember.IsFoldable())
                EditorGUI.indentLevel = IndentLevelForMember(mightyMember);

            GUILayout.EndVertical();
        }

        private int IndentLevelForMember(BaseMightyMember mightyMember) =>
            !m_indentLevelCache.TryGetValue(mightyMember, out var value)
                ? m_indentLevelCache[mightyMember] = EditorGUI.indentLevel
                : value;

        protected override void Enable(BaseMightyMember mightyMember, StyleAttribute attribute)
        {
            var styleInfo = mightyMember.GetStyleInfo(attribute.Target, attribute.StyleName, out var exception);

            m_styleCache[mightyMember] = (exception, styleInfo);
        }

        protected override void ClearCache()
        {
            m_styleCache.ClearCache();
            m_indentLevelCache.ClearCache();
        }

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_styleCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            if (m_styleCache[mightyMember].Item1)
                EnableDrawer(mightyMember, mightyAttribute);

            m_styleCache[mightyMember].Item2?.RefreshValue();
        }
    }
}
#endif