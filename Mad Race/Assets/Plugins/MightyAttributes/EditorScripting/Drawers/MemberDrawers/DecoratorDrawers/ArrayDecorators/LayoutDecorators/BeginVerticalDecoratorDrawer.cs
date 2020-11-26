#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class BeginVerticalDecoratorDrawer : BaseLayoutDecoratorDrawer<BeginVerticalAttribute>
    {
        protected override void BeginLayout(BaseMightyMember mightyMember, BeginVerticalAttribute baseAttribute) => GUILayout.BeginVertical();

        protected override void EndLayout(BaseMightyMember mightyMember, BeginVerticalAttribute baseAttribute)
        {
        }

        protected override void Enable(BaseMightyMember mightyMember, BeginVerticalAttribute mightyAttribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif