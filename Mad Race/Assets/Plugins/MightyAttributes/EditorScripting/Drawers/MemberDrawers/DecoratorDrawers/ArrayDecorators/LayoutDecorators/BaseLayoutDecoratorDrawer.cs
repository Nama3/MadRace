#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public abstract class BaseLayoutDecoratorDrawer<T> : BaseArrayDecoratorDrawer<T>, IArrayDecoratorDrawer, IDrawAnywhereDecorator
        where T : BaseLayoutAttribute
    {
        #region Not Implemented

        void IArrayDecoratorDrawer.BeginDrawArray(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
        }

        void IArrayDecoratorDrawer.EndDrawArray(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
        }

        void IArrayDecoratorDrawer.BeginDrawHeader(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
        }

        void IArrayDecoratorDrawer.EndDrawHeader(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
        }

        Rect IArrayDecoratorDrawer.BeginDrawElement(Rect position, MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute) => position;

        Rect IArrayDecoratorDrawer.EndDrawElement(Rect position, MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute) => position;

        float IArrayDecoratorDrawer.GetElementHeight(MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute) => 0;


        protected override void DrawDecorator(BaseMightyMember mightyMember, T attribute)
        {
        }

        protected override void DrawDecoratorElement(MightySerializedField serializedField, int index, T attribute)
        {
        }

        protected override Rect DrawDecoratorElement(Rect position, MightySerializedField serializedField, int index, T attribute) =>
            position;

        protected override float GetDecoratorHeight(MightySerializedField serializedField, int index, T attribute) => 0;

        #endregion /Not Implemented

        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginLayout(mightyMember, (T) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndLayout(mightyMember, (T) baseAttribute);

        protected override void BeginDrawMember(MightySerializedField serializedField, T attribute,
            MightyDrawer.PropertyDrawCallback propertyDrawCallback = null, BasePropertyDrawerAttribute drawerAttribute = null)
        {
            var property = serializedField.Property;

            if (property.IsCollection())
            {
                MightyGUIUtilities.DrawArray(property, index =>
                {
                    BeginDrawElement(serializedField, index, attribute);
                    propertyDrawCallback?.Invoke(serializedField, property, drawerAttribute);
                    EndDrawElement(serializedField, index, attribute);
                });
                return;
            }

            BeginLayout(serializedField, attribute);
            propertyDrawCallback?.Invoke(serializedField, property, drawerAttribute);
        }

        public override void EndDrawMember(MightySerializedField serializedField, T attribute,
            MightyDrawer.PropertyDrawCallback propertyDrawCallback, BasePropertyDrawerAttribute drawerAttribute = null)
        {
            if (!serializedField.Property.IsCollection())
                EndLayout(serializedField, attribute);
        }

        void IArrayDecoratorDrawer.BeginDrawElement(MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute) => BeginLayout(serializedField, (T) baseAttribute);

        void IArrayDecoratorDrawer.EndDrawElement(MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute) => EndLayout(serializedField, (T) baseAttribute);

        protected abstract void BeginLayout(BaseMightyMember mightyMember, T attribute);
        protected abstract void EndLayout(BaseMightyMember mightyMember, T attribute);
    }
}
#endif