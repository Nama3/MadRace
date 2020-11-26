#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public interface IExplorerDrawer : IPropertyDrawer
    {
    }
    
    public abstract class BaseExplorerDrawer<T> : BasePropertyDrawer<T>, IExplorerDrawer, IRefreshDrawer where T : BaseExplorerAttribute
    {
        private readonly MightyCache<MightyInfo<string>> m_pathCache = new MightyCache<MightyInfo<string>>();

        protected override void DrawProperty(MightySerializedField mightyMember, SerializedProperty property, T attribute)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                MightyGUIUtilities.DrawPropertyField(property);
                MightyGUIUtilities.DrawHelpBox($"{attribute.GetType()} can be used only on string fields");
                return;
            }

            if (!m_pathCache.Contains(mightyMember)) EnableDrawer(mightyMember, attribute);
            var defaultPath = m_pathCache[mightyMember].Value;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(property);
            if (MightyGUIUtilities.DrawButton("...", GUILayout.Width(35)))
            {
                var value = property.stringValue;

                value = DisplayPanel(property.displayName, !string.IsNullOrWhiteSpace(value)
                    ? FileUtilities.GetDirectoryPath(value) ?? value
                    : defaultPath, mightyMember, attribute);

                if (!string.IsNullOrWhiteSpace(value))
                    property.stringValue = value;
            }

            EditorGUILayout.EndHorizontal();
        }

        protected abstract string DisplayPanel(string panelTitle, string path, BaseMightyMember mightyMember, T attribute);

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, T attribute)
        {
            if (serializedField.Property.propertyType != SerializedPropertyType.String) return;


            if (!attribute.PathAsCallback || 
                !serializedField.GetInfoFromMember<string>(attribute.Target, attribute.DefaultPath, out var pathInfo))
                pathInfo = new MightyInfo<string>(attribute.DefaultPath ?? Application.dataPath);

            m_pathCache[serializedField] = pathInfo;
        }

        protected override void ClearCache() => m_pathCache.ClearCache();

        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_pathCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            m_pathCache[mightyMember].RefreshValue();
        }
    }
}
#endif