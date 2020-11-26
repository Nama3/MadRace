#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class ShowIfDrawer : BaseShowConditionDrawer<ShowIfAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<(bool, bool, MightyInfo<bool>[])> m_showIfCache = new MightyCache<(bool, bool, MightyInfo<bool>[])>();

        protected override bool CanDraw(BaseMightyMember mightyMember, ShowIfAttribute attribute)
        {
            if (!m_showIfCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, canDraw, _) = m_showIfCache[mightyMember];

            if (valid) return canDraw;

            MightyGUIUtilities.DrawHelpBox(
                $"{mightyMember.MemberName}'s ShowIf needs a valid boolean condition field or method name to work");

            return true;
        }

        protected override void Enable(BaseMightyMember mightyMember, ShowIfAttribute attribute)
        {
            var target = attribute.Target;

            var canDraw = true;
            var valid = false;
            var infos = new MightyInfo<bool>[attribute.ConditionCallbacks.Length];
            for (var i = 0; i < attribute.ConditionCallbacks.Length; i++)
            {
                var conditionName = attribute.ConditionCallbacks[i];
                if (!mightyMember.GetBoolInfo(target, conditionName, out infos[i])) continue;
                canDraw = canDraw && infos[i].Value;
                valid = true;
            }

            m_showIfCache[mightyMember] = (valid, canDraw, infos);
        }

        protected override void ClearCache() => m_showIfCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_showIfCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, _, infos) = m_showIfCache[mightyMember];
            if (!valid) return;

            var canDraw = true;
            foreach (var info in infos) canDraw = canDraw && info.RefreshValue();

            m_showIfCache[mightyMember] = (true, canDraw, infos);
        }
    }
}
#endif