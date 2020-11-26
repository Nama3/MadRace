#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IShowConditionDrawer : IMemberDrawer
    {
        bool CanDraw(BaseMightyMember mightyMember, BaseShowConditionAttribute attribute);
    }

    public abstract class BaseShowConditionDrawer<T> : BaseMemberDrawer<T>, IShowConditionDrawer where T : BaseShowConditionAttribute
    {
        public bool CanDraw(BaseMightyMember mightyMember, BaseShowConditionAttribute attribute) =>
            CanDraw(mightyMember, (T) attribute);

        protected abstract bool CanDraw(BaseMightyMember mightyMember, T attribute);
    }
}
#endif