using System;

namespace MightyAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public abstract class BaseMethodAttribute : BaseMightyAttribute
    {
        public bool ExecuteInPlayMode { get; }
        
        protected BaseMethodAttribute(bool executeInPlayMode) => ExecuteInPlayMode = executeInPlayMode;
    }
}