#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class BeginHorizontalDecoratorDrawer : BaseLayoutDecoratorDrawer<BeginHorizontalAttribute>
    {
        protected override void BeginLayout(BaseMightyMember mightyMember, BeginHorizontalAttribute baseAttribute) => 
            GUILayout.BeginHorizontal();

        protected override void EndLayout(BaseMightyMember mightyMember, BeginHorizontalAttribute baseAttribute)
        {
        }

        protected override void Enable(BaseMightyMember mightyMember, BeginHorizontalAttribute mightyAttribute)
        {
        }

        protected override void ClearCache()
        {
        }
    }
}
#endif