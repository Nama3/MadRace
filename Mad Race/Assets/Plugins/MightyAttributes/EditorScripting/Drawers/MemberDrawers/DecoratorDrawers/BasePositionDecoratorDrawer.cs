#if UNITY_EDITOR
using System;

namespace MightyAttributes.Editor
{
    public interface IPositionDecoratorDrawer : IDecoratorDrawer
    {
        DecoratorPosition PositionByMember(BaseMightyMember mightyMember, BasePositionDecoratorAttribute baseAttribute);
    }

    public abstract class BasePositionDecoratorDrawer<T> : BaseDecoratorDrawer<T>, IPositionDecoratorDrawer, IRefreshDrawer
        where T : BasePositionDecoratorAttribute
    {
        private readonly MightyCache<MightyInfo<DecoratorPosition>> m_positionCache = new MightyCache<MightyInfo<DecoratorPosition>>();

        protected override void BeginDraw(BaseMightyMember mightyMember, T attribute)
        {
            if (PositionByMember(mightyMember, attribute).Contains(DecoratorPosition.Before))
                DrawDecorator(mightyMember, attribute);
        }

        protected override void EndDraw(BaseMightyMember mightyMember, T attribute)
        {
            if (PositionByMember(mightyMember, attribute).Contains(DecoratorPosition.After))
                DrawDecorator(mightyMember, attribute);
        }

        protected abstract void DrawDecorator(BaseMightyMember mightyMember, T attribute);

        public DecoratorPosition PositionByMember(BaseMightyMember mightyMember, BasePositionDecoratorAttribute basePosition)
        {
            if (!m_positionCache.Contains(mightyMember)) EnableDrawer(mightyMember, basePosition);
            return m_positionCache[mightyMember].Value;
        }

        protected override void Enable(BaseMightyMember mightyMember, T attribute)
        {
            if (!mightyMember.GetInfoFromMember<DecoratorPosition>(attribute.Target, attribute.PositionCallback, out var positionInfo,
                Enum.TryParse)) positionInfo = new MightyInfo<DecoratorPosition>(attribute.Position);

            m_positionCache[mightyMember] = positionInfo;
        }

        protected override void ClearCache() => m_positionCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute attribute)
        {
            if (!m_positionCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, attribute);
                return;
            }

            m_positionCache[mightyMember].RefreshValue();
        }
    }
}
#endif