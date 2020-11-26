#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IReloadScriptsDrawer : ISpecialDrawer
    {
        void BeginReloadScripts(MightyType mightyType, BaseReloadScriptsAttribute baseAttribute);
        void EndReloadScripts(MightyType mightyType, BaseReloadScriptsAttribute baseAttribute);
    }

    public abstract class BaseReloadScriptsDrawer<T> : BaseSpecialDrawer<T>, IReloadScriptsDrawer where T : BaseReloadScriptsAttribute
    {
        public void BeginReloadScripts(MightyType mightyType, BaseReloadScriptsAttribute baseAttribute) =>
            BeginReloadScripts(mightyType, (T) baseAttribute);

        public void EndReloadScripts(MightyType mightyType, BaseReloadScriptsAttribute baseAttribute) =>
            EndReloadScripts(mightyType, (T) baseAttribute);

        protected abstract void BeginReloadScripts(MightyType mightyType, T attribute);
        protected abstract void EndReloadScripts(MightyType mightyType, T attribute);
    }
}
#endif