using System;

namespace MightyAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public abstract class BaseChangeCheckAttribute : BaseSerializedFieldAttribute
    {
        public bool ExecuteInEditMode { get; }
        
        protected BaseChangeCheckAttribute(bool executeInPlayMode) => ExecuteInEditMode = executeInPlayMode;
    }
}
