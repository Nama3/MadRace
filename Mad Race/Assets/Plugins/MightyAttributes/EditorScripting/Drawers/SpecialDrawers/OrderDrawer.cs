#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public class OrderDrawer : BaseSpecialDrawer<OrderAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<MightyInfo<short>> m_orderCache = new MightyCache<MightyInfo<short>>();

        public short GetOrder(BaseMightyMember mightyMember, OrderAttribute attribute)
        {
            if (!m_orderCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            return m_orderCache[mightyMember].Value;
        }

        protected override void Enable(BaseMightyMember mightyMember, OrderAttribute attribute)
        {
            var target = attribute.Target;

            if (!mightyMember.GetInfoFromMember(target, attribute.OrderCallback, out MightyInfo<short> info))
                info = mightyMember.GetInfoFromMember(target, attribute.OrderCallback, out MightyInfo<int> intInfo)
                    ? new MightyInfo<short>(intInfo, (short) intInfo.Value)
                    : new MightyInfo<short>(attribute.Order);

            m_orderCache[mightyMember] = info;
        }

        protected override void ClearCache() => m_orderCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_orderCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);

                mightyMember.Context.Drawer.ReorderMembers();
                throw MightyExceptionUtilities.AbortAfterReorderMembers;
            }

            var order = m_orderCache[mightyMember];
            var value = order.Value;
            if (order.RefreshValue() == value) return;

            mightyMember.Context.Drawer.ReorderMembers();
            throw MightyExceptionUtilities.AbortAfterReorderMembers;
        }
    }
}
#endif