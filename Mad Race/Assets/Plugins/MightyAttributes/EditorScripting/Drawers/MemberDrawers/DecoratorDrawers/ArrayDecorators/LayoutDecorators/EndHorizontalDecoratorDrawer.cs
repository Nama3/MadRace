#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class EndHorizontalDecoratorDrawer : BaseLayoutDecoratorDrawer<EndHorizontalAttribute>
    {
        protected override void BeginLayout(BaseMightyMember mightyMember, EndHorizontalAttribute baseAttribute)
        {
        }

        protected override void EndLayout(BaseMightyMember mightyMember, EndHorizontalAttribute baseAttribute) => GUILayout.EndHorizontal();

        protected override void Enable(BaseMightyMember mightyMember, EndHorizontalAttribute mightyAttribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif