#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ScriptFieldDecoratorDrawer : BasePositionDecoratorDrawer<ScriptFieldAttribute>, IDrawAnywhereDecorator
    {
        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (ScriptFieldAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (ScriptFieldAttribute) baseAttribute);

        protected override void DrawDecorator(BaseMightyMember mightyMember, ScriptFieldAttribute attribute)
        {
            var enabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.PropertyField(mightyMember.Context.ScriptProperty);
            GUI.enabled = enabled;
        }
    }
}
#endif