#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IMightyDrawer
    {
        void EnableDrawer(BaseMightyMember mightyMember, BaseMightyAttribute attribute);
        void ClearDrawerCache();
    }
    
    public abstract class BaseMightyDrawer<T> : IMightyDrawer where T : BaseMightyAttribute
    {
        public void EnableDrawer(BaseMightyMember mightyMember, BaseMightyAttribute attribute) => Enable(mightyMember, (T) attribute);
        public void ClearDrawerCache() => ClearCache();

        protected abstract void Enable(BaseMightyMember mightyMember, T attribute);
        protected abstract void ClearCache();
    }
}
#endif