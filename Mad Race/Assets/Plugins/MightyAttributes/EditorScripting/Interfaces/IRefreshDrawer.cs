#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IRefreshDrawer
    {
        void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute attribute);
    }
}
#endif