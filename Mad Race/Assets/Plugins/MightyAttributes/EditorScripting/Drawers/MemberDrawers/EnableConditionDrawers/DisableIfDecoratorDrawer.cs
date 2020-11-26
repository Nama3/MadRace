#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class DisableIfDecoratorDrawer : BaseEnableConditionDrawer<DisableIfAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<(bool, bool, MightyInfo<bool>[])> m_disableIfCache =
            new MightyCache<(bool, bool, MightyInfo<bool>[])>();

        protected override void BeginEnable(BaseMightyMember mightyMember, DisableIfAttribute attribute)
        {
            if (!m_disableIfCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (valid, disabled, _) = m_disableIfCache[mightyMember];

            if (valid)
                EditorGUI.BeginDisabledGroup(disabled);
            else
                MightyGUIUtilities.DrawHelpBox($"{nameof(DisableIfAttribute)} needs a valid boolean condition to work");
        }

        protected override void EndEnable(BaseMightyMember mightyMember, DisableIfAttribute attribute) =>
            EditorGUI.EndDisabledGroup();

        protected override void Enable(BaseMightyMember mightyMember, DisableIfAttribute attribute)
        {
            var target = attribute.Target;

            var disabled = false;
            var valid = false;
            var infos = new MightyInfo<bool>[attribute.ConditionCallbacks.Length];
            for (var i = 0; i < attribute.ConditionCallbacks.Length; i++)
            {
                var conditionName = attribute.ConditionCallbacks[i];
                if (!mightyMember.GetBoolInfo(target, conditionName, out infos[i])) continue;
                disabled = disabled || infos[i].Value;
                valid = true;
            }

            m_disableIfCache[mightyMember] = (valid, disabled, infos);
        }

        protected override void ClearCache() => m_disableIfCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_disableIfCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, _, infos) = m_disableIfCache[mightyMember];
            if (!valid) return;

            var disabled = false;
            foreach (var info in infos) disabled = disabled || info.RefreshValue();

            m_disableIfCache[mightyMember] = (true, disabled, infos);
        }
    }
}
#endif