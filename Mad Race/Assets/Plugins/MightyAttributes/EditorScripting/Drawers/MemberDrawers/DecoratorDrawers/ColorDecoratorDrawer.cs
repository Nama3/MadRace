#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ColorDecoratorDrawer : BaseDecoratorDrawer<ColorAttribute>, IDrawAnywhereDecorator, IRefreshDrawer
    {
        private readonly MightyCache<(MightyInfo<Color?>, MightyInfo<Color?>)> m_colorCache =
            new MightyCache<(MightyInfo<Color?>, MightyInfo<Color?>)>();
        
        public void BeginDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            BeginDraw(mightyMember, (ColorAttribute) baseAttribute);

        public void EndDrawAnywhere(BaseMightyMember mightyMember, IDrawAnywhereAttribute baseAttribute) =>
            EndDraw(mightyMember, (ColorAttribute) baseAttribute);

        protected override void BeginDraw(BaseMightyMember mightyMember, ColorAttribute attribute)
        {
            if (!m_colorCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var (background, content) = m_colorCache[mightyMember];

            MightyColorUtilities.BeginBackgroundColor(background.Value);
            MightyColorUtilities.BeginContentColor(content.Value);
        }

        protected override void EndDraw(BaseMightyMember mightyMember, ColorAttribute attribute)
        {
            MightyColorUtilities.EndBackgroundColor();
            MightyColorUtilities.EndContentColor();
        }

        protected override void Enable(BaseMightyMember mightyMember, ColorAttribute attribute)
        {
            var target = attribute.Target;

            var backgroundInfo = mightyMember.GetColorInfo(target, attribute.BackgroundColorName, attribute.BackgroundColor);
            var contentInfo = mightyMember.GetColorInfo(target, attribute.ContentColorName, attribute.ContentColor);

            m_colorCache[mightyMember] = (backgroundInfo, contentInfo);
        }

        protected override void ClearCache() => m_colorCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_colorCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (background, content) = m_colorCache[mightyMember];
            background.RefreshValue();
            content.RefreshValue();
        }
    }
}
#endif