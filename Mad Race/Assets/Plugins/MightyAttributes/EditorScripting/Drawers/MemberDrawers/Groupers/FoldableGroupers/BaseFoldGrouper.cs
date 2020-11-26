#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IFoldGrouper : IGrouper
    {
        bool CanDraw(BaseMightyMember mightyMember, BaseFoldGroupAttribute attribute);
        void BeginGroup(BaseMightyMember mightyMember);
        void EndGroup(BaseMightyMember mightyMember, bool canDraw);
    }

    public abstract class BaseFoldGrouper<T> : BaseGrouper<T>, IFoldGrouper where T : BaseFoldGroupAttribute
    {
        private readonly MightyCache<string, bool> m_foldoutStateCache = new MightyCache<string, bool>();

        protected abstract bool BeginFoldout(bool foldout, string label, int indentLevel, T attribute);

        public bool CanDraw(BaseMightyMember mightyMember, BaseFoldGroupAttribute attribute)
        {
            if (!m_colorsCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (background, content) = m_colorsCache[mightyMember];

            MightyColorUtilities.BeginBackgroundColor(background.Value);
            MightyColorUtilities.BeginContentColor(content.Value);

            return m_foldoutStateCache[mightyMember, mightyMember.GroupID] =
                BeginFoldout(GetFoldoutStateByProperty(mightyMember), attribute.DrawName ? mightyMember.GroupName : "",
                    IndentLevelForMember(mightyMember), (T) attribute);
        }

        public void BeginGroup(BaseMightyMember mightyMember) => BeginGroupImpl(IndentLevelForMember(mightyMember));

        public void EndGroup(BaseMightyMember mightyMember, bool canDraw)
        {
            if (!canDraw) return;

            EndGroupImpl(IndentLevelForMember(mightyMember));
            MightyColorUtilities.EndBackgroundColor();
            MightyColorUtilities.EndContentColor();
        }

        protected abstract void BeginGroupImpl(int indentLevel);

        protected abstract void EndGroupImpl(int indentLevel);

        protected bool GetFoldoutStateByProperty(BaseMightyMember mightyMember)
        {
            if (m_foldoutStateCache.TryGetValue(mightyMember, mightyMember.GroupID, out var value)) return value;

            m_foldoutStateCache[mightyMember, mightyMember.GroupID] = false;
            return false;
        }
    }
}
#endif