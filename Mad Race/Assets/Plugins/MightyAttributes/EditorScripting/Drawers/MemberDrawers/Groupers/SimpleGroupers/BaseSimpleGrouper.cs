#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface ISimpleGrouper : IGrouper
    {
        void BeginGroup(BaseMightyMember mightyMember, BaseSimpleGroupAttribute attribute);
        void EndGroup(BaseMightyMember mightyMember, BaseSimpleGroupAttribute attribute);
    }

    public abstract class BaseSimpleGrouper<T> : BaseGrouper<T>, ISimpleGrouper where T : BaseSimpleGroupAttribute
    {
        public void BeginGroup(BaseMightyMember mightyMember, BaseSimpleGroupAttribute attribute) => BeginGroup(mightyMember, (T) attribute);

        public void EndGroup(BaseMightyMember mightyMember, BaseSimpleGroupAttribute attribute) => EndGroup(mightyMember, (T) attribute);

        protected virtual void BeginGroup(BaseMightyMember mightyMember, T attribute)
        {
            if (!m_colorsCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (background, content) = m_colorsCache[mightyMember];

            MightyColorUtilities.BeginBackgroundColor(background.Value);
            MightyColorUtilities.BeginContentColor(content.Value);

            var indent = IndentLevelForMember(mightyMember);
            var groupID = mightyMember.GroupID;

            BeginDrawGroup(indent, groupID);

            if (attribute.DrawName) DrawGroupLabel(mightyMember.GroupName);
            if (attribute.DrawLine) DrawLine(attribute.LineColor);

            BeginDrawContent(indent, groupID);
        }

        protected virtual void EndGroup(BaseMightyMember mightyMember, T attribute)
        {
            var indent = IndentLevelForMember(mightyMember);

            EndDrawContent(indent);
            EndDrawGroup(indent);

            MightyColorUtilities.EndBackgroundColor();
            MightyColorUtilities.EndContentColor();
        }

        public virtual void DrawGroupLabel(string label = null, GUIStyle style = null)
        {
            if (!string.IsNullOrEmpty(label))
                EditorGUILayout.LabelField(label, style ?? MightyStyles.BoxGroupLabel);
        }

        public abstract void BeginDrawGroup(int indentLevel = 0, string groupID = null, bool indentInside = true);

        public abstract void EndDrawGroup(int indentLevel = 0);

        public virtual void BeginDrawContent(int indentLevel = 0, string groupID = null)
        {
        }

        public virtual void EndDrawContent(int indentLevel = 0)
        {
        }
    }
}
#endif