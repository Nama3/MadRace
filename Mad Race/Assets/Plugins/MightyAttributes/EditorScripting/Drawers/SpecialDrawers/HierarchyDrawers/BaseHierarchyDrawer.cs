#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface IHierarchyDrawer : ISpecialDrawer
    {
        void OnHierarchyChanged(MightyComponent mightyComponent, BaseHierarchyAttribute baseAttribute);
        void OnGUI(MightyComponent mightyComponent, Rect selectionRect, BaseHierarchyAttribute baseAttribute);
    }

    public abstract class BaseHierarchyDrawer<T> : BaseSpecialDrawer<T>, IHierarchyDrawer, IRefreshDrawer where T : BaseHierarchyAttribute
    {
        private readonly Dictionary<GameObject, MightyInfo<int>> m_priorityCache = new Dictionary<GameObject, MightyInfo<int>>();


        public void OnHierarchyChanged(MightyComponent mightyComponent, BaseHierarchyAttribute baseAttribute) => 
            OnHierarchyChanged(mightyComponent, (T)baseAttribute);

        public void OnGUI(MightyComponent mightyComponent, Rect selectionRect, BaseHierarchyAttribute baseAttribute) => 
            OnGUI(mightyComponent, selectionRect, (T) baseAttribute);

        protected abstract void OnHierarchyChanged(MightyComponent mightyComponent, T attribute);

        protected abstract void OnGUI(MightyComponent mightyComponent, Rect selectionRect, T attribute);

        protected override void Enable(BaseMightyMember mightyMember, T attribute)
        {
            if (!(mightyMember is MightyComponent mightyComponent)) return;

            var gameObject = mightyComponent.ComponentContext.GameObject;

            if (!mightyMember.GetInfoFromMember<int>(attribute.Target, attribute.PriorityCallback, out var priorityInfo))
                priorityInfo = new MightyInfo<int>(attribute.Priority);

            if (m_priorityCache.TryGetValue(gameObject, out var priority) && priority.Value >= priorityInfo.Value) return;

            m_priorityCache[gameObject] = priorityInfo;
            EnableDrawerImpl(mightyComponent, attribute);
        }

        protected abstract void EnableDrawerImpl(MightyComponent mightyComponent, T attribute);

        protected override void ClearCache() => m_priorityCache.Clear();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            var gameObject = ((MightyComponent) mightyMember).ComponentContext.GameObject;
            if (!m_priorityCache.ContainsKey(gameObject))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_priorityCache[gameObject].RefreshValue();
        }
    }
}
#endif