#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class EndVerticalDecoratorDrawer : BaseLayoutDecoratorDrawer<EndVerticalAttribute>
    {
        protected override void BeginLayout(BaseMightyMember mightyMember, EndVerticalAttribute baseAttribute)
        {
        }

        protected override void EndLayout(BaseMightyMember mightyMember, EndVerticalAttribute baseAttribute) => GUILayout.EndVertical();

        protected override void Enable(BaseMightyMember mightyMember, EndVerticalAttribute mightyAttribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif