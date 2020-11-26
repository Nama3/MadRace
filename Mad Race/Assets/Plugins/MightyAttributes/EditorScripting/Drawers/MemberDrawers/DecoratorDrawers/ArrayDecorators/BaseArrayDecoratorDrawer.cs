#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface IArrayDecoratorDrawer : IGlobalDecoratorDrawer
    {
        ArrayDecoratorPosition PositionByMember(BaseMightyMember mightyMember, BaseArrayDecoratorAttribute baseAttribute);
        
        void BeginDrawMember(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute,
            MightyDrawer.PropertyDrawCallback propertyDrawCallback = null, BasePropertyDrawerAttribute drawerAttribute = null);

        void EndDrawMember(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute,
            MightyDrawer.PropertyDrawCallback propertyDrawCallback = null, BasePropertyDrawerAttribute drawerAttribute = null);

        void BeginDrawArray(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute);
        void EndDrawArray(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute);
        
        void BeginDrawHeader(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute);
        void EndDrawHeader(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute);
        
        void BeginDrawElement(MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute);
        void EndDrawElement(MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute);  
        
        Rect BeginDrawElement(Rect position, MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute);
        Rect EndDrawElement(Rect position, MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute);
        
        float GetElementHeight(MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute);
    }

    public abstract class BaseArrayDecoratorDrawer<T> : BaseGlobalDecoratorDrawer<T>, IArrayDecoratorDrawer, IRefreshDrawer
        where T : BaseArrayDecoratorAttribute
    {
        private readonly MightyCache<MightyInfo<ArrayDecoratorPosition>> m_positionCache =
            new MightyCache<MightyInfo<ArrayDecoratorPosition>>();

        #region Interface Methods

        public void BeginDrawMember(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute,
            MightyDrawer.PropertyDrawCallback propertyDrawCallback = null, BasePropertyDrawerAttribute drawerAttribute = null) =>
            BeginDrawMember(serializedField, (T) baseAttribute, propertyDrawCallback, drawerAttribute);

        public void EndDrawMember(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute,
            MightyDrawer.PropertyDrawCallback propertyDrawCallback = null, BasePropertyDrawerAttribute drawerAttribute = null) =>
            EndDrawMember(serializedField, (T) baseAttribute, propertyDrawCallback, drawerAttribute);

        public void BeginDrawArray(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute) => 
            BeginDraw(serializedField, (T) baseAttribute);

        public void EndDrawArray(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute) => 
            EndDraw(serializedField, (T) baseAttribute);

        public void BeginDrawHeader(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
            if (PositionByMember(serializedField, baseAttribute).Contains(ArrayDecoratorPosition.BeforeHeader))
                DrawDecorator(serializedField, (T) baseAttribute);
        }

        public void EndDrawHeader(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
            var position = PositionByMember(serializedField, baseAttribute);
            if (position.Contains(ArrayDecoratorPosition.AfterHeader) ||
                position.Contains(ArrayDecoratorPosition.AfterHeaderFoldout) && serializedField.IsExpanded &&
                serializedField.ArraySize > 0)
                DrawDecorator(serializedField, (T) baseAttribute);
        }
        
        public void BeginDrawElement(MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute)
        {
            var attribute = (T) baseAttribute;
            
            var position = PositionByMember(serializedField, attribute);

            if (position.Contains(ArrayDecoratorPosition.BeforeElements))
                DrawDecoratorElement(serializedField, index, attribute);

            if (index != 0 && position.Contains(ArrayDecoratorPosition.BetweenElements))
                DrawDecoratorElement(serializedField, index, attribute);
        }

        public void EndDrawElement(MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute)
        {
            if (PositionByMember(serializedField, baseAttribute).Contains(ArrayDecoratorPosition.AfterElements))
                DrawDecoratorElement(serializedField, index, (T) baseAttribute);
        }

        public Rect BeginDrawElement(Rect rect, MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute)
        {
            var attribute = (T) baseAttribute;
            
            var position = PositionByMember(serializedField, attribute);

            if (position.Contains(ArrayDecoratorPosition.BeforeElements))
                rect = DrawDecoratorElement(rect, serializedField, index, attribute);

            if (index != 0 && position.Contains(ArrayDecoratorPosition.BetweenElements))
                rect = DrawDecoratorElement(rect, serializedField, index, attribute);

            return rect;
        }

        public Rect EndDrawElement(Rect rect, MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute)
        {
            if (PositionByMember(serializedField, baseAttribute).Contains(ArrayDecoratorPosition.AfterElements))
                rect = DrawDecoratorElement(rect, serializedField, index, (T) baseAttribute);

            return rect;
        }

        public float GetElementHeight(MightySerializedField serializedField, int index, BaseArrayDecoratorAttribute baseAttribute)
        {
            var position = PositionByMember(serializedField, baseAttribute);
            var decoratorHeight = GetDecoratorHeight(serializedField, index, (T) baseAttribute);
            var elementHeight = 0f;

            if (position.Contains(ArrayDecoratorPosition.BeforeElements))
                elementHeight += decoratorHeight;

            if (index != 0 && position.Contains(ArrayDecoratorPosition.BetweenElements))
                elementHeight += decoratorHeight;

            if (position.Contains(ArrayDecoratorPosition.AfterElements))
                elementHeight += decoratorHeight;

            return elementHeight;
        }

        #endregion Interface Methods

        protected virtual void BeginDrawMember(MightySerializedField serializedField, T attribute,
            MightyDrawer.PropertyDrawCallback propertyDrawCallback = null, BasePropertyDrawerAttribute drawerAttribute = null)
        {
            var property = serializedField.Property;
            if (!property.IsCollection())
            {
                BeginDraw(serializedField, attribute);

                propertyDrawCallback?.Invoke(serializedField, property, drawerAttribute);
                return;
            }

            BeginDrawArray(serializedField, attribute);
            BeginDrawHeader(serializedField, attribute);

            if (!MightyGUIUtilities.DrawFoldout(property))
            {
                EndDrawHeader(serializedField, attribute);
                EndDrawArray(serializedField, attribute);
                return;
            }

            EditorGUI.indentLevel++;
            MightyGUIUtilities.DrawArraySizeField(property);

            EndDrawHeader(serializedField, attribute);
        }

        public virtual void EndDrawMember(MightySerializedField serializedField, T attribute,
            MightyDrawer.PropertyDrawCallback propertyDrawCallback, BasePropertyDrawerAttribute drawerAttribute = null)
        {
            var property = serializedField.Property;
            if (!property.IsCollection())
            {
                EndDraw(serializedField, attribute);
                return;
            }

            if (!property.isExpanded) return;

            MightyGUIUtilities.DrawArrayBody(property, index =>
            {
                BeginDrawElement(serializedField, index, attribute);
                propertyDrawCallback?.Invoke(serializedField, property.GetArrayElementAtIndex(index), drawerAttribute);
                EndDrawElement(serializedField, index, attribute);
            });

            EditorGUI.indentLevel--;

            EndDrawArray(serializedField, attribute);
        }

        public virtual void BeginDraw(BaseMightyMember mightyMember, T attribute)
        {
            if (PositionByMember(mightyMember, attribute).Contains(ArrayDecoratorPosition.Before))
                DrawDecorator(mightyMember, attribute);
        }

        public virtual void EndDraw(BaseMightyMember mightyMember, T attribute)
        {
            if (PositionByMember(mightyMember, attribute).Contains(ArrayDecoratorPosition.After))
                DrawDecorator(mightyMember, attribute);
        }
        
        protected abstract void DrawDecorator(BaseMightyMember mightyMember, T attribute);

        protected abstract void DrawDecoratorElement(MightySerializedField serializedField, int index, T attribute);

        protected abstract Rect DrawDecoratorElement(Rect position, MightySerializedField serializedField, int index, T attribute);

        protected abstract float GetDecoratorHeight(MightySerializedField serializedField, int index, T attribute);

        public ArrayDecoratorPosition PositionByMember(BaseMightyMember mightyMember, BaseArrayDecoratorAttribute baseAttribute)
        {
            if (!m_positionCache.Contains(mightyMember)) EnableDrawer(mightyMember, baseAttribute);
            return m_positionCache[mightyMember].Value;
        }

        protected override void Enable(BaseMightyMember mightyMember, T attribute)
        {
            if (!mightyMember.GetInfoFromMember<ArrayDecoratorPosition>(attribute.Target, attribute.PositionCallback, out var positionInfo,
                Enum.TryParse)) positionInfo = new MightyInfo<ArrayDecoratorPosition>(attribute.Position);

            m_positionCache[mightyMember] = positionInfo;
        }

        protected override void ClearCache() => m_positionCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_positionCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_positionCache[mightyMember].RefreshValue();
        }
    }
}
#endif