#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface IScrollGrouper : ISimpleGrouper
    {
    }
    
    public abstract class BaseScrollGrouper<T> : BaseSimpleGrouper<T>, IScrollGrouper where T : BaseSimpleScrollGroupAttribute
    {
        private readonly Dictionary<string, (Vector2, float, float)> m_scrollGroupCache = new Dictionary<string, (Vector2, float, float)>();

        protected override void BeginGroup(BaseMightyMember mightyMember, T attribute)
        {
            var contentHeight = attribute.Height;
            var totalHeight = contentHeight;
            if (attribute.DrawName) totalHeight += EditorGUIUtility.singleLineHeight;
            if (attribute.DrawLine) totalHeight += MightyStyleUtilities.GetLineHeight(false);

            var groupId = mightyMember.GroupID;
            if (!m_scrollGroupCache.ContainsKey(groupId)) m_scrollGroupCache[groupId] = (Vector2.zero, contentHeight, totalHeight);

            base.BeginGroup(mightyMember, attribute);
        }

        public override void BeginDrawGroup(int indentLevel = 0, string groupID = null, bool indentInside = true)
        {
            var totalHeight = 120f;
            if (groupID != null)
                totalHeight = m_scrollGroupCache[groupID].Item3;

            GUILayout.BeginVertical(GetGroupStyle(indentLevel), GUILayout.Height(totalHeight));

            if (indentInside)
                EditorGUI.indentLevel = 1;
        }

        public abstract GUIStyle GetGroupStyle(int indentLevel);

        public override void BeginDrawContent(int indentLevel = 0, string groupID = null)
        {
            var (position, contentHeight, totalHeight) = (Vector2.zero, 100f, 120f);
            if (groupID != null)
                (position, contentHeight, totalHeight) = m_scrollGroupCache[groupID];

            position = MightyGUIUtilities.BeginDrawScrollView(position);

            if (groupID != null)
                m_scrollGroupCache[groupID] = (position, contentHeight, totalHeight);
        }

        public override void EndDrawContent(int indentLevel = 0) => MightyGUIUtilities.EndDrawScrollView();

        public override void EndDrawGroup(int indentLevel = 0)
        {
            EditorGUI.indentLevel = indentLevel;

            GUILayout.EndVertical();
        }
    }
}
#endif