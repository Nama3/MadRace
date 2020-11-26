#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IDecoratorDrawer : IGlobalDecoratorDrawer
    {
        void BeginDraw(BaseMightyMember mightyMember, BaseDecoratorAttribute baseAttribute);
        void EndDraw(BaseMightyMember mightyMember, BaseDecoratorAttribute baseAttribute);
    }

    public abstract class BaseDecoratorDrawer<T> : BaseGlobalDecoratorDrawer<T>, IDecoratorDrawer where T : BaseDecoratorAttribute
    {
        public void BeginDraw(BaseMightyMember mightyMember, BaseDecoratorAttribute baseAttribute) =>
            BeginDraw(mightyMember, (T) baseAttribute);

        public void EndDraw(BaseMightyMember mightyMember, BaseDecoratorAttribute baseAttribute) =>
            EndDraw(mightyMember, (T) baseAttribute);

        protected abstract void BeginDraw(BaseMightyMember mightyMember, T attribute);
        protected abstract void EndDraw(BaseMightyMember mightyMember, T attribute);
    }
}
#endif