#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IGlobalDecoratorDrawer : IMemberDrawer
    {
    }

    public abstract class BaseGlobalDecoratorDrawer<T> : BaseMemberDrawer<T>, IGlobalDecoratorDrawer where T : BaseGlobalDecoratorAttribute
    {
    }
}
#endif