#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class ReadOnlyDecoratorDrawer : BaseEnableConditionDrawer<ReadOnlyAttribute>
    {
        protected override void BeginEnable(BaseMightyMember mightyMember, ReadOnlyAttribute attribute) => EditorGUI.BeginDisabledGroup(true);

        protected override void EndEnable(BaseMightyMember mightyMember, ReadOnlyAttribute attribute) => EditorGUI.EndDisabledGroup();

        protected override void Enable(BaseMightyMember mightyMember, ReadOnlyAttribute attribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif