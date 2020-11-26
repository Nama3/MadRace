#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IMemberDrawer : IMightyDrawer
    {
    }
    
    public abstract class BaseMemberDrawer<T> : BaseMightyDrawer<T>, IMemberDrawer where T : BaseMemberAttribute
    {
    }
}
#endif