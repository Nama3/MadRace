using System;

namespace MightyAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseHierarchyAttribute : BaseSpecialAttribute
    {
        public int Priority { get; }
        public string PriorityCallback { get; }
        
        protected BaseHierarchyAttribute(int priority) => Priority = priority;

        protected BaseHierarchyAttribute(string priorityCallback) => PriorityCallback = priorityCallback;
    }
}