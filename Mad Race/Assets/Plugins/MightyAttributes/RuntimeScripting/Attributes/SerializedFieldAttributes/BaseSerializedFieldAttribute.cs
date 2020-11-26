using System;

namespace MightyAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public abstract class BaseSerializedFieldAttribute : BaseMightyAttribute
    {
    }
}