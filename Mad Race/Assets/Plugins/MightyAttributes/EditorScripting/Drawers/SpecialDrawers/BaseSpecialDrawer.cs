#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface ISpecialDrawer : IMightyDrawer
    {
    }
    
    public abstract class BaseSpecialDrawer<T> : BaseMightyDrawer<T>, ISpecialDrawer where T : BaseSpecialAttribute
    {
    }
}
#endif