#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public interface IEnableConditionDrawer : IMemberDrawer
    {
        void BeginEnable(BaseMightyMember mightyMember, BaseEnableConditionAttribute baseAttribute);
        void EndEnable(BaseMightyMember mightyMember, BaseEnableConditionAttribute baseAttribute);
    }

    public abstract class BaseEnableConditionDrawer<T> : BaseMemberDrawer<T>, IEnableConditionDrawer where T : BaseEnableConditionAttribute
    {
        public void BeginEnable(BaseMightyMember mightyMember, BaseEnableConditionAttribute baseAttribute) =>
            BeginEnable(mightyMember, (T) baseAttribute);

        public void EndEnable(BaseMightyMember mightyMember, BaseEnableConditionAttribute baseAttribute) => 
            EndEnable(mightyMember, (T) baseAttribute);

        protected abstract void BeginEnable(BaseMightyMember mightyMember, T attribute);
        protected abstract void EndEnable(BaseMightyMember mightyMember, T attribute);
    }
}
#endif