#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ItemNamesDrawer : BaseArrayDrawer<ItemNamesAttribute>, IRefreshDrawer
    {
        private readonly MightyCache<(bool, MightyInfo<object[]>)> m_itemNamesCache = new MightyCache<(bool, MightyInfo<object[]>)>();

        protected override void DrawArrayImpl(MightySerializedField serializedField, ItemNamesAttribute attribute, ArrayOption options, 
            BaseArrayDecoratorAttribute[] decoratorAttributes, IArrayElementDrawer drawer, BasePropertyDrawerAttribute drawerAttribute)
        {
            var property = serializedField.Property;

            if (!m_itemNamesCache.Contains(serializedField)) EnableDrawer(serializedField, attribute);
            var (valid, namesInfo) = m_itemNamesCache[serializedField];

            if (!valid)
            {
                MightyGUIUtilities.DrawHelpBox($"Callback name: \"{attribute.ItemNamesCallback}\" is invalid");
                MightyGUIUtilities.DrawPropertyField(property);
                return;
            }

            var names = namesInfo.Value;

            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).BeginDrawArray(serializedField, decoratorAttribute);

            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).BeginDrawHeader(serializedField, decoratorAttribute);

            if (!options.Contains(ArrayOption.HideLabel))
            {
                if (options.Contains(ArrayOption.DontFold))
                {
                    EditorGUILayout.LabelField(property.displayName,
                        options.Contains(ArrayOption.BoldLabel) ? EditorStyles.boldLabel : EditorStyles.label);
                    property.isExpanded = true;
                }
                else if (!MightyGUIUtilities.DrawFoldout(property,
                    options.Contains(ArrayOption.BoldLabel) ? MightyStyles.BoldFoldout : null))
                {
                    foreach (var decoratorAttribute in decoratorAttributes)
                        ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawHeader(serializedField, decoratorAttribute);

                    foreach (var decoratorAttribute in decoratorAttributes)
                        ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawArray(serializedField, decoratorAttribute);

                    return;
                }
            }
            else
                property.isExpanded = true;

            if (!options.Contains(ArrayOption.DontIndent))
                EditorGUI.indentLevel++;

            if (attribute.ForceSize)
            {
                var size = names.Length;
                if (property.arraySize != size)
                {
                    property.arraySize = names.Length;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }

            if (!options.Contains(ArrayOption.HideSizeField))
            {
                GUI.enabled = !options.Contains(ArrayOption.DisableSizeField);
                MightyGUIUtilities.DrawArraySizeField(property);
                GUI.enabled = true;
            }

            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawHeader(serializedField, decoratorAttribute);


            MightyGUIUtilities.DrawArrayBody(property, index =>
            {
                var element = property.GetArrayElementAtIndex(index);

                foreach (var decoratorAttribute in decoratorAttributes)
                    ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).BeginDrawElement(serializedField, index, decoratorAttribute);

                var canDrawName = index < names.Length && names[index] is string;

                if (drawer != null)
                {
                    if (canDrawName)
                        drawer.DrawElement(new GUIContent((string) names[index]), serializedField, index, drawerAttribute);
                    else
                        drawer.DrawElement(serializedField, index, drawerAttribute);
                }
                else
                {
                    if (canDrawName)
                        EditorGUILayout.PropertyField(element, new GUIContent((string) names[index]));
                    else
                        EditorGUILayout.PropertyField(element);
                }

                foreach (var decoratorAttribute in decoratorAttributes)
                    ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawElement(serializedField, index, decoratorAttribute);
            });

            if (!options.Contains(ArrayOption.DontIndent))
                EditorGUI.indentLevel--;


            foreach (var decoratorAttribute in decoratorAttributes)
                ((IArrayDecoratorDrawer) decoratorAttribute.Drawer).EndDrawArray(serializedField, decoratorAttribute);
        }

        protected override void EnableSerializeFieldDrawer(MightySerializedField serializedField, ItemNamesAttribute attribute)
        {
            base.EnableSerializeFieldDrawer(serializedField, attribute);
            
            var itemNames = attribute.ItemNames;

            var itemNamesInfo = new MightyInfo<object[]>(null, null, itemNames?.Cast<object>().ToArray());
            var valid = itemNames?.Length > 0;

            if (serializedField.GetArrayInfoFromMember(attribute.Target, attribute.ItemNamesCallback, out var namesInfo))
            {
                itemNamesInfo = new MightyInfo<object[]>(namesInfo, namesInfo.Value);
                valid = true;
            }

            m_itemNamesCache[serializedField] = (valid, itemNamesInfo);
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            m_itemNamesCache.ClearCache();
        }

        void IRefreshDrawer.RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            base.RefreshDrawer(mightyMember, mightyAttribute);

            if (!m_itemNamesCache.Contains(mightyMember))
            {
                EnableDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, info) = m_itemNamesCache[mightyMember];
            if (valid) info.RefreshValue();
        }
    }
}
#endif