#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class FilePanelDrawer : BaseExplorerDrawer<FilePanelAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<MightyInfo<string>> m_extensionCache = new MightyCache<MightyInfo<string>>();

        protected override string DisplayPanel(string panelTitle, string path, BaseMightyMember mightyMember, FilePanelAttribute attribute)
        {
            if (!m_extensionCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var extension = m_extensionCache[mightyMember].Value;

            return EditorUtility.OpenFilePanel(panelTitle, path, extension);
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, FilePanelAttribute attribute)
        {
            base.EnableSerializeFieldDrawer(serializedField, attribute);
            if (serializedField.Property.propertyType != SerializedPropertyType.String) return;

            var target = attribute.Target;


            if (!attribute.ExtensionAsCallback ||
                !serializedField.GetInfoFromMember<string>(target, attribute.Extension, out var extensionInfo))
                extensionInfo = new MightyInfo<string>(attribute.Extension);

            m_extensionCache[serializedField] = extensionInfo;
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            m_extensionCache.ClearCache();
        }

        void IRefreshDrawer.RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            base.RefreshDrawer(mightyMember, mightyAttribute);
            if (!m_extensionCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_extensionCache[mightyMember].RefreshValue();
        }
    }
}
#endif