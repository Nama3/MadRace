using System;

namespace MightyAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public abstract class BaseNonSerializedFieldAttribute : BaseMightyAttribute
    {
    }
}