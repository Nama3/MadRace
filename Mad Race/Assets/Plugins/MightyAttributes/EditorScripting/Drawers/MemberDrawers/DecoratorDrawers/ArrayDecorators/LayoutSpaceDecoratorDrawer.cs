#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class LayoutSpaceDecoratorDrawer : BaseArrayDecoratorDrawer<LayoutSpaceAttribute>, IDrawAnywhereDecorator
    {
        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (LayoutSpaceAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (LayoutSpaceAttribute) baseAttribute);

        protected override void DrawDecorator(BaseMightyMember mightyMember, LayoutSpaceAttribute attribute) =>
            GUILayout.Space(attribute.Size);

        protected override void DrawDecoratorElement(MightySerializedField serializedField, int index, LayoutSpaceAttribute attribute) =>
            GUILayout.Space(attribute.Size);

        protected override Rect DrawDecoratorElement(Rect position, MightySerializedField serializedField, int index,
            LayoutSpaceAttribute attribute) => new Rect(position.x, position.y + attribute.Size, position.width, position.height);

        protected override float GetDecoratorHeight(MightySerializedField serializedField, int index, LayoutSpaceAttribute attribute) =>
            attribute.Size;
    }
}
#endif