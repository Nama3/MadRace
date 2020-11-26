#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class VerticalDecoratorDrawer : BaseLayoutDecoratorDrawer<VerticalAttribute>
    {
        protected override void BeginLayout(BaseMightyMember mightyMember, VerticalAttribute baseAttribute) => GUILayout.BeginVertical();

        protected override void EndLayout(BaseMightyMember mightyMember, VerticalAttribute baseAttribute) => GUILayout.EndVertical();

        protected override void Enable(BaseMightyMember mightyMember, VerticalAttribute mightyAttribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif