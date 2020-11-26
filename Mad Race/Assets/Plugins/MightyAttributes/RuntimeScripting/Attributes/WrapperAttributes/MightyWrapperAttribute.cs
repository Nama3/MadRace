using System;

namespace MightyAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property |AttributeTargets.Method)]
    public abstract class BaseWrapperAttribute : BaseMightyAttribute
    {
    }
}