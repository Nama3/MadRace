using System;

namespace MightyAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public abstract class BaseNativePropertyAttribute : BaseMightyAttribute
    {
    }
}