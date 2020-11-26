#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class EnableIfDecoratorDrawer : BaseEnableConditionDrawer<EnableIfAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<(bool, bool, MightyInfo<bool>[])> m_enableIfCache =
            new MightyCache<(bool, bool, MightyInfo<bool>[])>();

        protected override void BeginEnable(BaseMightyMember mightyMember, EnableIfAttribute attribute)
        {
            if (!m_enableIfCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, enabled, _) = m_enableIfCache[mightyMember];


            if (valid)
                EditorGUI.BeginDisabledGroup(!enabled);
            else
                MightyGUIUtilities.DrawHelpBox($"{nameof(EnableIfAttribute)} needs a valid boolean condition to work");
        }

        protected override void EndEnable(BaseMightyMember mightyMember, EnableIfAttribute attribute) => 
            EditorGUI.EndDisabledGroup();

        protected override void Enable(BaseMightyMember mightyMember, EnableIfAttribute attribute)
        {
            var target = attribute.Target;

            var enabled = true;
            var valid = false;
            var infos = new MightyInfo<bool>[attribute.ConditionCallbacks.Length];
            for (var i = 0; i < attribute.ConditionCallbacks.Length; i++)
            {
                var conditionName = attribute.ConditionCallbacks[i];
                if (!mightyMember.GetBoolInfo(target, conditionName, out infos[i])) continue;
                enabled = enabled && infos[i].Value;
                valid = true;
            }

            m_enableIfCache[mightyMember] = (valid, enabled, infos);
        }

        protected override void ClearCache() => m_enableIfCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_enableIfCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, _, infos) = m_enableIfCache[mightyMember];
            if (!valid) return;

            var enabled = true;
            foreach (var info in infos) enabled = enabled && info.RefreshValue();

            m_enableIfCache[mightyMember] = (true, enabled, infos);
        }
    }
}
#endif