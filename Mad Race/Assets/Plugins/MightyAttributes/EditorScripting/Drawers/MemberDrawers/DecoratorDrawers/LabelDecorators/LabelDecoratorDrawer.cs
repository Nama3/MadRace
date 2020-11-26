#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class LabelDecoratorDrawer : BaseLabelDecoratorDrawer<LabelAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<(bool, MightyInfo<GUIStyle>)> m_styleCache = new MightyCache<(bool, MightyInfo<GUIStyle>)>();

        protected override void DrawLabel(BaseMightyMember mightyMember, LabelAttribute attribute, string prefix, string label)
        {
            if (!m_styleCache.Contains(mightyMember)) Enable(mightyMember, attribute);

            if (m_styleCache[mightyMember].Item1) Enable(mightyMember, attribute);
            var style = m_styleCache[mightyMember].Item2?.Value;

            if (string.IsNullOrEmpty(prefix))
            {
                if (style == null)
                    EditorGUILayout.LabelField(label, GUILayout.Width(MightyGUIUtilities.TextWidth(label)));
                else
                    EditorGUILayout.LabelField(label, style,
                        GUILayout.Width(MightyGUIUtilities.TextWidth(label)));
            }
            else
            {
                if (style == null)
                    EditorGUILayout.LabelField(prefix, label);
                else
                    EditorGUILayout.LabelField(prefix, label, style);
            }
        }

        protected override void Enable(BaseMightyMember mightyMember, LabelAttribute attribute)
        {
            base.Enable(mightyMember, attribute);

            var styleInfo = mightyMember.GetStyleInfo(attribute.Target, attribute.StyleName, out var exception);

            m_styleCache[mightyMember] = (exception, styleInfo);
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            m_styleCache.ClearCache();
        }

        void IRefreshDrawer.RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            base.RefreshDrawer(mightyMember, mightyAttribute);
            
            if (!m_styleCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (exception, _) = m_styleCache[mightyMember];

            if (exception) EnableDrawer(mightyMember, mightyAttribute);
            m_styleCache[mightyMember].Item2?.RefreshValue();
        }
    }
}
#endif