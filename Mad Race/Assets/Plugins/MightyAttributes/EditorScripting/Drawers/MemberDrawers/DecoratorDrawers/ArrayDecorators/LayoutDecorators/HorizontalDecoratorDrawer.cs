#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class HorizontalDecoratorDrawer : BaseLayoutDecoratorDrawer<HorizontalAttribute>
    {
        protected override void BeginLayout(BaseMightyMember mightyMember, HorizontalAttribute baseAttribute) =>
            GUILayout.BeginHorizontal();

        protected override void EndLayout(BaseMightyMember mightyMember, HorizontalAttribute baseAttribute) => GUILayout.EndHorizontal();

        protected override void Enable(BaseMightyMember mightyMember, HorizontalAttribute mightyAttribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif