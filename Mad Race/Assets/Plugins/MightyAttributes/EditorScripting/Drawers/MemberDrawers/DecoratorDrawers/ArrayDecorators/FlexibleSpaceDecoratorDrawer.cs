#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FlexibleSpaceDecoratorDrawer : BaseArrayDecoratorDrawer<FlexibleSpaceAttribute>, IDrawAnywhereDecorator
    {
        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (FlexibleSpaceAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (FlexibleSpaceAttribute) baseAttribute);

        protected override void DrawDecorator(BaseMightyMember mightyMember, FlexibleSpaceAttribute attribute) =>
            GUILayout.FlexibleSpace();

        protected override void DrawDecoratorElement(MightySerializedField serializedField, int index, FlexibleSpaceAttribute attribute) =>
            GUILayout.FlexibleSpace();

        protected override Rect DrawDecoratorElement(Rect position, MightySerializedField serializedField, int index,
            FlexibleSpaceAttribute attribute) => position;

        protected override float GetDecoratorHeight(MightySerializedField serializedField, int index, FlexibleSpaceAttribute attribute) =>
            0;
    }
}
#endif