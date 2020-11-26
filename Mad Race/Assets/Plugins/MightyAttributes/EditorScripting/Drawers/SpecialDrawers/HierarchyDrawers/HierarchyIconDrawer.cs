#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class HierarchyIconDrawer : BaseHierarchyDrawer<HierarchyIconAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<(bool, Texture2D, MightyInfo<string>)> m_iconsCache =
            new MightyCache<(bool, Texture2D, MightyInfo<string>)>();

        private readonly MightyCache<bool> m_hasChildrenCache = new MightyCache<bool>();

        protected override void OnHierarchyChanged(MightyComponent mightyComponent, HierarchyIconAttribute attribute) => 
            m_hasChildrenCache[mightyComponent] = mightyComponent.ComponentContext.Component.transform.childCount != 0;

        protected override void OnGUI(MightyComponent mightyComponent, Rect selectionRect, HierarchyIconAttribute attribute)
        {
            if (!m_iconsCache.Contains(mightyComponent)) return;

            var (valid, icon, _) = m_iconsCache[mightyComponent];
            if (!valid) return;

            if (!m_hasChildrenCache.Contains(mightyComponent)) OnHierarchyChanged(mightyComponent, attribute);
            var hasChildren = m_hasChildrenCache[mightyComponent];

            var textureRatio = icon.width / icon.height;
            var scaledWidth = textureRatio * selectionRect.height;

            selectionRect.x -= hasChildren ? scaledWidth + 14 : scaledWidth + 2;
            GUI.Label(selectionRect, icon);
        }

        protected override void EnableDrawerImpl(MightyComponent mightyComponent, HierarchyIconAttribute attribute)
        {
            var target = attribute.Target;
            var path = attribute.IconPath;

            if (!attribute.PathAsCallback || !mightyComponent.GetInfoFromMember<string>(target, path, out var pathInfo))
                pathInfo = new MightyInfo<string>(path);

            var icon = MightyGUIUtilities.GetTexture(pathInfo.Value);

            m_iconsCache[mightyComponent] = (icon, icon, pathInfo);
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            m_iconsCache.ClearCache();
            m_hasChildrenCache.ClearCache();
        }

        void IRefreshDrawer.RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            base.RefreshDrawer(mightyMember, mightyAttribute);

            if (!m_iconsCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, _, info) = m_iconsCache[mightyMember];
            if (!valid) return;

            var icon = MightyGUIUtilities.GetTexture(info.RefreshValue());
            m_iconsCache[mightyMember] = (true, icon, info);
        }
    }
}
#endif